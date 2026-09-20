using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel.Tables;
using Newtonsoft.Json;
using PDMSRestServices.Controllers.Facade;
using PDMSRestServices.Facade;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSRestServices.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[EnableCors("NewPolicy")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("GetAgentAdministrationDropdowns")]
        public IActionResult GetAgentAdministrationDropdowns(string loggedinUserID, string selectedProvAdminUserID)
        {
            PDMSRestServices.Models.UserAgentDropdowns UserAgentDropdowns = new PDMSRestServices.Models.UserAgentDropdowns();
            List<PDMSRestServices.Models.AgentRoles> agentRoles = new List<PDMSRestServices.Models.AgentRoles>();
            List<PDMSRestServices.Models.AssignedMedIDs> assignedMedID = new List<PDMSRestServices.Models.AssignedMedIDs>();
            List<PDMSRestServices.Models.PageSize> pageSizes = new List<PDMSRestServices.Models.PageSize>();
            List<PDMSRestServices.Models.AssignedAgents> assignedAgents = new List<PDMSRestServices.Models.AssignedAgents>();
            
            try
            {                 
                DataSet dataSet = HelperFacade.GetAgentAdministrationDropdowns(loggedinUserID, selectedProvAdminUserID);
                DataTable dt = dataSet.Tables[0];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.AgentRoles agentrolesList = new PDMSRestServices.Models.AgentRoles();

                        agentrolesList.AGENT_SUB_ROLES_ID = Convert.ToInt32(dr["AGENT_SUB_ROLES_ID"]);
                        agentrolesList.AGENT_SUB_ROLES_DESC = Convert.ToString(dr["AGENT_SUB_ROLES_DESC"]);

                        agentRoles.Add(agentrolesList);
                    }
                }

                dt = dataSet.Tables[1];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.AssignedMedIDs medidList = new PDMSRestServices.Models.AssignedMedIDs();

                        medidList.REG_ID = Convert.ToInt32(dr["REG_ID"]);
                        medidList.MEDICAID_ID = Convert.ToString(dr["MEDICAID_ID"]);

                        assignedMedID.Add(medidList);
                    }
                }

                dt = dataSet.Tables[2];

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PDMSRestServices.Models.AssignedAgents agentList = new PDMSRestServices.Models.AssignedAgents();

                        agentList.AGENT_USER_ID = Convert.ToString(dr["AGENT_USER_ID"]);
                        agentList.AGENT_OH_ID = Convert.ToString(dr["UserName"]);

                        assignedAgents.Add(agentList);
                    }
                }

                UserAgentDropdowns.AgentRole = agentRoles;
                UserAgentDropdowns.AssignedMedID = assignedMedID;
                UserAgentDropdowns.AssignedAgent = assignedAgents;

                UserAgentDropdowns.PageSizes = new List<Models.PageSize>()
                {
                    new Models.PageSize(){Value="5",Size="5"},
                   new Models.PageSize(){Value="10",Size="10"},
                    new Models.PageSize(){Value="20",Size="20"},
                     new Models.PageSize(){Value="30",Size="30"},
                      new Models.PageSize(){Value="40",Size="40"},
                       new Models.PageSize(){Value="50",Size="50"}
                };

            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal server error :" + ex.Message });
            }
            return Ok(UserAgentDropdowns);

        }

        [HttpPost]
        [Route("GetProviderAgentsBySearchCriteria")]
        public IActionResult GetProviderAgentsBySearchCriteria([FromBody] PDMSRestServices.Models.AgentSearchRequest payload)
        {
            try
            {
                DataSet ds = HelperFacade.GetProviderAgentsBySearchCriteria(payload.MedIdList,payload.AgentIdList,payload.AgentRoleList,payload.LoggedInUserID,payload.SelectedProvAdminUserID);
                string data = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    data = "300";
                }
                    return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(550, new { message = "Internal server error :" + ex.Message });
            }
        }

        [HttpPost]
        [Route("GetAgentRolesByProvAdminAndMedID")]
        public IActionResult GetAgentRolesByProvAdminAndMedID([FromBody] PDMSRestServices.Models.AddAgentValRequest payload)
        {
            AddAgentValResponse resp = new AddAgentValResponse();
            try
            {
                DataSet ds = HelperFacade.GetAgentRolesByProvAdminAndMedID(payload.MedID, payload.AgentID, payload.AgentEmail, payload.UserID, payload.CallType);
                string data = string.Empty;
                List<AgentRoles1> arList = new List<AgentRoles1>();
                string code = string.Empty;

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        AgentRoles1 ar = new AgentRoles1();
                        code = Helper.GetString("RESP_CODE", row);

                        if (code.Equals("200"))
                        {
                            ar.AGENT_SUB_ROLES_ID = Helper.GetInt("AGENT_SUB_ROLES_ID", row);
                            ar.AGENT_SUB_ROLES_DESC = Helper.GetString("AGENT_SUB_ROLES_DESC", row);
                            ar.IS_ENABLED = Helper.GetInt("IS_ENABLED",row);
                            ar.AGENT_SUB_ROLES_TOOLTIP = Helper.GetString("AGENT_SUB_ROLES_TOOLTIP", row);
                            arList.Add(ar);
                            resp.AgentRoles = arList;
                            resp.ResponseCode = code;
                            resp.ResponseDesc = "SUCCESS";
                        }
                        else
                        {
                            resp.AgentRoles = null;
                            if (code.Equals("300"))
                            {
                                resp.ResponseCode = code;
                                resp.ResponseDesc = "OH ID does not exist.";
                            }
                            else if (code.Equals("301"))
                            {
                                resp.ResponseCode = code;
                                resp.ResponseDesc = "No user found with OHID/email combination.";
                            }
                            else if (code.Equals("302"))
                            {
                                resp.ResponseCode = code;
                                resp.ResponseDesc = "OH ID is not a Provider Agent.";
                            }
                            else if (code.Equals("303"))
                            {
                                resp.ResponseCode = code;
                                resp.ResponseDesc = "OH ID is a Power Agent or has a MCO role.";
                            }
                            else if (code.Equals("304"))
                            {
                                resp.ResponseCode = code;
                                resp.ResponseDesc = "Medicaid ID is not assigned to your administrator.";
                            }
                        }
                    }
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.ResponseCode = "500";
                resp.ResponseDesc = ex.Message + " " + ex.StackTrace;
                return Ok(resp);
            }
        }

        [HttpPost]
        [Route("SaveAgentRolesByProviderAdminAndMedID")]
        public IActionResult SaveAgentRolesByProviderAdminAndMedID([FromBody] PDMSRestServices.Models.AddAgentRequest payload)
        {
            try 
            {
                HelperFacade.SaveAgentRolesByProviderAdminAndMedID(payload.MedID, payload.AgentID, payload.UserID, payload.AgentRoleList);

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Adding new agent and their roles - ", ex);                
            }
        }

        [HttpPost]
        [Route("DeleteProviderAgentByMedicaidID")]
        public IActionResult DeleteProviderAgentByMedicaidID(string agentOHID, string medID)
        {
            try
            {
                HelperFacade.DeleteProviderAgentByMedicaidID(agentOHID, medID);

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Adding new agent and their roles - ", ex);
            }
        }

        [HttpPost]
        [Route("SaveReassignAdministratorByMedicaidID")]
        public IActionResult SaveReassignAdministratorByMedicaidID(string adminOHID, string medID, string userID)
        {
            ResponseValue resp = new ResponseValue();
            try
            {
               DataSet ds = HelperFacade.SaveReassignAdministratorByMedicaidID(adminOHID, medID, userID);
                string data = string.Empty;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    resp.ResponseCode = Helper.GetString("RESP_CODE", ds.Tables[0].Rows[0]); 
                    resp.ResponseDesc = Helper.GetString("RESP_DESC", ds.Tables[0].Rows[0]);
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                resp.ResponseCode = "500";
                resp.ResponseDesc = ex.Message + " " + ex.StackTrace;
                return Ok(resp);
            }
        }
    }
}
