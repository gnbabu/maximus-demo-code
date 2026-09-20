using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace FileProcessorCore
{
	public static class ConnectionFactory
	{
		public const string CONNECTION_STRING_CONFIG_KEY = "pipDB";

		private static SqlConnection _connection;

		public static SqlConnection NewConnection()
		{
			 if (_connection == null)
			{
				_connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING_CONFIG_KEY].ConnectionString);
			}

			 if (_connection.State != System.Data.ConnectionState.Open)
			 {
				 _connection.Open();
			 }

			return _connection;
		}

		public static void CloseConnection()
		{
			if (_connection != null)
			{
				_connection.Close();
			}
		}
	}
}
