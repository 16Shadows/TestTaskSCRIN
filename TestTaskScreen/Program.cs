using CommandLine;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using TestTaskScreen.DatabaseModel;
using TestTaskScreen.XmlModel;

namespace TestTaskScreen
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<ApplicationArguments>(args).WithParsed(Execute);
		}

		/// <summary>
		/// Напечатать сообщение исключения и всех вложенных исключений
		/// </summary>
		/// <param name="exception">Исключение</param>
		static void PrintException(Exception exception)
		{
			Exception? cur = exception;
			while (cur != null)
			{
				Console.WriteLine(cur.Message);
				cur = cur.InnerException;
			}
		}

		/// <summary>
		/// Десериализировать xml-файл по указанному пути в объект с заказами.
		/// </summary>
		/// <param name="filePath">Путь до файла</param>
		/// <returns>Объект с заказами.</returns>
		static OrdersList DeserializeFile(string filePath)
		{
			using Stream file = File.OpenRead(filePath);

			XmlSerializer serializer = new XmlSerializer(typeof(XmlModel.OrdersList));
			return (XmlModel.OrdersList)serializer.Deserialize(file)!;
		}

		/// <summary>
		/// Создать контекст EF для БД
		/// </summary>
		/// <param name="dataSource">Параметр dataSource строки соединения</param>
		/// <returns>Контекст EF для БД</returns>
		static ShopDatabaseContext CreateDatabaseContext(string dataSource)
		{
			SqliteConnectionStringBuilder sqliteConnectionStringBuilder = new SqliteConnectionStringBuilder();
			sqliteConnectionStringBuilder.DataSource=dataSource;

			DbContextOptionsBuilder dbContextOptions = new DbContextOptionsBuilder();
			dbContextOptions.UseSqlite(sqliteConnectionStringBuilder.ToString());

			return new ShopDatabaseContext(dbContextOptions.Options);
		}

		/// <summary>
		/// Создать новый товар с указанными параметрами.
		/// </summary>
		/// <param name="product">Товар из xml-файла, для которого создаём товар</param>
		/// <param name="creationDate"></param>
		/// <param name="currency"></param>
		/// <returns></returns>
		static ProductSlice CreateNewProduct(XmlModel.Product product, DateTime creationDate, string currency)
		{
			var slice = new DatabaseModel.ProductSlice(
							product.Name,
							"-",
							product.Price,
							currency,
							creationDate
						);
			slice.OriginalProduct = new DatabaseModel.Product(
				product.Name,
				"-",
				product.Price,
				currency,
				creationDate
			);
			return slice;
		}

		static void Execute(ApplicationArguments arguments)
		{
			XmlModel.OrdersList orders;
			try
			{
				orders = DeserializeFile(arguments.File);
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to deserialize xml file:");
				PrintException(e);
				return;
			}

			try
			{
				using ShopDatabaseContext ctx = CreateDatabaseContext(arguments.Database);
				if (arguments.EnforceSchema)
					ctx.Database.EnsureCreated();

				//Проверим наличие конфликтов номеров заказов
				HashSet<int> orderIds = orders.Orders.Select(x => x.Id).ToHashSet();
				var conflictingIds = from purchase in ctx.Purchases where orderIds.Contains(purchase.Id) select purchase.Id;
				bool conflict = false;
				foreach (var conflictingId in conflictingIds)
				{
					Console.WriteLine($"Error: conflicting order no: {conflictingId}");
					conflict = true;
				}
				if (conflict)
					return;

				//Создаём объекты для всех уникальных пользователей из xml (определяем уникального пользователя по email)
				Dictionary<string, DatabaseModel.User> users = orders.Orders
																	 .Select(x => x.User)
																	 .GroupBy(x => x.Email)
																	 .Select(x => new DatabaseModel.User(x.Last().Username, x.Key, "-"))
																	 .ToDictionary(x => x.Email);

				//Ассоциируем пользователей из БД с пользователями из XML
				var dbUsers = (from user in ctx.Users where users.Keys.Contains(user.Email) select user);
				foreach (var user in dbUsers)
					users[user.Email] = user;

				foreach (var kvp in users)
					ctx.Users.Update(kvp.Value);

				//Сохраняем новые товары, чтобы избежать дублирования в БД
				Dictionary<string, DatabaseModel.ProductSlice> newProducts = new Dictionary<string, ProductSlice>();

				//Для каждого заказа создаём запись о заказе, выбирая срез товара в зависимости от даты заказа
				foreach (var order in orders.Orders)
				{
					DatabaseModel.Purchase purchase = new DatabaseModel.Purchase(
						0,
						order.OrderDate
					);
					purchase.Id = order.Id;
					purchase.User = users[order.User.Email];
					foreach (var product in order.Products)
					{
						//Либо находим срез продукта с таким же названием и датой среза раньше даты заказа, выбирая последний из них (т.е. самый последний перез заказом), либо создаём новый продукт и срез
						var productSlice = (from slice
											in ctx.ProductSlices
											where slice.Name == product.Name && slice.SlicedAt <= order.OrderDate
											orderby slice.SlicedAt descending
											select slice)
											.FirstOrDefault();

						if (productSlice == null)
						{
							//Если возможно, достаём из кеша новый продукт, чтобы избежать дублирования записей.
							if (!newProducts.TryGetValue(product.Name, out productSlice))
							{
								productSlice = CreateNewProduct(product, order.OrderDate, arguments.Currency);
								newProducts[product.Name] = productSlice;
								ctx.ProductSlices.Add(productSlice);
							}
							//Если заказы не упорядочены по дате заказа, то гарантируем, что дата среза и редактирования товара будут совпадать с датой самого раннего заказа.
							if (purchase.PurchaseDate < productSlice.SlicedAt)
							{
								productSlice.SlicedAt = purchase.PurchaseDate;
								productSlice.OriginalProduct!.EditedAt = purchase.PurchaseDate;
							}
						}

						var purchaseProduct = new PurchaseProduct();
						purchaseProduct.Product = productSlice;
						purchaseProduct.Purchase = purchase;
						purchaseProduct.Count = product.Quantity;

						purchase.Products.Add(purchaseProduct);
					}

					ctx.Purchases.Add( purchase );
				}

				ctx.SaveChanges();
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to apply changes to database.");
				PrintException(e);
			}
		}
	}
}