using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Usercontrols_AddressValidation : System.Web.UI.UserControl
{
    #region " Member Variables "
    private string _unitAddress;
    private string _streetAddress;
    private string _city;
    private string _state;
    private string _zip5;
    private string _zip4;
    #endregion

    #region " Properties "
    public string UnitAddressControl { get; set; }
    public string StreetAddressControl { get; set; }
    public string CityControl { get; set; }
    public string StateControl { get; set; }
    public string Zip5Control { get; set; }
    public string Zip4Control { get; set; }
    public string ValidationGroup
    {
        get { return cvAddress.ValidationGroup; }
        set { cvAddress.ValidationGroup = value; }
    }
    public string SaveButtonClientID { get; set; }
    public bool Confirmed 
    {
        get 
        {
            return (hdnAddressConfirm.Value == "1") ? true : false;
        }
        set 
        {
            hdnAddressConfirm.Value = (value) ? "1" : "0";
        }
    }
    #endregion

    #region " Event Handlers "
    protected void Page_Load(object sender, EventArgs e)
    {
        btnConfirmAddress.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
            //+ "$(\"#" + divConfirmAddress.ClientID + "\").hide(); "
            + "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
            + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
            + "return false;";
    }

    protected void ServerValidation(object source, ServerValidateEventArgs args)
    {
        Control ctrl = this.Parent.FindControl(UnitAddressControl);
        _unitAddress = ((TextBox)ctrl).Text;
        string origUnitAddress = _unitAddress;

        ctrl = this.Parent.FindControl(StreetAddressControl);
        _streetAddress = ((TextBox)ctrl).Text;
        string origStreetAddress = _streetAddress;

        ctrl = this.Parent.FindControl(CityControl);
        _city = ((TextBox)ctrl).Text;
        string origCity = _city;

        ctrl = this.Parent.FindControl(StateControl);
        _state = ((DropDownList)ctrl).SelectedValue;
        string origState = _state;

        ctrl = this.Parent.FindControl(Zip5Control);
        _zip5 = ((TextBox)ctrl).Text;
        string origZip5 = _zip5;

        ctrl = this.Parent.FindControl(Zip4Control);
        _zip4 = ((TextBox)ctrl).Text;
        string origZip4 = _zip4;
        
        XElement xeAddress = new XElement("Address",
            CreateElement("Address1", _unitAddress),
            CreateElement("Address2", _streetAddress),
            CreateElement("City", _city),
            CreateElement("State", _state),
            CreateElement("Zip5", _zip5),
            CreateElement("Zip4", _zip4)
            );
        xeAddress.SetAttributeValue("ID", "0");

        XElement xeRequest = new XElement("AddressValidateRequest",
            CreateElement("IncludeOptionalElements", "true"),
            CreateElement("ReturnCarrierRoute", "true"),
            xeAddress
        );
        xeRequest.SetAttributeValue("USERID", "715MAXIM4882");

        WebRequest request = WebRequest.Create("http://production.shippingapis.com/ShippingAPITest.dll?API=Verify&XML=" + xeRequest.ToString(SaveOptions.DisableFormatting));
        // Create a request for the URL. 
        // If required by the server, set the credentials.
        request.Credentials = CredentialCache.DefaultCredentials;

        // Get the response.
        WebResponse response = request.GetResponse();

        // Display the status.
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        // Get the stream containing content returned by the server.
        Stream dataStream = response.GetResponseStream();

        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);

        // Read the content.
        string responseFromServer = reader.ReadToEnd();

        // Display the content.
        Console.WriteLine(responseFromServer);

        // Clean up the streams and the response.
        reader.Close();
        response.Close();

        // Parse the xml
        XDocument xdResponse = XDocument.Parse(responseFromServer);

        // Find the error element if one exists
        XElement xeError = xdResponse.Descendants("Error").FirstOrDefault();

        // If no error, check for ReturnText. This is a suggestion, but treat like an error
        if (xeError == null)
        {
            XElement xeReturnText = xdResponse.Descendants("ReturnText").FirstOrDefault();

            // No ReturnText, this is a valid address!
            if (xeReturnText == null)
            {
                bool isCorrected = false;

                if (xdResponse.Descendants("Address1").FirstOrDefault() != null)
                    _unitAddress = xdResponse.Descendants("Address1").FirstOrDefault().Value;
                _streetAddress = xdResponse.Descendants("Address2").FirstOrDefault().Value;
                _city = xdResponse.Descendants("City").FirstOrDefault().Value;
                _state = xdResponse.Descendants("State").FirstOrDefault().Value;
                _zip5 = xdResponse.Descendants("Zip5").FirstOrDefault().Value;
                if (xdResponse.Descendants("Zip4").FirstOrDefault() != null)
                    _zip4 = xdResponse.Descendants("Zip4").FirstOrDefault().Value;

                ctrl = this.Parent.FindControl(UnitAddressControl);
                if (((TextBox)ctrl).Text.ToUpper() != _unitAddress.ToUpper())
                    isCorrected = true;
                ((TextBox)ctrl).Text = _unitAddress;

                ctrl = this.Parent.FindControl(StreetAddressControl);
                if (((TextBox)ctrl).Text.ToUpper() != _streetAddress.ToUpper())
                    isCorrected = true;
                ((TextBox)ctrl).Text = _streetAddress;

                ctrl = this.Parent.FindControl(CityControl);
                if (((TextBox)ctrl).Text.ToUpper() != _city.ToUpper())
                    isCorrected = true;
                ((TextBox)ctrl).Text = _city;

                ctrl = this.Parent.FindControl(StateControl);
                if (((DropDownList)ctrl).SelectedValue.ToUpper() != _state.ToUpper())
                    isCorrected = true;
                ((DropDownList)ctrl).SelectedValue = _state;

                ctrl = this.Parent.FindControl(Zip5Control);
                if (((TextBox)ctrl).Text.ToUpper() != _zip5.ToUpper())
                    isCorrected = true;
                ((TextBox)ctrl).Text = _zip5;

                ctrl = this.Parent.FindControl(Zip4Control);
                if (((TextBox)ctrl).Text.ToUpper() != _zip4.ToUpper())
                    isCorrected = true;
                ((TextBox)ctrl).Text = _zip4;

                if (1 != 0)
                    cvAddress.ErrorMessage = "";

                // If the user already confirmed the usps correction or no correction was needed
                if (hdnAddressConfirm.Value == "1" || isCorrected == false)
                {
                    divConfirmAddress.Style["display"] = "none";
                    args.IsValid = true;
                }
                else // the user has not confirmed changes and a correction was needed. Show the changes in a jquery-ui dialog.
                {
                    this.paraUSPSAddress.InnerHtml = origStreetAddress + "<br>"
                        + origUnitAddress + "<br>"
                        + origCity + "<br>"
                        + origState + "<br>"
                        + origZip5 + "<br>"
                        + origZip4 + "<br>";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm", 
                        "$(function() {var divConfirm = $( \"#" + divConfirmAddress.ClientID + "\" );"
                        + "setTimeout(function() {divConfirm.dialog({ modal: true }); divConfirm.css(\"display\", \"block\"); "
                        + "divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10));}, 100); "
                        + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);
                    args.IsValid = false;
                }
            }
            else // ReturnText found (usually helpful information)
            {
                divConfirmAddress.Style["display"] = "none";
                cvAddress.ErrorMessage = xeReturnText.Value;
                args.IsValid = false;
            }
        }
        else // If an error is found, populate the validator message
        {
            divConfirmAddress.Style["display"] = "none";
            cvAddress.ErrorMessage = xeError.Descendants("Description").FirstOrDefault().Value;
            args.IsValid = false;
        }

    }
    #endregion

    #region " Methods "
    protected XElement CreateElement(string name, object value, string attribName = "", string attribValue = "")
    {
        XElement rtn;

        rtn = new XElement(name, value);

        if (attribName != "")
            rtn.SetAttributeValue(attribName, attribValue);

        return rtn;
    }
    #endregion
}
