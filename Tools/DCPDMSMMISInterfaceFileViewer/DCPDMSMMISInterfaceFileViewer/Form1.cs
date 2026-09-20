using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DCPDMSMMISInterfaceFileViewer
{

    public partial class Form1 : Form
    {
        private const int MMIS_MD_ID_INDEX = 0;
        private const int INTERFACE_ID_INDEX = 1;
        private const int ELEMENT_NAME_INDEX = 2;
        private const int LENGTH_INDEX = 3;
        private const int SEQUENCE_NUMBER_INDEX = 4;
        private const int IS_REQUIRED_INDEX = 5;
        private List<MMISMetaData> m_metaData;
        private int m_widthDifference = 0, m_heightDifference = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // uncomment this if we need to use the combo box for multiple interface types
            //DataSet ds = Helper.ExecuteSql("SELECT DISTINCT INTERFACE_ID FROM dbo.[MMIS_INTERFACE_METADATA]");
            //if (Helper.HasRows(ds))
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        cmbInterfaceID.Items.Add(Helper.GetString("INTERFACE_ID", dr));
            //    }
            //}

          //  cmbInterfaceID.SelectedIndex = 0;
            InitializeFileViewGridFromFlatFile(ConfigurationManager.AppSettings["MetadataFilePath"]);
            m_widthDifference = 22; //dataGridView1.Parent.Width - dataGridView1.Width;
            m_heightDifference = 48;
            dataGridView1.Width = dataGridView1.Parent.ClientSize.Width - m_widthDifference;
            dataGridView1.Height = dataGridView1.Parent.ClientSize.Height - m_heightDifference;

        }

        private void InitializeFileViewGridFromFlatFile(string filePath)
        {
            try
            {
                StreamReader metadataFile = new StreamReader(filePath);
                if (metadataFile != null)
                {
                    if (m_metaData == null)
                    {
                        m_metaData = new List<MMISMetaData>();
                    }
                    else
                    {
                        m_metaData.Clear();
                    }
                    int totalLength = 0;
                    string currentLine = "";
                    dataGridView1.Columns.Clear();
                    while ((currentLine = metadataFile.ReadLine()) != null)
                    {
                        string[] currentMetadata = currentLine.Split('|');
                        if (currentMetadata != null && currentMetadata.Length == 6 && currentMetadata[MMIS_MD_ID_INDEX] != "MMIS_MD_ID")
                        {
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                            MMISMetaData dta = new MMISMetaData();
                            dta.IsRequired = (currentMetadata[IS_REQUIRED_INDEX] == "1");
                            dta.ElementName = currentMetadata[ELEMENT_NAME_INDEX] + (dta.IsRequired ? "*" : "");
                            col.Name = col.HeaderText = dta.ElementName;
                            col.Width = (int)(1.1 * (double)TextRenderer.MeasureText(col.HeaderText, dataGridView1.RowHeadersDefaultCellStyle.Font).Width);
                            dta.Length = Convert.ToInt32(currentMetadata[LENGTH_INDEX]);
                            totalLength += dta.Length;
                            dta.SeqNumber = Convert.ToInt32(currentMetadata[SEQUENCE_NUMBER_INDEX]);
                            dataGridView1.Columns.Add(col);
                            m_metaData.Add(dta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void cmbInterfaceID_SelectedIndexChanged(object sender, EventArgs e)
        {
           // uncomment this to go back to pulling metadata from the database
           // InitializeFileViewGridFromDatabase(Convert.ToInt32(cmbInterfaceID.SelectedItem));
        }

        private void InitializeFileViewGridFromDatabase(int interfaceID)
        {
            DataSet ds = Helper.ExecuteSql("SELECT * FROM dbo.[MMIS_INTERFACE_METADATA] WHERE INTERFACE_ID=" + interfaceID.ToString() + " ORDER BY SEQUENCE_NUM;");
            if (Helper.HasRows(ds))
            {
                if (m_metaData == null)
                {
                    m_metaData = new List<MMISMetaData>();
                }
                else
                {
                    m_metaData.Clear();
                }
                int totalLength = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                    MMISMetaData dta = new MMISMetaData();
                    dta.ElementName = Helper.GetString("ELEMENT_NAME", dr);
                    col.Name = col.HeaderText = dta.ElementName;
                    col.Width = (int)(1.1 * (double)TextRenderer.MeasureText(col.HeaderText, dataGridView1.RowHeadersDefaultCellStyle.Font).Width);
                    dta.Length = Helper.GetInt("LENGTH", dr);
                    totalLength += Helper.GetInt("LENGTH", dr);
                    dta.SeqNumber = Helper.GetInt("SEQUENCE_NUM", dr);
                    dataGridView1.Columns.Add(col);
                    m_metaData.Add(dta);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDataFileName.Text = ofd.FileName;
                StreamReader sr = new StreamReader(ofd.FileName);
                while (!sr.EndOfStream)
                {
                    string currentLine = sr.ReadLine();
                    int startIndex = 0;
                    int colIndex = 0;
                    int rowIndex = dataGridView1.Rows.Add();
                    foreach (MMISMetaData dt in m_metaData)
                    {
                        string curText = currentLine.Substring(startIndex, dt.Length);
                        int textWidth = (int)(1.1 * (double)TextRenderer.MeasureText(curText, dataGridView1.RowHeadersDefaultCellStyle.Font).Width);
                        if (textWidth > dataGridView1.Columns[colIndex].Width)
                        {
                            dataGridView1.Columns[colIndex].Width = textWidth;
                        }
                        dataGridView1.Rows[rowIndex].Cells[colIndex].Value = curText;
                        
                        startIndex += dt.Length;
                        colIndex++;
                    }
                }
                sr.Close();

            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Width = dataGridView1.Parent.ClientSize.Width - m_widthDifference;
            dataGridView1.Height = dataGridView1.Parent.ClientSize.Height - m_heightDifference;
        }
    }
    public class MMISMetaData
    {
        public string ElementName;
        public int Length;
        public int SeqNumber;
        public bool IsRequired;
    }
}
