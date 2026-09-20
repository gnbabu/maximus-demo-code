using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.Caching;



/// <summary>
/// ApplicationCache global methods
/// </summary>
public class ApplicationCache
{
    //TODO: EDV Take this out use and dictionarycache that is in appsettings
    private ApplicationCache()
    {
    }

    // Expire the cache item one week from today
    private static DateTime cacheExpirationDateTime
    {
        get { return DateTime.Today.AddDays(1); }
    }

    // Store the question types in the Application Cache
    public static DataTable QuestionTypes()
    {
        if (System.Web.HttpContext.Current.Cache["dtQuestionTypes"] == null)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataTable dtQuestionTypes = psc.SelectQuestionType(string.Empty).Tables[0];
            dtQuestionTypes.PrimaryKey = new DataColumn[] { dtQuestionTypes.Columns[0] };
            // Add PrimaryKey for good measure. If filter on the key it will be quicker.
            System.Web.HttpContext.Current.Cache.Add("dtQuestionTypes", dtQuestionTypes, null, cacheExpirationDateTime,
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
        }

        return (DataTable)System.Web.HttpContext.Current.Cache["dtQuestionTypes"];
    }

    // Store the state codes in the Application Cache
    public static DataTable StateAbbreviations()
    {
        // TODO: EDV Put in in our cache instead of HTTPContext cache
        if (System.Web.HttpContext.Current.Cache["dtStateCodes"] == null)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataTable dtStateCodes = psc.GetStates().Tables[0];
            dtStateCodes.PrimaryKey = new DataColumn[] { dtStateCodes.Columns[0] };
            // Add PrimaryKey for good measure. If filter on the key it will be quicker.
            System.Web.HttpContext.Current.Cache.Add("dtStateCodes", dtStateCodes, null, cacheExpirationDateTime,
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
        }

        return (DataTable)System.Web.HttpContext.Current.Cache["dtStateCodes"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static DataTable SelectCDSRequireStates()
    {
        // TODO: EDV Put in in our cache instead of HTTPContext cache
        if (HttpContext.Current.Cache["cdsStatesTable"] == null)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataTable cdsStatesTable = psc.SelectCDSRequireStates().Tables[0];
            HttpContext.Current.Cache.Add("cdsStatesTable", cdsStatesTable, null, cacheExpirationDateTime,
                Cache.NoSlidingExpiration, CacheItemPriority.BelowNormal, null);
        }

        return (DataTable)HttpContext.Current.Cache["cdsStatesTable"];
    }


    // Store the note types in the Application Cache
    public static DataTable ProviderNoteTypes()
    {
        // TODO: EDV Put in in our cache instead of HTTPContext cache
        if (System.Web.HttpContext.Current.Cache["dtNoteTypes"] == null)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataTable dtNoteTypes = psc.GetProviderNoteTypes().Tables[0];
            dtNoteTypes.PrimaryKey = new DataColumn[] { dtNoteTypes.Columns[0] };
            // Add PrimaryKey for good measure. If filter on the key it will be quicker.
            System.Web.HttpContext.Current.Cache.Add("dtNoteTypes", dtNoteTypes, null, cacheExpirationDateTime,
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
        }

        return (DataTable)System.Web.HttpContext.Current.Cache["dtNoteTypes"];
    }

    public static string ApplicationFeeCBossAccessToken()
    {

        
        if (System.Web.HttpContext.Current.Cache["ApplicationFeeCBossAccessTok"] == null)
        {
            ApplicationFee appFee = new ApplicationFee();
            string accessToken = appFee.CreateAccessToken();
            System.Web.HttpContext.Current.Cache.Add("ApplicationFeeCBossAccessTok", accessToken, null, DateTime.Now.AddMinutes(59),
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);    
        }
        
        return (string)System.Web.HttpContext.Current.Cache["ApplicationFeeCBossAccessTok"];
    }
    public static string AMAAccessToken(string username)
    {

        try {
        if (System.Web.HttpContext.Current.Cache["AMAAccessTok"] == null)
        {
            CredentialHelper appFee = new CredentialHelper();
            string accessToken = appFee.createToken(username);
            if(!string.IsNullOrEmpty(accessToken))
                {
            System.Web.HttpContext.Current.Cache.Add("AMAAccessTok", accessToken, null, DateTime.Now.AddMinutes(59),
                System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
                    SessionVarRetriever.APIToken = (string)System.Web.HttpContext.Current.Cache["AMAAccessTok"];
                }
            }

        return (string)System.Web.HttpContext.Current.Cache["AMAAccessTok"];
        }
        catch(System.Exception  ex)
        {
            throw ex;
        }
    }

    public static CredentialHelper.APIToken RestAPIAccessToken()
    {
        CredentialHelper api = new CredentialHelper();
        CredentialHelper.APIToken accessToken = api.CreateAPIAccessToken();
        SessionVarRetriever.RestAPIToken = accessToken.AccessToken;
        SessionVarRetriever.RefreshAPIToken = accessToken.RefreshToken;
        return accessToken;
    }
    public static CredentialHelper.APIToken RestAPIAccessToken(string username)
    {
        CredentialHelper api = new CredentialHelper();
        CredentialHelper.APIToken accessToken = api.CreateAPIAccessToken(username);
        SessionVarRetriever.RestAPIToken = accessToken.AccessToken;
        SessionVarRetriever.RefreshAPIToken = accessToken.RefreshToken;
        return accessToken;
    }

    public static DataSet StateCounties()
    {
		if (HttpContext.Current.Cache["dsStateCounties"] == null)
	    {
		    var psc = new PDMSService.PDMSServiceClient();
		    var dsStateCounties = psc.SelectCountiesByStateAbbreviation(AppSettings.Get("StateCode"));
		    HttpContext.Current.Cache.Add("dsStateCounties", dsStateCounties, null, cacheExpirationDateTime,
			    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.BelowNormal, null);
	    }

	    return (DataSet) HttpContext.Current.Cache["dsStateCounties"];
    }
}