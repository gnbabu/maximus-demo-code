namespace DCPDMS_MMISInterfaceMapper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dC_PDMS_RBM02DataSet = new DCPDMS_MMISInterfaceMapper.DC_PDMS_RBM02DataSet();
            this.mMISINTERFACEMETADATABindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mMIS_INTERFACE_METADATATableAdapter = new DCPDMS_MMISInterfaceMapper.DC_PDMS_RBM02DataSetTableAdapters.MMIS_INTERFACE_METADATATableAdapter();
            this.pnlMetadata = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dC_PDMS_RBM02DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMISINTERFACEMETADATABindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dC_PDMS_RBM02DataSet
            // 
            this.dC_PDMS_RBM02DataSet.DataSetName = "DC_PDMS_RBM02DataSet";
            this.dC_PDMS_RBM02DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mMISINTERFACEMETADATABindingSource
            // 
            this.mMISINTERFACEMETADATABindingSource.DataMember = "MMIS_INTERFACE_METADATA";
            this.mMISINTERFACEMETADATABindingSource.DataSource = this.dC_PDMS_RBM02DataSet;
            // 
            // mMIS_INTERFACE_METADATATableAdapter
            // 
            this.mMIS_INTERFACE_METADATATableAdapter.ClearBeforeFill = true;
            // 
            // pnlMetadata
            // 
            this.pnlMetadata.AutoScroll = true;
            this.pnlMetadata.Location = new System.Drawing.Point(13, 13);
            this.pnlMetadata.Name = "pnlMetadata";
            this.pnlMetadata.Size = new System.Drawing.Size(981, 701);
            this.pnlMetadata.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(400, 720);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(207, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Mapping";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(613, 720);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Write Interface Code File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 755);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlMetadata);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dC_PDMS_RBM02DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mMISINTERFACEMETADATABindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DC_PDMS_RBM02DataSet dC_PDMS_RBM02DataSet;
        private System.Windows.Forms.BindingSource mMISINTERFACEMETADATABindingSource;
        private DC_PDMS_RBM02DataSetTableAdapters.MMIS_INTERFACE_METADATATableAdapter mMIS_INTERFACE_METADATATableAdapter;
        private System.Windows.Forms.Panel pnlMetadata;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button1;
    }
}

