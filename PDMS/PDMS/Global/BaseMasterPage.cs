using System;
using System.Web;

/// <summary>
/// Inherits from System.Web.UI.Page
/// Calls BaseView.SetLanguagePreference during PreInit to make sure 
/// localization preference is set early in page life cycle
/// </summary>
public class BaseMasterPage : System.Web.UI.MasterPage
{
    public BaseMasterPage()
    {
        if (Helper.SecureWebPages) ForceSSL();
        else ForceNonSSL();
    }

    public void ForceSSL()
    {
        // This is the current url 
        System.Uri currentUrl = HttpContext.Current.Request.Url;
        // Don't redirect if this is localhost
        if (currentUrl.IsLoopback) return;
        if (!currentUrl.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.CurrentCultureIgnoreCase))
        {
            // Build the secure uri        
            System.UriBuilder secureUrlBuilder = new UriBuilder(currentUrl);
            secureUrlBuilder.Scheme = Uri.UriSchemeHttps;
            // Use the default port.         
            secureUrlBuilder.Port = -1;
            // Redirect and end the response.        
            System.Web.HttpContext.Current.Response.Redirect(secureUrlBuilder.Uri.ToString());
        }
    }

    public void ForceNonSSL()
    {
        // This is the current url 
        System.Uri currentUrl = HttpContext.Current.Request.Url;
        // Don't redirect if this is localhost
        if (currentUrl.IsLoopback) return;
        if (currentUrl.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.CurrentCultureIgnoreCase))
        {
            // Build the secure uri        
            System.UriBuilder nonSecureUrlBuilder = new UriBuilder(currentUrl);
            nonSecureUrlBuilder.Scheme = Uri.UriSchemeHttp;
            // Use the default port.         
            nonSecureUrlBuilder.Port = -1;
            // Redirect and end the response.        
            System.Web.HttpContext.Current.Response.Redirect(nonSecureUrlBuilder.Uri.ToString());
        }
    }
}
