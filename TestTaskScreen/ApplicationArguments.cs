using CommandLine;

namespace TestTaskScreen
{
	#nullable disable
	internal class ApplicationArguments
	{
		[Option('d', "database", Default = "database.sqlite3", Required = false, HelpText = "Database name")]
		public string Database { get; set; }

		[Option('f', "force", Default = false, Required = false, HelpText = "If this flag is set, an attempt to enforce database schema will be made")]
		public bool EnforceSchema { get; set; }

		[Option('s', "source", Required = true, HelpText = "Path to input file")]
		public string File { get; set; }
		
		[Option('c', "currency", Default = "RUB", Required = false, HelpText = "The currency to use")]
		public string Currency { get; set; }
	}
	#nullable enable
}
