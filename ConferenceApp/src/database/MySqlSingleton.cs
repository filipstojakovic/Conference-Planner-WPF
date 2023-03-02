using MySql.Data.MySqlClient;

namespace ConferenceApp.database
{
	public class MySqlSingleton
	{
		private static MySqlSingleton mySqlSingleton;
		public MySqlConnection connection { get; private set; }

		private MySqlSingleton() { }

		//get Opened connection to Database Server
		public static MySqlSingleton getInstance()
		{
			if (mySqlSingleton == null)
			{
				mySqlSingleton = new MySqlSingleton();
				var settings = new MySqlConnectionStringBuilder()
				{
					//TODO: maybe make property file 
					Server = "localhost",
					UserID = "root",
					Password = "root",
					Database = "conferencedb",
					Port = 3306
				};
				mySqlSingleton.connection = new MySqlConnection(settings.ConnectionString);
				mySqlSingleton.connection.Open();
			}
			return mySqlSingleton;
		}

		//Close connection to server
		public void closeConnection()
		{
			connection.Close();
		}
	}
}
