namespace DCPDMS_MMISInterfaceMapper
{
    partial class ucMMISInterfaceMappingItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInterfaceID = new System.Windows.Forms.Label();
            this.lblElementName = new System.Windows.Forms.Label();
            this.cmbSelectClause = new System.Windows.Forms.ComboBox();
            this.cmbTableName = new System.Windows.Forms.ComboBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblSequenceNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblInterfaceID
            // 
            this.lblInterfaceID.Location = new System.Drawing.Point(27, 17);
            this.lblInterfaceID.Name = "lblInterfaceID";
            this.lblInterfaceID.Size = new System.Drawing.Size(64, 19);
            this.lblInterfaceID.TabIndex = 12;
            this.lblInterfaceID.Text = "label1";
            // 
            // lblElementName
            // 
            this.lblElementName.Location = new System.Drawing.Point(215, 17);
            this.lblElementName.Name = "lblElementName";
            this.lblElementName.Size = new System.Drawing.Size(221, 23);
            this.lblElementName.TabIndex = 13;
            this.lblElementName.Text = "label1";
            // 
            // cmbSelectClause
            // 
            this.cmbSelectClause.FormattingEnabled = true;
            this.cmbSelectClause.Location = new System.Drawing.Point(691, 14);
            this.cmbSelectClause.Name = "cmbSelectClause";
            this.cmbSelectClause.Size = new System.Drawing.Size(371, 24);
            this.cmbSelectClause.TabIndex = 14;
            // 
            // cmbTableName
            // 
            this.cmbTableName.FormattingEnabled = true;
            this.cmbTableName.Location = new System.Drawing.Point(454, 14);
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.Size = new System.Drawing.Size(219, 24);
            this.cmbTableName.TabIndex = 15;
            // 
            // lblLength
            // 
            this.lblLength.Location = new System.Drawing.Point(1106, 17);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(67, 23);
            this.lblLength.TabIndex = 16;
            this.lblLength.Text = "label1";
            // 
            // lblSequenceNumber
            // 
            this.lblSequenceNumber.Location = new System.Drawing.Point(125, 17);
            this.lblSequenceNumber.Name = "lblSequenceNumber";
            this.lblSequenceNumber.Size = new System.Drawing.Size(64, 23);
            this.lblSequenceNumber.TabIndex = 17;
            this.lblSequenceNumber.Text = "label1";
            // 
            // ucMMISInterfaceMappingItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInterfaceID);
            this.Controls.Add(this.lblElementName);
            this.Controls.Add(this.cmbSelectClause);
            this.Controls.Add(this.cmbTableName);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblSequenceNumber);
            this.Name = "ucMMISInterfaceMappingItem";
            this.Size = new System.Drawing.Size(1201, 54);
            this.Load += new System.EventHandler(this.ucMMISInterfaceMappingItem_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblInterfaceID;
        private System.Windows.Forms.Label lblElementName;
        private System.Windows.Forms.ComboBox cmbSelectClause;
        private System.Windows.Forms.ComboBox cmbTableName;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblSequenceNumber;
    }
}
