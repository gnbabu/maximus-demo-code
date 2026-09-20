using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;
namespace MAXIMUS.Presentation.PDMS
{
   public  class ProfessionalLicensePresenter: PresenterBase, IPresenter<IProfessionalLicenseView, ProfessionalLicense>
    {

        private IProfessionalLicenseView view = null;
        private Logging log = new Logging();
        #region svc
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }
        #endregion

        public ProfessionalLicensePresenter(IProfessionalLicenseView view)
        {
            Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new ProfessionalLicense();
        }
        #endregion
        public int SaveRegprofessionalLicenseInformation(ProfessionalLicense lic)
        {

                int licenseId = 0;

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", lic.RegID.ToString());
            try
            {


                int? RegAddressId  = SaveLicenseAddress(lic);


                if (!string.IsNullOrWhiteSpace(lic.LicenseNumber))
                {

                    parms.Add("LICENSE_NUMBER", lic.LicenseNumber);
                }
                if (!string.IsNullOrWhiteSpace(lic.LicenseType))
                {

                    parms.Add("LICENSE_TYPE_ID", lic.LicenseType);
                }
             
               
                if (!string.IsNullOrWhiteSpace(lic.LicenseEffectiveDate.ToString()) && lic.LicenseEffectiveDate !=DateTime.MinValue)
                {
                    parms.Add("LICENSE_EFF_DATE", lic.LicenseEffectiveDate.ToString());
                }
                else
                {
                    parms.Add("LICENSE_EFF_DATE", null);
                }
               
                if (!string.IsNullOrWhiteSpace(lic.LicenseExpirationDate.ToString()) && lic.LicenseExpirationDate != DateTime.MinValue)
                {
                    parms.Add("LICENSE_END_DATE", lic.LicenseExpirationDate.ToString());
                }
                else
                {
                    parms.Add("LICENSE_END_DATE", null);
                }
                if (!string.IsNullOrWhiteSpace(lic.LicenseState))
                {
                    parms.Add("LICENSE_STATE", lic.LicenseState);
                }
                
                parms.Add("LICENSE_BOARD_NAME", lic.LicenseBoardName);
                

                if (!string.IsNullOrWhiteSpace(lic.LicenseStatus))
                {
                    parms.Add("LICENSE_STATUS", lic.LicenseStatus);
                }
                if (lic.ELicenseVerified)
                {
                    parms.Add("LICENSE_SUBSTATUS", lic.LicenseSubStatus);
                }

                parms.Add("ELICENSE_VERIFIED", lic.ELicenseVerified.ToString());
             

                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", lic.UserID.ToString());            
                parms.Add("REG_ADDRESSID", RegAddressId.ToString());         

                bool isEdit = lic.RegLicensureID>0 ? true : false;


                //Add Address First

              

                


                if (isEdit)
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_LICENSURE_ID", lic.RegLicensureID.ToString());
                    svc.UpdateRegistrationData(lic.RegID, "LICENSEcustom", parms);
                   // we are only getting reg id back from UpdateRegistrationData
                }
                else
                {
                    parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User",lic.Created_By_User.ToString());
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Inserted.ToString());
                    lic.RegLicensureID = svc.InsertRegistrationData(lic.RegID, "LICENSEcustom", parms);
                }
                licenseId = lic.RegLicensureID;
                //Now Update Specialty Focus

                if (lic.RegLicensureID > 0)
                {
                    //Check if the exits then delete and insert back again
                    DataSet ds = GetLicenseSpecialtyFocusInformation(lic.RegLicensureID);                    
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        //Delete 
                        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
                        {
                            psc.DeleteRegistrationData("ENDORSEMENT_SPECIALTY_FOCUS", "REG_LICENSURE_ID", lic.RegLicensureID);
                        }
                    }


                    Dictionary<string, string> parmsFocus = new Dictionary<string, string>();
                    foreach (SpecialtyFocusCertificate sp in lic.SpecialtyFocusCertificates)
                    {
                        parmsFocus.Clear();
                        bool emptyEndorsement = false;
                        bool checkDuplicateEndorsement = false;
                        DataSet dsEndor = GetLicenseSpecialtyFocusInformation(lic.RegLicensureID);
                        //DataTable dt = dsEndor.Tables[0];
                        if ( (sp.EndorsementSpecialty == string.Empty) &&
                            (sp.CertifyingOrganization == string.Empty) &&
                            (sp.EndorsementFocus == string.Empty))
                        {
                            emptyEndorsement = true;
                        }
                        
                        foreach (DataRow dr in dsEndor.Tables[0].Rows)
                        {
                            if((dr["ENDORSEMENT_NUMBER"].ToString() == sp.EndorsementNumber)
                                && (dr["ENDORSEMENT_SPECIALITY"].ToString() == sp.EndorsementSpecialty)
                                && (dr["ENDORSEMENT_FOCUS"].ToString() == sp.EndorsementFocus)
                                && (dr["CERTIFYING_ORGANIZATION"].ToString() == sp.CertifyingOrganization))
                            {
                                checkDuplicateEndorsement = true;
                            }
                        }
                        #region Add Endorsement to DB
                        //If Focus, certification, specialty status is empty, then we should n't add a record in endorsement table for license child table
                        //As it's causing transaction to Fail as an invalid record.

                        if (!emptyEndorsement && !checkDuplicateEndorsement)
                        {
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementSpecialty))
                            {
                                parmsFocus.Add("ENDORSEMENT_SPECIALITY", sp.EndorsementSpecialty);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementFocus))
                            {
                                parmsFocus.Add("ENDORSEMENT_FOCUS", sp.EndorsementFocus);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.CertifyingOrganization))
                            {
                                parmsFocus.Add("CERTIFYING_ORGANIZATION ", sp.CertifyingOrganization);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.CertificateDate.ToString()) && sp.CertificateDate != DateTime.MinValue)
                            {
                                parmsFocus.Add("CERTIFICATE_DATE", sp.CertificateDate.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("CERTIFICATE_DATE", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.CertificateExpirationDate.ToString()) && sp.CertificateExpirationDate != DateTime.MinValue)
                            {
                                parmsFocus.Add("CERTIFICATE_EXPIRATION", sp.CertificateExpirationDate.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("CERTIFICATE_EXPIRATION", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementStatus))
                            {
                                parmsFocus.Add("ENDORSEMENT_STATUS", sp.EndorsementStatus.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_STATUS", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementSubStatus))
                            {
                                parmsFocus.Add("ENDORSEMENT_SUB_STATUS", sp.EndorsementSubStatus.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_SUB_STATUS", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementNumber))
                            {
                                parmsFocus.Add("ENDORSEMENT_NUMBER", sp.EndorsementNumber.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_NUMBER", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementBoardAction))
                            {
                                parmsFocus.Add("ENDORSEMENT_BOARDACTION", sp.EndorsementBoardAction.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_BOARDACTION", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementIssuedDate.ToString()) && sp.EndorsementIssuedDate != DateTime.MinValue)
                            {
                                parmsFocus.Add("ENDORSEMENT_ISSUEDATE", sp.EndorsementIssuedDate.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_ISSUEDATE", null);
                            }
                            if (!string.IsNullOrWhiteSpace(sp.EndorsementExpirationDate.ToString()) && sp.EndorsementExpirationDate != DateTime.MinValue)
                            {
                                parmsFocus.Add("ENDORSEMENT_EXPIRATIONDATE", sp.EndorsementExpirationDate.ToString());
                            }
                            else
                            {
                                parmsFocus.Add("ENDORSEMENT_EXPIRATIONDATE", null);
                            }

                            parmsFocus.Add("REG_LICENSURE_ID", lic.RegLicensureID.ToString());
                            parmsFocus.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parmsFocus.Add("LAST_MODIFIED_USER", lic.UserID.ToString());

                            svc.InsertRegistrationData(lic.RegID, "ENDORSEMENT_SPECIALTY_FOCUS", parmsFocus);

                        }

                        #endregion
                    }
                }


            }
            catch(Exception ex)
            {
                log.CreateLogEntry("Failed to Save Reg Professional License Information"
                                     + " Exception Message " + ex.Message + " Exception Stack = "
                                     + ex.StackTrace, Logging.LogPriority.Error);
            }
            return licenseId;
        }


        public DataSet GetLicenseSpecialtyFocusInformation(int regLicensureId)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            parameters.Add("REG_LICENSURE_ID", regLicensureId.ToString());
            ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ENDORSEMENT_SPECIALTY_FOCUS", parameters);
          
            return ds;
        }

        public string GetLicenseTypeIDByAbbrevation(string LicenseCodeAbbrevation)
        {
            string LicenseTypeId = string.Empty;
            if(!string.IsNullOrEmpty(LicenseCodeAbbrevation))
            {
                LicenseTypeId =svc.SelectLicenseTypeIdByAbbrev(LicenseCodeAbbrevation);
            }
            return LicenseTypeId;
        }

        public DataRow GetLicenseAddress(int regID,int regAddressId)
        {
            DataRow drAddress = null;
            try
            {                
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectProviderAddressInfo(regID, CON.AddressType.ProfessionalLicenseAddress);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    drAddress = ds.Tables[0].Select("REG_ADDRESS_ID=" + regAddressId)[0];

                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to Read Reg Professional License Address Information: "
                                     + " Exception Message " + ex.Message + " Exception Stack = "
                                     + ex.StackTrace, Logging.LogPriority.Error);
            }

            return drAddress;
        }
        private int? SaveLicenseAddress(ProfessionalLicense lic)
        {
            int? RegAddressId = 0;
            try
            {
                Dictionary<string, string> parmsAddress = new Dictionary<string, string>();
                bool isEditLiceseAddress = lic.RegAddressID > 0 ? true : false;
                //seems like there namespaces are different for address related one s so can n't re-use
            
                    parmsAddress.Add("REG_ID", lic.RegID.ToString());
                    parmsAddress.Add("ADDRESS_TYPE_ID", CON.AddressType.ProfessionalLicenseAddress.ToString());
                    parmsAddress.Add("PRACTICE_NAME", string.Empty);

                    parmsAddress.Add("ADDRESS1", lic.Address1);
                    parmsAddress.Add("ADDRESS2", lic.Address2);

                    parmsAddress.Add("FIRST_NAME", string.Empty);
                    parmsAddress.Add("MIDDLE_NAME", string.Empty);
                    parmsAddress.Add("LAST_NAME", string.Empty);
                    parmsAddress.Add("SUFFIX", string.Empty);
                    parmsAddress.Add("CITY",lic.City);
                    parmsAddress.Add("STATE", lic.State);
                    parmsAddress.Add("ZIP", lic.Zip);
                   
                    parmsAddress.Add("EXT_ZIP", string.Empty);
                    parmsAddress.Add("COUNTY", lic.County);
                    parmsAddress.Add("COUNTYNAME", lic.CountyName);

                    parmsAddress.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parmsAddress.Add("LAST_MODIFIED_USER", lic.UserID.ToString());

                    if (!isEditLiceseAddress)
                    {
                        if(!string.IsNullOrEmpty(lic.Address1))
                        {
                            parmsAddress.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                            parmsAddress.Add("CREATED_BY_USER", lic.UserID.ToString());
                            RegAddressId = svc.InsertRegistrationData(lic.RegID, "ADDRESS", parmsAddress);
                        }
                        else
                            RegAddressId = null;
                    }
                    else
                    {
                        parmsAddress.Add("REG_ADDRESS_ID", lic.RegAddressID.ToString());
                        RegAddressId = lic.RegAddressID;
                        svc.UpdateRegistrationData(lic.RegAddressID, "ADDRESS", parmsAddress);
                    }


            }
            catch(Exception ex)
            {
                log.CreateLogEntry("Failed to Save License Address "
                                     + " Exception Message " + ex.Message + " Exception Stack = "
                                     + ex.StackTrace, Logging.LogPriority.Error);

            }

            return RegAddressId;
        }
    }
}
