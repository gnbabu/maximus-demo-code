<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    <p>TennCare Provider Registration Portal</p>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="pnlHomePage" runat="server" CssClass="infoPanel">
        <br />
            Welcome to the <b>TennCare Provider Registration Portal</b>. This portal allows providers the ability to submit key information electronically to obtain a 
            Medicaid ID for a new provider. In addition, existing Medicaid providers can enter key information that allows TennCare to receive updates electronically. 
            No matter if you are a new provider to TennCare / Medicaid or an existing TennCare / Medicaid provider; all providers will need to register their 
            information here. TennCare is now using web-based technology to simplify and improve the provider registration / re-verification process. 
        <br /><br />
            <b><u>Individual Provider Persons</u></b><br />
            Individual providers are no longer required to submit paper applications or update forms to TennCare by mail. TennCare has partnered with CAQH (Council 
            for Affordable Quality Healthcare) to allow for centralized collection of individual provider information. 
        <br /><br />
            If you are an individual provider, new or existing you must have a CAQH ID in order for TennCare to enroll and / or update your information. 
            To accomplish this you need to register here on the TennCare website regardless if you are new or existing to CAQH. When a provider enters his or 
            her information on the TennCare website; TennCare will submit the information to CAQH. CAQH will send us information each time a provider updates 
            his or her information with CAQH. The provider CAQH ID must be in an attested status in order for us to receive their information. Individual providers 
            only need to register once on this website. Once registered all other updates should be done through CAQH.
        <br /><br />
            If you are an individual provider, new or existing provider and you <b>do not</b> have a CAQH ID, please register on our web site to begin the process for 
            obtaining your CAQH ID. Once registered, TennCare will submit your information to CAQH. CAQH will assign you an ID and mail you information on how to access 
            CAQH and enter your provider data. The information will be mailed to the credentialing address entered when you register, so enter the address where 
            you would like the correspondence sent initially. If you do not receive your CAQH ID within one week, you can contact CAQH at 1-888-600-9802 
        <br /><br />
            If you are an individual provider, new or existing and you <b>do</b> have a CAQH ID, TennCare recommends you have your CAQH profile up to date with all service 
            locations before registering on the TennCare website. Please register here and TennCare will submit your information to CAQH. CAQH will submit your 
            data to us for enrollment and / or updating. 
        <br /><br />
            To begin the process to obtain a Medicaid ID, update information on your existing Medicaid ID or if you have been directed to this link by the EHR 
            Incentive Program, click on the <b>Create Account</b> link at the left of this page. 
        <br /><br />
            <b><u>Group/Entity</u></b><br /> 
            Single and Multi-Specialty groups should also register, update, revalidate, and add members through the TennCare Registration Portal. This process does 
            not negate any individual provider person from registering. All individual provider persons still have to register through the online registration portal, 
            and complete a CAQH application in order to link to a specific group. Please keep in mind that all providers affiliated with a group must list the group 
            or groups they are to be affiliated with in their CAQH application. To create your login information and begin the process of registering a group 
            with TennCare.....
        <br /><br />
            If you have questions, please E-mail 
            <asp:HyperLink ID="lnkStateEmail" runat="server" NavigateUrl="mailto:Provider.Registration@tn.gov" Text="Provider.Registration@tn.gov" Target="_blank" />  
            or contact us at 1-800-852-2683.
    </asp:Panel>
</asp:Content>
