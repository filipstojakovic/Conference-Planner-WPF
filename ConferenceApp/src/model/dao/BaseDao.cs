using ConferenceApp.database;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class BaseDao
	{
		public static string SELECT_SCALAR = "; SELECT CAST(scope_identity() AS int)";
		protected MySqlConnection connection;

		protected BaseDao() => connection = MySqlSingleton.getInstance().connection;
	}
}
