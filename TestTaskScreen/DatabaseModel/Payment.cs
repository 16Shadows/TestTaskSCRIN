namespace TestTaskScreen.DatabaseModel
{
	internal class Payment
	{
		public Payment(int status, string provider, decimal sum, string currency)
		{
			Status = status;
			Provider = provider ?? throw new ArgumentNullException(nameof(provider));
			Sum = sum;
			Currency = currency ?? throw new ArgumentNullException(nameof(currency));
		}

		public int Id { get; set; }
		
		/// <summary>
		/// Статус платежа
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Сервис, проводящий платёж
		/// </summary>
		public string Provider { get; set; }

		/// <summary>
		/// Сумма платежа
		/// </summary>
		public decimal Sum { get; set; }

		/// <summary>
		/// Валюта платежа
		/// </summary>
		public string Currency { get; set; }
	}
}
