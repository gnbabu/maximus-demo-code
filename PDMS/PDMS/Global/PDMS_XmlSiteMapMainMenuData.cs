using System;
using System.Web;

/// <summary>
/// Summary description for PDMS_XmlSiteMapMainMenuData
/// </summary>
public class PDMS_XmlSiteMapMainMenuData : XmlSiteMapProvider
{
    private const string _allRoles = "*"; 
	public PDMS_XmlSiteMapMainMenuData() : base()
	{
        // call the base constructor
	}

    public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
    {
        if (node == null) 
        { 
            throw new ArgumentNullException("node");
        } 
  
        if (context == null) 
        {
            throw new ArgumentNullException("context"); 
        }
 
        if (!SecurityTrimmingEnabled) 
        {
            return true; 
        }
  
        //if (node.Roles != null) 
        //{
        //    if (SessionVarRetriever.MyQueueSelectedRoleName != null &&
        //        SessionVarRetriever.MyQueueSelectedRoleName.Trim().Length > 0)
        //    {
        //        foreach (string role in node.Roles)
        //        {
        //            // Grant access if one of the roles is a "*". 
        //            if (role == _allRoles ||
        //                context.User != null && role.Trim().Equals(SessionVarRetriever.MyQueueSelectedRoleName.Trim(), StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //}
        return base.IsAccessibleToUser(context, node);
    }
}