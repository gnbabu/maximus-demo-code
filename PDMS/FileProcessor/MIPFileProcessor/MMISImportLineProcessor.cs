using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using FileProcessorCore;
using System.Configuration;


namespace MIPFileProcessor
{
	public class MMISImportLineProcessor : LineRecordImportLineProcessor
	{

		#region Constructors

		public MMISImportLineProcessor(LogFile log)
			: base(log, null, FileFormat.FixedLength)
		{
			
		}

		#endregion

		public override Record NewRecord(string workingFilePath, int lineNumber)
		{
			MMISProviderUpdateRecord record = new MMISProviderUpdateRecord(Log, workingFilePath, lineNumber);
			
			return record;
		}

		protected override bool IsInsertRecord(Record record)
		{
			/// For this processor, all records are inserts since they go into a staging table no matter what.
			return true;
		}

		protected override bool InsertRecord(Record record)
		{
			/// Always insert a record into the Staging table.
			Log.WriteVerbose(string.Format("Updating record with ID '{0}'", record.ID));
			ImportRecord importRecord = record as ImportRecord;

			SqlConnection conn = null;

			try
			{
				conn = ConnectionFactory.NewConnection();

				SqlCommand cmd = new SqlCommand(InsertStoredProc, conn);
				cmd.CommandType = CommandType.StoredProcedure;

                foreach (var templateField in TemplateFields)
                {
                    cmd.AddRecordDataParameter("@" + templateField.FieldName, SqlDbType.VarChar, templateField.EndIndex - templateField.StartIndex + 1, record, templateField.FieldName);
                }

                //cmd.AddRecordDataParameter("@ISAtypicalProvider", SqlDbType.VarChar, 1, record, "ISAtypicalProvider");
                //cmd.AddRecordDataParameter("@ProviderNumber", SqlDbType.VarChar, 11, record, "ProviderNumber");
                //cmd.AddRecordDataParameter("@ProviderNPI", SqlDbType.VarChar, 10, record, "ProviderNPI");
                //cmd.AddRecordDataParameter("@ProviderNPIFromDate", SqlDbType.Date, 8, record, "ProviderNPIFromDate");
                //cmd.AddRecordDataParameter("@ProviderNPIToDate", SqlDbType.Date, 8, record, "ProviderNPIToDate");
                //cmd.AddRecordDataParameter("@ProviderName", SqlDbType.VarChar, 35, record, "ProviderName");
                //cmd.AddRecordDataParameter("@ProviderAddress1", SqlDbType.VarChar, 20, record, "ProviderAddress1");
                //cmd.AddRecordDataParameter("@ProviderAddress2", SqlDbType.VarChar, 20, record, "ProviderAddress2");
                //cmd.AddRecordDataParameter("@ProviderCity", SqlDbType.VarChar, 18, record, "ProviderCity");
                //cmd.AddRecordDataParameter("@providerState", SqlDbType.VarChar, 2, record, "providerState");
                //cmd.AddRecordDataParameter("@ProviderZip", SqlDbType.VarChar, 5, record, "ProviderZip");
                //cmd.AddRecordDataParameter("@ProviderZip4", SqlDbType.VarChar, 4, record, "ProviderZip4");
                //cmd.AddRecordDataParameter("@ProviderType", SqlDbType.VarChar, 2, record, "ProviderType");
                //cmd.AddRecordDataParameter("@ProviderSpecialty", SqlDbType.VarChar, 2, record, "ProviderSpecialty");
                //cmd.AddRecordDataParameter("@ProviderLicenseNumberStateCode", SqlDbType.VarChar, 2, record, "ProviderLicenseNumberStateCode");
                //cmd.AddRecordDataParameter("@ProviderLicenseNumberProviderTypeCode", SqlDbType.VarChar, 2, record, "ProviderLicenseNumberProviderTypeCode");
                //cmd.AddRecordDataParameter("@ProviderLicenseNumberLicenseNumber", SqlDbType.VarChar, 9, record, "ProviderLicenseNumberLicenseNumber");
                //cmd.AddRecordDataParameter("@ProviderMedicareNumber", SqlDbType.VarChar, 10, record, "ProviderMedicareNumber");
                //cmd.AddRecordDataParameter("@ProviderEnrollmentStatus", SqlDbType.VarChar, 2, record, "ProviderEnrollmentStatus");
                //cmd.AddRecordDataParameter("@ProviderEligibilityStartDate", SqlDbType.Date, 8, record, "ProviderEligibilityStartDate");
                //cmd.AddRecordDataParameter("@providerEligibilityEndDate", SqlDbType.Date, 8, record, "providerEligibilityEndDate");
                //cmd.AddRecordDataParameter("@ProviderTaxonomyCode", SqlDbType.VarChar, 10, record, "ProviderTaxonomyCode");
                //cmd.AddRecordDataParameter("@PayToProviderNumber", SqlDbType.VarChar, 11, record, "PayToProviderNumber");
                //cmd.AddRecordDataParameter("@PaytoProviderNPI", SqlDbType.VarChar, 9, record, "PaytoProviderNPI");
                //cmd.AddRecordDataParameter("@ProviderTypeOfPractice", SqlDbType.VarChar, 2, record, "ProviderTypeOfPractice");
                //cmd.AddRecordDataParameter("@PayToProviderAddress1", SqlDbType.VarChar, 20, record, "PayToProviderAddress1");
                //cmd.AddRecordDataParameter("@PayToProviderAddress2", SqlDbType.VarChar, 20, record, "PayToProviderAddress2");
                //cmd.AddRecordDataParameter("@PayToProviderCity", SqlDbType.VarChar, 18, record, "PayToProviderCity");
                //cmd.AddRecordDataParameter("@PayToProviderState", SqlDbType.VarChar, 2, record, "PayToProviderState");
                //cmd.AddRecordDataParameter("@PayToProviderZip", SqlDbType.VarChar, 5, record, "PayToProviderZip");
                //cmd.AddRecordDataParameter("@PayToProviderZip4", SqlDbType.VarChar, 4, record, "PayToProviderZip4");
                //cmd.AddRecordDataParameter("@ProviderDBA", SqlDbType.VarChar, 35, record, "ProviderDBA");
                //cmd.AddRecordDataParameter("@ProviderEnrollmentAgreeDate", SqlDbType.Date, 11, record, "ProviderEnrollmentAgreeDate");
                //cmd.AddRecordDataParameter("@ProviderContactName", SqlDbType.VarChar, 35, record, "ProviderContactName");
                //cmd.AddRecordDataParameter("@ProviderContactTitle", SqlDbType.VarChar, 25, record, "ProviderContactTitle");
                //cmd.AddRecordDataParameter("@ProviderContactEmail", SqlDbType.VarChar, 50, record, "ProviderContactEmail");
                //cmd.AddRecordDataParameter("@ProviderContactPhone", SqlDbType.VarChar, 10, record, "ProviderContactPhone");
                //cmd.AddRecordDataParameter("@ProviderContactPhoneExt", SqlDbType.VarChar, 10, record, "ProviderContactPhoneExt");
                //cmd.AddRecordDataParameter("@ProviderContactFaxNo", SqlDbType.VarChar, 10, record, "ProviderContactFaxNo");
                //cmd.AddRecordDataParameter("@ProviderNCPDPNo", SqlDbType.VarChar, 7, record, "ProviderNCPDPNo");
                //cmd.AddRecordDataParameter("@ProviderNCPDPFromDate", SqlDbType.Date, 8, record, "ProviderNCPDPFromDate");
                //cmd.AddRecordDataParameter("@ProviderNCPDPToDate", SqlDbType.Date, 8, record, "ProviderNCPDPToDate");
                //cmd.AddRecordDataParameter("@ProviderCliaNo", SqlDbType.VarChar, 10, record, "ProviderCliaNo");
                //cmd.AddRecordDataParameter("@ProviderEFTType", SqlDbType.VarChar, 1, record, "ProviderEFTType");
                //cmd.AddRecordDataParameter("@ProviderEFTFromDate", SqlDbType.Date, 8, record, "ProviderEFTFromDate");
                //cmd.AddRecordDataParameter("@ProviderEFTToDate", SqlDbType.Date, 8, record, "ProviderEFTToDate");
                //cmd.AddRecordDataParameter("@ProviderRoutingNumber", SqlDbType.VarChar, 9, record, "ProviderRoutingNumber");
                //cmd.AddRecordDataParameter("@ProviderAccountNumber", SqlDbType.VarChar, 17, record, "ProviderAccountNumber");
                //cmd.AddRecordDataParameter("@ProviderAccountType", SqlDbType.VarChar, 1, record, "ProviderAccountType");
                //cmd.AddRecordDataParameter("@ProviderAccountName", SqlDbType.VarChar, 50, record, "ProviderAccountName");
                //cmd.AddRecordDataParameter("@ProviderPOAExemptInd", SqlDbType.VarChar, 1, record, "ProviderPOAExemptInd");
                //cmd.AddRecordDataParameter("@ProviderCBSANumber", SqlDbType.VarChar, 5, record, "ProviderCBSANumber");
                //cmd.AddRecordDataParameter("@ProviderCBSAFromDate", SqlDbType.Date, 8, record, "ProviderCBSAFromDate");
                //cmd.AddRecordDataParameter("@ProviderCBSAToDate", SqlDbType.Date, 8, record, "ProviderCBSAToDate");
                //cmd.AddRecordDataParameter("@ProviderRebateFromDate", SqlDbType.Date, 8, record, "ProviderRebateFromDate");
                //cmd.AddRecordDataParameter("@ProviderRebateToDate", SqlDbType.Date, 8, record, "ProviderRebateToDate");
                //cmd.AddRecordDataParameter("@ProviderW9Indicator", SqlDbType.VarChar, 1, record, "ProviderW9Indicator");
                //cmd.AddRecordDataParameter("@ProviderOwnerTypeInd", SqlDbType.VarChar, 2, record, "ProviderOwnerTypeInd");
                //cmd.AddRecordDataParameter("@ProviderOwnerDocumentationReq", SqlDbType.VarChar, 1, record, "ProviderOwnerDocumentationReq");
                //cmd.AddRecordDataParameter("@ProviderBedSize", SqlDbType.VarChar, 2, record, "ProviderBedSize");
                //cmd.AddRecordDataParameter("@GroupProviderId", SqlDbType.VarChar, 11, record, "GroupProviderId");
                //cmd.AddRecordDataParameter("@IndividualProviderSSN", SqlDbType.VarChar, 9, record, "IndividualProviderSSN");
                //cmd.AddRecordDataParameter("@FiscalYearEnd", SqlDbType.VarChar,  4, record, "FiscalYearEnd");

                cmd.AddParameter("@CreateDate", SqlDbType.DateTime, EWSConfiguration.CurrentDateTime);
                cmd.AddParameter("@LineNumber", SqlDbType.Int, importRecord.LineNumber);
                cmd.AddParameter("@Filename", SqlDbType.VarChar, 255, importRecord.Filename);


                SqlParameter txnRecordIDParameter = cmd.AddParameter("@TxnRecordID", SqlDbType.Int, ParameterDirection.Output);

				int result = cmd.ExecuteNonQuery();

				if (result != 1)
				{
					throw new Exception(string.Format("No records inserted when executing " + InsertStoredProc + " for line number {0}.", importRecord.LineNumber));
				}

				if (txnRecordIDParameter.Value == DBNull.Value)
				{
                    throw new Exception(string.Format("No primary key value returned when executing " + InsertStoredProc + " for line number {0}.", importRecord.LineNumber));
				}

				int txnRecordID = (int)txnRecordIDParameter.Value;

				if(txnRecordID <= 0)
				{
                    throw new Exception(string.Format("Non-positive primary key value '{1}' returned when executing " + InsertStoredProc + " for line number {0}.", importRecord.LineNumber, txnRecordID));
				}

				return true;
			}
			catch(Exception ex)
			{
				Log.WriteError(string.Format("Error inserting record: {0}", ex));
				return false;
			}
		}

        private SqlDbType DBTypeForDataType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.String:
                    return SqlDbType.VarChar;
                case DataType.DateDDMMYY:
                case DataType.DateMMDDYYSlash:
                case DataType.DateMMDDYYYY:
                case DataType.DateMMDDYYYYSlash:
                case DataType.DateYYYYMMDD:
                case DataType.DateYYYYMMDDDash:
                    return SqlDbType.DateTime;
                case DataType.Currency:
                    return SqlDbType.Money;
                case DataType.Integer:
                    return SqlDbType.Int;
                case DataType.PhoneNumber:
                    return SqlDbType.VarChar;
                case DataType.SSN:
                    return SqlDbType.VarChar;
                case DataType.Time:
                    return SqlDbType.Time;
                case DataType.Composite:
                case DataType.Filler:
                case DataType.Lookup:
                default:
                    throw new Exception("No conversion available for " + dataType.ToString());
            }
        }

		protected override bool IsUpdateRecord(Record record)
		{
			return false;
		}

		protected override bool UpdateRecord(Record record)
		{
			throw new NotImplementedException();
		}

		protected override bool IsDeleteRecord(Record record)
		{
			return false;
		}

		protected override bool DeleteRecord(Record record)
		{
			throw new NotImplementedException();
		}

		protected override string TemplateName
		{
            get { return "Templates\\" + ConfigurationManager.AppSettings["TemplateFileName"]; }
		}
	}
}
