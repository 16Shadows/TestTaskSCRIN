namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Товар
	/// </summary>
	internal class Product
	{
		public Product(string name, string description, decimal price, string currency, DateTime editedAt)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = price;
			Currency = currency ?? throw new ArgumentNullException(nameof(currency));
			EditedAt = editedAt;
			Images = new HashSet<Image>();
		}

		public int Id { get; set; }

		/// <summary>
		/// Название товара.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Описание товара.
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Цена товара.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// Валюта цены.
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Изображения этого товара.
		/// </summary>
		public ICollection<Image> Images { get; }
		
		/// <summary>
		/// Дата и время последнего редактирования этого товара (UTC).
		/// </summary>
		public DateTime EditedAt { get; set; }
	}
}
