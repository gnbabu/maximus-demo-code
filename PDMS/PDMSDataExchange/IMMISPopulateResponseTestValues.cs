using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;


namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISPopulateResponseTestValues<TRequest, TResponse>
		where TRequest : mt.Request
		where TResponse : mt.Response
	{
		void PopulateTestValues(TRequest request, TResponse response, DataRow row);
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues
{
	public abstract class BaseMMISPopulateResponseTestValues<TRequest, TResponse> : MMISTransactionStep, IMMISPopulateResponseTestValues<TRequest, TResponse>
		where TRequest : mt.Request
		where TResponse : mt.Response
	{
		#region Private Members

		private int? _testRecordCount;

		#endregion

		#region Properties

		private int TestRecordCount
		{
			get
			{
				if (!_testRecordCount.HasValue)
				{
					_testRecordCount = Convert.ToInt32(AppSettings.Get("MMIS-RetrieveRecordCount", "10"));
				}

				return _testRecordCount.Value;
			}
			set
			{
				_testRecordCount = value;
			}
		}

		#endregion

		#region Constructors

		public BaseMMISPopulateResponseTestValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region Protected Methods

		public bool OKToProcessTestRecord()
		{
			bool process = false;
			if (TestRecordCount > 0)
			{
				process = true;
			}

			TestRecordCount--;

			return process;
		}

		#endregion

		public abstract void PopulateTestValues(TRequest request, TResponse response, DataRow row);
	}

}
namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.IndividualSubmit
{

	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderCaqhSubmitRequest, mt.ProviderCaqhSubmitResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderCaqhSubmitRequest request, mt.ProviderCaqhSubmitResponse response, DataRow row)
		{
			if (response != null)
			{
				string testValue = DateTime.Now.ToString("ssfff");
				response.transactionId = "888" + testValue;
				response.correlationId = "999" + testValue;
				List<mt.Error> errorList = new List<mt.Error>();
				response.errors = errorList.ToArray<mt.Error>();
			}
		}
	}
}
namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.GroupSubmit
{
	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderGroupCaqhSubmitRequest, mt.ProviderGroupCaqhSubmitResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderGroupCaqhSubmitRequest request, mt.ProviderGroupCaqhSubmitResponse response, DataRow row)
		{
			if (response != null)
			{
				string testValue = DateTime.Now.ToString("ssfff");
				response.transactionId = "888" + testValue;
				response.correlationId = "999" + testValue;
				List<mt.Error> errorList = new List<mt.Error>();
				response.errors = errorList.ToArray<mt.Error>();
			}
		}
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.IndividualRetrieve
{

	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderCaqhGetRequest, mt.ProviderCaqhGetResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderCaqhGetRequest request, mt.ProviderCaqhGetResponse response, DataRow row)
		{
			if (response != null)
			{
				
				string testValue = DateTime.Now.ToString("ssfff");
				List<mt.Error> errorList = new List<mt.Error>();
				response.errors = errorList.ToArray<mt.Error>();
				response.statusCode = string.Empty;
                int length = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));
                string medicaidPrefix = new string('2', length - 5);

				/// We will skip processing more than the number of rows defined in the AppSettings key
				if (OKToProcessTestRecord())
				{
					response.countyCode = "00";
					response.errorCode = string.Empty;
                    response.providerId = medicaidPrefix + testValue;
					response.statusCode = "201";
					response.correlationId = Methods.GetStringValue(row["CORRELATION_ID"]);
					string medicareId = Methods.GetStringValue(row["MEDICARE_ID"].ToString());

					if (string.IsNullOrWhiteSpace(medicareId))
					{
						response.medicareId = "111" + testValue;
					}
					else
					{
						response.medicareId = medicareId;
					}
				}
			}
		}
	}
}


namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.GroupRetrieve
{

	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderGroupCaqhGetRequest, mt.ProviderGroupCaqhGetResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderGroupCaqhGetRequest request, mt.ProviderGroupCaqhGetResponse response, DataRow row)
		{
			if (response != null)
			{
				// get test values
				string groupStatus = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
				string groupError = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
                int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));

				if (OKToProcessTestRecord())
				{
					string testValue = DateTime.Now.ToString("ssfff");
					response.correlationId = "555" + testValue;
					List<mt.Error> errorList = new List<mt.Error>();
					response.errors = errorList.ToArray<mt.Error>();
					response.errorCode = groupError;
					response.statusCode = groupStatus;
					response.pdmsId = Methods.GetStringValue(row[MMISShared.groupStagingPk].ToString());
					string medicaidId = Methods.GetStringValue(row["MEDICAID_ID"]);

					if (string.IsNullOrWhiteSpace(medicaidId))
					{
                        string medicaidPrefix = new string('1', medicaidLength - 5);
                        response.medicareId = medicaidPrefix + testValue;
					}
					else
					{
						response.medicareId = medicaidId;
					}
				}
			}
		}
	}
}


namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.GroupAffiliationsSubmit
{
	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderGroupAffiliationsCaqhSubmitRequest, mt.ProviderGroupAffiliationsCaqhSubmitResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderGroupAffiliationsCaqhSubmitRequest request, mt.ProviderGroupAffiliationsCaqhSubmitResponse response, DataRow row)
		{
			if (response != null)
			{
				string testValue = DateTime.Now.ToString("ssfff");
				response.transactionId = "666" + testValue;
				response.correlationId = "777" + testValue;
				List<mt.Error> errorList = new List<mt.Error>();
				response.errors = errorList.ToArray<mt.Error>();
			}
		}
	}
}


namespace MAXIMUS.DataExchange.PDMS.MMISPopulateResponseTestValues.GroupAffiliationsRetrieve
{

	public class SuccessValues : BaseMMISPopulateResponseTestValues<mt.ProviderGroupAffiliationsCaqhGetRequest, mt.ProviderGroupAffiliationsCaqhGetResponse>
	{
		#region Constructors

		public SuccessValues(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods

		#endregion

		public override void PopulateTestValues(mt.ProviderGroupAffiliationsCaqhGetRequest request, mt.ProviderGroupAffiliationsCaqhGetResponse response, DataRow row)
		{
			if (response != null)
			{
				// get test values
				string groupStatus = AppSettings.Get("MMIS-TestGroupStatus", Constants.MMISStatusType.Processed.ToString());
				string groupError = AppSettings.Get("MMIS-TestGroupErrors", string.Empty);
				string affilStatus = AppSettings.Get("MMIS-TestAffilStatus", Constants.MMISStatusType.Processed.ToString());
				string affilError = AppSettings.Get("MMIS-TestAffilErrors", string.Empty);
                int medicaidLength = int.Parse(AppSettings.Get("MMIS-TestMedicaidLength", Constants.MMISStatusType.Processed.ToString()));

				string testValue = DateTime.Now.ToString("ssfff");
				List<mt.Error> errorList = new List<mt.Error>();
				// List<mt.ProviderMembers> providerList = new List<mt.ProviderMembers>();
				response.correlationId = "444" + testValue;
				string medicareId = Methods.GetStringValue(row["MEDICAID_ID"]);
				response.errorCode = groupError;

				if (OKToProcessTestRecord())
				{
					response.statusCode = groupStatus;
					if (string.IsNullOrWhiteSpace(medicareId))
					{
                        string medicaidPrefix = new string('2', medicaidLength - 5);
                        response.medicareId = medicaidPrefix + testValue;
					}
					else
					{
						response.medicareId = medicareId;
					}
					response.pdmsId = Methods.GetStringValue(row[MMISShared.groupStagingPk].ToString());

					// add affiliates to response
					// group = ms.GetSubmittedStagingRecords(transactionType);
					// DataRow groupRecord = group.Tables[0].Rows[0];
					List<mt.Affiliates> affilList = new List<mt.Affiliates>();

					// update all transactions for each affiliation
					foreach (DataRow affiliate in row.GetChildRows(MMISShared.dbrGrps2dbtAffs))
					{
						mt.Affiliates affil = new mt.Affiliates();
						affil.action = Methods.GetStringValue(affiliate["ACTION"], false).ToString();
						affil.medicareId = Methods.GetStringValue(affiliate["BASE_MEDICAID_ID"], false).ToString();
						affil.memberEffectiveDate = Methods.GetDateValue(affiliate["EFFECTIVE_DATE"]);
						affil.memberTermDate = Methods.GetDateValue(affiliate["TERM_DATE"]);
						affil.npiId = Methods.GetStringValue(affiliate["NPI"], false).ToString();
						affil.pdmsId = Methods.GetStringValue(affiliate["MMIS_STAGING_AFFILIATE_PK"], false).ToString();
						affil.statusCode = affilStatus;
						affil.errorCode = affilError;
						affilList.Add(affil);
					}
					response.affiliates = affilList.ToArray<mt.Affiliates>();
				}
			}
		}
	}
}