namespace Raven.Smuggler.Database.Impl.Remote
{
	public class DatabaseSmugglerRemoteConnectionOptions
	{
		public string ApiKey { get; set; }

		public string ConnectionStringName { get; set; }

		public string Database { get; set; }

		public string Url { get; set; }
	}
}