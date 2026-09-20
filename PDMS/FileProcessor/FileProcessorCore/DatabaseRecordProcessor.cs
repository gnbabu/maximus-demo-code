using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace FileProcessorCore
{
	public abstract class DatabaseRecordProcessor : BatchProcessor
	{

		#region Constructors
		
		public DatabaseRecordProcessor(LogFile log)
			: base(log)
		{

		}

		public DatabaseRecordProcessor()
			: base()
		{

		}

		#endregion

		public abstract string StoredProcedureName { get; }

		public override void Execute()
		{
			IEnumerable<DatabaseRecord> lstRecords = GetRecords();

			if (lstRecords == null)
			{
				throw new Exception("GetRecords() returned null.");
			}

			Log.Write(string.Format("Begin processing {0} records.", lstRecords.Count()));

			PreProcessRecords(lstRecords);

			foreach (DatabaseRecord record in lstRecords)
			{
				bool result = ProcessRecord(record);

				if (!result)
				{

				}
			}


			PostProcessRecords(lstRecords);

			Log.Write("End processing records.");

			/// Write summary.
		}

		protected abstract void PostProcessRecords(IEnumerable<DatabaseRecord> lstRecords);

		protected abstract bool ProcessRecord(DatabaseRecord record);

		protected abstract void PreProcessRecords(IEnumerable<DatabaseRecord> lstRecords);

		protected virtual IEnumerable<DatabaseRecord> GetRecords()
		{
			List<DatabaseRecord> lstRecords = new List<DatabaseRecord>();

			SqlConnection conn = null;
			SqlDataReader reader = null;

			try
			{
				conn = ConnectionFactory.NewConnection();

				SqlCommand cmd = new SqlCommand(StoredProcedureName, conn);
				cmd.CommandType = CommandType.StoredProcedure;

				AddStoredProcedureParameters(cmd);

				reader = cmd.ExecuteReader();

				if (reader != null)
				{
					while (reader.Read())
					{
						DatabaseRecord record = new DatabaseRecord(Log);

						for (int i = 0; i < reader.FieldCount; i++)
						{
							if (reader.GetFieldType(i) == typeof(DateTime) && !reader.IsDBNull(i))
							{
								record.Data[reader.GetName(i)] = Convert.ToDateTime(reader.GetValue(i)).ToString("yyyyMMdd");
							}
							else
							{
								record.Data[reader.GetName(i)] = Convert.ToString(reader.GetValue(i));

							}

						}


						lstRecords.Add(record);
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(string.Format("Error in GetRecords(): {0}", ex));
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
			

			return lstRecords;
		}

		protected virtual void AddStoredProcedureParameters(SqlCommand command)
		{

		}
	}
}
