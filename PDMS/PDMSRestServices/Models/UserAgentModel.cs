using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class UserAgentDropdowns
    {
        public List<AgentRoles> AgentRole { get; set; }

        public List<AssignedMedIDs> AssignedMedID { get; set; }

        public List<PageSize> PageSizes { get; set; }

        public List<AssignedAgents> AssignedAgent { get; set; }
    }

    public class AgentRoles
    {
        public int AGENT_SUB_ROLES_ID { get; set; }

        public string AGENT_SUB_ROLES_DESC { get; set; }
    }

    public class AssignedMedIDs
    {
        public int REG_ID { get; set; }

        public string MEDICAID_ID { get; set; }
    }

    public class AssignedAgents
    {
        public string AGENT_USER_ID { get; set; }

        public string AGENT_OH_ID { get; set; }
    }

    public class AgentSearchRequest
    {
        public string? MedIdList { get; set; }
        public string? AgentIdList { get; set; }
        public string? AgentRoleList { get; set; }

        public string LoggedInUserID { get; set; }
        public string SelectedProvAdminUserID { get; set; }
    }

    public class AddAgentValRequest
    {
        public string MedID { get; set; }
        public string AgentID { get; set; }
        public string AgentEmail { get; set; }

        public string UserID { get; set; }

        public string CallType { get; set; }
    }

    public class AgentRoles1
    {
        public int AGENT_SUB_ROLES_ID { get; set; }

        public string AGENT_SUB_ROLES_DESC { get; set; }

        public int IS_ENABLED { get; set; }
        public string AGENT_SUB_ROLES_TOOLTIP { get; set; }
    }

    public class AddAgentValResponse
    {
        public List<AgentRoles1> AgentRoles { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
    }

    public class AddAgentRequest
    {
        public string MedID { get; set; }
        public string AgentID { get; set; }
        public string UserID { get; set; }
        public string AgentRoleList { get; set; }
    }

    public class ResponseValue
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }
    }
}
