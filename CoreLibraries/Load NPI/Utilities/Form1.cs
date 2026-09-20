using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegexTester
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Match match = Regex.Match(this.txtInput.Text, this.txtPattern.Text, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                this.lblResult.Text = "Valid";
            }
			else
			{
				this.lblResult.Text = "Not Valid";
			}
		}

        private void button2_Click(object sender, EventArgs e)
        {

            using (System.IO.StreamReader reader = System.IO.File.OpenText(@"C:\NPI\In\NPIDATA_LOAD.CSV"))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\NPI\In\NPIDATA_LOAD_2.CSV"))
                {
                    // Throw away the first line in the file.
                    if (!reader.EndOfStream)
                    {
                        reader.ReadLine();
                    }

                    // Copy all remaining lines to a new file.
                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();
                        writer.WriteLine(currentLine);
                    }
                }
            }
        }
	}
}
