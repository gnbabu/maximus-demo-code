namespace MaxQuartz
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnGetJobs = new System.Windows.Forms.Button();
            this.lbJobs = new System.Windows.Forms.ListBox();
            this.lbTrigger = new System.Windows.Forms.ListBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.btnUpdateCronExpression = new System.Windows.Forms.Button();
            this.btnNewJob = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCronExpression = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnPauseJob = new System.Windows.Forms.Button();
            this.btnEnableJob = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cronErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.cronErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetJobs
            // 
            this.btnGetJobs.Location = new System.Drawing.Point(246, 320);
            this.btnGetJobs.Name = "btnGetJobs";
            this.btnGetJobs.Size = new System.Drawing.Size(75, 23);
            this.btnGetJobs.TabIndex = 0;
            this.btnGetJobs.Text = "Get Jobs";
            this.btnGetJobs.UseVisualStyleBackColor = true;
            this.btnGetJobs.Click += new System.EventHandler(this.btnGetJobs_Click);
            // 
            // lbJobs
            // 
            this.lbJobs.FormattingEnabled = true;
            this.lbJobs.Location = new System.Drawing.Point(13, 129);
            this.lbJobs.Name = "lbJobs";
            this.lbJobs.ScrollAlwaysVisible = true;
            this.lbJobs.Size = new System.Drawing.Size(310, 186);
            this.lbJobs.TabIndex = 1;
            this.lbJobs.SelectedIndexChanged += new System.EventHandler(this.lbJobs_SelectedIndexChanged);
            // 
            // lbTrigger
            // 
            this.lbTrigger.FormattingEnabled = true;
            this.lbTrigger.HorizontalScrollbar = true;
            this.lbTrigger.Location = new System.Drawing.Point(329, 129);
            this.lbTrigger.Name = "lbTrigger";
            this.lbTrigger.ScrollAlwaysVisible = true;
            this.lbTrigger.Size = new System.Drawing.Size(451, 186);
            this.lbTrigger.TabIndex = 2;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDatabase.CausesValidation = false;
            this.lblDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabase.Location = new System.Drawing.Point(13, 13);
            this.lblDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.lblDatabase.Size = new System.Drawing.Size(76, 23);
            this.lblDatabase.TabIndex = 3;
            this.lblDatabase.Text = "Database";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpdateCronExpression
            // 
            this.btnUpdateCronExpression.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCronExpression.Location = new System.Drawing.Point(705, 320);
            this.btnUpdateCronExpression.Name = "btnUpdateCronExpression";
            this.btnUpdateCronExpression.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateCronExpression.TabIndex = 4;
            this.btnUpdateCronExpression.Text = "Update";
            this.btnUpdateCronExpression.UseVisualStyleBackColor = true;
            this.btnUpdateCronExpression.Click += new System.EventHandler(this.btnTesting_Click);
            // 
            // btnNewJob
            // 
            this.btnNewJob.Location = new System.Drawing.Point(12, 320);
            this.btnNewJob.Name = "btnNewJob";
            this.btnNewJob.Size = new System.Drawing.Size(75, 23);
            this.btnNewJob.TabIndex = 5;
            this.btnNewJob.Text = "New Job";
            this.btnNewJob.UseVisualStyleBackColor = true;
            this.btnNewJob.Visible = false;
            this.btnNewJob.Click += new System.EventHandler(this.btnNewJob_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(359, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Cron Expression";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCronExpression
            // 
            this.txtCronExpression.Location = new System.Drawing.Point(465, 321);
            this.txtCronExpression.Name = "txtCronExpression";
            this.txtCronExpression.Size = new System.Drawing.Size(219, 20);
            this.txtCronExpression.TabIndex = 7;
            this.txtCronExpression.Validated += new System.EventHandler(this.txtCronExpression_Validated);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(380, 347);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(333, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://www.freeformatter.com/cron-expression-generator-quartz.html";
            // 
            // btnPauseJob
            // 
            this.btnPauseJob.Enabled = false;
            this.btnPauseJob.Location = new System.Drawing.Point(168, 320);
            this.btnPauseJob.Name = "btnPauseJob";
            this.btnPauseJob.Size = new System.Drawing.Size(75, 23);
            this.btnPauseJob.TabIndex = 9;
            this.btnPauseJob.Text = "Pause Job";
            this.btnPauseJob.UseVisualStyleBackColor = true;
            this.btnPauseJob.Click += new System.EventHandler(this.btnPauseJob_Click);
            // 
            // btnEnableJob
            // 
            this.btnEnableJob.Enabled = false;
            this.btnEnableJob.Location = new System.Drawing.Point(90, 320);
            this.btnEnableJob.Name = "btnEnableJob";
            this.btnEnableJob.Size = new System.Drawing.Size(75, 23);
            this.btnEnableJob.TabIndex = 10;
            this.btnEnableJob.Text = "Enable Job";
            this.btnEnableJob.UseVisualStyleBackColor = true;
            this.btnEnableJob.Click += new System.EventHandler(this.btnEnableJob_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(753, 67);
            this.label2.TabIndex = 11;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Job List";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(326, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Job Trigger";
            // 
            // cronErrorProvider
            // 
            this.cronErrorProvider.ContainerControl = this;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 369);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEnableJob);
            this.Controls.Add(this.btnPauseJob);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtCronExpression);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNewJob);
            this.Controls.Add(this.btnUpdateCronExpression);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lbTrigger);
            this.Controls.Add(this.lbJobs);
            this.Controls.Add(this.btnGetJobs);
            this.Name = "MainForm";
            this.Text = "Maximus Quartz Job Manager";
            ((System.ComponentModel.ISupportInitialize)(this.cronErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetJobs;
        private System.Windows.Forms.ListBox lbJobs;
        private System.Windows.Forms.ListBox lbTrigger;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Button btnUpdateCronExpression;
        private System.Windows.Forms.Button btnNewJob;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCronExpression;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnPauseJob;
        private System.Windows.Forms.Button btnEnableJob;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider cronErrorProvider;
    }
}