using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FileProcessorCore
{
	public class Authorization
	{
		public static bool StartProccessor(string sourceSystem, string sourceModule, out DateTime dtLastStarted, out bool useOverrideConfig, out string overrideConfigValues)
		{
			SqlConnection sqlConn = null;
			SqlCommand cmd = null;

			dtLastStarted = DateTime.MinValue;
			useOverrideConfig = false;
			overrideConfigValues = null;

			try
			{
				sqlConn = ConnectionFactory.NewConnection();

				cmd = new SqlCommand("proc_CheckBatchActivity", sqlConn);

				cmd.Parameters.Add("@Source_System", SqlDbType.VarChar, 255).Value = sourceSystem;
				cmd.Parameters.Add("@Source_Module", SqlDbType.VarChar, 255).Value = sourceModule;

				var statusParam = cmd.Parameters.Add("@Status", SqlDbType.Char, 1);
				statusParam.Direction = ParameterDirection.Output;

				var dtLastStartedParam = cmd.Parameters.Add("@DT_LastStarted", SqlDbType.DateTime);
				dtLastStartedParam.Direction = ParameterDirection.Output;

				var useOverrideConfigParam = cmd.Parameters.Add("@Use_Override_Config", SqlDbType.Bit);
				useOverrideConfigParam.Direction = ParameterDirection.Output;

				var overrideConfigValuesParam = cmd.Parameters.Add("@Override_Config_Values", SqlDbType.VarChar, 500);
				overrideConfigValuesParam.Direction = ParameterDirection.Output;

				//sqlConn.Open();
				cmd.CommandType = CommandType.StoredProcedure;
				int rows = cmd.ExecuteNonQuery();

				if (rows == 0)
				{
					return false;
				}

				string status = (string)statusParam.Value;

				if (dtLastStartedParam.Value != DBNull.Value)
				{
					dtLastStarted = (DateTime)dtLastStartedParam.Value;
				}

				if (useOverrideConfigParam.Value != DBNull.Value)
				{
					useOverrideConfig = (bool)useOverrideConfigParam.Value;
				}

				if (overrideConfigValuesParam.Value != DBNull.Value)
				{
					overrideConfigValues = (string)overrideConfigValuesParam.Value;
				}
				
				return string.Compare(status, "C") == 0;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (cmd != null)
				{
					cmd.Dispose();
				}
			}
		}


		public static bool FinishProccessor(string sourceSystem, string sourceModule)
		{
			SqlConnection sqlConn = null;
			SqlCommand cmd = null;

			try
			{
				sqlConn = ConnectionFactory.NewConnection();

				cmd = new SqlCommand("proc_MarkBatchActivityComplete", sqlConn);

				cmd.Parameters.Add("@Source_System", SqlDbType.VarChar, 255).Value = sourceSystem;
				cmd.Parameters.Add("@Source_Module", SqlDbType.VarChar, 255).Value = sourceModule;

				//sqlConn.Open();
				cmd.CommandType = CommandType.StoredProcedure;
				int result = cmd.ExecuteNonQuery();

				return result == 1;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				//if (sqlConn != null)
				//{
				//	sqlConn.Close();
				//	sqlConn.Dispose();
				//}

				if (cmd != null)
				{
					cmd.Dispose();
				}
			}
		}
	}
}
