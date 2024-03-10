using System;
using System.Configuration;
using System.IO;
using MySql.Data.MySqlClient;

namespace ConferenceApp.database
{
    public class MySqlSingleton
    {
        private static MySqlSingleton mySqlSingleton;
        public MySqlConnection connection { get; private set; }

        private MySqlSingleton()
        {
        }

        //get Opened connection to Database Server
        public static MySqlSingleton getInstance()
        {
            if (mySqlSingleton == null)
            {
                mySqlSingleton = new MySqlSingleton();
                var connectionString = ConfigurationManager.ConnectionStrings["mySqlConnection"].ConnectionString;
                var connection = new MySqlConnection(connectionString);
                connection.Open();

                var databaseName = ConfigurationManager.AppSettings["databaseName"];
                if (!DatabaseExists(connection, databaseName))
                {
                    initDatabase(connection, databaseName);
                    connection.ChangeDatabase(databaseName);
                    initData(connection, databaseName);
                    initTriggersAndProcedures(connection, databaseName);
                }
                else
                {
                    connection.ChangeDatabase(databaseName);
                }

                mySqlSingleton.connection = connection;
            }

            return mySqlSingleton;
        }

        private static void initTriggersAndProcedures(MySqlConnection mySqlConnection, string databaseName)
        {
            var triggersAndProcedures = ConfigurationManager.AppSettings["triggersAndProcedures"];
            executeScript(triggersAndProcedures, mySqlConnection, databaseName);
        }


        private static void initData(MySqlConnection mySqlConnection, string databaseName)
        {
            var dataPath = ConfigurationManager.AppSettings["dataPath"];
            executeScript(dataPath, mySqlConnection, databaseName);
        }

        private static void initDatabase(MySqlConnection mySqlConnection, string databaseName)
        {
            var ddlPath = ConfigurationManager.AppSettings["ddlPath"];
            executeScript(ddlPath, mySqlConnection, databaseName);
        }

        private static void executeScript(string relativePath, MySqlConnection mySqlConnection, string databaseName)
        {
            var basePath =
                System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var content = File.ReadAllText(@$"{basePath}\{relativePath}");
            MySqlCommand cmd = new MySqlCommand(content, mySqlConnection);
            cmd.ExecuteNonQuery();
        }

        //Close connection to server
        public void closeConnection()
        {
            connection.Close();
        }

        public static bool DatabaseExists(MySqlConnection connection, string databaseName)
        {
            try
            {
                string checkDatabaseQuery =
                    $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";
                MySqlCommand checkDatabaseCmd = new MySqlCommand(checkDatabaseQuery, connection);

                object result = checkDatabaseCmd.ExecuteScalar();

                return result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking database existence: " + ex.Message);
                return false;
            }
        }
    }
}