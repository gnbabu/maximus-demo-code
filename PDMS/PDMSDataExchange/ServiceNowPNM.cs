using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.MCPN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS
{

    public class ServiceNowPNM : BaseJob, IJob
    {
        public ServiceNowPNM(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }
        override public void ExecuteJob()
        {
            // Default Job - B2P Retrieve Payment Information
            this.ExecuteJob(Guid.Parse("18523204-8B74-4AB0-B17D-4E55D41E5AEE"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS Retrieve Payment Information
                case "18523204-8B74-4AB0-B17D-4E55D41E5AEE":
                    ExportDailyRecords();
                    break;
            }
        }
        public static void ExportDailyRecords()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet dsMembers = new DataSet();                 
                dsMembers = DataAccess.ExecuteStoredProcedure("USP_SELECT_CSV_STG_SERVICENOW_FILE", "DailyExtract");

                DirectoryInfo localDirectory;
                string localPath = AppSettings.Get("PNMService-ExportLocalPath");
                localDirectory = new DirectoryInfo(String.Format(localPath));

                if (!localDirectory.Exists)
                {
                    localDirectory.Create();
                }           
                string exportFile = localDirectory + "mcpexrt.CSV";
                DataRow dr = dsMembers.Tables[0].NewRow();
                dr["Id_provider"] = "Id_provider";
                dr["Name"] = "Name";
                dr["County"] = "County";
                dr["OutOfState"] = "OutOfState";
                dr["PrevNumber"] = "PrevNumber";
                dr["NewNumber"] = "NewNumber";
                dr["Type"] = "Type";
                dr["Specialty1"] = "Specialty1";
                dr["Specialty2"] = "Specialty2";
                dr["Telephone"] = "Telephone";
                dr["County"] = "County";
                dr["Address1"] = "Address1";
                dr["Address2"] = "Address2";
                dr["City"] = "City";
                dr["State"] = "State";
                dr["Zip"] = "Zip";
                dr["License Number"] = "License Number";
                dr["Active Code"] = "Active Code";
                dr["Status Date"] = "Status Date";
                dr["NPI"] = "NPI";
                dr["NPI Type"] = "NPI Type";
                dr["NPI VIND"] = "NPI VIND";
                dr["NPI Begin Date"] = "NPI Begin Date";
                dr["NPI End Date"] = "NPI End Date";
                dr["DEA Number"] = "DEA Number";
                dr["PCRI Attestation Verification"] = "PCRI Attestation Verification";
                dr["PCRI Begin Date"] = "PCRI Begin Date";
                dr["Specialty3"] = "Specialty3";
                dr["Specialty4"] = "Specialty4";
                dr["Specialty5"] = "Specialty5";
                dr["Specialty6"] = "Specialty6";
                dr["Specialty7"] = "Specialty7";
                dr["Specialty8"] = "Specialty8";
                dr["Specialty9"] = "Specialty9";
                dr["Specialty10"] = "Specialty10";
                dr["Specialty11"] = "Specialty11";
                dr["Specialty12"] = "Specialty12";
                dr["Tax Id"] = "Tax Id";
                dr["Tax Id Indicator"] = "Tax Id Indicator";              
                dsMembers.Tables[0].Rows.InsertAt(dr, 0);
                if (dsMembers != null &&
                    dsMembers.Tables != null &&
                    dsMembers.Tables.Count > 0 &&
                    dsMembers.Tables[0] != null &&
                    dsMembers.Tables[0].Rows != null &&
                    dsMembers.Tables[0].Rows.Count > 0)
                {

                    MCPShared.CreateCSV(dsMembers.Tables[0], exportFile);
                }
                else
                {
                    System.IO.File.WriteAllLines(exportFile, new string[0]);
                }

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
}
