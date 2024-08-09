using System.Xml.Serialization;

namespace TestTaskScreen.XmlModel
{
	/// <summary>
	/// Корневой элемент XML-файла.
	/// </summary>
	[XmlRoot("orders")]
	public class OrdersList
	{
		private OrdersList() : this(new Order[0]) {}

		public OrdersList(Order[] orders)
		{
			Orders = orders ?? throw new ArgumentNullException(nameof(orders));
		}

		[XmlElement("order")]
		public Order[] Orders { get; set; }
	}
}
