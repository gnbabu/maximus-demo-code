using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.Models
{
    [DataContractFormat]
    public class UserDetailModel
    {
        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public String AccountExpires { get; set; }

        [DataMember]
        public String BadPasswordTime { get; set; }

        [DataMember]
        public String BadPasswordCount { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public String DisplayName { get; set; }

        [DataMember]
        public String EmployeeID { get; set; }

        [DataMember]
        public String GivenName { get; set; }

        [DataMember]
        public String LastLogonTimestamp { get; set; }

        [DataMember]
        public String LogonCount { get; set; }

        [DataMember]
        public String Mail { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String PwdLastSet { get; set; }

        [DataMember]
        public String SN { get; set; }  // (surname / last name)

        [DataMember]
        public String TelephoneNumber { get; set; }

        [DataMember]
        public String WhenChanged { get; set; }

        [DataMember]
        public String WhenCreated { get; set; }

        [DataMember]
        public String ErrorInfo { get; set; }

        [DataMember]
        public String ErrorCode { get; set; }

        [DataMember]
        public String ErrorMessage { get; set; }
    }

    [DataContractFormat]
    public class UserAgentProvAssnModel
    {

        [DataMember]
        public UserAgentProvAssnResultModel Result { get; set; }

        [DataMember]
        public List<UserAgentProvAssnUserModel> User { get; set; }

    }

    [DataContractFormat]
    public class UserAgentProvAssnResultModel
    {

        [DataMember]
        public string ReturnStatus { get; set; }

        [DataMember]
        public string ReturnStatusCode { get; set; }

        [DataMember]
        public string ReturnStatusDescription { get; set; }

    }

    [DataContractFormat]
    public class UserAgentProvAssnUserModel
    {

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string UserTypeDesc { get; set; }

        [DataMember]
        public string SAKWebUser { get; set; }

        [DataMember]
        public string DateLastLogon { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string ActiveIndicator { get; set; }

        [DataMember]
        public string CurrentContractEndDate { get; set; }

        [DataMember]
        public List<UserRolesModel> UserRoles { get; set; }

        [DataMember]
        public List<UserAgentProvAssnProviderModel> Provider { get; set; }
    }

    [DataContractFormat]
    public class UserAgentProvAssnProviderModel
    {

        [DataMember]
        public string ProvUserName { get; set; }

        [DataMember]
        public string ProvDateLastLogon { get; set; }

        [DataMember]
        public string ProvContactName { get; set; }

        [DataMember]
        public string ProvEmailAddress { get; set; }

        [DataMember]
        public string ProvPhoneNumber { get; set; }

        [DataMember]
        public string SAKDefaultProvider { get; set; }

        [DataMember]
        public string ProvProviderID { get; set; }

        [DataMember]
        public string ProvProviderTypeID { get; set; }

        [DataMember]
        public string ProvActiveIndicator { get; set; }

        [DataMember]
        public string ProvCurrentcontractEndDate { get; set; }

        [DataMember]
        public string RenderingProviderPracticeName { get; set; }

        [DataMember]
        public string RenderingProviderPracticeAddress { get; set; }

        [DataMember]
        public string RenderingProviderPracticeCity { get; set; }

        [DataMember]
        public string RenderingProviderPracticeState { get; set; }

        [DataMember]
        public string RenderingProviderPracticeZipCode { get; set; }

        [DataMember]
        public string RenderingProviderPracticeTelephone { get; set; }

        [DataMember]
        public string RenderingProviderPracticeFax { get; set; }

        [DataMember]
        public string RenderingProviderPracticeEmail { get; set; }

        [DataMember]
        public string RenderingProviderPracticeContactName { get; set; }

        [DataMember]
        public string PracticeNPI { get; set; }

        [DataMember]
        public string ProviderNPI { get; set; }

        [DataMember]
        public string ProviderMCDID { get; set; }

        [DataMember]
        public string PracticeMCDID { get; set; }

        [DataMember]
        public string PracticeMITSAdministrator { get; set; }

        [DataMember]
        public List<UserAgentProvRolesModel> ProvRolesAssignedToAgent { get; set; }
    }

    [DataContractFormat]
    public class UserAgentProvRolesModel
    {

        [DataMember]
        public string WebRoleCode { get; set; }

        [DataMember]
        public string WebRoleName { get; set; }

        [DataMember]
        public string WebRoleDescription { get; set; }        
    }

    [DataContractFormat]
    public class UserRolesModel
    {

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public string RoleDescription { get; set; }
    }

    /*      
     <xs:complexType name="UserRoles"> 
       <xs:sequence> 
        <xs:element name="RoleName" type="string" minOccurs="0"/> 
        <xs:element name="RoleDescription" type="string" minOccurs="0"/> 
       </xs:sequence> 
     </xs:complexType> 
      
     
     */

}
 