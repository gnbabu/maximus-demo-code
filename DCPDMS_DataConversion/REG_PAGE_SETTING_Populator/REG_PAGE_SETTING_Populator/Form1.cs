using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace REG_PAGE_SETTING_Populator
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pdmsMMISProviderTypeColumnIndex = 1;
            int pdmsApplicationTypeColumnIndex = 7;
            int pdmsProviderCategoryColumnIndex = 10;
            List<PageNode> pageSectionInfo = new List<PageNode>();
            XLWorkbook xlPageData = new XLWorkbook("C:\\Users\\rmays\\Documents\\DCPDMS\\Look up Table Population\\DSD Screen Flow by Provider Type - Final (2).xlsx");

            IXLWorksheet workSheet = xlPageData.Worksheet("Provider Reference Data Master");

            IXLRow currentRow = workSheet.Row(2);
            for (int cellIndex = 14; cellIndex < 62; cellIndex++)
            {
                IXLCell currentCell = currentRow.Cell(cellIndex);
                string cellContent = currentCell.Value.ToString();
                if (!cellContent.Contains("worry"))
                {
                    PageNode currentPageNode = new PageNode();
                    currentPageNode.CellIndex = cellIndex;
                    if (cellContent.Contains("-"))
                    {
                        string[] parsedString = cellContent.Split('-');
                        currentPageNode.PageName = parsedString[0].Trim();
                        currentPageNode.SectionName = parsedString[1].Trim();
                    }
                    else
                    {
                        currentPageNode.PageName = cellContent.Trim();
                        currentPageNode.SectionName = "";
                    }
                    pageSectionInfo.Add(currentPageNode);
                }
            }

            CHelper.ExecuteSql("DELETE FROM  [dbo].[REG_PAGE_SETTING] WHERE LEN(RTRIM(LTRIM(ISNULL(TASK_NAME, '')))) = 0 ");

            for (int rowIndex = 3; rowIndex < 183; rowIndex++)
            {
                currentRow = workSheet.Row(rowIndex);
                if (currentRow.Cell(pdmsMMISProviderTypeColumnIndex).Value.ToString().Trim().Length > 0)
                {
                    string mmisProviderTypeID = currentRow.Cell(pdmsMMISProviderTypeColumnIndex).GetValue<string>().Trim();
                    int applicationID = currentRow.Cell(pdmsApplicationTypeColumnIndex).GetValue<int>();
                    int providerCatID = Convert.ToInt32(currentRow.Cell(pdmsProviderCategoryColumnIndex).GetValue<string>());
                    object rawProviderTypeID = CHelper.ExecuteSqlScalar("SELECT PROVIDER_TYPE_ID FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = '" + mmisProviderTypeID + "' AND APPLICATION_TYPE_ID=" +applicationID.ToString() + " AND PROVIDER_CATEGORY_TYPE_ID=" + providerCatID.ToString(), null);
                    if (rawProviderTypeID != null)
                    {
                        int provTypeID = (int)rawProviderTypeID;
                        foreach (PageNode pgNode in pageSectionInfo)
                        {
                            int isEditableVisible = 0;
                            if (currentRow.Cell(pgNode.CellIndex).Value.ToString() == "X")
                            {
                                isEditableVisible = 1;
                            }

                            string strSQL = "INSERT INTO [dbo].[REG_PAGE_SETTING] ([ENTITY_TYPE_ID],[PROVIDER_TYPE_ID],";
                            strSQL += "[REG_PAGE_NAME],[REG_PAGE_SECTION],[IS_VISIBLE],[IS_EDITABLE],[LAST_MODIFIED_DATE_TIME]";
                            strSQL += ",[LAST_MODIFIED_USER],[TASK_NAME],[IS_REQUIRED], [APPLICATION_TYPE_ID]) VALUES (";
                            strSQL += providerCatID.ToString() + ",";
                            strSQL += provTypeID.ToString() + ",";
                            strSQL += "'" + pgNode.PageName + "',";
                            strSQL += "'" + pgNode.SectionName + "',";
                            strSQL += isEditableVisible.ToString() + ",";
                            strSQL += isEditableVisible.ToString() + ",";
                            strSQL += "'7/20/2016', '5D0689A8-D885-4211-B9FD-56757474AB4D', '',0,";
                            strSQL += applicationID.ToString() + ")";
                            CHelper.ExecuteSql(strSQL);
                        }
                    }
                }
            }
            MessageBox.Show("All done!");
        }
    }

    public class PageNode
    {
        public int CellIndex;
        public string PageName;
        public string SectionName;
    }
}
