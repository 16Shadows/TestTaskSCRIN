namespace TestTaskScreen.DatabaseModel
{
	/// <summary>
	/// Пользователь
	/// </summary>
	internal class User
	{
		public User(string username, string email, string password)
		{
			Username = username ?? throw new ArgumentNullException(nameof(username));
			Email = email ?? throw new ArgumentNullException(nameof(email));
			Password = password ?? throw new ArgumentNullException(nameof(password));
		}

		public int Id { get; set; }
		
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// email пользователя
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Хэш пароля пользователя
		/// </summary>
		public string Password { get; set; }
	}
}
