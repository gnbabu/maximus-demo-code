using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.TaxonomyLookup
{
    public static class TaxonomyLookupHelper
    {
        public static DataSet SelectNPITaxonomyIDs(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectNPITaxonomyIDs", "SelectNPITaxonomyIDs");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateTaxonomyType(Logging log, Guid threadId, int taxonomyTypeId, string taxonomyDesc)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_TYPE_ID", DbType.Int32, taxonomyTypeId, true));
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_NAME", DbType.String, taxonomyDesc, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                DataAccess.ExecuteScalar("usp_UpdateTaxonomyTypeByTaxonomyTypeID", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }


        public static bool makeNPPESAPICall(Logging log, Guid threadId,int taxonomyTypeId, string NPI, string taxonomy)
        {
            string URL = "https://npiregistry.cms.hhs.gov/api/";
            string urlParameters = string.Format("?number={0}&limit=1&skip=&pretty=on&version=2.1", NPI);
            string taxonomyDesc = string.Empty;

            try
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    string dataObjects = response.Content.ReadAsStringAsync().Result;
                    NPPESAPIResult np = JsonConvert.DeserializeObject<NPPESAPIResult>(dataObjects);
                    if (np.result_count > 0)
                    {
                        foreach (var d in np.results)
                        {
                            foreach (var t in d.taxonomies)
                            {
                                if (t.code.ToString().Equals(taxonomy))
                                {
                                    taxonomyDesc = t.desc.ToString().ToUpper();
                                    UpdateTaxonomyType(log, threadId, taxonomyTypeId, taxonomyDesc);
                                    client.Dispose();
                                    return false;
                                }
                            }
                        }
                        client.Dispose();
                        return true;
                    }
                    else
                    {
                        client.Dispose();
                        return true;
                    }
                }
                else
                {
                    log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0} {1}", (int)response.StatusCode, response.ReasonPhrase), Logging.LogPriority.Error);
                    client.Dispose();
                    return false;
                }
            }catch(Exception ex)
            {
                log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                return false;
            }
        }

        internal static string FetchNewNPIFromTaxonomy(Logging log, Guid threadId, string taxonomy)
        {
            try
            {
                string newNPI = string.Empty;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TAXONOMY_CODE", DbType.String, taxonomy, true));
                newNPI = DataAccess.ExecuteScalar("usp_FetchNewNPIFromTaxonomy", parameters);
                return newNPI;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("TaxonomyLookup Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

    }
}
