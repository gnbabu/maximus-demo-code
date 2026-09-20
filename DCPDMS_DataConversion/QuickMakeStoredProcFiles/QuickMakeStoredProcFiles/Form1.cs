using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QuickMakeStoredProcFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tableNames = new List<string>();

            tableNames.Add("APPLICATION_CATEGORY_TYPE");
            tableNames.Add("APPLICATION_TYPE");
            tableNames.Add("CATEGORY_OF_SERVICE_TYPE");
            tableNames.Add("CERTIFICATION_ACTION_TYPE");
            tableNames.Add("CERTIFICATION_ELIGIBILITY_TYPE");
            tableNames.Add("CERTIFICATION_LTC_BED_BREAKDOWN_TYPE");
            tableNames.Add("CERTIFIED_BEDS");
            tableNames.Add("CITIZENSHIP_TYPE");
            tableNames.Add("DEFENDENT_TYPE");
            tableNames.Add("DEGREE_TYPE");
            tableNames.Add("INSURANCE_TYPE");
            tableNames.Add("OWNER_CATEGORY_TYPE");
            tableNames.Add("ORGANIZATION_TYPE");
            tableNames.Add("PROVIDER_CATEGORY_TYPE");
            tableNames.Add("PROVIDER_RISKLEVEL_MAPPING");
            tableNames.Add("PROVIDER_RISK_LEVEL");
            tableNames.Add("PROVTYPE_ORGTYPE_MAPPING");
            tableNames.Add("REG_PAGE_SETTING");
            tableNames.Add("REG_PAGE_SETTING_ACTION");
            tableNames.Add("SCREENING_ACTIVITY_DEFAULT_TEMPLATE");
            tableNames.Add("SCREENING_ACTIVITY_OWNER_TEMPLATE");
            tableNames.Add("SCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE");
            tableNames.Add("SCREENING_ACTIVITY_RISK_LEVEL_TEMPLATE");
            tableNames.Add("ENROLLMENT_STATUS_TYPE");

            string str = "";
            foreach (string tableName in tableNames)
            {
                str = str + Environment.NewLine + "EXEC DCConv_LkUp_Populate" + tableName + " @pin_conv_run_time";
            }

            int x = 1;
            //StringBuilder sbBody = new StringBuilder();
            //sbBody.AppendLine("/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_Populate<table_name>]    Script Date: 7/21/2016 10:55:22 PM ******/");
            //sbBody.AppendLine("SET ANSI_NULLS ON");
            //sbBody.AppendLine("GO");
            //sbBody.AppendLine("SET QUOTED_IDENTIFIER ON");
            //sbBody.AppendLine("GO");
            //sbBody.AppendLine("-- =============================================");
            //sbBody.AppendLine("-- Author:		Richard Mays");
            //sbBody.AppendLine("-- Create date: 7/21/2016");
            //sbBody.AppendLine("-- Description:	Fills the <table_name>");
            //sbBody.AppendLine("-- =============================================");
            //sbBody.AppendLine("CREATE PROCEDURE [dbo].[DCConv_LkUp_Populate<table_name>] ");
            //sbBody.AppendLine("	@pin_run_reference_time datetime");
            //sbBody.AppendLine("AS");
            //sbBody.AppendLine("BEGIN");
            //sbBody.AppendLine(" -- SET NOCOUNT ON added to prevent extra result sets from");
            //sbBody.AppendLine("	-- interfering with SELECT statements.");
            //sbBody.AppendLine("	SET NOCOUNT ON;");
            //sbBody.AppendLine();
            //sbBody.AppendLine("	DELETE FROM [dbo].[<table_name>];");
            //sbBody.AppendLine();
            //sbBody.AppendLine("	-- add inserts here");
            //sbBody.AppendLine();
            //sbBody.AppendLine("END");

            //string strBodyTemplate = sbBody.ToString();
            //string strFileName = "DCConv_LkUp_Populate<table_name>.sql";

            //foreach (string tableName in tableNames)
            //{
            //    StreamWriter sw = new StreamWriter(strFileName.Replace("<table_name>", tableName));

            //    sw.Write(strBodyTemplate.Replace("<table_name>", tableName));

            //    sw.Close();
            //}

        }
    }
}
