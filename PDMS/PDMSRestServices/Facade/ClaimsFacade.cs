using MAXIMUS.Core.Libraries;
using System.Data;
using Corp.Core.Libraries;
using Newtonsoft.Json;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using PDMSRestServices.Models;
using PDMSRestServices.Facade;
using System.Runtime.InteropServices;
using Corp.Core.Libraries.FI.ClaimsSearchReference;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Web.Services3.Security.Tokens;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace PDMSRestServices.Controllers.Facade
{
    public class Claims
    {
        public static string NPITextChangedTest(string _txtNPI, string RefMedId = "")
        {
            var data = string.Empty;

            if (!string.IsNullOrEmpty(_txtNPI))
            {
                if (_txtNPI.Trim().Length == 10)
                {
                    var isValid = ValidProviderNPI(_txtNPI.Trim());
                    if (!isValid)
                    {
                        data = JsonConvert.SerializeObject("NPI is not found in the system");

                    }
                    else
                    {
                        DataTable dt = GetData(_txtNPI.Trim(), RefMedId, "", "");
                        if (dt.Rows.Count > 0)
                        {

                            dt = dt.Select("NPI <> ''").CopyToDataTable();
                            data = JsonConvert.SerializeObject(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                RefMedId = row["MEDICAID_ID"].ToString();

                            }

                        }
                    }
                }
                else
                {

                    data = JsonConvert.SerializeObject("10-digit number is required");
                }
            }
            else
            {

            }
            return data;
        }
        public static bool ValidProviderNPI(string npi)
        {
            try
            {
                return MAXIMUS.Controllers.PDMS.ProviderController.VerifyProviderNPI(Convert.ToInt64(npi));
            }
            catch
            {
                return false;
            }
        }
        public static DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
        {
            DataTable dt = null;
            try
            {

                var ds = MAXIMUS.Controllers.PDMS.ProviderController.SearchClaimProviderNPI(npi, medicaidid, lastName, firstName);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }

            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }
        public static void SaveClaimsNPIDetails(string _txtNPI, string _txtMedicaidId, string _txtFirstName, string _txtLastName, string tableName, int is_Primary, string claimsPanelName, int ClaimID, string user)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsIndividualClaims = MAXIMUS.Controllers.PDMS.ClaimsController.SelectDentalClaimData(ClaimID, "claims_provider_information", claimsPanelName, is_Primary);
            if (!string.IsNullOrEmpty(ClaimID.ToString()))
            {
                if (Methods.HasRows(dsIndividualClaims) &&
                Convert.ToInt32(dsIndividualClaims.Tables[0].Rows[0]["Claim_ID"]) == ClaimID &&
                dsIndividualClaims.Tables[0].Rows[0]["Claims_Panel_Name"].ToString() == claimsPanelName)
                {
                    parms.Add("NPI", _txtNPI.ToString());
                    parms.Add("Medicaid_ID", _txtMedicaidId.ToString());
                    parms.Add("First_Name", _txtFirstName.ToString());
                    parms.Add("Last_Name", _txtLastName.ToString());
                    parms.Add("Claim_ID", ClaimID.ToString());
                    parms.Add("Is_Primary", is_Primary.ToString());
                    parms.Add("Last_Modified_User", Methods.GetUserId(user).ToString());
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                    MAXIMUS.Controllers.PDMS.ClaimsController.UpdatePanelsData(tableName, parms);
                }
                else
                {
                    parms.Add("NPI", _txtNPI.ToString());
                    parms.Add("Medicaid_ID", _txtMedicaidId.ToString());
                    parms.Add("First_Name", _txtFirstName.ToString());
                    parms.Add("Last_Name", _txtLastName.ToString());
                    parms.Add("Claim_ID", ClaimID.ToString());
                    parms.Add("Is_Primary", is_Primary.ToString());
                    parms.Add("Created_By_User", Methods.GetUserId(user).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                    MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData(tableName, parms);
                }
            }
        }
        public static DataSet LoadProviderInformation(string medicaidNumber)
        {
            DataSet ds = MAXIMUS.Controllers.PDMS.RegistrationController.SelectProviderByGRPMedicaidID(medicaidNumber);
            return ds;
        }
        public static DataSet FetchOtherPayerAdjustmentInformation(string hdnClaimID)
        {

            DataSet dsOtherPayerInfo = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimID);
            dsOtherPayerInfo = SelectPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parms);
            return dsOtherPayerInfo;
        }
        public static DataSet FetchOtherPayerAdjustmentInformationforEditting(string hdnClaimID, string Claims_Header_Other_Payer_Adjustment_Information_ID)
        {

            DataSet dsOtherPayerInfo = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimID);
            parms.Add("Claims_Header_Other_Payer_Adjustment_Information_ID", Claims_Header_Other_Payer_Adjustment_Information_ID);
            dsOtherPayerInfo = SelectPanelsData("edit_claims_header_other_payer_adjustment_information", parms);
            return dsOtherPayerInfo;
        }
        public static List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> DisplayInstitutionalHeaderAdjustmentPanel(string hdnClaimId)
        {

            List<PDMSRestServices.Models.OtherPayerAdjustmentInfo> listData = new List<PDMSRestServices.Models.OtherPayerAdjustmentInfo>();

            var data = string.Empty;
            if (hdnClaimId != "undefined")
            {
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
            }
            return listData;
        }
        public static DataSet GetDiagnosisData(string hdnClaimIdDiag, string hdnClaimType_Diag)
        {
            DataSet dsDiagnosis = new DataSet();
            //if ((!string.IsNullOrEmpty(ClaimID) && !string.IsNullOrEmpty(ClaimType)) || !string.IsNullOrEmpty(ICNNumber))
            if ((!string.IsNullOrEmpty(hdnClaimIdDiag) && !string.IsNullOrEmpty(hdnClaimType_Diag)))
            {
                dsDiagnosis = LoadDiagnosisData(hdnClaimIdDiag, hdnClaimType_Diag);
                if (Helper.HasRows(dsDiagnosis) &&
                    Convert.ToInt32(dsDiagnosis.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdDiag)
                    && (hdnClaimType_Diag != CON.ClaimType.INSTITUTIONAL.ToString()))
                {
                    // hdnDiagnosispanel.Value = dsDiagnosis.Tables[0].Rows[0]["Claims_Diagnosis_Information_ID"].ToString();
                    dsDiagnosis.Tables[0].DefaultView.Sort = "diag_seq ASC";


                    //DiagnosisGridviewCountValidation();
                }

                //if (hdnClaimType_Diagnosis.Value == CON.ClaimsType.Institutional)
                //{
                //    divSequencedescription.Visible = divPresetOnAdmission.Visible = true;
                //    btnICDVersionSearch.Visible = false;
                //    foreach (DataControlField column in gvDiagnosis.Columns)
                //    {
                //        if (column.HeaderText == "Sequence" || column.HeaderText == "Present On Admission")
                //            column.Visible = true;
                //        if (column.HeaderText == "Sequence ")
                //            column.Visible = false;
                //    }
                //}
                //else
                //{
                //    divSequencedescription.Visible = divPresetOnAdmission.Visible = false;
                //    btnICDVersionSearch.Visible = true;
                //    foreach (DataControlField column in gvDiagnosis.Columns)
                //    {
                //        if (column.HeaderText == "Sequence" || column.HeaderText == "Present On Admission")
                //            column.Visible = false;
                //        if (column.HeaderText == "Sequence ")
                //            column.Visible = true;
                //    }
                //}
            }
            return dsDiagnosis;
            // DiagnosisGridviewCountValidation();
        }
        private static DataSet LoadDiagnosisData(string hdnClaimIdDiag, string hdnClaimType_Diag)
        {

            DataSet dsDiagnosisCodes = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimIdDiag);
            parms.Add("Claim_Type", hdnClaimType_Diag);
            try
            {
                dsDiagnosisCodes = SelectPanelsData("Claims_Diagnosis_Information", parms);
            }
            catch (Exception ex) { }
            return dsDiagnosisCodes;
        }
        public static void InsertProviderNote(Dictionary<string, string> parms, string claimId, string notes, string User, int isBillingNote = 0)
        {
            parms.Add("Note", notes);
            parms.Add("Claim_ID", claimId);
            parms.Add("Last_Modified_User", HelperFacade.GetUserId(User).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            parms.Add("Created_By_User", HelperFacade.GetUserId(User).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("IS_BILLING_NOTE", isBillingNote.ToString());
            try
            {
                MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData("Claims_Providers_Note", parms);
            }
            catch
            {
            }
            parms.Clear();
        }
        public static DataSet GetProviderNotes(string hdnClaimIdDiag, string hdnClaimType_ProviderNote, int isBillingNote = 0)
        {
            DataSet dsProviderNotes = new DataSet();
            //if ((!string.IsNullOrEmpty(ClaimID) && !string.IsNullOrEmpty(ClaimType)) || !string.IsNullOrEmpty(ICNNumber))
            if ((!string.IsNullOrEmpty(hdnClaimIdDiag) && !string.IsNullOrEmpty(hdnClaimType_ProviderNote)))
            {

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", hdnClaimIdDiag);
                parms.Add("Claim_Type", hdnClaimType_ProviderNote);
                parms.Add("IS_BILLING_NOTE", isBillingNote.ToString());
                dsProviderNotes = SelectPanelsData("Claims_Providers_Note", parms);
            }
            return dsProviderNotes;
        }
        public static DataSet GetAmbulanceDropOff(string claimid)
        {

            DataSet dsAmbulanceInfoInfo = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(claimid))
            {
                parms.Add("Claim_ID", claimid);
                dsAmbulanceInfoInfo = SelectPanelsData("claims_ambulance_pick_up_drop_off_location", parms);
            }
            return dsAmbulanceInfoInfo;
        }
        public static DataSet GetDataInstiServiceDetail(int ClaimId)
        {
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
            DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
            return serviceLineDetails;

        }
        public static DataSet GetOtherPayerData(string claimid)
        {

            DataSet dsOtherPayerInfo = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", claimid);
            dsOtherPayerInfo = SelectPanelsData("claims_other_payer_information", parms);
            return dsOtherPayerInfo;

        }
        public static void SaveClaimsDentalServiceDetails(string claimid, string _slLine, string _producerDentalCode, string _placeofserviceDental, string _modifierDentalFirst,
          string _modifierDentalSecond, string _modifierDentalThird, string _modifierDentalFourth, string _chargesDental, string _dentalDateOfService,
          string _lineControllerNoDental, string _diagnosisDentalFirst, string _dignosisDentalSecond, string _dignosisDentalThird,
          string _dignosisDentalFourth, string _paidAmountDental, string _priorAuthNumberDental, string _oralCavityDentalFirst,
          string _oralCavityDentalSecond, string _OralCavityDentalThird, string _oralCavityDentalFourth, string _oralcavityDentalFifth,
          string _totalChages, string _billUnitDental, string _referralDental, string _inlyneCodeDental, string _paidUnitDental,
          string _prosthesisCrownInlayCode, string _statusServiceDetails, string tableName, string userName)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", claimid);

            // _slLine = (Convert.ToInt32("00")).ToString();

            parms.Add("Service_Line", _slLine);
            parms.Add("Procedure_Code", string.IsNullOrEmpty(_producerDentalCode) ? _producerDentalCode : _producerDentalCode.ToUpper());
            parms.Add("Date_of_Service", _dentalDateOfService.ToString());
            parms.Add("Line_Control_Number", string.IsNullOrEmpty(_lineControllerNoDental) ? null : _lineControllerNoDental);
            parms.Add("Prior_Authorization_Number", string.IsNullOrEmpty(_priorAuthNumberDental) ? null : _priorAuthNumberDental);
            parms.Add("Referral_Number", string.IsNullOrEmpty(_referralDental) ? null : _referralDental);
            parms.Add("Place_of_Service", string.IsNullOrEmpty(_placeofserviceDental) ? null : _placeofserviceDental);
            parms.Add("Modifier1", string.IsNullOrEmpty(_modifierDentalFirst) ? _modifierDentalFirst : _modifierDentalFirst.ToUpper() );
            parms.Add("Modifier2", string.IsNullOrEmpty(_modifierDentalSecond) ? _modifierDentalSecond : _modifierDentalSecond.ToUpper());
            parms.Add("Modifier3", string.IsNullOrEmpty(_modifierDentalThird) ? _modifierDentalThird : _modifierDentalThird.ToUpper());
            parms.Add("Modifier4", string.IsNullOrEmpty(_modifierDentalFourth) ? _modifierDentalFourth : _modifierDentalFourth.ToUpper());
            parms.Add("Diagnosis_Pointer1", _diagnosisDentalFirst);
            parms.Add("Diagnosis_Pointer2", _dignosisDentalSecond);
            parms.Add("Diagnosis_Pointer3", _dignosisDentalThird);
            parms.Add("Diagnosis_Pointer4", _dignosisDentalFourth);

            parms.Add("Oral_Cavity1", _oralCavityDentalFirst);
            parms.Add("Oral_Cavity2", _oralCavityDentalSecond);
            parms.Add("Oral_Cavity3", _OralCavityDentalThird);
            parms.Add("Oral_Cavity4", _oralCavityDentalFourth);
            parms.Add("Oral_Cavity5", _oralcavityDentalFifth);
            parms.Add("Prosthesis_Crown_InlayCode", string.IsNullOrEmpty(_prosthesisCrownInlayCode) ? null : _prosthesisCrownInlayCode);
            parms.Add("Status", _statusServiceDetails);
            parms.Add("Charges", string.IsNullOrEmpty(_chargesDental) ? null : _chargesDental);
            if (!string.IsNullOrEmpty(_paidAmountDental))
            {
                string paidamount = _paidAmountDental.Contains('$') ? _paidAmountDental.Replace('$', ' ').TrimStart() : _paidAmountDental;
                parms.Add("Paid_Amount", paidamount);
            }
            else
            {
                parms.Add("Paid_Amount", null);
            }
            parms.Add("Billed_Units", string.IsNullOrEmpty(_billUnitDental) ? null : _billUnitDental);
            parms.Add("Paid_Units", string.IsNullOrEmpty(_paidUnitDental) ? null : _paidUnitDental);
            parms.Add("Total_Charges", string.IsNullOrEmpty(_totalChages) ? null : _totalChages);
            parms.Add("Last_modified_date", DateTime.Now.ToString());
            parms.Add("Last_Modified_user", HelperFacade.GetUserId(userName).ToString());
            parms.Add("Created_date_time", DateTime.Now.ToString());
            parms.Add("Created_by_user", HelperFacade.GetUserId(userName).ToString());
            parms.Add("Claim_Type", "0");
            MAXIMUS.Controllers.PDMS.ClaimsController.InsertPanelsData(tableName, parms);
        }

        public static int GetInstiServiceDetailRows(string claimid)
        {
            List<PDMSRestServices.Models.DentalServiceDetail> InstServiceData = new List<PDMSRestServices.Models.DentalServiceDetail>();
            var data = string.Empty;
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parametersa = new List<SqlParameter>();
            parametersa.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
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

                    InstServiceData.Add(InstiServiceDetail);
                }
            }


            return InstServiceData.Count;
        }
        public static DataSet GetNDCDetails(string ClaimId)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsNDC = new DataSet();

            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsNDC = SelectPanelsData("Claims_NDC_Details", parms);
            }
            return dsNDC;
        }
        public static void updateservicelineTooThSurface(string claimid)
        {
            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();
            DataSet dsToothQuadrantInformation = new DataSet();
            if (!String.IsNullOrEmpty(claimid))
            {
                dsToothQuadrantInformation = FetchToothInformation(claimid);
                var data = string.Empty;
                DataTable dt = new DataTable();
                dt = dsToothQuadrantInformation.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.ToothQuadrantInfo ToothQuadrantInfoData = new PDMSRestServices.Models.ToothQuadrantInfo();
                        string specId = dr["Claims_Tooth_and_Surface_Information_ID"].ToString();
                        string ToothServiceLine = dr["Service_Line"].ToString();
                        int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                        string serviceLineUpdate = "";
                        if (serviceline <= 9)
                        {
                            serviceLineUpdate = "0" + serviceline.ToString();
                            List<SqlParameter> parametera = new List<SqlParameter>();
                            parametera.Add(SqlParms.CreateParameter("CLAIM_ID", DbType.Int32, claimid, true));
                            parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                            parametera.Add(SqlParms.CreateParameter("Claims_Tooth_and_Surface_Information_ID", DbType.Int32, specId, true));
                            DataAccess.ExecuteStoredProcedure("updateClaims_Tooth_and_Surface_InformationCustom", parametera, "Claims_Service_Details");

                        }


                    }
                }
            }

        }
        public static DataSet FetchToothInformation(string hdnClaimId)
        {

            DataSet dsToothQuadrant = new DataSet();
            if (!String.IsNullOrEmpty(hdnClaimId))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", hdnClaimId);

                dsToothQuadrant = SelectPanelsData("Claims_Tooth_and_Surface_Information", parms);
            }
            return dsToothQuadrant;

        }
        public static void updateNDCdetails(string claimid, string claimtype)
        {
            DataSet bindGrid = GetNDCDetails(claimid);
            NDCDetails details = new NDCDetails();

            DataTable dt = new DataTable();
            List<NDCDetails> NDCDetail = new List<NDCDetails>();
            dt = bindGrid.Tables[0];


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {


                    AdditionalProviderInformation ToothQuadrantInfoData = new AdditionalProviderInformation();
                    string specId = dr["Claims_NDC_Details_Screen_ID"].ToString();
                    string otherPayerServiceLine = dr["Service_Line"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Claims_NDC_Details_Screen_ID", DbType.Int32, specId, true));
                        //parametera.Add(SqlParms.CreateParameter("Health_Plan_ID", null));
                        //parametera.Add(SqlParms.CreateParameter("Amount_Paid", null));
                        //parametera.Add(SqlParms.CreateParameter("Paid_unit_Count", null));
                        //parametera.Add(SqlParms.CreateParameter("Last_Modified_user", null));
                        DataAccess.ExecuteStoredProcedure("Usp_update_Claims_NDC_Details", parametera, "Claims_NDC_Details");

                    }

                }
            }
        }
        public static DataSet GetOtherPayerAdjustmentInfo(string ClaimId)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOtherPayerAdjustmentInfo = new DataSet();

            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOtherPayerAdjustmentInfo = SelectPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);
            }
            return dsOtherPayerAdjustmentInfo;
        }
        public static void updateOtherPayerAdjustmentServiceDetail(string claimid, string claimtype)
        {
            DataSet bindGrid = new DataSet();
            if (!string.IsNullOrEmpty(claimid))
            {
                bindGrid = GetOtherPayerAdjustmentInfo(claimid);
            }

            DataTable dt = bindGrid.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AdditionalProviderInformation ToothQuadrantInfoData = new AdditionalProviderInformation();
                    string specId = dr["Other_Payer_Adjustment_Service_Detail_ID"].ToString();
                    string otherPayerServiceLine = dr["Service_Line"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Other_Payer_Adjustment_Service_Detail_ID", DbType.Int32, specId, true));
                        //parametera.Add(SqlParms.CreateParameter("Health_Plan_ID", null));
                        //parametera.Add(SqlParms.CreateParameter("Amount_Paid", null));
                        //parametera.Add(SqlParms.CreateParameter("Paid_unit_Count", null));
                        //parametera.Add(SqlParms.CreateParameter("Last_Modified_user", null));
                        DataAccess.ExecuteStoredProcedure("update_Claims_Other_Payer_Adjustment_Service_Detail", parametera, "claims_other_payer_adjustment_service_detail");

                    }

                }
            }
        }
        public static void updateOtherPayerPaidAmountServiceDetail(string claimid, string claimtype)
        {

            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOPPAmount = new DataSet();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(claimid))
            {
                parms.Add("Claim_ID", claimid);
                dsOPPAmount = SelectPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
            }

            dt = dsOPPAmount.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    AdditionalProviderInformation ToothQuadrantInfoData = new AdditionalProviderInformation();
                    string specId = dr["Other_Payer_Adjudication_Information_Service_Detail_ID"].ToString();
                    string otherPayerServiceLine = dr["Service_Line"].ToString();
                    int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                    string serviceLineUpdate = "";
                    if (serviceline <= 9)
                    {
                        serviceLineUpdate = "0" + serviceline.ToString();
                        List<SqlParameter> parametera = new List<SqlParameter>();
                        parametera.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
                        parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                        parametera.Add(SqlParms.CreateParameter("Other_Payer_Adjudication_Information_Service_Detail_ID", DbType.Int32, specId, true));
                        //parametera.Add(SqlParms.CreateParameter("Health_Plan_ID", null));
                        //parametera.Add(SqlParms.CreateParameter("Amount_Paid", null));
                        //parametera.Add(SqlParms.CreateParameter("Paid_unit_Count", null));
                        //parametera.Add(SqlParms.CreateParameter("Last_Modified_user", null));
                        DataAccess.ExecuteStoredProcedure("update_Claims_Other_Payer_Adjudication_Information_Service_Detail", parametera, "Claims_Other_Payer_Adjudication_Information_Service_Detail");

                    }

                }
            }
        }
        public static DataSet FetchAdditionalProviderInformation(string ClaimID)
        {
            DataSet dsAdditional = new DataSet();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", ClaimID);
            dsAdditional = SelectPanelsData("claims_additional_provider_information_service", parms);
            return dsAdditional;
        }
        public static void updateAdditionalProviderInfoServiceDetail(string claimid, string claimtype)
        {
            List<AdditionalProviderInformation> lstAddProviderInfoData = new List<AdditionalProviderInformation>();
            DataSet dsAdditionalProviderInformation = new DataSet();
            if (!String.IsNullOrEmpty(claimid))
            {
                dsAdditionalProviderInformation = FetchAdditionalProviderInformation(claimid);
                var data = string.Empty;
                DataTable dt = new DataTable();
                dt = dsAdditionalProviderInformation.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdditionalProviderInformation ToothQuadrantInfoData = new AdditionalProviderInformation();
                        string specId = dr["Claims_Additional_Provider_Information_Service_ID"].ToString();
                        string ToothServiceLine = dr["Service_Line"].ToString();
                        int serviceline = Convert.ToInt32(dr["Service_Line"].ToString());
                        string serviceLineUpdate = "";
                        if (serviceline <= 9)
                        {
                            serviceLineUpdate = "0" + serviceline.ToString();
                            List<SqlParameter> parametera = new List<SqlParameter>();
                            parametera.Add(SqlParms.CreateParameter("Claim_id", DbType.Int32, claimid, true));
                            parametera.Add(SqlParms.CreateParameter("Service_Line", DbType.String, serviceLineUpdate, true));
                            parametera.Add(SqlParms.CreateParameter("Claims_Additional_Provider_Information_Service_ID", DbType.Int32, specId, true));
                            DataAccess.ExecuteStoredProcedure("update_Claims_Additional_Provider_Information_Service_Custom", parametera, "Claims_Additional_Provider_Information_Service");

                        }


                    }
                }
            }

        }
        public static List<OtherPayerAdjustmentInfoServiceDetail> SelectOtherPayerAdjustmentServiceDetail(string claimid)
        {
            DataSet bindGrid = GetOtherPayerAdjustmentInfo(claimid);
            OtherPayerAdjustmentInfoServiceDetail OtherPayeAdjInfodetails = new OtherPayerAdjustmentInfoServiceDetail();

            DataTable dtBindgrid = new DataTable();
            List<OtherPayerAdjustmentInfoServiceDetail> AdjustmentDetail = new List<OtherPayerAdjustmentInfoServiceDetail>();
            dtBindgrid = bindGrid.Tables[0];

            if (dtBindgrid.Rows.Count > 0 && OtherPayeAdjInfodetails.Service_Line != "")
            {
                foreach (DataRow dr in dtBindgrid.Rows)
                {
                    OtherPayeAdjInfodetails = new OtherPayerAdjustmentInfoServiceDetail();
                    OtherPayeAdjInfodetails.Service_Line = dr["Service_Line"].ToString();
                    OtherPayeAdjInfodetails.Procedure_Code = dr["Procedure_Code"].ToString();
                    OtherPayeAdjInfodetails.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                    OtherPayeAdjInfodetails.Adjustment_Group = dr["Adjustment_Group"].ToString();
                    OtherPayeAdjInfodetails.Reason_Code = dr["Reason_Code"].ToString();
                    OtherPayeAdjInfodetails.Amount = dr["Amount"].ToString();
                    OtherPayeAdjInfodetails.Quantity = dr["Quantity"].ToString();
                    OtherPayeAdjInfodetails.Claim_ID = dr["Claim_ID"].ToString();
                    OtherPayeAdjInfodetails.Revenue_Code = dr["Revenue_Code"].ToString();
                    OtherPayeAdjInfodetails.Other_Payer_Adjustment_Service_Detail_ID = dr["Other_Payer_Adjustment_Service_Detail_ID"].ToString();
                    AdjustmentDetail.Add(OtherPayeAdjInfodetails);
                }
            }
            return AdjustmentDetail;
        }
        public static List<NDCDetails> SelectNDCDetails(string claimid)
        {
            DataSet bindGrid = GetNDCDetails(claimid);
            NDCDetails details = new NDCDetails();

            DataTable dt = new DataTable();
            List<NDCDetails> NDCDetail = new List<NDCDetails>();
            dt = bindGrid.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    details = new NDCDetails();
                    details.ServiceLine = dr["Service_Line"].ToString();
                    details.NDCCode = dr["NDC"].ToString();
                    details.UnitMeasure = dr["Units_of_Measure"].ToString();
                    details.PrescriptionNuber = dr["Prescription_Number"].ToString();
                    details.TotalUnit = dr["Total_Unit"].ToString();
                    details.Claims_NDC_Details_Screen_ID = dr["Claims_NDC_Details_Screen_ID"].ToString();
                    details.Claim_ID = dr["Claim_ID"].ToString();
                    NDCDetail.Add(details);
                }
            }
            return NDCDetail;
        }
        public static DataSet GetValueCodeDetails(string ClaimId)
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsvalueInformation = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsvalueInformation = SelectPanelsData("Claims_Value_Code_Information", parms);
            }
            return dsvalueInformation;
        }
        public static List<ValueCodeDetail> SelectValueCodeDetails(string claimid)
        {
            DataSet bindGrid = GetValueCodeDetails(claimid);
            ValueCodeDetail ValueCodeDetailsInfodata = new ValueCodeDetail();
            DataTable dt = new DataTable();
            List<ValueCodeDetail> ValueCodeDetailsData = new List<ValueCodeDetail>();
            dt = bindGrid.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ValueCodeDetailsInfodata = new ValueCodeDetail();
                    ValueCodeDetailsInfodata.Line = dr["Line"].ToString();
                    ValueCodeDetailsInfodata.Value_Code = dr["VALUE_CODE"].ToString();
                    ValueCodeDetailsInfodata.Amount = dr["Amount"].ToString();
                    ValueCodeDetailsInfodata.ValueCodeDesc = dr["CLAIMS_VALUE_CODE_DESC"].ToString();
                    ValueCodeDetailsInfodata.ClaimID = dr["Claim_ID"].ToString();
                    ValueCodeDetailsInfodata.hdnValue_Code = dr["Claims_Value_Code_Information_ID"].ToString();
                    ValueCodeDetailsData.Add(ValueCodeDetailsInfodata);
                }
            }
            return ValueCodeDetailsData;
        }
        public static DataSet GetOccurrenceInfoData(string ClaimId)
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOccurenceInformation = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOccurenceInformation = SelectPanelsData("Claims_Occurrence_Information", parms);
            }
            return dsOccurenceInformation;
        }
        public static DataSet GetOccurrenceSpanInfoData(string ClaimId)
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOccurenceInformation = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOccurenceInformation = SelectPanelsData("Claims_Occurrence_Code_Span_Information", parms);
            }
            return dsOccurenceInformation;
        }
        public static List<OccurrenceSpanInfo> SelectOccurrenceSpanDetails(string claimid)
        {
            List<OccurrenceSpanInfo> OccurrenceSpanInfoDataList = new List<OccurrenceSpanInfo>();
            if (!string.IsNullOrEmpty(claimid))
            {
                DataSet bindGrid = GetOccurrenceSpanInfoData(claimid);
                OccurrenceSpanInfo OccurrenceSpanInfodata = new OccurrenceSpanInfo();
                DataTable dt = new DataTable();
                dt = bindGrid.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        OccurrenceSpanInfodata = new OccurrenceSpanInfo();
                        OccurrenceSpanInfodata.Line = dr["Line"].ToString();
                        OccurrenceSpanInfodata.OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();
                        OccurrenceSpanInfodata.FromDate = Convert.ToDateTime(dr["FromDate"]).ToString("MM/dd/yyyy");
                        OccurrenceSpanInfodata.ToDate = Convert.ToDateTime(dr["ToDate"]).ToString("MM/dd/yyyy");
                        OccurrenceSpanInfodata.OccurrenceDesc = dr["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                        OccurrenceSpanInfodata.Claims_Occurrence_Code_Span_Information_ID = dr["Claims_Occurrence_Code_Span_Information_ID"].ToString();
                        OccurrenceSpanInfodata.Claim_ID = dr["Claim_ID"].ToString();
                        OccurrenceSpanInfoDataList.Add(OccurrenceSpanInfodata);
                    }
                }
            }
            return OccurrenceSpanInfoDataList;
        }
        public static List<ICDProcedureCode> GetICDProcedureData(string hdnClaimId)
        {
            List<ICDProcedureCode> ICDProcedureCodeList = new List<ICDProcedureCode>();
            DataSet dsICDProcedureCode = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId, true));
            int sequencePrincipalCount = 0;
            int sequenceOtherCount = 0;
            string SequenceCount_Principal = "0";
            string SequenceCount_Other = "0";
            DataSet dtSequenceDetails = DataAccess.ExecuteStoredProcedure("usp_Claims_ICD_Procedure_Code_Sequence_TotalSequenceCount", parameters, "Claims_ICD_Procedure_Code_Sequence");
            if (Helper.HasRows(dtSequenceDetails))
            {
                var sequencePrincipaldata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "3").ToList();
                var sequenceotherdata = dtSequenceDetails.Tables[0].AsEnumerable().Where(s => s.Field<string>("Sequence_No") == "2").ToList();

                if (sequencePrincipaldata.Count() != 0)
                {
                    sequencePrincipalCount = Convert.ToInt32(sequencePrincipaldata.FirstOrDefault().ItemArray[0]);
                    SequenceCount_Principal = sequencePrincipalCount.ToString();
                }
                if (sequenceotherdata.Count() != 0)
                {
                    sequenceOtherCount = Convert.ToInt32(sequenceotherdata.ToList().FirstOrDefault().ItemArray[0]);
                    if (sequenceOtherCount == 24)
                    {
                        SequenceCount_Other = sequenceOtherCount.ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(hdnClaimId))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", hdnClaimId);
                dsICDProcedureCode = SelectPanelsData("claims_icd_procedureCode", parms);
            }

            DataTable dt = new DataTable();
            dt = dsICDProcedureCode.Tables[0];

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
                    ICD_Procedue_Code.SequeneCount_Other = sequenceOtherCount.ToString();
                    ICD_Procedue_Code.SequeneCount_Principal = sequencePrincipalCount.ToString();
                    ICDProcedureCodeList.Add(ICD_Procedue_Code);
                }
            }

            return ICDProcedureCodeList;
        }
        public static List<ConditionCodeDetail> SelectConditionCodeData(string claimid)
        {
            DataSet bindGrid = GetConditionCodeDetails(claimid);
            ConditionCodeDetail ConditionCodeDetailsInfodata = new ConditionCodeDetail();
            DataTable dt = new DataTable();
            List<ConditionCodeDetail> ConditionCodeDetailsData = new List<ConditionCodeDetail>();
            dt = bindGrid.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ConditionCodeDetailsInfodata = new ConditionCodeDetail();
                    ConditionCodeDetailsInfodata.Line = dr["Line"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCode = dr["VALUE_CODE"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCodeDesc = dr["Amount"].ToString();
                    ConditionCodeDetailsInfodata.ClaimID = dr["Claim_ID"].ToString();
                    ConditionCodeDetailsInfodata.hdnConditionCode = dr["Claims_Value_Code_Information_ID"].ToString();
                    ConditionCodeDetailsData.Add(ConditionCodeDetailsInfodata);
                }
            }
            return ConditionCodeDetailsData;
        }
        public static DataSet GetConditionCodeDetails(string ClaimId)
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsConditionInformation = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsConditionInformation = SelectPanelsData("Claims_Condition_Code_Information", parms);
            }
            return dsConditionInformation;
        }
        public static List<ConditionCodeDetail> SelectConditionCodeDetails(string claimid)
        {
            DataSet bindGrid = GetConditionCodeDetails(claimid);
            ConditionCodeDetail ConditionCodeDetailsInfodata = new ConditionCodeDetail();
            DataTable dt = new DataTable();
            List<ConditionCodeDetail> ConditionCodeDetailsData = new List<ConditionCodeDetail>();
            dt = bindGrid.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ConditionCodeDetailsInfodata = new ConditionCodeDetail();
                    ConditionCodeDetailsInfodata.Line = dr["Line"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCode = dr["Condition_Code"].ToString();
                    ConditionCodeDetailsInfodata.ConditionCodeDesc = dr["Claims_Condition_Code_Description"].ToString();
                    ConditionCodeDetailsInfodata.ClaimID = dr["Claim_ID"].ToString();
                    ConditionCodeDetailsInfodata.hdnConditionCode = dr["Claims_Condition_Code_Information_ID"].ToString();
                    ConditionCodeDetailsData.Add(ConditionCodeDetailsInfodata);
                }
            }
            return ConditionCodeDetailsData;
        }
        public static DataSet OccurrenceInfoData(string ClaimId)
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOccurenceInformation = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOccurenceInformation = SelectPanelsData("Claims_Occurrence_Information", parms);
            }
            return dsOccurenceInformation;
        }
        public static Boolean validateToothNumber(string txtValue)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("TOOTH_NUMBER", DbType.String, txtValue, true));

            DataSet dsToothNumber = DataAccess.ExecuteStoredProcedure("usp_SelectPrior_auth_submit_claim_tooth_number", parameters, "TOOTH_NUMBER");

            if (Helper.HasRows(dsToothNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<PDMSRestServices.Models.ToothQuadrantInfo> GetToothQuadrtantPanelInfo(string hdnClaimId)
        {
            List<PDMSRestServices.Models.ToothQuadrantInfo> ToothQuadrantInfoList = new List<PDMSRestServices.Models.ToothQuadrantInfo>();
            DataSet dsToothQuadrantInformation = new DataSet();
            if (!String.IsNullOrEmpty(hdnClaimId))
            {
                dsToothQuadrantInformation = FetchToothInformation(hdnClaimId);
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
            return ToothQuadrantInfoList;
        }
        public static List<OccurenceInfo> GettOccurrenceDetails(string claimid)
        {
            DataSet bindGridOccInfo = OccurrenceInfoData(claimid);
            OccurenceInfo OccurrenceSpanInfodata = new OccurenceInfo();

            DataTable dt = new DataTable();
            List<OccurenceInfo> OccurrenceInfo = new List<OccurenceInfo>();
            dt = bindGridOccInfo.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OccurrenceSpanInfodata = new OccurenceInfo();
                    OccurrenceSpanInfodata.Line = dr["Claims_Occurrence_Line"].ToString();
                    OccurrenceSpanInfodata.OccurrenceInfoCode = dr["OccurrenceCode"].ToString();
                    OccurrenceSpanInfodata.OccurrenceDate = Convert.ToDateTime(dr["OccurrenceDate"]).ToString("MM/dd/yyyy");
                    OccurrenceSpanInfodata.OccurrenceDesc = dr["OccurrenceDesc"].ToString();
                    OccurrenceSpanInfodata.ClaimsOccurrenceInformationID = dr["ClaimsOccurrenceInformationID"].ToString();
                    OccurrenceSpanInfodata.ClaimId = dr["ClaimId"].ToString();
                    OccurrenceInfo.Add(OccurrenceSpanInfodata);
                }
            }
            return OccurrenceInfo;
        }
        public static List<ToothSurfaceList> GetToothSurfaceList(DataTable ToothSurface)
        {
            List<ToothSurfaceList> ListToothSurface = new List<ToothSurfaceList>();

            DataTable dt = new DataTable();
            dt = ToothSurface;

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ToothSurfaceList ToothSurfaceData = new ToothSurfaceList();
                    ToothSurfaceData.TOOTHSURFACE_ID = dr["PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE_ID"].ToString();
                    ToothSurfaceData.TOOTHSURFACE = dr["PRIOR_AUTH_SUBMIT_CLAIM_TOOTH_SURFACE"].ToString();
                    ListToothSurface.Add(ToothSurfaceData);
                }
            }
            return ListToothSurface;
        }
        public static List<OPPAServiceDetail> GetOtherPayerPaidAmount(string ClaimId)
        {

            List<OPPAServiceDetail> OPPAServiceDetailList = new List<OPPAServiceDetail>();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            DataSet dsOPPAmount = new DataSet();
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                parms.Add("Claim_ID", ClaimId);
                dsOPPAmount = SelectPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
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
            return OPPAServiceDetailList;
        }
        public static Models.RecipientInformation FindRecipient(string MedicaidID, Guid UserId, string medicaidbillingnumber, string dateofBirth)
        {
            EligibilityFacade recipientEligibility = new EligibilityFacade();
            Models.RecipientInformation recipientInfo = recipientEligibility.GetRecipientInformation(MedicaidID, UserId, medicaidbillingnumber, dateofBirth, "ClaimEligibility");
            return recipientInfo;
        }
        public static DataSet FetchHealthPlanIDWithServiceLine(string ClaimID, string serviceline)
        {

            DataSet dsServiceLine = new DataSet();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", ClaimID);
            parms.Add("Service_Line", serviceline);
            dsServiceLine = SelectPanelsData("claims_other_payer_adjudication_servicedetail_with_serviceline", parms);
            return dsServiceLine;
        }
        public static List<HealthPlanIDOPPA> GetHealthPlanIDForOPPAmountServiceDetail(string ClaimId)
        {

            List<HealthPlanIDOPPA> healthPlanIDOPPAList = new List<HealthPlanIDOPPA>();
            Dictionary<string, string> param = new Dictionary<string, string>();
            DataSet dsHealthID = new DataSet();
            if (!string.IsNullOrEmpty(ClaimId))
            {
                param.Add("Claim_ID", ClaimId.ToString());
                dsHealthID = SelectPanelsData("claims_other_payer_information", param);

                if (Helper.HasRows(dsHealthID) && Convert.ToInt32(dsHealthID.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimId))
                {
                    DataTable dt = dsHealthID.Tables[0];
                    IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                        where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Details) &&
                                                        row.Field<string>("Claim_Adjudication_Level") != null)
                                                        select row;
                    if (healthplanid.Count() > 0)
                    {
                        DataTable dthealthplanid = healthplanid.CopyToDataTable();
                        if (dthealthplanid.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dthealthplanid.Rows)
                            {
                                HealthPlanIDOPPA HealthPlanData = new HealthPlanIDOPPA();
                                HealthPlanData.Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                                HealthPlanData.Claims_Other_Payer_Information_ID = dr["Claims_Other_Payer_Information_ID"].ToString();
                                healthPlanIDOPPAList.Add(HealthPlanData);
                            }
                        }

                    }
                }

            }

            return healthPlanIDOPPAList;
        }
        public static DataSet FetchAdditionalProviderInformationWithServiceLine(string serviceline, string Claim_ID)
        {
            DataSet dsAdditionalServiceLine = new DataSet();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", Claim_ID);
            parms.Add("Service_Line", serviceline);
            dsAdditionalServiceLine = SelectPanelsData("claims_additional_provider_information_filter_serviceline", parms);
            return dsAdditionalServiceLine;
        }
        public static void GetAdditionalProviderData(DataSet dsAdditionalInformation, string ClaimID)
        {
            DataSet dsAdditionInfo = new DataSet();

            string hdnSequenceidAdditionalPanel = string.Empty;
            if (!string.IsNullOrEmpty(ClaimID))
            {

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", ClaimID);
                DataSet dsAdditional = SelectPanelsData("claims_additional_provider_information_service", parms);

                if (Helper.HasRows(dsAdditional) &&
                    Convert.ToInt32(dsAdditional.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimID))

                    if (Helper.HasRows(dsAdditionalInformation))
                    {
                        dsAdditionInfo = dsAdditionalInformation;
                    }
                    else
                    {
                        hdnSequenceidAdditionalPanel = dsAdditional.Tables[0].Rows[0]["Claims_Additional_Provider_Information_Service_ID"].ToString();
                        dsAdditionInfo = FetchAdditionalProviderInformation(ClaimID);
                    }
                // gvAdditionalProviderinfo.DataSource = dsAdditionInfo;
            }
            if (Helper.HasRows(dsAdditionInfo))
            {
                dsAdditionInfo.Tables[0].DefaultView.Sort = "Service_Line ASC";
                //gvAdditionalProviderinfo.DataSource = dsAdditionInfo;
                //gvAdditionalProviderinfo.DataBind();
            }
            else
            {
                //gvAdditionalProviderinfo.DataSource = null;
                //gvAdditionalProviderinfo.DataBind();
            }
            //if (DisplayReadOnly == true)
            //{
            //    divAddProv.Visible = false;
            //}
            //else
            //{
            //    divAddProv.Visible = true;
            //}
        }
        public static List<AdditionalProviderInformation> SelectAdditonalProviderInfoDetails(string claimid, string ClaimType)
        {
            DataSet additionalDetails = FetchAdditionalProviderInformation(claimid);
            AdditionalProviderInformation OccurrenceSpanInfodata = new AdditionalProviderInformation();
            DataTable dt = new DataTable();
            List<AdditionalProviderInformation> lstAdditionalProviderInformation = new List<AdditionalProviderInformation>();
            dt = additionalDetails.Tables[0];
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
                    if (ClaimType == CON.ClaimsType.Professional)
                        AddAdditionProviderInfo.MedicaidID = dr["Medicaid_ID"].ToString();
                    AddAdditionProviderInfo.Claims_Additional_Provider_Information_Service_ID = dr["Claims_Additional_Provider_Information_Service_ID"].ToString();
                    lstAdditionalProviderInformation.Add(AddAdditionProviderInfo);
                }
            }
            return lstAdditionalProviderInformation;
        }
        public static DataTable GetNPIForAdditional(string ProviderNPI)
        {
            DataTable dt = null;


            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("ProviderNPI", DbType.String, ProviderNPI, true));

            var ds = DataAccess.ExecuteStoredProcedure("usp_Search_NPI", parameters, "Search_NPI");

            if (ds != null)
            {
                dt = ds.Tables[0];
            }

            return dt;
        }
        public static List<NPIData> GetDataforNPI(string ProviderNPI)
        {
            string errormsg = string.Empty;
            string ProviderName = string.Empty;
            List<NPIData> lstdata = new List<NPIData>();
            NPIData NPIdata = new NPIData();
            if (!string.IsNullOrEmpty(ProviderNPI))
            {
                if (ProviderNPI.Length == 10)
                {
                    var isValid = Claims.ValidProviderNPI(ProviderNPI.Trim());
                    if (!isValid)
                    {
                        errormsg = string.Concat(ProviderNPI, "NPI is not found in the system");
                        //if (_lblMedicaidId != null)
                        //{
                        //    _lblMedicaidId = "";
                        //}                  
                        NPIdata.Errormessage = errormsg;
                        lstdata.Add(NPIdata);
                        //return;
                    }
                    else
                    {
                        DataTable dt = Claims.GetData(ProviderNPI, "", "", "");
                        if (dt.Rows.Count > 0)
                        {

                            dt = dt.Select("NPI <> ''").CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                //if (_lblMedicaidId != null)
                                //{
                                //    _lblMedicaidId = row["MEDICAID_ID"].ToString();
                                //}                           
                                ProviderName = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                                NPIdata.FirstName = row["FIRST_NAME"].ToString();
                                NPIdata.LastName = row["LAST_OR_BUSINESS_NAME"].ToString();
                                lstdata.Add(NPIdata);
                            }

                        }
                    }
                }
                else
                {
                    errormsg = "10-digit number is required";
                    NPIdata.Errormessage = errormsg;
                    lstdata.Add(NPIdata);
                }
            }

            return lstdata;
        }
        public static DataSet SelectPanelsData(string tableName, Dictionary<string, string> parms)
        {
            DataSet ds = new DataSet();
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedproc = string.Empty;
                switch (tableName.ToLower())
                {
                    case "claim_adjudication_summary_status_screen": storedproc = "usp_Selectclaim_Adjudication_Summary_Status_Screen"; break;
                    case "claims_recipent_information": storedproc = "usp_SelectClaims_Recipient_Information"; break;
                    case "claims_tooth_and_surface_information": storedproc = "usp_SelectClaims_Tooth_and_Surface_Information"; break;
                    case "claims_accident_information": storedproc = "usp_Selectclaims_accident_information"; break;
                    case "claims_other_payer_information": storedproc = "usp_SelectClaims_Other_Payer_Information"; break;
                    case "claim_prior_authorization_referral": storedproc = "usp_SelectClaim_Prior_Authorization_Referral"; break;
                    case "claims_service_details": storedproc = "usp_SelectClaims_Service_Details"; break;
                    case "claims_outpatient_adjudication_information": storedproc = "usp_SelectClaims_Outpatient_Adjudication_Information"; break;
                    case "claims_delayed_submission_resubmission_information": storedproc = "usp_SelectClaims_Delayed_Submission_Resubmission_Information"; break;
                    case "claims_service_information": storedproc = "usp_SelectClaims_Service_Details_GetServiceDate"; break;
                    case "claims_service_facility_information": storedproc = "usp_SelectClaims_Service_Facility_Information"; break;
                    case "claims_additional_provider_information_service": storedproc = "usp_Select_Claims_Additional_Provider_Information_Service"; break;
                    case "claims_diagnosis_information": storedproc = "usp_SelectClaimsDiagnosisInformation"; break;
                    case "claims_header_other_payer_adjustment_information":
                        storedproc = "usp_SelectClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "edit_claims_header_other_payer_adjustment_information":
                        storedproc = "usp_EditClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "claims_other_payer_adjudication_information_service_detail": storedproc = "usp_SelectClaimsOtherPayerAdjudicationInformation"; break;
                    case "claims_providers_note": storedproc = "Usp_SelectClaim_Provider_Notes"; break;
                    case "notereferencecode": storedproc = "Usp_Select_ProviderNote_ReferenceCode"; break;
                    case "claims_other_payer_adjustment_service_detail": storedproc = "usp_SelectClaims_Other_Payer_Adjustment_Service_Detail"; break;
                    case "claims_ambulance_pick_up_drop_off_location": storedproc = "usp_Select_Claims_Ambulance_Pick_Up_Drop_Off_Location"; break;
                    case "claims_ambulance_information": storedproc = "usp_Select_Claims_Ambulance_Information"; break;
                    case "claims_service_information_professional": storedproc = "Usp_Select_Claims_Service_Information_Professional"; break;
                    case "claims_service_information_institutional": storedproc = "Usp_Select_Claims_Service_Information_Institutional"; break;
                    case "claims_ndc_details": storedproc = "Usp_Select_Claims_NDC_Details"; break;

                    case "claims_occurrence_information": storedproc = "Usp_Select_Claims_Occurrence_Information"; break;

                    case "claims_inpatient_adjudication_information": storedproc = "usp_Select_Inpatient_Adjudication_Information"; break;
                    case "claims_icd_procedurecode": storedproc = "Usp_Select_Claims_ICD_Procedure_Code_Sequence"; break;
                    case "claims_occurrence_code_span_information": storedproc = "usp_Select_Claims_Occurrence_Code_Span_Information"; break;
                    case "claims_occurrence_code": storedproc = "Usp_Select_Claims_Occurrence_Code"; break;
                    case "claims_condition_code_information": storedproc = "usp_Select_Claims_Condition_Code_Information"; break;
                    case "claims_sequence_description": storedproc = "Usp_SelectClaimsDiagnosis_Sequence_Description"; break;
                    case "claims_value_code_information": storedproc = "Usp_Select_Claims_Value_Code_Information"; break;
                    case "claims_value_code": storedproc = "usp_Select_Claims_Value_Code"; break;
                    case "claims_attachment_screen": storedproc = "usp_Select_Claims_Attachments"; break;
                    case "claims_additional_provider_information_filter_serviceline": storedproc = "usp_Select_Claims_Additional_Provider_Information_Filter_ServiceLine"; break;
                    case "claims_other_payer_adjudication_servicedetail_with_serviceline": storedproc = "usp_Select_Claims_Other_Payer_Adjudication_ServiceDetail_With_ServiceLine"; break;

                    case "claims_search_details": storedproc = "usp_Search_ClaimDetails"; break;
                    case "claims_search_details_inst": storedproc = "usp_Search_ClaimDetails_Inst"; break;
                    case "claims_search_details_prof": storedproc = "usp_Search_ClaimDetails_Prof"; break;
                    case "destinationpayerid": storedproc = "usp_Select_DESTINATIONPAYERID"; break;
                    case "taxonomycodeforbillingprovider": storedproc = "usp_select_taxonomycodeforbillingprovider"; break;
                    case "claims_outbound_document_uploads": storedproc = "usp_Select_Claims_Attachments_Files"; break;
                    case "claim_attachment_type": storedproc = "usp_Select_Claims_Attachments_DocTypes"; break;
                    case "get_otherpayerpaidamount": storedproc = "usp_Get_OtherPayerPaidAmount"; break;
                    case "selectadjustmentgroupsforheaderotherpayeronedit": storedproc = "usp_SelectAdjustmentGroupsForHeaderOtherPayerOnEdit"; break;
                    case "selectadjustmentgroupsforotherpayeradjustmentonedit": storedproc = "usp_SelectAdjustmentGroupsForOtherPayerAdjustmentOnEdit"; break;
                    case "get_otherpayerpaidamount_onedit": storedproc = "usp_Get_OtherPayerPaidAmountOnEdit"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                ds = DataAccess.ExecuteStoredProcedure(storedproc, parameters, tableName);
                return ds;

            }
            catch (Exception ex)
            {
                //throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                //  ex.Message + " - " + ex.StackTrace));
            }
            return ds;
        }
      
    }
}


