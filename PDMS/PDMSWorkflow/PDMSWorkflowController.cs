using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public static class PDMSWorkflowController
    {
        private static Type[] workflowTypes { get; set; }
        private static Type[] WorkflowTypes
        {
            get
            {
                if (workflowTypes == null)
                    workflowTypes =Assembly.GetExecutingAssembly().GetTypes()
                               .Where(t => t.IsSubclassOf(typeof(BaseWorkflow))).ToArray();

                return workflowTypes;
            }
        }
        public static BaseWorkflow GetWorkflowInstance(int? applicationTypeID, int? providerCategoryTypeID, int? providerTypeID, int? referralTypeID,
            bool revalDue, bool referral, bool conversion, bool groupMember)
        {
            //simple static factory method
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ApplicationTypeID", DbType.Int32, applicationTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ProviderCategoryTypeID ", DbType.Int32, providerCategoryTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ProviderTypeID", DbType.Int32, providerTypeID, true));
                parameters.Add(SqlParms.CreateParameter("ReferralTypeID", DbType.Int32, referralTypeID, true));
                parameters.Add(SqlParms.CreateParameter("RevalDue", DbType.Boolean, revalDue, false));
                parameters.Add(SqlParms.CreateParameter("Referral", DbType.Boolean, referral, false));
                parameters.Add(SqlParms.CreateParameter("Conversion", DbType.Boolean, conversion, false));
                parameters.Add(SqlParms.CreateParameter("GroupMember", DbType.Boolean, groupMember, false)); 

                ds = DataAccess.ExecuteStoredProcedure("usp_SelectWorkflow", parameters, "Workflow");

                int workflowID = (int)ds.Tables[0].Rows[0]["WORKFLOW_ID"];
                string workflowName = ds.Tables[0].Rows[0]["WORKFLOW_NAME"].ToString();

                BaseWorkflow bw = null;
                foreach (Type t in WorkflowTypes)
                {
                    BaseWorkflow thisBW = (BaseWorkflow)Activator.CreateInstance(t);
                    if (thisBW.WorkflowID == workflowID)
                    {
                        bw = thisBW;
                        bw.WorkflowName = workflowName;                     
                    }
                }
                return bw; 
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
 

        }
    }
}
