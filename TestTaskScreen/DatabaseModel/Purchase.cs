namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Покупка
	/// </summary>
	internal class Purchase
	{
		public Purchase(int status, DateTime purchaseDate)
		{
			Status = status;
			PurchaseDate = purchaseDate;
			
			Products = new HashSet<PurchaseProduct>();
		}

		public int Id { get; set; }
		
		/// <summary>
		/// Статус покупки
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Дата и время покупки
		/// </summary>
		public DateTime PurchaseDate { get; set; }
		
		public int UserId { get; set; }
		
		/// <summary>
		/// Пользователь, который совершил покупку.
		/// </summary>
		public User? User { get; set; }

		/// <summary>
		/// Товары в этой покупке.
		/// </summary>
		public ICollection<PurchaseProduct> Products { get; }

		public int PaymentId { get; set; }

		/// <summary>
		/// Платёж за этот заказ.
		/// </summary>
		public Payment Payment { get; set; }
	}
}
