using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Services3.Referral;
using MigraDoc.DocumentObjectModel.Tables;
using Newtonsoft.Json;
using PdfSharp.Snippets.Drawing;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class EnrollmentController : ControllerBase
    {
        //[HttpPost]
        //[Route("GetAddressVerificaton")]
        //public IActionResult GetAddressVerificaton(AddressVerificationRequest address, int addressTypeId)
        //{
        //    AddressVerificatonDetail avd = new AddressVerificatonDetail();
        //    EnrollmentFacade ef = new EnrollmentFacade();
        //    avd = ef.GetAddressVerificaton(address, addressTypeId);            
        //    return Ok(avd);
        //}

        //[HttpPost]
        //[Route("GetAddressVerificatonWS")]
        //public IActionResult GetAddressVerificatonWS(AddressVerificationRequest address)
        //{
        //    AddressVerificatonDetail avd = new AddressVerificatonDetail();
        //    EnrollmentFacade ef = new EnrollmentFacade();
        //    avd = ef.GetAddressVerificatonWS(address);
        //    return Ok(avd);
        //}

        [HttpPost]
        [Route("GetAddressVerificatonWSTiger")]
        public IActionResult GetAddressVerificatonWSTiger(AddressVerificationRequest address)
        {
            AddressVerificatonDetail avd = new AddressVerificatonDetail();
            EnrollmentFacade ef = new EnrollmentFacade();
            avd = ef.GetAddressVerificatonWSTiger(address);
            return Ok(avd);
        }


        [HttpGet]
        [Route("GetWorkHistoryData")]
        public IActionResult GetWorkHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "ProviderWorkHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetMedicareHistoryData")]
        public IActionResult GetMedicareHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "MedicareHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetMedicaidHistoryData")]
        public IActionResult GetMedicaidHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "MedicaidHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCredentalingDelegatesHistoryData")]
        public IActionResult GetCredentalingDelegatesHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "CredentialedDelegateHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetGroupToIndividualAffiliationHistoryData")]
        public IActionResult GetGroupToIndividualAffiliationHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "GroupToIndivudalAffiliationHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetOrgInfoHistoryData")]
        public IActionResult GetOrgInfoHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "OrgInfoHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetProviderFeedNotesDetails")]
        public IActionResult GetProviderFeedNotesDetails(string regId)
        {
            try
            {
                if (!int.TryParse(regId, out int registrationId))
                    return BadRequest("Invalid registration ID.");

                var ef = new EnrollmentFacade();
                var ds = ef.GetRegistrationData(registrationId, "GetProviderFeedNotesData");
                var regApplicationStatus = new List<RegApplicationStatus>(); /*ef.LoadRegistrationApplicationStatus(Convert.ToInt32(regId));*/
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(new List<ProviderFeedVM>());

                var providerFeedVMList = ds.Tables[0].AsEnumerable()
                    .GroupBy(dr => Convert.ToInt32(dr["Reg_Provider_Feed_ID"]))
                    .Select(g => new ProviderFeedVM
                    {
                        ProviderFeedID = g.Key,
                        InitiatedBy = g.First()["Initiated_By"]?.ToString(),
                        PersonReviewedBy = g.First()["Person_Reviewed_By"]?.ToString(),
                        EnrollmentType = g.First()["Enrollment_Type"]?.ToString(),
                        FinalDispossion = g.First()["Final_Disposition"]?.ToString(),
                        NotesDate = g.First()["NotesDate"]?.ToString(),
                        ProcessID = g.First().Field<int>("PROCESS_ID"),
                        Notes = g.Select(dr => dr["NOTE"]?.ToString()).Where(note => !string.IsNullOrEmpty(note)).ToList()
                    }).ToList();

                foreach (var providerFeedVM in providerFeedVMList)
                {
                    try
                    {

                        if (providerFeedVM.EnrollmentType != Constants.EnrollmentType.Job && providerFeedVM.EnrollmentType != Constants.EnrollmentType.AdHocComment && providerFeedVM.ProcessID > 0)
                        {
                            string finalDisposition = regApplicationStatus.Where(x => x.RegId == Convert.ToInt32(regId) && x.ProcessId == providerFeedVM.ProcessID).Select(x => x.ApplicationStatus).FirstOrDefault();
                            providerFeedVM.FinalDispossion = !string.IsNullOrEmpty(finalDisposition) ? finalDisposition : providerFeedVM.FinalDispossion;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Error log 
                    }
                }

                return Ok(providerFeedVMList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "GetProviderFeedNotesDetails failed", Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetProviderFeedAddNotesDetails")]
        public IActionResult GetProviderFeedAddNotesDetails(string regId)
        {
            if (!int.TryParse(regId, out int registrationId))
                return BadRequest("Invalid registration ID.");

            var enrollmentFacade = new EnrollmentFacade();
            var dataSet = enrollmentFacade.GetRegistrationData(registrationId, "getproviderfeedaddnotedata");

            var addNoteDetails = new ProviderFeedAddNotes
            {
                Note_Date = DateTime.Now.ToString("MM/dd/yyyy"),
                Initiator_OHID = "Test Initiator"
            };

            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                addNoteDetails.Lasted_Reviewed_By = row.Field<string>(0);
                addNoteDetails.Enrollment_Type = row.Field<string>(1);
                addNoteDetails.Final_Disposition = row.Field<string>(2);
            }

            return Ok(addNoteDetails);
        }

        [HttpPost]
        [Route("InsertProviderFeedNotes")]
        public IActionResult InsertProviderFeedNotes([FromBody] ProviderFeedVM providerFeed)
        {
            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            try
            {
                var parameters = new List<SqlParameter>
                {
                    SqlParms.CreateParameter("REG_ID", DbType.Int32, providerFeed.RegID, true),
                    SqlParms.CreateParameter("REG_PROVIDER_FEED_ID", DbType.Int32, providerFeed.ProviderFeedID, true),
                    SqlParms.CreateParameter("INITIATED_BY", DbType.String, providerFeed.InitiatedBy, true),
                    SqlParms.CreateParameter("PERSON_REVIEWED_BY", DbType.String, providerFeed.PersonReviewedBy, true),
                    SqlParms.CreateParameter("ENROLLMENT_TYPE", DbType.String, providerFeed.EnrollmentType, true),
                    SqlParms.CreateParameter("FINAL_DISPOSITION", DbType.String, providerFeed.FinalDispossion, true),
                    SqlParms.CreateParameter("NOTES", DbType.String, providerFeed.Notes?.FirstOrDefault() ?? string.Empty, true)
                };

                DataAccess.ExecuteStoredProcedure("usp_InsertRegProviderFeedNotes", parameters);
                httpResponse.ResponseCode = "201";
            }
            catch (Exception ex)
            {
                httpResponse.ResponseCode = "500";
                httpResponse.ResponseDesc = "Internal Server error: " + ex.Message + " - " + ex.StackTrace;
            }
            return Ok(httpResponse);
        }

        [HttpGet]
        [Route("GetGroupAffiliationHistoryData")]
        public IActionResult GetGroupAffiliationHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "GroupAffiliationHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("GetOtherServcLocationsHistoryData")]
        public IActionResult GetOtherServcLocationsHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "OtherServiceLocations");

            if (ds.Tables[0].Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(ds.Tables[0]));
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetProviderFeedHistoryData")]
        public IActionResult GetProviderFeedHistoryData(string regId)
        {
            try
            {
                var ef = new EnrollmentFacade();
                DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "PROVIDER_NOTE_HIST");

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Ok(new List<ProviderFeedHistVM>());

                var providerFeedVMList = ds.Tables[0].AsEnumerable()
                     .GroupBy(dr => new
                     {
                         ProviderNoteID = dr.Field<int>("Provider_Note_ID"),
                         NOTE_DATE_TIME = dr.Field<DateTime>("NOTE_DATE_TIME").Date.ToString("yyyy-MM-dd")
                     })

                    .Select(g => new ProviderFeedHistVM
                    {
                        ProviderFeedID = g.Key.ProviderNoteID,
                        NOTE_DATE_TIME = g.Key.NOTE_DATE_TIME,

                        Type = g.First()["Type"]?.ToString(),
                        UserName = g.First()["UserName"]?.ToString(),
                        TASK_NAME = g.First()["TASK_NAME"]?.ToString(),
                        REG_PAGE_NAME = g.First()["REG_PAGE_NAME"]?.ToString(),
                        NOTE_TEXT = g.Select(dr => dr["NOTE_TEXT"]?.ToString()).Where(note => !string.IsNullOrEmpty(note)).ToList()
                    }).ToList();

                return Ok(providerFeedVMList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex.Message + " - " + ex.StackTrace);
            }

        }

        [HttpGet]
        [Route("GetHealthCareAffiliationHistoryData")]
        public IActionResult GetHealthCareAffiliationHistoryData(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "HealthCareAffiliationHistory");
            string data = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("FetchORPProviderSearchResults")]
        public IActionResult FetchORPProviderSearchResults([FromBody] PDMSRestServices.Models.ORPSearchRequest payload)
        {
            ORPSearchResponse osr = new ORPSearchResponse();
            try
            {
                string code = string.Empty;
                EnrollmentFacade ef = new EnrollmentFacade();
                DataTable dt = ef.GetORPProviderSearchResults(payload.ProviderNPI, payload.DOS);
                string data = string.Empty;
                List<ORPSearchDisplay> osdList = new List<ORPSearchDisplay>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ORPSearchDisplay osd = new ORPSearchDisplay();
                        if (!Helper.GetString("PROVIDER_NPI", row).Equals(""))
                        {
                            osd.ProviderNPI = Helper.GetString("PROVIDER_NPI", row);
                            osd.ProviderName = Helper.GetString("PROVIDER_NAME", row);
                            osdList.Add(osd);
                            osr.ORPSearchDisplay = osdList;
                        }
                        else
                        {
                            osr.ORPSearchDisplay = null;
                        }
                        code = Helper.GetString("RESP_CODE", row);
                        if (code.Equals("300"))
                        {
                            osr.ResponseCode = code;
                            osr.ResponseDescp = "NPI not registered in PNM. Please contact provider to verify NPI.";
                        }
                        else if (code.Equals("301"))
                        {
                            osr.ResponseCode = code;
                            osr.ResponseDescp = "NPI not active on Date of Service entered. Please contact provider to verify NPI.";
                        }
                        else if (code.Equals("302"))
                        {
                            osr.ResponseCode = code;
                            osr.ResponseDescp = "Only Type 1 NPIs can order, refer, or prescribe.";
                        }
                        else
                        {
                            osr.ResponseCode = "200";
                            osr.ResponseDescp = "SUCCESS";
                        }
                    }
                }
                return Ok(osr);
            }
            catch (Exception ex)
            {
                osr.ResponseCode = "500";
                osr.ResponseDescp = ex.Message + " " + ex.StackTrace;
                return Ok(osr);
            }
        }

        [HttpPost]
        [Route("GetProviderSpecialtySearchResults")]
        public IActionResult GetProviderSpecialtySearchResults([FromBody] PDMSRestServices.Models.SpecialtySearchRequest payload)
        {
            SpecialtySearchResponse sr = new SpecialtySearchResponse();
            List<SpecialtySearchResult> resList = new List<SpecialtySearchResult>();
            var data = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, payload.ProviderNPI, true));
                parameters.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, payload.ProviderMedID, true));

                var ds = DataAccess.ExecuteStoredProcedure("usp_GetProviderSpecialtySearchResults", parameters, "SpecialtySearch");
                if (Helper.HasRows(ds))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        SpecialtySearchResult res = new SpecialtySearchResult();
                        res.MedicaidID = Helper.GetString("MEDICAID_ID", row);
                        res.ProviderType = Helper.GetString("MMIS_PROVIDER_TYPE_ID", row);
                        res.SpecialtyType = Helper.GetString("SPECIALTY_TYPE_NAME", row);
                        res.PrimaryFlag = Helper.GetString("PRIMARY_FLAG", row);
                        res.StartDate = Helper.GetDateTime("START_DATE", row);
                        res.EndDate = Helper.GetDateTime("END_DATE", row);
                        res.EnrollStatusDesc = Helper.GetString("ENROLL_STATUS_DESC", row);
                        resList.Add(res);
                        sr.SpecialtySearchResult = resList;

                        if (Helper.GetString("RESPONSE_CODE", row) == "300")
                        {
                            sr.ResponseCode = "300";
                            sr.ResponseDescp = "NPI not registered in PNM. Please contact provider to verify NPI.";
                        }
                        else if (Helper.GetString("RESPONSE_CODE", row) == "301")
                        {
                            sr.ResponseCode = "301";
                            sr.ResponseDescp = "Medicaid ID not registered in PNM. Please contact provider to verify.";
                        }
                        else if (Helper.GetString("RESPONSE_CODE", row) == "302")
                        {
                            sr.ResponseCode = "302";
                            sr.ResponseDescp = "The NPI and Medicaid ID combination is not registered in PNM. Please contact provider to verify.";
                        }
                        else if (Helper.GetString("RESPONSE_CODE", row) == "200")
                        {
                            sr.ResponseCode = "200";
                            sr.ResponseDescp = "SUCCESS";
                        }
                    }

                }
                else
                {
                    sr.ResponseCode = "300";
                    sr.ResponseDescp = "No Data found";
                }
                return Ok(sr);
            }
            catch (Exception ex)
            {
                sr.ResponseCode = "500";
                sr.ResponseDescp = ex.Message + " " + ex.StackTrace;
                return Ok(sr);
            }
        }

        [HttpGet]
        [Route("GetAtypicalAppDropdowns")]
        public IActionResult GetAtypicalAppDropdowns()
        {
            PDMSRestServices.Models.StreamlinedAppDropDowns streamlinedAppDropDowns = new PDMSRestServices.Models.StreamlinedAppDropDowns();
            List<PDMSRestServices.Models.ProviderType> ptType = new List<PDMSRestServices.Models.ProviderType>();
            List<PDMSRestServices.Models.Gender> genders = new List<PDMSRestServices.Models.Gender>();
            List<PDMSRestServices.Models.States> states = new List<PDMSRestServices.Models.States>();

            try
            {
                DataSet dsProvType = HelperFacade.SelectRegistrationProviderTypesByCategory(CON.ApplicationType.Standard, CON.ProviderCategoryTypeID.Individual, 0);

                if (Helper.HasRows(dsProvType))
                {
                    string allowedPTTypesforStreamlined = AppSettings.Get("AllowedPTTypesForStreamLinedApp", "'07', '20', '23', '24', '27', '30'");
                    var allowedTypes = allowedPTTypesforStreamlined
                                        .Replace("'", "") // Remove single quotes
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .ToList();

                    DataTable dtfilteredPT = dsProvType.Tables[0].AsEnumerable()
                                            .Where(row => allowedTypes.Contains(row.Field<string>("MMIS_PROVIDER_TYPE_ID")))
                                            .CopyToDataTable();

                    if (dtfilteredPT != null && dtfilteredPT.Rows != null && dtfilteredPT.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtfilteredPT.Rows)
                        {
                            PDMSRestServices.Models.ProviderType ptTypeList = new PDMSRestServices.Models.ProviderType();

                            ptTypeList.PROVIDER_TYPE_NAME = Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]).Trim() + " - " + Convert.ToString(dr["PROVIDER_TYPE_NAME"]).Trim();
                            ptTypeList.MMIS_PROVIDER_TYPE_ID = Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]).Trim();

                            ptType.Add(ptTypeList);
                        }
                    }
                }

                DataSet dsGender = HelperFacade.SelectGender();
                if (Helper.HasRows(dsGender))
                {
                    DataTable dtfilteredgender = dsGender.Tables[0].AsEnumerable()
                     .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "B")
                     .CopyToDataTable();

                    if (dtfilteredgender != null && dtfilteredgender.Rows != null && dtfilteredgender.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtfilteredgender.Rows)
                        {
                            PDMSRestServices.Models.Gender gender = new PDMSRestServices.Models.Gender();
                            gender.PROVIDER_GENDER_NAME = Convert.ToString(dr["PROVIDER_GENDER_NAME"]);
                            gender.PROVIDER_GENDER_INITIAL = Convert.ToString(dr["PROVIDER_GENDER_INITIAL"]);

                            genders.Add(gender);
                        }
                    }

                }

                DataSet dsStates = HelperFacade.SelectStates();
                if (Helper.HasRows(dsStates))
                {
                    foreach (DataRow dr in dsStates.Tables[0].Rows)
                    {
                        PDMSRestServices.Models.States state = new PDMSRestServices.Models.States();
                        state.STATE_ABBREV = Convert.ToString(dr["StateId"]);
                        state.STATE_ID = Convert.ToString(dr["StateId"]);

                        states.Add(state);
                    }

                }

                streamlinedAppDropDowns.dtProviderTypes = ptType;
                streamlinedAppDropDowns.dtGender = genders;
                streamlinedAppDropDowns.dtState = states;

            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal server error :" + ex.Message });
            }
            return Ok(streamlinedAppDropDowns);
        }


        [HttpGet]
        [Route("GetStreamLinedAppDropdowns")]
        public IActionResult GetStreamLinedAppDropdowns()
        {
            PDMSRestServices.Models.StreamlinedAppDropDowns streamlinedAppDropDowns = new PDMSRestServices.Models.StreamlinedAppDropDowns();
            List<PDMSRestServices.Models.ProviderType> ptType = new List<PDMSRestServices.Models.ProviderType>();
            List<PDMSRestServices.Models.Gender> genders = new List<PDMSRestServices.Models.Gender>();
            List<PDMSRestServices.Models.States> states = new List<PDMSRestServices.Models.States>();

            try
            {
                DataSet dsProvType = HelperFacade.SelectRegistrationProviderTypesByCategory(CON.ApplicationType.Standard, CON.ProviderCategoryTypeID.Individual, 0);

                if (Helper.HasRows(dsProvType))
                {
                    string allowedPTTypesforStreamlined = AppSettings.Get("AllowedPTTypesForStreamLinedApp", "'07', '20', '23', '24', '27', '30'");
                    var allowedTypes = allowedPTTypesforStreamlined
                                        .Replace("'", "") // Remove single quotes
                                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s => s.Trim())
                                        .ToList();

                    DataTable dtfilteredPT = dsProvType.Tables[0].AsEnumerable()
                                            .Where(row => allowedTypes.Contains(row.Field<string>("MMIS_PROVIDER_TYPE_ID")))
                                            .CopyToDataTable();

                    if (dtfilteredPT != null && dtfilteredPT.Rows != null && dtfilteredPT.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtfilteredPT.Rows)
                        {
                            PDMSRestServices.Models.ProviderType ptTypeList = new PDMSRestServices.Models.ProviderType();

                            ptTypeList.PROVIDER_TYPE_NAME = Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]).Trim() + " - " + Convert.ToString(dr["PROVIDER_TYPE_NAME"]).Trim();
                            ptTypeList.MMIS_PROVIDER_TYPE_ID = Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]).Trim();

                            ptType.Add(ptTypeList);
                        }
                    }
                }

                DataSet dsGender = HelperFacade.SelectGender();
                if (Helper.HasRows(dsGender))
                {
                    DataTable dtfilteredgender = dsGender.Tables[0].AsEnumerable()
                     .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "B")
                     .CopyToDataTable();

                    if (dtfilteredgender != null && dtfilteredgender.Rows != null && dtfilteredgender.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtfilteredgender.Rows)
                        {
                            PDMSRestServices.Models.Gender gender = new PDMSRestServices.Models.Gender();
                            gender.PROVIDER_GENDER_NAME = Convert.ToString(dr["PROVIDER_GENDER_NAME"]);
                            gender.PROVIDER_GENDER_INITIAL = Convert.ToString(dr["PROVIDER_GENDER_INITIAL"]);

                            genders.Add(gender);
                        }
                    }

                }

                DataSet dsStates = HelperFacade.SelectStates();
                if (Helper.HasRows(dsStates))
                {
                    foreach (DataRow dr in dsStates.Tables[0].Rows)
                    {
                        PDMSRestServices.Models.States state = new PDMSRestServices.Models.States();
                        state.STATE_ABBREV = Convert.ToString(dr["StateId"]);
                        state.STATE_ID = Convert.ToString(dr["StateId"]);

                        states.Add(state);
                    }

                }

                streamlinedAppDropDowns.dtProviderTypes = ptType;
                streamlinedAppDropDowns.dtGender = genders;
                streamlinedAppDropDowns.dtState = states;

            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal server error :" + ex.Message });
            }
            return Ok(streamlinedAppDropDowns);
        }

        [HttpPost]
        [Route("ValidateNPIfromNPPES")]
        public IActionResult ValidateNPIfromNPPES([FromBody] PDMSRestServices.Models.NPPESAPIRequest npiRequest)
        {
            NPPESAPIResponse nppesAPIResponse = new NPPESAPIResponse();
            NPPESAPIResult apiresult = new NPPESAPIResult();
            DataTable dtTaxonomies = new DataTable();
            List<PDMSRestServices.Models.TaxonomyTypes> taxTypes = new List<PDMSRestServices.Models.TaxonomyTypes>();
            List<PDMSRestServices.Models.SpecialtyTypes> specTypes = new List<PDMSRestServices.Models.SpecialtyTypes>();
            string resultcode = string.Empty;
            string resultmessage = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("NPI", DbType.Int32, npiRequest.NPI, true));
                //parameters.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, this.Model.ProviderTypeID, true));
                parameters.Add(SqlParms.CreateParameter("REGID", DbType.Int32, 0, true));
                parameters.Add(SqlParms.CreateParameter("MMIS_PROVIDER_TYPE_ID", DbType.String, npiRequest.ProviderType, true));
                parameters.Add(SqlParms.CreateParameter("isKeyFieldEditRequest", DbType.Boolean, false, true));
                DataSet dsNPIAlreadyPresent = DataAccess.ExecuteStoredProcedure("usp_SelectNPIWithProviderType", parameters, "NPIProviderTypeIDCheck");

                if (Helper.HasRows(dsNPIAlreadyPresent))
                {
                    foreach (DataRow dr in dsNPIAlreadyPresent.Tables[0].Rows)
                    {
                        if (Convert.ToString(dr["MMIS_PROVIDER_TYPE_ID"]) == npiRequest.ProviderType)
                        {
                            resultcode = "300";
                            resultmessage = resultmessage + "This NPI and Provider type already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2" + "<br />";

                        }
                        else
                        {
                            List<SqlParameter> parameters1 = new List<SqlParameter>();

                            parameters1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, dr["REG_ID"], true));
                            parameters1.Add(SqlParms.CreateParameter("UserID", DbType.String, npiRequest.UserID, true));
                            DataSet dsNPIRegStatus = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationStatuses", parameters1, "NPIRegStatus");
                            if (Helper.HasRows(dsNPIRegStatus))
                            {
                                if ((dsNPIRegStatus.Tables[0].Rows[0]["TennCare Status"].ToString() == CON.PDMSCareStatus
                                    || dsNPIRegStatus.Tables[0].Rows[0]["TennCare Status"].ToString() == CON.PDMSApplicationStatusNotProcessed)
                                    && (dsNPIRegStatus.Tables[0].Rows[0][1].ToString() == CON.PDMSApplicationStatusComplete
                                    || dsNPIRegStatus.Tables[0].Rows[0][1].ToString() == CON.PDMSApplicationStatusApproved
                                    || dsNPIRegStatus.Tables[0].Rows[0][1].ToString() == CON.PDMSApplicationStatusNotProcessed)
                                    && dsNPIRegStatus.Tables[0].Rows[0]["Effective Date"] == DBNull.Value)
                                {
                                    //do nothing here as the NPI can be re-used
                                }
                                else
                                {
                                    resultcode = "300";
                                    resultmessage = resultmessage + "This NPI already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2" + "<br />";

                                }
                            }
                            // OHPNM-15626 we should never have an instance where if statement returns no rows, but just in case, we already know we have an NPI that should not be allowed
                            else
                            {
                                resultcode = "300";
                                resultmessage = resultmessage + "This NPI already exists in the system. If you have questions, please contact the Integrated Help Desk at 1-800-686-1516, Option 2, Option 2" + "<br />";
                            }
                        }
                    }
                }
                else
                {
                    DataSet lookup = RegistrationController.VerifyTaxIdExistsAsEINForSSN(npiRequest.SSN);

                    if (Helper.HasRows(lookup))
                    {
                        resultcode = "300";
                        resultmessage = resultmessage + "Tax ID entered already exists as an EIN. Verify this is a valid SSN." + "<br />";
                    }
                    else
                    {
                        bool isNPIAPIEnabled = AppSettings.Get("NPI-Registry-Enabled").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false;

                        if (!isNPIAPIEnabled)
                        {
                            if (!EnrollmentFacade.ValidateForNPITypeFromDB(npiRequest.NPI))
                            {
                                resultcode = "300";
                                resultmessage = resultmessage + "This NPI entered must be a Type 1 NPI." + "<br />";
                            }
                        }
                        else
                        {
                            if (!EnrollmentFacade.ValidateNPIUniqueness(0, npiRequest.NPI))
                            {
                                resultcode = "300";
                                resultmessage = resultmessage + "The NPI is already active with another registration or is currently being processed.If you have questions, please contact the Integrated Help Desk at 1 - 800 - 686 - 1516, Option 2, Option 2'" + "<br />";
                            }
                            else
                            {
                                apiresult = ProviderController.ValidNPIinNPPESApi(Convert.ToInt64(npiRequest.NPI));
                                if (apiresult.result_count > 0)
                                {
                                    int nppesTypeID = Convert.ToInt32(apiresult.results[0].enumeration_type.Split(new string[] { "NPI-" }, StringSplitOptions.None).Last());
                                    if (nppesTypeID != 1)
                                    {
                                        resultcode = "300";
                                        resultmessage = resultmessage + "This NPI entered must be a Type 1 NPI." + "<br />";
                                    }
                                    else
                                    {
                                        bool isValidGender = false;
                                        bool isValidName = false;
                                        foreach (Result rs in apiresult.results) //currently one record found, need to change match logic if more found
                                        {
                                            Basic basic = rs.basic;
                                            if (basic.first_name.ToLower().Trim() == npiRequest.FirstName.ToLower().Trim() && basic.last_name.ToLower().Trim() == npiRequest.LastName.ToLower().Trim())
                                            {
                                                isValidName = true;
                                            }

                                            if (basic.gender == npiRequest.Gender || basic.sex == npiRequest.Gender)
                                                isValidGender = true;
                                        }
                                        if (!isValidName)
                                        {
                                            resultcode = "300";
                                            resultmessage = resultmessage + "There is a name mis-match with NPPES." + "<br />";
                                        }
                                        if (!isValidGender)
                                        {
                                            resultcode = "300";
                                            resultmessage = resultmessage + "There is a gender mis-match with NPPES." + "<br />";
                                        }
                                        if (isValidName && isValidGender)
                                        {
                                            resultcode = "200";
                                            resultmessage = "Success";
                                            dtTaxonomies = EnrollmentFacade.LoadTaxonomyFromNPPES(apiresult.results[0]);
                                            if (dtTaxonomies.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtTaxonomies.Rows)
                                                {
                                                    PDMSRestServices.Models.TaxonomyTypes taxtype = new PDMSRestServices.Models.TaxonomyTypes();
                                                    taxtype.TAXONOMY_CODE = Convert.ToString(dr["TaxonomyCode"]);
                                                    taxtype.TAXONOMY_DESCRIPTION = Convert.ToString(dr["TaxonomyNameWithCode"]);

                                                    taxTypes.Add(taxtype);
                                                }

                                            }


                                            DataSet dsProvType = ProviderController.SelectProviderTypesByMMISProviderTypeID(npiRequest.ProviderType);
                                            if (Helper.HasRows(dsProvType))
                                            {
                                                EnumerableRowCollection<DataRow> query1 = from ptType in dsProvType.Tables[0].AsEnumerable()
                                                                                          let appType = ptType.Field<int?>("APPLICATION_TYPE_ID")
                                                                                          where appType.HasValue && appType.Value == 1
                                                                                          select ptType;

                                                if (query1.Any())
                                                {
                                                    DataTable dt = query1.CopyToDataTable<DataRow>();
                                                    int providerTypeID = Convert.ToInt32(dt.Rows[0]["PROVIDER_TYPE_ID"]);

                                                    // Based on Provider Type Role fetch specialties
                                                    DataSet dsSpec = LookupTableController.SelectGroupSpecialtiesByProviderTypeRole(providerTypeID, false, 0);
                                                    DataTable validSpecialites = new DataTable();
                                                    EnumerableRowCollection<DataRow> query = from specialty in dsSpec.Tables[0].AsEnumerable()
                                                                                             where (specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "P" ||
                                                                                                    specialty.Field<string>("PT_PS_ALLOWED_PORTAL") == "B")
                                                                                                   && specialty.Field<string>("PT_PS_ALLOWED_STREAMLINE_APP") == "Y"
                                                                                             select specialty;
                                                    if (query.Any())
                                                    {
                                                        validSpecialites = query.CopyToDataTable<DataRow>();
                                                    }
                                                    if (Helper.HasRows(validSpecialites))
                                                    {
                                                        foreach (DataRow dr in validSpecialites.Rows)
                                                        {
                                                            PDMSRestServices.Models.SpecialtyTypes specialty = new PDMSRestServices.Models.SpecialtyTypes();
                                                            specialty.SPECIALTY_TYPE_NAME = Convert.ToString(dr["MMIS_SPECIALTY_TYPE_ID"]).Trim() + " - " + Convert.ToString(dr["SPECIALTY_TYPE_NAME"]).Trim();
                                                            specialty.SPECIALTY_TYPE_ID = Convert.ToInt32(dr["SPECIALTY_TYPE_ID"]);
                                                            specTypes.Add(specialty);
                                                        }
                                                    }
                                                }
                                            }


                                            nppesAPIResponse.TaxnomyTypes = taxTypes;
                                            nppesAPIResponse.SpecialtyTypes = specTypes;
                                        }
                                    }
                                }
                                else
                                {
                                    resultcode = "300";
                                    resultmessage = resultmessage + "The NPI entered is not in the NPPES list." + "<br />";
                                }
                            }
                        }
                    }

                }
                nppesAPIResponse.ResponseCode = resultcode;
                nppesAPIResponse.ResponseDesc = resultmessage;
                nppesAPIResponse.TaxnomyTypes = taxTypes;

                return Ok(nppesAPIResponse);
            }
            catch (Exception ex)
            {
                nppesAPIResponse.ResponseCode = "500";
                nppesAPIResponse.ResponseDesc = ex.Message + " " + ex.StackTrace;
                return Ok(nppesAPIResponse);
            }

        }

        [HttpPost]
        [Route("GetCountyByState")]
        public IActionResult GetCountyByState(string stateID)
        {
            List<County> counties = new List<County>();
            try
            {
                DataSet dsCounty = LookupTableController.SelectCountiesByStateAbbreviation(stateID);
                if (Helper.HasRows(dsCounty))
                {
                    foreach (DataRow dr in dsCounty.Tables[0].Rows)
                    {
                        PDMSRestServices.Models.County county = new PDMSRestServices.Models.County();
                        county.County_Value = Convert.ToString(dr["MMIS_COUNTY_CODEWithName"]);
                        county.County_Text = Convert.ToString(dr["COUNTY_NAME"]);

                        counties.Add(county);
                    }
                }
                return Ok(counties);
            }
            catch (Exception ex)
            {
                return Ok(counties);
            }
        }

        [HttpPost]
        [Route("CreateNewRegistrationStreamlined")]
        public IActionResult CreateNewRegistrationStreamlined([FromBody] PDMSRestServices.Models.ProviderRegistrationData provData)
        {
            PDMSRestServices.Models.ResponseValue resp = new PDMSRestServices.Models.ResponseValue();
            int RegistrationID = 0;
            try
            {
                RegistrationID = EnrollmentFacade.InsertNewStreamlinedProviderRegistration(provData);
                resp.ResponseCode = "200";
                resp.ResponseDesc = RegistrationID.ToString();
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.ResponseCode = "500";
                resp.ResponseDesc = ex.Message + " " + ex.StackTrace;
                return Ok(resp);
            }
        }

        [HttpGet]
        [Route("GetDynamicControlsData")]
        public IActionResult GetAllDynamicControlsData()
        {
            try
            {
                var enrollmentFacade = new EnrollmentFacade();
                var dataSet = enrollmentFacade.GetAllDynamicControlsData();

                var table = dataSet.Tables[0];

                var dynamicControlsList = table.AsEnumerable()
                    .Select(row => new DynamicControls
                    {
                        ID = row.Field<int>("DYNAMIC_FIELD_CONFIGURATION_ID"),
                        RegID = row.Field<string>("REG_ID") ?? string.Empty,
                        ProviderType = row.Field<string>("PROVIDER_TYPE_NAME"),
                        SectionType = row.Field<string>("REG_SECTION_TYPE_NAME"),
                        FieldName = row.Field<string>("FieldName"),
                        DataType = row.Field<string>("data_type"),
                        ControlType = row.Field<string>("control_type_id"),
                        ControlLevel = row.Field<string>("control_level_id"),
                        IsActive = row.Field<bool>("IsActive") ? "Yes" : "No",
                        SelectedValues = row.Field<string>("SelectValues"),
                        ControlId = row.Field<string>("control_id"),
                        ModifiedUser = row.Field<Guid>("LAST_MODIFIED_USER").ToString(),
                        ModifiedDate = row.Field<DateTime>("LAST_MODIFIED_DATE_TIME").ToString(),
                        Record_Status = row.Field<string>("Record_Status"),
                        Approval_Date = row.Field<DateTime?>("Approval_Date"),
                        Review_Date = row.Field<DateTime?>("Review_Date"),
                    }).ToList();

                return Ok(dynamicControlsList);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message} - {ex.StackTrace}";
                return StatusCode(500, errorMessage);
            }
        }



        [HttpPost]
        [Route("UpdateDynamicControlStatus")]
        public IActionResult UpdateDynamicControlStatus([FromBody] UpdateDynamicControls updateControls)
        {
            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            if (updateControls == null)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                var enrollmentFacade = new EnrollmentFacade();
                enrollmentFacade.UpdateDynamicControlsData(updateControls);
                httpResponse.ResponseCode = "200";
                httpResponse.ResponseDesc = "Control status updated successfully.";
                return Ok(httpResponse);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message} - {ex.StackTrace}";
                return StatusCode(500, errorMessage);
            }
        }

        [HttpGet]
        [Route("GetUIControlVisibilityByRegId")]
        public IActionResult GetUIControlVisibilityByRegId(int regId)
        {
            try
            {
                var visibilityFacade = new EnrollmentFacade();
                var dataSet = visibilityFacade.GetUIControlVisibilityByRegId(regId);

                var table = dataSet.Tables[0];

                var visibilityList = table.AsEnumerable()
                    .Select(row => new UIControlVisibility
                    {
                        UIControlId = row.Field<int>("UIControlVisibilityConfig_ID"),
                        PageName = row.Field<string>("PageName"),
                        ControlID = row.Field<string>("ControlID"),
                        Reg_Id = row.Field<int>("reg_id"),
                        IsVisible = row.Field<bool>("IsVisible"),
                        IsActive = row.Field<bool>("IsActive") ? "Yes" : "No",
                        Record_Status = row.Field<string>("Record_Status"),
                        Approval_Date = row.Field<DateTime?>("Approval_Date"),
                        Review_Date = row.Field<DateTime?>("Review_Date"),
                    }).ToList();

                return Ok(visibilityList);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message} - {ex.StackTrace}";
                return StatusCode(500, errorMessage);
            }
        }

        [HttpPost]
        [Route("DeleteUIControlConfig")]
        public IActionResult DeleteUIControlConfig([FromBody] DeleteUI ids)
        {
            if (ids == null || ids.Ids == null || ids.Ids.Count() <= 0)
            {
                return BadRequest("No IDs provided for deletion.");
            }
            var visibilityFacade = new EnrollmentFacade();
            visibilityFacade.DeleteUIConfigById(ids);

            return Ok(new
            {
                message = "Records deleted successfully"
            });
        }

        [HttpPost]
        [Route("DeleteDynamicFieldConfig")]
        public IActionResult DeleteDynamicFieldConfig([FromBody] DeleteDynamicFields ids)
        {
            if (ids == null || ids.Ids == null || ids.Ids.Count() <= 0)
            {
                return BadRequest("No IDs provided for deletion.");
            }
            var visibilityFacade = new EnrollmentFacade();
            visibilityFacade.DeleteDynamicFieldConfigById(ids);

            return Ok(new
            {
                message = "Records deleted successfully"
            });
        }

        [HttpPost]
        [Route("UpdateUIControlVisibilityStatus")]
        public IActionResult UpdateUIControlVisibilityStatus([FromBody] UpdateUIControls updateControls)
        {
            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            if (updateControls == null)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                var enrollmentFacade = new EnrollmentFacade();
                enrollmentFacade.UpdateUIControlVisibilityData(updateControls);

                httpResponse.ResponseCode = "200";
                httpResponse.ResponseDesc = "UI Control visibility status updated successfully.";
                return Ok(httpResponse);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message} - {ex.StackTrace}";
                return StatusCode(500, errorMessage);
            }
        }

        [HttpPost]
        [Route("UpdateDynamicConfigApproval")]
        public IActionResult UpdateDynamicConfigApproval([FromBody] UpdateDynamicControls updateControls)
        {
            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            if (updateControls == null)
            {
                return BadRequest("Invalid input data.");
            }

            try
            {
                var enrollmentFacade = new EnrollmentFacade();
                enrollmentFacade.UpdateDynamicFieldConfiguration(updateControls);

                httpResponse.ResponseCode = "200";
                httpResponse.ResponseDesc = "Dynamic configuration status updated successfully.";
                return Ok(httpResponse);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message} - {ex.StackTrace}";
                return StatusCode(500, errorMessage);
            }
        }


        [HttpPost]
        [Route("AddDisclosure")]
        [Route("UpdateDisclosure")]
        public IActionResult AddDisclosure([FromBody] DisclosureDto dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var enrollmentFacade = new EnrollmentFacade();
            enrollmentFacade.SaveRegDisclosure(dto);
            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            httpResponse.ResponseCode = "200";
            httpResponse.ResponseDesc = "Registration disclosure added successfully.";
            return Ok(httpResponse);
        }

        [HttpGet]
        [Route("GetDisclosures")]
        public IActionResult GetDisclosures(string regId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            //TODO: Need to Implement
            DataSet ds = ef.GetRegistrationData(Convert.ToInt32(regId), "LoadDisclosure");
            string data = string.Empty;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) 
            { 
                return Ok(JsonConvert.SerializeObject(ds.Tables[0])); 
            }

            //var data = new List<DisclosureDto>();
            //data.Add(new DisclosureDto
            //{
            //    REG_QUESTION_ID = 1,
            //    REG_ID = 12345,
            //    REG_DISCLOSURES_ID = 1,
            //    STATE_CODE = "CA",
            //    INCIDENT_DATE = System.DateTime.Now.ToString("MM/dd/yyyy"),
            //    ACTION_TAKEN = "Test Action1",
            //    AGENCY_TAKING_ACTION = "Test Agency Action1",
            //    EXPLANATION_DETAILS = "Sample test",
            //    PROGRAM_AFFECTED = "Program affected test",
            //    QUESTION_TYPE_ID = "D01"

            //});
            //data.Add(new DisclosureDto
            //{
            //    REG_QUESTION_ID = 1,
            //    REG_ID = 12345,
            //    REG_DISCLOSURES_ID = 3,
            //    STATE_CODE = "MA",
            //    INCIDENT_DATE = System.DateTime.Now.AddDays(1).ToString("MM/dd/yyyy"),
            //    ACTION_TAKEN = "Test Action2",
            //    AGENCY_TAKING_ACTION = "Test Agency Action2",
            //    EXPLANATION_DETAILS = "Sample test",
            //    PROGRAM_AFFECTED = "Program affected test",
            //    QUESTION_TYPE_ID = "D01"

            //});
            //data.Add(new DisclosureDto
            //{
            //    REG_QUESTION_ID = 1,
            //    REG_ID = 12345,
            //    REG_DISCLOSURES_ID = 2,
            //    STATE_CODE = "CO",
            //    INCIDENT_DATE = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy"),
            //    ACTION_TAKEN = "Test Action2",
            //    AGENCY_TAKING_ACTION = "Test Agency Action2",
            //    EXPLANATION_DETAILS = "Sample test",
            //    PROGRAM_AFFECTED = "Program affected test",
            //    QUESTION_TYPE_ID = "D01"

            //});
            return Ok("[]");
        }

        [HttpPost]
        [Route("DeleteDisclosure")]
        public IActionResult DeleteDisclosure(int regDisclosureId)
        {
            EnrollmentFacade ef = new EnrollmentFacade();
            //TODO: Need to Implement
            ef.DeleteRegDisclosure(regDisclosureId);

            Models.HttpResponseMessage httpResponse = new Models.HttpResponseMessage();
            httpResponse.ResponseCode = "200";
            httpResponse.ResponseDesc = "Disclosure deleted successfully.";
            return Ok(httpResponse);
        }
    }

}
