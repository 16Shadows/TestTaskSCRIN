using System.Globalization;
using System.Xml.Serialization;

namespace TestTaskScreen.XmlModel
{
	/// <summary>
	/// Заказ в XML-файле
	/// </summary>
	public class Order
	{
		const string RegDatePattern = "yyyy.MM.dd";

		public Order(int id, DateTime orderDate, decimal sum, User user, Product[] products)
		{
			Id = id;
			OrderDate = orderDate;
			Sum = sum;
			User = user ?? throw new ArgumentNullException(nameof(user));
			Products = products ?? throw new ArgumentNullException(nameof(products));
		}

		private Order() : this(0, DateTime.MinValue, 0, new User("Unknown", "Unknown"), new Product[0]) {}


		[XmlElement("no")]
		public int Id { get; set; }
		[XmlElement("reg_date")]
		public string RegDate
		{
			get => OrderDate.ToString(RegDatePattern, CultureInfo.InvariantCulture);
			set => OrderDate = DateTime.ParseExact(value, RegDatePattern, CultureInfo.InvariantCulture);
		}

		[XmlIgnore]
		public DateTime OrderDate { get; set; }


		[XmlElement("sum")]
		public decimal Sum { get; set; }
		[XmlElement("user")]
		public User User { get; set; }
		[XmlElement("product")]
		public Product[] Products { get; set; }
	}
}
