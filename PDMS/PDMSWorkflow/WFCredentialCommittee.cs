using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFCredentialCommittee : BaseWorkflowTask, IWorkflowTask
    {
        string nextStep = null;

        public WFCredentialCommittee(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool returnValue = false;

            int regID = 0;
            try
            {
                Process p = new Process(ProcessID);
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }
                            

                int credentialingId = 0;
                DataSet dsCredential = CredentialController.SelectProviderCredentialingData(regID);
                if (ObjectControllerHelper.HasRows(dsCredential))
                {
                    credentialingId = Convert.ToInt32(dsCredential.Tables[0].Rows[0]["credentialing_id"]);
                    DataSet dsMembers = CredentialController.GetCredentialCommitteeMembers();
                    if (ObjectControllerHelper.HasRows(dsMembers))
                    {
                        int committeeMemberId = 0;
                        string memberUserName = string.Empty;
                        foreach (DataRow dr in dsMembers.Tables[0].Rows)
                        {
                            committeeMemberId = Convert.ToInt32(dr["committee_member_id"]);
                            memberUserName = dr["member_username"].ToString();
                            if (committeeMemberId > 0 && !string.IsNullOrEmpty(memberUserName))
                            {
                                CredentialController.InsertProviderCredentialingCommitteeMember(credentialingId, committeeMemberId, memberUserName, DateTime.Now, Constants.appAdminUserId);
                            }
                            committeeMemberId = 0;
                            memberUserName = string.Empty;
                        }

                        nextStep = "Next";
                        returnValue = true;
                        return returnValue;
                    }

                }

                return returnValue;

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, "RegistrationId: " + regID.ToString() + " - " + logMsg);
            }

            return returnValue;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}
