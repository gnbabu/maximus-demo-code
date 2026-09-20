using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;
using System.Data;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISPopulateRequest<TRequest>
        where TRequest : MMISRequest
	{
		TRequest PopulateRequest(DataRow row, MMISShared ms);
		void PopulateTransactionData(TransactionData data, DataRow row);

	}
}
namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.GroupSubmit
{
	public class Populate : MMISTransactionStep, IMMISPopulateRequest<SubmitWaiverProvider>
	{
		#region Constants

		const string COLUMN_NAME_TRANSACTION_QUEUE_ID = "TRANSACTION_QUEUE_ID";

		#endregion

		#region Constructors

		public Populate(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		public SubmitWaiverProvider PopulateRequest(DataRow row, MMISShared ms)
		{
			// create the object to submit
			SubmitWaiverProvider group = new SubmitWaiverProvider();
			
            int partyId = Convert.ToInt32(row["PARTY_ID"].ToString());
            group.pdmsProviderId = row["PARTY_ID"].ToString();
			group.actionFlag = row["ACTION"].ToString();
            group.transactionId = row[COLUMN_NAME_TRANSACTION_QUEUE_ID].ToString();
            group.organizationId = row["MEDICAID_ID"].ToString();
            group.businessName = row["NAME"].ToString();
            group.profitStatus = row["PROFIT_STATUS"].ToString();
            if (!string.IsNullOrEmpty(row["EFFECTIVE_DATE"].ToString()))
            {
                group.eligibilityStartDate = Methods.GetDateValue(row["EFFECTIVE_DATE"]);
            }
            if (!String.IsNullOrWhiteSpace(row["END_DATE"].ToString()))
            {
                group.eligibilityEndDate = Methods.GetDateValue(row["END_DATE"]);
            }
			// populate addresses
            foreach (DataRow address in row.GetChildRows(MMISShared.dbrGrps2Addrs))
            {
                string type = Methods.GetStringValue(address["ADDRESS_TYPE"], false).ToString();
                string groupName = Methods.GetStringValue(address["NAME"], false).ToString().ToUpper();
                string street1 = Methods.GetStringValue(address["STREET1"], false).ToString().ToUpper();
                string street2 = Methods.GetStringValue(address["STREET2"], false).ToString().ToUpper();
                string city = Methods.GetStringValue(address["CITY"], false).ToString().ToUpper();
                string county = Methods.GetStringValue(address["COUNTY"], false).ToString().ToUpper();
                string state = Methods.GetStringValue(address["STATE"], false).ToString().ToUpper();
                string zip = Methods.GetStringValue(address["ZIP5"], false).ToString();
                string zipFour = Methods.GetStringValue(address["ZIP4"], false).ToString();
                string phone = Methods.GetStringValue(address["PHONE"], false).ToString();
                string phoneExt = Methods.GetStringValue(address["PHONE_EXT"], false).ToString();
                string fax = Methods.GetStringValue(address["FAX"], false).ToString();
                string email = Methods.GetStringValue(address["EMAIL"], false).ToString().ToUpper();

                switch (type)
                {
                    case "C":
                        group.contactEMail = email;
                        break;

                    case "S":
                        group.address = street1;
                        group.address2 = street2;
                        group.city = city;
                        group.state = state;
                        group.zip = zip;
                        group.zipPlusFour = zipFour;
                        group.phone = phone;
                        break;

                    case "M":
                        group.mailAddress = street1;
                        group.mailAddress2 = street2;
                        group.mailCity = city;
                        group.mailState = state;
                        group.mailZip = zip;
                        group.mailZip4 = zipFour;
                        group.mailPhone = phone;
                        break;
                }
            }
            group.dba = row["DBA"].ToString();

            if (!string.IsNullOrEmpty(row["TERM_DATE"].ToString()))
            {
                group.termDate = Methods.GetDateValue(row["TERM_DATE"]);
            } 
            group.termReason = row["TERM_REASON"].ToString();
            group.ftin = row["TAX_ID_VALUE"].ToString();
            group.ftinType = row["FTIN_TYPE"].ToString();
            group.taxFormType = row["TAX_FORM_TYPE"].ToString();
            group.taxClassificationType = row["ORGANIZATION_CODE"].ToString();

            group.routingNumber = row["ROUTING_NUMBER"].ToString();
            group.accountNumber = row["ACCOUNT_NUMBER"].ToString();
            group.accountType = row["ACCOUNT_TYPE"].ToString();
            group.accountName = row["ACCOUNT_NAME"].ToString().ToUpper();

            group.bankName = row["BANK_NAME"].ToString().ToUpper();
            group.accountTypeEntity = row["ACCOUNT_TYPE_ENTITY"].ToString().ToUpper();
            group.debitIndicator = row["DEBIT_INDICATOR"].ToString().ToUpper();

            return group;
		}

		public void PopulateTransactionData(TransactionData data, DataRow row)
		{
			if (row == null)
			{
				return;
			}

			data.PrimaryKey = Convert.ToInt32(row[MMISShared.groupStagingPk]);
			if (row[COLUMN_NAME_TRANSACTION_QUEUE_ID] != DBNull.Value)
			{
				data.TransactionQueueID = Convert.ToInt32(row[COLUMN_NAME_TRANSACTION_QUEUE_ID]);
			}

			data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
			if (row["MEDICAID_PK"] != DBNull.Value)
			{
				data.MedicaidPK = Convert.ToInt32(row["MEDICAID_PK"]);
			}
            data.SakTransID = "Submitted";
		}
	}
}
namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.IndividualSubmit
{
    public class Populate : MMISTransactionStep, IMMISPopulateRequest<SubmitWaiverProvider>
	{
		#region Constants

		const string dbtProvs = "PROVIDER_STAGING";
		const string dbtAddr = "ADDRESS_STAGING";
		const string dbrProv2Addrs = dbtProvs + "2" + dbtAddr;
		const string dbfPK = "PROVIDER_STAGING_PK";
		const string COLUMN_NAME_TRANSACTION_QUEUE_ID = "TRANSACTION_QUEUE_ID";

		#endregion
		
		#region Constructors

		public Populate(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

        public SubmitWaiverProvider PopulateRequest(DataRow row, MMISShared ms)
		{
            // create the object to submit
            SubmitWaiverProvider individual = new SubmitWaiverProvider();

            individual.pdmsProviderId = row["PARTY_ID"].ToString();
            individual.actionFlag = row["ACTION"].ToString();
            individual.transactionId = row[COLUMN_NAME_TRANSACTION_QUEUE_ID].ToString();
            individual.organizationId = row["MEDICARE_ID"].ToString();
            individual.lastName = row["LAST_NAME"].ToString();
            individual.firstName = row["FIRST_NAME"].ToString();
            individual.middleName = row["MIDDLE_NAME"].ToString();
            // Bug 6761 - The  value that Maximus sends in the MAX-PROV-NAME filed must be in upper case and formatted 
            // as ‘LAST NAME, FIRST NAME MIDDLE NAME’ when the name is for an individual.
            individual.businessName = individual.lastName.ToUpper() + ", " + individual.firstName.ToUpper() + " " + individual.middleName.ToUpper();
            individual.gender = row["GENDER"].ToString();
            if (!string.IsNullOrEmpty(row["BIRTH_DATE_TIME"].ToString()))
            {
                individual.dateofBirth = Methods.GetDateValue(row["BIRTH_DATE_TIME"]);
            }

            individual.profitStatus = row["PROFIT_STATUS"].ToString();
            
            if (!string.IsNullOrEmpty(row["EFFECTIVE_DATE"].ToString()))
            {
                individual.eligibilityStartDate = Methods.GetDateValue(row["EFFECTIVE_DATE"]);
            }
            if (!String.IsNullOrWhiteSpace(row["END_DATE"].ToString()))
            {
                individual.eligibilityEndDate = Methods.GetDateValue(row["END_DATE"]);
            }
            // populate addresses
            foreach (DataRow address in row.GetChildRows(MMISShared.dbrProv2Addrs))
            {
                int addressType = Convert.ToInt32(address["ADDRESS_STAGING_TYPE_ID"]);
                string providerName = Methods.GetStringValue(address["NAME"], false).ToString().ToUpper();
                string street1 = Methods.GetStringValue(address["ADDRESS_STREET1"], false).ToString().ToUpper();
                string street2 = Methods.GetStringValue(address["ADDRESS_STREET2"], false).ToString().ToUpper();
                string city = Methods.GetStringValue(address["ADDRESS_CITY"], false).ToString().ToUpper();
                string state = Methods.GetStringValue(address["ADDRESS_STATE"], false).ToString().ToUpper();
                string zip = Methods.GetStringValue(address["ADDRESS_ZIP"], false).ToString();
                string zipFour = Methods.GetStringValue(address["ADDRESS_ZIP_FOUR"], false).ToString();
                string phone = Methods.GetStringValue(address["ADDRESS_PHONE"], false).ToString();
                string phoneExt = Methods.GetStringValue(address["ADDRESS_PHONE_EXT"], false).ToString();
                string fax = Methods.GetStringValue(address["ADDRESS_FAX"], false).ToString();
                string email = Methods.GetStringValue(address["ADDRESS_EMAIL"], false).ToString().ToUpper();


                switch (addressType)
                {
                    case (int)Enumerations.ContactMechanismRoleTypeId.MailTo:
                        individual.mailAddress = street1;
                        individual.mailAddress2 = street2;
                        individual.mailCity = city;
                        individual.mailState = state;
                        individual.mailZip = zip;
                        individual.mailZip4 = zipFour;
                        individual.mailPhone = phone;
                        break;

                    case (int)Enumerations.ContactMechanismRoleTypeId.Servicing:
                        individual.address = street1;
                        individual.address2 = street2;
                        individual.city = city;
                        individual.state = state;
                        individual.zip = zip;
                        individual.zipPlusFour = zipFour;
                        individual.phone = phone;
                        break;

                    case (int)Enumerations.ContactMechanismRoleTypeId.Credentialing:
                        individual.contactEMail = email;
                        break;
                }
            }
            individual.dba = row["DBA"].ToString();
            if (!string.IsNullOrEmpty(row["TERM_DATE_TIME"].ToString()))
            {
                individual.termDate = Methods.GetDateValue(row["TERM_DATE_TIME"]);
            }
            individual.termReason = row["TERM_REASON"].ToString(); ;
            individual.ftin = row["TAX_ID"].ToString();
            individual.ftinType = row["FTIN_TYPE"].ToString();
            individual.taxFormType = row["TAX_FORM_TYPE"].ToString();
            individual.taxClassificationType = row["TAX_CLS_TYPE"].ToString();

            // todo update proc usp_InsertMMISStagingindividuals to include ACH_REQUEST table
            individual.routingNumber = row["ROUTING_NUMBER"].ToString();
            individual.accountNumber = row["ACCOUNT_NUMBER"].ToString();
            individual.accountType = row["ACCOUNT_TYPE"].ToString();
            individual.accountName = row["ACCOUNT_NAME"].ToString().ToUpper();

            individual.bankName = row["BANK_NAME"].ToString().ToUpper(); ;
            individual.accountTypeEntity = row["ACCOUNT_TYPE_ENTITY"].ToString().ToUpper(); ;
            individual.debitIndicator = row["DEBIT_INDICATOR"].ToString().ToUpper(); ;

            return individual;
		}

		public void PopulateTransactionData(TransactionData data, DataRow row)
		{
			if (row == null)
			{
				return;
			}

			data.PrimaryKey = Convert.ToInt32(row[dbfPK]);
			
			if (row[COLUMN_NAME_TRANSACTION_QUEUE_ID] != DBNull.Value)
			{
				data.TransactionQueueID = Convert.ToInt32(row[COLUMN_NAME_TRANSACTION_QUEUE_ID].ToString());
			}

			data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
			if (row["MEDICAID_PK"] != DBNull.Value)
			{
				data.MedicaidPK = Convert.ToInt32(row["MEDICAID_PK"]);
			}
            data.PDMSStatus = Constants.PDMSStatusType.SubmittedtoMMIS;
            data.SakTransID = "Submitted";
		}
	}

}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.IndividualRetrieve
{
	public class Populate : MMISTransactionStep, IMMISPopulateRequest<RetrieveWaiverProvider>
	{
		#region Constants

		//const string dbtProvs = "PROVIDER_STAGING";
		//const string dbtAddr = "ADDRESS_STAGING";
		//const string dbrProv2Addrs = dbtProvs + "2" + dbtAddr;
		const string COLUMN_NAME_PROVIDER_STAGING_PK = "PROVIDER_STAGING_PK";
		//const string COLUMN_NAME_TRANSACTION_QUEUE_ID = "TRANSACTION_QUEUE_ID";

		#endregion

		#region Constructors

		public Populate(Guid threadID)
			: base(threadID)
		{

		}

		#endregion



        public RetrieveWaiverProvider PopulateRequest(DataRow row, MMISShared ms)
		{
            // create the object to submit
            RetrieveWaiverProvider request = new RetrieveWaiverProvider();

            //// populate request object
            //request.sak_trans = Methods.GetStringValue(row["SAK_TRANS"]);

            return request;
		}

		public void PopulateTransactionData(TransactionData data, DataRow row)
		{
			if (row == null)
			{
				return;
			}

			data.PrimaryKey = Convert.ToInt32(row[COLUMN_NAME_PROVIDER_STAGING_PK]);
			if (row["TRANSACTION_QUEUE_ID"] != DBNull.Value)
			{
				data.TransactionQueueID = Convert.ToInt32(row["TRANSACTION_QUEUE_ID"].ToString());
			}
			data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
			if (row["MEDICAID_PK"] != DBNull.Value)
			{
				data.MedicaidPK = Methods.GetIntValue(row["MEDICAID_PK"].ToString());
			}
			data.SakTransID = "Processed";
            data.PDMSStatus = Constants.PDMSStatusType.Processed;
		}
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.GroupRetrieve
{
    public class Populate : MMISTransactionStep, IMMISPopulateRequest<RetrieveWaiverProvider>
	{
		#region Constructors

		public Populate(Guid threadID)
			: base(threadID)
		{

		}

		#endregion



        public RetrieveWaiverProvider PopulateRequest(DataRow row, MMISShared ms)
		{
            RetrieveWaiverProvider request = new RetrieveWaiverProvider();
			
            //// populate the provider object
            //request.sak_trans = Methods.GetStringValue(row["SAK_TRANS"]);

			return request;
		}

		public void PopulateTransactionData(TransactionData data, DataRow row)
		{
			if (row == null)
			{
				return;
			}

			data.PrimaryKey = Convert.ToInt32(row[MMISShared.groupStagingPk]);
			if (row["TRANSACTION_QUEUE_ID"] != DBNull.Value)
			{
				data.TransactionQueueID = Convert.ToInt32(row["TRANSACTION_QUEUE_ID"].ToString());
			}
			data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
			if (row["MEDICAID_PK"] != DBNull.Value)
			{
				data.MedicaidPK = Convert.ToInt32(row["MEDICAID_PK"].ToString());
			}

            data.SakTransID = "Processed";
		}
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.WaiverServicesSubmit
{
    public class Populate : MMISTransactionStep, IMMISPopulateRequest<SubmitWaiverServices>
    {
        #region Constants

        const string COLUMN_NAME_TRANSACTION_QUEUE_ID = "TRANSACTION_QUEUE_ID";

        #endregion

        #region Constructors

        public Populate(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        public SubmitWaiverServices PopulateRequest(DataRow row, MMISShared ms)
        {
            // create the object to submit
            SubmitWaiverServices waiverservice = new SubmitWaiverServices();

            waiverservice.actionFlag = row["ACTION"].ToString();
            waiverservice.transactionId = row["MMIS_STAGING_WAIVER_SERVICES_ID"].ToString();
            waiverservice.pdmsProviderId = row["PARTY_ID"].ToString();
            waiverservice.organizationId = row["ORGANIZATION_ID"].ToString();

            waiverservice.serviceType = row["SERVICE_CODE"].ToString();
            waiverservice.programCode = row["PROGRAM_CODE"].ToString();

            if (!string.IsNullOrEmpty(row["SERV_PRG_FROMDATE"].ToString()))
            {
                waiverservice.programBeginDate = Methods.GetDateValue(row["SERV_PRG_FROMDATE"].ToString());
            }
            if (!string.IsNullOrEmpty(row["SERV_PRG_TODATE"].ToString()))
            {
                waiverservice.programEndDate = Methods.GetDateValue(row["SERV_PRG_TODATE"].ToString());
            }

            return waiverservice;
        }

        public void PopulateTransactionData(TransactionData data, DataRow row)
        {
            if (row == null)
            {
                return;
            }

            data.PrimaryKey = Convert.ToInt32(row["MMIS_STAGING_WAIVER_SERVICES_ID"]);

            if (row[COLUMN_NAME_TRANSACTION_QUEUE_ID] != DBNull.Value)
            {
                data.TransactionQueueID = Convert.ToInt32(row[COLUMN_NAME_TRANSACTION_QUEUE_ID].ToString());
            }

            if (row["MEDICAID_PK"] != DBNull.Value)
            {
                data.MedicaidPK = Methods.GetIntValue(row["MEDICAID_PK"].ToString());
            }
            data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
            data.PDMSStatus = Constants.PDMSStatusType.SubmittedtoMMIS;
            data.SakTransID = "Submitted";
        }
    }
}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateRequest.WaiverServicesRetrieve
{
    public class Populate : MMISTransactionStep, IMMISPopulateRequest<RetrieveWaiverServices>
    {
        #region Constants

        const string COLUMN_NAME_PK = "MMIS_STAGING_WAIVER_SERVICES_ID";

        #endregion

        #region Constructors

        public Populate(Guid threadID)
            : base(threadID)
        {

        }

        #endregion

        public RetrieveWaiverServices PopulateRequest(DataRow row, MMISShared ms)
        {
            // create the object to submit
            RetrieveWaiverServices request = new RetrieveWaiverServices();

            return request;
        }

        public void PopulateTransactionData(TransactionData data, DataRow row)
        {
            if (row == null)
            {
                return;
            }

            data.PrimaryKey = Convert.ToInt32(row[COLUMN_NAME_PK]);
            if (row["TRANSACTION_QUEUE_ID"] != DBNull.Value)
            {
                data.TransactionQueueID = Convert.ToInt32(row["TRANSACTION_QUEUE_ID"].ToString());
            }
            data.PartyID = Convert.ToInt32(row["PARTY_ID"]);
            if (row["MEDICAID_PK"] != DBNull.Value)
            {
                data.MedicaidPK = Methods.GetIntValue(row["MEDICAID_PK"].ToString());
            }
            data.SakTransID = "Processed";
            data.PDMSStatus = Constants.PDMSStatusType.Processed;
        }
    }
}