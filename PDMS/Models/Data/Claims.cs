using MAXIMUS.Core.Libraries;
using System;
using System.Net;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Corp.Core.Libraries;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Models.Data
{
    public class Claims
    {
        public static PDMSService.PDMSServiceClient _svc;

        public static PDMSService.PDMSServiceClient svc
        {
            get { return _svc ?? (_svc = new PDMSService.PDMSServiceClient()); }
        }

        public static string NPITextChangedTest(string _txtNPI, string RefMedId)
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
                        DataTable dt = GetData(_txtNPI.Trim(), "", "", "");
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
        public static void NPITextChanged(string _txtNPI, string _txtMedicaidId, TextBox _txtproviderName, Label _lblFirstName, Label _lblLastName, Label _lblMi, Label errorLabel, string providerType = "")
        {
            errorLabel.Text = "";
            if (!string.IsNullOrEmpty(_txtNPI))
            {
                if (_txtNPI.Trim().Length == 10)
                {
                    var isValid = ValidProviderNPI(_txtNPI.Trim());
                    if (!isValid)
                    {
                        errorLabel.Text = string.Concat(providerType, "NPI is not found in the system");
                        _lblLastName.Text = "";
                        _lblFirstName.Text = "";
                        return;
                    }
                    else
                    {
                        DataTable dt = GetData(_txtNPI.Trim(), _txtMedicaidId.Trim(), "", "");
                        if (dt.Rows.Count > 0)
                        {

                            dt = dt.Select("NPI <> ''").CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                if (_txtproviderName != null)
                                {
                                    _txtproviderName.Text = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                                }

                                if (_lblFirstName != null)
                                {
                                    _lblFirstName.Text = row["FIRST_NAME"].ToString();
                                }
                                if (_lblLastName != null)
                                {
                                    _lblLastName.Text = row["LAST_OR_BUSINESS_NAME"].ToString();
                                }

                            }

                        }
                    }
                }
                else
                {
                    _lblLastName.Text = string.Empty;
                    _lblFirstName.Text = string.Empty;
                    errorLabel.Text = "10-digit number is required";
                }
            }
        }

        public static void NPITextChanged(string _txtNPI, Label _lblMedicaidId, TextBox _txtproviderName, Label _lblFirstName, Label _lblLastName, Label _lblMi, Label errorLabel,string providerType="")
        {
            errorLabel.Text = "";
            if (!string.IsNullOrEmpty(_txtNPI))
            {
                if (_txtNPI.Trim().Length == 10)
                {
                    var isValid = ValidProviderNPI(_txtNPI.Trim());
                    if (!isValid)
                    {
                        errorLabel.Text = string.Concat(providerType, "NPI is not found in the system");
                        if (_lblMedicaidId != null)
                        {
                            _lblMedicaidId.Text = "";
                        }
                        _lblLastName.Text = "";
                        _lblFirstName.Text = "";
                        return;
                    }
                    else
                    {
                        DataTable dt = GetData(_txtNPI.Trim(), "", "", "");
                        if (dt.Rows.Count >0)
                        {

                            dt = dt.Select("NPI <> ''").CopyToDataTable();
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                if (_lblMedicaidId != null)
                                {
                                    _lblMedicaidId.Text = row["MEDICAID_ID"].ToString();
                                }

                                if (_txtproviderName != null)
                                {
                                    _txtproviderName.Text = row["FIRST_NAME"].ToString() + " " + row["LAST_OR_BUSINESS_NAME"].ToString();
                                }

                                if (_lblFirstName != null)
                                {
                                    _lblFirstName.Text = row["FIRST_NAME"].ToString();
                                }
                                if (_lblLastName != null)
                                {
                                    _lblLastName.Text = row["LAST_OR_BUSINESS_NAME"].ToString();
                                }

                            }

                        }
                    }
                }
                else
                {
                    if (_lblMedicaidId != null)
                    {
                        _lblMedicaidId.Text = string.Empty;
                    }
                    _lblLastName.Text = string.Empty;
                    _lblFirstName.Text = string.Empty;
                    errorLabel.Text= "10-digit number is required";
                }
            }
            else
            {
                //_lblMedicaidId.Text = string.Empty;
                //_txtproviderName.Text = string.Empty;
            }
        }

        public static bool ValidProviderNPI(string npi)
        {
            try
            {
                using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
                {
                    return psc.VerifyProviderNPI(Convert.ToInt64(npi));
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
        {
            DataTable dt = null;
            try
            {
                using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
                {
                    var ds = psc.SearchClaimProviderNPI(npi, medicaidid, lastName, firstName);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception)
            {
                return dt;
            }
            return dt;
        }

        public static void SaveClaimsNPIDetails(string _txtNPI, string _txtMedicaidId, string _txtFirstName, string _txtLastName, string tableName, int is_Primary, string claimsPanelName, int ClaimID)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>(); 
            DataSet dsIndividualClaims = svc.SelectDentalClaimData(ClaimID, "claims_provider_information", claimsPanelName, is_Primary);
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
                    parms.Add("Last_Modified_User", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                    svc.UpdatePanelsData(tableName, parms);
                }
                else
                {
                    parms.Add("NPI", _txtNPI.ToString());
                    parms.Add("Medicaid_ID", _txtMedicaidId.ToString());
                    parms.Add("First_Name", _txtFirstName.ToString());
                    parms.Add("Last_Name", _txtLastName.ToString());
                    parms.Add("Claim_ID", ClaimID.ToString());
                    parms.Add("Is_Primary", is_Primary.ToString());
                    parms.Add("Created_By_User", Methods.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Claims_Panel_Name", claimsPanelName.ToString());
                    svc.InsertPanelsData(tableName, parms);
                }
            }
        }

        public static DataSet LoadProviderInformation(string medicaidNumber)
        {
            DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
            return ds;
        }
    }
}
