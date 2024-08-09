using System.Xml.Serialization;

namespace TestTaskScreen.XmlModel
{
	/// <summary>
	/// Пользователь в XML-файле
	/// </summary>
	public class User
	{
		private User() : this("Unknown", "Unknown") {}

		public User(string username, string email)
		{
			Username = username ?? throw new ArgumentNullException(nameof(username));
			Email = email ?? throw new ArgumentNullException(nameof(email));
		}

		[XmlElement("fio")]
		public string Username { get; set; }

		[XmlElement("email")]
		public string Email { get; set; }
	}
}