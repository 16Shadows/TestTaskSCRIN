namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Изображение
	/// </summary>
	internal class Image
	{
		public Image(string url)
		{
			Url = url ?? throw new ArgumentNullException(nameof(url));
		}

		public int Id { get; set; }
		
		/// <summary>
		/// URL этого изображения.
		/// </summary>
		public string Url { get; set; }
	}
}
