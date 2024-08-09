namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Срез состояния продукта перед его изменением
	/// </summary>
	internal class ProductSlice
	{
		public ProductSlice(string name, string description, decimal price, string currency, DateTime slicedAt)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description ?? throw new ArgumentNullException(nameof(description));
			Price = price;
			Currency = currency ?? throw new ArgumentNullException(nameof(currency));
			SlicedAt = slicedAt;
			Images = new HashSet<Image>();
		}

		public int Id { get; set; }
		
		/// <summary>
		/// Название товара
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание товара
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Цена товара
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// Валюта цены товара
		/// </summary>
		public string Currency { get; set; }

		/// <summary>
		/// Изображения этого товара.
		/// </summary>
		public ICollection<Image> Images { get; }

		/// <summary>
		/// Дата и время среза состояния (UTC).
		/// </summary>
		public DateTime SlicedAt { get; set; }

		public int OriginalProductId { get; set; }

		/// <summary>
		/// Товар, срез которого был создан.
		/// </summary>
		public Product? OriginalProduct { get; set; }
	}
}
