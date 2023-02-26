using ConferenceApp.database;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class BaseDao
	{
		protected MySqlConnection connection;

		protected BaseDao() => connection = MySqlSingleton.getInstance().connection;

	}
}
