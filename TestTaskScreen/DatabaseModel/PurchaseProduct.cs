namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Товар в покупке
	/// </summary>
	internal class PurchaseProduct
	{
		public int PurchaseId { get; set; }
		/// <summary>
		/// Покупка, в которой находится этот товар
		/// </summary>
		public Purchase? Purchase { get; set; }
		
		public int ProductId { get; set; }
		/// <summary>
		/// Этот товар
		/// </summary>
		public ProductSlice? Product { get; set; }
		
		/// <summary>
		/// Количество позиций этого товара.
		/// </summary>
		public int Count { get; set; }
	}
}
