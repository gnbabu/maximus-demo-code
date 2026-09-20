using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_MQLoggingMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            GET_MQ_Message();
        }
    }

    private void GET_MQ_Message()
    {
        var paramId = Request.QueryString["id"];
        if(!string.IsNullOrEmpty(paramId))
        {
            int id = 0;
            var isValid = int.TryParse(paramId, out id);
            if(isValid)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                var ds = psc.GetSearchMQMessage(id);
                if(ds != null)
                {
                    lblMessage.Text = ds.Tables[0].Rows[0]["MESSAGE"].ToString();
                }
            }
        }
    }
}