using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for Class1
/// </summary>

public class ApplicationFee
{

    #region Methods

    /// <summary>
    /// Creates the session.
    /// </summary>
    /// <param name="applicationIdentifier">The application identifier.</param>
    /// <returns>
    /// The session identifier for the session that was created.
    /// </returns>

    public string CreateSession(string applicationIdentifier)
    {
        //string sessionId = "";
        string walletIdentifier = "";

        string accessToken = "";
        
            accessToken = ApplicationCache.ApplicationFeeCBossAccessToken();
        

        //checked if wallet identifier exists for the registration if so get from PNM database
        string s = "";
        if (!string.IsNullOrEmpty(s))
        {
            walletIdentifier = s;
        }
        else
        {
            // Create a new wallet using "api/v1.0/wallet".
            walletIdentifier = CreateWallet(applicationIdentifier, accessToken);

            // Set the wallet identifier cookie.
        }
        


        
        

        const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        ServicePointManager.SecurityProtocol = Tls12;
        string CBOSSURL = AppSettings.Get("ApplicationFeeCBOSSUrl");
        string CBOSSsessionURL = CBOSSURL + "/api/v1.0/Session";
        WebRequest request = WebRequest.Create(CBOSSsessionURL);
        request.UseDefaultCredentials = true;
        request.PreAuthenticate = true;
        request.Credentials = CredentialCache.DefaultCredentials;
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);
        //request.ContentLength = data.Length;//http://UORADMMWEB01OHP.MAXCORP.MAXIMUS http://UORADMMWEB01OHP.MAXCORP.MAXIMUS/OH_PNM_DEV/Account/Login.aspx

        

        string hurl = AppSettings.Get("PNMHostedURL");

        


        var myDict = new Dictionary<string, string>() { { "ApplicationIdentifier", applicationIdentifier },
            { "HostedURL", hurl }, { "WalletIdentifier", walletIdentifier } };


        var myDictTransformed = myDict.Keys.AsEnumerable().Select(key => new MyDictEntry { name = key, value = myDict[key] });
        

        //var json = JsonConvert.SerializeObject(myDictTransformed);
        
        

        string jsonString = JsonConvert.SerializeObject(myDictTransformed);

        using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            

            streamWriter.Write(jsonString);
        }

        string responseContent = null;

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr99 = new StreamReader(stream))
                {
                    responseContent = sr99.ReadToEnd();
                }
            }
        }
        SessionIdentifier si = new SessionIdentifier();
        si = JsonConvert.DeserializeObject<SessionIdentifier>(responseContent);
        return si.identifier;
        //return responseContent;

        /*MessageBox.Show(responseContent);
        // Did the HTTP post to "api/v1.0/session" fail? If so, raise an exception to handle the error.
        if (httpResponseMessage.IsSuccessStatusCode == false)
        {
            throw new HttpRequestException(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }*/

        // Read the session identifier that was returned in the HTTP response message.
        //return sessionId;// httpResponseMessage.Content.ReadAsAsync<dynamic>().Result.identifier;
    }

    public string CreateAccessToken()
    {
        var sec = new SecretsManager(AppSettings.Get("SecretsRegion"));
        var result = sec.GetSuperSecretPassword(AppSettings.Get("CBOSSSecretDictionary"));
        result.Wait();

        string client_secret = result.Result["ApplicationFeeClientSecret"];

        if (string.IsNullOrEmpty(client_secret))
        {
            throw new System.Exception("client_secret empty");
        }
        string client_id = AppSettings.Get("ApplicationFeeCredentialingIdentifier");
        //string client_secret = AppSettings.Get("ApplicationFeeClientSecret");
        //byte[] data = Encoding.ASCII.GetBytes("client_id=c0a89de8-d70e-4381-b9db-e6099f923ce4&client_secret=(1o4Lcsq2s4(LFSgTB15)5WT6&grant_type=client_credentials");
        byte[] data = Encoding.ASCII.GetBytes("client_id="+ client_id +"&client_secret="+ client_secret +"&grant_type=client_credentials");

        const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        ServicePointManager.SecurityProtocol = Tls12;
        string CBOSSURL = AppSettings.Get("ApplicationFeeCBOSSUrl");
        string CBOSStokenURL = CBOSSURL + "/token";
        WebRequest request = WebRequest.Create(CBOSStokenURL);
        request.Method = "POST";
        request.ContentType = "application /x-www-form-urlencoded";
        
        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        
        

        string responseContent = "";

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr99 = new StreamReader(stream))
                {
                    responseContent = sr99.ReadToEnd();
                }
            }
        }
        AccessToken ac = new AccessToken();
        ac = JsonConvert.DeserializeObject<AccessToken>(responseContent);
        
         
        return ac.access_token;
    }
    /// <summary>
    /// Creates the wallet.
    /// </summary>
    /// <returns>
    /// The wallet identifier for the wallet that was created.
    /// </returns>
    /// <exception cref="HttpRequestException"></exception>
    protected string CreateWallet(string applicationIdentifier, string accessToken)
    {
        // Create a wallet by performing an HTTP post to "api/v1.0/wallet".
        
        const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        ServicePointManager.SecurityProtocol = Tls12;
        string CBOSSURL = AppSettings.Get("ApplicationFeeCBOSSUrl");
        string CBOSSwalletURL = CBOSSURL + "/api/v1.0/wallet";
        
        WebRequest request = WebRequest.Create(CBOSSwalletURL);
        request.UseDefaultCredentials = true;
        request.PreAuthenticate = true;
        request.Credentials = CredentialCache.DefaultCredentials;
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);
        //request.ContentLength = data.Length;

        using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = new JavaScriptSerializer().Serialize(new
            {
                //ApplicationIdentifier = applicationIdentifier
            });

            streamWriter.Write(json);
        }

        string responseContent = null;

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr99 = new StreamReader(stream))
                {
                    responseContent = sr99.ReadToEnd();
                }
            }
        }
        WalletIdentifier wi = new WalletIdentifier();
        wi = JsonConvert.DeserializeObject<WalletIdentifier>(responseContent);
        return wi.identifier;

        // Read the wallet identifier that was returned in the HTTP response message.
        //return httpResponseMessage.Content.ReadAsAsync<dynamic>().Result.identifier;
    }
    
    /// <summary>
    /// Performs the wallet payment.
    /// </summary>
    /// <param name="demonstrationModel">The demonstration model.</param>
    /// <exception cref="HttpRequestException"></exception>
    public ApplicationFeeModel PerformWalletPayment(string hdnAccountIdentifier, string hdnAccountNumber, string hdnAccountType, string hdnAccountFirstName, string hdnLastName, string thisProviderFee, string businessName = "", string regId = "")
    {
        string OriginatorIdentifier = AppSettings.Get("ApplicationFeeOriginatorIdentifier");
        var IdempotentIdentifier = Guid.NewGuid().ToString();
        string client_id = AppSettings.Get("ApplicationFeeClientIdentifier");


        // The account identifier, amount, and originator identifier must be provided in the model.
        dynamic model = new
        {



            account = new
            {
                bankAccountNumber = "",
                bankCheckNumber = "",
                bankRoutingNumber = "",
                billingAddress = new
                {
                    addressLine1 = "",
                    addressLine2 = "",
                    city = "",
                    country = "",
                    //emailAddress = "test@test.com",
                    firstName = "",
                    lastOrBusinessName = "",
                    middleName = "",
                    phoneNumber = "",
                    state = "",
                    zip = ""
                },
                cashReceiptNumber = "",
                creditCardCVVCode = "",
                creditCardExpirationMonth = "",
                creditCardExpirationYear = "",
                creditCardNumber = "",
                emvDeviceIdentifier = "",
                emvDeviceMethod = "Credit",
                identifier = hdnAccountIdentifier,
                isBankAccountCorporate = false,
                isBankAccountSavings = false,
                isDebitCard = false,
                ownerBusinessTaxID = "",
                ownerDriverLicense = "",
                ownerDriverLicenseState = "",
                ownerMilitaryID = "",
                ownerSocialSecurityNumber = "",
                paymentMethod = "Credit",
                swipeKeyName = "",
                swipeKeySerialNumber = "",
                swipeTrack1 = "",
                swipeTrack2 = ""
            },
            behavior = new
            {
                rollbackOnFailure = true,
                rollbackOnSystemError = true
            },
            payments = new List<dynamic>
            {
                new{

                                    account = new
                                    {
                                    bankAccountNumber = "",
                                    bankCheckNumber = "",
                                    bankRoutingNumber = "",
                                    billingAddress = new {
                                          addressLine1 = "",
                                          addressLine2 = "",
                                          city = "",
                                          country = "",
                                          //emailAddress = "test@test.com",
                                          firstName = "",
                                          lastOrBusinessName = "",
                                          middleName = "",
                                          phoneNumber = "",
                                          state = "",
                                          zip = ""
                                        },
                                cashReceiptNumber = "",
                                creditCardCVVCode = "",
                                creditCardExpirationMonth = "",
                                creditCardExpirationYear = "",
                                creditCardNumber = "",
                                emvDeviceIdentifier = "",
                                emvDeviceMethod = "",
                                identifier = hdnAccountIdentifier,
                                isBankAccountCorporate = false,
                                isBankAccountSavings = false,
                                isDebitCard = false,
                                ownerBusinessTaxID = "",
                                ownerDriverLicense = "",
                                ownerDriverLicenseState = "",
                                ownerMilitaryID = "",
                                ownerSocialSecurityNumber = "",
                                paymentMethod = "Credit",
                                swipeKeyName = "",
                                swipeKeySerialNumber = "",
                                swipeTrack1 = "",
                                swipeTrack2 = ""
                            },
                            
            
                amount = thisProviderFee,
                amountLine1 = 0,
                amountLine2 = 0,
                authorizedDate = "",
                clientIdentifier = client_id,
                comment1 = regId,
                comment2 = businessName,
                customerBrowser = "",
                customerHostName = "",
                customerIPAddress = "",
                idempotentIdentifier = IdempotentIdentifier,
                items = new List<dynamic>
                {
                    new
                    {
                        description = "",
                        price = thisProviderFee,
                        quantity = 1
                    }
                },
                originatorIdentifier = OriginatorIdentifier,
                requestIdentifier = "",
                source = "Internet",
                transactionNumber = "",
                username = ""
            }
          }

        };

            

        // Create a transaction by performing an HTTP post to "api/v1.0/transaction".
        const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
        ServicePointManager.SecurityProtocol = Tls12;
        string CBOSSURL = AppSettings.Get("ApplicationFeeCBOSSUrl");
        string CBOSStransactionURL = CBOSSURL + "/api/v1.0/transaction";
        WebRequest request = WebRequest.Create(CBOSStransactionURL);
        request.UseDefaultCredentials = true;
        request.PreAuthenticate = true;
        request.Credentials = CredentialCache.DefaultCredentials;
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + ApplicationCache.ApplicationFeeCBossAccessToken());

        string jsonString = JsonConvert.SerializeObject(model);

        using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
        {


            streamWriter.Write(jsonString);
        }

        string responseContent = null;

        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr99 = new StreamReader(stream))
                {
                    responseContent = sr99.ReadToEnd();
                }
            }
        }
        
        string transID = "";
        ApplicationFeeModel app = new ApplicationFeeModel();
        app.statusCode = JsonConvert.DeserializeObject<dynamic>(responseContent).results[0].statusCode;
        if (JsonConvert.DeserializeObject<dynamic>(responseContent).results[0].statusCode == "0000")
        {
            transID = JsonConvert.DeserializeObject<dynamic>(responseContent).results[0].transactionNumber;
            app.transactionNumber = JsonConvert.DeserializeObject<dynamic>(responseContent).results[0].transactionNumber;
        }
        else
        {
            app.transactionNumber = "";
        }
        app.transactionStatus = JsonConvert.DeserializeObject<dynamic>(responseContent).results[0].transactionStatus;

        return app;

        // Populate the receipt part of the demonstration model.
        
    }
    
    #endregion Methods
}
public class AccessToken
{
    public string access_token;
    public string toke_type;
    public int expires_in;
}

public class WalletIdentifier
{
    public string identifier;
}

class MyDictEntry
{
    public string name { get; set; }
    public string value { get; set; }
}

public class SessionIdentifier
{
    public string identifier;
}
