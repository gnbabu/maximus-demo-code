using Corp.Core.Libraries.Proxy;
using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for ElicenseVerificationHelper
/// </summary>
public static class ElicenseVerificationHelper
{
     static ElicenseVerificationHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

  public static Dictionary<string,string> ActiveElicenseStatus()
    {
        Dictionary<string, string> licneseStats = new Dictionary<string, string>();
        licneseStats.Add("Board Action","Active");
        licneseStats.Add("Probation","Active");
        licneseStats.Add("Restricted", "Active");
        licneseStats.Add("Active", "Active");
        licneseStats.Add("Escrow", "Active");
        licneseStats.Add("Renewing", "Active");
        licneseStats.Add("Additional Endorsement Pending", "Active");
        licneseStats.Add("Pending Receipt of Plan", "Active");
        licneseStats.Add("Plan Under Review", "Active");
        licneseStats.Add("", "Active"); 

        return licneseStats;
    }
  public static Object GetLicenseVerificationResult(ELicenseVerificationRequest eRequest, int regID,string licenseTypeId, DataTable dtLicense, int RegLicensureID = 0, string licenseNumber = "")
    {
        ELicenseVerificationResponse eResponse = new ELicenseVerificationResponse();
       
        List <Object> lstResponseObject = new List<object>();
        string strResponse = string.Empty;
        string strRequest = string.Empty;
        string contents = string.Empty;
        try
        {
            if (eRequest != null)
            {
                string Uri = AppSettings.Get("eLicenseVerificationWebserviceURL");                
                string testFlag= AppSettings.Get("eLicenseVerificationTestEnabled");
                strRequest = JsonConvert.SerializeObject(eRequest);
              


                if (!string.IsNullOrEmpty(testFlag) && testFlag == "true")
                {
                    string testFile = AppSettings.Get("eLicenseVerificationTestResponseFile");
                    string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                   
                    string fileComplete = templateActualPath + testFile;
                    
                    FileTransferServiceClient client = new FileTransferServiceClient();
                    contents = client.GetFileContents(fileComplete);
                                                   
                   
                }
                else
                {
                    var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
                    var result = sec.GetSuperSecretPassword(AppSettings.Get("eLICSecretDictionary"));
                    result.Wait();

                    string passcode = result.Result["eLicenseVerificationWebservicePassCode"];

                    if (string.IsNullOrEmpty(passcode))
                    {
                        throw new Exception("client_secret empty");
                    }
                    //Make service call
                    //To do AFTER We Receive Service information from State.
                    string urllicense = AppSettings.Get("eLicenseVerificationWebserviceURL");
                    string user = AppSettings.Get("eLicenseVerificationWebserviceUser");
                    //string passcode = AppSettings.Get("eLicenseVerificationWebservicePassCode");
                    Uri apiUrl = new Uri(urllicense);
                    WebRequest erequet = HttpWebRequest.Create(urllicense);
                    HttpWebRequest eVerificationRequest = (HttpWebRequest)erequet;
                    eVerificationRequest.ContentType = "application/json";
                    eVerificationRequest.Method = "POST";
                    NetworkCredential ncredential = new NetworkCredential(user, passcode);
                    CredentialCache ecredentialCache = new CredentialCache();
                    ecredentialCache.Add(apiUrl, "Basic", ncredential);
                    eVerificationRequest.PreAuthenticate = true;
                    eVerificationRequest.Credentials = ecredentialCache;
                   

                    using (StreamWriter stream = new StreamWriter(eVerificationRequest.GetRequestStream()))
                    {
                        stream.Write(strRequest);
                    }

                   
                    //getresponse
                    using (WebResponse rsElicense = eVerificationRequest.GetResponse())
                    {
                        HttpWebResponse rsHttpLicense = (HttpWebResponse)rsElicense;

                        HttpStatusCode statuscode = rsHttpLicense.StatusCode;

                        using (Stream stream = rsHttpLicense.GetResponseStream())
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                contents = sr.ReadToEnd();
                            }
                        }

                    }
                                      
                  
                }

                if (!string.IsNullOrEmpty(contents))
                {
                    if (contents.Contains("No license found"))
                    {
                        //No License Found
                        var response = JsonConvert.DeserializeObject(contents);
                        strResponse = response.ToString();
                        lstResponseObject.Add(response);
                    }
                    else
                    {
                        var response = JsonConvert.DeserializeObject<List<ELicenseVerificationResponse>>(contents);
                        strResponse = JsonConvert.SerializeObject(response).ToString();
                        if(RegLicensureID > 0 && !string.IsNullOrEmpty(licenseNumber))
                        {
                            foreach(ELicenseVerificationResponse li in response)
                            {
                                if (li.license_number.ToString().ToLower().Trim() == licenseNumber.ToLower().Trim())
                                {
                                    lstResponseObject.Add(li);
                                }
                            }
                        }
                        else
                        {
                            // OHPNM-19201 get the license that is already not on the provider record
                            if (dtLicense.Rows.Count > 0)
                            {
                                foreach (ELicenseVerificationResponse li in response)
                                {
                                    if (dtLicense.AsEnumerable().Where(r => r.Field<string>("LICENSE_NUMBER").ToLower().Trim() == li.license_number.ToString().ToLower().Trim()).Count() == 0)
                                    {
                                        lstResponseObject.Add(li);
                                        break;
                                    }
                                }

                                if(lstResponseObject.Count == 0)
                                {
                                    lstResponseObject.Add(response[0]);
                                }
                            }
                            else
                            {
                                lstResponseObject.Add(response[0]);
                            }

                        }
                        

                    }
                }
            }
        }
        catch(Exception ex)
        {


            lstResponseObject = null;
            strResponse = "Content Response :" +contents+ MAXIMUS.Core.Libraries.CoreException.FormatException(ex) + " Current Base Directory:" + System.AppDomain.CurrentDomain.BaseDirectory;

            //Need to log the Error for tracking purpose later
        }
        
        //Before Returning store Erequest n Response
        InsertEverificationRequestResult(strRequest, strResponse, eRequest.last_name, eRequest.dob, licenseTypeId, eRequest.last_4_ssn, regID);
        return lstResponseObject[0];
    }

    public static int InsertEverificationRequestResult(string request,string response, string lastName, string DOB, string licenseTypeId, string last4SSN, int regID)
    {
        int regLicenseVerificationID = 0;

        
        try
        {
            
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("LAST_NAME", lastName);
            parms.Add("LAST4SSN", last4SSN);
            parms.Add("DOB", DOB);
            parms.Add("LICENSE_TYPE_ID", licenseTypeId);
            parms.Add("REG_ID", regID.ToString());
            parms.Add("ELICENSE_REQUEST", request);
            parms.Add("ELICENSE_RESPONSE", response);
            parms.Add("ELICENSE_VERIFIED_DATE", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());           
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            regLicenseVerificationID = psc.InsertRegistrationData(regID, "ELICENSE_VERIFICATION", parms);

            
        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
        }
        return regLicenseVerificationID;

    }
    public static int  UpdateLicenseEverificationRequestResult(string lastName, string DOB, string licenseTypeId, string last4SSN, int regID,int reg_licensure_Id)
    {
        int regLicenseVerificationID = 0;


        try
        {

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("LAST_NAME", lastName);
            parms.Add("LAST4SSN", last4SSN);
            parms.Add("DOB", DOB);
            parms.Add("LICENSE_TYPE_ID", licenseTypeId);
            parms.Add("REG_ID", regID.ToString());
            parms.Add("REG_LICENSURE_ID", reg_licensure_Id.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            regLicenseVerificationID = psc.UpdateRegistrationData(regID, "ELICENSE_VERIFICATION", parms);


        }
        catch (Exception ex)
        {

            Console.Write(ex.Message);
        }
        return regLicenseVerificationID;

    }
}