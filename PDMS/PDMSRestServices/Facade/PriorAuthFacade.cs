using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Facade
{
    public static class PriorAuthFacade
    {
        public static List<DiagnosisCodeTypes> GetDiagnosisCodeType()
        {
            DataSet ds = LookupTableController.GetDiagnosisCodeType();
            DataTable dt = ds.Tables[0];
            var data = string.Empty;

            List<DiagnosisCodeTypes> list = new List<DiagnosisCodeTypes>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DiagnosisCodeTypes diagnosisCodeTypes = new DiagnosisCodeTypes();
                    diagnosisCodeTypes.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]);
                    diagnosisCodeTypes.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"]);
                    list.Add(diagnosisCodeTypes);
                }
                return list;
            }
            return list;
        }

        public static List<PriorAuthDiagnosis> GetPriorAuthDiagnosisFromSession(string paType, string medicaidID, string linkId)
        {
            DataSet ds = null;
            DataTable dt = null;
            List<PriorAuthDiagnosis> diagnosis = new List<PriorAuthDiagnosis>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", paType);
            parms.Add("MedicaidID", medicaidID);
            parms.Add("LINK_SECTIONS", linkId);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

            if (ds != null)
                dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PriorAuthDiagnosis priorAuthDiagnosis = new PriorAuthDiagnosis();

                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_ID = dr["PRIOR_AUTH_DIAGNOSIS_ID"] != null && dr["PRIOR_AUTH_DIAGNOSIS_ID"] != DBNull.Value && dr["PRIOR_AUTH_DIAGNOSIS_ID"].ToString().Length > 0 ? Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_ID"]) : 0;
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DESC = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_DESC"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_DATE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_STATUS = Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_STATUS"]);
                    priorAuthDiagnosis.PRIOR_AUTH_TYPE = Convert.ToInt32(dr["PRIOR_AUTH_TYPE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID = dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] != null && dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] != DBNull.Value && dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString().Length > 0 ? Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) : 0;

                    diagnosis.Add(priorAuthDiagnosis);
                }
            }
            return diagnosis;
        }


        public static string ValidateDiagnosisLineItem(int claimID, string medicaidID, string codeType, string codeValue, string linkId)
        {
            try
            {

                DataSet ds = new DataSet();

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("PRIOR_AUTH_TYPE", claimID.ToString());
                parms.Add("MedicaidID", medicaidID);
                parms.Add("LINK_SECTIONS", linkId);
                ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

                if (ds.Tables[0].Columns.IndexOf("RowState") > 0)
                {
                    DataRow[] rowsToRemove = ds.Tables[0].Select("RowState='delete'");
                    if (rowsToRemove != null && rowsToRemove.Count() > 0)
                    {
                        foreach (DataRow dataRow in rowsToRemove)
                        {
                            ds.Tables[0].Rows.Remove(dataRow);
                        }
                    }
                }


                if (ds != null && ds.Tables.Count > 0)
                {
                    var dataTable = ds.Tables[0];
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows.Count >= 12)
                        {
                            return "Diagnosis Line Items Exeeded";
                        }
                        else
                        {
                            // btnDiagnosisAdd.Visible = true;
                        }

                        if (codeType.ToUpper() == "PRINCIPAL")
                        {
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == Convert.ToString(codeValue))
                                {
                                    //lblDiagnosisSaveError.Visible = true;
                                    //lblDiagnosisSaveError.Text = "Principal diagnosis type cannot be added more than one.";
                                    return "Principal diagnosis type cannot be added more than one.";
                                }
                            }
                        }
                        if (codeType.ToUpper() == "ADMITTING")
                        {
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                if (Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) == Convert.ToString(codeValue))
                                {
                                    //lblDiagnosisSaveError.Visible = true;
                                    //lblDiagnosisSaveError.Text = "Admitting diagnosis type cannot be added more than one.";
                                    return "Admitting diagnosis type cannot be added more than one.";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (codeType.ToUpper() != "PRINCIPAL")
                        {
                            //lblDiagnosisSaveError.Visible = true;
                            //lblDiagnosisSaveError.Text = "Default line 01 should be Principal.";
                            return "Default line 01 should be Principal.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "";
        }


        public static List<PriorAuthDiagnosis> GetPriorAuthDiagnosisFromSession(int paType, string medicaidID, string linkId)
        {
            DataSet ds = null;
            DataTable dt = null;
            List<PriorAuthDiagnosis> diagnosis = new List<PriorAuthDiagnosis>();

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("PRIOR_AUTH_TYPE", paType.ToString());
            parms.Add("MedicaidID", medicaidID);
            parms.Add("LINK_SECTIONS", linkId);
            ds = PriorAuthHospitalController.GetPriorAuthPanelData("GETPRIORAUTH_DIAGNOSIS", parms);

            if (ds != null)
                dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PriorAuthDiagnosis priorAuthDiagnosis = new PriorAuthDiagnosis();

                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_ID = dr["PRIOR_AUTH_DIAGNOSIS_ID"] != null && dr["PRIOR_AUTH_DIAGNOSIS_ID"] != DBNull.Value && dr["PRIOR_AUTH_DIAGNOSIS_ID"].ToString().Length > 0 ? Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_ID"]) : 0;
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_CODE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DESC = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_DESC"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_DATE = Convert.ToString(dr["PRIOR_AUTH_DIAGNOSIS_DATE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_STATUS = Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_STATUS"]);
                    priorAuthDiagnosis.PRIOR_AUTH_TYPE = Convert.ToInt32(dr["PRIOR_AUTH_TYPE"]);
                    priorAuthDiagnosis.PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID = dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] != null && dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"] != DBNull.Value && dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"].ToString().Length > 0 ? Convert.ToInt32(dr["PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID"]) : 0;

                    diagnosis.Add(priorAuthDiagnosis);
                }
            }
            return diagnosis;
        }

        public static string GetToothInfoByToothID(string toothidentifier, bool isCodeAvailable)
        {
            string tooth = string.Empty;

            if (!isCodeAvailable && string.IsNullOrEmpty(toothidentifier))
            {
                return "0";
            }
            if (isCodeAvailable && string.IsNullOrEmpty(toothidentifier))
            {
                return "";
            }

            List<DentalToothInfo> toothInfo = LoadDentalToothInfo();

            if (isCodeAvailable)
            {
                if (toothInfo != null && toothInfo.Count > 0)
                {
                    DentalToothInfo DentalToothInfo = toothInfo.Where(t => t.PRIOR_AUTH_TOOTH_NUMBER_CODE == toothidentifier).FirstOrDefault();
                    tooth = DentalToothInfo != null ? Convert.ToString(DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_ID) : "";
                }
            }
            else
            {
                if (toothInfo != null && toothInfo.Count > 0)
                {
                    DentalToothInfo DentalToothInfo = toothInfo.Where(t => t.PRIOR_AUTH_TOOTH_NUMBER_ID == toothidentifier).FirstOrDefault();
                    tooth = DentalToothInfo != null ? DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_CODE : "";
                }
            }
            return tooth;
        }

        private static List<DentalToothInfo> LoadDentalToothInfo()
        {
            //string key = "DentalToothInfo";
            List<DentalToothInfo> toothInfo = new List<DentalToothInfo>();

            //if (!DictionaryCache.Exists(key))
            //{

                DataSet dsToothNumbers = PriorAuthHospitalController.GetPriorAuthDentalToothNumber();
                DataTable dtToothNumber = dsToothNumbers.Tables[0];

                if (dtToothNumber != null && dtToothNumber.Rows.Count > 0)
                {
                    foreach (DataRow currentRow in dtToothNumber.Rows)
                    {
                        DentalToothInfo DentalToothInfo = new DentalToothInfo();

                        DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_ID = currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != null && currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_TOOTH_NUMBER_ID"]) : "";
                        DentalToothInfo.PRIOR_AUTH_TOOTH_NUMBER_CODE = currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != null && currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"] != DBNull.Value ? Convert.ToString(currentRow["PRIOR_AUTH_TOOTH_NUMBER_CODE"]) : "";

                        toothInfo.Add(DentalToothInfo);
                    }

                    //DictionaryCache.Add<List<DentalToothInfo>>(key, toothInfo);
                }
            //}
            //else
            //{
            //    toothInfo = DictionaryCache.Get<List<DentalToothInfo>>(key);
            //}
            return toothInfo;
        }

        public static DataSet FetchServiceDetailsInformation(string claimID)
        {
            DataSet dsServiceDetailsInfo = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimID, true));
            dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            return dsServiceDetailsInfo;
        }

        public static void SaveInstitutionalServiceDetailsDataToDb(ServiceDetailsIstitutionalSaveReq req)
        {
            try
            {

                int codeTypeId = !string.IsNullOrEmpty(req.codeType) ? Convert.ToInt32(req.codeType) : 0;
                int levelofCareId = !string.IsNullOrEmpty(req.levelCareID) ? Convert.ToInt32(req.levelCareID) : 0;
                int unitMeasurementId = !string.IsNullOrEmpty(req.unitMeasure) ? Convert.ToInt32(req.unitMeasure) : 0;

                decimal reqUnitsFee = !string.IsNullOrEmpty(req.requnitsfee) ? Convert.ToDecimal(req.requnitsfee) : 0;

                // decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);

                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                // int serviceTrackingNo = Convert.ToInt64(txtLnServiceTrackingNo.Text);
                int statusId = 1;
                int authUnits = 0;

                Dictionary<string, object> parms = new Dictionary<string, object>();


                //parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR", reqUnitsFee);

                parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", codeTypeId);
                parms.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", Convert.ToString(req.revenueCode));
                parms.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", Convert.ToString(req.procedureCode));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", Convert.ToString(req.reqUnits));
                parms.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", unitMeasurementId);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", reqUnitsFee);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                parms.Add("PRIOR_AUTH_STATUS_ID", 1);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", Convert.ToString(req.procCodeDesc));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", Convert.ToString(req.providerServiceNote));
                parms.Add("PRIOR_AUTH_LEVEL_CARE_ID", levelofCareId);
                int remainingUnits = !string.IsNullOrEmpty("") ? Convert.ToInt32("") : 0;
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", authUnits);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", 0);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", "");
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", ""); ;
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", req.trackingNo);

                parms.Add("MedicaidID", Convert.ToString(req.MedicaidId));
                parms.Add("Line", 0);    //0

                parms.Add("LINK_SECTIONS", new Guid());

                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);


                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_SERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SaveProfessionalServiceDetailsDataToDb(ServiceDetailsProfessionalSaveReq req)
        {
            try
            {

                decimal reqUnitsFee = Convert.ToDecimal(req.requnitsfee);
                decimal authDollars = 0;
                int authUnits = 0;
                int remainingUnits = !string.IsNullOrEmpty(req.remainUnits) ? Convert.ToInt32(req.remainUnits) : 0;

                if (req.serviceTrackingNo == null)
                    req.serviceTrackingNo = string.Empty;

                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                var requestUser = Guid.NewGuid();
                int statusId = 1;
                if (req.status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (req.status == "Approved")
                {
                    statusId = 2;
                }
                else if (req.status == "Denied")
                {
                    statusId = 3;
                }

                Dictionary<string, object> parms = new Dictionary<string, object>();

                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", req.procedureCode);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", req.modifier1);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", req.modifier2);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", req.modifier3);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", req.modifier4);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", req.reqUnits);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", reqFDOS);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", reqTDOS);
                parms.Add("PRIOR_AUTH_STATUS_ID", statusId.ToString());
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", req.procCodeDesc);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", req.providerServiceNote);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", req.serviceTrackingNo);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", authDollars);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", remainingUnits);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", req.unitMeasurement);


                parms.Add("MedicaidID", req.MedicaidId);
                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", new Guid());
                parms.Add("Line", 0);

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_PROFFSERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SaveDentalServiceDetailsDataToDb(ServiceDetailsDentalSaveReq req)
        {
            try
            {

                int toothNumber = Convert.ToInt32(req.toothNum);
                int prosthesisNumber = Convert.ToInt32(req.prosthesisNum);

                int authUnits = 0;
                decimal authDollars = 0;

                decimal reqUnitsFee = Convert.ToDecimal(req.requnitsfee);
                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                var requestUser = Guid.NewGuid();
                int statusId = 1;
                if (req.status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (req.status == "Approved")
                {
                    statusId = 2;
                }
                else if (req.status == "Denied")
                {
                    statusId = 3;
                }


                int remainingUnits = !string.IsNullOrEmpty(req.remainUnits) ? Convert.ToInt32(req.remainUnits) : 0;

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", toothNumber);
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", req.procedureCode);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", req.cavity1);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", req.cavity2);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", req.cavity3);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", req.cavity4);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", req.cavity5);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", req.surface1);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", req.surface2);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", req.surface3);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", req.surface4);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", req.surface5);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", req.reqUnits);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                parms.Add("PRIOR_AUTH_STATUS_ID", Convert.ToString(statusId));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", Convert.ToString(req.procCodeDesc));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", Convert.ToString(req.providerServiceNote));
                parms.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", req.prosthesisNum);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", req.serviceTrackingNo);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", Convert.ToString(authDollars));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                parms.Add("MedicaidID", Convert.ToString(req.MedicaidId));
                parms.Add("Line", 0);
                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", new Guid());

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_DENTALSERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateInstitutionalServiceDetailsDataToDb(ServiceDetailsIstitutionalSaveReq req)
        {
            try
            {

                int codeTypeId = !string.IsNullOrEmpty(req.codeType) ? Convert.ToInt32(req.codeType) : 0;
                int levelofCareId = !string.IsNullOrEmpty(req.levelCareID) ? Convert.ToInt32(req.levelCareID) : 0;
                int unitMeasurementId = !string.IsNullOrEmpty(req.unitMeasure) ? Convert.ToInt32(req.unitMeasure) : 0;

                decimal reqUnitsFee = !string.IsNullOrEmpty(req.requnitsfee) ? Convert.ToDecimal(req.requnitsfee) : 0;

                // decimal reqUnitsFee = Convert.ToDecimal(requnitsfee);

                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                // int serviceTrackingNo = Convert.ToInt64(txtLnServiceTrackingNo.Text);
                int statusId = 1;
                int authUnits = 0;

                Dictionary<string, object> parms = new Dictionary<string, object>();


                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_ID", Convert.ToInt32(req.lineNumber));
                parms.Add("PRIOR_AUTH_SERVICE_CODE_TYPE_ID", codeTypeId);

                parms.Add("PRIOR_AUTH_SERVICE_REVENUE_CODE", Convert.ToString(req.revenueCode));
                parms.Add("PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE", Convert.ToString(req.procedureCode));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS", Convert.ToString(req.reqUnits));
                parms.Add("PRIOR_AUTH_REQUESTED_UNITS_ID", unitMeasurementId);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE", reqUnitsFee);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                parms.Add("PRIOR_AUTH_STATUS_ID", 1);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC", Convert.ToString(req.procCodeDesc));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE", Convert.ToString(req.providerServiceNote));
                parms.Add("PRIOR_AUTH_LEVEL_CARE_ID", levelofCareId);
                int remainingUnits = !string.IsNullOrEmpty("") ? Convert.ToInt32("") : 0;
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS", authUnits);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR", 0);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS", "");
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS", ""); ;
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO", req.trackingNo);
                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("Created_By_User", new Guid(CON.appAdminUserId));
                parms.Add("Created_On_Date_Time", DateTime.Now);


                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("updatePRIOR_AUTH_SERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateProfessionalServiceDetailsDataToDb(ServiceDetailsProfessionalSaveReq req)
        {
            try
            {

                decimal reqUnitsFee = Convert.ToDecimal(req.requnitsfee);
                decimal authDollars = 0;
                int authUnits = 0;
                int remainingUnits = !string.IsNullOrEmpty(req.remainUnits) ? Convert.ToInt32(req.remainUnits) : 0;

                if (req.serviceTrackingNo == null)
                    req.serviceTrackingNo = string.Empty;

                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                var requestUser = Guid.NewGuid();
                int statusId = 1;
                if (req.status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (req.status == "Approved")
                {
                    statusId = 2;
                }
                else if (req.status == "Denied")
                {
                    statusId = 3;
                }

                Dictionary<string, object> parms = new Dictionary<string, object>();

                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_ID", Convert.ToString(req.lineNumber));
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", req.procedureCode);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1", req.modifier1);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2", req.modifier2);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3", req.modifier3);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4", req.modifier4);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS", req.reqUnits);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS", reqFDOS);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS", reqTDOS);
                parms.Add("PRIOR_AUTH_STATUS_ID", statusId.ToString());
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC", req.procCodeDesc);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", req.providerServiceNote);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM", req.serviceTrackingNo);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR", authDollars);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS", remainingUnits);
                parms.Add("PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID", req.unitMeasurement);


                parms.Add("MedicaidID", req.MedicaidId);
                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", new Guid());
                parms.Add("Line", 0);

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_PROFFSERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateDentalServiceDetailsDataToDb(ServiceDetailsDentalSaveReq req)
        {
            try
            {
                int toothNumber = Convert.ToInt32(req.toothNum);
                int prosthesisNumber = Convert.ToInt32(req.prosthesisNum);

                int authUnits = 0;
                decimal authDollars = 0;

                decimal reqUnitsFee = Convert.ToDecimal(req.requnitsfee);
                DateTime reqFDOS = Convert.ToDateTime(req.fdos);
                DateTime reqTDOS = Convert.ToDateTime(req.tdos);

                var requestUser = Guid.NewGuid();
                int statusId = 1;
                if (req.status == "Submission Pending")
                {
                    statusId = 1;
                }
                else if (req.status == "Approved")
                {
                    statusId = 2;
                }
                else if (req.status == "Denied")
                {
                    statusId = 3;
                }


                int remainingUnits = !string.IsNullOrEmpty(req.remainUnits) ? Convert.ToInt32(req.remainUnits) : 0;

                Dictionary<string, object> parms = new Dictionary<string, object>();
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_ID", Convert.ToString(req.lineNumber));
                parms.Add("PRIOR_AUTH_TOOTH_NUMBER_ID", toothNumber);
                parms.Add("PRIOR_AUTH_PROCEDURE_CODE_ID", req.procedureCode);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS", req.cavity1);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS", req.cavity2);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS", req.cavity3);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS", req.cavity4);
                parms.Add("PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS", req.cavity5);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE", req.surface1);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE", req.surface2);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE", req.surface3);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE", req.surface4);
                parms.Add("PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE", req.surface5);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS", req.reqUnits);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR", Convert.ToString(reqUnitsFee));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS", Convert.ToString(reqFDOS));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS", Convert.ToString(reqTDOS));
                parms.Add("PRIOR_AUTH_STATUS_ID", Convert.ToString(statusId));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC", Convert.ToString(req.procCodeDesc));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE", Convert.ToString(req.providerServiceNote));
                parms.Add("PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID", req.prosthesisNum);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM", req.serviceTrackingNo);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS", Convert.ToString(authUnits));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR", Convert.ToString(authDollars));
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS", string.Empty);
                parms.Add("PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS", string.Empty);
                parms.Add("PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS", Convert.ToString(remainingUnits));
                parms.Add("MedicaidID", Convert.ToString(req.MedicaidId));
                parms.Add("Line", 0);
                parms.Add("LAST_MODIFIED_USER", new Guid(CON.appAdminUserId));
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                parms.Add("CREATED_BY_USER", new Guid(CON.appAdminUserId));
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now);
                parms.Add("LINK_SECTIONS", new Guid());

                int value = PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("insertPRIOR_AUTH_DENTALSERVICE_DETAIL", parms);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataSet GetPriorAuthServiceDetail(int? statusId, string MedicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, MedicaidId, false));
                parameters.Add(SqlParms.CreateParameter("Status_Id", DbType.Int32, Convert.ToInt32(statusId), false));
                return DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_SERVICE_DETAIL", parameters, "Service_Details");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static DataSet GetPriorAuthServiceDetailById(int? Id, string MedicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("medicaidID", DbType.String, MedicaidId, false));
                parameters.Add(SqlParms.CreateParameter("Id", DbType.Int32, Convert.ToInt32(Id), false));
                return DataAccess.ExecuteStoredProcedure("usp_SelectPRIOR_AUTH_SERVICE_DETAILById", parameters, "Service_Details");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region Prior Auth Notes

        public static void SaveUpdateNote(PriorAuthNotesSave priorAuthNotes)
        {
            try
            {
                Dictionary<string, object> parms = new Dictionary<string, object>();

                if (priorAuthNotes.notesType == Convert.ToString(0))
                {

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_DENT_NOTES", DbType.String, priorAuthNotes.noteDesc, false));
                    parameters.Add(SqlParms.CreateParameter("LINK_SECTIONS", DbType.String, priorAuthNotes.linkId, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.appAdminUserId, false));
                    parameters.Add(SqlParms.CreateParameter("Operation", DbType.String, priorAuthNotes.operation, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("InsertUpdatePRIOR_AUTH_SUB_DENT_Notes", parameters, "PANotes");

                }
                else if (priorAuthNotes.notesType == Convert.ToString(1))
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_INST_NOTES", DbType.String, priorAuthNotes.noteDesc, false));
                    parameters.Add(SqlParms.CreateParameter("LINK_SECTIONS", DbType.String, priorAuthNotes.linkId, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.appAdminUserId, false));
                    parameters.Add(SqlParms.CreateParameter("Operation", DbType.String, priorAuthNotes.operation, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("InsertUpdatePRIOR_AUTH_SUB_INST_Notes", parameters, "PANotes");

                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("PRIOR_AUTH_PROF_NOTES", DbType.String, priorAuthNotes.noteDesc, false));
                    parameters.Add(SqlParms.CreateParameter("LINK_SECTIONS", DbType.String, priorAuthNotes.linkId, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, CON.appAdminUserId, false));
                    parameters.Add(SqlParms.CreateParameter("Operation", DbType.String, priorAuthNotes.operation, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("InsertUpdatePRIOR_AUTH_SUB_PROF_Notes", parameters, "PANotes");
                    //return PriorAuthHospitalController.InsertPriorAuthServiceDetailsData("InsertUpdatePRIOR_AUTH_SUB_PROF_Notes", parms);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static PDMSRestServices.Models.ProviderNotes GetNotes(string linkId, string paType)
        {
            PDMSRestServices.Models.ProviderNotes ProviderNotes = new PDMSRestServices.Models.ProviderNotes();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LINK_SECTIONS", DbType.Guid, new Guid(linkId), false));
                parameters.Add(SqlParms.CreateParameter("PAType", DbType.Int32, Convert.ToInt32(paType), false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_getPRIOR_AUTH_SUB__Notes", parameters, "PANotes");
                DataTable dt = ds.Tables[0];


                foreach (DataRow dr in dt.Rows)
                {
                    ProviderNotes.Note = dr["NOTES"] != null && dr["NOTES"] != DBNull.Value ? Convert.ToString(dr["NOTES"]) : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ProviderNotes;
        }

        public static NoteResult DeleteNotes(string linkId, int? paType)
        {
            NoteResult result = new NoteResult();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("LINK_SECTIONS", DbType.Guid, new Guid(linkId), false));
                parameters.Add(SqlParms.CreateParameter("PAType", DbType.Int64, paType, false));
                DataAccess.ExecuteStoredProcedure("usp_DeletePRIOR_AUTH_SUB_Notes", parameters);

                result.status = true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        #endregion

    }
}
