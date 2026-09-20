using Corp.Core.Libraries;
using Corp.Core.Libraries.FI.ClaimsSearchReference;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDMSRestServices.Controllers.Facade;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using CON = MAXIMUS.Core.Libraries.Constants;
using PDMSRestServices.Models.ClaimsSearch;
using static MAXIMUS.Core.Libraries.Constants;
using System.Web;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class ClaimSearchController : ControllerBase
    {
        [HttpPost]
        [Route("ClaimSearch")]
        public IActionResult ClaimSearch([FromBody] PDMSRestServices.Models.SearchClaimsRequest payload)
        {

            string payloaddata = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            PDMSRestServices.Models.SearchClaimsResponse searchClaimsResponse = new PDMSRestServices.Models.SearchClaimsResponse();
            try
            {
                if (payload == null)
                {
                    return NotFound();
                }
                DataSet dsresponse = new DataSet();
                if (payload.AllowDBSearch != null && payload.AllowDBSearch == true)
                {
                    if (payload.ClaimType != null && payload.ClaimType != "")
                    {
                        if (payload.ClaimType == "D")
                        {
                            payload.ClaimType = "0";
                        }
                        if (payload.ClaimType == "P")
                        {
                            payload.ClaimType = "2";
                        }
                        if (payload.ClaimType == "I")
                        {
                            payload.ClaimType = "1";
                        }
                    }

                    dsresponse = PDMSRestServices.Controllers.Facade.ClaimSearch.GetClaimsDBData(payload);
                }
                else
                {
                    payload.TotalChargesSpecified = true;
                    var response = PDMSRestServices.Controllers.Facade.ClaimSearch.CallClaimsSearchService(payload);

                    try
                    {
                        Logging log = new Logging(Guid.NewGuid(), "ClaimSearchAPI_Result:");
                        log.CreateLogEntry(string.Format("{0} {1}", "", response), Logging.LogPriority.Information);
                    }
                    catch { }

                    var xDoc = XDocument.Parse(response);
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.XmlResolver = null;
                    XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);
                    dsresponse.ReadXml(xmlReader);
                }
                DataTable dtResponseHeader = null;
                DataTable dtClaimsResponse = null;
                DataTable dtClaimSearchResponse = null;
                int totalRecord = 0;
                IEnumerable<DataRow> dtResponse;
                string ClaimType = string.Empty;
                string DentalclaimType = string.Empty;
                string ProfclaimType = string.Empty;
                string InsticlaimType = string.Empty;

                if (Helper.HasRows(dsresponse))
                {
                    if (dsresponse.Tables.Contains("ResponseHeader"))
                    {
                        dtResponseHeader = dsresponse.Tables["ResponseHeader"];
                    }
                    if (dsresponse.Tables.Contains("ClaimHeaderResponse"))
                    {
                        dtClaimsResponse = dsresponse.Tables["ClaimHeaderResponse"];
                    }

                    if (dsresponse.Tables.Contains("SearchClaimsResponse"))
                    {
                        dtClaimSearchResponse = dsresponse.Tables["SearchClaimsResponse"];

                        if (Helper.HasRows(dtClaimSearchResponse))
                        {
                            if (dtClaimSearchResponse.Rows[0].Table.Columns.Contains("TotalRecords"))
                            {
                                if (!string.IsNullOrEmpty(dtClaimSearchResponse.Rows[0]["TotalRecords"].ToString()))
                                {
                                    totalRecord = Convert.ToInt32(dtClaimSearchResponse.Rows[0]["TotalRecords"].ToString());
                                }
                            }
                        }
                    }

                    //Reading the claim type from xml
                    if (!string.IsNullOrEmpty(payload.ClaimType))
                    {
                        if (Helper.HasRows(dtClaimsResponse))
                        {
                            foreach (DataRow dr in dtClaimsResponse.Rows)
                            {
                                if (dr["ClaimType"].ToString().ToUpper() == "DENTAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "D" || dr["ClaimType"].ToString() == "0")
                                    DentalclaimType = dr["ClaimType"].ToString();
                                if (dr["ClaimType"].ToString().ToUpper() == "PROFESSIONAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "P" || dr["ClaimType"].ToString() == "2")
                                    ProfclaimType = dr["ClaimType"].ToString();
                                if (dr["ClaimType"].ToString().ToUpper() == "INSTITUTIONAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "I" || dr["ClaimType"].ToString() == "1")
                                    InsticlaimType = dr["ClaimType"].ToString();
                            }
                        }
                    }
                    if (Helper.HasRows(dtClaimsResponse))
                    {
                        if (!String.IsNullOrEmpty(payload.ClaimType) && String.IsNullOrEmpty(payload.Status))
                        {
                            dtResponse = from row in dtClaimsResponse.AsEnumerable()
                                         where row.Field<string>("ClaimType").ToLower().Trim() == payload.ClaimType.ToLower()
                                         select row;
                            if (dtResponse.Any())
                            {
                                dtClaimsResponse = dtResponse.CopyToDataTable();
                            }
                        }
                        else if (!String.IsNullOrEmpty(payload.ClaimType) && !String.IsNullOrEmpty(payload.Status))
                        {
                            dtResponse = from row in dtClaimsResponse.AsEnumerable()
                                         where (row.Field<string>("ClaimType").ToLower().Trim() == payload.ClaimType.ToLower() &&
                                         row.Field<string>("ClaimStatus").ToLower().Trim() == payload.Status.ToLower())
                                         select row;
                            if (dtResponse.Any())
                            {
                                dtClaimsResponse = dtResponse.CopyToDataTable();
                            }
                        }
                        else if (!String.IsNullOrEmpty(payload.Status))
                        {
                            dtResponse = from row in dtClaimsResponse.AsEnumerable()
                                         where (row.Field<string>("ClaimStatus").ToLower().Trim() == payload.Status.ToLower())
                                         select row;
                            if (dtResponse.Any())
                            {
                                dtClaimsResponse = dtResponse.CopyToDataTable();
                            }
                        }
                        else if (String.IsNullOrEmpty(payload.ClaimType) && !String.IsNullOrEmpty(payload.PayorType))
                        {
                            dtResponse = from row in dtClaimsResponse.AsEnumerable()
                                         where (row.Field<string>("PayorType")) == payload.PayorType
                                         select row;
                            if (dtResponse.Any())
                            {
                                dtClaimsResponse = dtResponse.CopyToDataTable();
                            }
                        }
                    }
                }

                PDMSRestServices.Models.ResponseHeader responseHeader = new PDMSRestServices.Models.ResponseHeader();
                List<ClaimHeaderResponse> claims = new List<ClaimHeaderResponse>();

                if (payload.AllowDBSearch == null || payload.AllowDBSearch == false)
                {
                    if (dtResponseHeader != null && dtResponseHeader.Rows != null && dtResponseHeader.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtResponseHeader.Rows)
                        {
                            if (dtResponseHeader.Columns.Contains("SITransactionKey"))
                            {
                                responseHeader.SITransactionKey = Convert.ToString(row["SITransactionKey"]);
                            }
                            if (dtResponseHeader.Columns.Contains("ResponseCode"))
                            {
                                responseHeader.ResponseCode = Convert.ToString(row["ResponseCode"]);
                            }
                            if (dtResponseHeader.Columns.Contains("ResponseType"))
                            {
                                responseHeader.ResponseType = Convert.ToString(row["ResponseType"]);
                            }
                            if (dtResponseHeader.Columns.Contains("ResponseMessage"))
                            {
                                responseHeader.ResponseMessage = Convert.ToString(row["ResponseMessage"]);
                            }
                            if (dtResponseHeader.Columns.Contains("ResponseDetails"))
                            {
                                responseHeader.ResponseDetails = Convert.ToString(row["ResponseDetails"]);
                            }
                        }
                    }
                    if (dtClaimsResponse != null && dtClaimsResponse.Rows != null && dtClaimsResponse.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtClaimsResponse.Rows)
                        {
                            ClaimHeaderResponse claimHeaderResponse = new ClaimHeaderResponse();

                            if (dtClaimsResponse.Columns.Contains("PayorType"))
                            {
                                if (row["PayorType"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["PayorType"])))
                                    claimHeaderResponse.PayorType = Convert.ToString(row["PayorType"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ICN"))
                            {
                                if (row["ICN"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ICN"])))
                                    claimHeaderResponse.ICN = Convert.ToString(row["ICN"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ClaimType"))
                            {
                                if (row["ClaimType"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimType"])))
                                    claimHeaderResponse.ClaimType = Convert.ToString(row["ClaimType"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ClaimStatus"))
                            {
                                if (row["ClaimStatus"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimStatus"])))
                                    claimHeaderResponse.ClaimStatus = Convert.ToString(row["ClaimStatus"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("PatientAccountNumber"))
                            {
                                if (row["PatientAccountNumber"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["PatientAccountNumber"])))
                                    claimHeaderResponse.PatientAccountNumber = Convert.ToString(row["PatientAccountNumber"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("MemberId"))
                            {
                                if (row["MemberId"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["MemberId"])))
                                    claimHeaderResponse.MemberId = Convert.ToString(row["MemberId"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("MemberName"))
                            {
                                if (row["MemberName"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["MemberName"])))
                                    claimHeaderResponse.MemberName = Convert.ToString(row["MemberName"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("TotalPaidAmount"))
                            {
                                if (row["TotalPaidAmount"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["TotalPaidAmount"])))
                                    claimHeaderResponse.TotalPaidAmount = Convert.ToDouble(row["TotalPaidAmount"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ClaimPaidDate"))
                            {
                                if (row["ClaimPaidDate"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimPaidDate"])))
                                    claimHeaderResponse.ClaimPaidDate = Convert.ToDateTime(row["ClaimPaidDate"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("AdjudicationDate"))
                            {
                                if (row["AdjudicationDate"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["AdjudicationDate"])))
                                    claimHeaderResponse.AdjudicationDate = Convert.ToDateTime(row["AdjudicationDate"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ClaimSubmissionDate"))
                            {
                                if (row["ClaimSubmissionDate"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimSubmissionDate"])))
                                    claimHeaderResponse.ClaimSubmissionDate = Convert.ToDateTime(row["ClaimSubmissionDate"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("TotalCharges"))
                            {
                                if (row["TotalCharges"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["TotalCharges"])))
                                    claimHeaderResponse.TotalCharges = Convert.ToDouble(row["TotalCharges"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("FromDOS"))
                            {
                                if (row["FromDOS"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["FromDOS"])))
                                    claimHeaderResponse.FromDOS = Convert.ToDateTime(row["FromDOS"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("ThruDOS"))
                            {
                                if (row["ThruDOS"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ThruDOS"])))
                                    claimHeaderResponse.ThruDOS = Convert.ToDateTime(row["ThruDOS"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("OriginalClaimICN"))
                            {
                                if (row["OriginalClaimICN"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["OriginalClaimICN"])))
                                    claimHeaderResponse.OriginalClaimICN = Convert.ToString(row["OriginalClaimICN"]);
                            }

                            if (dtClaimsResponse.Columns.Contains("RemittanceAdviceDate"))
                            {
                                if (row["RemittanceAdviceDate"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["RemittanceAdviceDate"])))
                                    claimHeaderResponse.RemittanceAdviceDate = Convert.ToDateTime(row["RemittanceAdviceDate"]).ToString("MM/dd/yyyy");
                            }

                            claims.Add(claimHeaderResponse);
                        }
                    }

                    searchClaimsResponse.ClaimHeaderResponse = claims;
                    searchClaimsResponse.ResponseHeader = responseHeader;
                    searchClaimsResponse.TotalRecords = totalRecord;
                    var totalPages = (int)Math.Ceiling((double)searchClaimsResponse.TotalRecords / Convert.ToInt16(payload.PageSize));
                    searchClaimsResponse.TotalPages = totalPages;
                }
                else if (payload.AllowDBSearch != null && payload.AllowDBSearch == true)
                {
                    dtClaimsResponse = dsresponse.Tables[0];
                    foreach (DataRow row in dtClaimsResponse.Rows)
                    {
                        ClaimHeaderResponse claimHeaderResponse = new ClaimHeaderResponse();
                        if (dtClaimsResponse.Columns.Contains("Claim_ID"))
                        {
                            if (row["Claim_ID"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["Claim_ID"])))
                            {
                                string rowClaimID = Convert.ToString(row["Claim_ID"]);
                                // rowClaimID = Convert.ToString(row[13]);
                                claimHeaderResponse.ClaimID = rowClaimID;
                            }
                        }
                        if (dtClaimsResponse.Columns.Contains("PayorType"))
                        {
                            if (row["PayorType"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["PayorType"])))
                                claimHeaderResponse.PayorType = Convert.ToString(row["PayorType"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("ICN"))
                        {
                            if (row["ICN"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ICN"])))
                                claimHeaderResponse.ICN = Convert.ToString(row["ICN"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("ClaimType"))
                        {
                            if (row["ClaimType"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimType"])))
                                claimHeaderResponse.ClaimType = Convert.ToString(row["ClaimType"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("ClaimStatus"))
                        {
                            if (row["ClaimStatus"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ClaimStatus"])))
                                claimHeaderResponse.ClaimStatus = Convert.ToString(row["ClaimStatus"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("PatientAccountNumber"))
                        {
                            if (row["PatientAccountNumber"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["PatientAccountNumber"])))
                                claimHeaderResponse.PatientAccountNumber = Convert.ToString(row["PatientAccountNumber"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("MemberId"))
                        {
                            if (row["MemberId"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["MemberId"])))
                                claimHeaderResponse.MemberId = Convert.ToString(row["MemberId"]);
                        }


                        if (dtClaimsResponse.Columns.Contains("TotalPaidAmount"))
                        {
                            if (row["TotalPaidAmount"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["TotalPaidAmount"])))
                                claimHeaderResponse.TotalPaidAmount = Convert.ToDouble(row["TotalPaidAmount"]);
                        }


                        if (dtClaimsResponse.Columns.Contains("TotalCharges"))
                        {
                            if (row["TotalCharges"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["TotalCharges"])))
                                claimHeaderResponse.TotalCharges = Convert.ToDouble(row["TotalCharges"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("FromDOS"))
                        {
                            if (row["FromDOS"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["FromDOS"])))
                                claimHeaderResponse.FromDOS = Convert.ToDateTime(row["FromDOS"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("ThruDOS"))
                        {
                            if (row["ThruDOS"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["ThruDOS"])))
                                claimHeaderResponse.ThruDOS = Convert.ToDateTime(row["ThruDOS"]);
                        }

                        if (dtClaimsResponse.Columns.Contains("RemittanceAdviceDate"))
                        {
                            if (row["RemittanceAdviceDate"] != DBNull.Value && !string.IsNullOrEmpty(Convert.ToString(row["RemittanceAdviceDate"])))
                                claimHeaderResponse.RemittanceAdviceDate = Convert.ToDateTime(row["RemittanceAdviceDate"]).ToString("MM/dd/yyyy");
                        }

                        claims.Add(claimHeaderResponse);
                    }

                    responseHeader = new Models.ResponseHeader();
                    responseHeader.ResponseMessage = "SUCCESS";
                    responseHeader.ResponseType = "SUCCESS";
                    responseHeader.ResponseCode = "1001";

                    if (payload.AllowDBSearch != null && payload.AllowDBSearch == true)
                    {

                        List<ClaimHeaderResponse> claimsPagedData = PDMSRestServices.Controllers.Facade.ClaimSearch
                            .GetPagedList(claims, Convert.ToInt16(payload.Offset), Convert.ToInt16(payload.PageSize));
                        searchClaimsResponse.ClaimHeaderResponse = claimsPagedData;
                    }
                    else
                    {
                        searchClaimsResponse.ClaimHeaderResponse = claims;
                    }
                    searchClaimsResponse.ResponseHeader = responseHeader;
                    searchClaimsResponse.TotalRecords = claims.Count;
                    var totalPages = (int)Math.Ceiling((double)searchClaimsResponse.TotalRecords / Convert.ToInt16(payload.PageSize));
                    searchClaimsResponse.TotalPages = totalPages;
                }
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "ClaimSearchAPI:");
                log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
                return StatusCode(550, new { message = "Internal servcer error :" + ex.Message });
            }
            return Ok(searchClaimsResponse);
        }

        [HttpPost]
        [Route("EncryptData")]
        public IActionResult EncryptData([FromBody] List<EncryptionPyalod> encryptionPyalods)
        {
            try
            {
                if (encryptionPyalods != null && encryptionPyalods.Count > 0)
                {
                    foreach (var item in encryptionPyalods)
                    {
                        item.KeySecret = HttpUtility.UrlEncode(Helper.Encrypt(item.KeyValue));
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal servcer error :" + ex.Message });
            }
            return Ok(encryptionPyalods);
        }

        [HttpGet]
        [Route("GetSavedClaimID")]
        public IActionResult GetSavedClaimID(string icn = "", string MedicalBillingNumber = "",
            string PatientAccountNumber = "", string MedicaidID = "", string claimType = "")
        {
            string claimId = string.Empty;
            DataSet dsresponse;
            DataTable dtClaimsResponse;

            try
            {
                if (string.IsNullOrEmpty(icn))
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    if (!string.IsNullOrEmpty(MedicalBillingNumber))
                    {
                        parameters.Add(SqlParms.CreateParameter("MedicalBillingNumber", DbType.String, MedicalBillingNumber.Trim(), true));
                    }
                    if (!string.IsNullOrEmpty(PatientAccountNumber))
                    {
                        parameters.Add(SqlParms.CreateParameter("PatientAccountNumber", DbType.String, PatientAccountNumber.Trim(), true));
                    }
                    if (!string.IsNullOrEmpty(MedicaidID))
                    {
                        parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, MedicaidID.Trim(), true));
                    }

                    dsresponse = DataAccess.ExecuteStoredProcedure("usp_Search_Claim", parameters, "Search_Claim");
                    if (Helper.HasRows(dsresponse))
                    {
                        dtClaimsResponse = dsresponse != null ? dsresponse.Tables[0] : null;
                        claimId = dtClaimsResponse.Rows[0]["Claim_ID"].ToString();
                    }
                }
                DataSet ClaimsData = new DataSet();
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("Medicaid_Id", MedicaidID);
                param.Add("Claim_ID", claimId);

                if (claimType.ToUpper().Trim() == "DENTAL" || claimType.ToUpper().Trim() == "D" || claimType == "0")
                {
                    ClaimsData = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("claims_search_details", param);
                }
                else if (claimType.ToUpper() == "INSTITUTIONAL" || claimType.ToUpper().Trim() == "I" || claimType == "1")
                {
                    ClaimsData = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("claims_search_details_inst", param);
                }
                else if (claimType.ToUpper() == "PROFESSIONAL" || claimType.ToUpper().Trim() == "P" || claimType == "2")
                {
                    ClaimsData = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("claims_search_details_prof", param);
                }

                foreach (DataTable dt in ClaimsData.Tables)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Ok(claimId);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal servcer error :" + ex.Message });
            }
            return Ok(claimId);
        }


        [HttpGet]
        [Route("GetSearchClaimsDropdowns")]
        public IActionResult GetSearchClaimsDropdowns()
        {
            PDMSRestServices.Models.SearchClaimDropdowns SearchClaimDropdowns = new PDMSRestServices.Models.SearchClaimDropdowns();
            List<PDMSRestServices.Models.ClaimStaus> claimStatuses = new List<PDMSRestServices.Models.ClaimStaus>();
            List<PDMSRestServices.Models.DestinationPayer> payerList = new List<PDMSRestServices.Models.DestinationPayer>();
            List<PDMSRestServices.Models.PageSize> pageSizes = new List<PDMSRestServices.Models.PageSize>();
            List<PDMSRestServices.Models.ClaimType> claimTypes = new List<PDMSRestServices.Models.ClaimType>();

            try
            {
                DataSet dataSet = LookupTableController.GetClaimStatus();
                DataTable dt = dataSet.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.ClaimStaus status = new PDMSRestServices.Models.ClaimStaus();

                        status.StatusId = Convert.ToString(dr["PRIOR_AUTH_CLAIM_STATUS_ID"]);
                        status.StatusType = Convert.ToString(dr["PRIOR_AUTH_CLAIM_STATUS_TYPE"]);

                        claimStatuses.Add(status);
                    }
                }

                dataSet = LookupTableController.LoadDestinationPayer();
                dt = dataSet.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.DestinationPayer payer = new PDMSRestServices.Models.DestinationPayer();

                        payer.PayerId = Convert.ToString(dr["DESTINATION_PAYER_ID"]);
                        payer.PayerDesc = Convert.ToString(dr["DESTINATION_PAYER_DESC"]);

                        payerList.Add(payer);
                    }
                }

                SearchClaimDropdowns.Payers = payerList;
                SearchClaimDropdowns.ClaimStaus = claimStatuses;
                SearchClaimDropdowns.ClaimTypes = new List<Models.ClaimType>()
                {
                    new Models.ClaimType(){Text="Dental",Value="0"},
                    new Models.ClaimType(){Text="Institutional",Value="1"},
                    new Models.ClaimType(){Text="Professional",Value="2"}
                };
                SearchClaimDropdowns.PageSizes = new List<Models.PageSize>()
                {
                    new Models.PageSize(){Value="5",Size="5"},
                   new Models.PageSize(){Value="10",Size="10"},
                    new Models.PageSize(){Value="20",Size="20"},
                     new Models.PageSize(){Value="30",Size="30"},
                      new Models.PageSize(){Value="40",Size="40"},
                       new Models.PageSize(){Value="50",Size="50"}
                };
            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal servcer error :" + ex.Message });
            }
            return Ok(SearchClaimDropdowns);
        }

        [HttpGet]
        [Route("GetPayerType")]
        public IActionResult GetPayerType()
        {
            List<PDMSRestServices.Models.DestinationPayer> payerList = new List<PDMSRestServices.Models.DestinationPayer>();
            try
            {
                DataSet dataSet = LookupTableController.LoadDestinationPayer();
                DataTable dt = dataSet.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.DestinationPayer payer = new PDMSRestServices.Models.DestinationPayer();

                        payer.PayerId = Convert.ToString(dr["DESTINATION_PAYER_ID"]);
                        payer.PayerDesc = Convert.ToString(dr["DESTINATION_PAYER_DESC"]);

                        payerList.Add(payer);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal servcer error :" + ex.Message });
            }
            return Ok(payerList);
        }


        [HttpGet]
        [Route("GetItemsPaging")]
        public IActionResult GetItemsPaging(int page = 1, int pageSize = 3)
        {

            List<Product> items = new List<Product>() {
                new Product() { Id = 1, Name = "item1",Price=10.99M },
                new Product() { Id = 1, Name = "item2",Price=10.99M },
                new Product() { Id = 1, Name = "item3",Price=10.99M },
                new Product() { Id = 1, Name = "item4",Price=10.99M },
                new Product() { Id = 1, Name = "item5",Price=10.99M },
                new Product() { Id = 1, Name = "item6",Price=10.99M },
                new Product() { Id = 1, Name = "item7",Price=10.99M },
                new Product() { Id = 1, Name = "item8",Price=10.99M },
                new Product() { Id = 1, Name = "item9",Price=10.99M }

            };

            var totalItems = items.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedData = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Items = pagedData
            });
        }
    }
}
