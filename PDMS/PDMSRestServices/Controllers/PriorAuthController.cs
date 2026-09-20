using Amazon.Runtime.Internal.Transform;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class PriorAuthController : ControllerBase
    {
        [HttpGet]
        [Route("GetPlaceOfServiceDetails")]
        public IActionResult GetPlaceOfServiceDetails(string val = "", string desc = "")
        {

            DataTable dt = null;
            DataSet ds = ProviderController.GetPlaceOfserviceByCode(val.TrimAndReduce(), desc.TrimAndReduce());
            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            List<PlaceOfService> list = new List<PlaceOfService>();
            if (dt.Rows.Count > 0)
            {
                PlaceOfService Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new PlaceOfService(dr["PRIOR_AUTH_PLACE_OF_SERVICE_MMIS"].ToString(), dr["PRIOR_AUTH_PLACE_OF_SERVICE_DESC"].ToString());
                    list.Add(Vdetails);
                }
                return Ok(list);
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("LoadDestinationPayerIDs")]
        public IActionResult LoadDestinationPayerIDs(int destPayerIDINT)
        {
            List<DestinationPayerIDs> ldDestinationPayerIDs = new List<DestinationPayerIDs>();
            try
            {
                DataSet dataSet = LookupTableController.GetSubCapitaPayerIDs(destPayerIDINT);
                DataTable dt = dataSet.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DestinationPayerIDs paDestinationPayerIDs = new DestinationPayerIDs();
                        paDestinationPayerIDs.MCE_ID = Convert.ToString(dr["MCE_ID"]);
                        paDestinationPayerIDs.PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG = Convert.ToString(dr["PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG"]);
                        ldDestinationPayerIDs.Add(paDestinationPayerIDs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error at LoadDestinationPayerIDs method", ex);
            }
            return Ok(ldDestinationPayerIDs);
        }

        [HttpGet]
        [Route("GetICDDiagnosis")]
        public IActionResult GetICDDiagnosis(string code = "", string icdVersion = "", string diagnosisDes = "")
        {
            DataTable dt = null;
            DataSet ds = LookupTableController.GetICDDiagnosis(code.TrimAndReduce(), icdVersion.TrimAndReduce(), diagnosisDes.TrimAndReduce());

            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            List<PDMSRestServices.Models.Diagnosis> list = new List<PDMSRestServices.Models.Diagnosis>();
            if (dt.Rows.Count > 0)
            {

                PDMSRestServices.Models.Diagnosis Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new PDMSRestServices.Models.Diagnosis(dr["ICD10Diag"].ToString(), dr["ICDVersion"].ToString(), dr["DiagDesc"].ToString());
                    list.Add(Vdetails);
                }
            }
            return Ok(list);
        }


        [HttpDelete]
        [Route("DeleteDiagnosis")]
        public IActionResult DeleteDiagnosis(string diagnosisID = "", string linkId = "")
        {
            try
            {
                int lineNumber = Convert.ToInt32(diagnosisID);

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("LINK_SECTIONS", linkId);
                DataSet dsValues = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

                DataTable dtDiagnosis = dsValues.Tables[0];

                if (dsValues != null && dsValues.Tables.Count > 0)
                {
                    if (dsValues.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = dsValues.Tables[0].Select("PRIOR_AUTH_DIAGNOSIS_ID='" + lineNumber + "'")[0];
                        Dictionary<string, string> parms2 = new Dictionary<string, string>();
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_ID", lineNumber.ToString());
                        parms2.Add("LINK_SECTIONS", linkId);
                        PriorAuthHospitalController.DeletePriorAuthPanelData("DELETEPRIORAUTH_DIAGNOSIS", parms2);
                        dsValues.Tables[0].Rows.Remove(dr);
                        return Ok(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(false);
        }

        [HttpGet]
        [Route("GetDiagnosisNewLineAdd")]
        public IActionResult GetDiagnosisNewLineAdd(int paType, string medicaidID = "", string linkId = "")
        {
            return Ok(new DiagnosisNewLineAdd()
            {
                diagnosisCodeTypes = PriorAuthFacade.GetDiagnosisCodeType(),
                priorAuthDiagnoses = PriorAuthFacade.GetPriorAuthDiagnosisFromSession(paType, medicaidID, linkId)
            });

        }

        [HttpPost]
        [Route("DiagnosisSave")]
        public IActionResult DiagnosisSave(string claimType = "", string diagnosisCode = "", string diagnosisCodeDesc = "", string diagonsisType = "",
            string diagnosisTypeText = "", string diagUpdateDate = "", string MedicaidId = "", string action = "", string diagId = "", string linkId = "", string userName = "")
        {
            var id = HelperFacade.GetUserId(userName);
            try
            {
                if (action == "Add")
                {
                    string errorMessage = PriorAuthFacade.ValidateDiagnosisLineItem(Convert.ToInt32(claimType), MedicaidId, diagnosisTypeText, diagonsisType, linkId);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return Ok(errorMessage);
                    }
                }

                int saveId = PriorAuthHospitalController.GetPriorAuthDiagnosisSaveID(claimType);

                if (saveId <= 0)
                {
                    saveId = saveId + 1;
                }

                DataSet dsDiagnosis = new DataSet();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", claimType);
                parms.Add("MedicaidID", MedicaidId);
                parms.Add("LINK_SECTIONS", linkId);
                dsDiagnosis = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

                DataTable dtDiagnosis = dsDiagnosis.Tables[0];
                if (dtDiagnosis.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dtDiagnosis.Columns.Add(dcRowState);
                }
                if (dtDiagnosis.Columns.IndexOf("PRIOR_AUTH_INSTITUTIONALSAVE_ID") < 0)
                {
                    DataColumn dcInstSaveID = new DataColumn("PRIOR_AUTH_INSTITUTIONALSAVE_ID", typeof(string));
                    dtDiagnosis.Columns.Add(dcInstSaveID);
                }
                if (dtDiagnosis.Columns.IndexOf("PRIOR_AUTH_DENTALSAVE_ID") < 0)
                {
                    DataColumn dcDentalSaveID = new DataColumn("PRIOR_AUTH_DENTALSAVE_ID", typeof(string));
                    dtDiagnosis.Columns.Add(dcDentalSaveID);
                }
                if (dtDiagnosis.Columns.IndexOf("PRIOR_AUTH_PROFESSIONALSAVE_ID") < 0)
                {
                    DataColumn dcprofeeSaveID = new DataColumn("PRIOR_AUTH_PROFESSIONALSAVE_ID", typeof(string));
                    dtDiagnosis.Columns.Add(dcprofeeSaveID);
                }

                if (action == "Add")
                {
                    DataRow drDiagnosisRow = dsDiagnosis.Tables[0].NewRow();

                    drDiagnosisRow["RowState"] = "newadded";

                    if (Convert.ToInt32(claimType) == Convert.ToInt32(CON.ClaimsType.Dental))
                    {
                        drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = Convert.ToString(0);
                        drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = saveId;
                        drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                    }
                    if (Convert.ToInt32(claimType) == Convert.ToInt32(CON.ClaimsType.Professional))
                    {
                        drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = Convert.ToString(0);
                        drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                        drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = saveId;
                    }
                    if (Convert.ToInt32(claimType) == Convert.ToInt32(CON.ClaimsType.Institutional))
                    {
                        drDiagnosisRow["PRIOR_AUTH_INSTITUTIONALSAVE_ID"] = saveId;
                        drDiagnosisRow["PRIOR_AUTH_DENTALSAVE_ID"] = Convert.ToString(0);
                        drDiagnosisRow["PRIOR_AUTH_PROFESSIONALSAVE_ID"] = Convert.ToString(0);
                    }

                    drDiagnosisRow["PRIOR_AUTH_TYPE"] = claimType;
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE"] = diagnosisCode;
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DESC"] = diagnosisCodeDesc;
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] = diagonsisType;

                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"] = diagnosisTypeText;
                    drDiagnosisRow["LAST_MODIFIED_USER"] = id;
                    drDiagnosisRow["CREATED_BY_USER"] = id;
                    drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_STATUS"] = Convert.ToString(0);

                    if (string.IsNullOrEmpty(diagUpdateDate))
                        drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DATE"] = DBNull.Value;
                    else
                        drDiagnosisRow["PRIOR_AUTH_DIAGNOSIS_DATE"] = diagUpdateDate;

                    drDiagnosisRow["MedicaidID"] = MedicaidId;
                    dtDiagnosis.Rows.Add(drDiagnosisRow);
                }
                else
                {
                    DataView dataView = dtDiagnosis.AsDataView();
                    dataView.RowFilter = "PRIOR_AUTH_DIAGNOSIS_ID='" + diagId + "'";

                    if (dataView.Count > 0)
                    {
                        if (dataView[0]["RowState"] == DBNull.Value || dataView[0]["RowState"] == null)
                        {
                            dataView[0]["RowState"] = "updated";
                        }
                        dataView[0]["PRIOR_AUTH_TYPE"] = claimType;
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE"] = diagnosisCode;
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_DESC"] = diagnosisCodeDesc;
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] = diagonsisType;
                        dataView[0]["LAST_MODIFIED_USER"] = id;
                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_STATUS"] = Convert.ToString(0);

                        dataView[0]["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"] = diagnosisTypeText;

                        if (string.IsNullOrEmpty(diagUpdateDate))
                            dataView[0]["PRIOR_AUTH_DIAGNOSIS_DATE"] = DBNull.Value;
                        else
                            dataView[0]["PRIOR_AUTH_DIAGNOSIS_DATE"] = diagUpdateDate;

                        dataView[0]["MedicaidID"] = MedicaidId;
                    }
                }

                foreach (DataRow row in dsDiagnosis.Tables[0].Rows)
                {
                    if (action.Equals("Add"))
                    {
                        if (row["RowState"].ToString().Equals("newadded"))
                        {
                            Dictionary<string, string> parms2 = new Dictionary<string, string>();
                            parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", row["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                            parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", row["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                            parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", row["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                            parms2.Add("PRIOR_AUTH_TYPE", row["PRIOR_AUTH_TYPE"].ToString());
                            parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE", row["PRIOR_AUTH_DIAGNOSIS_CODE"].ToString());
                            parms2.Add("PRIOR_AUTH_DIAGNOSIS_DESC", row["PRIOR_AUTH_DIAGNOSIS_DESC"].ToString());
                            parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", row["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString());
                            parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms2.Add("LAST_MODIFIED_USER", row["LAST_MODIFIED_USER"].ToString());
                            parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                            parms2.Add("Created_By_User", row["Created_By_User"].ToString());
                            parms2.Add("PRIOR_AUTH_DIAGNOSIS_STATUS", row["PRIOR_AUTH_DIAGNOSIS_STATUS"].ToString());
                            parms2.Add("PRIOR_AUTH_DIAGNOSIS_DATE", row["PRIOR_AUTH_DIAGNOSIS_DATE"].ToString());
                            parms2.Add("LINK_SECTIONS", linkId);
                            parms2.Add("MedicaidID", row["MedicaidID"].ToString());
                            parms2.Add("Line", row["Line"].ToString());
                            parms2.Add("RowState", "added");
                            PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_DIAGNOSIS", parms2);
                            return Ok("true");
                        }
                    }
                    else
                    {
                        Dictionary<string, string> parms2 = new Dictionary<string, string>();
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_ID", diagId);
                        parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", row["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                        parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", row["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                        parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", row["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                        parms2.Add("PRIOR_AUTH_TYPE", claimType);
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE", diagnosisCode);
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_DESC", diagnosisCodeDesc);
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID", diagonsisType);
                        parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms2.Add("LAST_MODIFIED_USER", Convert.ToString(id));
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_STATUS", row["PRIOR_AUTH_DIAGNOSIS_STATUS"].ToString());
                        parms2.Add("PRIOR_AUTH_DIAGNOSIS_DATE", diagUpdateDate);
                        parms2.Add("LINK_SECTIONS", linkId);
                        parms2.Add("MedicaidID", MedicaidId);
                        parms2.Add("Line", row["Line"].ToString());
                        parms2.Add("RowState", "updated");
                        PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("UPDATEPRIORAUTH_DIAGNOSIS", parms2);
                        return Ok("true");
                    }
                }
                return Ok("true");
            }
            catch (Exception ex)
            {
                throw new Exception("Error at DiagnosisUpdatedSave method", ex);
            }
        }

        [HttpGet]
        [Route("GetDiagnosisNewItems")]
        public IActionResult GetDiagnosisNewItems(string paType = "", string medicaidID = "", string linkId = "")
        {
            return Ok(new DiagnosisNewLineAdd()
            {
                diagnosisCodeTypes = null,
                priorAuthDiagnoses = PriorAuthFacade.GetPriorAuthDiagnosisFromSession(paType, medicaidID, linkId)
            });
        }

        [HttpGet]
        [Route("CheckDiagnosisDataExist")]
        public IActionResult CheckDiagnosisDataExist(int paType, string medicaidID = "", string linkId = "")
        {
            DataSet ds = null;
            DataTable dt = null;

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", paType.ToString());
            parms.Add("MedicaidID", medicaidID);
            parms.Add("LINK_SECTIONS", linkId);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

            if (ds != null)
                dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpGet]
        [Route("GetProviderNPI")]
        public IActionResult GetProviderNPI(string npi = "", string medicaidID = "", string lastName = "", string firstName = "")
        {
            DataTable dt = null;
            DataSet ds = ProviderController.SearchProviderNPI(npi.TrimAndReduce(), medicaidID.TrimAndReduce(), lastName.TrimAndReduce(), firstName.TrimAndReduce());
            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            List<RegProvider> list = new List<RegProvider>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RegProvider Vdetails = new RegProvider();

                    Vdetails.NPI = Convert.ToString(dr["NPI"]);
                    Vdetails.MEDICAID_ID = Convert.ToString(dr["MEDICAID_ID"]);
                    Vdetails.FIRST_NAME = Convert.ToString(dr["FIRST_NAME"]);
                    Vdetails.ADDRESS1 = Convert.ToString(dr["ADDRESS1"]);
                    Vdetails.ADDRESS2 = Convert.ToString(dr["ADDRESS2"]);
                    Vdetails.CITY = Convert.ToString(dr["CITY"]);
                    Vdetails.STATE = Convert.ToString(dr["STATE"]);
                    Vdetails.ZIP = Convert.ToString(dr["ZIP"]);
                    Vdetails.EXT_ZIP = Convert.ToString(dr["EXT_ZIP"]);
                    Vdetails.LAST_OR_BUSINESS_NAME = Convert.ToString(dr["LAST_OR_BUSINESS_NAME"]);

                    list.Add(Vdetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRevenueCodeData")]
        public IActionResult GetRevenueCodeData(string revenueCode = "", string revenueCodeDesc = "", bool isTextChangesEvent = false)
        {
            List<RevenueCodes> revenueCodeslist = new List<RevenueCodes>();
            DataTable dt = null;
            DataSet ds = null;

            if (!isTextChangesEvent)
                ds = LookupTableController.GetRevenueCodePopupSearch(revenueCode.TrimAndReduce(), revenueCodeDesc.TrimAndReduce());
            else if (isTextChangesEvent)
                ds = LookupTableController.GetRevenueCodePopupSearch(revenueCode.TrimAndReduce(), "", true);

            if (ds != null)
            {
                dt = ds.Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    revenueCodeslist.Add(new RevenueCodes()
                    {
                        RevenueCode = Convert.ToString(dr["PRIOR_AUTH_REVENUE_CODE_MMIS"]),
                        RevenueCodeDesc = Convert.ToString(dr["PRIOR_AUTH_REVENUE_CODE_DESC"])
                    });
                }
            }
            return Ok(revenueCodeslist);
        }

        [HttpGet]
        [Route("GetProcedureCodeData")]
        public IActionResult GetProcedureCodeData(string procCode = "", string procCodeDesc = "", bool isTextChangesEvent = false)
        {
            List<ProcedureCodes> procCodeslist = new List<ProcedureCodes>();
            DataTable dt = null;
            DataSet ds = null;

            if (!isTextChangesEvent)
                dt = LookupTableController.GetProcedureCodeServiceDetail(procCode.TrimAndReduce(), procCodeDesc.TrimAndReduce());
            else if (isTextChangesEvent)
                dt = LookupTableController.GetProcedureCodeServiceDetail(procCode.TrimAndReduce(), "", true);

            if (ds != null)
            {
                dt = ds.Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    procCodeslist.Add(new ProcedureCodes()
                    {
                        ProcedureCode = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"]),
                        ProcedureCodeDesc = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_DESC"])
                    });
                }
            }
            return Ok(procCodeslist);
        }

        [HttpGet]
        [Route("GetProcedureCodeData_InstitutionalDynamic")]
        public IActionResult GetProcCodeInstitutionalDynamic(string procCode = "", string procCodeDesc = "")
        {
            List<ProcedureCodes> revenueCodeslist = new List<ProcedureCodes>();
            DataTable dt = null;
            DataSet ds = null;

            dt = LookupTableController.GetProcedureCodeServiceDetail_InstitutionalDynamic(procCode.TrimAndReduce(), procCodeDesc.TrimAndReduce());

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    revenueCodeslist.Add(new ProcedureCodes()
                    {
                        ProcedureCode = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"]),
                        ProcedureCodeDesc = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_DESC"])
                    });
                }
            }

            return Ok(revenueCodeslist);
        }

        [HttpPost]
        [Route("SaveNote")]
        public IActionResult SaveNote(string note = "", string medicaidID = "", string paType = "")
        {
            try
            {
                Guid lnkID = Guid.NewGuid();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("LINK_SECTIONS", lnkID.ToString());
                parms.Add("PRIOR_AUTH_SAVE_PROVIDER_MEDICAIDID", medicaidID);
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", paType.ToString());
                parms.Add("PRIOR_AUTH_NOTES", note);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", lnkID.ToString());
                parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", lnkID.ToString());

                int noteid = PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_NOTES", parms);

                return Ok(new NoteResult()
                {
                    ID = noteid,
                    status = true
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateNote")]
        public IActionResult UpdateNote(string noteID = "", string operation = "", string noteText = "", string paType = "", string userName = "")
        {
            try
            {
                if (operation == "Update")
                {
                    var createdBy = HelperFacade.GetUserId(userName);
                    if (!string.IsNullOrEmpty(noteID) && !string.IsNullOrEmpty(noteText))
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("PRIOR_AUTH_Authorization_Type_ID", paType.ToString());
                        parms.Add("PRIOR_AUTH_NOTES", noteText.ToString());
                        parms.Add("PRIOR_AUTH_NOTES_ID", noteID);
                        PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("UPDATEPRIORAUTH_NOTES", parms);
                    }

                    return Ok(new NoteResult()
                    {
                        status = true
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(new NoteResult()
            {
                status = false
            });
        }


        [HttpDelete]
        [Route("DeleteNote")]
        public IActionResult DeleteNote(string noteID = "", string paType = "")
        {
            try
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_NOTES_ID", noteID);
                parms.Add("PRIOR_AUTH_Authorization_Type_ID", paType);
                PriorAuthHospitalController.DeletePriorAuthPanelData("DELETEPRIORAUTH_NOTES", parms);

                return Ok(new NoteResult() { status = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(new NoteResult() { status = false });
        }

        [HttpGet]
        [Route("GetNotes")]
        public IActionResult GetNotes(string noteID = "", string paType = "")
        {
            ProviderNotes notes = new ProviderNotes();
            try
            {
                if (!string.IsNullOrEmpty(noteID) && !noteID.Equals("undefined"))
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_NOTES_ID", noteID);
                    parms.Add("PRIOR_AUTH_Authorization_Type_ID", paType.ToString());
                    DataSet notesDataDS = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_NOTES", parms);

                    if (Methods.HasRows(notesDataDS))
                    {
                        notes.noteId = Convert.ToInt32(notesDataDS.Tables[0].Rows[0]["PRIOR_AUTH_NOTES_ID"]);
                        notes.Note = notesDataDS.Tables[0].Rows[0]["PRIOR_AUTH_NOTES"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(notes);
        }

        [HttpGet]
        [Route("GetServiceDetailsInstitutionals")]
        public IActionResult GetServiceDetailsInstitutionals(string paType = "", string medicaidID = "", string linkId = "")
        {
            List<ServiceDetailsInstitutional> serviceDetails = new List<ServiceDetailsInstitutional>();
            try
            {
                DataSet ds = null; DataTable dataTable = null;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", paType.ToString());
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                dataTable = ds.Tables[0];

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (dataTable.Columns.IndexOf("RowState") < 0)
                    {
                        DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                        dataTable.Columns.Add(dcRowState);
                    }

                    dataTable = ds.Tables[0].Copy();
                    DataRow[] dtRows = dataTable.Select("RowState='deleted'");
                    foreach (DataRow dr in dtRows)
                    {
                        dataTable.Rows.Remove(dr);
                    }
                    DataSet serviceTypeCodes = LookupTableController.GetServiceTypeCode();
                    DataTable serviceTypeDt = serviceTypeCodes.Tables[0];

                    if (!dataTable.Columns.Contains("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"))
                    {
                        dataTable.Columns.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR");
                    }

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            ServiceDetailsInstitutional priorAuthDiagnosis = new ServiceDetailsInstitutional();

                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_ID = dr["PRIOR_AUTH_SERVICE_DETAIL_ID"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_SERVICE_DETAIL_ID"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_REVENUE_CODE = dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != null && dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_ID = dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != null && dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE = dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != null && dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]) : string.Empty;

                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value
                            && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]) : string.Empty;

                            }
                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value
                             && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]) : string.Empty;

                            }

                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR = (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"]) != "" && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value) ? Convert.ToDecimal(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR = (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]) != "" && dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value) ? Convert.ToDecimal(dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_STATUS_ID = dr["PRIOR_AUTH_STATUS_ID"] != null && dr["PRIOR_AUTH_STATUS_ID"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_STATUS_ID"]) : string.Empty;
                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString() != "")
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);

                            string serviceTypeCode = Convert.ToString(dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);

                            DataRow dataRow = serviceTypeDt.AsEnumerable().SingleOrDefault(r => r.Field<int>("PRIOR_AUTH_SERVICE_CODE_TYPE_ID") == Convert.ToInt32(serviceTypeCode));

                            if (dataRow != null && dataRow["PRIOR_AUTH_SERVICE_CODE_TYPE_CODE"] != null)
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = Convert.ToString(dataRow["PRIOR_AUTH_SERVICE_CODE_TYPE_CODE"]);
                            serviceDetails.Add(priorAuthDiagnosis);
                        }
                    }
                }
                return Ok(serviceDetails);
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetServiceDetails method", ex);
            }
        }

        [HttpGet]
        [Route("GetServiceDetailsInstitutional")]
        public IActionResult GetServiceDetailsInstitutional(string lineNumber = "", string MedicaidId = "", string patype = "", string linkId = "")
        {
            ServiceDetailsInstitutional priorAuthDiagnosis = new ServiceDetailsInstitutional();
            try
            {

                DataSet ds; DataTable dataTable = null;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", patype);
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                dataTable = ds != null ? ds.Tables[0] : null;

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow currentRow = null;

                    dataTable = ds.Tables[0];

                    if (!dataTable.Columns.Contains("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"))
                    {
                        dataTable.Columns.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR");
                    }

                    int gridSno = 0;
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        gridSno++;
                        if (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_ID"]) == lineNumber)
                        {
                            currentRow = dr;
                            break;
                        }

                    }

                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_REVENUE_CODE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_ID = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]);
                    //priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]);
                    priorAuthDiagnosis.PRIOR_AUTH_STATUS_ID = Convert.ToString(currentRow["PRIOR_AUTH_STATUS_ID"]);
                    // priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT"]);


                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] != null &&
                    currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"] != DBNull.Value)
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_ID"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != null &&
                       currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != DBNull.Value)
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_REVENUE_CODE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_REVENUE_CODE"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null && Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]) != "" &&
                       currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value)
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                       currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value &&
                       Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]) != "")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != null &&
                       currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != DBNull.Value && Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]) != "")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                       currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                       !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                    }
                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != null &&
                     currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value &&
                     !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"]);
                    }


                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value
                              && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] != DBNull.Value
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]).ToShortDateString() != "1/1/1900"
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]) >= Convert.ToDateTime("1/1/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]).ToShortDateString() != "1/1/0001")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] != DBNull.Value
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]).ToShortDateString() != "1/1/1900"
                        && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]) >= Convert.ToDateTime("1/1/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]).ToShortDateString() != "1/1/0001")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != null &&
                       currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != DBNull.Value &&
                       !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_ID = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);

                        DataSet serviceTypeCodes = LookupTableController.GetServiceTypeCode();
                        DataTable serviceTypeDt = serviceTypeCodes.Tables[0];
                        DataRow dataRow = serviceTypeDt.AsEnumerable().SingleOrDefault(r => r.Field<int>("PRIOR_AUTH_SERVICE_CODE_TYPE_ID") == Convert.ToInt32(priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_ID));

                        if (dataRow != null)
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = Convert.ToString(dataRow["PRIOR_AUTH_SERVICE_CODE_TYPE_CODE"]);

                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != null &&
                      currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != DBNull.Value &&
                      !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null &&
                      currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value &&
                      !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);
                    }

                    if (currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"] != null &&
                      currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"] != DBNull.Value &&
                      !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_REQUESTED_UNITS_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_REQUESTED_UNITS_ID"]);

                    }
                    //txtAuthorizedUnits.Text = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != null &&
                      currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != DBNull.Value &&
                      !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"]);
                    }

                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != null &&
                      currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != DBNull.Value &&
                      !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString()))
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"]);
                    }

                    if (currentRow["PRIOR_AUTH_LEVEL_CARE_ID"] != null &&
                      currentRow["PRIOR_AUTH_LEVEL_CARE_ID"] != DBNull.Value &&
                       !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"].ToString()) &&
                      Convert.ToInt32(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"]) != 0)
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_LEVEL_CARE_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_LEVEL_CARE_ID"]);
                    }


                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != null &&
                        currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                        !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString()) &&
                        Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]) >= 0)
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]);
                    }


                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != null && currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value
                        && Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]) != "")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"]);
                    }


                    if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != null &&
                     currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != DBNull.Value &&
                     Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"]) != "")
                    {
                        priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO = Convert.ToString(currentRow["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"]);
                    }

                    int value = 0;

                    if (currentRow["PRIOR_AUTH_STATUS_ID"] != null &&
                     currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                      Convert.ToString(currentRow["PRIOR_AUTH_STATUS_ID"]) != "")
                    {
                        string x = currentRow["PRIOR_AUTH_STATUS_ID"].ToString();
                        if (!string.IsNullOrEmpty(x))
                        {

                            if (int.TryParse(x, out value))
                            {
                                if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 1)
                                {
                                    priorAuthDiagnosis.StatusDesc = "Submission Pending";
                                }
                                else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 2)
                                {
                                    priorAuthDiagnosis.StatusDesc = "Approved";
                                }
                                else if (Convert.ToInt32(currentRow["PRIOR_AUTH_STATUS_ID"]) == 3)
                                {
                                    priorAuthDiagnosis.StatusDesc = "Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetServiceDetails method", ex);
            }
            return Ok(priorAuthDiagnosis);
        }

        [HttpGet]
        [Route("GetServiceDetailsInstitutionalsById")]
        public IActionResult GetServiceDetailsInstitutionalsById(int? lineNumber, string medicaidID = "")
        {
            ServiceDetailsInstitutional priorAuthDiagnosis = new ServiceDetailsInstitutional();
            try
            {
                DataSet ds = null; DataTable dataTable = null;

                if (!string.IsNullOrEmpty(medicaidID))
                {
                    ds = PriorAuthFacade.GetPriorAuthServiceDetailById(lineNumber, medicaidID.TrimAndReduce());
                    dataTable = ds.Tables[0];
                }
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataSet serviceTypeCodes = LookupTableController.GetServiceTypeCode();
                    DataTable serviceTypeDt = serviceTypeCodes.Tables[0];

                    if (!dataTable.Columns.Contains("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"))
                    {
                        dataTable.Columns.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR");
                    }

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {


                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_ID = dr["PRIOR_AUTH_SERVICE_DETAIL_ID"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_SERVICE_DETAIL_ID"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_REVENUE_CODE = dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != null && dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_REVENUE_CODE"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_ID = dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != null && dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE = dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != null && dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"]) : string.Empty;

                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value
                            && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"]) : string.Empty;

                            }
                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value
                             && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS = dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"]) : string.Empty;

                            }

                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR = (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"]) != "" && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value) ? Convert.ToDecimal(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR = (Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]) != "" && dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value) ? Convert.ToDecimal(dr["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_STATUS_ID = dr["PRIOR_AUTH_STATUS_ID"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_STATUS_ID"]) : string.Empty;
                            if (dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString() != "")
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(dr["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"]);

                            string serviceTypeCode = Convert.ToString(dr["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"]);

                            DataRow dataRow = serviceTypeDt.AsEnumerable().SingleOrDefault(r => r.Field<int>("PRIOR_AUTH_SERVICE_CODE_TYPE_ID") == Convert.ToInt32(serviceTypeCode));

                            if (dataRow != null && dataRow["PRIOR_AUTH_SERVICE_CODE_TYPE_CODE"] != null)
                                priorAuthDiagnosis.PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = Convert.ToString(dataRow["PRIOR_AUTH_SERVICE_CODE_TYPE_CODE"]);


                            priorAuthDiagnosis.PRIOR_AUTH_LEVEL_CARE_ID = dr["PRIOR_AUTH_LEVEL_CARE_ID"] != null && dr["PRIOR_AUTH_LEVEL_CARE_ID"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_LEVEL_CARE_ID"]) : 0;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC = dr["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE = dr["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"]) : string.Empty;
                            priorAuthDiagnosis.PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO = dr["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != null && dr["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"]) : string.Empty;

                        }
                    }
                }
                return Ok(priorAuthDiagnosis);
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetServiceDetails method", ex);
            }
        }

        [HttpDelete]
        [Route("DeleteServiceDetails")]
        public IActionResult DeleteServiceDetails(string lineNumber = "", string paType = "", string linkId = "")
        {
            try
            {
                string clmType;
                if (paType.Equals("dental"))
                {
                    clmType = "0";
                }
                else if (paType.Equals("Professional"))
                {
                    clmType = "2";
                }
                else
                {
                    clmType = "1";
                }
                if (paType == "Institutional")
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", clmType);
                    parms.Add("LINK_SECTIONS", linkId);
                    DataSet dsValues = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    if (dsValues != null)
                    {
                        Dictionary<string, string> parms1 = new Dictionary<string, string>();
                        parms1.Add("PRIOR_AUTH_TYPE", clmType);
                        parms1.Add("LineNumber", lineNumber);
                        PriorAuthHospitalController.DeletePriorAuthPanelData("DELETEPRIORAUTH_SERVICEDETAILS", parms1);
                        return Ok(true);
                    }
                }
                if (paType == "dental")
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", clmType);
                    parms.Add("LINK_SECTIONS", linkId);
                    DataSet dsValues = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    if (dsValues != null)
                    {
                        Dictionary<string, string> parms2 = new Dictionary<string, string>();
                        parms2.Add("PRIOR_AUTH_TYPE", clmType);
                        parms2.Add("LineNumber", lineNumber);
                        PriorAuthHospitalController.DeletePriorAuthPanelData("DELETEPRIORAUTH_SERVICEDETAILS", parms2);
                        return Ok(true);
                    }
                }
                if (paType == "Professional")
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", clmType);
                    parms.Add("LINK_SECTIONS", linkId);
                    DataSet dsValues = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                    if (dsValues != null)
                    {
                        Dictionary<string, string> parms3 = new Dictionary<string, string>();
                        parms3.Add("PRIOR_AUTH_TYPE", clmType);
                        parms3.Add("LineNumber", lineNumber);
                        PriorAuthHospitalController.DeletePriorAuthPanelData("DELETEPRIORAUTH_SERVICEDETAILS", parms3);
                        return Ok(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while deleting the service details...!");
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("SaveInstitutionalServiceDetails")]
        public IActionResult SaveInstitutionalServiceDetails(string codeType = "", string procedureCode = "", string procCodeDesc = "", string providerServiceNote = "",
        string revenueCode = "", string levelCareID = "", string reqUnits = "", string unitMeasure = "", string requnitsfee = "", string fdos = "", string tdos = "", string status = "", string MedicaidId = "", string trackingNo = "",
        string codeTypeText = "", string operation = "", string lineNumber = "", string patype = "", string linkId = "", string userName = "")
        {
            try
            {
                if (operation.ToLower().Equals("add"))
                {
                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    //int codeTypeId = Convert.ToInt32(codeType);
                    //int levelofCareId = Convert.ToInt32(levelCareID);
                    //int unitMeasurementId = Convert.ToInt32(unitMeasure);

                    int codeTypeId = !string.IsNullOrEmpty(codeType) ? Convert.ToInt32(codeType) : 0;
                    int levelofCareId = !string.IsNullOrEmpty(levelCareID) ? Convert.ToInt32(levelCareID) : 0;
                    int unitMeasurementId = !string.IsNullOrEmpty(unitMeasure) ? Convert.ToInt32(unitMeasure) : 0;

                    decimal reqUnitsFee = !string.IsNullOrEmpty(requnitsfee) ? Convert.ToDecimal(requnitsfee) : 0;

                    // decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);

                    DateTime reqFDOS = Convert.ToDateTime(fdos);
                    DateTime reqTDOS = Convert.ToDateTime(tdos);

                    // int serviceTrackingNo = Convert.ToInt64(txtLnServiceTrackingNo.Text);
                    int statusId = 1;
                    if (status == "Submission Pending")
                    {
                        statusId = 1;
                    }
                    else if (status == "Approved")
                    {
                        statusId = 2;
                    }
                    else if (status == "Denied")
                    {
                        statusId = 3;
                    }

                    var createdBy = HelperFacade.GetUserId(userName);
                    int saveId = PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Institutional", MedicaidId);
                    int lineId = PriorAuthHospitalController.GetPriorAuthServiceDetailLineID("Institutional", MedicaidId);
                    parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", codeTypeId);
                    //parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT", codeTypeText);
                    parms.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", Convert.ToString(revenueCode));
                    parms.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", Convert.ToString(procedureCode));
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", Convert.ToString(reqUnits));
                    parms.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", unitMeasurementId);
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", reqUnitsFee);
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR", reqUnitsFee);
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                    parms.Add("PRIOR_AUTH_STATUS_ID", 1);
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", Convert.ToString(procCodeDesc));
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", Convert.ToString(providerServiceNote));
                    parms.Add("PRIOR_AUTH_LEVEL_CARE_ID", levelofCareId);
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", trackingNo);
                    parms.Add("MedicaidID", Convert.ToString(MedicaidId));
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", "");
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", ""); ;
                    int authUnits = 0;
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", authUnits);


                    int remainingUnits = !string.IsNullOrEmpty("") ? Convert.ToInt32("") : 0;
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", Convert.ToString(""));

                    if (saveId > 0)
                    {
                        parms.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", Convert.ToString(saveId));
                    }
                    else
                    {
                        parms.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", Convert.ToString(saveId + 1));
                    }

                    if (lineId > 0)
                    {
                        parms.Add("Line", Convert.ToString(lineId));
                    }
                    else
                    {
                        parms.Add("Line", Convert.ToString(lineId + 1));
                    }
                    Random random = new Random();
                    parms.Add("PRIOR_AUTH_SERVICE_DETAIL_ID", random.Next());

                    Dictionary<string, string> parms3 = new Dictionary<string, string>();
                    parms3.Add("PRIOR_AUTH_TYPE", patype);
                    parms3.Add("LINK_SECTIONS", linkId);
                    DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms3);

                    if (ds != null && ds.Tables[0] != null && !ds.Tables[0].Columns.Contains("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"))
                    {
                        ds.Tables[0].Columns.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR");
                    }

                    DataRow dataRowForAdd = ds.Tables[0].NewRow();

                    foreach (var key in parms)
                    {
                        if (key.Value == null)
                            dataRowForAdd[key.Key] = DBNull.Value;

                        if ((key.Key == "PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS" || key.Key == "PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS") && (key.Value == null || key.Value == string.Empty))
                        {
                            continue;
                        }

                        if (key.Key == "PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR" && key.Value == string.Empty)
                        {
                            continue;
                        }
                        dataRowForAdd[key.Key] = key.Value;

                    }

                    ds.Tables[0].Rows.Add(dataRowForAdd);
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", dataRowForAdd["PRIOR_AUTH_SERVICE_REVENUE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", dataRowForAdd["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", dataRowForAdd["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", dataRowForAdd["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", dataRowForAdd["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_LEVEL_CARE_ID", dataRowForAdd["PRIOR_AUTH_LEVEL_CARE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", linkId);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", linkId);
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_STATUS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_STATUS"].ToString());
                    parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", dataRowForAdd["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                    parms2.Add("MedicaidID", dataRowForAdd["MedicaidID"].ToString());
                    parms2.Add("Line", dataRowForAdd["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", dataRowForAdd["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                    parms2.Add("LINK_SECTIONS", linkId);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_INSTSERVICEDETAILS", parms2);
                    return Ok(true);
                }
                else if (operation.ToLower().Equals("update"))
                {
                    int codeTypeId = !string.IsNullOrEmpty(codeType) ? Convert.ToInt32(codeType) : 0;
                    int levelofCareId = !string.IsNullOrEmpty(levelCareID) ? Convert.ToInt32(levelCareID) : 0;
                    int unitMeasurementId = !string.IsNullOrEmpty(unitMeasure) ? Convert.ToInt32(unitMeasure) : 0;

                    decimal reqUnitsFee = !string.IsNullOrEmpty(requnitsfee) ? Convert.ToDecimal(requnitsfee) : 0;

                    DateTime reqFDOS = Convert.ToDateTime(fdos);
                    DateTime reqTDOS = Convert.ToDateTime(tdos);

                    DateTime? authFDOS = null;
                    DateTime? authTDOS = null;

                    int statusId = 1;

                    if (status == "Submission Pending")
                    {
                        statusId = 1;
                    }
                    else if (status == "Approved")
                    {
                        statusId = 2;
                    }
                    else if (status == "Denied")
                    {
                        statusId = 3;
                    }
                    var createdBy = HelperFacade.GetUserId(userName);

                    int authUnits = 0;
                    decimal authDollars = 0;

                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("PRIOR_AUTH_TYPE", patype);
                    parms.Add("LINK_SECTIONS", linkId);
                    DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);

                    string searchKey = "PRIOR_AUTH_SERVICE_DETAIL_ID ='" + Convert.ToInt32(lineNumber) + "'";
                    DataRow dataRowForUpdate = ds.Tables[0].Select(searchKey).FirstOrDefault();


                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_ID"] = Convert.ToInt32(lineNumber);
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"] = codeTypeId;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_REVENUE_CODE"] = revenueCode;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"] = procedureCode;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"] = reqUnits;
                    dataRowForUpdate["PRIOR_AUTH_REQUESTED_UNITS_ID"] = unitMeasurementId;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"] = reqUnitsFee;

                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR"] = reqUnitsFee;

                    //  PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR

                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"] = reqFDOS;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"] = reqTDOS;
                    dataRowForUpdate["PRIOR_AUTH_STATUS_ID"] = statusId;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"] = procCodeDesc;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"] = providerServiceNote;
                    dataRowForUpdate["PRIOR_AUTH_LEVEL_CARE_ID"] = levelofCareId;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"] = authUnits;
                    int remainingUnits = !string.IsNullOrEmpty("") ? Convert.ToInt32("") : 0;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] = remainingUnits;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"] = authDollars;
                    if (authTDOS != null)
                        dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"] = authTDOS;
                    if (authFDOS != null)
                        dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"] = authFDOS;
                    dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"] = trackingNo;

                    dataRowForUpdate["MedicaidID"] = MedicaidId;

                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_ID", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", dataRowForUpdate["PRIOR_AUTH_SERVICE_REVENUE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", dataRowForUpdate["PRIOR_AUTH_SERVICE_CODE_TYPE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", dataRowForUpdate["PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", dataRowForUpdate["PRIOR_AUTH_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", dataRowForUpdate["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_LEVEL_CARE_ID", dataRowForUpdate["PRIOR_AUTH_LEVEL_CARE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", linkId);
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO"].ToString());
                    parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_STATUS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_STATUS"].ToString());
                    parms2.Add("PRIOR_AUTH_INSTITUTIONALSAVE_ID", dataRowForUpdate["PRIOR_AUTH_INSTITUTIONALSAVE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", dataRowForUpdate["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                    parms2.Add("MedicaidID", dataRowForUpdate["MedicaidID"].ToString());
                    parms2.Add("Line", dataRowForUpdate["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", dataRowForUpdate["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                    parms2.Add("LINK_SECTIONS", linkId);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("UPDATEPRIORAUTH_INSTSERVICEDETAILS", parms2);
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Add Inst Saving Service Details", ex);
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("SaveProfessionalServiceDetails")]
        public IActionResult SaveProfessionalServiceDetails(string procedureCode = "", string procCodeDesc = "", string providerServiceNote = "",
          string reqUnits = "", string remainUnits = "", string requnitsfee = "", string fdos = "", string tdos = "", string status = "", string unitMeasurement = "",
          string modifier1 = "", string modifier2 = "", string modifier3 = "", string modifier4 = "",
           string MedicaidId = "", string serviceTrackingNo = "", string operation = "", string lineNumber = "", string patype = "", string linkId = "", string userName = "")
        {
            try
            {
                if (operation.ToLower().Equals("add"))
                {
                    decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);
                    decimal authDollars = 0;
                    int remainingUnits = !string.IsNullOrEmpty(remainUnits) ? Convert.ToInt32(remainUnits) : 0;

                    DateTime reqFDOS = Convert.ToDateTime(fdos);
                    DateTime reqTDOS = Convert.ToDateTime(tdos);

                    var requestUser = Guid.NewGuid();
                    int statusId = 1;
                    if (status == "Submission Pending")
                    {
                        statusId = 1;
                    }
                    else if (status == "Approved")
                    {
                        statusId = 2;
                    }
                    else if (status == "Denied")
                    {
                        statusId = 3;
                    }
                    var createdBy = HelperFacade.GetUserId(userName);

                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    var id = HelperFacade.GetUserId(userName);
                    int saveId = PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Professional", MedicaidId);
                    int lineId = PriorAuthHospitalController.GetPriorAuthServiceDetailLineID("Professional", MedicaidId);
                    parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", procedureCode);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", reqUnits);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));

                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", modifier1);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", modifier2);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", modifier3);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", modifier4);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", unitMeasurement);
                    //parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", reqUnits);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", reqFDOS);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", reqTDOS);
                    parms.Add("PRIOR_AUTH_STATUS_ID", statusId.ToString());
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", procCodeDesc);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", providerServiceNote);

                    int authUnits = 0;
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", authDollars);

                    if (serviceTrackingNo == null) serviceTrackingNo = string.Empty;


                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", serviceTrackingNo);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", remainingUnits);

                    parms.Add("MedicaidID", MedicaidId);

                    Random rand = new Random();
                    parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", rand.Next());
                    parms.Add("RowState", "added");

                    if (saveId > 0)
                    {
                        parms.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", saveId);
                    }
                    else
                    {
                        parms.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", saveId + 1);
                    }

                    if (lineId > 0)
                    {
                        parms.Add("Line", lineId);//int
                    }
                    else
                    {
                        parms.Add("Line", lineId + 1);
                    }

                    Dictionary<string, string> parms3 = new Dictionary<string, string>();
                    parms3.Add("PRIOR_AUTH_TYPE", patype);
                    parms3.Add("LINK_SECTIONS", linkId);
                    DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms3);
                    DataRow dataRowForAdd = ds.Tables[0].NewRow();

                    foreach (var key in parms)
                    {
                        if ((key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER_1") ||
                                (key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER_2") ||
                                (key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER_3") ||
                                (key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER_4") ||
                                (key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS") ||
                                (key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS") ||
                            (key.Key == "PRIOR_AUTH_DISCHARGE_STATUS_ID") || (key.Key == "PRIOR_AUTH_PROFESSIONALSAVE_ID"))
                        {
                            continue;
                        }

                        dataRowForAdd[key.Key] = key.Value;

                    }
                    dataRowForAdd["RowState"] = "added";

                    ds.Tables[0].Rows.Add(dataRowForAdd);
                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", dataRowForAdd["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", dataRowForAdd["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", linkId);
                    parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms2.Add("Created_By_User", linkId);
                    parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", dataRowForAdd["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("MedicaidID", dataRowForAdd["MedicaidID"].ToString());
                    parms2.Add("Line", dataRowForAdd["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", dataRowForAdd["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", dataRowForAdd["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                    parms2.Add("LINK_SECTIONS", linkId);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_PROFFSERVICEDETAILS", parms2);
                    return Ok(true);
                }
                else if (operation.ToLower().Equals("update"))
                {
                    decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);
                    decimal authDollars = 0;
                    int remainingUnits = !string.IsNullOrEmpty(remainUnits) ? Convert.ToInt32(remainUnits) : 0;

                    Dictionary<string, string> parms8 = new Dictionary<string, string>();
                    parms8.Add("PRIOR_AUTH_TYPE", patype);
                    parms8.Add("LINK_SECTIONS", linkId);
                    DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms8);
                    string RowFilter = "PRIOR_AUTH_PROFFSERVICE_DETAIL_ID ='" + lineNumber + "'";
                    DataRow dataRowForUpdate = ds.Tables[0].Select(RowFilter).FirstOrDefault();
                    if (dataRowForUpdate != null)
                    {
                        int authUnits = 0;

                        DateTime reqFDOS;
                        if (DateTime.TryParse(fdos, out reqFDOS))
                        {
                            reqFDOS = Convert.ToDateTime(fdos);
                        }

                        DateTime reqTDOS;
                        if (DateTime.TryParse(tdos, out reqTDOS))
                        {
                            reqTDOS = Convert.ToDateTime(tdos);
                        }



                        int statusId = 1;
                        if (status == "Submission Pending")
                        {
                            statusId = 1;
                        }
                        else if (status == "Approved")
                        {
                            statusId = 2;
                        }
                        else if (status == "Denied")
                        {
                            statusId = 3;
                        }

                        var createdBy = HelperFacade.GetUserId(userName);

                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        var id = HelperFacade.GetUserId(userName);

                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", Convert.ToString(lineNumber));
                        parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", procedureCode);
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", reqUnits);
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", Convert.ToString(modifier1));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", Convert.ToString(modifier2));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", Convert.ToString(modifier3));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", Convert.ToString(modifier4));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", Convert.ToString(unitMeasurement));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                        parms.Add("PRIOR_AUTH_STATUS_ID", Convert.ToString(statusId));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", Convert.ToString(procCodeDesc));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", Convert.ToString(providerServiceNote));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", Convert.ToString(serviceTrackingNo));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", Convert.ToString(authDollars));
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                        parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));

                        foreach (var key in parms)
                        {
                            if (key.Value == null)
                                dataRowForUpdate[key.Key] = DBNull.Value;

                            if ((key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS" || key.Key == "PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS") && (key.Value == null || key.Value == string.Empty))
                            {
                                continue;
                            }
                            //else*/
                            dataRowForUpdate[key.Key] = key.Value;

                        }
                        dataRowForUpdate["RowState"] = "updated";
                    }

                    Dictionary<string, string> parms2 = new Dictionary<string, string>();
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", dataRowForUpdate["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_STATUS_ID", dataRowForUpdate["PRIOR_AUTH_STATUS_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString());
                    parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms2.Add("LAST_MODIFIED_USER", linkId);
                    parms2.Add("PRIOR_AUTH_PROFESSIONALSAVE_ID", dataRowForUpdate["PRIOR_AUTH_PROFESSIONALSAVE_ID"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"].ToString());
                    parms2.Add("MedicaidID", dataRowForUpdate["MedicaidID"].ToString());
                    parms2.Add("Line", dataRowForUpdate["Line"].ToString());
                    parms2.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS", dataRowForUpdate["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS"].ToString());
                    parms2.Add("PRIOR_AUTH_SAVE_CODE_ID", dataRowForUpdate["PRIOR_AUTH_SAVE_CODE_ID"].ToString());
                    parms2.Add("LINK_SECTIONS", linkId);
                    PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("UPDATEPRIORAUTH_PROFFSERVICEDETAILS", parms2);
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Add Professional Saving Service Details", ex);
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("SaveDentalServiceDetails")]
        public IActionResult SaveDentalServiceDetails(string procedureCode = "", string procCodeDesc = "", string providerServiceNote = "",
          string reqUnits = "", string remainUnits = "", string requnitsfee = "", string fdos = "", string tdos = "", string status = "",
          string toothNum = "", string prosthesisNum = "", string cavity1 = "", string cavity2 = "", string cavity3 = "", string cavity4 = "", string cavity5 = "",
          string surface1 = "", string surface2 = "", string surface3 = "", string surface4 = "", string surface5 = "",
           string MedicaidId = "", string serviceTrackingNo = "", string operation = "", string lineNumber = "", string patype = "", string linkId = "", string userName = "")
        {
            if (operation.ToLower().Equals("add"))
            {
                int toothNumber = Convert.ToInt32(toothNum);
                int prosthesisNumber = Convert.ToInt32(prosthesisNum);

                int authUnits = 0;
                decimal authDollars = 0;

                decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);
                DateTime reqFDOS = Convert.ToDateTime(fdos);
                DateTime reqTDOS = Convert.ToDateTime(tdos);

                var requestUser = Guid.NewGuid();
                int statusId = 1;
                if (status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (status == "Approved")
                {
                    statusId = 2;
                }
                else if (status == "Denied")
                {
                    statusId = 3;
                }


                int remainingUnits = !string.IsNullOrEmpty(remainUnits) ? Convert.ToInt32(remainUnits) : 0;

                var createdBy = HelperFacade.GetUserId(userName);

                Dictionary<string, string> parms3 = new Dictionary<string, string>();
                parms3.Add("PRIOR_AUTH_TYPE", patype);
                parms3.Add("LINK_SECTIONS", linkId);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms3);

                if (ds.Tables[0].Columns.IndexOf("PRIOR_AUTH_TOOTH_NUMBER_CODE") < 0)
                {
                    DataColumn ToothCode = new DataColumn("PRIOR_AUTH_TOOTH_NUMBER_CODE", typeof(string));
                    ds.Tables[0].Columns.Add(ToothCode);
                }

                DataRow dataRowForAdd = ds.Tables[0].NewRow();

                //add new row to addset.. write code for that

                Dictionary<string, object> parms = new Dictionary<string, object>();
                var id = HelperFacade.GetUserId(userName);
                int saveId = PriorAuthHospitalController.GetPriorAuthServiceDetailSaveID("Dental", MedicaidId);
                int lineId = PriorAuthHospitalController.GetPriorAuthServiceDetailLineID("Dental", MedicaidId);
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", procedureCode);

                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", toothNumber);

                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_CODE", PriorAuthFacade.GetToothInfoByToothID(Convert.ToString(toothNumber), false));

                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", cavity1);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", cavity2);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", cavity3);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", cavity4);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", cavity5);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", surface1);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", surface2);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", surface3);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", surface4);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", surface5);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", reqUnits);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                parms.Add("PRIOR_AUTH_STATUS_ID", Convert.ToString(statusId));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", Convert.ToString(procCodeDesc));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", Convert.ToString(providerServiceNote));
                parms.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", prosthesisNum);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", serviceTrackingNo);


                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", Convert.ToString(authDollars));


                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                Random random = new Random();
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", random.Next());
                if (saveId > 0)
                {
                    parms.Add("PRIOR_AUTH_DENTALSAVE_ID", Convert.ToString(saveId));
                }
                else
                {
                    parms.Add("PRIOR_AUTH_DENTALSAVE_ID", Convert.ToString(saveId + 1));
                }

                if (lineId > 0)
                {
                    parms.Add("Line", Convert.ToString(lineId + 1));
                }
                else
                {
                    parms.Add("Line", Convert.ToString(lineId));
                }


                parms.Add("MedicaidID", Convert.ToString(MedicaidId));

                foreach (var key in parms)
                {
                    if (key.Value == null)
                        dataRowForAdd[key.Key] = DBNull.Value;
                    if (key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR" && key.Value == string.Empty)
                    {
                        continue;
                    }
                    if (key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM" && (key.Value == null || key.Value == string.Empty))
                    {
                        continue;
                    }
                    if ((key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS" || key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS") && (key.Value == null || key.Value == string.Empty))
                    {
                        continue;
                    }
                    else
                        dataRowForAdd[key.Key] = key.Value;

                }
                dataRowForAdd["RowState"] = "added";

                ds.Tables[0].Rows.Add(dataRowForAdd);

                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", dataRowForAdd["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                parms2.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", dataRowForAdd["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", dataRowForAdd["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", dataRowForAdd["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", dataRowForAdd["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", dataRowForAdd["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", dataRowForAdd["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", dataRowForAdd["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", dataRowForAdd["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", dataRowForAdd["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", dataRowForAdd["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", dataRowForAdd["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_ID", dataRowForAdd["PRIOR_AUTH_STATUS_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", dataRowForAdd["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                parms2.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", dataRowForAdd["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                parms2.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dataRowForAdd["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                parms2.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms2.Add("LAST_MODIFIED_USER", linkId);
                parms2.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms2.Add("Created_By_User", linkId);
                parms2.Add("PRIOR_AUTH_DENTALSAVE_ID", dataRowForAdd["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                parms2.Add("MedicaidID", dataRowForAdd["MedicaidID"].ToString());
                parms2.Add("Line", dataRowForAdd["Line"].ToString());
                parms2.Add("PRIOR_AUTH_STATUS_TYPE", patype.ToString());
                parms2.Add("LINK_SECTIONS", linkId);
                PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("INSERTPRIORAUTH_DENTALSERVICEDETAILS", parms2);
                return Ok(true);
            }
            else if (operation.ToLower().Equals("update"))
            {

                int toothNumber = Convert.ToInt32(toothNum);
                int prosthesisNumber = Convert.ToInt32(prosthesisNum);


                decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);
                decimal authDollars = 0;
                int authUnits = 0;
                int remainingUnits = !string.IsNullOrEmpty(remainUnits) ? Convert.ToInt32(remainUnits) : 0;

                DateTime reqFDOS;
                if (DateTime.TryParse(fdos, out reqFDOS))
                {
                    reqFDOS = Convert.ToDateTime(fdos);
                }

                DateTime reqTDOS;
                if (DateTime.TryParse(tdos, out reqTDOS))
                {
                    reqTDOS = Convert.ToDateTime(tdos);
                }


                int statusId = 1;
                if (status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (status == "Approved")
                {
                    statusId = 2;
                }
                else if (status == "Denied")
                {
                    statusId = 3;
                }

                var createdBy = HelperFacade.GetUserId(userName);

                Dictionary<string, object> parms = new Dictionary<string, object>();
                var id = HelperFacade.GetUserId(userName);


                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", Convert.ToString(lineNumber));
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", procedureCode);
                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", Convert.ToString(toothNumber));
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", cavity1);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", cavity2);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", cavity3);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", cavity4);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", cavity5);

                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", surface1);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", surface2);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", surface3);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", surface4);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", surface5);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", reqUnits);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", reqFDOS);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", reqTDOS);
                parms.Add("PRIOR_AUTH_STATUS_ID", Convert.ToString(statusId));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", Convert.ToString(procCodeDesc));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", Convert.ToString(providerServiceNote));
                parms.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", Convert.ToString(prosthesisNum));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", Convert.ToString(serviceTrackingNo));

                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));

                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", authDollars != 0 ? Convert.ToString(authDollars) : "");
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                parms.Add("MedicaidID", MedicaidId);

                Dictionary<string, string> parms2 = new Dictionary<string, string>();
                parms2.Add("PRIOR_AUTH_TYPE", patype);
                parms2.Add("LINK_SECTIONS", linkId);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms2);

                string RowFilter = "PRIOR_AUTH_DENTALSERVICE_DETAIL_ID ='" + lineNumber + "'";
                DataRow dataRowForUpdate = ds.Tables[0].Select(RowFilter).FirstOrDefault();

                foreach (var key in parms)
                {
                    if (key.Value == null)
                        dataRowForUpdate[key.Key] = DBNull.Value;
                    if (key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR" && key.Value == string.Empty)
                    {
                        continue;
                    }
                    if ((key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS" || key.Key == "PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS") && (key.Value == null || key.Value == string.Empty))
                    {
                        continue;
                    }
                    else
                        dataRowForUpdate[key.Key] = key.Value;

                }

                if (dataRowForUpdate["RowState"] != null)
                    dataRowForUpdate["RowState"] = "updated";


                Dictionary<string, string> parms3 = new Dictionary<string, string>();
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"].ToString());
                parms3.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", dataRowForUpdate["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString());
                parms3.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", dataRowForUpdate["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString());
                parms3.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", dataRowForUpdate["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", dataRowForUpdate["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", dataRowForUpdate["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", dataRowForUpdate["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", dataRowForUpdate["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", dataRowForUpdate["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", dataRowForUpdate["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", dataRowForUpdate["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", dataRowForUpdate["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", dataRowForUpdate["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString());
                parms3.Add("PRIOR_AUTH_STATUS_ID", dataRowForUpdate["PRIOR_AUTH_STATUS_ID"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", dataRowForUpdate["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString());
                parms3.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", dataRowForUpdate["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString());
                parms3.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", dataRowForUpdate["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString());
                parms3.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms3.Add("LAST_MODIFIED_USER", linkId);
                parms3.Add("PRIOR_AUTH_DENTALSAVE_ID", dataRowForUpdate["PRIOR_AUTH_DENTALSAVE_ID"].ToString());
                parms3.Add("MedicaidID", dataRowForUpdate["MedicaidID"].ToString());
                parms3.Add("Line", dataRowForUpdate["Line"].ToString());
                parms3.Add("PRIOR_AUTH_STATUS_TYPE", patype.ToString());
                parms3.Add("LINK_SECTIONS", linkId);
                PriorAuthHospitalController.InsertUpdatePriorAuthPanelData("UPDATEPRIORAUTH_DENTALSERVICEDETAILS", parms3);
                return Ok(true);
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("GetProfessionalServiceDetail")]
        public IActionResult GetProfessionalServiceDetail(string lineNumber = "", string MedicaidId = "", string patype = "", string linkId = "")
        {
            ServiceDetailsProfessional serviceDetailsProfessional = new ServiceDetailsProfessional();
            try
            {
                DataSet dataSet = LookupTableController.GetRequestedUnitMeasures();
                DataTable dt = dataSet.Tables[0];
                dt.Columns.Add("PRIOR_AUTH_REQUESTED_UNITS", typeof(System.String));

                foreach (DataRow dr in dt.Rows)
                {
                    dr["PRIOR_AUTH_REQUESTED_UNITS"] = dr["PRIOR_AUTH_REQUESTED_UNITS_CODE"] + "-" + dr["PRIOR_AUTH_REQUESTED_UNITS_DESC"];
                }

                DataRow currentRow = null;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", patype);
                parms.Add("LINK_SECTIONS", linkId);
                DataSet ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr != null &&
                        dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != null &&
                        Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"]) == lineNumber)
                    {
                        currentRow = dr;
                        break;
                    }
                }
                serviceDetailsProfessional.Line = Convert.ToInt32(currentRow["Line"]);
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_ID = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"]) : 0;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != DBNull.Value ? Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"]) : 0;
                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString() != "")
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"]);

                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString() != "" && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value ? Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS"]) : 0;
                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString() != "")
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"]);

                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != "" && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value ? Convert.ToDecimal(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]) : 0;

                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString()))
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                }


                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString()) &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString()) &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"].ToString()) &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToShortDateString() != "1/1/1900" &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]) >= Convert.ToDateTime("1/1/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToShortDateString() != "1/1/0001")
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"].ToString()) &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToShortDateString() != "1/1/1900" &&
                    Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]) >= Convert.ToDateTime("1/1/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToShortDateString() != "1/1/0001")
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                serviceDetailsProfessional.PRIOR_AUTH_PROCEDURE_CODE_ID = Convert.ToString(currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"]);

                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != null &&
                    currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"].ToString()) &&
                    Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]) >= 0)
                {
                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value ? Convert.ToInt32(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS"]) : 0;
                }


                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM"]) : string.Empty;

                string x = currentRow["PRIOR_AUTH_STATUS_ID"] != null && currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value ? currentRow["PRIOR_AUTH_STATUS_ID"].ToString() : string.Empty;
                serviceDetailsProfessional.StatusDesc = x;
                int value = 0;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC"]) : string.Empty;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"]) : string.Empty;

                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1 = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"]) : string.Empty;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2 = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"]) : string.Empty;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3 = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"]) : string.Empty;
                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4 = currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"]) : string.Empty; ;



                if (currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != null && currentRow["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != DBNull.Value)
                {
                    string unitID = Convert.ToString(serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID);
                    DataRow dataRow = dt.AsEnumerable().SingleOrDefault(r => r.Field<int>("PRIOR_AUTH_REQUESTED_UNITS_ID") == Convert.ToInt32(unitID));

                    serviceDetailsProfessional.PRIOR_AUTH_REQUESTED_UNITS = dataRow != null && dataRow["PRIOR_AUTH_REQUESTED_UNITS"] != null && dataRow["PRIOR_AUTH_REQUESTED_UNITS"] != DBNull.Value ? Convert.ToString(dataRow["PRIOR_AUTH_REQUESTED_UNITS"]) : string.Empty;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetProfessionalServiceDetail method", ex);
            }
            return Ok(serviceDetailsProfessional);
        }




        [HttpGet]
        [Route("GetProfessionalServiceDetails")]
        public IActionResult GetProfessionalServiceDetails(string MedicaidId = "", string patype = "", string linkId = "")
        {
            List<ServiceDetailsProfessional> serviceDetailsProfessionals = new List<ServiceDetailsProfessional>();
            try
            {
                DataSet ds = null;
                DataTable dataTable = null;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", patype);
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                dataTable = ds.Tables[0];

                if (dataTable != null && dataTable.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dataTable.Columns.Add(dcRowState);
                }

                string ROWSTATE_DELETE = "deleted";
                DataTable dt = null;
                DataSet dataSet = LookupTableController.GetRequestedUnitMeasures();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    dt = dataSet.Tables[0];
                    dt.Columns.Add("PRIOR_AUTH_REQUESTED_UNITS", typeof(System.String));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["PRIOR_AUTH_REQUESTED_UNITS"] = dr["PRIOR_AUTH_REQUESTED_UNITS_CODE"] + "-" + dr["PRIOR_AUTH_REQUESTED_UNITS_DESC"];
                    }
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] dtRows = dataTable.Select("RowState='" + ROWSTATE_DELETE + "'");
                    foreach (DataRow dr in dtRows)
                    {
                        dataTable.Rows.Remove(dr);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            #region 

                            ServiceDetailsProfessional serviceDetailsProfessional = new ServiceDetailsProfessional();
                            try
                            {
                                serviceDetailsProfessional.Line = Convert.ToInt32(dr["Line"]);
                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_ID = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_ID"]) : 0;
                                serviceDetailsProfessional.PRIOR_AUTH_PROCEDURE_CODE_ID = dr["PRIOR_AUTH_PROCEDURE_CODE_ID"] != null && dr["PRIOR_AUTH_PROCEDURE_CODE_ID"] != DBNull.Value ? dr["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString() : string.Empty;

                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1 = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1"]) : string.Empty;
                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2 = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2"]) : string.Empty;
                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3 = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3"]) : string.Empty;
                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4 = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"] != DBNull.Value ? Convert.ToString(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4"]) : string.Empty; ;

                                if (dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != null && !string.IsNullOrEmpty(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString()))
                                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"]);
                                //serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"].ToString() != "" && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS"]) : null;

                                serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"] != DBNull.Value ? Convert.ToInt32(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID"]) : 0;

                                if (dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != null && !string.IsNullOrEmpty(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"].ToString()))
                                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR = Convert.ToDecimal(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"]);
                                //serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR = dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value ? Convert.ToDecimal(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR"]) : 0;

                                if (dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"].ToString()) && Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" &&
                                    Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                                {
                                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS = Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                                }

                                if (dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"].ToString()) && Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" &&
                                    Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                                {
                                    serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS = Convert.ToDateTime(dr["PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                                }

                                serviceDetailsProfessional.PRIOR_AUTH_STATUS_ID = dr["PRIOR_AUTH_STATUS_ID"] != null && dr["PRIOR_AUTH_STATUS_ID"] != DBNull.Value ? dr["PRIOR_AUTH_STATUS_ID"].ToString() : string.Empty;


                                string unitID = Convert.ToString(serviceDetailsProfessional.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    DataRow dataRow = dt.AsEnumerable().SingleOrDefault(r => r.Field<int>("PRIOR_AUTH_REQUESTED_UNITS_ID") == Convert.ToInt32(unitID));

                                    serviceDetailsProfessional.PRIOR_AUTH_REQUESTED_UNITS = dataRow != null && dataRow["PRIOR_AUTH_REQUESTED_UNITS"] != null && dataRow["PRIOR_AUTH_REQUESTED_UNITS"] != DBNull.Value ? Convert.ToString(dataRow["PRIOR_AUTH_REQUESTED_UNITS"]) : string.Empty;
                                }
                            }
                            catch (Exception ex)
                            {
                                serviceDetailsProfessional.isErrorOccured = true;
                                serviceDetailsProfessional.ErrorMessage = ex.Message + ex.StackTrace;
                            }

                            #endregion

                            serviceDetailsProfessionals.Add(serviceDetailsProfessional);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceDetailsProfessional serviceDetailsProfessional = new ServiceDetailsProfessional();
                serviceDetailsProfessional.isErrorOccured = true;
                serviceDetailsProfessional.ErrorMessage = ex.Message + ex.StackTrace;
                serviceDetailsProfessionals.Add(serviceDetailsProfessional);
            }
            return Ok(serviceDetailsProfessionals);
        }

        [HttpGet]
        [Route("GetDentalServiceDetail")]
        public IActionResult GetDentalServiceDetail(string lineNumber = "", string MedicaidId = "", string patype = "", string linkId = "")
        {
            ServiceDetailsDental ServiceDetailsDental = new ServiceDetailsDental();
            try
            {
                DataSet ds;
                DataRow currentRow = null;

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", patype);
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);

                DataTable dataTable = ds.Tables[0];

                bool isToothIdExist = dataTable.Columns.Contains("PRIOR_AUTH_TOOTH_NUMBER_ID");
                bool isToothCodeExist = dataTable.Columns.Contains("PRIOR_AUTH_TOOTH_NUMBER_CODE");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"]) == lineNumber)
                    {
                        currentRow = dr;
                        break;
                    }
                }

                if (currentRow == null)
                {
                    return Ok(ServiceDetailsDental);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"].ToString())
                    && Convert.ToDouble(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"]) > 0)
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR = Convert.ToDecimal(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR"]);
                }

                DateTime dateTimeValidate = DateTime.Now;

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != null && currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != null && currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value
                    && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }


                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"] != null && currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"] != DBNull.Value
                             && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToShortDateString() != "1/1/1900"
                             && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"]) >= Convert.ToDateTime("01/01/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToShortDateString() != "1/1/0001")
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"] != null && currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"] != DBNull.Value
                                       && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToShortDateString() != "1/1/1900"
                                       && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]) >= Convert.ToDateTime("01/01/1900") && Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToShortDateString() != "1/1/0001")
                {
                    dateTimeValidate = Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]);
                    if (dateTimeValidate.Year != 1900 && dateTimeValidate.Year >= 1900)
                    {
                        ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS = Convert.ToDateTime(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }



                if (isToothIdExist && currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != null &&
                    currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"]);
                    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_CODE = PriorAuthFacade.GetToothInfoByToothID(Convert.ToString(ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID), false);

                }
                else if (isToothCodeExist && currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != null &&
                    currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_CODE = Convert.ToString(currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"]);
                    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID = Convert.ToInt32(PriorAuthFacade.GetToothInfoByToothID(Convert.ToString(ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID), true));

                }




                if (currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"] != null &&
                    currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID = Convert.ToInt32(currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"]);
                    if (Convert.ToString(currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"]) == "I")
                    {
                        // ddDentalProsthsis.SelectedValue = "1";
                    }
                    else if (Convert.ToString(currentRow["PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID"]) == "R")
                    {
                        // ddDentalProsthsis.SelectedValue = "2";
                    }
                }

                if (currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"] != null &&
                    currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS = Convert.ToString(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"] != null &&
                    currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS = Convert.ToString(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"] != null &&
                    currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS = Convert.ToString(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"] != null &&
                    currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS = Convert.ToString(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"] != null &&
                    currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS = Convert.ToString(currentRow["PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE"]);
                }

                if (currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"] != null &&
                    currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_PROCEDURE_CODE_ID = Convert.ToString(currentRow["PRIOR_AUTH_PROCEDURE_CODE_ID"]);
                }

                if (currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != null
                    && currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"] != DBNull.Value
                    && !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"].ToString())
                    && Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]) >= 0)
                {
                    ServiceDetailsDental.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS = Convert.ToInt32(currentRow["PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS"]);
                }


                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM = Convert.ToString(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM"]);
                }

                string x = string.Empty;
                if (currentRow["PRIOR_AUTH_STATUS_ID"] != null &&
                    currentRow["PRIOR_AUTH_STATUS_ID"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_STATUS_ID"].ToString()))
                {
                    x = currentRow["PRIOR_AUTH_STATUS_ID"].ToString();

                }

                string sts = string.Empty;
                int outStatus;
                var isNumeric = int.TryParse(x, out outStatus);
                if (isNumeric)
                {
                    switch (x)
                    {
                        case "1":
                            sts = "Submission Pending";
                            break;
                        case "2":
                            sts = "Approved";
                            break;
                        case "3":
                            sts = "Denied";
                            break;
                        default:
                            sts = "Submission Pending";
                            break;
                    }
                    ServiceDetailsDental.StatusDesc = sts;
                    ServiceDetailsDental.PRIOR_AUTH_STATUS_ID = x;
                }
                else
                {
                    ServiceDetailsDental.StatusDesc = x;
                }
                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC = Convert.ToString(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC"]);
                }

                if (currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != null &&
                    currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"] != DBNull.Value &&
                    !string.IsNullOrEmpty(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"].ToString()))
                {
                    ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE = Convert.ToString(currentRow["PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetDentalServiceDetail method", ex);
            }
            return Ok(ServiceDetailsDental);
        }


        [HttpGet]
        [Route("GetDentalServiceDetailsPA")]
        public IActionResult GetDentalServiceDetailsPA(string MedicaidId = "", string patype = "", string linkId = "")
        {
            List<ServiceDetailsDental> serviceDetailsDentals = new List<ServiceDetailsDental>();
            try
            {
                DataSet ds = null;
                DataTable dataTable = null;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", patype);
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_SERVICEDETAILS", parms);
                dataTable = ds.Tables[0];

                if (dataTable.Columns.IndexOf("RowState") < 0)
                {
                    DataColumn dcRowState = new DataColumn("RowState", typeof(string));
                    dataTable.Columns.Add(dcRowState);
                }

                string ROWSTATE_DELETE = "deleted";


                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow[] dtRows = dataTable.Select("RowState='" + ROWSTATE_DELETE + "'");
                    foreach (DataRow dr in dtRows)
                    {
                        dataTable.Rows.Remove(dr);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            ServiceDetailsDental ServiceDetailsDental = new ServiceDetailsDental();
                            ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_ID = Convert.ToInt32(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_ID"]);
                            ServiceDetailsDental.PRIOR_AUTH_PROCEDURE_CODE_ID = Convert.ToString(dr["PRIOR_AUTH_PROCEDURE_CODE_ID"]);
                            if (dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"] != null && !string.IsNullOrEmpty(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"].ToString()))
                                ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS = Convert.ToInt32(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS"]);
                            if (dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"] != null && !String.IsNullOrEmpty(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"].ToString()))
                                ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR = Convert.ToDecimal(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR"]);



                            if (dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != null && dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"].ToString()) &&
                                        Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS = Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);
                            }

                            if (dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != null && dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"].ToString()) &&
                                        Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/1900" && Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToShortDateString() != "1/1/0001")
                            {
                                ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS = Convert.ToDateTime(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]).ToString("M/dd/yyyy", CultureInfo.InvariantCulture);

                            }


                            //if (dr["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != null && dr["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_TOOTH_NUMBER_CODE"].ToString()))
                            //{
                            //    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_CODE = Convert.ToString(dr["PRIOR_AUTH_TOOTH_NUMBER_CODE"]);
                            //    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID = Convert.ToInt32(GetToothInfoByToothID(ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_CODE, true));
                            //}
                            //else 
                            if (dr["PRIOR_AUTH_TOOTH_NUMBER_ID"] != null && dr["PRIOR_AUTH_TOOTH_NUMBER_ID"] != DBNull.Value && !string.IsNullOrEmpty(dr["PRIOR_AUTH_TOOTH_NUMBER_ID"].ToString()))
                            {
                                ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID = Convert.ToInt32(dr["PRIOR_AUTH_TOOTH_NUMBER_ID"]);

                                if (ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID != null && ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID > 0)
                                {
                                    int toothID = ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_ID;
                                    ServiceDetailsDental.PRIOR_AUTH_TOOTH_NUMBER_CODE = PriorAuthFacade.GetToothInfoByToothID(Convert.ToString(toothID), false);
                                }
                            }

                            //ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS = Convert.ToString(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS"]);
                            //ServiceDetailsDental.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS = Convert.ToString(dr["PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS"]);

                            ServiceDetailsDental.PRIOR_AUTH_STATUS_ID = Convert.ToString(dr["PRIOR_AUTH_STATUS_ID"]);
                            ServiceDetailsDental.PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS = Convert.ToString(dr["PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS"]);
                            serviceDetailsDentals.Add(ServiceDetailsDental);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error at GetDentalServiceDetails method", ex);
            }
            return Ok(serviceDetailsDentals);
        }

        [HttpGet]
        [Route("GetPARecipientInformation")]
        public IActionResult GetPARecipientInformation(string medicaidId = "", string Username = "", string medicaidbillingnumber = "", string dateofBirth = "",
            string medicalBillingPrev = "", string dateofBirthPrev = "")
        {
            Models.RecipientInformation recipientInformation = null;
            DataSet ds = RegistrationController.SelectProviderByGRPMedicaidID(medicaidId.TrimAndReduce());
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            if (Helper.HasRows(dtMisc))
            {
                DataRow dr = dtMisc.Rows[0];
                var npi = Helper.GetString("NPI", dr);
                try
                {
                    Guid userId = HelperFacade.GetUserId(Username);
                    Models.RecipientEligibilitySearchResponse recipientInfo = null;
                    //if the data has changed either Billing# or DOB then make the call
                    if (medicalBillingPrev != medicaidbillingnumber || dateofBirthPrev != dateofBirth)
                    {
                        recipientInfo = new EligibilityFacade().SearchRequest(medicaidId, userId, string.Empty, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), string.Empty, medicaidbillingnumber, "PriorAuthEligibility");
                    }
                    else
                    {
                        recipientInfo = new EligibilityFacade().SearchRequest(medicaidId, userId, string.Empty, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"), string.Empty, medicaidbillingnumber, "PriorAuthEligibility");
                    }
                    if (recipientInfo != null)
                    {
                        recipientInformation = new Models.RecipientInformation();
                        recipientInformation.AddressLine1 = recipientInfo.RecipientInfo.AddressLine1;
                        recipientInformation.AddressLine2 = recipientInfo.RecipientInfo.AddressLine2;
                        recipientInformation.FirstName = recipientInfo.RecipientInfo.FirstName;
                        recipientInformation.LastName = recipientInfo.RecipientInfo.LastName;
                        recipientInformation.MiddleName = recipientInfo.RecipientInfo.MiddleName;
                        recipientInformation.City = recipientInfo.RecipientInfo.City;
                        recipientInformation.StateCode = recipientInfo.RecipientInfo.StateCode;
                        recipientInformation.ZipCode5 = recipientInfo.RecipientInfo.ZipCode5;
                        recipientInformation.DateOfBirth = recipientInfo.RecipientInfo.DateOfBirth;
                        recipientInformation.DateOfDeath = recipientInfo.RecipientInfo.DateOfDeath;
                        recipientInformation.Gender = recipientInfo.RecipientInfo.Gender;
                        recipientInformation.ErrorMsg = recipientInfo.RecipientInfo.ErrorMsg;
                        recipientInformation.MedicaidId = recipientInfo.RecipientInfo.MedicaidId;
                        recipientInformation.SSN = recipientInfo.RecipientInfo.SSN;
                        List<Models.ErrorDetail> errors = null;
                        if (recipientInfo.RecipientInfo.Errors != null)
                        {
                            errors = new List<Models.ErrorDetail>();
                            foreach (var error in recipientInfo.RecipientInfo.Errors)
                            {
                                var err = new Models.ErrorDetail();
                                err.Code = error.Code;
                                err.Description = error.Description;
                                errors.Add(err);
                            }
                            recipientInformation.Errors = errors;
                        }
                    }
                }
                catch (Exception ex)
                {
                    recipientInformation = new Models.RecipientInformation();
                    recipientInformation.Errors = new List<Models.ErrorDetail>()
                    {
                        new Models.ErrorDetail()
                        {
                            Code="Exception",
                            Description="Error: An error occurred while processing the request "
                        }
                    };
                }
            }

            return Ok(recipientInformation);
        }

        [HttpGet]
        [Route("GetProcCodeDetails")]
        public IActionResult GetProcCodeDetails(string desc = "", string val = "", bool includeICDCodes = true)
        {
            DataTable Procdt = LookupTableController.GetProcedureCodeServiceDetail(val.TrimAndReduce(), desc.TrimAndReduce(), false, includeICDCodes);
            List<Proccode> list = new List<Proccode>();
            if (Procdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Procdt;
                Proccode ProcCodeDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    ProcCodeDetails = new Proccode(dr["PRIOR_AUTH_PROCEDURE_CODE_MMIS"].ToString(), dr["PRIOR_AUTH_PROCEDURE_CODE_DESC"].ToString());
                    list.Add(ProcCodeDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetPlaceOfServiceCodeDetails")]
        public IActionResult GetPlaceOfServiceCodeDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataTable Procdt = LookupTableController.GetPlaceofServiceDetail(val.TrimAndReduce(), desc.TrimAndReduce());

            List<PlaceOfService> list = new List<PlaceOfService>();
            if (Procdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Procdt;
                PlaceOfService PlaceOfServiceCodeDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    PlaceOfServiceCodeDetails = new PlaceOfService(dr["PRIOR_AUTH_PLACE_OF_SERVICE_MMIS"].ToString(), dr["PRIOR_AUTH_PLACE_OF_SERVICE_DESC"].ToString());
                    list.Add(PlaceOfServiceCodeDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRevenueDetails")]
        public IActionResult GetRevenueDetails(string val = "", string desc = "")
        {
            DataTable dt = LookupTableController.GetRevenueCode(val.TrimEnd(), desc.TrimEnd());
            var data = string.Empty;

            List<revenueDetails> list = new List<revenueDetails>();
            if (dt.Rows.Count > 0)
            {

                revenueDetails Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new revenueDetails(dr["RevenueCode"].ToString(), dr["RevenueDesc"].ToString());
                    list.Add(Vdetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetFacilityTypeData")]
        public IActionResult GetFacilityTypeData(string facilityCode = "", string facilityDesc = "", bool isTextChangesEvent = false)
        {
            List<FacilityType> facilityTypes = new List<FacilityType>();
            DataTable dt = null;
            DataSet ds = null;
            try
            {
                if (isTextChangesEvent)
                    ds = ProviderController.GetFacilityTypeByFacilityCode(facilityCode, facilityDesc);//psc.GetFacilityTypeByFacilityCode(facilityCode, facilityDesc);
                else
                    ds = ProviderController.GetFacilityTypes(facilityCode, facilityDesc);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        facilityTypes.Add(new FacilityType()
                        {
                            FacilityTypeCode = Convert.ToString(dr["PRIOR_AUTH_FACILITY_TYPE_CODE_MMIS"]),
                            FacilityTypeDesc = Convert.ToString(dr["PRIOR_AUTH_FACILITY_TYPE_CODE_DESC"])
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(facilityTypes);
        }

        [HttpGet]
        [Route("GetDiagCodeDetails")]
        public IActionResult GetDiagCodeDetails(string Code = "", string DiagVer = "", string val = "")
        {

            DataTable dt = null;
            var ds = LookupTableController.GetICDDiagnosisCodeSearchResults(Code, DiagVer, val);
            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            List<Diagnosiscodedetails> list = new List<Diagnosiscodedetails>();
            if (dt.Rows.Count > 0)
            {

                Diagnosiscodedetails Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    if (list.Count <= 50)
                    {
                        Vdetails = new Diagnosiscodedetails(dr["ICD10Diag"].ToString(), dr["ICDVersion"].ToString(), dr["DiagDesc"].ToString());
                        list.Add(Vdetails);
                    }
                }

            }
            return Ok(list);
        }


        [HttpGet]
        [Route("GetNPICodeDetails")]
        public IActionResult GetNPICodeDetails(string firstname = "", string lastname = "", string Medicateid = "", string hspsccode = "")
        {
            DataTable dt = null;
            if (string.IsNullOrEmpty(firstname)) firstname = "";
            if (string.IsNullOrEmpty(lastname)) lastname = "";
            if (string.IsNullOrEmpty(Medicateid)) Medicateid = "";
            if (string.IsNullOrEmpty(hspsccode)) hspsccode = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, hspsccode.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, Medicateid.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, firstname.Trim(), true));
            parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lastname.Trim(), true));

            var ds = DataAccess.ExecuteStoredProcedure("usp_Search_NPI", parameters, "Search_NPI");
            if (ds != null)
            {
                dt = ds.Tables[0];
            }
            List<NPIcodedetails> list = new List<NPIcodedetails>();

            if (dt.Rows.Count > 0)
            {
                NPIcodedetails Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new NPIcodedetails(dr["NPI"].ToString(), dr["MEDICAID_ID"].ToString(), dr["FIRST_NAME"].ToString(), dr["LAST_OR_BUSINESS_NAME"].ToString(), dr["ADDRESS1"].ToString(), dr["ADDRESS2"].ToString(), dr["CITY"].ToString(), dr["STATE"].ToString(), dr["ZIP"].ToString());
                    list.Add(Vdetails);
                }
            }

            return Ok(list);
        }

        [HttpGet]
        [Route("GetDestinationPayerID")]
        public IActionResult GetDestinationPayerID(string DestnationPayer = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("DestinationPayerMapID", DestnationPayer);
            var data = string.Empty;
            var ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("DestinationPayerID", parms);
            List<destinationPayerID> list = new List<destinationPayerID>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                destinationPayerID Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new destinationPayerID(dr["DESTINATION_PAYER_DESC"].ToString(), dr["Code"].ToString());
                    list.Add(Vdetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetAdmissionType")]
        public IActionResult GetAdmissionType(string AdmissionType = "")
        {
            DataSet dataSet = LookupTableController.GetClaimAdmitSource();
            DataTable dt = dataSet.Tables[0];
            List<destinationPayerID> list = new List<destinationPayerID>();
            if (AdmissionType == "4")
            {
                var row_EPSDTCondition = from row in dt.AsEnumerable()
                                         where
                                         row.Field<int>("Claims_Admit_Source_ID") == 10 || row.Field<int>("Claims_Admit_Source_ID") == 11
                                         select row;
                DataTable dt_AddmissionSourseFilter = row_EPSDTCondition.CopyToDataTable();
                destinationPayerID Vdetails;
                foreach (DataRow dr in dt_AddmissionSourseFilter.Rows)
                {
                    Vdetails = new destinationPayerID(dr["Claims_Admit_Source_Description"].ToString(), dr["Claims_Admit_Source_ID"].ToString());
                    list.Add(Vdetails);
                }
            }
            else
            {
                var row_EPSDTCondition = from row in dt.AsEnumerable()
                                         where
                                         row.Field<int>("Claims_Admit_Source_ID") != 10 && row.Field<int>("Claims_Admit_Source_ID") != 11
                                         select row;
                DataTable dt_AddmissionSourseFilter = row_EPSDTCondition.CopyToDataTable();
                destinationPayerID Vdetails;
                foreach (DataRow dr in dt_AddmissionSourseFilter.Rows)
                {
                    Vdetails = new destinationPayerID(dr["Claims_Admit_Source_Description"].ToString(), dr["Claims_Admit_Source_ID"].ToString());
                    list.Add(Vdetails);
                }
            }

            return Ok(list);

        }


        [HttpGet]
        [Route("PlaceofServiceTextChanged")]
        public IActionResult PlaceofServiceTextChanged(string txtPlaceofService = "", string ClaimId = "")
        {

            string ErrorMessage = string.Empty;
            if (!string.IsNullOrEmpty(txtPlaceofService))
            {
                var dsOC = LookupTableController.GetPlaceofServiceDetail(txtPlaceofService, null);
                if (Helper.HasRows(dsOC))
                {
                    ErrorMessage = "";
                }
                else
                {
                    ErrorMessage = "*Place of service code is invalid";
                }

            }
            return Ok(ErrorMessage);
        }
    }
}
