using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;

namespace DCPDMS_MMISInterfaceMapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataSet dsViewsAndTables = Helper.ExecuteSql("SELECT TABLE_SCHEMA, TABLE_NAME, 'table' AS [Type] FROM information_schema.tables WHERE TABLE_SCHEMA='dbo' AND NOT TABLE_NAME LIKE 'aspnet_%' AND NOT TABLE_NAME LIKE 'MMIS_%' AND  NOT TABLE_NAME LIKE 'vw_%' UNION SELECT TABLE_SCHEMA, TABLE_NAME, 'view' AS [Type]  FROM information_schema.views WHERE NOT TABLE_NAME LIKE 'vw_%' ORDER BY [Type], TABLE_NAME");
            
            DataSet dsMMISElements = Helper.ExecuteSql("SELECT * FROM dbo.MMIS_INTERFACE_METADATA;");
          //  drMMISElements.DataSource = dsMMISElements.Tables[0];

            //for (int index = 0; index < drMMISElements.ItemCount; index++)
            //{
            //    drMMISElements.CurrentItemIndex = index;
            //    ComboBox cmbTables = (ComboBox)drMMISElements.CurrentItem.Controls["cmbTableName"];
            //    cmbTables.DisplayMember = "TABLE_NAME";
            //    cmbTables.DataSource = dsViewsAndTables.Tables[0];
            //}
            if (Helper.HasRows(dsMMISElements))
            {
                int yLocation = 0;
                foreach (DataRow dr in dsMMISElements.Tables[0].Rows)
                {
                    ucMMISInterfaceMappingItem newItem = new ucMMISInterfaceMappingItem();
                    newItem.SetData(dr, dsViewsAndTables.Tables[0]);
                    newItem.Location = new Point(0, yLocation);
                    pnlMetadata.Controls.Add(newItem);
                    yLocation += 35;
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in pnlMetadata.Controls)
            {
                if (ctrl.GetType().Name == "ucMMISInterfaceMappingItem")
                {
                    ucMMISInterfaceMappingItem item = (ucMMISInterfaceMappingItem)ctrl;
                    item.Save();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet dsMMISElements = Helper.ExecuteSql("SELECT * FROM dbo.MMIS_INTERFACE_METADATA;");
             if (Helper.HasRows(dsMMISElements))
            {
                StringBuilder code = new StringBuilder("");
                int totalLength = 0;
                foreach (DataRow dr in dsMMISElements.Tables[0].Rows)
                {
                    totalLength += Helper.GetInt("LENGTH", dr);
                    code.AppendLine("        [FieldFixedLength(" + Helper.GetInt("LENGTH", dr).ToString() + ")]");
                    code.AppendLine("        [FieldTrim(TrimMode.Both, Constants.TrimChar)]");
                    string elementName = Helper.GetString("ELEMENT_NAME", dr).Trim();
                    if (elementName.Contains("-DT"))
                    {
                        code.AppendLine("        [FieldConverter(typeof(CustomDateConverter), \"yyyy-MM-dd\")]");
                        code.AppendLine("        public DateTime? " + elementName.Replace("P-", "").Replace("-", "_").ToLower() + ";");
                    }
                    else
                    {
                        code.AppendLine("        public string " + elementName.Replace("P-", "").Replace("-", "_").ToLower() + ";");
                    }
                    code.AppendLine();

                }
            }
        }
    }
}
