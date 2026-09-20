using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using mt = MAXIMUS.DataExchange.PDMS.TN_MMIS_Service_Test;

namespace MAXIMUS.DataExchange.PDMS
{
	public interface IMMISGetResponse<TRequest, TResponse>
		where TRequest : mt.Request
		where TResponse : mt.Response
	{
		TResponse GetResponse(MMISShared ms, TRequest request, DataRow row, IMMISPopulateResponseTestValues<TRequest, TResponse> testValuesStep);
		bool OKToProcessResult(TResponse response);
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.IndividualSubmit
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderCaqhSubmitRequest, mt.ProviderCaqhSubmitResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion
		
		#region IMMISGetResponse Methods


		public mt.ProviderCaqhSubmitResponse GetResponse(MMISShared ms, mt.ProviderCaqhSubmitRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderCaqhSubmitRequest, mt.ProviderCaqhSubmitResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderCaqhSubmitResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.SubmitProvider(request);
			}
			else
			{
				response = new mt.ProviderCaqhSubmitResponse();

				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}
			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderCaqhSubmitResponse response)
		{
			/// For submit transactions, we always process.
			return true;
		}
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.GroupSubmit
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderGroupCaqhSubmitRequest, mt.ProviderGroupCaqhSubmitResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods


		public mt.ProviderGroupCaqhSubmitResponse GetResponse(MMISShared ms, mt.ProviderGroupCaqhSubmitRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderGroupCaqhSubmitRequest, mt.ProviderGroupCaqhSubmitResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderGroupCaqhSubmitResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.SubmitProviderGroup(request);
			}
			else
			{
				response = new mt.ProviderGroupCaqhSubmitResponse();
				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}
			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderGroupCaqhSubmitResponse response)
		{
			/// For submit transactions, we always process.
			return true;
		}
	}

}

namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.IndividualRetrieve
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderCaqhGetRequest, mt.ProviderCaqhGetResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods


		public mt.ProviderCaqhGetResponse GetResponse(MMISShared ms, mt.ProviderCaqhGetRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderCaqhGetRequest, mt.ProviderCaqhGetResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderCaqhGetResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.GetProvider(request);
			}
			else
			{
				response = new mt.ProviderCaqhGetResponse();

				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}

				
			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderCaqhGetResponse response)
		{
			/// For get transactions, we only process if we received a status code.
			return !string.IsNullOrEmpty(response.statusCode);
		}
	}
}



namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.GroupRetrieve
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderGroupCaqhGetRequest, mt.ProviderGroupCaqhGetResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods


		public mt.ProviderGroupCaqhGetResponse GetResponse(MMISShared ms, mt.ProviderGroupCaqhGetRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderGroupCaqhGetRequest, mt.ProviderGroupCaqhGetResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderGroupCaqhGetResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.GetProviderGroup(request);
			}
			else
			{
				response = new mt.ProviderGroupCaqhGetResponse();

				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}


			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderGroupCaqhGetResponse response)
		{
			/// For get transactions, we only process if we received a status code.
			return !string.IsNullOrEmpty(response.statusCode);
		}
	}
}

namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.GroupAffiliationsSubmit
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderGroupAffiliationsCaqhSubmitRequest, mt.ProviderGroupAffiliationsCaqhSubmitResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods


		public mt.ProviderGroupAffiliationsCaqhSubmitResponse GetResponse(MMISShared ms, mt.ProviderGroupAffiliationsCaqhSubmitRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderGroupAffiliationsCaqhSubmitRequest, mt.ProviderGroupAffiliationsCaqhSubmitResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderGroupAffiliationsCaqhSubmitResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.SubmitProviderGroupAffiliations(request);
			}
			else
			{
				response = new mt.ProviderGroupAffiliationsCaqhSubmitResponse();
				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}
			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderGroupAffiliationsCaqhSubmitResponse response)
		{
			/// For submit transactions, we always process.
			return true;
		}
	}

}



namespace MAXIMUS.DataExchange.PDMS.MMISGetResponse.GroupAffiliationsRetrieve
{
	public class GetNormalResponse : MMISTransactionStep, IMMISGetResponse<mt.ProviderGroupAffiliationsCaqhGetRequest, mt.ProviderGroupAffiliationsCaqhGetResponse>
	{
		#region Constructors

		public GetNormalResponse(Guid threadID)
			: base(threadID)
		{

		}

		#endregion

		#region IMMISGetResponse Methods


		public mt.ProviderGroupAffiliationsCaqhGetResponse GetResponse(MMISShared ms, mt.ProviderGroupAffiliationsCaqhGetRequest request, DataRow row, IMMISPopulateResponseTestValues<mt.ProviderGroupAffiliationsCaqhGetRequest, mt.ProviderGroupAffiliationsCaqhGetResponse> testValuesStep)
		{
			/// TODO: See if can make this work without making MMISShared class public...
			mt.ProviderGroupAffiliationsCaqhGetResponse response;

			if (AppSettings.Get("MMIS-InterfaceTesting", bool.TrueString).ToLower() == bool.FalseString.ToLower())
			{
				// submit to the MMIS
				mt.CAQH_DOTNET_WebServiceSoapClient mmis = ms.GetServiceClient();
				response = mmis.GetProviderGroupAffiliations(request);
			}
			else
			{
				response = new mt.ProviderGroupAffiliationsCaqhGetResponse();

				if (testValuesStep != null)
				{
					testValuesStep.PopulateTestValues(request, response, row);
				}
			}

			return response;
		}

		#endregion


		public bool OKToProcessResult(mt.ProviderGroupAffiliationsCaqhGetResponse response)
		{
			/// For get transactions, we only process if we received a status code.
			return !string.IsNullOrEmpty(response.statusCode);
		}
	}
}