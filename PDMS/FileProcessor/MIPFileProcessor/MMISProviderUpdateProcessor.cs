using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

using FileProcessorCore;

namespace MIPFileProcessor
{
	public class MMISProviderUpdateProcessor : DatabaseRecordProcessor
	{
		#region Constructors

		public MMISProviderUpdateProcessor(LogFile log)
			: base(log)
		{

		}

		public MMISProviderUpdateProcessor()
			: base()
		{

		}

		#endregion
		/// <summary>
		///  This stored procedure is called to get all records from the database that need to be processed.
		/// </summary>
		public override string StoredProcedureName
		{
			get { return "T_ISTG_MMIS_PROVIDER_ELIG_GetByIsProcessed"; }
		}

		protected override bool ProcessRecord(DatabaseRecord record)
		{
			// Perform the insert or update operation. If successful, mark the staging record as processed.

			bool result;

			if (!string.IsNullOrEmpty(record.Data["ProviderNPI"]))
			{
				result = InsertOrUpdateMMISProviderEligRecord(record);
			}
			else
			{
				result = true;
			}
			
			if (result)
			{
				result = SetStagingRecordAsProcessed(record);
			}
			else
			{
				Log.WriteError(string.Format("Error processing record with TxnRecordID {0}", record.Data["TxnRecordID"]));
			}

			return result;
		}


		/// <summary>
		/// Helper method to call the stored procedure to insert or update the MMISProviderEligibility record.
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		private bool InsertOrUpdateMMISProviderEligRecord(DatabaseRecord record)
		{
			SqlConnection conn = null;

			try
			{
				conn = ConnectionFactory.NewConnection();

				SqlCommand cmd = new SqlCommand("sp_MMISProviderEligibility_UpdateOrInsert", conn);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.AddRecordDataParameter("@ProviderNumber", SqlDbType.VarChar, 11, record, "ProviderNumber");
				cmd.AddRecordDataParameter("@ProviderNPI", SqlDbType.VarChar, 10, record, "ProviderNPI");
				cmd.AddRecordDataParameter("@ProviderBusinessName", SqlDbType.VarChar, 35, record, "ProviderBusinessName");
				cmd.AddRecordDataParameter("@ProviderAddressLine1", SqlDbType.VarChar, 20, record, "ProviderAddressLine1");
				cmd.AddRecordDataParameter("@ProviderAddressLine2", SqlDbType.VarChar, 20, record, "ProviderAddressLine2");
				cmd.AddRecordDataParameter("@ProviderAddressCity", SqlDbType.VarChar, 18, record, "ProviderAddressCity");
				cmd.AddRecordDataParameter("@ProviderAddressState", SqlDbType.VarChar, 2, record, "ProviderAddressState");
				cmd.AddRecordDataParameter("@ProviderAddressZip5", SqlDbType.VarChar, 5, record, "ProviderAddressZip5");
				cmd.AddRecordDataParameter("@ProviderAddressZip4", SqlDbType.VarChar, 4, record, "ProviderAddressZip4");
				cmd.AddRecordDataParameter("@ProviderTypeCode", SqlDbType.VarChar, 2, record, "ProviderTypeCode");
				cmd.AddRecordDataParameter("@ProviderTypeName", SqlDbType.VarChar, 100, record, "ProviderTypeName");
				cmd.AddRecordDataParameter("@ProviderSpecialtyCode", SqlDbType.VarChar, 4, record, "ProviderSpecialtyCode");
				cmd.AddRecordDataParameter("@ProviderSpecialtyName", SqlDbType.VarChar, 150, record, "ProviderSpecialtyName");
				cmd.AddRecordDataParameter("@ProviderLicenseNumberStateCode", SqlDbType.VarChar, 2, record, "ProviderLicenseNumberStateCode");
				cmd.AddRecordDataParameter("@ProviderLicenseNumberStateName", SqlDbType.VarChar, 50, record, "ProviderLicenseNumberStateName");
				cmd.AddRecordDataParameter("@ProviderLicenseNumberProviderTypeCode", SqlDbType.VarChar, 2, record, "ProviderLicenseNumberProviderTypeCode");
				cmd.AddRecordDataParameter("@ProviderLicenseNumberProviderTypeName", SqlDbType.VarChar, 100, record, "ProviderLicenseNumberProviderTypeName");
				cmd.AddRecordDataParameter("@ProviderLicenseNumberLicenseNumber", SqlDbType.VarChar, 9, record, "ProviderLicenseNumberLicenseNumber");
				cmd.AddRecordDataParameter("@ProviderMedicareNumber", SqlDbType.VarChar, 10, record, "ProviderMedicareNumber");
				cmd.AddRecordDataParameter("@ProviderEligibilityStartDate1", SqlDbType.Date, record, "ProviderEligibilityStartDate1");
				cmd.AddRecordDataParameter("@ProviderEligibilityEndDate1", SqlDbType.Date, record, "ProviderEligibilityEndDate1");
				cmd.AddRecordDataParameter("@ProviderEligibilityStartDate2", SqlDbType.Date, record, "ProviderEligibilityStartDate2");
				cmd.AddRecordDataParameter("@ProviderEligibilityEndDate2", SqlDbType.Date, record, "ProviderEligibilityEndDate2");
				cmd.AddRecordDataParameter("@ProviderTaxonomyCode", SqlDbType.VarChar, 10, record, "ProviderTaxonomyCode");
				cmd.AddRecordDataParameter("@ProviderTaxonomyName", SqlDbType.VarChar, 50, record, "ProviderTaxonomyName");
				cmd.AddRecordDataParameter("@PayToProviderNumber", SqlDbType.VarChar, 11, record, "PayToProviderNumber");
				cmd.AddRecordDataParameter("@PayToProviderNPI", SqlDbType.VarChar, 10, record, "PayToProviderNPI");
				cmd.AddRecordDataParameter("@PayToProviderFTIN", SqlDbType.VarChar, 9, record, "PayToProviderFTIN");
				cmd.AddRecordDataParameter("@ProviderPracticeTypeCode", SqlDbType.VarChar, 2, record, "ProviderPracticeTypeCode");
				cmd.AddRecordDataParameter("@ProviderPracticeTypeName", SqlDbType.VarChar, 100, record, "ProviderPracticeTypeName");
				cmd.AddRecordDataParameter("@PayToProviderAddressLine1", SqlDbType.VarChar, 20, record, "PayToProviderAddressLine1");
				cmd.AddRecordDataParameter("@PayToProviderAddressLine2", SqlDbType.VarChar, 20, record, "PayToProviderAddressLine2");
				cmd.AddRecordDataParameter("@PayToProviderAddressCity", SqlDbType.VarChar, 18, record, "PayToProviderAddressCity");
				cmd.AddRecordDataParameter("@PayToProviderAddressState", SqlDbType.VarChar, 2, record, "PayToProviderAddressState");
				cmd.AddRecordDataParameter("@PayToProviderAddressZip5", SqlDbType.VarChar, 5, record, "PayToProviderAddressZip5");
				cmd.AddRecordDataParameter("@PayToProviderAddressZip4", SqlDbType.VarChar, 4, record, "PayToProviderAddressZip4");
				cmd.AddParameter("@CreateDate", SqlDbType.DateTime, EWSConfiguration.CurrentDateTime);
				cmd.AddParameter("@CreateUser", SqlDbType.VarChar, 25, EWSConfiguration.AppSettings("BatchUsername"));
				cmd.AddParameter("@UpdateDate", SqlDbType.DateTime, EWSConfiguration.CurrentDateTime);
				cmd.AddParameter("@UpdateUser", SqlDbType.VarChar, 25, EWSConfiguration.AppSettings("BatchUsername"));
				cmd.AddParameter("@UpdateColumns", SqlDbType.Xml, null);

				int result = cmd.ExecuteNonQuery();

				if (result != 1)
				{
					throw new Exception(string.Format("No records inserted or updated when executing 'sp_MMISProviderEligibility_UpdateOrInsert' for Provider Number '{0}'.", record.Data["ProviderNumber"]));
				}


				return true;
			}
			catch (Exception ex)
			{
				Log.WriteError(string.Format("Error inserting record: {0}", ex));
				return false;
			}
		}

		/// <summary>
		/// Helper method to call stored procedure to flag the staging record as processed.
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		private bool SetStagingRecordAsProcessed(DatabaseRecord record)
		{
			SqlConnection conn = null;

			try
			{
				conn = ConnectionFactory.NewConnection();

				SqlCommand cmd = new SqlCommand("T_ISTG_MMIS_PROVIDER_ELIG_UpdateIsProcessed", conn);
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.AddRecordDataParameter("@TxnRecordID", SqlDbType.Int, record, "TxnRecordID");
				cmd.AddParameter("@IsProcessed", SqlDbType.Bit, true);
				cmd.AddParameter("@UpdateDate", SqlDbType.DateTime, EWSConfiguration.CurrentDateTime);
				cmd.AddParameter("@UpdateUser", SqlDbType.VarChar, 25, EWSConfiguration.AppSettings("BatchUsername"));
				

				int result = cmd.ExecuteNonQuery();

				if (result != 1)
				{
					throw new Exception(string.Format("No records updated when executing 'T_ISTG_MMIS_PROVIDER_ELIG_UpdateIsProcessed' for TxnRecord '{0}'.", record.Data["TxnRecordID"]));
				}

				return true;
			}
			catch (Exception ex)
			{
				Log.WriteError(string.Format("Error setting IsProcessed = true: {0}", ex));
				return false;
			}
		}

		/// <summary>
		/// Overridden method to add parameters to the stored procedure that gathers all records to be processed. Here passing in IsProcessed = 0
		/// </summary>
		/// <param name="command"></param>
		protected override void AddStoredProcedureParameters(SqlCommand command)
		{
			command.AddParameter("@IsProcessed", SqlDbType.Bit, false);
			
			base.AddStoredProcedureParameters(command);
		}

		protected override void PostProcessRecords(IEnumerable<DatabaseRecord> lstRecords)
		{
			/// Call Teresa's stored proc.
			SqlConnection conn = null;
			
			try
			{
				Log.Write("Begin post-processing records. Calling stored procedure 'MMISAuditMatch_Insert'.");
				conn = ConnectionFactory.NewConnection();

				SqlCommand cmd = new SqlCommand("MMISAuditMatch_Insert", conn);
				cmd.CommandType = CommandType.StoredProcedure;

				int count = Convert.ToInt32(cmd.ExecuteScalar());

				Log.Write(string.Format("{0} MMIS Audit Match(es) added calling stored procedure 'MMISAuditMatch_Insert'.", count));

				Log.Write("End post-processing records.");
				
			}
			catch (Exception ex)
			{
				Log.WriteError(string.Format("Error in PostProcessRecords(): {0}", ex));
			}
			finally
			{
				if (conn != null)
				{
					conn.Close();
				}
			}
			
		}

		protected override void PreProcessRecords(IEnumerable<DatabaseRecord> lstRecords)
		{
			// No need for PreProcessing is this application
		}
	}
}
