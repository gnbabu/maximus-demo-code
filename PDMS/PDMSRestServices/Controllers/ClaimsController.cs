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
using Microsoft.IdentityModel.Tokens;
using System.Collections.Frozen;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        [HttpGet]
        [Route("GetValueCodeDescription")]
        public IActionResult GetValueCodeDescription(string desc = "")

        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("CLAIMS_VALUE_CODE", desc.TrimAndReduce());
            var data = string.Empty;

            var ds = Claims.SelectPanelsData("CLAIMS_VALUE_CODE", parms);
            // return ds.GetXml();
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);

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
        [Route("validateProcedureCodeModifiers")]
        public IActionResult validateProcedureCodeModifiers(string provider_type_id = "", string Procedure_code = "", string M1 = "", string M2 = "", string M3 = "", string M4 = "", string claimsorPA = "", string claimORPAType = "")
        {
            string data = JsonConvert.SerializeObject("false");
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.CreateParameter("provider_type_id", DbType.Int32, string.IsNullOrEmpty(provider_type_id) ? 0 : Convert.ToInt32(provider_type_id), true));
            param.Add(SqlHelper.CreateParameter("Procedure_code", DbType.String, Procedure_code.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("M1", DbType.String, M1.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("M2", DbType.String, M2.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("M3", DbType.String, M3.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("M4", DbType.String, M4.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("claimsorPA", DbType.String, claimsorPA.TrimAndReduce(), true));
            param.Add(SqlHelper.CreateParameter("claimORPAType", DbType.String, claimORPAType.TrimAndReduce(), true));
            DataSet dsToothNumber = MAXIMUS.Core.Libraries.DataAccess.ExecuteStoredProcedure("usp_SelectProcedure_Code_Rules_Config", param, "TOOTH_NUMBER");
            if (Helper.HasRows(dsToothNumber))
            {
                data = JsonConvert.SerializeObject(dsToothNumber.Tables[0].Rows[0]["error_message"].ToString());

            }
            return Ok(data);
        }
        [HttpGet]
        [Route("validateToothNo")]
        public IActionResult validateToothNo(string ToothNumber = "")
        {
            string data = JsonConvert.SerializeObject("false");
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.CreateParameter("TOOTH_NUMBER", DbType.String, ToothNumber.TrimAndReduce(), true));
            DataSet dsToothNumber = MAXIMUS.Core.Libraries.DataAccess.ExecuteStoredProcedure("usp_SelectPrior_auth_submit_claim_tooth_number", param, "TOOTH_NUMBER");
            if (Helper.HasRows(dsToothNumber))
            {
                data = JsonConvert.SerializeObject("Non-individual provider cannot be entered as referring provider");

            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetnpiCount")]
        public IActionResult GetnpiCount(string RefNpi = "", string RefMedId = "", string ClaimType = "")
        {
            var data = JsonConvert.SerializeObject(string.Empty);

            DataTable dt = Claims.GetData(RefNpi, RefMedId, "", "");
            if (Helper.HasRows(dt))
            {
                data = Claims.NPITextChangedTest(RefNpi, RefMedId);

                if (!string.IsNullOrEmpty(RefMedId))
                {
                    DataSet dsProviderInformation = Claims.LoadProviderInformation(RefMedId.ToString());
                    if (Helper.HasRows(dsProviderInformation.Tables[0]))
                    {
                        string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                        if (entityTypeId != "1")
                        {
                            if (ClaimType.ToString() == CON.ClaimsType.Dental || ClaimType.ToString() == CON.ClaimsType.Professional || ClaimType.ToString() == CON.ClaimsType.Institutional)
                            {
                                data = JsonConvert.SerializeObject("Non-individual provider cannot be entered as referring provider");
                            }

                            // ClearReferFields();
                            //return;
                        }
                    }
                }
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetAsstnpiCount")]
        public IActionResult GetAsstnpiCount(string AsstNpi = "", string AsstMedId = "", string ClaimType = "")
        {
            var data = string.Empty;

            DataTable dt = Claims.GetData(AsstNpi, AsstMedId, "", "");
            if (Helper.HasRows(dt))
            {
                data = Claims.NPITextChangedTest(AsstNpi, AsstMedId);

                if (!string.IsNullOrEmpty(AsstMedId))
                {
                    DataSet dsProviderInformation = Claims.LoadProviderInformation(AsstMedId.ToString());
                    string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                    if (entityTypeId != "1")
                    {
                        if (ClaimType.ToString() == CON.ClaimsType.Dental || ClaimType.ToString() == CON.ClaimsType.Professional)
                        {
                            data = JsonConvert.SerializeObject("Non-individual provider cannot be entered as referring provider");
                        }
                        if (ClaimType.ToString() == CON.ClaimsType.Institutional)
                        {
                            data = JsonConvert.SerializeObject("Non - individual provider cannot be entered as other operative physician");
                        }
                    }
                }

            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetnpiCountServiceFacility")]
        public IActionResult GetnpiCountServiceFacility(string ServiceNpi = "", string ServiceMedId = "")
        {

            var data = "0";
            if (!string.IsNullOrWhiteSpace(ServiceNpi.ToString()))
            {
                DataTable dt = Claims.GetData(ServiceNpi, ServiceMedId, "", "");
                if (Helper.HasRows(dt))
                {
                    data = Claims.NPITextChangedTest(ServiceNpi, ServiceMedId);

                    if (!string.IsNullOrEmpty(ServiceMedId))
                    {
                        DataSet dsProviderInformation = Claims.LoadProviderInformation(ServiceMedId.ToString());
                        if (Helper.HasRows(dsProviderInformation.Tables[0]))
                        {
                            string entityTypeId = Helper.GetString("ENTITY_TYPE_ID", dsProviderInformation.Tables[0].Rows[0]).Trim();

                            if (entityTypeId == "1")
                            {

                                data = JsonConvert.SerializeObject("Individual provider cannot be entered as facility provider");

                            }
                        }
                    }
                }
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("DisplayInstitutionalHeaderAdjustmentPanel")]
        public IActionResult DisplayInstitutionalHeaderAdjustmentPanel(string hdnClaimId = "")
        {
            return Ok(Claims.DisplayInstitutionalHeaderAdjustmentPanel(hdnClaimId));
        }

        [HttpGet]
        [Route("GetEdittedFieldsHeaderAdjustmentInstitutional")]
        public IActionResult GetEdittedFieldsHeaderAdjustmentInstitutional(string claim_id = "", string Claims_Header_Other_Payer_Adjustment_Information_ID = "")
        {


            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> listData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();

            var data = string.Empty;
            DataSet bindGrid = Claims.FetchOtherPayerAdjustmentInformationforEditting(claim_id, Claims_Header_Other_Payer_Adjustment_Information_ID);
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_health_plan_id = dr["Health_Plan_ID"].ToString();
                    otherPayerAdjustmentInfo.cde_adjustment_group = dr["cde_adjustment_group"].ToString();
                    otherPayerAdjustmentInfo.cde_reason_code = dr["cde_reason_code"].ToString();
                    otherPayerAdjustmentInfo.cde_amount = dr["cde_amount"].ToString();
                    otherPayerAdjustmentInfo.cde_quantity = dr["cde_quantity"].ToString();
                    otherPayerAdjustmentInfo.Claim_ID = dr["Claim_ID"].ToString();
                    otherPayerAdjustmentInfo.Claims_Header_Other_Payer_Adjustment_Information_ID = dr["Claims_Header_Other_Payer_Adjustment_Information_ID"].ToString();

                    listData.Add(otherPayerAdjustmentInfo);
                }
            }
            return Ok(listData);
        }

        [HttpGet]
        [Route("GetPayerAdjustmentInfoInstitutional")]
        public IActionResult GetPayerAdjustmentInfoInstitutional()
        {

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> listData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            DataSet dataSet = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();
            DataTable dt = dataSet.Tables[0];
            //DataTable dtable = dt.AsEnumerable().Where(r => r.Field<string>("PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC") != AdjustmentGroup).CopyToDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_adjustment_group = dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString();
                    listData.Add(otherPayerAdjustmentInfo);
                }
            }
            return Ok(listData);
        }

        [HttpPost]
        [Route("AddHeaderAdjustmentOtherPayerInfo_Institutional")]
        public IActionResult AddHeaderAdjustmentOtherPayerInfo_Institutional(string hdnClaimId = "", string lblErrorMessageHeaderOtherAdjustment = "", string ddlOtherPayerHealthPlanID = "", string ddlOtherPayerAdjustmentGroup = "", string txtOtherPayerReasonCode = "", string txtOtherPayerAmount = "", string txtOtherPayerQuantity = "", string userid = "")
        {
            lblErrorMessageHeaderOtherAdjustment = string.Empty;

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> listData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();

            if (!string.IsNullOrEmpty(hdnClaimId))
            {
                DataSet Otherpayer = new DataSet();
                Dictionary<string, string> parm = new Dictionary<string, string>();
                parm.Add("Claim_ID", hdnClaimId.ToString());
                Otherpayer = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parm);

                int samepayer = 0;
                for (int i = 0; i < Otherpayer.Tables[0].Rows.Count; i++)
                {
                    String OtherPayerReasonCode = Otherpayer.Tables[0].Rows[i]["cde_reason_code"].ToString();
                    String drpdownHelthplanId = Otherpayer.Tables[0].Rows[i]["Health_Plan_ID"].ToString();
                    String drpdownAdjustment_Group = Otherpayer.Tables[0].Rows[i]["cde_adjustment_group"].ToString();
                    if (!string.IsNullOrEmpty(txtOtherPayerReasonCode))
                    {
                        if (txtOtherPayerReasonCode == OtherPayerReasonCode && drpdownAdjustment_Group == ddlOtherPayerAdjustmentGroup
                            && drpdownHelthplanId == ddlOtherPayerHealthPlanID)
                        {
                            lblErrorMessageHeaderOtherAdjustment = "Same reason code cannot be reported multiple times with same adjustment group for the same payer. ";
                        }
                    }
                }
                DataTable dtrc = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Trim(), null);
                if (Helper.HasRows(dtrc))
                {
                    if (dtrc.Rows.Count >= 1)
                    {
                        txtOtherPayerReasonCode = txtOtherPayerReasonCode.ToUpper();
                    }
                    else
                    {
                        lblErrorMessageHeaderOtherAdjustment = "Invalid Reason Code";

                    }
                }

                if (!string.IsNullOrEmpty(lblErrorMessageHeaderOtherAdjustment))
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_health_plan_id = null;
                    otherPayerAdjustmentInfo.cde_adjustment_group = null;
                    otherPayerAdjustmentInfo.cde_reason_code = null;
                    otherPayerAdjustmentInfo.cde_amount = null;
                    otherPayerAdjustmentInfo.cde_quantity = null;
                    otherPayerAdjustmentInfo.Claim_ID = null;
                    otherPayerAdjustmentInfo.lblErrorMessageHeaderOtherAdjustment = lblErrorMessageHeaderOtherAdjustment;

                    listData.Add(otherPayerAdjustmentInfo);
                    return Ok(listData);
                }
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", hdnClaimId);
                if (!string.IsNullOrEmpty(ddlOtherPayerHealthPlanID))
                {
                    parms.Add("Health_Plan_ID", ddlOtherPayerHealthPlanID);
                }
                parms.Add("Adjustment_Group", ddlOtherPayerAdjustmentGroup);
                parms.Add("Reason_Code", string.IsNullOrEmpty(txtOtherPayerReasonCode) ? null : txtOtherPayerReasonCode);
                parms.Add("Amount", string.IsNullOrEmpty(txtOtherPayerAmount) ? null : txtOtherPayerAmount);
                parms.Add("Quantity", string.IsNullOrEmpty(txtOtherPayerQuantity) ? null : txtOtherPayerQuantity);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                try
                {
                    MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parms);
                }
                catch (Exception ex) { }
            }
            var data = string.Empty;
            DataSet bindGrid = Claims.FetchOtherPayerAdjustmentInformation(hdnClaimId);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_health_plan_id = dr["Health_Plan_ID"].ToString();
                    otherPayerAdjustmentInfo.cde_adjustment_group = dr["cde_adjustment_group"].ToString();
                    otherPayerAdjustmentInfo.cde_reason_code = dr["cde_reason_code"].ToString();
                    otherPayerAdjustmentInfo.cde_amount = dr["cde_amount"].ToString();
                    otherPayerAdjustmentInfo.cde_quantity = dr["cde_quantity"].ToString();
                    otherPayerAdjustmentInfo.Claim_ID = dr["Claim_ID"].ToString();
                    otherPayerAdjustmentInfo.Claims_Header_Other_Payer_Adjustment_Information_ID = dr["Claims_Header_Other_Payer_Adjustment_Information_ID"].ToString();

                    listData.Add(otherPayerAdjustmentInfo);
                }
            }

            return Ok(listData);
        }

        [HttpPut]
        [Route("UpdateHeaderAdjustmentOtherPayerInfo_Institutional")]
        public IActionResult UpdateHeaderAdjustmentOtherPayerInfo_Institutional(string hdnClaimId = "", string lblErrorMessageHeaderOtherAdjustment = "", string Claims_Header_Other_Payer_Adjustment_Information_ID = "",
            string ddlOtherPayerHealthPlanID = "", string ddlOtherPayerAdjustmentGroup = "", string txtOtherPayerReasonCode = "", string txtOtherPayerAmount = "", string txtOtherPayerQuantity = "", string userid = "")
        {
            lblErrorMessageHeaderOtherAdjustment = string.Empty;

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> listData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            DataSet Otherpayer = new DataSet();
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Claim_ID", hdnClaimId.ToString());
            Otherpayer = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parm);

            for (int i = 0; i < Otherpayer.Tables[0].Rows.Count; i++)
            {
                string ClaimsHeaderOtherPayerAdjustmentInformationID = Otherpayer.Tables[0].Rows[i]["Claims_Header_Other_Payer_Adjustment_Information_ID"].ToString();
                string OtherPayerReasonCode = Otherpayer.Tables[0].Rows[i]["cde_reason_code"].ToString();
                string drpdownHelthplanId = Otherpayer.Tables[0].Rows[i]["Health_Plan_ID"].ToString();
                string drpdownAdjustment_Group = Otherpayer.Tables[0].Rows[i]["cde_adjustment_group"].ToString();
                if (!string.IsNullOrEmpty(txtOtherPayerReasonCode))
                {
                    if (txtOtherPayerReasonCode == OtherPayerReasonCode && drpdownAdjustment_Group == ddlOtherPayerAdjustmentGroup
                        && drpdownHelthplanId == ddlOtherPayerHealthPlanID && ClaimsHeaderOtherPayerAdjustmentInformationID != Claims_Header_Other_Payer_Adjustment_Information_ID && Otherpayer.Tables[0].Rows.Count > 1)
                    {
                        lblErrorMessageHeaderOtherAdjustment = "Same reason code cannot be reported multiple times with same adjustment group for the same payer. ";
                    }
                }
            }
            DataTable dtrc = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Trim(), null);
            if (Helper.HasRows(dtrc))
            {
                if (dtrc.Rows.Count >= 1)
                {
                    txtOtherPayerReasonCode = txtOtherPayerReasonCode.ToUpper();
                }
                else
                {
                    lblErrorMessageHeaderOtherAdjustment = "Invalid Reason Code";
                }
            }

            if (!string.IsNullOrEmpty(lblErrorMessageHeaderOtherAdjustment))
            {
                PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                otherPayerAdjustmentInfo.cde_health_plan_id = null;
                otherPayerAdjustmentInfo.cde_adjustment_group = null;
                otherPayerAdjustmentInfo.cde_reason_code = null;
                otherPayerAdjustmentInfo.cde_amount = null;
                otherPayerAdjustmentInfo.cde_quantity = null;
                otherPayerAdjustmentInfo.Claim_ID = null;
                otherPayerAdjustmentInfo.lblErrorMessageHeaderOtherAdjustment = lblErrorMessageHeaderOtherAdjustment;

                listData.Add(otherPayerAdjustmentInfo);
                return Ok(listData);
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimId);
            if (!string.IsNullOrEmpty(ddlOtherPayerHealthPlanID))
            {
                parms.Add("Health_Plan_ID", ddlOtherPayerHealthPlanID);
            }
            parms.Add("Adjustment_Group", ddlOtherPayerAdjustmentGroup);
            parms.Add("Reason_Code", string.IsNullOrEmpty(txtOtherPayerReasonCode) ? null : txtOtherPayerReasonCode);
            parms.Add("Amount", string.IsNullOrEmpty(txtOtherPayerAmount) ? null : txtOtherPayerAmount);
            parms.Add("Quantity", string.IsNullOrEmpty(txtOtherPayerQuantity) ? null : txtOtherPayerQuantity);
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Claims_Header_Other_Payer_Adjustment_Information_ID", Claims_Header_Other_Payer_Adjustment_Information_ID);
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("claims_header_other_payer_adjustment_information", parms);
            }
            catch { }
            parms.Clear();

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> details = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            details = Claims.DisplayInstitutionalHeaderAdjustmentPanel(hdnClaimId);

            return Ok(details);

        }

        [HttpDelete]
        [Route("DeleteHeaderAdjustmentOtherPayerInfo_Institutional")]
        public IActionResult DeleteHeaderAdjustmentOtherPayerInfo_Institutional(string Claim_ID = "", string Claims_Header_Other_Payer_Adjustment_Information_ID = "")
        {
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> OtherPayerAdjustmentInfo = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("claims_header_other_payer_adjustment_information", "Claims_Header_Other_Payer_Adjustment_Information_ID", Int32.Parse(Claims_Header_Other_Payer_Adjustment_Information_ID));
            }
            catch { }

            var data = string.Empty;
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> details = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            details = Claims.DisplayInstitutionalHeaderAdjustmentPanel(Claim_ID);
            OtherPayerAdjustmentInfo = details;
            return Ok(OtherPayerAdjustmentInfo);
        }

        [HttpGet]
        [Route("AddDiagnosisCode")]
        public IActionResult AddDiagnosisCode(string hdnClaimIdDiag = "", string hdnClaimType_Diag = "", string sequence = "", string DiagCode = "", string ICDVer = "", string presentOnAdmission = "", string DiagDesc = "", string User = "")
        {
            List<DiagnosisPanel> diagnosispanelData = new List<DiagnosisPanel>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimIdDiag);
            parms.Add("Claim_Type", hdnClaimType_Diag);
            DataSet dsDiagnosis = Claims.SelectPanelsData("Claims_Diagnosis_Information", parms);

            bool result = false;
            if (hdnClaimType_Diag == "1")
            {
                DataTable dtDiagnosis = dsDiagnosis.Tables[0];
                IEnumerable<DataRow> dtDiagnosisDetails = from row in dtDiagnosis.AsEnumerable()
                                                          where row.Field<int>("Claims_Sequence_Description_ID") == Convert.ToInt32(sequence)
                                                          select row;
                if (dtDiagnosisDetails.Any())
                {
                    if ((sequence == CON.SequencesCode.Other) && (dtDiagnosisDetails.Count() >= 24))
                    {
                        DiagnosisPanel dp = new DiagnosisPanel();
                        dp.lblErrorMessageDiagnosis = "Other repeats max 24 times.";
                        diagnosispanelData.Add(dp);
                        result = true;
                    }
                    if ((sequence == CON.SequencesCode.Principal) && (dtDiagnosisDetails.Count() >= 1))
                    {
                        DiagnosisPanel dp = new DiagnosisPanel();
                        dp.lblErrorMessageDiagnosis = "Principal can't repeat.";
                        diagnosispanelData.Add(dp);
                        result = true;
                    }
                    if ((sequence == CON.SequencesCode.Admitting) && (dtDiagnosisDetails.Count() >= 1))
                    {
                        DiagnosisPanel dp = new DiagnosisPanel();
                        dp.lblErrorMessageDiagnosis = "Admitting repeats max 1 time.";
                        diagnosispanelData.Add(dp);
                        result = true;
                    }
                    if ((sequence == CON.SequencesCode.PatientReasonforVisit) && (dtDiagnosisDetails.Count() >= 3))
                    {
                        DiagnosisPanel dp = new DiagnosisPanel();
                        dp.lblErrorMessageDiagnosis = "Patient Reason for Visit repeats max 3 times.";
                        diagnosispanelData.Add(dp);
                        result = true;
                    }
                    if ((sequence == CON.SequencesCode.ExternalCauseofInjury) && (dtDiagnosisDetails.Count() >= 12))
                    {
                        DiagnosisPanel dp = new DiagnosisPanel();
                        dp.lblErrorMessageDiagnosis = "External Cause of Injury repeats max 12 times.";
                        diagnosispanelData.Add(dp);
                        result = true;
                    }
                }
            }
            parms.Clear();
            if (!result)
            {
                if (!string.IsNullOrEmpty(DiagCode) && !string.IsNullOrEmpty(hdnClaimIdDiag))
                {
                    int rows = 0;
                    string line = "";
                    if (Helper.HasRows(dsDiagnosis))
                    {
                        rows = dsDiagnosis.Tables[0].Rows.Count;
                    }

                    line = (rows + 1).ToString();
                    parms.Add("Sequence", line);
                    parms.Add("Diagnosis_Code", DiagCode);
                    parms.Add("ICD_Version", ICDVer);
                    parms.Add("Last_Modified_User", HelperFacade.GetUserId(User).ToString());
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(User).ToString());
                    parms.Add("Claim_ID", hdnClaimIdDiag);

                    if (hdnClaimType_Diag == CON.ClaimsType.Institutional)
                    {
                        parms.Add("Sequence_Description_ID", sequence);
                        if (!String.IsNullOrEmpty(presentOnAdmission))
                        {
                            parms.Add("Present_On_Admission", presentOnAdmission.Trim());
                        }
                        else
                            parms.Add("Present_On_Admission", string.Empty);
                    }
                    else
                    {
                        parms.Add("Sequence_Description_ID", null);
                        parms.Add("Present_On_Admission", null);
                    }

                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Diagnosis_Information", parms);
                    }
                    catch { }
                }

                var data = string.Empty;
                DataSet bindGrid = Claims.GetDiagnosisData(hdnClaimIdDiag, hdnClaimType_Diag);

                DataTable dt = new DataTable();
                dt = bindGrid.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DiagnosisPanel DiagPaneldetails = new DiagnosisPanel();
                        DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                        DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                        DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                        string PresentOnAdmission = "";
                        if (dr["Present_On_Admission"].ToString().Equals("W"))
                        {
                            PresentOnAdmission = "Not Applicable";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("N"))
                        {
                            PresentOnAdmission = "No";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                        {
                            PresentOnAdmission = "Yes";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("U"))
                        {
                            PresentOnAdmission = "Unknown";
                        }
                        else
                        {
                            PresentOnAdmission = dr["Present_On_Admission"].ToString();
                        }
                        DiagPaneldetails.PreasentOnAdmission = PresentOnAdmission;
                        DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToString();
                        DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                        DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                        // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                        diagnosispanelData.Add(DiagPaneldetails);
                    }
                }
            }
            return Ok(diagnosispanelData);
        }

        [HttpGet]
        [Route("AddProviderNotes")]
        public IActionResult AddProviderNotes(string Claimtype = "", string claimId = "", string notes = "", string referenceCode = "", string User = "")
        {
            int max = 0;
            Dictionary<string, string> parm = new Dictionary<string, string>();
            DataSet dsProvNotes = new DataSet();
            parm.Add("Claim_ID", claimId);
            parm.Add("Claim_Type", Claimtype);
            dsProvNotes = Claims.SelectPanelsData("Claims_Providers_Note", parm);
            if (Helper.HasRows(dsProvNotes))
            {
                max = dsProvNotes.Tables[0].Rows.Count;
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (Claimtype == CON.ClaimsType.Dental)  //Dental
            {
                parms.Add("line", (max + 1).ToString("D2"));
                parms.Add("Note_ID", null);
                parms.Add("Claim_Type", Claimtype);
            }
            else if (Claimtype == CON.ClaimsType.Institutional) //Institutional.
            {
                parms.Add("line", (max + 1).ToString("D2"));
                parms.Add("Note_ID", referenceCode);
                parms.Add("Claim_Type", Claimtype);
            }

            Claims.InsertProviderNote(parms, claimId, notes, User);

            var data = string.Empty;
            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);
        }

        [HttpGet]
        [Route("AddProviderBillingNotes")]
        public IActionResult AddProviderBillingNotes(string Claimtype = "", string claimId = "", string notes = "", string referenceCode = "", string User = "")
        {
            int max = 0;
            Dictionary<string, string> parm = new Dictionary<string, string>();
            DataSet dsProvNotes = new DataSet();
            parm.Add("Claim_ID", claimId);
            parm.Add("Claim_Type", Claimtype);
            parm.Add("IS_BILLING_NOTE", "1");
            dsProvNotes = Claims.SelectPanelsData("Claims_Providers_Note", parm);
            if (Helper.HasRows(dsProvNotes))
            {
                max = dsProvNotes.Tables[0].Rows.Count;
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("line", (max + 1).ToString("D2"));
            parms.Add("Note_ID", referenceCode);
            parms.Add("Claim_Type", Claimtype);

            Claims.InsertProviderNote(parms, claimId, notes, User, 1);

            var data = string.Empty;
            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype, 1);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);

        }

        [HttpGet]
        [Route("AddAmbulanceInfo")]
        public IActionResult AddAmbulanceInfo(string claimid = "", string serviceLine = "", string pickUpAddressLine1 = "", string pickUpAddressLine2 = "",
            string pickupcity = "", string pickUPState = "", string pickUPZip = "", string dropOffLocationName = "", string dropOffAddressLine1 = "", string dropOffAddressLine2 = "",
            string dropOffCity = "", string dropOffState = "", string dropOffZip = "", string User = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Service_Line", serviceLine);
            parms.Add("Pick_Up_Address_Line1", pickUpAddressLine1);
            parms.Add("Pick_Up_Address_Line2", pickUpAddressLine2);
            parms.Add("Pick_Up_City", pickupcity);
            parms.Add("Pick_Up_State", pickUPState);
            parms.Add("Pick_Up_ZIP", pickUPZip);
            parms.Add("Drop_Off_Location_Name", dropOffLocationName);
            parms.Add("Drop_Off_Address_Line1", dropOffAddressLine1);
            parms.Add("Drop_Off_Address_Line2", dropOffAddressLine2);
            parms.Add("Drop_Off_City", dropOffCity);
            parms.Add("Drop_Off_State", dropOffState);
            parms.Add("Drop_Off_Zip", dropOffZip);
            parms.Add("Last_Modified_date", DateTime.Now.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(User).ToString().Trim());
            parms.Add("Claim_ID", claimid);
            parms.Add("Created_By_User", HelperFacade.GetUserId(User).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());

            List<AmbulanceDropOffPanel> AmbulanceDropOff = new List<AmbulanceDropOffPanel>();
            bool result = false;
            if (!string.IsNullOrEmpty(claimid))
            {
                try
                {
                    if (!string.IsNullOrEmpty(claimid))
                    {
                        AmbulanceDropOffPanel Ambulance = new AmbulanceDropOffPanel();
                        DataSet dsGetData = Claims.GetAmbulanceDropOff(claimid);
                        DataTable dtambulance = dsGetData.Tables[0];

                        if (dtambulance.AsEnumerable().Any(p => p.Field<int>("Service_Line") == Convert.ToInt32(serviceLine)))
                        {
                            Ambulance.Error_Message = "Same Service Line number cannot be added again.";
                            result = true;
                        }
                        if (result)
                        {
                            AmbulanceDropOff.Add(Ambulance);
                        }
                        if (!result)
                        {
                            MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("claims_ambulance_pick_up_drop_off_location", parms);
                        }
                    }
                }
                catch
                {
                }
            }

            var data = string.Empty;
            DataSet bindGrid = Claims.GetAmbulanceDropOff(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            if (!result)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AmbulanceDropOffPanel AmbulanceDropOffInd = new AmbulanceDropOffPanel();
                        AmbulanceDropOffInd.serviceLine = dr["Service_Line"].ToString();
                        AmbulanceDropOffInd.pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                        AmbulanceDropOffInd.pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                        AmbulanceDropOffInd.pickUpCity = dr["Pick_Up_City"].ToString();
                        AmbulanceDropOffInd.pickUpState = dr["Pick_Up_State"].ToString();
                        AmbulanceDropOffInd.pickUpZip = dr["Pick_Up_ZIP"].ToString();
                        AmbulanceDropOffInd.dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                        AmbulanceDropOffInd.dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                        AmbulanceDropOffInd.dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                        AmbulanceDropOffInd.dropOffLocationCity = dr["Drop_Off_City"].ToString();
                        AmbulanceDropOffInd.dropOffLocationState = dr["Drop_Off_State"].ToString();
                        AmbulanceDropOffInd.dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                        AmbulanceDropOffInd.Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                        AmbulanceDropOffInd.Claim_ID = dr["Claim_ID"].ToString();
                        // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                        AmbulanceDropOff.Add(AmbulanceDropOffInd);
                    }
                }
            }
            return Ok(AmbulanceDropOff);

        }


        [HttpGet]
        [Route("GetClaimProfessionalServiceDetail")]
        public IActionResult GetClaimProfessionalServiceDetail(string claimid = "")
        {
            List<PDMSRestServices.Models.ProfessionalServiceDetail> ProfessionalServiceData = new List<PDMSRestServices.Models.ProfessionalServiceDetail>();
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, false));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            DataTable dt = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.ProfessionalServiceDetail professionalServiceDetail = new PDMSRestServices.Models.ProfessionalServiceDetail();
                    professionalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    professionalServiceDetail.plc_service = dr["plc_service"].ToString();
                    professionalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    professionalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    professionalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    professionalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    professionalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    professionalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    professionalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    professionalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    professionalServiceDetail.service_line = dr["Service_Line"].ToString();
                    ProfessionalServiceData.Add(professionalServiceDetail);
                }
            }
            return Ok(ProfessionalServiceData);

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
                                         row.Field<int>("Claims_Admit_Source_ID") != 10 && row.Field<int>("Claims_Admit_Source_ID") != 11 &&
                                         row.Field<int>("Claims_Admit_Source_ID") != 3 && row.Field<int>("Claims_Admit_Source_ID") != 7
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
        [Route("GetPlaceOfServiceCodeDetails")]
        public IActionResult GetPlaceOfServiceCodeDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataTable Procdt = LookupTableController.GetPlaceofServiceDetail(val, desc);

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
        [Route("GetInstitutionalServiceDetails")]
        public IActionResult GetInstitutionalServiceDetails(string claimid = "")
        {
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
            DataTable dt = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail InstiServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();

                    InstiServiceDetail.revenue_Code = dr["Revenue_Code"].ToString();
                    InstiServiceDetail.service_Line = dr["Service_Line"].ToString();
                    InstiServiceDetail.cde_proc = dr["Procedure_Code"].ToString();
                    InstiServiceDetail.procedure_type = dr["Procedure_Type"].ToString();
                    InstiServiceDetail.unit = dr["Billed_Units"].ToString();
                    InstiServiceDetail.unit_of_measurement = dr["Unit_Of_Measurement"].ToString();
                    InstiServiceDetail.fromDOS = Convert.ToDateTime(dr["Date_of_Service"].ToString());
                    if (!string.IsNullOrEmpty(dr["To_Date_Of_Service"].ToString()))
                    {
                        InstiServiceDetail.toDOS = Convert.ToDateTime(dr["To_Date_Of_Service"].ToString());
                    }
                    InstiServiceDetail.total_charges = dr["Total_Charges"].ToString();
                    InstiServiceDetail.paid_amount = dr["Paid_Amount"].ToString();
                    InstiServiceDetail.status = dr["Status"].ToString();

                    InstiServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    InstiServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    InstiServiceDetail.total_amount_billed = dr["Total_Amount_Billed"].ToString();
                    InstiServiceDetail.total_amount_paid = dr["Total_Amount_Paid"].ToString();
                    InstServiceData.Add(InstiServiceDetail);
                }
            }


            return Ok(InstServiceData);
        }

        [HttpGet]
        [Route("CheckServiceLinePriorAuthNumberExists")]
        public IActionResult CheckServiceLinePriorAuthNumberExists(string claimid = "", string claimType ="0")
        {
            bool ValidatePriorAuthNum = false;
            DataSet dsServiceDetailsInfo = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.Int32, claimType, true));
            dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_DataForClaims", parameters, "Claims_Service_Details");
            
            if(dsServiceDetailsInfo.Tables.Count>0)
            {
                var result = dsServiceDetailsInfo.Tables[0]
                                .AsEnumerable()
                                .Where(myRow => !string.IsNullOrWhiteSpace(myRow.Field<string>("prior_auth")) ).Any();

                ValidatePriorAuthNum = result;
            }
            
            return Ok(ValidatePriorAuthNum);
        }


        [HttpPost]
        [Route("AddInstitutionalServiceDetails")]
        public IActionResult AddInstitutionalServiceDetails(string claimid = "", string InsTotalAmountBilled = "", string InsTotalAmountPaid = "",
            string InsFinalEAPG = "", string InsPaymentAction = "", string paidAmt = "", string revenueCode = "", string procedureType = "", string procedureCode = "",
            string modifierDentalFirst = "", string modifierDentalSecond = "", string modifierDentalThird = "", string modifierDentalFourth = "",
            string lineControlNumber = "", string fromDOB = "", string toDOB = "", string nonCoveredCharges = "", string status = "", string Unit = "",
            string UnitOfMeasurement = "", string totalCharges = "", string user = "")
        {
            if (!string.IsNullOrEmpty(paidAmt) && paidAmt.ToLower() == "undefined")
            {
                paidAmt = "";
            }
            if (!string.IsNullOrEmpty(InsFinalEAPG) && InsFinalEAPG.ToLower() == "undefined")
            {
                InsFinalEAPG = "";
            }
            if (!string.IsNullOrEmpty(InsPaymentAction) && InsPaymentAction.ToLower() == "undefined")
            {
                InsPaymentAction = "";
            }
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataSet InstitutionalData = Claims.GetDataInstiServiceDetail(int.Parse(claimid));
            DataTable dtCurrentTable = InstitutionalData != null ? InstitutionalData.Tables[0] : null;
            int serline = Claims.GetInstiServiceDetailRows(claimid);
            serline++;
            if (InstitutionalData != null)
            {
                DataRow drCurrentRow = dtCurrentTable.NewRow();

                drCurrentRow["Revenue_code"] = revenueCode;
                drCurrentRow["Procedure_Type"] = procedureType;
                drCurrentRow["Procedure_code"] = procedureCode.ToUpper();
                drCurrentRow["Billed_Units"] = Unit;
                drCurrentRow["Unit_Of_Measurement"] = UnitOfMeasurement;
                drCurrentRow["Date_of_Service"] = fromDOB;

                if (!string.IsNullOrEmpty(toDOB))
                {
                    drCurrentRow["To_Date_of_Service"] = toDOB;
                }

                if (!string.IsNullOrEmpty(paidAmt))
                {
                    drCurrentRow["Paid_Amount"] = paidAmt;
                }

                if (!string.IsNullOrEmpty(totalCharges))
                {
                    drCurrentRow["Total_Charges"] = totalCharges;
                }

                drCurrentRow["Status"] = status;

                dtCurrentTable.Rows.Add(drCurrentRow);

                DataSet InstiData = new DataSet();

                DataTable UpdateddtCopy = dtCurrentTable.Copy();

                InstiData.Tables.Add(UpdateddtCopy);

                InstitutionalData = InstiData;
                string lblDetailsItemDental = serline.ToString();

                Dictionary<string, string> parms = new Dictionary<string, string>
            {
                { "Service_Line", lblDetailsItemDental },
                { "Procedure_Code", procedureCode.ToUpper() },
                { "Status", status },
                { "Date_of_Service", fromDOB },
                { "To_Date_of_Service", toDOB },
                { "Modifier1", modifierDentalFirst.ToUpper() },
                { "Modifier2", modifierDentalSecond.ToUpper()  },
                { "Modifier3", modifierDentalThird.ToUpper() },
                { "Modifier4",  modifierDentalFourth.ToUpper() },
                { "Total_Charges", totalCharges },
                { "Line_Control_Number", lineControlNumber  },
                { "Billed_Units", Unit },
                { "Revenue_Code", revenueCode },
                { "Claim_ID", Convert.ToString(claimid) },
                { "Procedure_Type", procedureType },
                { "Final_EAPG", InsFinalEAPG },
                { "Unit_of_Measurement", UnitOfMeasurement },
                { "Payment_Action", InsPaymentAction },
                { "Non_Covered_Charges", !string.IsNullOrEmpty(nonCoveredCharges) ? nonCoveredCharges : "0" },
                { "Paid_Amount", !string.IsNullOrEmpty(paidAmt) ? paidAmt : "0"},
                { "Total_Amount_Paid", !string.IsNullOrEmpty(InsTotalAmountPaid) ? InsTotalAmountPaid : "0"},
                { "Total_Amount_Billed", !string.IsNullOrEmpty(InsTotalAmountBilled) ? InsTotalAmountBilled : "0"},
                { "Created_Date_Time", DateTime.Now.ToString() },
                { "Claim_Type", "1" },

            };
                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Service_Details", parms);
            }
            return Ok(true);
        }


        [HttpPost]
        [Route("AddProfessionalServiceDetails")]
        public IActionResult AddProfessionalServiceDetails(string claimid = "", string ProcedureCode = "", string DateOfService = "", string DMECertType = "", string DMEDuration = "",
            string ProfPlaceOfService = "", string Modifier1 = "", string Modifier2 = "", string Modifier3 = "", string Modifier4 = "", string DiagnosisPointer1 = "", string DiagnosisPointer2 = "",
            string DiagnosisPointer3 = "", string DiagnosisPointer4 = "", string Charges = "", string UnitsOfMeasurement = "", string BilledUnits = "", string lblDetailsItemProf = "",
            string paidunits = "", string paidAmount = "", string status = "", string PriorAuth = "", string Refralnumber = "", string Linenumber = "", string Referred_EPSDT_Service = "",
            string Family_Planning = "", string Emergency = "", string Final_EAPG = "", string Payment_Action = "", string Paid_Amount = "", string Total_Charges = "", string Cert_Revision = "", string user = "")
        {
            if (ProcedureCode.ToLower() == "undefined")
            {
                ProcedureCode = "";
            }
            if (DateOfService.ToLower() == "undefined")
            {
                DateOfService = "";
            }
            if (!string.IsNullOrEmpty(DMECertType) && DMECertType.ToLower() == "undefined")
            {
                DMECertType = "";
            }
            if (!string.IsNullOrEmpty(DMEDuration) && DMEDuration.ToLower() == "undefined")
            {
                DMEDuration = "";
            }
            if (!string.IsNullOrEmpty(ProfPlaceOfService) && ProfPlaceOfService.ToLower() == "undefined")
            {
                ProfPlaceOfService = "";
            }
            if (!string.IsNullOrEmpty(Charges) && Charges.ToLower() == "undefined")
            {
                Charges = "";
            }
            if (!string.IsNullOrEmpty(UnitsOfMeasurement) && UnitsOfMeasurement.ToLower() == "undefined")
            {
                UnitsOfMeasurement = "";
            }
            if (!string.IsNullOrEmpty(BilledUnits) && BilledUnits.ToLower() == "undefined")
            {
                BilledUnits = "";
            }
            if (!string.IsNullOrEmpty(lblDetailsItemProf) && lblDetailsItemProf.ToLower() == "undefined")
            {
                lblDetailsItemProf = "";
            }
            if (!string.IsNullOrEmpty(paidunits) && paidunits.ToLower() == "undefined")
            {
                paidunits = "";
            }
            if (!string.IsNullOrEmpty(paidAmount) && paidAmount.ToLower() == "undefined")
            {
                paidAmount = "";
            }
            if (!string.IsNullOrEmpty(Paid_Amount) && Paid_Amount.ToLower() == "undefined")
            {
                Paid_Amount = "";
            }
            if (!string.IsNullOrEmpty(Total_Charges) && Total_Charges.ToLower() == "undefined")
            {
                Total_Charges = "";
            }
            if (!string.IsNullOrEmpty(Refralnumber) && Refralnumber.ToLower() == "undefined")
            {
                Refralnumber = "";
            }

            string cde_proc_add = ProcedureCode;
            List<PDMSRestServices.Models.ProfessionalServiceDetail> listData = new List<PDMSRestServices.Models.ProfessionalServiceDetail>();
            DataSet dsProfessionalclaimGrid = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(claimid))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>
            {
              { "Service_Line", lblDetailsItemProf },
                { "Procedure_Code", !string.IsNullOrEmpty(ProcedureCode)? ProcedureCode.ToUpper(): ProcedureCode},
                { "Place_of_Service", ProfPlaceOfService },
                { "Line_Control_Number", Linenumber  },
                { "Prior_Authorization_Number", PriorAuth },
                { "Referral_Number", Refralnumber },
                { "Modifier1", !string.IsNullOrEmpty(Modifier1)? Modifier1.ToUpper() : Modifier1 },
                { "Modifier2", !string.IsNullOrEmpty(Modifier2)? Modifier2.ToUpper() : Modifier2 },
                { "Modifier3", !string.IsNullOrEmpty(Modifier3)? Modifier3.ToUpper() : Modifier3 },
                { "Modifier4", !string.IsNullOrEmpty(Modifier4)? Modifier4.ToUpper() : Modifier4 },
                { "Diagnosis_Pointer1", DiagnosisPointer1 },
                { "Diagnosis_Pointer2", DiagnosisPointer2 },
                { "Diagnosis_Pointer3", DiagnosisPointer3 },
                { "Diagnosis_Pointer4", DiagnosisPointer4 },
                { "Status", status },
                { "Referred_EPSDT_Service", Referred_EPSDT_Service },
                { "Unit_Of_Measurement", UnitsOfMeasurement },
                { "Family_Planning", Family_Planning },
                { "Emergency", Emergency},
                { "DME_Cert_Type", DMECertType },
                { "Final_EAPG", Final_EAPG },
                { "Claim_ID", claimid },
                { "Date_of_Service", DateOfService },
                { "Payment_Action", Payment_Action },
                { "Charges",Charges},
                { "Paid_Amount", Paid_Amount },
                { "Paid_Units", paidunits },
                { "Total_Charges", Total_Charges },
                { "ToTal_Amount_Paid", paidAmount},
                { "Billed_Units", BilledUnits },
                { "DME_Duration", DMEDuration },
                { "Cert_Revision", Cert_Revision },
                { "Created_Date_Time", DateTime.Now.ToString() },
                 { "Claim_Type", "2" },
            };

                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Service_Details", parms);
            }
            return Ok(true);
        }

        [HttpPost]
        [Route("SaveClaimsProfessionalServiceDetails")]
        public IActionResult SaveClaimsProfessionalServiceDetails(string claimid = "", string _slLine = "", string _procedureProfessionalCode = "", string _placeofserviceProfessional = "", string _modifierProfessionalFirst = "",
          string _modifierProfessionalSecond = "", string _modifierProfessionalThird = "", string _modifierProfessionalFourth = "", string _chargesProfessional = "", string _ProfessionalDateOfService = "",
          string _unitofmeasurementProfessional = "", string _diagnosisProfessionalFirst = "", string _dignosisProfessionalSecond = "", string _dignosisProfessionalThird = "",
          string _dignosisProfessionalFourth = "", string _paidAmountProfessional = "", string _dmeCertTypeProfessional = "", string _dmeDurationProfessional = "",
          string _totalCharges = "", string _billUnitDental = "", string _paidUnitDental = "",
          string _statusServiceDetails = "", string tableName = "", string _priorAuth = "", string _refralnumber = "", string _linenumber = "", string userid = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", claimid);

            //_slLine = (Convert.ToInt32("00")).ToString();
            _slLine = (Convert.ToInt32(_slLine)).ToString();
            parms.Add("Service_Line", _slLine);
            parms.Add("Procedure_Code", string.IsNullOrEmpty(_procedureProfessionalCode) ? null : _procedureProfessionalCode.ToUpper());
            parms.Add("Date_of_Service", _ProfessionalDateOfService.ToString());
            parms.Add("Line_Control_Number", string.IsNullOrEmpty(_linenumber) ? null : _linenumber);
            parms.Add("Prior_Authorization_Number", string.IsNullOrEmpty(_priorAuth) ? null : _priorAuth);
            parms.Add("Referral_Number", string.IsNullOrEmpty(_refralnumber) ? null : _refralnumber);
            parms.Add("Place_of_Service", string.IsNullOrEmpty(_placeofserviceProfessional) ? null : _placeofserviceProfessional);
            parms.Add("Modifier1", !string.IsNullOrEmpty(_modifierProfessionalFirst) ? _modifierProfessionalFirst.ToUpper() : _modifierProfessionalFirst);
            parms.Add("Modifier2", !string.IsNullOrEmpty(_modifierProfessionalSecond) ? _modifierProfessionalSecond.ToUpper() : _modifierProfessionalSecond);
            parms.Add("Modifier3", !string.IsNullOrEmpty(_modifierProfessionalThird) ? _modifierProfessionalThird.ToUpper() : _modifierProfessionalThird);
            parms.Add("Modifier4", !string.IsNullOrEmpty(_modifierProfessionalFourth) ? _modifierProfessionalFourth.ToUpper() : _modifierProfessionalFourth);
            parms.Add("Diagnosis_Pointer1", _diagnosisProfessionalFirst);
            parms.Add("Diagnosis_Pointer2", _dignosisProfessionalSecond);
            parms.Add("Diagnosis_Pointer3", _dignosisProfessionalThird);
            parms.Add("Diagnosis_Pointer4", _dignosisProfessionalFourth);
            parms.Add("Status", _statusServiceDetails);
            parms.Add("Charges", string.IsNullOrEmpty(_chargesProfessional) ? null : _chargesProfessional);
            if (!string.IsNullOrEmpty(_paidAmountProfessional))
            {
                string paidamount = _paidAmountProfessional.Contains('$') ? _paidAmountProfessional.Replace('$', ' ').TrimStart() : _paidAmountProfessional;
                parms.Add("Paid_Amount", paidamount);
            }
            else
            {
                parms.Add("Paid_Amount", null);

            }
            parms.Add("Billed_Units", string.IsNullOrEmpty(_billUnitDental) ? null : _billUnitDental);
            parms.Add("Paid_Units", string.IsNullOrEmpty(_paidUnitDental) ? null : _paidUnitDental);
            parms.Add("Total_Charges", string.IsNullOrEmpty(_totalCharges) ? null : _totalCharges);
            parms.Add("Unit_Of_Measurement", string.IsNullOrEmpty(_unitofmeasurementProfessional) ? null : _unitofmeasurementProfessional);
            parms.Add("Last_modified_date", DateTime.Now.ToString());
            // parms.Add("Family_Planning", string.IsNullOrEmpty(_fai) ? null : _billUnitDental);
            // parms.Add("Emergency", string.IsNullOrEmpty(_emer) ? null : _billUnitDental);
            parms.Add("DME_Cert_Type", string.IsNullOrEmpty(_dmeCertTypeProfessional) ? null : _dmeCertTypeProfessional);
            // parms.Add("Final_EAPG", string.IsNullOrEmpty(_fin) ? null : _dmeCertTypeProfessional);
            //parms.Add("Payment_Action", string.IsNullOrEmpty(_pay) ? null : _dmeCertTypeProfessional);
            parms.Add("DME_Duration", string.IsNullOrEmpty(_dmeDurationProfessional) ? null : _dmeDurationProfessional);
            // parms.Add("Cert_Revision", string.IsNullOrEmpty(_dmeCertTypeProfessional) ? null : _dmeCertTypeProfessional);
            // parms.Add("Final_EAPG", string.IsNullOrEmpty(_fin) ? null : _dmeCertTypeProfessional);
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Created_date_time", DateTime.Now.ToString());
            parms.Add("Created_by_user", HelperFacade.GetUserId(userid).ToString());
            MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData(tableName, parms);

            return Ok(true);
        }


        [HttpGet]
        [Route("GetDentailServiceDetails")]
        public IActionResult GetDentailServiceDetails(string claimid = "")
        {
            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, "0", true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_DataForClaims", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.DentalServiceDetail> dentalServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            DataTable claimsDataTable = new DataTable();
            string lblServiceDateInformation = null;
            claimsDataTable = dt != null ? dt : null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail dentalServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();
                    dentalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    dentalServiceDetail.plc_service = dr["plc_service"].ToString();
                    dentalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    dentalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    dentalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    dentalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    dentalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    dentalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    dentalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    dentalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    dentalServiceDetail.service_Line = dr["Service_Line"].ToString();
                    dentalServiceDetail.ServiceInfo_DOS = lblServiceDateInformation;
                    dentalServiceData.Add(dentalServiceDetail);
                }
            }
            return Ok(dentalServiceData);

        }

        [HttpPost]
        [Route("AddDentalServiceDetails")]
        public IActionResult AddDentalServiceDetails(string claimid = "", string procedureCode = "", string placeOfService = "", string dateOfService = "",
            string charges = "", string billedUnits = "", string lineControlNumber = "", string priorAuthNumber = "", string referralNumber = "", string ProsthesisCode = "",
            string oralCavityDentalFirst = "", string oralCavityDentalSecond = "", string oralCavityDentalThird = "", string oralCavityDentalFourth = "",
            string oralCavityDentalFifth = "", string diagnosisPointerFirst = "", string diagnosisPointerSecond = "", string diagnosisPointerThird = "",
            string diagnosisPointerFourth = "", string modifierDentalFirst = "", string modifierDentalSecond = "", string modifierDentalThird = "",
            string modifierDentalFourth = "", string lblDetailsItemDental = "", string lblpaidAmountDental = "", string lblPaidUnitDental = "", string lblStatusServiceDetails = "", string user = "")
        {
            if (!string.IsNullOrEmpty(lblDetailsItemDental) && lblDetailsItemDental.ToLower() == "undefined")
            {
                lblDetailsItemDental = "";
            }
            if (!string.IsNullOrEmpty(lblpaidAmountDental) && lblpaidAmountDental.ToLower() == "undefined")
            {
                lblpaidAmountDental = "";
            }
            if (!string.IsNullOrEmpty(lblPaidUnitDental) && lblPaidUnitDental.ToLower() == "undefined")
            {
                lblPaidUnitDental = "";
            }
            if (!string.IsNullOrEmpty(lblStatusServiceDetails) && lblStatusServiceDetails.ToLower() == "undefined")
            {
                lblStatusServiceDetails = "";
            }

            string cde_proc_add = procedureCode;
            List<PDMSRestServices.Models.DentalServiceDetail> listData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataSet dsDentalclaimGrid = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(claimid))
            {
                var dentalservicedetail = new PDMSRestServices.Models.DentalServiceDetail();
                if (cde_proc_add != string.Empty && !string.IsNullOrEmpty(charges)
                    && !string.IsNullOrEmpty(billedUnits)
                    && !string.IsNullOrEmpty(dateOfService))
                {
                    dentalservicedetail.num_dtl_total = lblDetailsItemDental;
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Claim_id", claimid);
                    DataSet dsSerialNo = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Service_Details", parms);

                    // lblDetailsItemDental = (listData.Count + 1).ToString();
                    dentalservicedetail.cde_proc = !string.IsNullOrEmpty(procedureCode) ? procedureCode.ToUpper() : procedureCode;
                    dentalservicedetail.plc_service = placeOfService;
                    dentalservicedetail.mdf_first = !string.IsNullOrEmpty(modifierDentalFirst) ? modifierDentalFirst.ToUpper() : modifierDentalFirst;
                    dentalservicedetail.mdf_secnd = !string.IsNullOrEmpty(modifierDentalSecond) ? modifierDentalSecond.ToUpper() : modifierDentalSecond;
                    dentalservicedetail.mdf_thrd = !string.IsNullOrEmpty(modifierDentalThird) ? modifierDentalThird.ToUpper() : modifierDentalThird;
                    dentalservicedetail.mdf_forth = !string.IsNullOrEmpty(modifierDentalFourth) ? modifierDentalFourth.ToUpper() : modifierDentalFourth;
                    dentalservicedetail.cde_clm_chrge = string.IsNullOrEmpty(charges) ? "0" : charges;
                    dentalservicedetail.line_ctr_num = lineControlNumber;
                    dentalservicedetail.digno_first = diagnosisPointerFirst;
                    dentalservicedetail.digno_sec = diagnosisPointerSecond;
                    dentalservicedetail.digno_third = diagnosisPointerThird;
                    dentalservicedetail.digno_forth = diagnosisPointerFourth;
                    dentalservicedetail.pad_amnt = lblpaidAmountDental;
                    dentalservicedetail.prior_auth = priorAuthNumber;
                    dentalservicedetail.orl_cvt_first = oralCavityDentalFirst;
                    dentalservicedetail.orl_cvt_sec = oralCavityDentalSecond;
                    dentalservicedetail.orl_cvt_third = oralCavityDentalThird;
                    dentalservicedetail.orl_cvt_forth = oralCavityDentalFourth;
                    dentalservicedetail.orl_cvt_fifth = oralCavityDentalFifth;
                    dentalservicedetail.bil_unt = billedUnits;
                    dentalservicedetail.ref_num = referralNumber;
                    dentalservicedetail.prosthesis_cd = ProsthesisCode;
                    dentalservicedetail.pad_unt = lblPaidUnitDental;
                    dentalservicedetail.qty_billed = string.IsNullOrEmpty(lblpaidAmountDental) ? "0" : lblpaidAmountDental;
                    if (!string.IsNullOrEmpty(dateOfService))
                        dentalservicedetail.ServiceDate = Convert.ToDateTime(dateOfService);
                    dentalservicedetail.amt_billed = string.IsNullOrEmpty(charges) ? "0" : charges;
                    dentalservicedetail.cde_clm_status = "Pending Submission";
                }

                Claims.SaveClaimsDentalServiceDetails(claimid, lblDetailsItemDental, procedureCode, placeOfService, modifierDentalFirst,
                        modifierDentalSecond, modifierDentalThird, modifierDentalFourth, charges, dateOfService, lineControlNumber,
                        diagnosisPointerFirst, diagnosisPointerSecond, diagnosisPointerThird, diagnosisPointerFourth,
                       lblpaidAmountDental, priorAuthNumber, oralCavityDentalFirst, oralCavityDentalSecond,
                       oralCavityDentalThird, oralCavityDentalFourth, oralCavityDentalFifth, charges,
                       billedUnits, referralNumber, ProsthesisCode, lblPaidUnitDental, ProsthesisCode,
                       lblStatusServiceDetails, "Claims_Service_Details", user);
            }
            return Ok(true);
        }

        [HttpPost]
        [Route("AddOtherPayerDetails")]
        public IActionResult AddOtherPayerDetails(string claimid = "", string otherPayerName = "", string healthPlanID = "", string claimFilingIndicator = "",
            string payerResponsibilitySequence = "", string subscriberNumber = "", string policyNumber = "", string groupName = "", string insuranceTypeCode = "",
            string patientRelationshipSuscriber = "", string subscriberFirstName = "", string subscriberLastName = "", string subscriberMiddleName = "",
            string subscriberAddressLine1 = "", string subscriberAddressLine2 = "", string subscriberCity = "", string subscriberState = "", string subscriberZip = "",
            string claimAdjudicationLevel = "", string claimNumber = "", string paidDate = "", string paidAmount = "", string nonCoveredAmoount = "", string userid = "")

        {
            // if (isAddressVerified)
            //{
            //  if (ValidateData())
            //{

            List<OtherPayerDetail> otherPayerPanelData = new List<OtherPayerDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Other_Payer_Name", otherPayerName);
            parms.Add("Health_Plan_ID", healthPlanID);
            parms.Add("Claim_Filing_Indicator", claimFilingIndicator);
            parms.Add("Payer_Responsibility_Sequence", payerResponsibilitySequence);
            parms.Add("Subscriber_Subscriber_Number", subscriberNumber);
            parms.Add("Policy_Number", policyNumber);
            parms.Add("Group_Name", groupName);
            parms.Add("Insurance_Type_Code", insuranceTypeCode);
            parms.Add("Claim_ID", claimid);
            parms.Add("Patient_to_Subscriber", patientRelationshipSuscriber);
            parms.Add("Subscriber_First_Name", subscriberFirstName);
            parms.Add("Subscriber_Last_Name", subscriberLastName);
            parms.Add("Subscriber_Middle_Name", subscriberMiddleName);
            parms.Add("Subscriber_AddressLine1", subscriberAddressLine1);
            parms.Add("Subscriber_AddressLine2", subscriberAddressLine2);
            parms.Add("Subscriber_City", subscriberCity);
            parms.Add("Subscriber_State", subscriberState);
            parms.Add("Subscriber_ZIP", subscriberZip);
            parms.Add("Claim_Adjudication_Level", claimAdjudicationLevel);
            parms.Add("Claim_Number", claimNumber);
            parms.Add("Paid_Date", paidDate);
            parms.Add("Paid_Amount", paidAmount);
            parms.Add("Total_Non_Covered_Amount", nonCoveredAmoount);
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
            if (!string.IsNullOrEmpty(claimid))
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Other_Payer_Information", parms);
                //GetFilteredPayerSequence();
                //GetClaimFilingIndicator();
                //GetOtherPayerInfo(null);
                //ClearOtherPayerFields();
                //ucHeaderOtherPayerAdjustmentMappingProfessional.GetHealthPlanIDForHeaderOtherPayer();
                //ucHeaderOtherPayerAdjustmentMapping.GetHealthPlanIDForHeaderOtherPayer();
                //ucHeaderOtherPayerAdjustmentMappingInstitutional.GetHealthPlanIDForHeaderOtherPayer();
            }
            // FieldEnableDisable();

            // }
            //RefreshOtherPayerPaidAmountDropdown();
            // }

            var data = string.Empty;
            DataSet bindGrid = Claims.GetOtherPayerData(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherPayerDetail otherPayerDetail = new OtherPayerDetail();
                    otherPayerDetail.otherPayerName = dr["Other_Payer_Name"].ToString();
                    otherPayerDetail.healthPlanID = dr["Health_Plan_ID"].ToString();
                    otherPayerDetail.subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    otherPayerDetail.subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    otherPayerDetail.payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    otherPayerDetail.claimAdjudicationLevel = dr["Claim_Adjudication_Level"].ToString();
                    otherPayerDetail.otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    otherPayerPanelData.Add(otherPayerDetail);
                }
            }

            return Ok(otherPayerPanelData);
        }

        [HttpPut]
        [Route("UpdateOtherPayerDetails")]
        public IActionResult UpdateOtherPayerDetails(string otherpayerInfoId = "", string claimid = "", string otherPayerName = "", string healthPlanID = "",
            string claimFilingIndicator = "", string payerResponsibilitySequence = "", string subscriberNumber = "", string policyNumber = "", string groupName = "",
            string insuranceTypeCode = "", string patientRelationshipSuscriber = "", string subscriberFirstName = "", string subscriberLastName = "",
            string subscriberMiddleName = "", string subscriberAddressLine1 = "", string subscriberAddressLine2 = "", string subscriberCity = "",
            string subscriberState = "", string subscriberZip = "", string claimAdjudicationLevel = "", string claimNumber = "", string paidDate = "",
            string paidAmount = "", string nonCoveredAmoount = "", string userid = "")

        {

            //if (Page.IsValid)
            //{
            //    if (isAddressVerified)
            //    {
            //        if (ValidateEditData())
            //        {

            List<OtherPayerDetail> otherPayerPanelData = new List<OtherPayerDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claims_Other_Payer_Information_ID", otherpayerInfoId);
            parms.Add("Other_Payer_Name", otherPayerName);
            parms.Add("Health_Plan_ID", healthPlanID);
            parms.Add("Claim_Filing_Indicator", claimFilingIndicator);
            parms.Add("Payer_Responsibility_Sequence", payerResponsibilitySequence);
            parms.Add("Subscriber_Subscriber_Number", subscriberNumber);
            parms.Add("Policy_Number", policyNumber);
            parms.Add("Group_Name", groupName);
            parms.Add("Insurance_Type_Code", insuranceTypeCode);
            parms.Add("Claim_ID", claimid);
            parms.Add("Patient_to_Subscriber", patientRelationshipSuscriber);
            parms.Add("Subscriber_First_Name", subscriberFirstName);
            parms.Add("Subscriber_Last_Name", subscriberLastName);
            parms.Add("Subscriber_Middle_Name", subscriberMiddleName);
            parms.Add("Subscriber_AddressLine1", subscriberAddressLine1);
            parms.Add("Subscriber_AddressLine2", subscriberAddressLine2);
            parms.Add("Subscriber_City", subscriberCity);
            parms.Add("Subscriber_State", subscriberState);
            parms.Add("Subscriber_ZIP", subscriberZip);
            parms.Add("Claim_Adjudication_Level", claimAdjudicationLevel);
            parms.Add("Claim_Number", claimNumber);
            parms.Add("Paid_Date", paidDate);
            parms.Add("Paid_Amount", paidAmount);
            parms.Add("Total_Non_Covered_Amount", nonCoveredAmoount);
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            if (!string.IsNullOrEmpty(claimid))
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Other_Payer_Information", parms);
                //GetOtherPayerInfo(null);
                //DataSet dsFilteredPayerSeq = FetchFilteredOtherPayerSequence();
                //ClearOtherPayerFields();
                //ddlPayerSequence.ClearSelection();
                //ddlPayerSequence.SelectedValue = null;
                //ddlPayerSequence.DataBind();
                //Helper.LoadList(ddlPayerSequence, dsFilteredPayerSeq.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_ID", true);
                //DataSet dsFilteredClaimFilingIndicator = GetFilteredClaimIndicator();
                //ddlClaimFilingIndicator.ClearSelection();
                //ddlClaimFilingIndicator.SelectedValue = null;
                //ddlClaimFilingIndicator.DataBind();
                //Helper.LoadList(ddlClaimFilingIndicator, dsFilteredClaimFilingIndicator.Tables[0], "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC", "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE", true);
                //Dictionary<string, string> pars = new Dictionary<string, string>();
                //pars.Add("Claim_ID", hdnClaimId.Value.ToString());
                //pars.Add("HealthPlanId", hdnHealthPlanIdOnEdit.Value);
                //MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsDataWithParams("claim_other_payer_details", pars);
                //RefreshOtherPayers();
                //btnOtherPayerAdd.Visible = false;
                //if (grdOtherPayer.Rows.Count <= 10)
                //{
                //    btnOtherPayerAdd.Visible = true;
                //}

            }
            //btnOtherPayerUpdate.Visible = false;
            //btnOtherPayerCancel.Visible = false;
            //btnOtherPayerAdd.Visible = true;
            //FieldEnableDisable();
            //        }
            //    }
            //}

            var data = string.Empty;
            DataSet bindGrid = Claims.GetOtherPayerData(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherPayerDetail otherPayerDetail = new OtherPayerDetail();
                    otherPayerDetail.otherPayerName = dr["Other_Payer_Name"].ToString();
                    otherPayerDetail.healthPlanID = dr["Health_Plan_ID"].ToString();
                    otherPayerDetail.subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    otherPayerDetail.subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    otherPayerDetail.payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    otherPayerDetail.claimAdjudicationLevel = dr["Claim_Adjudication_Level"].ToString();
                    otherPayerDetail.otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    otherPayerDetail.claimFilingIndicator = dr["Claim_Filing_Indicator"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    otherPayerPanelData.Add(otherPayerDetail);
                }
            }

            return Ok(otherPayerPanelData);

        }

        [HttpDelete]
        [Route("DeleteDentalOtherPayerLineitem")]
        public IActionResult DeleteDentalOtherPayerLineitem(string otherpayerInfoID = "", string claimid = "")
        {
            List<OtherPayerDetail> otherPayerPanelData = new List<OtherPayerDetail>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("claims_other_payer_information", "Claims_Other_Payer_Information_ID", Convert.ToInt32(otherpayerInfoID));
            }
            catch { }

            DataSet bindGrid = Claims.GetOtherPayerData(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherPayerDetail otherPayerDetail = new OtherPayerDetail();
                    otherPayerDetail.otherPayerName = dr["Other_Payer_Name"].ToString();
                    otherPayerDetail.healthPlanID = dr["Health_Plan_ID"].ToString();
                    otherPayerDetail.subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    otherPayerDetail.subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    otherPayerDetail.payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    otherPayerDetail.claimAdjudicationLevel = dr["Claim_Adjudication_Level"].ToString();
                    otherPayerDetail.otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    otherPayerDetail.claimFilingIndicator = dr["Claim_Filing_Indicator"].ToString();
                    otherPayerPanelData.Add(otherPayerDetail);
                }
            }
            return Ok(otherPayerPanelData);
        }

        [HttpGet]
        [Route("GetDentalOtherPayerLineitem")]
        public IActionResult GetDentalOtherPayerLineitem(string claimid = "")
        {
            List<OtherPayerDetail> otherPayerPanelData = new List<OtherPayerDetail>();
            var data = string.Empty;
            DataSet bindGrid = Claims.GetOtherPayerData(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherPayerDetail otherPayerDetail = new OtherPayerDetail();
                    otherPayerDetail.otherPayerName = dr["Other_Payer_Name"].ToString();
                    otherPayerDetail.healthPlanID = dr["Health_Plan_ID"].ToString();
                    otherPayerDetail.subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    otherPayerDetail.subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    otherPayerDetail.payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    otherPayerDetail.claimAdjudicationLevel = dr["Claim_Adjudication_Level"].ToString();
                    otherPayerDetail.otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    otherPayerDetail.claimFilingIndicator = dr["Claim_Filing_Indicator"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    otherPayerPanelData.Add(otherPayerDetail);
                }
            }
            return Ok(otherPayerPanelData);

        }

        [HttpPut]
        [Route("EditDiagnosisCode")]
        public IActionResult EditDiagnosisCode(string hdnClaimIdDiag = "", string claimDiag_Info_Id = "", string hdnClaimType_Diag = "",
            string sequence = "", string DiagCode = "", string ICDVer = "", string presentOnAdmission = "", string DiagDesc = "", string userid = "")
        {

            List<DiagnosisPanel> diagnosispanelData = new List<DiagnosisPanel>();
            string diagCode = DiagCode;
            string icdVersion = ICDVer;
            string dropDownSeqDesc = null;
            string drpdownPresentOnAdmission = null;
            if (hdnClaimType_Diag == CON.ClaimsType.Institutional)
            {
                dropDownSeqDesc = sequence;
                drpdownPresentOnAdmission = presentOnAdmission;
            }
            //if (hdnClaimType_Diag == CON.ClaimsType.Institutional &&
            //        (dropDownSeqDesc == "Principal" || dropDownSeqDesc == "Other" || dropDownSeqDesc == "External Cause of Injury")
            //        && String.IsNullOrEmpty(drpdownPresentOnAdmission))
            //{
            //    lblPresentOnAdmissionEror.Text = "Present on Admission is required";
            //    return;
            //}
            //else
            //{
            //    lblPresentOnAdmissionEror.Text = string.Empty;
            //}
            //Updating db
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claims_Diagnosis_Information_ID", claimDiag_Info_Id);
            parms.Add("Diagnosis_Code", DiagCode);
            parms.Add("ICD_Version", icdVersion);
            parms.Add("Claim_ID", hdnClaimIdDiag);
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            if (hdnClaimType_Diag == CON.ClaimsType.Institutional)
            {
                parms.Add("Sequence_Description_ID", dropDownSeqDesc);
                parms.Add("Present_On_admission", drpdownPresentOnAdmission);
            }
            else
            {
                parms.Add("Sequence_Description_ID", null);
                parms.Add("Present_On_admission", null);
            }
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Diagnosis_Information", parms);
            }
            catch (Exception ex) { }
            parms.Clear();

            var data = string.Empty;
            DataSet bindGrid = Claims.GetDiagnosisData(hdnClaimIdDiag, hdnClaimType_Diag);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DiagnosisPanel DiagPaneldetails = new DiagnosisPanel();
                    DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                    DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                    DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                    string PresentOnAdmission = "";
                    if (dr["Present_On_Admission"].ToString().Equals("W"))
                    {
                        PresentOnAdmission = "Not Applicable";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("N"))
                    {
                        PresentOnAdmission = "No";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                    {
                        PresentOnAdmission = "Yes";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("U"))
                    {
                        PresentOnAdmission = "Unknown";
                    }
                    else
                    {
                        PresentOnAdmission = dr["Present_On_Admission"].ToString();
                    }
                    DiagPaneldetails.PreasentOnAdmission = PresentOnAdmission;
                    DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToString();
                    DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                    DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    diagnosispanelData.Add(DiagPaneldetails);
                }
            }
            return Ok(diagnosispanelData);


        }

        [HttpPut]
        [Route("UpdateProviderNotes")]
        public IActionResult UpdateProviderNotes(string Claimtype = "", string claimId = "", string notes = "", string ProviderNoteID = "", string referenceCode = "", string user = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (Claimtype == CON.ClaimsType.Dental)
            {
                parms.Add("Notes", notes);
                parms.Add("Claim_Type", Claimtype);
                parms.Add("Note_Reference_Code_ID", null);
            }
            else if (Claimtype == CON.ClaimsType.Institutional)
            {
                parms.Add("Notes", notes);
                parms.Add("Claim_Type", Claimtype);
                parms.Add("Note_Reference_Code_ID", referenceCode);
            }
            parms.Add("Claim_ID", claimId);
            parms.Add("Claims_Providers_Note_ID", ProviderNoteID);
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(user).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Providers_Note", parms);
            }
            catch { }
            parms.Clear();

            var data = string.Empty;
            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype, 0);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);
        }

        [HttpPut]
        [Route("UpdateProviderBillingNotes")]
        public IActionResult UpdateProviderBillingNotes(string Claimtype = "", string claimId = "", string notes = "", string ProviderNoteID = "",
            string referenceCode = "", string userid = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (Claimtype == CON.ClaimsType.Dental)
            {
                parms.Add("Notes", notes);
                parms.Add("Claim_Type", Claimtype);
                parms.Add("Note_Reference_Code_ID", null);
            }
            else if (Claimtype == CON.ClaimsType.Institutional)
            {
                parms.Add("Notes", notes);
                parms.Add("Claim_Type", Claimtype);
                parms.Add("Note_Reference_Code_ID", referenceCode);
            }
            parms.Add("Claim_ID", claimId);
            parms.Add("Claims_Providers_Note_ID", ProviderNoteID);
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Providers_Note", parms);
            }
            catch { }
            parms.Clear();

            var data = string.Empty;
            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype, 1);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);
        }

        [HttpPut]
        [Route("EditAmbulanceDropOffCode")]
        public IActionResult EditAmbulanceDropOffCode(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = "", string claimid = "", string serviceLine = "",
            string pickUpAddressLine1 = "", string pickUpAddressLine2 = "", string pickupcity = "", string pickUPState = "", string pickUPZip = "",
            string dropOffLocationName = "", string dropOffAddressLine1 = "", string dropOffAddressLine2 = "", string dropOffCity = "",
            string dropOffState = "", string dropOffZip = "", string userid = "")
        {


            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claims_Ambulance_Pick_Up_Drop_Off_Location_ID", Claims_Ambulance_Pick_Up_Drop_Off_Location_ID);
            parms.Add("Service_Line", serviceLine.Length == 1 ? serviceLine.PadLeft(2, '0') : serviceLine);
            parms.Add("Pick_Up_Address_Line1", pickUpAddressLine1);
            parms.Add("Pick_Up_Address_Line2", pickUpAddressLine2);
            parms.Add("Pick_Up_City", pickupcity);
            parms.Add("Pick_Up_State", pickUPState);
            parms.Add("Pick_Up_ZIP", pickUPZip);
            parms.Add("Drop_Off_Location_Name", dropOffLocationName);
            parms.Add("Drop_Off_Address_Line1", dropOffAddressLine1);
            parms.Add("Drop_Off_Address_Line2", dropOffAddressLine2);
            parms.Add("Drop_Off_City", dropOffCity);
            parms.Add("Drop_Off_State", dropOffState);
            parms.Add("Drop_Off_Zip", dropOffZip);
            parms.Add("Last_Modified_date", DateTime.Now.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString().Trim());
            parms.Add("Claim_ID", claimid);

            List<AmbulanceDropOffPanel> AmbulanceDropOff = new List<AmbulanceDropOffPanel>();
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(claimid))
                {
                    AmbulanceDropOffPanel Ambulance = new AmbulanceDropOffPanel();
                    DataSet dsGetData = Claims.GetAmbulanceDropOff(claimid);
                    DataTable dtambulance = dsGetData.Tables[0];
                    if (dtambulance.Rows.Count > 1)
                    {
                        var filtered_dt = dtambulance.AsEnumerable().Where(dr => dr.Field<int>("Claims_Ambulance_Pick_Up_Drop_Off_Location_ID") != Convert.ToInt32(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID));
                        DataTable dtResult = filtered_dt.CopyToDataTable();

                        if (dtResult.AsEnumerable().Any(p => p.Field<int>("Service_Line") == Convert.ToInt32(serviceLine)))
                        {
                            Ambulance.Error_Message = "Same Service Line number cannot be added again.";
                            result = true;
                        }
                        if (result)
                        {
                            AmbulanceDropOff.Add(Ambulance);
                        }
                    }
                    if (!result)
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Ambulance_Pick_Up_Drop_Off_Location", parms);
                    }

                }
            }
            catch { }
            parms.Clear();

            var data = string.Empty;
            DataSet bindGrid = Claims.GetAmbulanceDropOff(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            if (!result)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AmbulanceDropOffPanel AmbulanceDropOffInd = new AmbulanceDropOffPanel();
                        AmbulanceDropOffInd.serviceLine = dr["Service_Line"].ToString();
                        AmbulanceDropOffInd.pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                        AmbulanceDropOffInd.pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                        AmbulanceDropOffInd.pickUpCity = dr["Pick_Up_City"].ToString();
                        AmbulanceDropOffInd.pickUpState = dr["Pick_Up_State"].ToString();
                        AmbulanceDropOffInd.pickUpZip = dr["Pick_Up_ZIP"].ToString();
                        AmbulanceDropOffInd.dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                        AmbulanceDropOffInd.dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                        AmbulanceDropOffInd.dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                        AmbulanceDropOffInd.dropOffLocationCity = dr["Drop_Off_City"].ToString();
                        AmbulanceDropOffInd.dropOffLocationState = dr["Drop_Off_State"].ToString();
                        AmbulanceDropOffInd.dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                        AmbulanceDropOffInd.Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                        AmbulanceDropOffInd.Claim_ID = dr["Claim_ID"].ToString();
                        // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                        AmbulanceDropOff.Add(AmbulanceDropOffInd);
                    }
                }
            }
            return Ok(AmbulanceDropOff);

        }


        [HttpPut]
        [Route("EditProfessionalServiceDetailCode")]
        public IActionResult EditProfessionalServiceDetailCode(string claimid = "", string ProcedureCode = "", string DateOfService = "", string DMECertType = "",
            string DMEDuration = "", string ProfPlaceOfService = "", string Modifier1 = "", string Modifier2 = "", string Modifier3 = "",
            string Modifier4 = "", string DiagnosisPointer1 = "", string DiagnosisPointer2 = "", string DiagnosisPointer3 = "", string DiagnosisPointer4 = "",
            string Charges = "", string UnitsOfMeasurement = "", string BilledUnits = "", string lblDetailsItemProf = "", string paidunits = "", string paidAmount = "",
            string status = "", string hdnProfessionalClaimServiceId = "", string hdnProfessionalServiceDetails = "", string Referred_EPSDT_Service = "",
            string Family_Planning = "", string Emergency = "", string Final_EAPG = "", string Payment_Action = "", string Paid_Amount = "", string Total_Charges = "",
            string Cert_Revision = "", string Linenumber = "", string PriorAuth = "", string Refralnumber = "", string user = "")
        {
            if (!string.IsNullOrEmpty(ProcedureCode) && ProcedureCode.ToLower() == "undefined")
            {
                ProcedureCode = "";
            }
            if (!string.IsNullOrEmpty(DateOfService) && DateOfService.ToLower() == "undefined")
            {
                DateOfService = "";
            }
            if (!string.IsNullOrEmpty(DMECertType) && DMECertType.ToLower() == "undefined")
            {
                DMECertType = "";
            }
            if (!string.IsNullOrEmpty(DMEDuration) && DMEDuration.ToLower() == "undefined")
            {
                DMEDuration = "";
            }
            if (!string.IsNullOrEmpty(ProfPlaceOfService) && ProfPlaceOfService.ToLower() == "undefined")
            {
                ProfPlaceOfService = "";
            }
            if (!string.IsNullOrEmpty(Charges) && Charges.ToLower() == "undefined")
            {
                Charges = "";
            }
            if (!string.IsNullOrEmpty(UnitsOfMeasurement) && UnitsOfMeasurement.ToLower() == "undefined")
            {
                UnitsOfMeasurement = "";
            }
            if (!string.IsNullOrEmpty(BilledUnits) && BilledUnits.ToLower() == "undefined")
            {
                BilledUnits = "";
            }
            if (!string.IsNullOrEmpty(lblDetailsItemProf) && lblDetailsItemProf.ToLower() == "undefined")
            {
                lblDetailsItemProf = "";
            }
            if (!string.IsNullOrEmpty(paidunits) && paidunits.ToLower() == "undefined")
            {
                paidunits = "";
            }
            if (!string.IsNullOrEmpty(paidAmount) && paidAmount.ToLower() == "undefined")
            {
                paidAmount = "";
            }
            if (!string.IsNullOrEmpty(Paid_Amount) && Paid_Amount.ToLower() == "undefined")
            {
                Paid_Amount = "";
            }
            if (!string.IsNullOrEmpty(Total_Charges) && Total_Charges.ToLower() == "undefined")
            {
                Total_Charges = "";
            }
            if (!string.IsNullOrEmpty(Refralnumber) && Refralnumber.ToLower() == "undefined")
            {
                Refralnumber = "";
            }

            string cde_proc_add = ProcedureCode;

            var Professionalservicedetail = new PDMSRestServices.Models.ProfessionalServiceDetail();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, lblDetailsItemProf, true));
            parms.Clear();
            DataSet dsClaimServiceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
            parms.Add("Claim_ID", claimid);
            parms.Add("Service_Line", lblDetailsItemProf);
            parms.Add("Procedure_Code", !string.IsNullOrEmpty(ProcedureCode) ? ProcedureCode.ToUpper() : ProcedureCode);
            parms.Add("Status", status);
            if (!string.IsNullOrEmpty(DateOfService))
                parms.Add("Date_of_Service", Convert.ToDateTime(DateOfService).ToString("MM/dd/yyyy"));
            parms.Add("Modifier1", !string.IsNullOrEmpty(Modifier1)? Modifier1.ToUpper(): Modifier1);
            parms.Add("Modifier2", !string.IsNullOrEmpty(Modifier2)?Modifier2.ToUpper(): Modifier2);
            parms.Add("Modifier3", !string.IsNullOrEmpty(Modifier3)? Modifier3.ToUpper(): Modifier3);
            parms.Add("Modifier4", !string.IsNullOrEmpty(Modifier4) ? Modifier4.ToUpper() : Modifier4);
            parms.Add("Diagnosis_Pointer1", DiagnosisPointer1);
            parms.Add("Diagnosis_Pointer2", DiagnosisPointer2);
            parms.Add("Diagnosis_Pointer3", DiagnosisPointer3);
            parms.Add("Diagnosis_Pointer4", DiagnosisPointer4);
            parms.Add("Charges", Charges);
            parms.Add("Line_Control_Number", Linenumber);
            parms.Add("Billed_Units", BilledUnits);
            parms.Add("Prior_Authorization_Number", PriorAuth);
            parms.Add("Referred_EPSDT_Service", Referred_EPSDT_Service);
            parms.Add("Unit_of_Measurement", UnitsOfMeasurement);
            parms.Add("Referral_Number", Refralnumber);
            parms.Add("Family_Planning", Family_Planning);
            parms.Add("Emergency", Emergency);
            parms.Add("DME_Cert_Type", DMECertType);
            if (!string.IsNullOrEmpty(paidunits))
                parms.Add("Paid_Units", paidunits);
            if (!string.IsNullOrEmpty(Paid_Amount))
                parms.Add("Paid_Amount", Paid_Amount);
            parms.Add("DME_Duration", DMEDuration);
            parms.Add("Final_EAPG", Final_EAPG);
            parms.Add("Cert_Revision", string.IsNullOrEmpty(Cert_Revision) ? "" : Convert.ToDateTime(Cert_Revision).ToString("MM/dd/yyyy"));
            parms.Add("Payment_Action", Payment_Action);

            if (!string.IsNullOrEmpty(Total_Charges))
            {
                parms.Add("Total_Charges", Total_Charges);
            }
            if (!string.IsNullOrEmpty(paidAmount))
            {
                parms.Add("Total_Amount_Paid", paidAmount);
            }
            parms.Add("Place_of_Service", ProfPlaceOfService);
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Service_Details", parms);

            return Ok(true);
        }

        [HttpGet]
        [Route("GetDentalServiceDetails")]
        public IActionResult getDentalServiceDetails(string claimid = "")
        {
            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.DentalServiceDetail> dentalServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail dentalServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();

                    dentalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    dentalServiceDetail.plc_service = dr["plc_service"].ToString();
                    dentalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    dentalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    dentalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    dentalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    dentalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    dentalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    dentalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    dentalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    dentalServiceDetail.service_Line = dr["Service_Line"].ToString();
                    dentalServiceData.Add(dentalServiceDetail);
                }
            }
            return Ok(dentalServiceData);

        }

        [HttpPut]
        [Route("EditDentalServiceDetailCode")]
        public IActionResult EditDentalServiceDetailCode(string claimid = "", string procedureCode = "", string placeOfService = "", string dateOfService = "", string charges = "",
            string billedUnits = "", string lineControlNumber = "", string priorAuthNumber = "", string referralNumber = "", string ProsthesisCode = "",
            string oralCavityDentalFirst = "", string oralCavityDentalSecond = "", string oralCavityDentalThird = "", string oralCavityDentalFourth = "",
            string oralCavityDentalFifth = "", string diagnosisPointerFirst = "", string diagnosisPointerSecond = "", string diagnosisPointerThird = "",
            string diagnosisPointerFourth = "", string modifierDentalFirst = "", string modifierDentalSecond = "", string modifierDentalThird = "",
            string modifierDentalFourth = "", string lblDetailsItemDental = "", string lblpaidAmountDental = "", string lblPaidUnitDental = "",
            string lblStatusServiceDetails = "", string hdnDentalServiceDetails = "", string hdnDentalClaimServiceId = "", string user = "")
        {
            if (!string.IsNullOrEmpty(lblDetailsItemDental) && lblDetailsItemDental.ToLower() == "undefined")
            {
                lblDetailsItemDental = "";
            }
            if (!string.IsNullOrEmpty(lblpaidAmountDental) && lblpaidAmountDental.ToLower() == "undefined")
            {
                lblpaidAmountDental = "";
            }
            if (!string.IsNullOrEmpty(lblPaidUnitDental) && lblPaidUnitDental.ToLower() == "undefined")
            {
                lblPaidUnitDental = "";
            }
            if (!string.IsNullOrEmpty(lblStatusServiceDetails) && lblStatusServiceDetails.ToLower() == "undefined")
            {
                lblStatusServiceDetails = "";
            }
            string cde_proc_add = procedureCode;
            var dentalservicedetail = new PDMSRestServices.Models.DentalServiceDetail();
            if (cde_proc_add != string.Empty && !string.IsNullOrEmpty(charges)
                 && !string.IsNullOrEmpty(billedUnits)
                 && !string.IsNullOrEmpty(dateOfService))
            {
                dentalservicedetail.num_dtl_total = hdnDentalServiceDetails;
                dentalservicedetail.cde_proc = !string.IsNullOrEmpty(procedureCode) ? procedureCode.ToUpper() : procedureCode;
                dentalservicedetail.plc_service = placeOfService;
                dentalservicedetail.mdf_first = !string.IsNullOrEmpty(modifierDentalFirst) ? modifierDentalFirst.ToUpper(): modifierDentalFirst;
                dentalservicedetail.mdf_secnd = !string.IsNullOrEmpty(modifierDentalSecond) ? modifierDentalSecond.ToUpper(): modifierDentalSecond;
                dentalservicedetail.mdf_thrd = !string.IsNullOrEmpty(modifierDentalThird) ? modifierDentalThird.ToUpper(): modifierDentalThird;
                dentalservicedetail.mdf_forth = !string.IsNullOrEmpty(modifierDentalFourth) ? modifierDentalFourth.ToUpper(): modifierDentalFourth;
                dentalservicedetail.cde_clm_chrge = string.IsNullOrEmpty(charges) ? "0" : charges;
                dentalservicedetail.line_ctr_num = lineControlNumber;
                dentalservicedetail.digno_first = diagnosisPointerFirst;
                dentalservicedetail.digno_sec = diagnosisPointerSecond;
                dentalservicedetail.digno_third = diagnosisPointerThird;
                dentalservicedetail.digno_forth = diagnosisPointerFourth;
                dentalservicedetail.pad_amnt = lblpaidAmountDental;
                dentalservicedetail.prior_auth = priorAuthNumber;
                dentalservicedetail.orl_cvt_first = oralCavityDentalFirst;
                dentalservicedetail.orl_cvt_sec = oralCavityDentalSecond;
                dentalservicedetail.orl_cvt_third = oralCavityDentalThird;
                dentalservicedetail.orl_cvt_forth = oralCavityDentalFourth;
                dentalservicedetail.orl_cvt_fifth = oralCavityDentalFifth;
                dentalservicedetail.bil_unt = billedUnits;
                dentalservicedetail.ref_num = referralNumber;
                dentalservicedetail.prosthesis_cd = ProsthesisCode;
                dentalservicedetail.pad_unt = lblPaidUnitDental;
                dentalservicedetail.qty_billed = string.IsNullOrEmpty(lblpaidAmountDental) ? "0" : lblpaidAmountDental;
                dentalservicedetail.ServiceDate = Convert.ToDateTime(dateOfService);
                dentalservicedetail.amt_billed = string.IsNullOrEmpty(charges) ? "0" : charges;
                if (!string.IsNullOrEmpty(dateOfService))
                    dentalservicedetail.ServiceDate = Convert.ToDateTime(dateOfService);

                dentalservicedetail.cde_clm_status = "Pending Submission";
            }
            Dictionary<string, string> parms = new Dictionary<string, string>();
            int serviceline = Convert.ToInt32(lblDetailsItemDental);
            lblDetailsItemDental = lblDetailsItemDental.ToString();
            parms.Add("Claim_ID", claimid);
            parms.Add("Service_Line", string.IsNullOrEmpty(lblDetailsItemDental) ? null : lblDetailsItemDental);
            parms.Add("Procedure_Code", string.IsNullOrEmpty(procedureCode) ? null : procedureCode.ToUpper());
            parms.Add("Date_of_Service", string.IsNullOrEmpty(dateOfService) ? null : dateOfService);
            parms.Add("Line_Control_Number", string.IsNullOrEmpty(lineControlNumber) ? null : lineControlNumber);
            parms.Add("Prior_Authorization_Number", string.IsNullOrEmpty(priorAuthNumber) ? null : priorAuthNumber);
            parms.Add("Referral_Number", string.IsNullOrEmpty(referralNumber) ? null : referralNumber);
            parms.Add("Place_of_Service", string.IsNullOrEmpty(placeOfService) ? string.Empty : placeOfService);
            parms.Add("Modifier1", string.IsNullOrEmpty(modifierDentalFirst) ? null : modifierDentalFirst.ToUpper());
            parms.Add("Modifier2", string.IsNullOrEmpty(modifierDentalSecond) ? null : modifierDentalSecond.ToUpper());
            parms.Add("Modifier3", string.IsNullOrEmpty(modifierDentalThird) ? null : modifierDentalThird.ToUpper());
            parms.Add("Modifier4", string.IsNullOrEmpty(modifierDentalFourth) ? null : modifierDentalFourth.ToUpper());

            parms.Add("Diagnosis_Pointer1", string.IsNullOrEmpty(diagnosisPointerFirst) ? string.Empty : diagnosisPointerFirst);
            parms.Add("Diagnosis_Pointer2", string.IsNullOrEmpty(diagnosisPointerSecond) ? string.Empty : diagnosisPointerSecond);
            parms.Add("Diagnosis_Pointer3", string.IsNullOrEmpty(diagnosisPointerThird) ? string.Empty : diagnosisPointerThird);
            parms.Add("Diagnosis_Pointer4", string.IsNullOrEmpty(diagnosisPointerFourth) ? string.Empty : diagnosisPointerFourth);

            parms.Add("Oral_Cavity1", string.IsNullOrEmpty(oralCavityDentalFirst) ? null : oralCavityDentalFirst);
            parms.Add("Oral_Cavity2", string.IsNullOrEmpty(oralCavityDentalSecond) ? null : oralCavityDentalSecond);
            parms.Add("Oral_Cavity3", string.IsNullOrEmpty(oralCavityDentalThird) ? null : oralCavityDentalThird);
            parms.Add("Oral_Cavity4", string.IsNullOrEmpty(oralCavityDentalFourth) ? null : oralCavityDentalFourth);
            parms.Add("Oral_Cavity5", string.IsNullOrEmpty(oralCavityDentalFifth) ? null : oralCavityDentalFifth);

            parms.Add("Prosthesis_Crown_InlayCode", string.IsNullOrEmpty(ProsthesisCode) ? null : ProsthesisCode);
            parms.Add("Status", string.IsNullOrEmpty(lblStatusServiceDetails) ? null : lblStatusServiceDetails);

            parms.Add("Charges", string.IsNullOrEmpty(charges) ? null : charges);

            parms.Add("Paid_Amount", string.IsNullOrEmpty(lblpaidAmountDental) ? null : lblpaidAmountDental);
            parms.Add("Billed_Units", string.IsNullOrEmpty(billedUnits) ? null : billedUnits);
            parms.Add("Paid_Units", string.IsNullOrEmpty(lblPaidUnitDental) ? null : lblPaidUnitDental);
            parms.Add("Total_Charges", string.IsNullOrEmpty(charges) ? null : charges);
            parms.Add("Last_modified_date", DateTime.Now.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(user).ToString());
            //parms.Add("Created_date_time", DateTime.Now.ToString());
            //parms.Add("Created_by_user", HelperFacade.GetUserId(userName).ToString());
            MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Service_Details", parms);

            return Ok(true);
        }

        [HttpPut]
        [Route("EditInstiServiceDetailCode")]
        public IActionResult EditInstiServiceDetailCode(string claimid = "", string service_line = "", string InsTotalAmountBilled = "", string InsTotalAmountPaid = "",
            string InsFinalEAPG = "", string InsPaymentAction = "", string paidAmt = "", string revenueCode = "", string procedureType = "", string procedureCode = "",
            string modifierDentalFirst = "", string modifierDentalSecond = "", string modifierDentalThird = "", string modifierDentalFourth = "", string lineControlNumber = "",
            string fromDOB = "", string toDOB = "", string nonCoveredCharges = "", string status = "", string Unit = "", string UnitOfMeasurement = "", string totalCharges = "",
            string user = "")
        {
            if (!string.IsNullOrEmpty(paidAmt) && paidAmt.ToLower() == "undefined")
            {
                paidAmt = "";
            }
            if (!string.IsNullOrEmpty(InsFinalEAPG) && InsFinalEAPG.ToLower() == "undefined")
            {
                InsFinalEAPG = "";
            }
            if (!string.IsNullOrEmpty(InsPaymentAction) && InsPaymentAction.ToLower() == "undefined")
            {
                InsPaymentAction = "";
            }
            if ((!string.IsNullOrEmpty(InsTotalAmountBilled) && InsTotalAmountBilled.ToLower() == "undefined") || InsTotalAmountBilled.ToLower() == "null")
            {
                InsTotalAmountBilled = "0";
            }
            if ((!string.IsNullOrEmpty(InsTotalAmountPaid) && InsTotalAmountPaid.ToLower() == "undefined") || InsTotalAmountPaid.ToLower() == "null")
            {
                InsTotalAmountPaid = "0";
            }
            Dictionary<string, string> parms = new Dictionary<string, string>();
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.String, service_line, true));

            DataSet dsClaimServiceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
            if (dsClaimServiceLineDetails != null && Helper.HasRows(dsClaimServiceLineDetails))
            {
                parms.Add("Claim_ID", claimid);
                parms.Add("Service_Line", service_line);
                parms.Add("Procedure_Code", procedureCode.ToUpper());
                parms.Add("Status", status);
                parms.Add("Date_of_Service", Convert.ToDateTime(fromDOB).ToString("MM/dd/yyyy"));
                if (!string.IsNullOrEmpty(toDOB))
                    parms.Add("To_Date_of_Service", Convert.ToDateTime(toDOB).ToString("MM/dd/yyyy"));
                parms.Add("Modifier1", modifierDentalFirst.ToUpper());
                parms.Add("Modifier2", modifierDentalSecond.ToUpper());
                parms.Add("Modifier3", modifierDentalThird.ToUpper());
                parms.Add("Modifier4", modifierDentalFourth.ToUpper());
                if (!string.IsNullOrEmpty(totalCharges))
                    parms.Add("Total_Charges", totalCharges);
                parms.Add("Line_Control_Number", lineControlNumber);
                if (!string.IsNullOrEmpty(Unit))
                    parms.Add("Billed_Units", Unit);
                parms.Add("Revenue_Code", revenueCode);
                parms.Add("Procedure_Type", procedureType);
                parms.Add("Final_EAPG", InsFinalEAPG);
                parms.Add("Unit_of_Measurement", UnitOfMeasurement);
                parms.Add("Payment_Action", InsPaymentAction);
                parms.Add("Non_Covered_Charges", nonCoveredCharges);
                if (!string.IsNullOrEmpty(paidAmt))
                    parms.Add("Paid_Amount", paidAmt);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                if (!string.IsNullOrEmpty(InsTotalAmountPaid))
                {
                    parms.Add("Total_Amount_Paid", InsTotalAmountPaid);
                }
                if (!string.IsNullOrEmpty(InsTotalAmountBilled))
                {
                    parms.Add("Total_Amount_Billed", InsTotalAmountBilled);
                }

                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Service_Details", parms);
            }
            return Ok(true);
        }

        [HttpDelete]
        [Route("DeleteDiagnosisCode")]
        public IActionResult DeleteDiagnosisCode(string hdnClaimIdDiag = "", string claimDiag_Info_Id = "", string hdnClaimType_Diag = "")
        {
            List<DiagnosisPanel> diagnosispanelData = new List<DiagnosisPanel>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Diagnosis_Information", "Claims_Diagnosis_Information_ID", Int32.Parse(claimDiag_Info_Id));
            }
            catch { }

            DataSet bindGrid = Claims.GetDiagnosisData(hdnClaimIdDiag, hdnClaimType_Diag);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DiagnosisPanel DiagPaneldetails = new DiagnosisPanel();
                    DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                    DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                    DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                    string PresentOnAdmission = "";
                    if (dr["Present_On_Admission"].ToString().Equals("W"))
                    {
                        PresentOnAdmission = "Not Applicable";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("N"))
                    {
                        PresentOnAdmission = "No";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                    {
                        PresentOnAdmission = "Yes";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("U"))
                    {
                        PresentOnAdmission = "Unknown";
                    }
                    else
                    {
                        PresentOnAdmission = dr["Present_On_Admission"].ToString();
                    }
                    DiagPaneldetails.PreasentOnAdmission = PresentOnAdmission;
                    DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToString();
                    DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                    DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                    diagnosispanelData.Add(DiagPaneldetails);
                }
            }
            return Ok(diagnosispanelData);
        }

        [HttpDelete]
        [Route("DeleteProviderNote")]
        public IActionResult DeleteProviderNote(string Claimtype = "", string claimId = "", string ProviderNoteID = "")
        {
            int index = 0;
            if (Claimtype == CON.ClaimsType.Dental)
            {
                index = Convert.ToInt32(ProviderNoteID);
            }
            else if (Claimtype == CON.ClaimsType.Institutional)
            {
                index = Convert.ToInt32(ProviderNoteID);
            }
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Providers_Note", "Claims_Providers_Note_ID", index);
            }
            catch { }

            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype, 0);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);
        }

        [HttpDelete]
        [Route("DeleteProviderBillingNote")]
        public IActionResult DeleteProviderBillingNote(string Claimtype = "", string claimId = "", string ProviderNoteID = "")
        {
            int index = 0;
            if (Claimtype == CON.ClaimsType.Dental)
            {
                index = Convert.ToInt32(ProviderNoteID);
            }
            else if (Claimtype == CON.ClaimsType.Institutional)
            {
                index = Convert.ToInt32(ProviderNoteID);
            }
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Providers_Note", "Claims_Providers_Note_ID", index);
            }
            catch { }

            DataSet bindGrid = Claims.GetProviderNotes(claimId, Claimtype, 1);
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (Claimtype != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();
                    providerNotePanelData.Add(providerNote);
                }
            }
            return Ok(providerNotePanelData);
        }

        [HttpDelete]
        [Route("DeleteAmbulanceDropOffCode")]
        public IActionResult DeleteAmbulanceDropOffCode(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = "", string claimid = "")
        {
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Ambulance_Pick_Up_Drop_Off_Location", "Claims_Ambulance_Pick_Up_Drop_Off_Location_ID", Convert.ToInt32(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID));
            }
            catch { }

            List<AmbulanceDropOffPanel> AmbulanceDropOff = new List<AmbulanceDropOffPanel>();
            DataSet bindGrid = Claims.GetAmbulanceDropOff(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AmbulanceDropOffPanel AmbulanceDropOffInd = new AmbulanceDropOffPanel();
                    AmbulanceDropOffInd.serviceLine = dr["Service_Line"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                    AmbulanceDropOffInd.pickUpCity = dr["Pick_Up_City"].ToString();
                    AmbulanceDropOffInd.pickUpState = dr["Pick_Up_State"].ToString();
                    AmbulanceDropOffInd.pickUpZip = dr["Pick_Up_ZIP"].ToString();
                    AmbulanceDropOffInd.dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                    AmbulanceDropOffInd.dropOffLocationCity = dr["Drop_Off_City"].ToString();
                    AmbulanceDropOffInd.dropOffLocationState = dr["Drop_Off_State"].ToString();
                    AmbulanceDropOffInd.dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                    AmbulanceDropOffInd.Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                    AmbulanceDropOffInd.Claim_ID = dr["Claim_ID"].ToString();
                    AmbulanceDropOff.Add(AmbulanceDropOffInd);
                }
            }
            return Ok(AmbulanceDropOff);
        }

        [HttpPut]
        [Route("BindDiagCode")]
        public IActionResult BindDiagCode(string hdnClaimIdDiag = "", string hdnClaimType_Diag = "")
        {

            List<DiagnosisPanel> diagnosispanelData = new List<DiagnosisPanel>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Clear();
            var data = string.Empty;
            DataSet bindGrid = Claims.GetDiagnosisData(hdnClaimIdDiag, hdnClaimType_Diag);
            DataTable dt = new DataTable();
            if (bindGrid.Tables.Count > 0)
            {
                dt = bindGrid.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DiagnosisPanel DiagPaneldetails = new DiagnosisPanel();
                        DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                        DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                        DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                        string PresentOnAdmission = "";
                        if (dr["Present_On_Admission"].ToString().Equals("W"))
                        {
                            PresentOnAdmission = "Not Applicable";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("N"))
                        {
                            PresentOnAdmission = "No";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                        {
                            PresentOnAdmission = "Yes";
                        }
                        else if (dr["Present_On_Admission"].ToString().Equals("U"))
                        {
                            PresentOnAdmission = "Unknown";
                        }
                        else
                        {
                            PresentOnAdmission = dr["Present_On_Admission"].ToString();
                        }
                        DiagPaneldetails.PreasentOnAdmission = PresentOnAdmission;
                        DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToString();
                        DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                        DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                        diagnosispanelData.Add(DiagPaneldetails);
                    }
                }
            }

            return Ok(diagnosispanelData);
        }

        [HttpDelete]
        [Route("DeleteClaimServiceLine")]
        public IActionResult DeleteClaimServiceLine(string service_line = "", string claimid = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, false));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, service_line, false));
            DataAccess.ExecuteStoredProcedure("usp_deleteClaims_ServiceLine_Details", parameters);

            return Ok(true);
        }

        [HttpDelete]
        [Route("DeleteProfessionalserviceDetailCode")]
        public IActionResult DeleteProfessionalserviceDetailCode(string service_line = "", string claimid = "")
        {

            Dictionary<string, string> parms = new Dictionary<string, string>
            {
                { "Service_Line", Convert.ToString(service_line) },
                { "Claim_ID", Convert.ToString(claimid) }
            };

            MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsDataWithParams("Claims_Service_Details", parms);

            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.ProfessionalServiceDetail> ProfessionalServiceData = new List<PDMSRestServices.Models.ProfessionalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.ProfessionalServiceDetail professionalServiceDetail = new PDMSRestServices.Models.ProfessionalServiceDetail();
                    professionalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    professionalServiceDetail.plc_service = dr["plc_service"].ToString();
                    professionalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    professionalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    professionalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    professionalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    professionalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    professionalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    professionalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    professionalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    professionalServiceDetail.service_line = dr["Service_Line"].ToString();


                    string specId = dr["Claims_Service_Details_ID"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Claims_Service_Details_ID", DbType.Int32, specId, true));
                        DataAccess.ExecuteStoredProcedure("updateClaims_Service_Details", parametera, "Claims_Service_Details");
                        professionalServiceDetail.service_line = serviceLineUpdate;
                    }
                    ProfessionalServiceData.Add(professionalServiceDetail);
                }
            }


            Claims.updateservicelineTooThSurface(claimid);
            Claims.updateAdditionalProviderInfoServiceDetail(claimid, "0");
            Claims.updateOtherPayerPaidAmountServiceDetail(claimid, "0");
            Claims.updateOtherPayerAdjustmentServiceDetail(claimid, "0");
            Claims.updateNDCdetails(claimid, "0");
            return Ok(ProfessionalServiceData);
        }

        [HttpDelete]
        [Route("DeleteDentalserviceDetailCode")]
        public IActionResult DeleteDentalserviceDetailCode(string Claim_service_Id = "", string claimid = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claims_Service_Details_ID", Claim_service_Id);
            List<SqlParameter> parameters1 = new List<SqlParameter>();

            foreach (KeyValuePair<string, string> pair in parms)
            {
                if (pair.Value == null)
                {
                    parameters1.Add(new SqlParameter(pair.Key, DBNull.Value));
                }
                else
                {
                    parameters1.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }

            DataAccess.ExecuteStoredProcedure("deleteClaims_Service_Details", parameters1);
            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.DentalServiceDetail> dentalServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            parameters.Clear();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail dentalServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();
                    dentalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    dentalServiceDetail.plc_service = dr["plc_service"].ToString();
                    dentalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    dentalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    dentalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    dentalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    dentalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    dentalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    dentalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    string specId = dr["Claims_Service_Details_ID"].ToString();
                    dentalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    dentalServiceDetail.service_Line = dr["Service_Line"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Claims_Service_Details_ID", DbType.Int32, specId, true));
                        DataAccess.ExecuteStoredProcedure("updateClaims_Service_Details", parametera, "Claims_Service_Details");
                        dentalServiceDetail.service_Line = serviceLineUpdate;
                    }
                    dentalServiceData.Add(dentalServiceDetail);
                }
            }
            Claims.updateservicelineTooThSurface(claimid);
            Claims.updateAdditionalProviderInfoServiceDetail(claimid, "0");
            Claims.updateOtherPayerPaidAmountServiceDetail(claimid, "0");
            Claims.updateOtherPayerAdjustmentServiceDetail(claimid, "0");

            return Ok(dentalServiceData);

        }

        [HttpPut]
        [Route("updateNDCdetails")]
        public IActionResult updateNDCdetails(string claimid = "", string claimtype = "")
        {
            Claims.updateNDCdetails(claimid, claimtype);
            return Ok(true);
        }

        [HttpPut]
        [Route("updateOtherPayerAdjustmentServiceDetail")]
        public IActionResult updateOtherPayerAdjustmentServiceDetail(string claimid = "", string claimtype = "")
        {
            Claims.updateOtherPayerAdjustmentServiceDetail(claimid, claimtype);
            return Ok(true);
        }

        [HttpPut]
        [Route("updateOtherPayerPaidAmountServiceDetail")]
        public IActionResult updateOtherPayerPaidAmountServiceDetail(string claimid = "", string claimtype = "")
        {
            Claims.updateOtherPayerPaidAmountServiceDetail(claimid, claimtype);
            return Ok(true);
        }

        [HttpPut]
        [Route("updateAdditionalProviderInfoServiceDetail")]
        public IActionResult updateAdditionalProviderInfoServiceDetail(string claimid = "", string claimtype = "")
        {
            Claims.updateAdditionalProviderInfoServiceDetail(claimid, claimtype);
            return Ok(true);
        }

        [HttpPut]
        [Route("updateservicelineTooThSurface")]
        public IActionResult updateservicelineTooThSurface(string claimid = "")
        {
            Claims.updateservicelineTooThSurface(claimid);
            return Ok(true);
        }

        [HttpGet]
        [Route("getOtherPayerAdjSerDetailBind")]
        public IActionResult getOtherPayerAdjSerDetailBind(string ClaimId = "", string ClaimType = "")
        {
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjustmentServiceDetailData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfoServiceDetail>();

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfoServiceDetail> Adjustmentdetails = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfoServiceDetail>();
            Adjustmentdetails = Claims.SelectOtherPayerAdjustmentServiceDetail(ClaimId);
            OtherPayerAdjustmentServiceDetailData = Adjustmentdetails;
            return Ok(OtherPayerAdjustmentServiceDetailData);
        }

        [HttpGet]
        [Route("getHeaderOtherPayerBind")]
        public IActionResult GetHeaderOtherPayerBind(string ClaimId = "")
        {
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> OtherPayerAdjustmentServiceDetailData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> Adjustmentdetails = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();
            Adjustmentdetails = Claims.DisplayInstitutionalHeaderAdjustmentPanel(ClaimId);
            OtherPayerAdjustmentServiceDetailData = Adjustmentdetails;
            return Ok(OtherPayerAdjustmentServiceDetailData);

        }

        [HttpGet]
        [Route("GetNDCDetailsData")]
        public IActionResult GetNDCDetailsData(string ClaimID = "", string ClaimType = "")
        {
            List<NDCDetails> NDCDetailsData = new List<NDCDetails>();
            List<NDCDetails> details = new List<NDCDetails>();
            details = Claims.SelectNDCDetails(ClaimID);
            NDCDetailsData = details;
            return Ok(NDCDetailsData);
        }

        [HttpDelete]
        [Route("DeleteInstiserviceDetailCode")]
        public IActionResult DeleteInstiserviceDetailCode(string Claim_Id = "", string service_Line = "")
        {
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();

            Dictionary<string, string> parms = new Dictionary<string, string>
        {
                { "Service_Line",  service_Line},
                { "Claim_ID", Convert.ToString(Claim_Id) }
          };

            MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsDataWithParams("Claims_Service_Details", parms);


            var data = string.Empty;

            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parametersa = new List<SqlParameter>();
            parametersa.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_Id, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parametersa, "Claims_Service_Details");
            DataTable dt = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail InstiServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();

                    InstiServiceDetail.revenue_Code = dr["Revenue_Code"].ToString();
                    InstiServiceDetail.service_Line = dr["Service_Line"].ToString();
                    InstiServiceDetail.cde_proc = dr["Procedure_Code"].ToString();
                    InstiServiceDetail.procedure_type = dr["Procedure_Type"].ToString();
                    InstiServiceDetail.unit = dr["Billed_Units"].ToString();
                    InstiServiceDetail.unit_of_measurement = dr["Unit_Of_Measurement"].ToString();
                    InstiServiceDetail.fromDOS = Convert.ToDateTime(dr["Date_of_Service"].ToString());
                    if (!string.IsNullOrEmpty(dr["To_Date_Of_Service"].ToString()))
                    {
                        InstiServiceDetail.toDOS = Convert.ToDateTime(dr["To_Date_Of_Service"].ToString());
                    }
                    InstiServiceDetail.total_charges = dr["Total_Charges"].ToString();
                    InstiServiceDetail.paid_amount = dr["Paid_Amount"].ToString();
                    InstiServiceDetail.status = dr["Status"].ToString();

                    InstiServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    InstiServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    InstiServiceDetail.total_amount_billed = dr["Total_Amount_Billed"].ToString();
                    InstiServiceDetail.total_amount_paid = dr["Total_Amount_Paid"].ToString();
                    string specId = dr["Claims_Service_Details_ID"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Claims_Service_Details_ID", DbType.Int32, specId, true));
                        DataAccess.ExecuteStoredProcedure("updateClaims_Service_Details", parametera, "Claims_Service_Details");
                        InstiServiceDetail.service_Line = serviceLineUpdate;
                    }

                    InstServiceData.Add(InstiServiceDetail);
                }
            }
            string claimid = Claim_Id;
            Claims.updateservicelineTooThSurface(claimid);
            Claims.updateAdditionalProviderInfoServiceDetail(claimid, "0");
            Claims.updateOtherPayerPaidAmountServiceDetail(claimid, "0");
            Claims.updateOtherPayerAdjustmentServiceDetail(claimid, "0");
            Claims.updateNDCdetails(claimid, "0");

            return Ok(InstServiceData);

        }

        [HttpGet]
        [Route("ProfessionalServiceDetailBindGrid")]
        public IActionResult ProfessionalServiceDetailBindGrid(string claimid = "")
        {

            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.ProfessionalServiceDetail> professionalServiceData = new List<PDMSRestServices.Models.ProfessionalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.ProfessionalServiceDetail professionalServiceDetail = new PDMSRestServices.Models.ProfessionalServiceDetail();
                    professionalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    professionalServiceDetail.plc_service = dr["plc_service"].ToString();
                    professionalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    professionalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    professionalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    professionalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    professionalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    professionalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    professionalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    professionalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    professionalServiceDetail.service_line = dr["Service_Line"].ToString();
                    professionalServiceData.Add(professionalServiceDetail);
                }
            }

            return Ok(professionalServiceData);
        }

        [HttpGet]
        [Route("DentalServiceDetailBindGrid")]
        public IActionResult DentalServiceDetailBindGrid(string claimid = "")
        {
            var data = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
            DataSet bindGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            List<PDMSRestServices.Models.DentalServiceDetail> dentalServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail dentalServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();
                    dentalServiceDetail.cde_proc = dr["cde_proc"].ToString();
                    dentalServiceDetail.plc_service = dr["plc_service"].ToString();
                    dentalServiceDetail.bil_unt = dr["bil_unt"].ToString();
                    dentalServiceDetail.pad_unt = dr["pad_unt"].ToString();
                    dentalServiceDetail.ServiceDate = Convert.ToDateTime(dr["ServiceDate"].ToString());
                    dentalServiceDetail.cde_clm_chrge = dr["cde_clm_chrge"].ToString();
                    dentalServiceDetail.pad_amnt = dr["pad_amnt"].ToString();
                    dentalServiceDetail.cde_clm_status = dr["cde_clm_status"].ToString();
                    dentalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    dentalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    dentalServiceDetail.service_Line = dr["Service_Line"].ToString();

                    dentalServiceData.Add(dentalServiceDetail);
                }
            }

            return Ok(dentalServiceData);
        }

        [HttpGet]
        [Route("AmbulanceServiceBindGrid")]
        public IActionResult AmbulanceServiceBindGrid(string claimid = "")
        {
            List<AmbulanceDropOffPanel> AmbulanceDropOff = new List<AmbulanceDropOffPanel>();

            DataSet bindGrid = Claims.GetAmbulanceDropOff(claimid);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AmbulanceDropOffPanel AmbulanceDropOffInd = new AmbulanceDropOffPanel();
                    AmbulanceDropOffInd.serviceLine = dr["Service_Line"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                    AmbulanceDropOffInd.pickUpCity = dr["Pick_Up_City"].ToString();
                    AmbulanceDropOffInd.pickUpState = dr["Pick_Up_State"].ToString();
                    AmbulanceDropOffInd.pickUpZip = dr["Pick_Up_ZIP"].ToString();
                    AmbulanceDropOffInd.dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                    AmbulanceDropOffInd.dropOffLocationCity = dr["Drop_Off_City"].ToString();
                    AmbulanceDropOffInd.dropOffLocationState = dr["Drop_Off_State"].ToString();
                    AmbulanceDropOffInd.dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                    AmbulanceDropOffInd.Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                    AmbulanceDropOffInd.Claim_ID = dr["Claim_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    AmbulanceDropOff.Add(AmbulanceDropOffInd);
                }
            }
            return Ok(AmbulanceDropOff);

        }

        [HttpGet]
        [Route("CheckModifierText")]
        public IActionResult CheckModifierText(string variable = "")
        {
            var data = string.Empty;
            DataTable dt = new DataTable();
            dt = LookupTableController.GetProcedureModifierDetail(variable, "");
            if (ObjectControllerHelper.HasRows(dt))
            {
                data = "true";
            }
            else { data = "false"; }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetIndAmbulanceInfo")]
        public IActionResult GetIndAmbulanceInfo(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = "")
        {
            List<AmbulanceDropOffPanel> AmbulanceDropOff = new List<AmbulanceDropOffPanel>();

            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetIndAmbulanceDropOffCode(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AmbulanceDropOffPanel AmbulanceDropOffInd = new AmbulanceDropOffPanel();
                    AmbulanceDropOffInd.serviceLine = dr["Service_Line"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine1 = dr["Pick_Up_Address_Line1"].ToString();
                    AmbulanceDropOffInd.pickUpAddressLine2 = dr["Pick_Up_Address_Line2"].ToString();
                    AmbulanceDropOffInd.pickUpCity = dr["Pick_Up_City"].ToString();
                    AmbulanceDropOffInd.pickUpState = dr["Pick_Up_State"].ToString();
                    AmbulanceDropOffInd.pickUpZip = dr["Pick_Up_ZIP"].ToString();
                    AmbulanceDropOffInd.dropOffLocationName = dr["Drop_Off_Location_Name"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline1 = dr["Drop_Off_Address_Line1"].ToString();
                    AmbulanceDropOffInd.dropOffLocationAddressline2 = dr["Drop_Off_Address_Line2"].ToString();
                    AmbulanceDropOffInd.dropOffLocationCity = dr["Drop_Off_City"].ToString();
                    AmbulanceDropOffInd.dropOffLocationState = dr["Drop_Off_State"].ToString();
                    AmbulanceDropOffInd.dropOffLocationZip = dr["Drop_Off_Zip"].ToString();
                    AmbulanceDropOffInd.Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = dr["Claims_Ambulance_Pick_Up_Drop_Off_Location_ID"].ToString();
                    AmbulanceDropOffInd.Claim_ID = dr["Claim_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    AmbulanceDropOff.Add(AmbulanceDropOffInd);
                }
            }
            return Ok(AmbulanceDropOff);
        }


        [HttpGet]
        [Route("GetIndDiagnosisCode")]
        public IActionResult GetIndDiagnosisCode(string claim_id = "", string claim_diag_info_id = "", string claim_type = "")
        {
            List<DiagnosisPanel> diagnosispanelData = new List<DiagnosisPanel>();

            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetIndDiagCode(claim_id, claim_diag_info_id, claim_type);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DiagnosisPanel DiagPaneldetails = new DiagnosisPanel();
                    DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                    DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                    DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                    string PresentOnAdmission = "";
                    if (dr["Present_On_Admission"].ToString().Equals("W"))
                    {
                        PresentOnAdmission = "Not Applicable";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("N"))
                    {
                        PresentOnAdmission = "No";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("Y"))
                    {
                        PresentOnAdmission = "Yes";
                    }
                    else if (dr["Present_On_Admission"].ToString().Equals("U"))
                    {
                        PresentOnAdmission = "Unknown";
                    }
                    else
                    {
                        PresentOnAdmission = dr["Present_On_Admission"].ToString();
                    }
                    DiagPaneldetails.PreasentOnAdmission = PresentOnAdmission;
                    DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToString();
                    DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                    DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    diagnosispanelData.Add(DiagPaneldetails);
                }
            }
            return Ok(diagnosispanelData);

        }

        [HttpGet]
        [Route("GetIndProviderNote")]
        public IActionResult GetIndProviderNote(string Claim_ID = "", string providerNoteID = "", string claim_type = "")
        {
            List<providerNotes> providerNotePanelData = new List<providerNotes>();
            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetIndProviderNote(Claim_ID, providerNoteID, claim_type);

            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    providerNotes providerNote = new providerNotes();
                    providerNote.claimID = dr["Claim_ID"].ToString();
                    providerNote.notes = dr["Note"].ToString();
                    providerNote.providerNoteID = dr["Claims_Providers_Note_ID"].ToString();
                    if (claim_type != "0") providerNote.noteRefCode = dr["NoteReferenceCode"].ToString();

                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    providerNotePanelData.Add(providerNote);
                }
            }

            return Ok(providerNotePanelData);
        }

        [HttpGet]
        [Route("GetIndProfessionalServiceDetail")]
        public IActionResult GetIndProfessionalServiceDetail(string Claim_service_Id = "", string claim_id = "", string serviceline = "")
        {

            DataSet serviceLineDetails = new DataSet();
            DataTable claimsDataTable = new DataTable();
            List<PDMSRestServices.Models.ProfessionalServiceDetail> professionalServiceData = new List<PDMSRestServices.Models.ProfessionalServiceDetail>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, serviceline, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");

            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = serviceLineDetails.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.ProfessionalServiceDetail ProfessionalServiceDetail = new PDMSRestServices.Models.ProfessionalServiceDetail();
                    ProfessionalServiceDetail.cde_proc = dr["Procedure_Code"].ToString();
                    ProfessionalServiceDetail.plc_service = dr["Place_of_Service"].ToString();
                    ProfessionalServiceDetail.bil_unt = dr["Billed_Units"].ToString();
                    ProfessionalServiceDetail.pad_unt = dr["Paid_Units"].ToString();
                    ProfessionalServiceDetail.ServiceDate = Convert.ToDateTime(dr["Date_of_Service"].ToString());
                    ProfessionalServiceDetail.cde_clm_chrge = dr["Charges"].ToString();
                    ProfessionalServiceDetail.pad_amnt = dr["Paid_Amount"].ToString();
                    ProfessionalServiceDetail.cde_clm_status = dr["Status"].ToString();
                    ProfessionalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    ProfessionalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    ProfessionalServiceDetail.mdf_first = dr["Modifier1"].ToString();
                    ProfessionalServiceDetail.mdf_secnd = dr["Modifier2"].ToString();
                    ProfessionalServiceDetail.mdf_thrd = dr["Modifier3"].ToString();
                    ProfessionalServiceDetail.mdf_forth = dr["Modifier4"].ToString();
                    ProfessionalServiceDetail.digno_first = dr["Diagnosis_Pointer1"].ToString();
                    ProfessionalServiceDetail.digno_sec = dr["Diagnosis_Pointer2"].ToString();
                    ProfessionalServiceDetail.digno_third = dr["Diagnosis_Pointer3"].ToString();
                    ProfessionalServiceDetail.digno_forth = dr["Diagnosis_Pointer4"].ToString();
                    ProfessionalServiceDetail.unt_of_measurment = dr["Unit_Of_Measurement"].ToString();
                    ProfessionalServiceDetail.Referred_EPSDT_Service = dr["Referred_EPSDT_Service"].ToString();
                    ProfessionalServiceDetail.Family_Planning = dr["Family_Planning"].ToString();
                    ProfessionalServiceDetail.Emergency = dr["Emergency"].ToString();
                    ProfessionalServiceDetail.Final_EAPG = dr["Final_EAPG"].ToString();
                    ProfessionalServiceDetail.Payment_Action = dr["Payment_Action"].ToString();
                    ProfessionalServiceDetail.Total_Charges = dr["Total_Charges"].ToString();
                    ProfessionalServiceDetail.Cert_Revision = dr["Cert_Revision"].ToString();
                    ProfessionalServiceDetail.dme_cert_type = dr["DME_Cert_Type"].ToString();
                    ProfessionalServiceDetail.dme_duration = dr["DME_Duration"].ToString();
                    ProfessionalServiceDetail.prior_auth = dr["Prior_Authorization_Number"].ToString();
                    ProfessionalServiceDetail.ref_num = dr["Referral_Number"].ToString();
                    ProfessionalServiceDetail.Line_Control_Number = dr["Line_Control_Number"].ToString();


                    professionalServiceData.Add(ProfessionalServiceDetail);
                }
            }

            return Ok(professionalServiceData);
        }

        [HttpGet]
        [Route("CopyServiceDetail")]
        public IActionResult CopyServiceDetail(string Claim_service_Id = "", string claim_id = "", string serviceline = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, serviceline, true));
            DataAccess.ExecuteStoredProcedure("usp_copyClaims_Service_Details", parameters);
            return Ok(true);
        }

        [HttpGet]
        [Route("GetIndDentalServiceDetail")]
        public IActionResult GetIndDentalServiceDetail(string Claim_service_Id = "", string claim_id = "", string serviceline = "")
        {

            DataSet serviceLineDetails = new DataSet();
            DataTable claimsDataTable = new DataTable();
            List<PDMSRestServices.Models.DentalServiceDetail> dentalServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, serviceline, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");

            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = serviceLineDetails.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail dentalServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();
                    //DiagPaneldetails.sequence = dr["seq_Desc"].ToString();
                    //DiagPaneldetails.DiagnosisCode = dr["diag_code"].ToString();
                    //DiagPaneldetails.ICDVersion = dr["ICDVersion"].ToString();
                    //DiagPaneldetails.PreasentOnAdmission = dr["Present_On_Admission"].ToString();
                    //DiagPaneldetails.DianosisCodeDescription = dr["diagnosisDescription"].ToSetValueCodeDetailstring();
                    //DiagPaneldetails.Claims_Diagnosis_ID = dr["Claims_Diagnosis_Information_ID"].ToString();
                    //DiagPaneldetails.Claim_ID = dr["Claim_ID"].ToString();
                    // Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    dentalServiceDetail.cde_proc = dr["Procedure_Code"].ToString();
                    dentalServiceDetail.plc_service = dr["Place_of_Service"].ToString();
                    dentalServiceDetail.bil_unt = dr["Billed_Units"].ToString();
                    dentalServiceDetail.pad_unt = dr["Paid_Units"].ToString();
                    dentalServiceDetail.ServiceDate = Convert.ToDateTime(dr["Date_of_Service"].ToString());
                    dentalServiceDetail.cde_clm_chrge = dr["Charges"].ToString();
                    dentalServiceDetail.pad_amnt = dr["Paid_Amount"].ToString();
                    dentalServiceDetail.cde_clm_status = dr["Status"].ToString();
                    dentalServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    dentalServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    dentalServiceDetail.line_ctr_num = dr["Line_Control_Number"].ToString();
                    dentalServiceDetail.prior_auth = dr["Prior_Authorization_Number"].ToString();
                    dentalServiceDetail.ref_num = dr["Referral_Number"].ToString();
                    dentalServiceDetail.mdf_first = dr["Modifier1"].ToString();
                    dentalServiceDetail.mdf_secnd = dr["Modifier2"].ToString();
                    dentalServiceDetail.mdf_thrd = dr["Modifier3"].ToString();
                    dentalServiceDetail.mdf_forth = dr["Modifier4"].ToString();
                    dentalServiceDetail.digno_first = dr["Diagnosis_Pointer1"].ToString();
                    dentalServiceDetail.digno_sec = dr["Diagnosis_Pointer2"].ToString();
                    dentalServiceDetail.digno_third = dr["Diagnosis_Pointer3"].ToString();
                    dentalServiceDetail.digno_forth = dr["Diagnosis_Pointer4"].ToString();
                    dentalServiceDetail.orl_cvt_first = dr["Oral_Cavity1"].ToString();
                    dentalServiceDetail.orl_cvt_sec = dr["Oral_Cavity2"].ToString();
                    dentalServiceDetail.orl_cvt_third = dr["Oral_Cavity3"].ToString();
                    dentalServiceDetail.orl_cvt_forth = dr["Oral_Cavity4"].ToString();
                    dentalServiceDetail.orl_cvt_fifth = dr["Oral_Cavity5"].ToString();
                    dentalServiceDetail.prosthesis_cd = dr["Prosthesis_Crown_InlayCode"].ToString();


                    dentalServiceData.Add(dentalServiceDetail);
                }
            }

            return Ok(dentalServiceData);
        }

        [HttpGet]
        [Route("GetIndDentalOtherPayerDetail")]
        public IActionResult GetIndDentalOtherPayerDetail(string otherpayerInfoID = "")
        {
            DataSet otherPayerDetails = new DataSet();
            DataTable claimsDataTable = new DataTable();
            List<OtherPayerDetail> dentalOtherPayerData = new List<OtherPayerDetail>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(SqlParms.CreateParameter("Claims_Other_Payer_Information_ID", DbType.Int32, otherpayerInfoID, true));

            DataSet OtherPayerDetails = DataAccess.ExecuteStoredProcedure("usp_SelectIndClaims_Other_Payer_Information", parameters, "Claims_Other_Payer_Information");

            DataSet dsPaidAmont = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(otherpayerInfoID))
            {
                parms.Add("Other_Payer_Information_ID", otherpayerInfoID);
                dsPaidAmont = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("get_otherpayerpaidamount_onedit", parms);
            }

            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = OtherPayerDetails.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherPayerDetail otherPayerDetail = new OtherPayerDetail();
                    otherPayerDetail.otherpayerInfoID = dr["Claims_Other_Payer_Information_ID"].ToString();
                    otherPayerDetail.otherPayerName = dr["Other_Payer_Name"].ToString();
                    otherPayerDetail.healthPlanID = dr["Health_Plan_ID"].ToString();
                    otherPayerDetail.claimFilingIndicator = dr["Claim_Filing_Indicator"].ToString();
                    otherPayerDetail.payerResponsibilitySequence = dr["Payer_Responsibility_Sequence"].ToString();
                    otherPayerDetail.subscriberNumber = dr["Subscriber_Subscriber_Number"].ToString();
                    otherPayerDetail.policyNumber = dr["Policy_Number"].ToString();
                    otherPayerDetail.groupName = dr["Group_Name"].ToString();
                    otherPayerDetail.insuranceTypeCode = dr["Insurance_Type_Code"].ToString();
                    otherPayerDetail.patientRelationshipSuscriber = dr["Patient_to_Subscriber"].ToString();
                    otherPayerDetail.subscriberFirstName = dr["Subscriber_First_Name"].ToString();
                    otherPayerDetail.subscriberLastName = dr["Subscriber_Last_Name"].ToString();
                    otherPayerDetail.subscriberMiddleName = dr["Subscriber_Middle_Name"].ToString();
                    otherPayerDetail.subscriberAddressLine1 = dr["Subscriber_AddressLine1"].ToString();
                    otherPayerDetail.subscriberAddressLine2 = dr["Subscriber_AddressLine2"].ToString();
                    otherPayerDetail.subscriberCity = dr["Subscriber_City"].ToString();
                    // dte_last_svc
                    otherPayerDetail.subscriberState = dr["Subscriber_State"].ToString();
                    otherPayerDetail.subscriberZip = dr["Subscriber_ZIP"].ToString();
                    otherPayerDetail.claimAdjudicationLevel = dr["Claim_Adjudication_Level"].ToString();
                    otherPayerDetail.claimNumber = dr["Claim_Number"].ToString();
                    otherPayerDetail.paidDate = dr["Paid_Date"].ToString();
                    if (dr["Claim_Adjudication_Level"] != DBNull.Value)
                    {
                        if (dr["Claim_Adjudication_Level"].ToString() != "") //&& Convert.ToInt32(dr["Claim_Adjudication_Level"]) == CON.ClaimsAdjudicationLevel.Header)
                            otherPayerDetail.paidAmount = dr["Paid_Amount"].ToString();
                    }
                    else
                    {
                        if (Helper.HasRows(dsPaidAmont))
                        {
                            foreach (DataRow drpaidamt in dsPaidAmont.Tables[0].Rows)
                            {
                                if (dr["Health_Plan_ID"].ToString() == drpaidamt["Health_Plan_ID"].ToString())
                                {
                                    otherPayerDetail.paidAmount = drpaidamt["TotalPaidAmount"].ToString();
                                }
                            }
                        }
                    }
                    otherPayerDetail.nonCoveredAmoount = dr["Total_Non_Covered_Amount"].ToString();

                    dentalOtherPayerData.Add(otherPayerDetail);
                }
            }

            return Ok(dentalOtherPayerData);
        }

        [HttpGet]
        [Route("GetIndInstiServiceDetail")]
        public IActionResult GetIndInstiServiceDetail(string Claim_service_Id = "", string claim_id = "", string service_Line = "")
        {
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            DataSet serviceLineDetails = new DataSet();
            if (service_Line == "01" || service_Line == "02" || service_Line == "03" || service_Line == "04" || service_Line == "05" || service_Line == "06" || service_Line == "07" || service_Line == "08" || service_Line == "09")
            { service_Line.Remove(0, 0); }
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, service_Line, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
            DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;

            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = serviceLineDetails.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.DentalServiceDetail InstiServiceDetail = new PDMSRestServices.Models.DentalServiceDetail();
                    InstiServiceDetail.revenue_Code = dr["Revenue_Code"].ToString();
                    InstiServiceDetail.service_Line = dr["Service_Line"].ToString();
                    InstiServiceDetail.cde_proc = dr["Procedure_Code"].ToString();
                    InstiServiceDetail.procedure_type = dr["Procedure_Type"].ToString();
                    InstiServiceDetail.unit = dr["Billed_Units"].ToString();
                    InstiServiceDetail.unit_of_measurement = dr["Unit_Of_Measurement"].ToString();
                    InstiServiceDetail.fromDOS = Convert.ToDateTime(dr["Date_of_Service"].ToString());
                    if (!string.IsNullOrEmpty(dr["To_Date_Of_Service"].ToString()))
                    {
                        InstiServiceDetail.toDOS = Convert.ToDateTime(dr["To_Date_Of_Service"].ToString());
                    }
                    InstiServiceDetail.total_charges = dr["Total_Charges"].ToString();
                    InstiServiceDetail.paid_amount = dr["Paid_Amount"].ToString();
                    InstiServiceDetail.status = dr["Status"].ToString();
                    InstiServiceDetail.Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    InstiServiceDetail.Claim_Id = dr["Claim_ID"].ToString();
                    InstiServiceDetail.mdf_first = dr["Modifier1"].ToString();
                    InstiServiceDetail.mdf_secnd = dr["Modifier2"].ToString();
                    InstiServiceDetail.mdf_thrd = dr["Modifier3"].ToString();
                    InstiServiceDetail.mdf_forth = dr["Modifier4"].ToString();
                    InstiServiceDetail.line_ctr_num = dr["Line_Control_Number"].ToString();
                    InstiServiceDetail.final_EAPG = dr["Final_EAPG"].ToString();
                    InstiServiceDetail.payment_Action = dr["Payment_Action"].ToString();
                    InstiServiceDetail.non_Covered_Charges = dr["Non_Covered_Charges"].ToString();
                    InstiServiceDetail.status = dr["Status"].ToString();

                    InstServiceData.Add(InstiServiceDetail);
                }
            }
            return Ok(InstServiceData);

        }

        [HttpGet]
        [Route("GetValueCodeDetails")]
        public IActionResult GetValueCodeDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("CLAIMS_VALUE_CODE", desc);
            parms.Add("CLAIMS_VALUE_CODE_DESC", val);
            var data = string.Empty;

            var ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("CLAIMS_VALUE_CODE", parms);
            List<valuecodedetails> list = new List<valuecodedetails>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                valuecodedetails Vdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Vdetails = new valuecodedetails(dr["CLAIMS_VALUE_CODE"].ToString(), dr["CLAIMS_VALUE_CODE_DESC"].ToString());
                    list.Add(Vdetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetConditionCodeDetails")]
        public IActionResult GetConditionCodeDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CLAIMS_CONDITION_CODE", DbType.String, val, true));
            parameters.Add(SqlParms.CreateParameter("CLAIMS_CONDITION_CODE_DESCRIPTION", DbType.String, desc, true));

            DataSet ds = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Condition_Code", parameters, "CLAIMS_CONDITION_CODE");

            List<conditioncodedetails> list = new List<conditioncodedetails>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                conditioncodedetails Cdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Cdetails = new conditioncodedetails(dr["CLAIMS_CONDITION_CODE"].ToString(), dr["CLAIMS_CONDITION_CODE_DESCRIPTION"].ToString());
                    list.Add(Cdetails);
                }
            }
            return Ok(list);

        }

        [HttpGet]
        [Route("GetOccurenceCodeDetails")]
        public IActionResult GetOccurenceCodeDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parms.Add("@CLAIMS_OCCURRENCE_CODE", val);
            parms.Add("@CLAIMS_OCCURRENCE_CODE_DESC", desc);
            var data = string.Empty;

            DataSet ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("CLAIMS_OCCURRENCE_CODE", parms);
            List<occerencecodedetails> list = new List<occerencecodedetails>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                occerencecodedetails Odetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Odetails = new occerencecodedetails(dr["CLAIMS_OCCURRENCE_CODE"].ToString(), dr["CLAIMS_OCCURRENCE_CODE_DESC"].ToString());
                    list.Add(Odetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetOccurenceDetails")]
        public IActionResult GetOccurenceDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet ds = LookupTableController.GetOccurreneceCode(val, desc);
            List<occerencedetails> list = new List<occerencedetails>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                occerencedetails Occurdetails;
                foreach (DataRow dr in dt.Rows)
                {
                    Occurdetails = new occerencedetails(dr["CLAIMS_OCCURRENCE_CODE"].ToString(), dr["CLAIMS_OCCURRENCE_CODE_DESC"].ToString());
                    list.Add(Occurdetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetICDProcedureCodeDetails")]
        public IActionResult GetICDProcedureCodeDetails(string desc = "", string val = "", string dropdownvalue = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataTable Icddt = LookupTableController.GetICDProcedureCode(val, desc, dropdownvalue);

            List<IcdProccodedetails> list = new List<IcdProccodedetails>();
            if (Icddt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Icddt;
                IcdProccodedetails IcdDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    IcdDetails = new IcdProccodedetails(dr["CLAIMS_ICD_PROCEDURE_CODE"].ToString(), dr["ICD_VERSION"].ToString(), dr["LONG_DESC"].ToString());
                    list.Add(IcdDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetHeaderAndOtherPayerDetail")]
        public IActionResult GetHeaderAndOtherPayerDetail(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            DataTable Reasondt = LookupTableController.GetCrcReasoneCode(val, desc);

            List<Reasoncode> list = new List<Reasoncode>();
            if (Reasondt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Reasondt;
                Reasoncode ReasonCodeDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    ReasonCodeDetails = new Reasoncode(dr["CLAIMS_REASON_CODE"].ToString(), dr["CLAIMS_REASON_CODE_DESC"].ToString());
                    list.Add(ReasonCodeDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetRevenueCodeDetails")]
        public IActionResult GetRevenueCodeDetails(string desc = "", string val = "")
        {
            DataTable Revendt = LookupTableController.GetRevenueCode(val, desc);

            List<Revencode> list = new List<Revencode>();
            if (Revendt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Revendt;
                Revencode RevenCodeDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    RevenCodeDetails = new Revencode(dr["RevenueCode"].ToString(), dr["RevenueDesc"].ToString());
                    list.Add(RevenCodeDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetSpecialtyTypes")]
        public IActionResult GetSpecialtyTypes(string medId = "", string npi = "")
        {
            DataTable rs = LookupTableController.GetSpecialtyTypes(medId, npi);

            List<RegSpecialtyType> list = new List<RegSpecialtyType>();
            if (rs.Rows.Count > 0)
            {
                RegSpecialtyType RegSpecialtyType;
                foreach (DataRow dr in rs.Rows)
                {
                    //int regId, int regSpecialtyId, int specialtyTypeId, string mmisSpecialtyTypeId
                    RegSpecialtyType = new RegSpecialtyType(Convert.ToInt32(dr["REG_ID"].ToString()), Convert.ToInt32(dr["REG_SPECIALTY_ID"].ToString()), 
                        Convert.ToInt32(dr["SPECIALTY_TYPE_ID"].ToString()), dr["MMIS_SPECIALTY_TYPE_ID"].ToString());
                    list.Add(RegSpecialtyType);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetCodeSetRule")]
        public IActionResult GetCodeSetRule(string providerTypeId = "", string specialtyTypeId = "", string code = "", string codeType = "", string claimsorPA = "Claims", string claimORPAType = "")
        {
            DataTable rs = LookupTableController.GetCodeSetRule(providerTypeId, specialtyTypeId, code, codeType, claimsorPA, claimORPAType);

            List<CodesetRule> list = new List<CodesetRule>();
            if (rs.Rows.Count > 0)
            {
                CodesetRule codesetRule;
                foreach (DataRow dr in rs.Rows)
                {
                    codesetRule = new CodesetRule(dr["CODE_ALLOWED"].ToString(), dr["ERROR_MESSAGE"].ToString());
                    list.Add(codesetRule);
                }
            }
            return Ok(list);
        }



        [HttpGet]
        [Route("GetTypeOfBillDetails")]
        public IActionResult GetTypeOfBillDetails(string desc = "", string val = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            DataSet dsTOB = MAXIMUS.Controllers.PDMS.LookupTableController.GetInstitutionalTypeOfBill(string.IsNullOrEmpty(val) ? "" : val, string.IsNullOrEmpty(desc) ? "" : desc);
            DataTable Procdt = dsTOB.Tables[0];
            List<TypeOfBill> list = new List<TypeOfBill>();
            if (Procdt.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = Procdt;
                TypeOfBill ProcCodeDetails;
                foreach (DataRow dr in dt.Rows)
                {
                    ProcCodeDetails = new TypeOfBill(dr["CLAIMS_TYPE_OF_BILL_Code"].ToString(), dr["CLAIMS_TYPE_OF_BILL_DESC"].ToString());
                    list.Add(ProcCodeDetails);
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("GetClaimOccurenceDescription")]

        public IActionResult GetClaimOccurenceDescription(string desc = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("CLAIMS_OCCURRENCE_CODE", desc);
            var data = JsonConvert.SerializeObject(string.Empty);

            if (!string.IsNullOrWhiteSpace(desc))
            {
                var ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("CLAIMS_OCCURRENCE_CODE", parms);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetClaimOccurenceInfoDescription")]
        public IActionResult GetClaimOccurenceInfoDescription(string desc = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            var data = JsonConvert.SerializeObject(string.Empty);
            if (!string.IsNullOrWhiteSpace(desc))
            {
                var ds = LookupTableController.GetOccurreneceCode(desc, "");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
            return Ok(data);
        }


        [HttpGet]
        [Route("GetClaimConditionCodeDescription")]
        public IActionResult GetClaimConditionCodeDescription(string desc = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("CLAIMS_CONDITION_CODE", desc);
            var data = string.Empty;

            var ds = MAXIMUS.Controllers.PDMS.ClaimsController.GetConditionCodeDescription(desc);
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);

        }

        [HttpGet]
        [Route("GetDiagnosisCodeDescrption")]
        public IActionResult GetDiagnosisCodeDescrption(string Code = "", string lICDVer = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            var data = string.Empty;
            var ds = LookupTableController.GetICDDiagnosisCode(Code, lICDVer, "");
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetICDProcedureCodeDescription")]
        public IActionResult GetICDProcedureCodeDescription(string desc = "", string dropdownvalue = "")
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            parms.Add("CLAIMS_ICD_PROCEDURE_CODE", desc);
            var data = string.Empty;

            DataTable dt = LookupTableController.GetICDProcedureCode(desc, "", dropdownvalue);
            // return ds.GetXml();
            if (dt.Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(dt);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("GetNDCCodeDescription")]
        public IActionResult GetNDCCodeDescription(string desc = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("NDCCode", DbType.String, desc, true));
            var data = string.Empty;

            var ds = DataAccess.ExecuteStoredProcedure("Usp_Select_CLAIMS_NDC_CODE", parameters, "CLAIMS_NDC_CODE");

            // return ds.GetXml();
            if (ds.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(ds.Tables[0]);
            }

            return Ok(data);
        }

        [HttpGet]
        [Route("GetReasonCode")]
        public IActionResult GetReasonCode(string desc = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CLAIMS_REASON_CODE", DbType.String, desc, true));
            var data = string.Empty;
            var ds = DataAccess.ExecuteStoredProcedure("usp_CLAIMS_CARC", parameters, "CLAIMS_CARC");

            if (Helper.HasRows(ds))
            {
                // return ds.GetXml();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }

            return Ok(data);
        }

        [HttpGet]
        [Route("ValidationForTypeOfBill")]
        public IActionResult ValidationForTypeOfBill(string TypeOfBill = "")
        {
            string errorMessage = string.Empty;

            if (TypeOfBill.Length > 0)
            {
                if (!TypeOfBill.StartsWith("0"))
                {
                    errorMessage = "Type of bill must start from 0.";
                }

                else if (TypeOfBill.Length < 4)
                {
                    errorMessage = "Type Of Bill less than 4";
                }
                else
                {
                    if (!string.IsNullOrEmpty(TypeOfBill))
                    {
                        DataSet dsTOB = LookupTableController.GetInstitutionalTypeOfBill(TypeOfBill, "");

                        if (!Helper.HasRows(dsTOB))
                            errorMessage = "Type of Bill code is invalid";
                    }
                }
            }
            errorMessage = JsonConvert.SerializeObject(errorMessage);
            return Ok(errorMessage);
        }


        [HttpPost]
        [Route("AddValueCodeData")]
        public IActionResult AddValueCodeData(string ClaimID = "", string Value_Code = "", string Amount = "", string ValueCodeDesc = "", string hdnValue_Code = "", string userid = "")
        {

            List<ValueCodeDetail> ValueCodeData = new List<ValueCodeDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet bindGrid = Claims.GetValueCodeDetails(ClaimID);
            string line = string.Empty;
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            int rows = dt.Rows.Count;
            bool duplicatecodeExist = false;
            List<ValueCodeDetail> data = new List<ValueCodeDetail>();
            ValueCodeDetail ValueCodeInformation = new ValueCodeDetail();
            if (!string.IsNullOrEmpty(Value_Code))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ValueCodefromDB = dr["Value_Code"].ToString();
                        ValueCodeInformation = new ValueCodeDetail();
                        if (ValueCodefromDB == Value_Code)
                        {
                            ValueCodeInformation.ErrorMessage = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        data.Add(ValueCodeInformation);
                    }
                    ValueCodeData = data;
                }
            }

            if (duplicatecodeExist != true)
            {
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    line = (rows + 1).ToString();
                    parms.Add("Line", line);
                    parms.Add("Value_Code", Value_Code);
                    parms.Add("Amount", Amount);
                    parms.Add("Claim_ID", ClaimID);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    parms.Add("Last_Modified_date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Value_Code_Information", parms);
                    }
                    catch (Exception ex) { }
                    //GetOccurenceSpanInformationData(null);
                    List<ValueCodeDetail> details = new List<ValueCodeDetail>();
                    details = Claims.SelectValueCodeDetails(ClaimID);
                    ValueCodeData = details;
                }
            }
            return Ok(ValueCodeData);
        }

        [HttpPut]
        [Route("EditValueCodeData")]
        public IActionResult EditValueCodeData(string ClaimID = "", string Claims_Value_Code_Information_ID = "", string Value_Code = "",
            string Amount = "", string ValueCodeDesc = "", string Line = "", string userid = "")
        {

            List<ValueCodeDetail> ValueCodeData = new List<ValueCodeDetail>();
            DataSet bindGrid = Claims.GetValueCodeDetails(ClaimID);
            string line = string.Empty;
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            int rows = dt.Rows.Count;
            bool duplicatecodeExist = false;
            List<ValueCodeDetail> data = new List<ValueCodeDetail>();
            ValueCodeDetail ValueCodeInformation = new ValueCodeDetail();
            if (!string.IsNullOrEmpty(Value_Code))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ValueCodefromDB = dr["Value_Code"].ToString();
                        string ValueCodeInfoId = dr["Claims_Value_Code_Information_ID"].ToString();
                        ValueCodeInformation = new ValueCodeDetail();
                        if (ValueCodefromDB == Value_Code && (ValueCodeInfoId != Claims_Value_Code_Information_ID))
                        {
                            ValueCodeInformation.ErrorMessage = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        data.Add(ValueCodeInformation);
                    }
                    ValueCodeData = data;
                }
            }

            if (duplicatecodeExist != true)
            {
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    //Updating db
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Line", Line);
                    parms.Add("Claims_Value_Code_Information_ID", Claims_Value_Code_Information_ID);
                    parms.Add("Value_Code", Value_Code);
                    parms.Add("Amount", Amount);
                    parms.Add("Claim_ID", ClaimID);
                    //parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    // parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    parms.Add("Last_Modified_date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Value_Code_Information", parms);// need to add SP
                    }
                    catch (Exception ex) { }
                    parms.Clear();

                    List<ValueCodeDetail> details = new List<ValueCodeDetail>();
                    details = Claims.SelectValueCodeDetails(ClaimID);
                    ValueCodeData = details;

                }
            }
            return Ok(ValueCodeData);
        }

        [HttpPost]
        [Route("AddOccurrenceSpanData")]
        public IActionResult AddOccurrenceSpanData(string Claim_ID = "", string Occurrence_Code_Span = "", string FromDate = "",
            string ToDate = "", string OccurrenceDescription = "", string userid = "")
        {
            List<OccurrenceSpanInfo> OccurrenceSpanInfoData = new List<OccurrenceSpanInfo>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet bindGrid = Claims.GetOccurrenceSpanInfoData(Claim_ID);
            List<OccurrenceSpanInfo> details = new List<OccurrenceSpanInfo>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            OccurrenceSpanInfo OccurrenceSpanInfodata = new OccurrenceSpanInfo();
            bool duplicatecodeExist = false;
            if (!string.IsNullOrEmpty(Occurrence_Code_Span))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();
                        OccurrenceSpanInfodata = new OccurrenceSpanInfo();
                        if (Occurrence_Code_Span == OccurrenceSpanCode)
                        {
                            OccurrenceSpanInfodata.Error = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        details.Add(OccurrenceSpanInfodata);
                    }
                    OccurrenceSpanInfoData = details;
                }
            }
            if (duplicatecodeExist != true)
            {
                string line = string.Empty;
                int rows = 0;
                rows = dt.Rows.Count;
                line = (rows + 1).ToString();
                if (rows < 24 && (!string.IsNullOrWhiteSpace(Claim_ID)))
                {
                    parms.Add("Line", line);
                    parms.Add("Claim_ID", Claim_ID);
                    parms.Add("Occurrence_Code_Span", Occurrence_Code_Span);
                    parms.Add("FromDate", FromDate);
                    parms.Add("ToDate", ToDate);
                    parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Occurrence_Code_Span_Information", parms);
                    }
                    catch { }
                    //GetOccurenceSpanInformationData(null);
                }

                details = Claims.SelectOccurrenceSpanDetails(Claim_ID);
                OccurrenceSpanInfoData = details;
            }
            return Ok(OccurrenceSpanInfoData);

        }

        [HttpGet]
        [Route("GetOccurrenceSpanData")]
        public IActionResult GetOccurrenceSpanData(string Claims_Occurrence_Code_Span_Information_ID = "", string Claim_ID = "")
        {
            List<OccurrenceSpanInfo> OccSpanCodeData = new List<OccurrenceSpanInfo>();

            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetOccurrenceSpanInfo(Claims_Occurrence_Code_Span_Information_ID, Claim_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OccurrenceSpanInfo lstOccurrenceSpanInfo = new OccurrenceSpanInfo();
                    lstOccurrenceSpanInfo.Line = dr["Line"].ToString();
                    lstOccurrenceSpanInfo.OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();
                    lstOccurrenceSpanInfo.FromDate = Convert.ToDateTime(dr["FromDate"]).ToString("MM/dd/yyyy");
                    lstOccurrenceSpanInfo.ToDate = Convert.ToDateTime(dr["ToDate"]).ToString("MM/dd/yyyy");
                    lstOccurrenceSpanInfo.OccurrenceDesc = dr["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                    lstOccurrenceSpanInfo.Claims_Occurrence_Code_Span_Information_ID = dr["Claims_Occurrence_Code_Span_Information_ID"].ToString();
                    lstOccurrenceSpanInfo.Claim_ID = dr["Claim_ID"].ToString();

                    OccSpanCodeData.Add(lstOccurrenceSpanInfo);
                }
            }

            return Ok(OccSpanCodeData);
        }

        [HttpPut]
        [Route("EditOccurrenceSpanInfoData")]
        public IActionResult EditOccurrenceSpanInfoData(string Claim_ID = "", string Claims_Occurrence_Code_Span_Information_ID = "",
            string Occurrence_Code_Span = "", string FromDate = "", string ToDate = "", string OccurrenceDesc = "", string Line = "", string userid = "")
        {
            List<OccurrenceSpanInfo> OccurrenceSpanInfoData = new List<OccurrenceSpanInfo>();
            DataSet bindGrid = Claims.GetOccurrenceSpanInfoData(Claim_ID);
            List<OccurrenceSpanInfo> details = new List<OccurrenceSpanInfo>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            OccurrenceSpanInfo OccurrenceSpanInfodata = new OccurrenceSpanInfo();
            bool duplicatecodeExist = false;
            if (!string.IsNullOrEmpty(Occurrence_Code_Span))
            {
                if (dt.Rows.Count > 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();
                        string CodespanInfoId = dr["Claims_Occurrence_Code_Span_Information_ID"].ToString();
                        OccurrenceSpanInfodata = new OccurrenceSpanInfo();
                        if (Occurrence_Code_Span == OccurrenceSpanCode && (CodespanInfoId != Claims_Occurrence_Code_Span_Information_ID))
                        {
                            OccurrenceSpanInfodata.Error = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        details.Add(OccurrenceSpanInfodata);
                    }
                    OccurrenceSpanInfoData = details;
                }

            }
            if (duplicatecodeExist != true)
            {

                //Updating db
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", Claim_ID);
                parms.Add("Claims_Occurrence_Code_Span_Information_ID", Claims_Occurrence_Code_Span_Information_ID);
                parms.Add("Line", Line);
                parms.Add("Occurrence_Code_Span", Occurrence_Code_Span);
                parms.Add("FromDate", FromDate);
                parms.Add("ToDate", ToDate);
                parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                try
                {
                    MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Occurrence_Code_Span_Information", parms);// need to add SP
                }
                catch { }
                parms.Clear();

                var data = string.Empty;
                details = new List<OccurrenceSpanInfo>();
                details = Claims.SelectOccurrenceSpanDetails(Claim_ID);
                OccurrenceSpanInfoData = details;
            }
            return Ok(OccurrenceSpanInfoData);

        }

        [HttpDelete]
        [Route("DeleteOccurrencSPanCode")]
        public IActionResult DeleteOccurrencSPanCode(string Claim_ID = "", string Claims_Occurrence_Code_Span_Information_ID = "")
        {
            List<OccurrenceSpanInfo> OccurrenceSpanInfoData = new List<OccurrenceSpanInfo>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Occurrence_Code_Span_Information", "Claims_Occurrence_Code_Span_Information_ID", Int32.Parse(Claims_Occurrence_Code_Span_Information_ID));
            }
            catch { }

            parms.Clear();

            var data = string.Empty;
            List<OccurrenceSpanInfo> details = new List<OccurrenceSpanInfo>();
            details = Claims.SelectOccurrenceSpanDetails(Claim_ID);
            OccurrenceSpanInfoData = details;
            return Ok(OccurrenceSpanInfoData);
        }

        [HttpGet]
        [Route("BindOccuranceCode")]
        public IActionResult BindOccuranceCode(string Claim_IDOccurrence = "", string Occurrence_Code_Span = "")
        {



            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Clear();
            var data = "true";
            DataSet bindGrid = Claims.GetOccurrenceInfoData(Claim_IDOccurrence);

            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];

            if (!string.IsNullOrEmpty(Occurrence_Code_Span))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();

                        if (Occurrence_Code_Span == OccurrenceSpanCode)
                        {
                            data = "false";
                        }
                    }

                }

            }
            return Ok(data);

        }

        [HttpGet]
        [Route("GetValueCodeData")]
        public IActionResult GetValueCodeData(string valuecodeInfoID = "", string ClaimID = "")
        {
            List<ValueCodeDetail> valuecodedetailsData = new List<ValueCodeDetail>();
            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetValueCodeData(valuecodeInfoID, ClaimID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ValueCodeDetail ValueCodeDetailsIn = new ValueCodeDetail();
                    ValueCodeDetailsIn.Line = dr["Line"].ToString();
                    ValueCodeDetailsIn.Value_Code = dr["Value_Code"].ToString();
                    ValueCodeDetailsIn.Amount = dr["Amount"].ToString();
                    ValueCodeDetailsIn.ValueCodeDesc = dr["CLAIMS_VALUE_CODE_DESC"].ToString();
                    ValueCodeDetailsIn.hdnValue_Code = dr["Claims_Value_Code_Information_ID"].ToString();
                    ValueCodeDetailsIn.ClaimID = dr["Claim_ID"].ToString();
                    valuecodedetailsData.Add(ValueCodeDetailsIn);
                }
            }

            return Ok(valuecodedetailsData);
        }

        [HttpPut]
        [Route("EditValueCodeInfoData")]
        public IActionResult EditValueCodeInfoData(string Claim_ID = "", string Claims_Value_Code_Information_ID = "",
            string ValueCode = "", string Amount = "", string ValueCodeDesc = "", string userid = "")
        {

            List<ValueCodeDetail> ValueCodeData = new List<ValueCodeDetail>();
            //Updating db
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", Claim_ID);
            parms.Add("Claims_Value_Code_Information_ID", Claims_Value_Code_Information_ID);
            parms.Add("Line", "");
            parms.Add("Value_Code", ValueCode);
            parms.Add("Amount", Amount);
            parms.Add("ValueCodeDesc", ValueCodeDesc);
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Value_Code_Information", parms);// need to add SP
            }
            catch (Exception ex) { }
            parms.Clear();

            List<ValueCodeDetail> details = new List<ValueCodeDetail>();
            details = Claims.SelectValueCodeDetails(Claim_ID);
            ValueCodeData = details;

            return Ok(ValueCodeData);


        }


        [HttpGet]
        [Route("GetToothServiceLine")]

        public IActionResult GetToothServiceLine(string claimId = "")
        {
            var data = string.Empty;
            List<ServiceLine> dentalServiceData = new List<ServiceLine>();

            DataTable dt = new DataTable();


            var ds = MAXIMUS.Controllers.PDMS.ClaimsController.GetToothServiceLine(Convert.ToInt32(claimId));
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ServiceLine dentalServiceDetail = new ServiceLine();
                    dentalServiceDetail.Service_Line = dr["Service_Line"].ToString();
                    dentalServiceData.Add(dentalServiceDetail);
                }
            }
            return Ok(dentalServiceData);
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
                return Ok(list);
            }
            return Ok(list);
        }

        [HttpDelete]
        [Route("DeleteValueCodeData")]
        public IActionResult DeleteValueCodeData(string ClaimID = "", string Claims_Value_Code_Information_ID = "")
        {

            List<ValueCodeDetail> ValueCodeData = new List<ValueCodeDetail>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Value_Code_Information", "Claims_Value_Code_Information_ID", Int32.Parse(Claims_Value_Code_Information_ID));
            }
            catch (Exception ex) { }

            parms.Clear();

            var data = string.Empty;

            List<ValueCodeDetail> details = new List<ValueCodeDetail>();
            details = Claims.SelectValueCodeDetails(ClaimID);
            ValueCodeData = details;
            return Ok(ValueCodeData);
        }

        [HttpPost]
        [Route("AddNDCDetailsData")]
        public IActionResult AddNDCDetailsData(string ClaimID = "", string ClaimType = "", string hdnNDC_Code = "",
            string serviceline = "", string NDCCode = "", string totalUnit = "", string PrescriptionNumber = "", string UnitMeasure = "", string userid = "")
        {
            if (PrescriptionNumber == null)
                PrescriptionNumber = string.Empty;
            List<NDCDetails> NDCDetailsData = new List<NDCDetails>();
            Dictionary<string, string> parms = new Dictionary<string, string>();

            parms.Add("Service_Line", serviceline.ToString());
            parms.Add("NDC", NDCCode.ToString());
            parms.Add("Units_of_Measure", UnitMeasure.ToString());
            parms.Add("Prescription_Number", PrescriptionNumber.ToString());
            parms.Add("Total_Unit", totalUnit.ToString());

            parms.Add("Claim_ID", ClaimID.ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Last_Modified_date", DateTime.Now.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_NDC_Details", parms);
            }
            catch (Exception ex) { }
            List<NDCDetails> details = new List<NDCDetails>();
            details = Claims.SelectNDCDetails(ClaimID);
            NDCDetailsData = details;
            return Ok(NDCDetailsData);

        }

        [HttpDelete]
        [Route("DeleteNDCDetails")]
        public IActionResult DeleteNDCDetails(string Claim_ID = "", string Claims_NDC_Details_Screen_ID = "")
        {

            List<NDCDetails> NDCDetail = new List<NDCDetails>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_NDC_Details", "Claims_NDC_Details_Screen_ID", Int32.Parse(Claims_NDC_Details_Screen_ID));
            }
            catch (Exception ex) { }
            // }


            parms.Clear();

            var data = string.Empty;
            List<NDCDetails> details = new List<NDCDetails>();
            details = Claims.SelectNDCDetails(Claim_ID);
            NDCDetail = details;
            return Ok(NDCDetail);

        }

        [HttpPost]
        [Route("EditNDCDetails")]
        public IActionResult EditNDCDetails(string Claims_NDC_Details_Screen_ID = "", string Claim_ID = "")
        {
            List<NDCDetails> NDCDetails = new List<NDCDetails>();


            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.EditNDCCodeData(Claims_NDC_Details_Screen_ID, Claim_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    NDCDetails NDCDetailsdata = new NDCDetails();
                    NDCDetailsdata.ServiceLine = dr["Service_Line"].ToString();
                    NDCDetailsdata.NDCCode = dr["NDC"].ToString();
                    NDCDetailsdata.UnitMeasure = dr["Units_of_Measure"].ToString();
                    NDCDetailsdata.PrescriptionNuber = dr["Prescription_Number"].ToString();
                    NDCDetailsdata.TotalUnit = dr["Total_Unit"].ToString();
                    NDCDetailsdata.Claims_NDC_Details_Screen_ID = dr["Claims_NDC_Details_Screen_ID"].ToString();
                    NDCDetailsdata.Claim_ID = dr["Claim_ID"].ToString();

                    NDCDetails.Add(NDCDetailsdata);
                }
            }
            return Ok(NDCDetails);

        }

        [HttpPost]
        [Route("EditNDCDetailsData")]
        public IActionResult EditNDCDetailsData(string ClaimID = "", string ClaimType = "", string hdnNDC_Code = "", string serviceline = "",
            string NDCCode = "", string totalUnit = "", string PrescriptionNumber = "", string UnitMeasure = "", string userid = "")
        {
            if (PrescriptionNumber == null)
                PrescriptionNumber = string.Empty;
            List<NDCDetails> NDCdetailsData = new List<NDCDetails>();

            //Updating db
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", ClaimID.ToString());
            parms.Add("Claims_NDC_Details_Screen_ID", hdnNDC_Code.ToString());
            parms.Add("Service_Line", serviceline);
            parms.Add("NDC", NDCCode);
            parms.Add("Units_of_Measure", UnitMeasure);
            parms.Add("Prescription_Number", PrescriptionNumber);
            parms.Add("Total_Unit", totalUnit.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
            parms.Add("Last_Modified_date", DateTime.Now.ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_NDC_Details", parms);
            }
            catch (Exception ex) { }
            parms.Clear();

            var data = string.Empty;
            List<NDCDetails> details = new List<NDCDetails>();
            details = Claims.SelectNDCDetails(ClaimID);
            NDCdetailsData = details;
            return Ok(NDCdetailsData);

        }

        [HttpPost]
        [Route("Addicd10procedurecode")]
        public IActionResult Addicd10procedurecode(string hdnClaimId = "", string ddlSequenceText = "", string txtICD10Procedurecode = "",
            string ddlICDVersion = "", string txtICDPrCdDate = "", string lblICDDesc = "", string userid = "")
        {
            List<ICDProcedureCode> ICDProcedureCodeList = new List<ICDProcedureCode>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId, true));
            int sequencePrincipalCount = 0;
            int sequenceOtherCount = 0;
            string SequeceCount = "0";
            DataSet dtSequenceDetails = DataAccess.ExecuteStoredProcedure("usp_Claims_ICD_Procedure_Code_Sequence_TotalSequenceCount", parameters, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(dtSequenceDetails))
            {
                var sequencePrincipaldata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "3").ToList();
                var sequenceotherdata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "2").ToList();

                if (sequencePrincipaldata.Count() != 0)
                {
                    sequencePrincipalCount = Convert.ToInt32(sequencePrincipaldata.FirstOrDefault().ItemArray[0]);
                }

                if (sequenceotherdata.Count() != 0)
                {
                    sequenceOtherCount = Convert.ToInt32(sequenceotherdata.ToList().FirstOrDefault().ItemArray[0]);
                }
            }
            if ((sequencePrincipalCount >= 1) && (ddlSequenceText == "3"))
            {
                ICDProcedureCode ICD_Procedue_Code = new ICDProcedureCode();
                ICD_Procedue_Code.SequeneCount_Principal = "already added";
                ICDProcedureCodeList.Add(ICD_Procedue_Code);
                return Ok(ICDProcedureCodeList);
            }
            else
            {
                //lblError.Visible = false;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_Id", hdnClaimId);
                parms.Add("Sequence_NO", ddlSequenceText);
                parms.Add("ICD_Version", string.IsNullOrEmpty(ddlICDVersion) ? null : ddlICDVersion);
                parms.Add("Procedure_Code_Description", string.IsNullOrEmpty(lblICDDesc) ? null : lblICDDesc.Trim());
                parms.Add("ICD_Procedure_Code", string.IsNullOrEmpty(txtICD10Procedurecode) ? null : txtICD10Procedurecode);
                parms.Add("ICD_Date", string.IsNullOrEmpty(txtICDPrCdDate) ? null : txtICDPrCdDate);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());

                parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());

                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("claims_icd_procedureCode", parms);
            }
            ICDProcedureCodeList = Claims.GetICDProcedureData(hdnClaimId);
            return Ok(ICDProcedureCodeList);

        }


        [HttpGet]
        [Route("GetICD_Procedure_CodeDetails")]
        public IActionResult GetICD_Procedure_CodeDetails(string Claim_ID = "", string Claims_ICD_Procedure_Code_Sequence_ID = "")
        {
            List<ICDProcedureCode> ICDProcedureCodepanelData = new List<ICDProcedureCode>();


            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetICDProcCode(Claim_ID, Claims_ICD_Procedure_Code_Sequence_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ICDProcedureCode ICD_Procedue_Code = new ICDProcedureCode();
                    ICD_Procedue_Code.Claims_ICD_Procedure_Code_Sequence_ID = dr["Claims_ICD_Procedure_Code_Sequence_ID"].ToString();
                    ICD_Procedue_Code.Sequence = dr["Sequence_No"].ToString();
                    ICD_Procedue_Code.IcdProcedureCode = dr["ICD_Procedure_Code"].ToString();
                    ICD_Procedue_Code.ICDVersion = dr["ICD_Version"].ToString();
                    ICD_Procedue_Code.IcdProcedureCodeDescription = dr["Procedure_Code_Description"].ToString();
                    ICD_Procedue_Code.Claim_ID = dr["Claim_ID"].ToString();
                    ICD_Procedue_Code.Date = Convert.ToDateTime(dr["ICD_Date"]).ToString("MM/dd/yyyy");
                    ICD_Procedue_Code.Perior_Auth_Claim_Sequence_Desc = dr["PRIOR_AUTH_CLAIM_SEQUENCE_Desc"].ToString();

                    ICDProcedureCodepanelData.Add(ICD_Procedue_Code);
                }
            }
            return Ok(ICDProcedureCodepanelData);

        }

        [HttpPut]
        [Route("EditICD10ProcedureCode")]
        public IActionResult EditICD10ProcedureCode(string hdnClaimId = "", string Claims_ICDProc_Code_Sequence = "",
            string Sequence = "", string ICD10Procedurecode = "", string ddlICDVersion = "", string txtICDPrCdDate = "", string ICDDescription = "", string userid = "")
        {

            List<ICDProcedureCode> ICDProcedureCodeList = new List<ICDProcedureCode>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId, true));
            int sequencePrincipalCount = 0;
            int sequenceOtherCount = 0;
            DataSet dtSequenceDetails = DataAccess.ExecuteStoredProcedure("usp_Claims_ICD_Procedure_Code_Sequence_TotalSequenceCount", parameters, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(dtSequenceDetails))
            {
                var sequencePrincipaldata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "3").ToList();
                var sequenceotherdata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "2").ToList();

                if (sequencePrincipaldata.Count() != 0)
                {
                    sequencePrincipalCount = Convert.ToInt32(sequencePrincipaldata.FirstOrDefault().ItemArray[0]);
                }
                else
                {

                }
                if (sequenceotherdata.Count() != 0)
                {
                    sequenceOtherCount = Convert.ToInt32(sequenceotherdata.ToList().FirstOrDefault().ItemArray[0]);
                }
            }

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId, true));
            param.Add(SqlParms.CreateParameter("Claims_ICD_Procedure_Code_Sequence_ID", DbType.Int32, Claims_ICDProc_Code_Sequence, true));
            DataSet PricipleICDProcedure = DataAccess.ExecuteStoredProcedure("usp_Select_PricipleICDProcedureCode", param, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(PricipleICDProcedure) && Sequence == "3")
            {
                ICDProcedureCode ICD_Procedue_Code = new ICDProcedureCode();
                ICD_Procedue_Code.SequeneCount_Principal = "already added";
                ICDProcedureCodeList.Add(ICD_Procedue_Code);
                return Ok(ICDProcedureCodeList);
            }
            else
            {

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claims_ICD_Procedure_Code_Sequence_ID", Claims_ICDProc_Code_Sequence);
                parms.Add("Sequence_NO", Sequence);
                parms.Add("ICD_Procedure_Code", ICD10Procedurecode);
                parms.Add("ICD_Version", ddlICDVersion);
                parms.Add("Procedure_Code_Description", ICDDescription);
                parms.Add("Claim_ID", hdnClaimId);
                parms.Add("ICD_DATE", txtICDPrCdDate);

                parms.Add("LAST_MODIFIED_USER", HelperFacade.GetUserId(userid).ToString());
                parms.Add("LAST_MODIFIED_DATE", DateTime.Now.ToString());
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("claims_icd_procedurecode", parms);

                parms.Clear();
            }

            ICDProcedureCodeList = Claims.GetICDProcedureData(hdnClaimId);

            return Ok(ICDProcedureCodeList);
        }

        [HttpDelete]
        [Route("DeleteICDProcedeCodeSequence")]
        public IActionResult DeleteICDProcedeCodeSequence(string hdnClaimId = "", string Claims_ICD_Procedure_Code_ID = "")
        {
            List<ICDProcedureCode> ICDProcedureCodeList = new List<ICDProcedureCode>();

            List<ICDProcedureCode> ICDProcedureCodeDataList = new List<ICDProcedureCode>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_ICD_ProcedureCode", "Claims_ICD_Procedure_Code_Sequence_ID", Int32.Parse(Claims_ICD_Procedure_Code_ID));
            }
            catch (Exception ex) { }
            parms.Clear();
            ICDProcedureCodeList = Claims.GetICDProcedureData(hdnClaimId);
            return Ok(ICDProcedureCodeList);
        }

        [HttpPost]
        [Route("AddOccurrenceInfoData")]
        public IActionResult AddOccurrenceInfoData(string Claim_ID = "", string Occurrence_Code = "", string OccurrenceDate = "", string OccurrenceDescription = "", string userid = "")
        {

            List<OccurenceInfo> OccurrenceInfoData = new List<OccurenceInfo>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet bindGrid = Claims.GetOccurrenceInfoData(Claim_ID);
            List<OccurenceInfo> details = new List<OccurenceInfo>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            OccurenceInfo Occurrencedata = new OccurenceInfo();
            bool duplicatecodeExist = false;
            if (!string.IsNullOrEmpty(Occurrence_Code))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string OccCode = dr["OccurrenceCode"].ToString();
                        Occurrencedata = new OccurenceInfo();
                        if (Occurrence_Code == OccCode)
                        {
                            Occurrencedata.Error = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        details.Add(Occurrencedata);
                    }
                    OccurrenceInfoData = details;
                }

            }
            if (duplicatecodeExist != true)
            {
                string line = string.Empty;
                int rows = 0;
                rows = dt.Rows.Count;
                line = (rows + 1).ToString();
                if (rows < 24 && (!string.IsNullOrWhiteSpace(Claim_ID)))
                {
                    parms.Add("Claims_Occurrence_Line", line);
                    parms.Add("Claim_ID", Claim_ID);
                    parms.Add("Claims_Occurrence_Code", Occurrence_Code);
                    parms.Add("Claims_Occurrence_Date", string.IsNullOrEmpty(OccurrenceDate) ? "" : OccurrenceDate);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Occurrence_Information", parms);
                    }
                    catch (Exception ex) { }
                    //GetOccurenceSpanInformationData(null);

                }

                details = Claims.GettOccurrenceDetails(Claim_ID);
                OccurrenceInfoData = details;
            }
            return Ok(OccurrenceInfoData);
        }


        [HttpGet]
        [Route("GetNPICodeDetails")]
        public IActionResult GetNPICodeDetails(string firstname = "", string lastname = "", string Medicateid = "", string hspsccode = "")
        {
            DataTable dt = null;
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
        [Route("GetOccurrenceInfoData")]
        public IActionResult GetOccurrenceInfoData(string ClaimsOccurrenceInformationID = "", string Claim_ID = "")
        {
            List<OccurenceInfo> OccurrenceInfodata = new List<OccurenceInfo>();

            OccurenceInfo OccurrenceInfoData = new OccurenceInfo();
            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.EditOccurrenceInfoData(ClaimsOccurrenceInformationID, Claim_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OccurrenceInfoData = new OccurenceInfo();
                    OccurrenceInfoData.Line = dr["Claims_Occurrence_Line"].ToString();
                    OccurrenceInfoData.OccurrenceInfoCode = dr["OccurrenceCode"].ToString();
                    OccurrenceInfoData.OccurrenceDate = Convert.ToDateTime(dr["OccurrenceDate"]).ToString("MM/dd/yyyy");
                    OccurrenceInfoData.OccurrenceDesc = dr["OccurrenceDesc"].ToString();
                    OccurrenceInfoData.ClaimsOccurrenceInformationID = dr["ClaimsOccurrenceInformationID"].ToString();
                    OccurrenceInfoData.ClaimId = dr["ClaimId"].ToString();
                    OccurrenceInfodata.Add(OccurrenceInfoData);

                }
            }
            return Ok(OccurrenceInfodata);
        }

        [HttpDelete]
        [Route("DeleteOccurrencInfoCode")]
        public IActionResult DeleteOccurrencInfoCode(string Claim_ID = "", string ClaimsOccurrenceInformationID = "")
        {

            List<OccurenceInfo> OccurrenceSpanInfoData = new List<OccurenceInfo>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("usp_delete_claims_occurrence_information", "Claims_Occurrence_Information_ID", Int32.Parse(ClaimsOccurrenceInformationID));
            }
            catch (Exception ex) { }
            // }


            parms.Clear();

            var data = string.Empty;
            List<OccurenceInfo> details = new List<OccurenceInfo>();
            details = Claims.GettOccurrenceDetails(Claim_ID);
            OccurrenceSpanInfoData = details;
            return Ok(OccurrenceSpanInfoData);

        }

        [HttpPost]
        [Route("EditDataOfOccurrenceInfo")]
        public IActionResult EditDataOfOccurrenceInfo(string ClaimID = "", string ClaimsOccurrenceInformationID = "", string OccurrenceCode = "",
            string OccurrecneDate = "", string OccurrenceDesc = "", string OccLine = "", string userid = "")
        {

            List<OccurenceInfo> OccurrenceSpanInfoData = new List<OccurenceInfo>();

            //Updating db
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet bindGrid = Claims.GetOccurrenceInfoData(ClaimID);
            List<OccurenceInfo> details = new List<OccurenceInfo>();
            DataTable dt = new DataTable();
            dt = bindGrid.Tables[0];
            OccurenceInfo Occurrencedata = new OccurenceInfo();
            bool duplicatecodeExist = false;
            if (!string.IsNullOrEmpty(OccurrenceCode))
            {
                if (dt.Rows.Count > 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string OccCode = dr["OccurrenceCode"].ToString();
                        string OccurrenceInfoID = dr["ClaimsOccurrenceInformationID"].ToString();
                        Occurrencedata = new OccurenceInfo();
                        if (OccurrenceCode == OccCode && (ClaimsOccurrenceInformationID != OccurrenceInfoID))
                        {
                            Occurrencedata.Error = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        details.Add(Occurrencedata);
                    }
                    OccurrenceSpanInfoData = details;
                }

            }
            if (duplicatecodeExist != true)
            {
                parms.Add("Claims_Occurrence_Line", OccLine);
                parms.Add("Claim_ID", ClaimID);
                parms.Add("Claims_Occurrence_Code", OccurrenceCode);
                parms.Add("Claims_Occurrence_Date", OccurrecneDate);
                parms.Add("Claims_Occurrence_Information_ID", ClaimsOccurrenceInformationID);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", HelperFacade.GetUserId(userid).ToString());
                try
                {
                    MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Occurrence_Information", parms);// need to add SP
                }
                catch (Exception ex) { }
                parms.Clear();

                var data = string.Empty;
                List<OccurenceInfo> OccInfodetails = new List<OccurenceInfo>();
                OccInfodetails = Claims.GettOccurrenceDetails(ClaimID);
                OccurrenceSpanInfoData = OccInfodetails;
            }

            return Ok(OccurrenceSpanInfoData);
        }

        [HttpPost]
        [Route("AddConditionCodeData")]
        public IActionResult AddConditionCodeData(string ClaimID = "", string ConditionCode = "", string ConditionDescription = "", string hdnConditionCode = "", string userid = "")
        {

            List<ConditionCodeDetail> ConditionCodeData = new List<ConditionCodeDetail>();
            DataSet gridbind = Claims.GetConditionCodeDetails(ClaimID);
            DataTable dt = gridbind.Tables[0];
            Dictionary<string, string> parms = new Dictionary<string, string>();
            string line = string.Empty;
            int max = dt.Rows.Count;
            bool duplicatecodeExist = false;
            List<ConditionCodeDetail> data = new List<ConditionCodeDetail>();
            ConditionCodeDetail ConditionCodeInformation = new ConditionCodeDetail();
            if (!string.IsNullOrEmpty(ConditionCode))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ConditionCodefromDB = dr["Condition_Code"].ToString();
                        ConditionCodeInformation = new ConditionCodeDetail();
                        if (ConditionCodefromDB == ConditionCode)
                        {
                            ConditionCodeInformation.ErrorMsg = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        data.Add(ConditionCodeInformation);
                    }
                    ConditionCodeData = data;
                }

            }
            if (duplicatecodeExist != true)
            {
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    line = max < 9 ? string.Concat("0", max + 1) : (max + 1).ToString();
                    parms.Add("Line", line);
                    parms.Add("Condition_Code", ConditionCode);
                    parms.Add("Claim_ID", ClaimID);

                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    parms.Add("Last_Modified_date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Condition_Code_Information", parms);
                    }
                    catch (Exception ex) { }
                    //GetOccurenceSpanInformationData(null);
                    List<ConditionCodeDetail> details = new List<ConditionCodeDetail>();
                    details = Claims.SelectConditionCodeDetails(ClaimID);
                    ConditionCodeData = details;
                }
            }
            return Ok(ConditionCodeData);
        }

        [HttpDelete]
        [Route("DeleteConditionCodeData")]
        public IActionResult DeleteConditionCodeData(string ClaimID = "", string Claims_Condition_Code_Information_ID = "")
        {

            List<ConditionCodeDetail> ConditionCodeData = new List<ConditionCodeDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Condition_Code_Information", "Claims_Condition_Code_Information_ID", Int32.Parse(Claims_Condition_Code_Information_ID));
            }
            catch (Exception ex) { }
            parms.Clear();
            var data = string.Empty;
            List<ConditionCodeDetail> details = new List<ConditionCodeDetail>();
            details = Claims.SelectConditionCodeDetails(ClaimID);
            ConditionCodeData = details;
            return Ok(ConditionCodeData);
        }


        [HttpPut]
        [Route("EditConditionCodeData")]
        public IActionResult EditConditionCodeData(string ClaimID = "", string Claims_Condition_Code_Information_ID = "",
            string ConditionCode = "", string ConditionCodeDesc = "", string Line = "", string userid = "")
        {


            List<ConditionCodeDetail> ConditionCodeData = new List<ConditionCodeDetail>();
            DataSet gridbind = Claims.GetConditionCodeDetails(ClaimID);
            DataTable dt = gridbind.Tables[0];
            Dictionary<string, string> parms = new Dictionary<string, string>();
            string line = string.Empty;
            int max = dt.Rows.Count;
            bool duplicatecodeExist = false;
            List<ConditionCodeDetail> data = new List<ConditionCodeDetail>();
            ConditionCodeDetail ConditionCodeInformation = new ConditionCodeDetail();
            if (!string.IsNullOrEmpty(ConditionCode))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string ConditionCodefromDB = dr["Condition_Code"].ToString();
                        string ConditioncodeInfoId = dr["Claims_Condition_Code_Information_ID"].ToString();
                        ConditionCodeInformation = new ConditionCodeDetail();
                        if (ConditionCodefromDB == ConditionCode && (ConditioncodeInfoId != Claims_Condition_Code_Information_ID))
                        {
                            ConditionCodeInformation.ErrorMsg = "Same Code Exist";
                            duplicatecodeExist = true;
                        }
                        data.Add(ConditionCodeInformation);
                    }
                    ConditionCodeData = data;
                }
            }
            if (duplicatecodeExist != true)
            {
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    //Updating db                
                    parms.Add("Line", Line);
                    parms.Add("Claims_Condition_Code_Information_ID", Claims_Condition_Code_Information_ID);
                    parms.Add("Condition_Code", ConditionCode);
                    parms.Add("Claim_ID", ClaimID);
                    parms.Add("Last_Modified_date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Condition_Code_Information", parms);// need to add SP
                    }
                    catch (Exception ex) { }
                    parms.Clear();

                    List<ConditionCodeDetail> details = new List<ConditionCodeDetail>();
                    details = Claims.SelectConditionCodeDetails(ClaimID);
                    ConditionCodeData = details;

                }
            }
            return Ok(ConditionCodeData);
        }

        [HttpGet]
        [Route("GetConditionCode")]
        public IActionResult GetConditionCode(string conditioncodeInfoID = "", string ClaimID = "")
        {

            List<ConditionCodeDetail> ConditionCodeDetailsData = new List<ConditionCodeDetail>();

            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetConditionCodeData(conditioncodeInfoID, ClaimID);
            var data = string.Empty;
            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ConditionCodeDetail ConditionCodeDetailsInfodata = new ConditionCodeDetail();
                    ConditionCodeDetailsInfodata.Line = dr["Line"].ToString();
                    // ValueCodeDetailsInfodata.hdnValue_Code = dr["Line"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCode = dr["Condition_Code"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCodeDesc = dr["Claims_Condition_Code_Description"].ToString();
                    ConditionCodeDetailsInfodata.ClaimID = dr["Claim_ID"].ToString();
                    ConditionCodeDetailsInfodata.hdnConditionCode = dr["Claims_Condition_Code_Information_ID"].ToString();
                    ConditionCodeDetailsData.Add(ConditionCodeDetailsInfodata);
                }
            }

            return Ok(ConditionCodeDetailsData);
        }

        [HttpPost]
        [Route("AddToothQuadrantInfo")]
        public IActionResult AddToothQuadrantInfo(string hdnClaimId = "", string ToothNumber = "", string ToothServiceLine = "",
            string ToothSurface1 = "", string ToothSurface2 = "", string ToothSurface3 = "", string ToothSurface4 = "", string ToothSurface5 = "", string userid = "")
        {

            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();
            if (ToothNumber != null)
            {
                int toothValue;
                bool isNumeric = int.TryParse(ToothNumber, out toothValue);
            }

            string hdnToothPanelId = "0";
            if (Claims.validateToothNumber(ToothNumber) && (!string.IsNullOrEmpty(hdnClaimId)))
            {
                int toothPanelId = 0;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms = new Dictionary<string, string>();
                parms.Add("Service_Line", ToothServiceLine);
                parms.Add("Tooth_Number", ToothNumber);
                parms.Add("Tooth_Surface1", ToothSurface1);
                parms.Add("Tooth_Surface2", ToothSurface2);
                parms.Add("Tooth_Surface3", ToothSurface3);
                parms.Add("Tooth_Surface4", ToothSurface4);
                parms.Add("Tooth_Surface5", ToothSurface5);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Claim_id", hdnClaimId);
                toothPanelId = MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Tooth_and_Surface_Information", parms);
                hdnToothPanelId = toothPanelId.ToString();

            }


            ToothQuadrantInfoList = Claims.GetToothQuadrtantPanelInfo(hdnClaimId);

            return Ok(ToothQuadrantInfoList);

        }



        [HttpGet]
        [Route("GetToothQuadrtantPanelInfodata")]
        public IActionResult GetToothQuadrtantPanelInfodata(string hdnClaimId = "")
        {
            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();
            DataSet dsToothQuadrantInformation = new DataSet();
            if (!String.IsNullOrEmpty(hdnClaimId))
            {
                dsToothQuadrantInformation = Claims.FetchToothInformation(hdnClaimId);
                var data = string.Empty;
                DataTable dt = new DataTable();
                dt = dsToothQuadrantInformation.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.ToothQuadrantInfo ToothQuadrantInfoData = new PDMSRestServices.Models.ToothQuadrantInfo();
                        ToothQuadrantInfoData.Claims_Tooth_and_Surface_Information_ID = dr["Claims_Tooth_and_Surface_Information_ID"].ToString();
                        ToothQuadrantInfoData.ToothServiceLine = dr["Service_Line"].ToString();
                        ToothQuadrantInfoData.ToothNumber = dr["Tooth_Number"].ToString();
                        ToothQuadrantInfoData.ToothSurface1 = dr["ToothSurface1"].ToString();
                        ToothQuadrantInfoData.ToothSurface2 = dr["ToothSurface2"].ToString();
                        ToothQuadrantInfoData.ToothSurface3 = dr["ToothSurface3"].ToString();
                        ToothQuadrantInfoData.ToothSurface4 = dr["ToothSurface4"].ToString();
                        ToothQuadrantInfoData.ToothSurface5 = dr["Toothsurface5"].ToString();

                        ToothQuadrantInfoList.Add(ToothQuadrantInfoData);
                    }
                }
            }

            return Ok(ToothQuadrantInfoList);
        }

        [HttpGet]
        [Route("Get_ToothQuadrantInfoDetails")]
        public IActionResult Get_ToothQuadrantInfoDetails(string Claim_ID = "", string Claims_Tooth_and_Surface_Information_ID = "")
        {
            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();


            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.Get_ToothQuadrantInfoData(Claim_ID, Claims_Tooth_and_Surface_Information_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PDMSRestServices.Models.ToothQuadrantInfo ToothQuadrantInfoData = new PDMSRestServices.Models.ToothQuadrantInfo();
                    ToothQuadrantInfoData.hdnClaimId = dr["Claim_ID"].ToString();
                    ToothQuadrantInfoData.Claims_Tooth_and_Surface_Information_ID = dr["Claims_Tooth_and_Surface_Information_ID"].ToString();
                    ToothQuadrantInfoData.ToothServiceLine = dr["Service_Line"].ToString();
                    ToothQuadrantInfoData.ToothNumber = dr["Tooth_Number"].ToString();
                    ToothQuadrantInfoData.ToothSurface1 = dr["ToothSurface1"].ToString();
                    ToothQuadrantInfoData.ToothSurface2 = dr["ToothSurface2"].ToString();
                    ToothQuadrantInfoData.ToothSurface3 = dr["ToothSurface3"].ToString();
                    ToothQuadrantInfoData.ToothSurface4 = dr["ToothSurface4"].ToString();
                    ToothQuadrantInfoData.ToothSurface5 = dr["ToothSurface5"].ToString();
                    ToothQuadrantInfoList.Add(ToothQuadrantInfoData);
                }
            }
            return Ok(ToothQuadrantInfoList);

        }

        [HttpDelete]
        [Route("DeleteToothQuadrantInfo")]
        public IActionResult DeleteToothQuadrantInfo(string hdnClaimId = "", string Claims_Tooth_and_Surface_Information_ID = "")
        {
            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("claims_tooth_and_surface_information", "Claims_Tooth_and_Surface_Information_ID", Int32.Parse(Claims_Tooth_and_Surface_Information_ID));

            }
            catch (Exception ex) { }
            parms.Clear();
            ToothQuadrantInfoList = Claims.GetToothQuadrtantPanelInfo(hdnClaimId);
            return Ok(ToothQuadrantInfoList);
        }

        [HttpPost]
        [Route("UpdateToothQuadrantInfo")]
        public IActionResult UpdateToothQuadrantInfo(string hdnClaimId = "", string ToothServiceLine = "", string Claims_Tooth_and_Surface_Information_ID = "",
              string Tooth_Number = "", string Tooth_Surface1 = "", string Tooth_Surface2 = "", string Tooth_Surface3 = "", string Tooth_Surface4 = "", string Tooth_Surface5 = "", string userid = "")
        {

            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();
            if (Tooth_Number != null)
            {
                int toothValue;
                bool isNumeric = int.TryParse(Tooth_Number, out toothValue);
                //if (isNumeric && !Tooth_Number.Contains("0") && toothValue < 10)
                //{
                //    Tooth_Number = "0" + Tooth_Number;
                //}
            }
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms = new Dictionary<string, string>();
            if (Claims.validateToothNumber(Tooth_Number) && (!string.IsNullOrEmpty(hdnClaimId)))
            {
                if (Tooth_Number != null)
                {
                    parms.Add("Tooth_Number", Tooth_Number);
                }
                parms.Add("Service_Line", ToothServiceLine);
                parms.Add("Tooth_Surface1", Tooth_Surface1);
                parms.Add("Tooth_Surface2", Tooth_Surface2);
                parms.Add("Tooth_Surface3", Tooth_Surface3);
                parms.Add("Tooth_Surface4", Tooth_Surface4);
                parms.Add("Tooth_Surface5", Tooth_Surface5);
                parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                parms.Add("Claims_Tooth_and_Surface_Information_ID", Claims_Tooth_and_Surface_Information_ID);
                parms.Add("CLAIM_ID", hdnClaimId);
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Tooth_and_Surface_Information", parms);
                ToothQuadrantInfoList = Claims.GetToothQuadrtantPanelInfo(hdnClaimId);
            }
            return Ok(ToothQuadrantInfoList);

        }


        [HttpGet]
        [Route("GetPriorToothSurfaceList")]
        public IActionResult GetPriorToothSurfaceList()
        {
            List<ToothSurfaceList> ListToothSurface1 = new List<ToothSurfaceList>();

            DataSet dataSetToothSurface = MAXIMUS.Controllers.PDMS.LookupTableController.GetToothSurface();
            DataTable dt = new DataTable();
            if (Helper.HasRows(dataSetToothSurface))
            {
                dt = dataSetToothSurface.Tables[0];

            }
            ListToothSurface1 = Claims.GetToothSurfaceList(dt);
            return Ok(ListToothSurface1);

        }


        [HttpGet]
        [Route("GetToothSurface1List")]
        public IActionResult GetToothSurface1List(string ToothSurface1 = "")
        {
            List<ToothSurfaceList> ListToothSurface1 = new List<ToothSurfaceList>();

            DataSet dataSetToothSurface = MAXIMUS.Controllers.PDMS.LookupTableController.GetToothSurface();
            DataTable selectedTable = new DataTable();
            if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ToothSurface1))
            {
                DataTable dt = dataSetToothSurface.Tables[0];
                selectedTable = dt.AsEnumerable()
                                   .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface1))
                                   .CopyToDataTable();
            }
            ListToothSurface1 = Claims.GetToothSurfaceList(selectedTable);
            return Ok(ListToothSurface1);

        }

        [HttpGet]
        [Route("GetToothSurface2")]
        public IActionResult GetToothSurface2(string ToothSurface1 = "", string ToothSurface2 = "")
        {
            List<ToothSurfaceList> ListToothSurface2 = new List<ToothSurfaceList>();

            DataSet dataSetToothSurface = MAXIMUS.Controllers.PDMS.LookupTableController.GetToothSurface();
            DataTable selectedTable = new DataTable();
            if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ToothSurface2))
            {
                DataTable dt = dataSetToothSurface.Tables[0];
                selectedTable = dt.AsEnumerable()
                                   .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface1) &&
                                  r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface2))
                                   .CopyToDataTable();
            }

            ListToothSurface2 = Claims.GetToothSurfaceList(selectedTable);
            return Ok(ListToothSurface2);

        }

        [HttpGet]
        [Route("GetToothSurface3")]
        public IActionResult GetToothSurface3(string ToothSurface1 = "", string ToothSurface2 = "", string ToothSurface3 = "")
        {
            List<ToothSurfaceList> ListToothSurface3 = new List<ToothSurfaceList>();

            DataSet dataSetToothSurface = MAXIMUS.Controllers.PDMS.LookupTableController.GetToothSurface();
            DataTable selectedTable = new DataTable();
            if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ToothSurface3))
            {
                DataTable dt = dataSetToothSurface.Tables[0];
                selectedTable = dt.AsEnumerable()
                                   .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface1) &&
                                               r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface2) &&
                                               r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface3))
                                   .CopyToDataTable();
            }
            ListToothSurface3 = Claims.GetToothSurfaceList(selectedTable);
            return Ok(ListToothSurface3);

        }

        [HttpGet]
        [Route("GetToothSurface4")]
        public IActionResult GetToothSurface4(string ToothSurface1 = "", string ToothSurface2 = "", string ToothSurface3 = "", string ToothSurface4 = "")
        {
            List<ToothSurfaceList> ListToothSurface4 = new List<ToothSurfaceList>();

            DataSet dataSetToothSurface = MAXIMUS.Controllers.PDMS.LookupTableController.GetToothSurface();
            DataTable selectedTable = new DataTable();
            if (Helper.HasRows(dataSetToothSurface) && !string.IsNullOrWhiteSpace(ToothSurface4))
            {
                DataTable dt = dataSetToothSurface.Tables[0];
                selectedTable = dt.AsEnumerable()
                                   .Where(r => r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface1) &&
                                               r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface2) &&
                                               r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface3) &&
                                               r.Field<int>("PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID") != Convert.ToInt32(ToothSurface4))
                                   .CopyToDataTable();
            }
            ListToothSurface4 = Claims.GetToothSurfaceList(selectedTable);
            return Ok(ListToothSurface4);

        }

        [HttpPost]
        [Route("AddOtherPayerAdjustmentDetails")]
        public IActionResult AddOtherPayerAdjustmentDetails(string Claim_ID = "", string serviceline = "", string lblAdjRevenueCode = "", string lblAdjProc_Code = "", string ddlOPPAdjustmentHealthPlanID = "", string txtOtherPayerReasonCode = "", string ddlOPPAdjustmentGroup = "", string txtOPPAdjustmentAmount = "",
            string txtOPPAdjustmentQuantity = "", string ClaimType = "", string lblOtherPayerAdjustmenterrormsg = "", string userid = "")
        {
            lblOtherPayerAdjustmenterrormsg = string.Empty;
            serviceline = Convert.ToInt32(serviceline).ToString();

            List<OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjustmentServiceDetailData = new List<OtherPayerAdjustmentInfoServiceDetail>();

            if (!string.IsNullOrEmpty(Claim_ID))
            {
                DataSet OtherpayerAdjust = new DataSet();
                Dictionary<string, string> parm = new Dictionary<string, string>();
                parm.Add("Claim_ID", Claim_ID.ToString());
                OtherpayerAdjust = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parm);
                lblOtherPayerAdjustmenterrormsg = "";
                if (Helper.HasRows(OtherpayerAdjust))
                {
                    DataTable dt = OtherpayerAdjust.Tables[0];
                    for (int i = 0; i < OtherpayerAdjust.Tables[0].Rows.Count; i++)
                    {
                        string OtherPayerReasonCode = OtherpayerAdjust.Tables[0].Rows[i]["Reason_Code"].ToString();
                        string serviceLineNo = Convert.ToInt32(OtherpayerAdjust.Tables[0].Rows[i]["Service_Line"].ToString()).ToString();
                        string drpdownHelthplanId = OtherpayerAdjust.Tables[0].Rows[i]["Health_Plan_ID"].ToString();
                        string drpdownAdjustment_Group = OtherpayerAdjust.Tables[0].Rows[i]["Adjustment_Group"].ToString();
                        if (!string.IsNullOrEmpty(txtOtherPayerReasonCode))
                        {
                            if (txtOtherPayerReasonCode.Equals(OtherPayerReasonCode) && drpdownAdjustment_Group.Equals(ddlOPPAdjustmentGroup)
                                && drpdownHelthplanId.Equals(ddlOPPAdjustmentHealthPlanID) && serviceLineNo.Equals(serviceline))
                            {
                                lblOtherPayerAdjustmenterrormsg = "Same reason code cannot be reported multiple times with same adjustment group for the same payer.";
                            }
                        }
                    }
                }
                else
                {
                    DataTable dtreasoncode = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Trim(), null);
                    if (Helper.HasRows(dtreasoncode))
                    {
                        if (dtreasoncode.Rows.Count >= 1)
                        {
                            txtOtherPayerReasonCode = txtOtherPayerReasonCode.ToUpper();
                        }
                        else
                        {
                            lblOtherPayerAdjustmenterrormsg = "Invalid Reason Code";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(lblOtherPayerAdjustmenterrormsg))
                {
                    OtherPayerAdjustmentInfoServiceDetail OtherPayeAdjInfodetails = new OtherPayerAdjustmentInfoServiceDetail();
                    OtherPayeAdjInfodetails.Health_Plan_ID = null;
                    OtherPayeAdjInfodetails.Adjustment_Group = null;
                    OtherPayeAdjInfodetails.Reason_Code = null;
                    OtherPayeAdjInfodetails.Amount = null;
                    OtherPayeAdjInfodetails.Quantity = null;
                    OtherPayeAdjInfodetails.Claim_ID = null;
                    OtherPayeAdjInfodetails.Error_Message = lblOtherPayerAdjustmenterrormsg;

                    OtherPayerAdjustmentServiceDetailData.Add(OtherPayeAdjInfodetails);
                    return Ok(OtherPayerAdjustmentServiceDetailData);
                }
                else
                {
                    OtherPayerAdjustmentServiceDetailData = new List<OtherPayerAdjustmentInfoServiceDetail>();
                    Dictionary<string, string> parms = new Dictionary<string, string>();

                    parms.Add("Service_Line", serviceline);
                    parms.Add("Procedure_Code", lblAdjProc_Code);
                    parms.Add("Health_Plan_ID", ddlOPPAdjustmentHealthPlanID);
                    parms.Add("Adjustment_Group", ddlOPPAdjustmentGroup);
                    parms.Add("Reason_Code", txtOtherPayerReasonCode);
                    parms.Add("Amount", txtOPPAdjustmentAmount);

                    string Quantity = null;
                    if (!string.IsNullOrEmpty(txtOPPAdjustmentQuantity))
                    {
                        decimal quantity = Convert.ToDecimal(txtOPPAdjustmentQuantity);
                        Quantity = (Math.Round(quantity)).ToString();
                    }
                    parms.Add("Quantity", Quantity);
                    parms.Add("Claim_ID", Claim_ID);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    if (ClaimType == "1")
                    {
                        parms.Add("Revenue_Code", lblAdjRevenueCode);
                    }
                    MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);

                    List<OtherPayerAdjustmentInfoServiceDetail> Adjustmentdetails = new List<OtherPayerAdjustmentInfoServiceDetail>();
                    Adjustmentdetails = Claims.SelectOtherPayerAdjustmentServiceDetail(Claim_ID);
                    OtherPayerAdjustmentServiceDetailData = Adjustmentdetails;
                }
            }
            return Ok(OtherPayerAdjustmentServiceDetailData);
        }



        [HttpDelete]
        [Route("DeleteOtherPayerAdjustmentServiceDetail")]
        public IActionResult DeleteOtherPayerAdjustmentServiceDetail(string Claim_ID = "", string Other_Payer_Adjustment_Service_Detail_ID = "")
        {

            List<OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjustmentServiceDetailData = new List<OtherPayerAdjustmentInfoServiceDetail>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Other_Payer_Adjustment_Service_Detail", "Other_Payer_Adjustment_Service_Detail_ID", Convert.ToInt32(Other_Payer_Adjustment_Service_Detail_ID));
            }
            catch (Exception ex) { }
            parms.Clear();
            List<OtherPayerAdjustmentInfoServiceDetail> Adjustmentdetails = new List<OtherPayerAdjustmentInfoServiceDetail>();
            Adjustmentdetails = Claims.SelectOtherPayerAdjustmentServiceDetail(Claim_ID);
            OtherPayerAdjustmentServiceDetailData = Adjustmentdetails;
            return Ok(OtherPayerAdjustmentServiceDetailData);

        }

        [HttpPut]
        [Route("UpdateOtherPayerAdjustmentServiceDetailPanel")]
        public IActionResult UpdateOtherPayerAdjustmentServiceDetailPanel(string OtherPayerAdjustmentServiceDetail_ID = "", string Claim_ID = "", string serviceline = "",
            string lblAdjRevenueCode = "", string lblAdjProc_Code = "", string ddlOPPAdjustmentHealthPlanID = "", string ddlOPPAdjustmentGroup = "",
            string txtOtherPayerReasonCode = "", string txtOPPAdjustmentAmount = "", string txtOPPAdjustmentQuantity = "", string ClaimType = "",
            string lblOtherPayerAdjustmenterrormsg = "", string userid = "")
        {
            lblOtherPayerAdjustmenterrormsg = string.Empty;

            List<OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjustmentServiceDetailData = new List<OtherPayerAdjustmentInfoServiceDetail>();
            if (!string.IsNullOrEmpty(Claim_ID))
            {
                DataSet OtherpayerAdjust = new DataSet();
                Dictionary<string, string> parm = new Dictionary<string, string>();
                parm.Add("Claim_ID", Claim_ID.ToString());
                OtherpayerAdjust = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parm);
                bool IsErrorMsgExist = false;
                if (Helper.HasRows(OtherpayerAdjust) && OtherpayerAdjust.Tables[0].Rows.Count > 1)
                {
                    int samepayer = 0;
                    DataTable dt = OtherpayerAdjust.Tables[0];
                    for (int i = 0; i < OtherpayerAdjust.Tables[0].Rows.Count; i++)
                    {
                        string OtherPayerAdjustmentServiceDetailID = OtherpayerAdjust.Tables[0].Rows[i]["Other_Payer_Adjustment_Service_Detail_ID"].ToString();
                        string OtherPayerReasonCode = OtherpayerAdjust.Tables[0].Rows[i]["Reason_Code"].ToString();
                        string serviceLineNo = OtherpayerAdjust.Tables[0].Rows[i]["Service_Line"].ToString();
                        string drpdownHelthplanId = OtherpayerAdjust.Tables[0].Rows[i]["Health_Plan_ID"].ToString();
                        string drpdownAdjustment_Group = OtherpayerAdjust.Tables[0].Rows[i]["Adjustment_Group"].ToString();
                        if (!string.IsNullOrEmpty(txtOtherPayerReasonCode))
                        {
                            if (txtOtherPayerReasonCode == OtherPayerReasonCode && drpdownAdjustment_Group == ddlOPPAdjustmentGroup
                                && drpdownHelthplanId == ddlOPPAdjustmentHealthPlanID && OtherPayerAdjustmentServiceDetailID != OtherPayerAdjustmentServiceDetail_ID && serviceLineNo == serviceline && OtherpayerAdjust.Tables[0].Rows.Count > 1)
                            {
                                lblOtherPayerAdjustmenterrormsg = "Same reason code cannot be reported multiple times with same adjustment group for the same payer. ";
                            }
                        }
                    }
                }
                DataTable dtreasoncode = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Trim(), null);
                if (Helper.HasRows(dtreasoncode))
                {
                    if (dtreasoncode.Rows.Count >= 1)
                    {
                        txtOtherPayerReasonCode = txtOtherPayerReasonCode.ToUpper();
                    }
                    else
                    {
                        lblOtherPayerAdjustmenterrormsg = "Invalid Reason Code";
                    }
                }
                if (!string.IsNullOrEmpty(lblOtherPayerAdjustmenterrormsg))
                {
                    OtherPayerAdjustmentInfoServiceDetail OtherPayeAdjInfodetails = new OtherPayerAdjustmentInfoServiceDetail();
                    OtherPayeAdjInfodetails.Health_Plan_ID = null;
                    OtherPayeAdjInfodetails.Adjustment_Group = null;
                    OtherPayeAdjInfodetails.Reason_Code = null;
                    OtherPayeAdjInfodetails.Amount = null;
                    OtherPayeAdjInfodetails.Quantity = null;
                    OtherPayeAdjInfodetails.Claim_ID = null;
                    OtherPayeAdjInfodetails.Error_Message = lblOtherPayerAdjustmenterrormsg;

                    OtherPayerAdjustmentServiceDetailData.Add(OtherPayeAdjInfodetails);
                    return Ok(OtherPayerAdjustmentServiceDetailData);
                }
                if (!IsErrorMsgExist)
                {
                    OtherPayerAdjustmentServiceDetailData = new List<OtherPayerAdjustmentInfoServiceDetail>();
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Claim_ID", Claim_ID);
                    parms.Add("Other_Payer_Adjustment_Service_Detail_ID", OtherPayerAdjustmentServiceDetail_ID);
                    parms.Add("Service_Line", serviceline);
                    parms.Add("Health_Plan_ID", ddlOPPAdjustmentHealthPlanID);
                    parms.Add("Adjustment_Group", ddlOPPAdjustmentGroup);
                    parms.Add("Reason_Code", txtOtherPayerReasonCode);
                    parms.Add("Amount", txtOPPAdjustmentAmount.ToString());
                    parms.Add("Quantity", txtOPPAdjustmentQuantity);
                    parms.Add("Procedure_Code", lblAdjProc_Code.ToString());
                    parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
                    if (ClaimType == CON.ClaimType.INSTITUTIONAL.ToString())
                    {
                        parms.Add("Revenue_Code", lblAdjRevenueCode.ToString());
                    }
                    else
                    {
                        parms.Add("Revenue_Code", lblAdjRevenueCode.ToString());
                    }
                    try
                    {
                        MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);
                    }
                    catch (Exception ex)
                    { }
                    parms.Clear();

                    List<OtherPayerAdjustmentInfoServiceDetail> Adjustmentdetails = new List<OtherPayerAdjustmentInfoServiceDetail>();
                    Adjustmentdetails = Claims.SelectOtherPayerAdjustmentServiceDetail(Claim_ID);
                    OtherPayerAdjustmentServiceDetailData = Adjustmentdetails;
                }
            }
            return Ok(OtherPayerAdjustmentServiceDetailData);

        }

        [HttpGet]
        [Route("ddlServiceLine_SelectedIndexChanged")]
        public IActionResult ddlServiceLine_SelectedIndexChanged(string ServiceLine = "", string ClaimType = "", string ClaimId = "")
        {
            var data = string.Empty;
            List<SqlParameter> param = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(ClaimId) && (!string.IsNullOrEmpty(ServiceLine.Trim())))
            {
                param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
                DataSet dsServiceLineNo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");

                if (Helper.HasRows(dsServiceLineNo) && !string.IsNullOrEmpty(ServiceLine))
                {
                    DataTable dt = dsServiceLineNo.Tables[0];
                    IEnumerable<DataRow> dtDetails = from row in dt.AsEnumerable()
                                                     where row.Field<int>("Service_Line") == Convert.ToInt32(ServiceLine)
                                                     select row;
                    if (dtDetails.Any())
                    {
                        DataTable dtServiceLine = dtDetails.CopyToDataTable();
                        data = JsonConvert.SerializeObject(dtServiceLine);
                    }
                }
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("txtOtherPayerReasonCode_TextChanged")]
        public IActionResult txtOtherPayerReasonCode_TextChanged(string OtherPayerReasonCode = "")
        {
            DataTable dt = LookupTableController.GetCrcReasoneCode(OtherPayerReasonCode, "");
            var data = string.Empty;
            data = JsonConvert.SerializeObject(dt);

            return Ok(data);
        }


        [HttpPut]
        [Route("EditOtherPayerAdjustmentServiceDetail")]
        public IActionResult EditOtherPayerAdjustmentServiceDetail(string Other_Payer_Adjustment_Service_Detail_ID = "", string Claim_ID = "", string ClaimType = "")
        {
            List<OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjustmentInfoData = new List<OtherPayerAdjustmentInfoServiceDetail>();


            DataSet AdjustmentDetails = MAXIMUS.Controllers.PDMS.ClaimsController.EditOtherPayerAdjustmentInfoServiceDetails(Other_Payer_Adjustment_Service_Detail_ID, Claim_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = AdjustmentDetails.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                OtherPayerAdjustmentInfoServiceDetail AdjustmentInfoServiceDetail = new OtherPayerAdjustmentInfoServiceDetail();
                AdjustmentInfoServiceDetail.Service_Line = dr["Service_Line"].ToString();
                AdjustmentInfoServiceDetail.Other_Payer_Adjustment_Service_Detail_ID = dr["Other_Payer_Adjustment_Service_Detail_ID"].ToString();
                AdjustmentInfoServiceDetail.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                AdjustmentInfoServiceDetail.Adjustment_Group = dr["Adjustment_Group"].ToString();
                AdjustmentInfoServiceDetail.Reason_Code = dr["Reason_Code"].ToString();
                AdjustmentInfoServiceDetail.Amount = dr["Amount"].ToString();
                AdjustmentInfoServiceDetail.Quantity = dr["Quantity"].ToString();
                AdjustmentInfoServiceDetail.Claim_ID = dr["Claim_ID"].ToString();
                AdjustmentInfoServiceDetail.Procedure_Code = dr["Procedure_Code"].ToString();
                if (ClaimType == "1")
                {
                    AdjustmentInfoServiceDetail.Revenue_Code = dr["Revenue_Code"].ToString();
                }
                OtherPayerAdjustmentInfoData.Add(AdjustmentInfoServiceDetail);
            }
            return Ok(OtherPayerAdjustmentInfoData);

        }

        [HttpGet]
        [Route("txtCheckForPerson_TextChanged")]
        public IActionResult txtCheckForPerson_TextChanged(string MedicaidID = "", string Username = "", string DateOfBirth = "", string MedicaidBillingNumber = "")
        {
            Models.RecipientInformation recipientInformation = null;
            try
            {
                Guid userId = HelperFacade.GetUserId(Username);
                //string error = string.Empty;
                var recipientInfo = Claims.FindRecipient(MedicaidID, userId, MedicaidBillingNumber, DateOfBirth);
                if (recipientInfo != null)
                {
                    recipientInformation = new Models.RecipientInformation();
                    recipientInformation.AddressLine1 = recipientInfo.AddressLine1;
                    recipientInformation.AddressLine2 = recipientInfo.AddressLine2;
                    recipientInformation.FirstName = recipientInfo.FirstName;
                    recipientInformation.LastName = recipientInfo.LastName;
                    recipientInformation.MiddleName = recipientInfo.MiddleName;
                    recipientInformation.City = recipientInfo.City;
                    recipientInformation.StateCode = recipientInfo.StateCode;
                    recipientInformation.ZipCode5 = recipientInfo.ZipCode5;
                    recipientInformation.DateOfBirth = recipientInfo.DateOfBirth;
                    recipientInformation.DateOfDeath = recipientInfo.DateOfDeath;
                    recipientInformation.Gender = recipientInfo.Gender;
                    recipientInformation.ErrorMsg = recipientInfo.ErrorMsg;
                    recipientInformation.MedicaidId = recipientInfo.MedicaidId;
                    recipientInformation.SSN = recipientInfo.SSN;
                    List<Models.ErrorDetail> errors = null;
                    if (recipientInfo.Errors != null)
                    {
                        errors = new List<Models.ErrorDetail>();
                        foreach (var error in recipientInfo.Errors)
                        {
                            var err = new Models.ErrorDetail();
                            err.Code = error.Code;
                            err.Description = error.Description;
                            errors.Add(err);
                        }
                        recipientInformation.Errors = errors;
                    }
                }
                else
                {
                    recipientInformation = new Models.RecipientInformation();
                    recipientInformation.Errors = new List<Models.ErrorDetail>()
                    {
                        new Models.ErrorDetail()
                        {
                          Code="Exception",
                          Description= "Error: Enter a Valid recipient information"
                        }
                     };
                    return BadRequest(recipientInformation.Errors);
                }
            }
            catch
            {
                recipientInformation = new Models.RecipientInformation();
                //recipientInformation.ErrorMsg = "Error: An error occurred while processing the request";
                recipientInformation.Errors = new List<Models.ErrorDetail>()
                {
                    new Models.ErrorDetail()
                    {
                        Code="Exception",
                        Description= "Error: An error occurred while processing the request"
                    }
                };

                return BadRequest(recipientInformation.Errors);
            }
            return Ok(recipientInformation);
        }


        [HttpPost]
        [Route("AddOPPAServiceDetails")]
        public IActionResult AddOPPAServiceDetails(string ClaimID = "", string Service_Line = "", string Procedure_Code = "",
            string Health_Plan_ID = "", string Amount_Paid = "", string Paid_Date = "", string Paid_unit_Count = "",
        string ClaimType = "", string Revenue_Code = "", string userid = "")
        {
            Service_Line = Convert.ToInt32(Service_Line).ToString();
            String errorMessage = string.Empty;
            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();
            DataSet bindGrid = Claims.GetOtherPayerAdjustmentInfo(ClaimID);
            OtherPayerAdjustmentInfoServiceDetail OtherPayeAdjInfodetails = new OtherPayerAdjustmentInfoServiceDetail();

            DataTable dtBindgrid = new DataTable();
            List<OtherPayerAdjustmentInfoServiceDetail> AdjustmentDetail = new List<OtherPayerAdjustmentInfoServiceDetail>();
            dtBindgrid = bindGrid.Tables[0];

            if (dtBindgrid.Rows.Count > 0 && OtherPayeAdjInfodetails.Service_Line != "")
            {
                foreach (DataRow dr in dtBindgrid.Rows)
                {
                    string iServiceLine = Convert.ToInt32(dr["Service_Line"].ToString()).ToString();
                    string iProcedureCode = dr["Procedure_Code"].ToString();
                    string iHealthPlanId = dr["Health_Plan_ID"].ToString();
                    string iAdjustmentGroup = dr["Adjustment_Group"].ToString();
                    string iReasonCode = dr["Reason_Code"].ToString();

                    if (iServiceLine.Equals(Service_Line) && iHealthPlanId.Equals(Health_Plan_ID))
                    {
                        errorMessage = "Only one payer may be defined per service line";
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(errorMessage))
            {

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", Service_Line);
                parms.Add("Procedure_Code", Procedure_Code);
                parms.Add("Health_Plan_ID", Health_Plan_ID);
                parms.Add("Amount_Paid", Amount_Paid);
                parms.Add("Paid_Date", Paid_Date);
                parms.Add("Paid_unit_Count", Paid_unit_Count);
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    if (ClaimType == CON.ClaimsType.Institutional)
                    {
                        parms.Add("Revenue_Code", Revenue_Code);
                    }
                    else
                    {
                        parms.Add("Revenue_Code", null);
                    }
                    parms.Add("Claim_ID", ClaimID);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
                }
                OPPAServiceDetailList = Claims.GetOtherPayerPaidAmount(ClaimID);
            }
            return Ok(OPPAServiceDetailList);

        }

        [HttpGet]
        [Route("GetServiceLineNoFromServiceDetailPanel")]
        public IActionResult GetServiceLineNoFromServiceDetailPanel(string ClaimID = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataSet dsOPPAmount = new DataSet();
            DataSet dsServiceNo = new DataSet();
            List<ServiceLineOPPA> ServicelineList = new List<ServiceLineOPPA>();
            if (!string.IsNullOrEmpty(ClaimID))
            {
                param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimID, true));
                dsServiceNo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
                if (Helper.HasRows(dsServiceNo) &&
                    Convert.ToInt32(dsServiceNo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimID))
                {
                    DataTable dtServiceNo = dsServiceNo.Tables[0];
                    if (dtServiceNo.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtServiceNo.Rows)
                        {
                            ServiceLineOPPA ServiceLinedata = new ServiceLineOPPA();
                            ServiceLinedata.Service_Line = dr["Service_Line"].ToString();
                            ServiceLinedata.Claims_Service_Details_ID = dr["Claims_Service_Details_ID"].ToString();
                            ServicelineList.Add(ServiceLinedata);
                        }
                    }
                }
            }
            return Ok(ServicelineList);

        }

        [HttpGet]
        [Route("GetHealthPlanIDForOPPAmountServiceDetail")]
        public IActionResult GetHealthPlanIDForOPPAmountServiceDetail(string ClaimId = "")
        {
            return Ok(Claims.GetHealthPlanIDForOPPAmountServiceDetail(ClaimId));

        }

        [HttpGet]
        [Route("GetOtherPayerPaidAmountBind")]
        public IActionResult GetOtherPayerPaidAmountBind(string ClaimId = "", string ClaimType = "")
        {

            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOPPAmount = new DataSet();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOPPAmount = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
            }
            dt = dsOPPAmount.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                OPPAServiceDetail OPPAServiceDetailData = new OPPAServiceDetail();
                OPPAServiceDetailData.OPPAServiceDetailId = dr["Other_Payer_Adjudication_Information_Service_Detail_ID"].ToString();
                OPPAServiceDetailData.ClaimId = dr["Claim_ID"].ToString();
                OPPAServiceDetailData.Service_Line = dr["Service_Line"].ToString();
                OPPAServiceDetailData.Procedure_Code = dr["Procedure_Code"].ToString();
                OPPAServiceDetailData.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                OPPAServiceDetailData.Amount_Paid = dr["Amount_Paid"].ToString();
                OPPAServiceDetailData.Paid_Date = Convert.ToDateTime(dr["Paid_Date"]).ToString("MM/dd/yyyy");
                OPPAServiceDetailData.Paid_unit_Count = dr["Paid_unit_Count"].ToString();
                OPPAServiceDetailData.Revenue_Code = dr["Revenue_Code"].ToString();
                OPPAServiceDetailList.Add(OPPAServiceDetailData);
            }
            return Ok(OPPAServiceDetailList);

        }


        [HttpPost]
        [Route("EditOPPAServiceDetailLineItem")]
        public IActionResult EditOPPAServiceDetailLineItem(string ClaimId = "", string OPPAServiceDetailId = "")
        {

            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();

            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.GetOPPAServiceDetail(ClaimId, OPPAServiceDetailId);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OPPAServiceDetail OPPAServiceDetailData = new OPPAServiceDetail();
                    OPPAServiceDetailData.OPPAServiceDetailId = dr["Other_Payer_Adjudication_Information_Service_Detail_ID"].ToString();
                    OPPAServiceDetailData.ClaimId = dr["Claim_ID"].ToString();
                    OPPAServiceDetailData.Service_Line = dr["Service_Line"].ToString();
                    OPPAServiceDetailData.Procedure_Code = dr["Procedure_Code"].ToString();
                    OPPAServiceDetailData.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                    OPPAServiceDetailData.Amount_Paid = dr["Amount_Paid"].ToString();
                    OPPAServiceDetailData.Paid_Date = Convert.ToDateTime(dr["Paid_Date"]).ToString("MM/dd/yyyy");
                    OPPAServiceDetailData.Paid_unit_Count = dr["Paid_unit_Count"].ToString();
                    OPPAServiceDetailData.Revenue_Code = dr["Revenue_Code"].ToString();

                    OPPAServiceDetailList.Add(OPPAServiceDetailData);
                }
            }
            return Ok(OPPAServiceDetailList);
        }

        [HttpDelete]
        [Route("DeleteOPPAServiceDetail")]
        public IActionResult DeleteOPPAServiceDetail(string ClaimID = "", string OPPAServiceDetailId = "")
        {
            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();

            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", "Other_Payer_Adjudication_Information_Service_Detail_ID", Convert.ToInt32(OPPAServiceDetailId));
            }
            catch (Exception ex) { }

            OPPAServiceDetailList = Claims.GetOtherPayerPaidAmount(ClaimID);
            return Ok(OPPAServiceDetailList);

        }

        [HttpPost]
        [Route("UpdateOPPAserviceDeatils")]
        public IActionResult UpdateOPPAserviceDeatils(string ClaimID = "", string OPPAServiceDetailID = "", string Service_Line = "",
            string Health_Plan_ID = "", string Amount_Paid = "", string Paid_unit_Count = "", string Revenue_Code = "", string ClaimType = "",
            string Procedure_Code = "", string userid = "")
        {
            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();
            DataSet datasetOtherPayerPaidAmount = new DataSet();

            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Claim_ID", ClaimID);
            datasetOtherPayerPaidAmount = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parm);

            if ((Health_Plan_ID != null) && (Service_Line != null))
            {
                DataSet Otherpayer = new DataSet();
                Otherpayer = datasetOtherPayerPaidAmount;
                DataTable dt = Otherpayer.Tables[0];

                if (dt.AsEnumerable().Any(p => p.Field<int>("Other_Payer_Adjudication_Information_Service_Detail_ID") != Convert.ToInt32(OPPAServiceDetailID) && p.Field<string>("Health_Plan_ID") == Health_Plan_ID.ToString() && p.Field<int>("Service_Line") == Convert.ToInt32(Service_Line.ToString())))
                {

                    OPPAServiceDetail OPPAServiceDetailData = new OPPAServiceDetail();
                    OPPAServiceDetailData.ErrorMsg = "Same Health Plan ID can not be add";
                    OPPAServiceDetailList.Add(OPPAServiceDetailData);
                    return Ok(OPPAServiceDetailList);

                }
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("Claim_id", ClaimID.ToString());
            param.Add("Service_Line", Service_Line);
            DataSet ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("claims_other_payer_adjudication_servicedetail_with_serviceline", param);
            DataTable dtBillUnit = ds.Tables[0];
            IEnumerable<DataRow> dtBillUnitDetails = from row in dtBillUnit.AsEnumerable()
                                                     where row.Field<int>("Other_Payer_Adjudication_Information_Service_Detail_ID") == Convert.ToInt32(OPPAServiceDetailID)
                                                     select row;

            if (dtBillUnitDetails.Any())
            {
                DataTable dtbill = dtBillUnitDetails.CopyToDataTable();
                string res = dtbill.Rows[0]["Billed_Units"].ToString();
                if (Convert.ToDouble(Paid_unit_Count.ToString()) > Convert.ToDouble(res))
                {
                    OPPAServiceDetail OPPAServiceDetailData = new OPPAServiceDetail();
                    OPPAServiceDetailData.ErrorMsg = "Other payer paid service unit count cannot be greater than billed unit";
                    OPPAServiceDetailList.Add(OPPAServiceDetailData);
                    return Ok(OPPAServiceDetailList);
                }

            }
            Dictionary<string, string> parms = new Dictionary<string, string>();

            parms.Add("Claim_ID", ClaimID.ToString());
            parms.Add("Other_Payer_Adjudication_Information_Service_Detail_ID", OPPAServiceDetailID.ToString());
            parms.Add("Service_Line", Service_Line.ToString());
            parms.Add("Health_Plan_ID", Health_Plan_ID.ToString());
            parms.Add("Amount_Paid", Amount_Paid.ToString());
            parms.Add("Paid_unit_Count", Paid_unit_Count.ToString());
            parms.Add("Procedure_Code", string.IsNullOrEmpty(Procedure_Code) ? "" : Procedure_Code.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userid).ToString());
            if (ClaimType == CON.ClaimType.INSTITUTIONAL.ToString())
            {
                parms.Add("Revenue_Code", Revenue_Code.ToString());
            }
            else
            {
                parms.Add("Revenue_Code", Revenue_Code.ToString());
            }
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
            }
            catch (Exception ex)
            {

            }

            OPPAServiceDetailList = Claims.GetOtherPayerPaidAmount(ClaimID);
            return Ok(OPPAServiceDetailList);
        }

        [HttpGet]
        [Route("ddlDetailId_SelectedIndexChanged")]
        public IActionResult ddlDetailId_SelectedIndexChanged(string Claim_ID = "", string Service_Line = "", string ClaimType = "")
        {
            var OPPAServiceDetailData = string.Empty;

            List<SqlParameter> param = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(Claim_ID) && (!string.IsNullOrEmpty(Service_Line.Trim())))
            {
                param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, true));
                DataSet dsServiceLine = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
                if (Helper.HasRows(dsServiceLine) && !string.IsNullOrEmpty(Service_Line))
                {
                    DataTable dt = dsServiceLine.Tables[0];
                    IEnumerable<DataRow> dtDetails = from row in dt.AsEnumerable()
                                                     where row.Field<int>("Service_Line") == Convert.ToInt32(Service_Line)
                                                     select row;
                    if (dtDetails.Any())
                    {
                        DataTable dtServiceLine = dtDetails.CopyToDataTable();
                        OPPAServiceDetailData = JsonConvert.SerializeObject(dtServiceLine);
                    }
                }
            }
            return Ok(OPPAServiceDetailData);

        }

        [HttpDelete]
        [Route("RemoveAlreadyAddedHealthPlanID")]
        public IActionResult RemoveAlreadyAddedHealthPlanID(string ClaimID = "", string serviceLine = "")
        {
            List<HealthPlanIDOPPA> HealthPlanIDOPPAList = new List<HealthPlanIDOPPA>();
            Dictionary<string, string> param = new Dictionary<string, string>();
            DataSet dsHealthID = new DataSet();
            DataTable dthealthplanidlist = new DataTable();
            param.Add("Claim_ID", ClaimID.ToString());
            dsHealthID = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("claims_other_payer_information", param);

            if (Helper.HasRows(dsHealthID) && Convert.ToInt32(dsHealthID.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimID))
            {
                DataTable dt = dsHealthID.Tables[0];
                IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                    where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Details) &&
                                                    row.Field<string>("Claim_Adjudication_Level") != null)
                                                    select row;
                if (healthplanid.Count() > 0)
                {
                    dthealthplanidlist = healthplanid.CopyToDataTable();
                }
            }

            //Removing already added provider for detail / ServiceLine number.

            DataSet dtHealthPlan = Claims.FetchHealthPlanIDWithServiceLine(ClaimID, serviceLine);
            DataTable dtHealthPlanID = dtHealthPlan.Tables[0];

            var HealthPlanID = dtHealthPlanID.AsEnumerable()
                .Select(row => row.Field<string>("Health_Plan_ID"));

            if (Helper.HasRows(dtHealthPlanID))
            {
                foreach (string row in HealthPlanID)
                {
                    for (int i = dthealthplanidlist.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dthealthplanidlist.Rows[i];
                        if (dr["Health_Plan_ID"].ToString() == row)
                            dr.Delete();
                        dthealthplanidlist.AcceptChanges();

                    }
                }
                if (Helper.HasRows(dthealthplanidlist))
                {
                    foreach (DataRow dr in dthealthplanidlist.Rows)
                    {
                        HealthPlanIDOPPA HealthPlanData = new HealthPlanIDOPPA();
                        HealthPlanData.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                        HealthPlanData.Claims_Other_Payer_Information_ID = dr["Claims_Other_Payer_Information_ID"].ToString();
                        HealthPlanIDOPPAList.Add(HealthPlanData);
                    }
                }

            }
            else
            {
                HealthPlanIDOPPAList = Claims.GetHealthPlanIDForOPPAmountServiceDetail(ClaimID);
            }
            return Ok(HealthPlanIDOPPAList);

        }


        [HttpGet]
        [Route("ddlOPPHealhPlanId_SelectedIndexChanged")]
        public IActionResult ddlOPPHealhPlanId_SelectedIndexChanged(string ClaimID = "", string HealthplanId = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var OPPAServiceDetailData = "true";
            try
            {
                if (!string.IsNullOrEmpty(ClaimID))
                {
                    param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimID, true));
                    DataSet dsServiceLineNo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Other_Payer_Information", param, "Claims_other_payer");
                    if (Helper.HasRows(dsServiceLineNo) && !string.IsNullOrEmpty(HealthplanId))
                    {
                        for (int i = 0; dsServiceLineNo.Tables[0].Rows.Count > 0; i++)
                        {
                            if (dsServiceLineNo.Tables[0].Rows[i]["Health_Plan_ID"].ToString() == HealthplanId)
                            {
                                if (!string.IsNullOrEmpty(dsServiceLineNo.Tables[0].Rows[i]["Paid_Date"].ToString()))
                                {
                                    OPPAServiceDetailData = Convert.ToDateTime(dsServiceLineNo.Tables[0].Rows[i]["Paid_Date"]).ToString("MM/dd/yyyy");

                                    return Ok(JsonConvert.SerializeObject(OPPAServiceDetailData));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return Ok(JsonConvert.SerializeObject(OPPAServiceDetailData));
        }


        [HttpGet]
        [Route("GetServiceLineNoForNDCDetails")]
        public IActionResult GetServiceLineNoForNDCDetails(string ClaimID = "")
        {
            List<NDCServiceLine> NDCServiceLineList = new List<NDCServiceLine>();
            List<SqlParameter> param = new List<SqlParameter>();
            DataSet dsServiceLine = new DataSet();
            DataTable dtServiceLine = new DataTable();
            if (!string.IsNullOrEmpty(ClaimID))
            {
                param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimID, true));
                dsServiceLine = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
                dtServiceLine = dsServiceLine.Tables[0];
                if (Helper.HasRows(dsServiceLine) &&
                    Convert.ToInt32(dsServiceLine.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimID))
                {
                    DataTable selectNDC = Claims.GetNDCDetails(ClaimID).Tables[0];
                    var Service_Line = selectNDC.AsEnumerable()
                        .Select(row => row.Field<int>("Service_Line")).ToArray();
                    if (Helper.HasRows(selectNDC))
                    {
                        foreach (int row in Service_Line)
                        {
                            for (int i = dtServiceLine.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow dr = dtServiceLine.Rows[i];
                                string ServiceLine = dr["Service_Line"].ToString();
                                if (Convert.ToInt32(ServiceLine) == row)
                                    dr.Delete();
                                dtServiceLine.AcceptChanges();
                            }
                        }
                    }
                }
            }
            if (Helper.HasRows(dtServiceLine))
            {
                foreach (DataRow dr in dtServiceLine.Rows)
                {
                    NDCServiceLine ServiceLineData = new NDCServiceLine();
                    ServiceLineData.Service_Line = dr["Service_Line"].ToString();
                    ServiceLineData.Claims_Service_Details_ID = dr["Claims_Service_Details_ID"].ToString();
                    NDCServiceLineList.Add(ServiceLineData);
                }
            }
            return Ok(NDCServiceLineList);
        }


        [HttpPost]
        [Route("AddAdditionProviderInformationData")]
        public IActionResult AddAdditionProviderInformationData(string ServiceLine = "", string ProviderType = "", string ProviderNPI = "", string MedicaidId = "",
            string LastName = "", string FirstName = "",
       string MiddleName = "", string Claim_ID = "", string ClaimType = "", string BillingNPI = "", string hdnRenderingProviderNPI = "", string hdnRenderingProvider_MedicaidID = "",
       string hdnAssistantProviderNPI = "",
       string hdnAssistant_MedicaidID = "", string hdnSupervisingProviderNPI = "", string hdnSupervisingProvider_Medicaid = "", string hdnServiceFacilityProviderNPI = "",
       string hdnServiceFacilityProvider_Medicaid = "", string hdnReferringProviderNPI = "", string hdnReferringProvider_Medicaid = "",
       string hdnPrimaryProviderNPI = "", string hdnPrimaryProvider_Medicaid = "", string userid = "")
        {
            if (hdnRenderingProvider_MedicaidID == "null")
                hdnRenderingProvider_MedicaidID = "";
            if (hdnAssistant_MedicaidID == "null")
                hdnAssistant_MedicaidID = "";
            if (hdnSupervisingProvider_Medicaid == "null")
                hdnSupervisingProvider_Medicaid = "";
            if (hdnServiceFacilityProvider_Medicaid == "null")
                hdnServiceFacilityProvider_Medicaid = "";
            if (hdnReferringProvider_Medicaid == "null")
                hdnReferringProvider_Medicaid = "";
            if (hdnPrimaryProvider_Medicaid == "null")
                hdnPrimaryProvider_Medicaid = "";

            if (hdnRenderingProviderNPI == "null")
                hdnRenderingProviderNPI = "";
            if (hdnAssistantProviderNPI == "null")
                hdnAssistantProviderNPI = "";
            if (hdnSupervisingProviderNPI == "null")
                hdnSupervisingProviderNPI = "";
            if (hdnServiceFacilityProviderNPI == "null")
                hdnServiceFacilityProviderNPI = "";
            if (hdnReferringProviderNPI == "null")
                hdnReferringProviderNPI = "";
            if (hdnPrimaryProviderNPI == "null")
                hdnPrimaryProviderNPI = "";
            List<AdditionalProviderInformation> lstAdditionalProviderInformation = new List<AdditionalProviderInformation>();

            AdditionalProviderInformation AdditionalProviderInfo = new AdditionalProviderInformation();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsGetData = Claims.FetchAdditionalProviderInformation(Claim_ID);
            DataTable dt = dsGetData.Tables[0];
            DataTable dtNPI = Claims.GetData(ProviderNPI, MedicaidId, "", "");
            #region Validtions
            if (!Helper.HasRows(dtNPI))
            {
                Claims.GetAdditionalProviderData(null, Claim_ID);
                //return;
            }
            bool result = false;
            bool count = false;
            bool data = false;
            // compare billing NPI with Rendering
            if ((ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderNPI == BillingNPI))
            {
                AdditionalProviderInfo.Error_Msg = "Rendering and Billing NPI Cannot be same.";
                result = true;
            }
            //check if header panels already have data
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && (string.IsNullOrEmpty(hdnRenderingProviderNPI) && string.IsNullOrEmpty(hdnRenderingProvider_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Rendering Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && (string.IsNullOrEmpty(hdnAssistantProviderNPI) && string.IsNullOrEmpty(hdnAssistant_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Assistant Surgeon Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && (string.IsNullOrEmpty(hdnSupervisingProviderNPI) && string.IsNullOrEmpty(hdnSupervisingProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Supervising Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ServiceFacilityProvider && (string.IsNullOrEmpty(hdnServiceFacilityProviderNPI) && string.IsNullOrEmpty(hdnServiceFacilityProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Service Facility information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && (string.IsNullOrEmpty(hdnReferringProviderNPI) && string.IsNullOrEmpty(hdnReferringProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Referring Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && (string.IsNullOrEmpty(hdnPrimaryProviderNPI) && string.IsNullOrEmpty(hdnPrimaryProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Primary Care Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && (string.IsNullOrEmpty(hdnSupervisingProviderNPI) && string.IsNullOrEmpty(hdnSupervisingProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Operating Physician Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && (string.IsNullOrEmpty(hdnAssistantProviderNPI) && string.IsNullOrEmpty(hdnAssistant_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Other Operating Physician Provider information should be reported in the header.";
                result = true;
            }
            if (result)
            {
                lstAdditionalProviderInformation.Add(AdditionalProviderInfo);
            }
            if (!result)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string AdditonalProviderDetail = dr["Service_Line"].ToString();
                        string ProviderTypedropdon = dr["Provider_Type"].ToString();
                        AdditionalProviderInfo = new AdditionalProviderInformation();

                        if (AdditonalProviderDetail == ServiceLine &&
                        ProviderTypedropdon == ProviderType)
                        {
                            count = true;
                            break; // If any one time of loops if already exists return true and break the loop to show the error message.
                        }
                        else
                        {
                            count = false;
                        }

                    }
                    if (count)
                    {
                        AdditionalProviderInfo.Error_Msg = "Same provider type and Service Line number cannot be added again. ";
                        lstAdditionalProviderInformation.Add(AdditionalProviderInfo);
                    }
                    if (!count && !result)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string AdditonalProviderDetail = dr["Service_Line"].ToString();
                            string ProviderTypedropdon = dr["Provider_Type"].ToString();
                            string ProviderNPIGrid = dr["Provider_NPI"].ToString();
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                            ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
                            (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Supervising provider and Rendering provider cannot be same";
                                data = true;
                            }
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
                                (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Assistant surgeon and Supervising provider Cannot be same.";
                                data = true;
                            }
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                 ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider) ||
                                   (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Assistant surgeon and Rendering provider Cannot be same.";
                                data = true;
                            }
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                 ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician) ||
                                 (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Operating physician and Other operating physician NPI Cannot be same.";
                                data = true;
                            }
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                             ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider) ||
                              (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Rendering and Referring Provider NPI Cannot be same.";
                                data = true;
                            }
                            if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                               ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider) ||
                               (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                            {
                                AdditionalProviderInfo.Error_Msg = "Rendering Provider Cannot be same as Primary Care Provider";
                                data = true;
                            }

                            // Compare primary and referring - // Panel level check
                            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider)
                            {
                                DataTable dtPriRef = Claims.FetchAdditionalProviderInformationWithServiceLine(ServiceLine, Claim_ID).Tables[0];
                                DataRow[] refrows = dtPriRef.Select("Provider_Type ='" + CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider + "'");
                                if (refrows.Length == 0)
                                {
                                    AdditionalProviderInfo.Error_Msg = "Primary care provider(referral) cannot be entered without entering referring provider";
                                    data = true;
                                }
                            }
                        }
                    }

                    if (data)
                    {
                        lstAdditionalProviderInformation.Add(AdditionalProviderInfo);
                    }

                }
            }
            #endregion
            if (!result && !count && !data)
            {
                if (!string.IsNullOrEmpty(Claim_ID) && !string.IsNullOrEmpty(ServiceLine))
                {
                    parms.Add("Claim_id", Claim_ID);
                    parms.Add("Service_Line", ServiceLine);
                    parms.Add("Provider_Type", ProviderType);
                    parms.Add("Provider_NPI", ProviderNPI);
                    parms.Add("Last_Name", LastName);
                    parms.Add("First_Name", FirstName);
                    parms.Add("Middle_Name", MiddleName);
                    if (ClaimType == CON.ClaimsType.Professional)
                        parms.Add("Medicaid_ID", MedicaidId);
                    parms.Add("Last_Modified_Date", null);
                    parms.Add("Created_By_User", HelperFacade.GetUserId(userid).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Additional_Provider_Information_Service", parms);
                }
                DataSet AdditionalTabledata = Claims.FetchAdditionalProviderInformation(Claim_ID);
                DataTable dtTable = new DataTable();
                dtTable = AdditionalTabledata.Tables[0];

                if (dtTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTable.Rows)
                    {
                        AdditionalProviderInformation AddAdditionProviderInfo = new AdditionalProviderInformation();
                        AddAdditionProviderInfo.ServiceLine = dr["Service_Line"].ToString();
                        AddAdditionProviderInfo.ProviderType = dr["Provider_Type"].ToString();
                        AddAdditionProviderInfo.ProviderNPI = dr["Provider_NPI"].ToString();
                        AddAdditionProviderInfo.LastName = dr["Last_Name"].ToString();
                        AddAdditionProviderInfo.FirstName = dr["First_Name"].ToString();
                        AddAdditionProviderInfo.MiddleName = dr["Middle_Name"].ToString();
                        AddAdditionProviderInfo.ClaimID = dr["Claim_ID"].ToString();
                        if (ClaimType == CON.ClaimsType.Professional)
                            AddAdditionProviderInfo.MedicaidID = dr["Medicaid_ID"].ToString();
                        AddAdditionProviderInfo.Claims_Additional_Provider_Information_Service_ID = dr["Claims_Additional_Provider_Information_Service_ID"].ToString();

                        lstAdditionalProviderInformation.Add(AddAdditionProviderInfo);
                    }
                }
            }
            return Ok(lstAdditionalProviderInformation);
        }

        [HttpGet]
        [Route("FetchAdditionalProviderInformationdata")]
        public IActionResult FetchAdditionalProviderInformationdata(string Claim_ID = "", string ClaimType = "")
        {
            List<AdditionalProviderInformation> lstAdditionalProviderInformation = new List<AdditionalProviderInformation>();
            DataSet AdditionalTabledata = Claims.FetchAdditionalProviderInformation(Claim_ID);
            DataTable dtTable = new DataTable();
            dtTable = AdditionalTabledata.Tables[0];

            if (dtTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTable.Rows)
                {
                    AdditionalProviderInformation AddAdditionProviderInfo = new AdditionalProviderInformation();
                    AddAdditionProviderInfo.ServiceLine = dr["Service_Line"].ToString();
                    AddAdditionProviderInfo.ProviderType = dr["Provider_Type"].ToString();
                    AddAdditionProviderInfo.ProviderNPI = dr["Provider_NPI"].ToString();
                    AddAdditionProviderInfo.LastName = dr["Last_Name"].ToString();
                    AddAdditionProviderInfo.FirstName = dr["First_Name"].ToString();
                    AddAdditionProviderInfo.MiddleName = dr["Middle_Name"].ToString();
                    AddAdditionProviderInfo.ClaimID = dr["Claim_ID"].ToString();
                    if (ClaimType == CON.ClaimsType.Professional)
                        AddAdditionProviderInfo.MedicaidID = dr["Medicaid_ID"].ToString();
                    AddAdditionProviderInfo.Claims_Additional_Provider_Information_Service_ID = dr["Claims_Additional_Provider_Information_Service_ID"].ToString();

                    lstAdditionalProviderInformation.Add(AddAdditionProviderInfo);
                }
            }
            return Ok(lstAdditionalProviderInformation);

        }

        [HttpDelete]
        [Route("DeleteAdditionalProviderInfo")]
        public IActionResult DeleteAdditionalProviderInfo(string Claim_ID = "", string Claims_Additional_Provider_Information_Service_ID = "", string ClaimType = "")
        {
            List<AdditionalProviderInformation> lstAdditionaldata = new List<AdditionalProviderInformation>();
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.DeletePanelsData("Claims_Additional_Provider_Information_Service", "Claims_Additional_Provider_Information_Service_ID", Convert.ToInt32(Claims_Additional_Provider_Information_Service_ID));
            }
            catch { }

            lstAdditionaldata = Claims.SelectAdditonalProviderInfoDetails(Claim_ID, ClaimType);
            return Ok(lstAdditionaldata);
        }


        [HttpPut]
        [Route("UpdateAdditionalProviderInfo")]
        public IActionResult UpdateAdditionalProviderInfo(string ServiceLine = "", string ProviderType = "", string ProviderNPI = "", string MedicaidId = "", string LastName = "", string FirstName = "", string MiddleName = "", string Claim_ID = "",
            string ClaimType = "", string Claims_Additional_Provider_Information_Service_ID = "",
           string BillingNPI = "", string hdnRenderingProviderNPI = "", string hdnRenderingProvider_MedicaidID = "", string hdnAssistantProviderNPI = "",
           string hdnAssistant_MedicaidID = "", string hdnSupervisingProviderNPI = "", string hdnSupervisingProvider_Medicaid = "",
           string hdnServiceFacilityProviderNPI = "", string hdnServiceFacilityProvider_Medicaid = "",
           string hdnReferringProviderNPI = "", string hdnReferringProvider_Medicaid = "", string hdnPrimaryProviderNPI = "", string hdnPrimaryProvider_Medicaid = "", string userid = "")
        {
            if (hdnRenderingProvider_MedicaidID == "null")
                hdnRenderingProvider_MedicaidID = "";
            if (hdnAssistant_MedicaidID == "null")
                hdnAssistant_MedicaidID = "";
            if (hdnSupervisingProvider_Medicaid == "null")
                hdnSupervisingProvider_Medicaid = "";
            if (hdnServiceFacilityProvider_Medicaid == "null")
                hdnServiceFacilityProvider_Medicaid = "";
            if (hdnReferringProvider_Medicaid == "null")
                hdnReferringProvider_Medicaid = "";
            if (hdnReferringProvider_Medicaid == "null")
                hdnReferringProvider_Medicaid = "";
            if (hdnPrimaryProvider_Medicaid == "null")
                hdnPrimaryProvider_Medicaid = "";
            if (hdnSupervisingProviderNPI == "null")
                hdnSupervisingProviderNPI = "";
            if (hdnServiceFacilityProviderNPI == "null")
                hdnServiceFacilityProviderNPI = "";
            if (hdnReferringProviderNPI == "null")
                hdnReferringProviderNPI = "";
            if (hdnPrimaryProviderNPI == "null")
                hdnPrimaryProviderNPI = "";

            List<AdditionalProviderInformation> lstAddProviderInfoData = new List<AdditionalProviderInformation>();
            AdditionalProviderInformation AdditionalProviderInfo = new AdditionalProviderInformation();
            DataSet dsGetData = Claims.FetchAdditionalProviderInformation(Claim_ID);
            DataTable dt = dsGetData.Tables[0];
            bool result = false;
            bool count = false;
            bool data = false;

            // validation for header panel check
            DataTable dtNPI = Claims.GetNPIForAdditional(ProviderNPI);
            if (!Helper.HasRows(dtNPI))
            {
                Claims.GetAdditionalProviderData(null, Claim_ID);
                //return;
            }

            // compare billing NPI with Rendering
            if ((ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderNPI == BillingNPI))
            {
                AdditionalProviderInfo.Error_Msg = "Rendering and Billing NPI Cannot be same.";
                result = true;
            }
            //check if header panels already have data
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && (string.IsNullOrEmpty(hdnRenderingProviderNPI) && string.IsNullOrEmpty(hdnRenderingProvider_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Rendering Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && (string.IsNullOrEmpty(hdnAssistantProviderNPI) && string.IsNullOrEmpty(hdnAssistant_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Assistant Surgeon Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && (string.IsNullOrEmpty(hdnSupervisingProviderNPI) && string.IsNullOrEmpty(hdnSupervisingProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Supervising Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ServiceFacilityProvider && (string.IsNullOrEmpty(hdnServiceFacilityProviderNPI) && string.IsNullOrEmpty(hdnServiceFacilityProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Service Facility information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && (string.IsNullOrEmpty(hdnReferringProviderNPI) && string.IsNullOrEmpty(hdnReferringProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Referring Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && (string.IsNullOrEmpty(hdnPrimaryProviderNPI) && string.IsNullOrEmpty(hdnPrimaryProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Primary Care Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && (string.IsNullOrEmpty(hdnSupervisingProviderNPI) && string.IsNullOrEmpty(hdnSupervisingProvider_Medicaid)))
            {
                AdditionalProviderInfo.Error_Msg = "Operating Physician Provider information should be reported in the header.";
                result = true;
            }
            if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && (string.IsNullOrEmpty(hdnAssistantProviderNPI) && string.IsNullOrEmpty(hdnAssistant_MedicaidID)))
            {
                AdditionalProviderInfo.Error_Msg = "Other Operating Physician Provider information should be reported in the header.";
                result = true;
            }
            if (result)
            {
                lstAddProviderInfoData.Add(AdditionalProviderInfo);
            }
            if (dt.Rows.Count > 1)
            {
                var filtered_dt = dt.AsEnumerable().Where(dr => dr.Field<int>("Claims_Additional_Provider_Information_Service_ID") != Convert.ToInt32(Claims_Additional_Provider_Information_Service_ID));
                DataTable dtResult = filtered_dt.CopyToDataTable();

                #region Validtions

                if (!result)
                {
                    if (dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            string AdditonalProviderDetail = dr["Service_Line"].ToString();
                            string ProviderTypedropdon = dr["Provider_Type"].ToString();
                            AdditionalProviderInfo = new AdditionalProviderInformation();

                            if (AdditonalProviderDetail == ServiceLine &&
                            ProviderTypedropdon == ProviderType)
                            {
                                count = true;
                                break;
                            }
                            else
                            {
                                count = false;
                            }

                        }
                        if (count)
                        {
                            AdditionalProviderInfo.Error_Msg = "Same provider type and Service Line number cannot be added again. ";
                            lstAddProviderInfoData.Add(AdditionalProviderInfo);
                        }
                        if (!count && !result)
                        {
                            foreach (DataRow dr in dtResult.Rows)
                            {
                                string AdditonalProviderDetail = dr["Service_Line"].ToString();
                                string ProviderTypedropdon = dr["Provider_Type"].ToString();
                                string ProviderNPIGrid = dr["Provider_NPI"].ToString();
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
                                (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Supervising provider and Rendering provider cannot be same";
                                    data = true;
                                }
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                    ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider) ||
                                    (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.SupervisingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Assistant surgeon and Supervising provider Cannot be same.";
                                    data = true;
                                }
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                     ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider) ||
                                       (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.AssistantSurgeon)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Assistant surgeon and Rendering provider Cannot be same.";
                                    data = true;
                                }
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                     ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician) ||
                                     (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.OtherOperatingPhysician && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.OperatingPhysician)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Operating physician and Other operating physician NPI Cannot be same.";
                                    data = true;
                                }
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                 ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider) ||
                                  (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Rendering and Referring Provider NPI Cannot be same.";
                                    data = true;
                                }
                                if (AdditonalProviderDetail == ServiceLine && ProviderNPI == ProviderNPIGrid &&
                                   ((ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider) ||
                                   (ProviderTypedropdon == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider && ProviderType == CON.ClaimsAdditionalPanelProviderTypes.RenderingProvider)))
                                {
                                    AdditionalProviderInfo.Error_Msg = "Rendering Provider Cannot be same as Primary Care Provider";
                                    data = true;
                                }

                                // Compare primary and referring - // Panel level check
                                if (ProviderType == CON.ClaimsAdditionalPanelProviderTypes.PrimaryCareProvider)
                                {
                                    DataTable dtPriRef = Claims.FetchAdditionalProviderInformationWithServiceLine(ServiceLine, Claim_ID).Tables[0];
                                    DataRow[] refrows = dtPriRef.Select("Provider_Type ='" + CON.ClaimsAdditionalPanelProviderTypes.ReferringProvider + "'");
                                    if (refrows.Length == 0)
                                    {
                                        AdditionalProviderInfo.Error_Msg = "Primary care provider(referral) cannot be entered without entering referring provider";
                                        data = true;
                                    }
                                }
                            }
                        }

                        if (data)
                        {
                            lstAddProviderInfoData.Add(AdditionalProviderInfo);
                        }

                    }
                }
                #endregion
            }
            if (!result && !count && !data && ProviderNPI.Trim().Length == 10)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Claim_ID))
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("Claims_Additional_Provider_Information_Service_ID", Claims_Additional_Provider_Information_Service_ID);
                        parms.Add("Claim_id", Claim_ID);
                        parms.Add("Service_Line", ServiceLine);
                        parms.Add("Provider_Type", ProviderType);
                        parms.Add("Provider_NPI", ProviderNPI);
                        if (ClaimType == CON.ClaimsType.Professional)
                            parms.Add("Medicaid_ID", MedicaidId);
                        parms.Add("Last_Name", LastName);
                        parms.Add("First_Name", FirstName);
                        parms.Add("Middle_Name", MiddleName);
                        parms.Add("Last_Modified_User", HelperFacade.GetUserId(userid).ToString());
                        MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData("Claims_Additional_Provider_Information_Service", parms);

                    }
                }
                catch (Exception ex) { }
                lstAddProviderInfoData = Claims.SelectAdditonalProviderInfoDetails(Claim_ID, ClaimType);
            }

            return Ok(lstAddProviderInfoData);
        }

        [HttpPut]
        [Route("EditAdditionalProviderInfo")]
        public IActionResult EditAdditionalProviderInfo(string Claims_Additional_Provider_Information_Service_ID = "", string Claim_ID = "", string ClaimType = "")
        {
            List<AdditionalProviderInformation> lstAdditionalProviderInformation = new List<AdditionalProviderInformation>();


            DataSet bindFields = MAXIMUS.Controllers.PDMS.ClaimsController.EditAdditionalProviderInformation(Claim_ID, Claims_Additional_Provider_Information_Service_ID);
            var data = string.Empty;

            DataTable dt = new DataTable();
            dt = bindFields.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AdditionalProviderInformation AddAdditionProviderInfo = new AdditionalProviderInformation();
                    AddAdditionProviderInfo.ServiceLine = dr["Service_Line"].ToString();
                    AddAdditionProviderInfo.ProviderType = dr["Provider_Type"].ToString();
                    AddAdditionProviderInfo.ProviderNPI = dr["Provider_NPI"].ToString();
                    AddAdditionProviderInfo.LastName = dr["Last_Name"].ToString();
                    AddAdditionProviderInfo.FirstName = dr["First_Name"].ToString();
                    AddAdditionProviderInfo.MiddleName = dr["Middle_Name"].ToString();
                    AddAdditionProviderInfo.ClaimID = dr["Claim_ID"].ToString();
                    if (ClaimType == CON.ClaimsType.Professional)
                        AddAdditionProviderInfo.MedicaidID = dr["Medicaid_ID"].ToString();
                    AddAdditionProviderInfo.Claims_Additional_Provider_Information_Service_ID = dr["Claims_Additional_Provider_Information_Service_ID"].ToString();
                    lstAdditionalProviderInformation.Add(AddAdditionProviderInfo);

                    //ddlAdditonalProviderDetail_SelectedIndexChanged(Claim_ID, ClaimType, AddAdditionProviderInfo.ServiceLine);
                }
            }

            return Ok(lstAdditionalProviderInformation);

        }


        [HttpGet]
        [Route("GetFilteredClaimIndicator")]
        public IActionResult GetFilteredClaimIndicator(string Claim_ID = "")
        {
            var data = string.Empty;
            //
            //DataSet dataSetProvType = MAXIMUS.Controllers.PDMS.ClaimsController.GetClaimFilingIndicator();
            //DataTable dtProvType = dataSetProvType.Tables[0];
            List<PDMSRestServices.Models.ClaimFilingIndicator> list = new List<PDMSRestServices.Models.ClaimFilingIndicator>();

            DataSet dsClaimFilingIndicator = new DataSet();
            if (!string.IsNullOrWhiteSpace(Claim_ID))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, true));
                dsClaimFilingIndicator = DataAccess.ExecuteStoredProcedure("usp_SelectFilteredClaimFilingIndicator", parameters, "PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR");

            }

            DataTable dtClaimFilingIndicator = dsClaimFilingIndicator.Tables[0];

            if (dtClaimFilingIndicator.Rows.Count > 0)
            {

                PDMSRestServices.Models.ClaimFilingIndicator details;
                // Removing already added provider for detail/ServiceLine number.        

                if (dtClaimFilingIndicator.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimFilingIndicator.Rows)
                    {
                        details = new PDMSRestServices.Models.ClaimFilingIndicator(dr["PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE"].ToString(), dr["PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC"].ToString());

                        list.Add(details);
                    }
                }
            }

            return Ok(list);

        }

        [HttpGet]
        [Route("ddlAdditonalProviderDetail_SelectedIndexChanged")]
        public IActionResult ddlAdditonalProviderDetail_SelectedIndexChanged(string Claim_ID = "", string ClaimType = "", string ddlServiceLine = "")
        {
            var data = string.Empty;

            DataSet dataSetProvType = DataAccess.ExecuteStoredProcedure("usp_Get_PRIOR_AUTH_CLAIM_PROVIDERTYPE");
            DataTable dtProvType = dataSetProvType.Tables[0];
            if (ClaimType == CON.ClaimsType.Dental)
            {
                var row_ClaimTypes = from row in dtProvType.AsEnumerable()
                                     where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 2 ||
                                     row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4
                                     select row;
                DataTable dt_ProviderType_Dental = row_ClaimTypes.CopyToDataTable();
                //  Helper.LoadList(ddlProviderType, dt_ProviderType_Dental, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Dental;
            }
            if (ClaimType == CON.ClaimsType.Institutional)
            {
                var row_Institutional = from row in dtProvType.AsEnumerable()
                                        where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 8 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 9 ||
                                        row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5
                                        select row;
                DataTable dt_ProviderType_Institutional = row_Institutional.CopyToDataTable();
                //Helper.LoadList(ddlProviderType, dt_ProviderType_Institutional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Institutional;
            }
            if (ClaimType == CON.ClaimsType.Professional)
            {
                var row_Professional = from row in dtProvType.AsEnumerable()
                                       where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 6 ||
                                       row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 ||
                                       row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 7
                                       select row;
                DataTable dt_ProviderType_Professional = row_Professional.CopyToDataTable();
                //  Helper.LoadList(ddlProviderType, dt_ProviderType_Professional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Professional;
            }

            List<ProviderTypes> list = new List<ProviderTypes>();
            if (dtProvType.Rows.Count > 0)
            {

                ProviderTypes details;
                // Removing already added provider for detail/ServiceLine number.        
                DataTable dtAdditional = Claims.FetchAdditionalProviderInformationWithServiceLine(ddlServiceLine, Claim_ID).Tables[0];
                DataTable dtprovider_type = dtProvType.AsEnumerable()
                                            .Where(ra => !dtAdditional.AsEnumerable()
                                            .Any(rb => rb.Field<string>("Provider_Type") == ra.Field<string>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC")))
                                            .CopyToDataTable();
                if (dtprovider_type.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtprovider_type.Rows)
                    {
                        details = new ProviderTypes(dr["PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID"].ToString(), dr["PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC"].ToString());
                        list.Add(details);
                    }
                }
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("ddlAdditonalProvidertype_onEditButtonClick")]
        public IActionResult ddlAdditonalProvidertype_onEditButtonClick(string Claim_ID = "", string ClaimType = "",
            string ddlServiceLine = "", string ClickedProviderType = "")
        {
            var data = string.Empty;

            DataSet dataSetProvType = MAXIMUS.Controllers.PDMS.LookupTableController.GetSubmiclaimtProviderType();
            DataTable dtProvType = dataSetProvType.Tables[0];
            if (ClaimType == CON.ClaimsType.Dental)
            {
                var row_ClaimTypes = from row in dtProvType.AsEnumerable()
                                     where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 2 ||
                                     row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4
                                     select row;
                DataTable dt_ProviderType_Dental = row_ClaimTypes.CopyToDataTable();
                //  Helper.LoadList(ddlProviderType, dt_ProviderType_Dental, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Dental;
            }
            if (ClaimType == CON.ClaimsType.Institutional)
            {
                var row_Institutional = from row in dtProvType.AsEnumerable()
                                        where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 8 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 9 ||
                                        row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5
                                        select row;
                DataTable dt_ProviderType_Institutional = row_Institutional.CopyToDataTable();
                //Helper.LoadList(ddlProviderType, dt_ProviderType_Institutional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Institutional;
            }
            if (ClaimType == CON.ClaimsType.Professional)
            {
                var row_Professional = from row in dtProvType.AsEnumerable()
                                       where row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 6 ||
                                       row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 3 ||
                                       row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 4 || row.Field<int>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID") == 7
                                       select row;
                DataTable dt_ProviderType_Professional = row_Professional.CopyToDataTable();
                //  Helper.LoadList(ddlProviderType, dt_ProviderType_Professional, "PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC", "PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID", true);
                dtProvType = dt_ProviderType_Professional;
            }

            List<ProviderTypes> list = new List<ProviderTypes>();
            if (dtProvType.Rows.Count > 0)
            {

                ProviderTypes details;
                // Removing already added provider for detail/ServiceLine number.

                DataTable dtAdditional = Claims.FetchAdditionalProviderInformationWithServiceLine(ddlServiceLine, Claim_ID).Tables[0];
                DataTable dtprovider_type = dtProvType;
                if (dtAdditional.Rows.Count > 1)
                {
                    dtAdditional = dtAdditional.AsEnumerable().Where(w => w.Field<string>("Provider_Type") != ClickedProviderType.ToString()).CopyToDataTable();

                    dtprovider_type = dtprovider_type.AsEnumerable()
                                          .Where(ra => !dtAdditional.AsEnumerable()
                                          .Any(rb => rb.Field<string>("Provider_Type") == ra.Field<string>("PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC")))
                                          .CopyToDataTable();
                }
                //dtprovider_type.Rows.Add(ClickedProviderType);
                if (dtprovider_type.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtprovider_type.Rows)
                    {
                        details = new ProviderTypes(dr["PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID"].ToString(), dr["PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC"].ToString());
                        list.Add(details);
                    }
                }
            }
            return Ok(list);

        }


        [HttpGet]
        [Route("txtAdditionalProviderNPI_TextChangedNPI")]
        public IActionResult txtAdditionalProviderNPI_TextChangedNPI(string Claim_ID = "", string ClaimType = "", string ProviderNPI = "")
        {
            NPIData data = new NPIData();
            List<NPIData> lstNPIdata = new List<NPIData>();
            if (ProviderNPI.Trim().Length != 10)
            {
                data.Errormessage = "10 digit NPI is required";
                lstNPIdata.Add(data);
            }
            if (ClaimType == CON.ClaimsType.Professional)
            {
                DataTable dt = Claims.GetNPIForAdditional(ProviderNPI);

                if (Helper.HasRows(dt))
                {
                    if (dt.Rows.Count >= 2)
                    {
                        // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);
                    }
                    else
                    {
                        DataRow row = dt.Rows[0];
                        data.FirstName = row["FIRST_NAME"].ToString();
                        data.LastName = row["LAST_OR_BUSINESS_NAME"].ToString();
                        data.MedicaidId = row["MEDICAID_ID"].ToString();
                        lstNPIdata.Add(data);
                    }
                }
                else
                {
                    data.Errormessage = "NPI is Unknown";
                    lstNPIdata.Add(data);
                    //ResetAdditionalProviderPanel();
                }
            }
            else
            {
                DataTable dt = Claims.GetNPIForAdditional(ProviderNPI);
                if (Helper.HasRows(dt) && !string.IsNullOrEmpty(ProviderNPI))
                {
                    if (dt.Rows.Count >= 2)
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { $('#myModalpop').modal('show')});</script>", false);
                    }
                    else
                    {
                        DataRow row = dt.Rows[0];
                        data.FirstName = row["FIRST_NAME"].ToString();
                        data.LastName = row["LAST_OR_BUSINESS_NAME"].ToString();
                        data.MedicaidId = row["MEDICAID_ID"].ToString();
                        lstNPIdata.Add(data);
                    }
                }
                else
                {
                    data.Errormessage = "NPI is Unknown";
                    lstNPIdata.Add(data);
                    //  ResetAdditionalProviderPanel();
                }
            }
            return Ok(lstNPIdata);

        }

        [HttpGet]
        [Route("GetNDCode")]
        public IActionResult GetNDCode(string NDCCode = "", string TradeName = "")
        {
            List<NDCCode> NDCCodeList = new List<NDCCode>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("NDCCode", DbType.String, NDCCode, true));
            parameters.Add(SqlParms.CreateParameter("TradeName", DbType.String, TradeName, true));

            DataSet dataSetNDC = DataAccess.ExecuteStoredProcedure("Usp_Select_CLAIMS_NDC_CODE", parameters, "CLAIMS_NDC_CODE");
            DataTable dt = new DataTable();
            dt = dataSetNDC.Tables[0];
            if (Helper.HasRows(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    NDCCode NDCCodeData = new NDCCode();
                    NDCCodeData.CLAIMS_NDC_CODE = dr["CLAIMS_NDC_CODE"].ToString();
                    NDCCodeData.LAY_DESC = dr["LAY_DESC"].ToString();
                    NDCCodeList.Add(NDCCodeData);
                }
            }
            return Ok(NDCCodeList);

        }

        [HttpGet]
        [Route("ddlHeaderOtherPayerHealthPlanID_SelectedIndexChanged")]
        public IActionResult ddlHeaderOtherPayerHealthPlanID_SelectedIndexChanged(string Claim_ID = "", string ddlHeaderOtherHealthPlanID = "")
        {
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> AdjlistData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();

            DataTable dtCO = new DataTable();
            DataTable dtCR = new DataTable();
            DataTable dtOA = new DataTable();
            DataTable dtPI = new DataTable();
            DataTable dtPR = new DataTable();

            DataSet dsAdjGroup = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();
            DataSet ds = Claims.FetchOtherPayerAdjustmentInformation(Claim_ID);

            List<string> AdjustmentGroupList = new List<string>();
            foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
            {
                AdjustmentGroupList.Add(dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString());
            }

            //Remove adjustment group from the list
            if (!string.IsNullOrEmpty(ddlHeaderOtherHealthPlanID) && Helper.HasRows(ds))
            {
                var dsCO = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlHeaderOtherHealthPlanID.ToString()
                                                  && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID) && w.Field<string>("cde_adjustment_group") == CON.AdjustmentGroup.CO).ToList();
                if (dsCO.Any())
                {
                    dtCO = dsCO.CopyToDataTable();
                }
                var dsCR = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlHeaderOtherHealthPlanID.ToString()
                                                 && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID) && w.Field<string>("cde_adjustment_group") == CON.AdjustmentGroup.CR).ToList();
                if (dsCR.Any())
                {
                    dtCR = dsCR.CopyToDataTable();
                }
                var dsOA = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlHeaderOtherHealthPlanID.ToString()
                                                 && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID) && w.Field<string>("cde_adjustment_group") == CON.AdjustmentGroup.OA).ToList();
                if (dsOA.Any())
                {
                    dtOA = dsOA.CopyToDataTable();
                }
                var dsPI = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlHeaderOtherHealthPlanID.ToString()
                                                 && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID) && w.Field<string>("cde_adjustment_group") == CON.AdjustmentGroup.PI).ToList();
                if (dsPI.Any())
                {
                    dtPI = dsPI.CopyToDataTable();
                }
                var dsPR = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlHeaderOtherHealthPlanID.ToString()
                                                 && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID) && w.Field<string>("cde_adjustment_group") == CON.AdjustmentGroup.PR).ToList();

                if (dsPR.Any())
                {
                    dtPR = dsPR.CopyToDataTable();
                }

                if (dtCO.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.CO);
                }
                if (dtCR.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.CR);
                }
                if (dtOA.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.OA);
                }
                if (dtPI.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.PI);
                }
                if (dtPR.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.PR);
                }
                foreach (var item in AdjustmentGroupList)
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_adjustment_group = item.ToString();
                    AdjlistData.Add(otherPayerAdjustmentInfo);
                }
            }
            else
            {
                foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
                {
                    PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                    otherPayerAdjustmentInfo.cde_adjustment_group = dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString();
                    AdjlistData.Add(otherPayerAdjustmentInfo);
                }
            }
            return Ok(AdjlistData);
        }

        [HttpGet]
        [Route("BindAdjustmentGroupDropdown")]
        public IActionResult BindAdjustmentGroupDropdown()
        {
            var data = string.Empty;

            DataSet dsAdjGroup = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();
            if (dsAdjGroup.Tables[0].Rows.Count > 0)
            {
                data = JsonConvert.SerializeObject(dsAdjGroup.Tables[0]);
            }
            return Ok(data);
        }

        [HttpGet]
        [Route("ReloadAdjustmentGrouponEditClick")]
        public IActionResult ReloadAdjustmentGrouponEditClick(string Claim_ID = "", string HealthPlanID = "", string ClickedAdjustmentGroup = "")
        {
            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> AdjGroupsListData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();


            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", Claim_ID);
            parms.Add("Health_Plan_ID", HealthPlanID);
            DataSet ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("SelectAdjustmentGroupsForHeaderOtherPayerOnEdit", parms);
            List<string> FilteredAdjGroupList = new List<string>();
            DataTable dtAdj = new DataTable();
            if (ds.Tables[0].Rows.Count > 1)
            {
                dtAdj = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Adjustment_Group") != ClickedAdjustmentGroup.ToString()).CopyToDataTable();

                foreach (DataRow dr in dtAdj.Rows)
                {
                    FilteredAdjGroupList.Add(dr["Adjustment_Group"].ToString()); //List1 result
                }
            }
            List<string> AdjustmentGroupList = new List<string>();
            DataSet dsAdjGroup = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();
            foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
            {
                AdjustmentGroupList.Add(dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString()); //List2 result
            }

            List<string> result = AdjustmentGroupList.Except(FilteredAdjGroupList).ToList();

            foreach (var item in result)
            {
                PDMSRestServices.Models.OtherPayerAdjustmentInfo otherPayerAdjustmentInfo = new PDMSRestServices.Models.OtherPayerAdjustmentInfo();
                otherPayerAdjustmentInfo.cde_adjustment_group = item.ToString();
                AdjGroupsListData.Add(otherPayerAdjustmentInfo);
            }
            return Ok(AdjGroupsListData);

        }


        [HttpGet]
        [Route("ddlOtherPayerAdjustmentServiceDetailHealthPlanID_SelectedIndexChanged")]
        public IActionResult ddlOtherPayerAdjustmentServiceDetailHealthPlanID_SelectedIndexChanged(string Claim_ID = "", string ddlOPAHealthPlanID = "", string ddlOPAServiceLineNo = "")
        {
            List<OtherPayerAdjustmentInfoServiceDetail> OtherPayerAdjlistData = new List<OtherPayerAdjustmentInfoServiceDetail>();

            DataTable dtCO = new DataTable();
            DataTable dtCR = new DataTable();
            DataTable dtOA = new DataTable();
            DataTable dtPI = new DataTable();
            DataTable dtPR = new DataTable();

            DataSet dsAdjGroup = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();

            List<string> AdjustmentGroupList = new List<string>();
            foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
            {
                AdjustmentGroupList.Add(dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString());
            }

            DataSet ds = Claims.GetOtherPayerAdjustmentInfo(Claim_ID);
            //Remove adjustment group from the list
            if (!string.IsNullOrEmpty(ddlOPAHealthPlanID) && Helper.HasRows(ds))
            {
                var dsCO = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlOPAHealthPlanID.ToString()
                                                    && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID)
                                                    && w.Field<int>("Service_Line") == Convert.ToInt32(ddlOPAServiceLineNo.ToString())
                                                    && w.Field<string>("Adjustment_Group") == CON.AdjustmentGroup.CO).ToList();
                if (dsCO.Any())
                {
                    dtCO = dsCO.CopyToDataTable();
                }
                var dsCR = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlOPAHealthPlanID.ToString()
                                                     && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID)
                                                     && w.Field<int>("Service_Line") == Convert.ToInt32(ddlOPAServiceLineNo.ToString())
                                                     && w.Field<string>("Adjustment_Group") == CON.AdjustmentGroup.CR).ToList();
                if (dsCR.Any())
                {
                    dtCR = dsCR.CopyToDataTable();
                }
                var dsOA = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlOPAHealthPlanID.ToString()
                                                    && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID)
                                                    && w.Field<int>("Service_Line") == Convert.ToInt32(ddlOPAServiceLineNo.ToString())
                                                    && w.Field<string>("Adjustment_Group") == CON.AdjustmentGroup.OA).ToList();
                if (dsOA.Any())
                {
                    dtOA = dsOA.CopyToDataTable();
                }
                var dsPI = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlOPAHealthPlanID.ToString()
                                                    && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID)
                                                    && w.Field<int>("Service_Line") == Convert.ToInt32(ddlOPAServiceLineNo.ToString())
                                                    && w.Field<string>("Adjustment_Group") == CON.AdjustmentGroup.PI).ToList();
                if (dsPI.Any())
                {
                    dtPI = dsPI.CopyToDataTable();
                }
                var dsPR = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Health_Plan_ID") == ddlOPAHealthPlanID.ToString()
                                                    && w.Field<int>("Claim_ID") == Convert.ToInt32(Claim_ID)
                                                    && w.Field<int>("Service_Line") == Convert.ToInt32(ddlOPAServiceLineNo.ToString())
                                                    && w.Field<string>("Adjustment_Group") == CON.AdjustmentGroup.PR).ToList();

                if (dsPR.Any())
                {
                    dtPR = dsPR.CopyToDataTable();
                }

                if (dtCO.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.CO);
                }
                if (dtCR.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.CR);
                }
                if (dtOA.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.OA);
                }
                if (dtPI.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.PI);
                }
                if (dtPR.Rows.Count == 6)
                {
                    AdjustmentGroupList.Remove(CON.AdjustmentGroup.PR);
                }
                foreach (var item in AdjustmentGroupList)
                {
                    OtherPayerAdjustmentInfoServiceDetail otherPayerAdjustmentInfo = new OtherPayerAdjustmentInfoServiceDetail();
                    otherPayerAdjustmentInfo.Adjustment_Group = item.ToString();
                    OtherPayerAdjlistData.Add(otherPayerAdjustmentInfo);
                }
            }
            else
            {
                foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
                {
                    OtherPayerAdjustmentInfoServiceDetail otherPayerAdjustmentInfo = new OtherPayerAdjustmentInfoServiceDetail();
                    otherPayerAdjustmentInfo.Adjustment_Group = dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString();
                    OtherPayerAdjlistData.Add(otherPayerAdjustmentInfo);
                }
            }
            return Ok(OtherPayerAdjlistData);

        }

        [HttpGet]
        [Route("ReloadAdjustmentGrouponOtherPayerAdjustmentEditClick")]
        public IActionResult ReloadAdjustmentGrouponOtherPayerAdjustmentEditClick(string Claim_ID = "", string ServiceLineNo = "", string HealthPlanID = "", string ClickedAdjustmentGroup = "")
        {
            List<OtherPayerAdjustmentInfoServiceDetail> AdjGroupsListData = new List<OtherPayerAdjustmentInfoServiceDetail>();


            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", Claim_ID);
            parms.Add("Service_Line", ServiceLineNo);
            parms.Add("Health_Plan_ID", HealthPlanID);
            DataSet ds = MAXIMUS.Controllers.PDMS.ClaimsController.SelectPanelsData("SelectAdjustmentGroupsForOtherPayerAdjustmentOnEdit", parms);
            List<string> FilteredAdjGroupList = new List<string>();
            DataTable dtAdj = new DataTable();
            if (ds.Tables[0].Rows.Count > 1)
            {
                dtAdj = ds.Tables[0].AsEnumerable().Where(w => w.Field<string>("Adjustment_Group") != ClickedAdjustmentGroup.ToString()).CopyToDataTable();

                foreach (DataRow dr in dtAdj.Rows)
                {
                    FilteredAdjGroupList.Add(dr["Adjustment_Group"].ToString()); //List1 result
                }
            }
            List<string> AdjustmentGroupList = new List<string>();
            DataSet dsAdjGroup = MAXIMUS.Controllers.PDMS.LookupTableController.GetAdjustmentGroup();
            foreach (DataRow dr in dsAdjGroup.Tables[0].Rows)
            {
                AdjustmentGroupList.Add(dr["PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC"].ToString()); //List2 result
            }

            List<string> result = AdjustmentGroupList.Except(FilteredAdjGroupList).ToList();

            foreach (var item in result)
            {
                OtherPayerAdjustmentInfoServiceDetail otherPayerAdjustmentInfo = new OtherPayerAdjustmentInfoServiceDetail();
                otherPayerAdjustmentInfo.Adjustment_Group = item.ToString();
                AdjGroupsListData.Add(otherPayerAdjustmentInfo);
            }
            return Ok(AdjGroupsListData);

        }

        [HttpGet]
        [Route("CheckDiagnosisCodeUse")]
        public IActionResult CheckDiagnosisCodeUse(string claimid = "", String diagnosisCode = "")
        {
            string ErrorMessage = null;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.String, claimid, true));
            parameters.Add(SqlParms.CreateParameter("Diagnosis_Code", DbType.String, diagnosisCode, true));

            DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectServiceDetail_Diagnosis", parameters, "Claims_Service_Details");


            if (Helper.HasRows(lookup))
            {
                ErrorMessage = "Diagnosis code cannot be deleted since diagnosis pointer is used  in service detail for this diagnosis code.";

            }
            else
            {
                ErrorMessage = "";

            }

            return Ok(JsonConvert.SerializeObject(ErrorMessage));

        }

        [HttpGet]
        [Route("ValidProviderNPI")]
        public IActionResult ValidProviderNPI(string npi = "")
        {
            var validNpi = Claims.ValidProviderNPI(npi);
            return Ok(validNpi);
        }


    }
}
