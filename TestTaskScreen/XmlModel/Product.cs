using System.Xml.Serialization;

namespace TestTaskScreen.XmlModel
{
	/// <summary>
	/// Товар в xml-файле
	/// </summary>
	public class Product
	{
		private Product() : this(0, "Unknown", 0) {}

		public Product(int quantity, string name, decimal price)
		{
			Quantity = quantity;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Price = price;
		}

		[XmlElement("quantity")]
		public int Quantity { get; set; }
		
		[XmlElement("name")]
		public string Name { get; set; }

		[XmlElement("price")]
		public decimal Price { get; set; }
	}
}
