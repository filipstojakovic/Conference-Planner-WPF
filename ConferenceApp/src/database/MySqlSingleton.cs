using System.Configuration;
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
				var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
				mySqlSingleton.connection = new MySqlConnection(connectionString);
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
