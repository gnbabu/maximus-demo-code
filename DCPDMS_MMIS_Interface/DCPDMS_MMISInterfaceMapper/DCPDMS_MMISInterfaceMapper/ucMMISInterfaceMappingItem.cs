using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCPDMS_MMISInterfaceMapper
{
    public partial class ucMMISInterfaceMappingItem : UserControl
    {
        DataRow dr = null;
        int m_metadataID = 0;

        public void Save()
        {
            string sql = "UPDATE dbo.MMIS_INTERFACE_METADATA SET TABLE_NAME=@table_name, SELECT_CLAUSE= @select_clause  WHERE MMIS_MD_ID=@mmis_md_id;"; 
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@table_name", cmbTableName.Text));
            parms.Add(new SqlParameter("@select_clause", cmbSelectClause.Text));
            parms.Add(new SqlParameter("@mmis_md_id", m_metadataID));
            Helper.ExecuteSql(sql, parms);
        }
        public void AddDataRow(DataTable dt)
        {
            DataRow retDr = dt.NewRow();
            retDr["MMIS_MD_ID"] = m_metadataID;
            retDr["INTERFACE_ID"] = Convert.ToInt32(lblInterfaceID.Text);
            retDr["SEQUENCE_NUM"] = Convert.ToInt32(lblSequenceNumber.Text);
            retDr["ELEMENT_NAME"] = lblElementName.Text.Trim();
            retDr["TABLE_NAME"] = cmbTableName.Text;
            retDr["SELECT_CLAUSE"] = cmbSelectClause.Text;
            retDr["LENGTH"] = Convert.ToInt32(lblLength.Text);

            dt.Rows.Add(retDr);
        }

        public DataRow GetData()
        {
            DataRow retDr = CreateMetaDataRow();
            retDr["MMIS_MD_ID"] = m_metadataID;
            retDr["INTERFACE_ID"] = Convert.ToInt32(lblInterfaceID.Text);
            retDr["SEQUENCE_NUM"] = Convert.ToInt32(lblSequenceNumber.Text);
            retDr["ELEMENT_NAME"] = lblElementName.Text.Trim();
            retDr["TABLE_NAME"] = cmbTableName.Text;
            retDr["SELECT_CLAUSE"] = cmbSelectClause.Text;
            retDr["LENGTH"] = Convert.ToInt32(lblLength.Text);

            return retDr;
        }

        private DataRow CreateMetaDataRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MMIS_MD_ID", typeof(int));
            dt.Columns.Add("INTERFACE_ID", typeof(int));
            dt.Columns.Add("SEQUENCE_NUM", typeof(int));
            dt.Columns.Add("ELEMENT_NAME", typeof(string));
            dt.Columns.Add("TABLE_NAME", typeof(string));
            dt.Columns.Add("SELECT_CLAUSE", typeof(string));
            dt.Columns.Add("LENGTH", typeof(int));

            return dt.NewRow();
        }
        public ucMMISInterfaceMappingItem()
        {
            InitializeComponent();
        }

        public void SetData(DataRow newDr, DataTable viewAndTables)
        {
            dr = newDr;
            cmbTableName.DisplayMember = "TABLE_NAME";
            cmbTableName.DataSource = viewAndTables;
            cmbTableName.SelectedValueChanged += cmbTableName_SelectedValueChanged;
        }

        void cmbTableName_SelectedValueChanged(object sender, EventArgs e)
        {
            DataSet ds = Helper.ExecuteSql("SELECT * FROM information_schema.columns WHERE TABLE_NAME='" + Helper.GetString("TABLE_NAME", ((DataRowView)cmbTableName.SelectedItem).Row) + "' ORDER BY ORDINAL_POSITION");
            cmbSelectClause.DisplayMember = "COLUMN_NAME";
            cmbSelectClause.DataSource = ds.Tables[0];
        }


        private void ucMMISInterfaceMappingItem_Load(object sender, EventArgs e)
        {
            m_metadataID = Helper.GetInt("MMIS_MD_ID", dr);
            lblInterfaceID.Text = Helper.GetInt("INTERFACE_ID", dr).ToString();
            lblSequenceNumber.Text = Helper.GetInt("SEQUENCE_NUM", dr).ToString();
            lblElementName.Text = Helper.GetString("ELEMENT_NAME", dr);
            cmbTableName.Text = Helper.GetString("TABLE_NAME", dr);
            cmbSelectClause.Text = Helper.GetString("SELECT_CLAUSE", dr);
            lblLength.Text = Helper.GetInt("LENGTH", dr).ToString();
        }
    }
}
