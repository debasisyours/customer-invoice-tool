namespace CustomerInvoice.UI
{
    partial class SettingsForm
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
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.btnBrowsePdf = new System.Windows.Forms.Button();
            this.btnBrowseCustomer = new System.Windows.Forms.Button();
            this.btnBrowseInvoice = new System.Windows.Forms.Button();
            this.txtPdfExportPath = new System.Windows.Forms.TextBox();
            this.txtCustomerExportPath = new System.Windows.Forms.TextBox();
            this.txtInvoiceExportPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbCompany = new System.Windows.Forms.GroupBox();
            this.txtSortCode = new System.Windows.Forms.TextBox();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCompanyAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbSmtp = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtFromAddress = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gbMain.SuspendLayout();
            this.gbCompany.SuspendLayout();
            this.gbSmtp.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMain.Controls.Add(this.btnBrowsePdf);
            this.gbMain.Controls.Add(this.btnBrowseCustomer);
            this.gbMain.Controls.Add(this.btnBrowseInvoice);
            this.gbMain.Controls.Add(this.txtPdfExportPath);
            this.gbMain.Controls.Add(this.txtCustomerExportPath);
            this.gbMain.Controls.Add(this.txtInvoiceExportPath);
            this.gbMain.Controls.Add(this.label3);
            this.gbMain.Controls.Add(this.label2);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Location = new System.Drawing.Point(8, 2);
            this.gbMain.Margin = new System.Windows.Forms.Padding(4);
            this.gbMain.Name = "gbMain";
            this.gbMain.Padding = new System.Windows.Forms.Padding(4);
            this.gbMain.Size = new System.Drawing.Size(876, 156);
            this.gbMain.TabIndex = 0;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Glaobal path settings";
            // 
            // btnBrowsePdf
            // 
            this.btnBrowsePdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowsePdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowsePdf.Location = new System.Drawing.Point(797, 106);
            this.btnBrowsePdf.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowsePdf.Name = "btnBrowsePdf";
            this.btnBrowsePdf.Size = new System.Drawing.Size(71, 34);
            this.btnBrowsePdf.TabIndex = 8;
            this.btnBrowsePdf.Text = "Browse";
            this.btnBrowsePdf.UseVisualStyleBackColor = true;
            // 
            // btnBrowseCustomer
            // 
            this.btnBrowseCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseCustomer.Location = new System.Drawing.Point(797, 64);
            this.btnBrowseCustomer.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseCustomer.Name = "btnBrowseCustomer";
            this.btnBrowseCustomer.Size = new System.Drawing.Size(71, 34);
            this.btnBrowseCustomer.TabIndex = 7;
            this.btnBrowseCustomer.Text = "Browse";
            this.btnBrowseCustomer.UseVisualStyleBackColor = true;
            // 
            // btnBrowseInvoice
            // 
            this.btnBrowseInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseInvoice.Location = new System.Drawing.Point(797, 23);
            this.btnBrowseInvoice.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseInvoice.Name = "btnBrowseInvoice";
            this.btnBrowseInvoice.Size = new System.Drawing.Size(71, 34);
            this.btnBrowseInvoice.TabIndex = 6;
            this.btnBrowseInvoice.Text = "Browse";
            this.btnBrowseInvoice.UseVisualStyleBackColor = true;
            // 
            // txtPdfExportPath
            // 
            this.txtPdfExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPdfExportPath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtPdfExportPath.Location = new System.Drawing.Point(257, 111);
            this.txtPdfExportPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPdfExportPath.MaxLength = 500;
            this.txtPdfExportPath.Name = "txtPdfExportPath";
            this.txtPdfExportPath.ReadOnly = true;
            this.txtPdfExportPath.Size = new System.Drawing.Size(527, 22);
            this.txtPdfExportPath.TabIndex = 5;
            // 
            // txtCustomerExportPath
            // 
            this.txtCustomerExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerExportPath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtCustomerExportPath.Location = new System.Drawing.Point(257, 70);
            this.txtCustomerExportPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCustomerExportPath.MaxLength = 500;
            this.txtCustomerExportPath.Name = "txtCustomerExportPath";
            this.txtCustomerExportPath.ReadOnly = true;
            this.txtCustomerExportPath.Size = new System.Drawing.Size(527, 22);
            this.txtCustomerExportPath.TabIndex = 4;
            // 
            // txtInvoiceExportPath
            // 
            this.txtInvoiceExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInvoiceExportPath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtInvoiceExportPath.Location = new System.Drawing.Point(257, 30);
            this.txtInvoiceExportPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtInvoiceExportPath.MaxLength = 500;
            this.txtInvoiceExportPath.Name = "txtInvoiceExportPath";
            this.txtInvoiceExportPath.ReadOnly = true;
            this.txtInvoiceExportPath.Size = new System.Drawing.Size(527, 22);
            this.txtInvoiceExportPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "PDF file generation path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(221, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Customer export file generation path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Invoice export file generation path";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(735, 511);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 31);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(813, 511);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 31);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbCompany
            // 
            this.gbCompany.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCompany.Controls.Add(this.txtSortCode);
            this.gbCompany.Controls.Add(this.txtAccountNumber);
            this.gbCompany.Controls.Add(this.label8);
            this.gbCompany.Controls.Add(this.label7);
            this.gbCompany.Controls.Add(this.txtAccountName);
            this.gbCompany.Controls.Add(this.label6);
            this.gbCompany.Controls.Add(this.txtCompanyAddress);
            this.gbCompany.Controls.Add(this.label5);
            this.gbCompany.Controls.Add(this.txtCompanyName);
            this.gbCompany.Controls.Add(this.label4);
            this.gbCompany.Location = new System.Drawing.Point(8, 160);
            this.gbCompany.Margin = new System.Windows.Forms.Padding(4);
            this.gbCompany.Name = "gbCompany";
            this.gbCompany.Padding = new System.Windows.Forms.Padding(4);
            this.gbCompany.Size = new System.Drawing.Size(876, 250);
            this.gbCompany.TabIndex = 3;
            this.gbCompany.TabStop = false;
            this.gbCompany.Text = "Company and Account details";
            // 
            // txtSortCode
            // 
            this.txtSortCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSortCode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtSortCode.Location = new System.Drawing.Point(257, 215);
            this.txtSortCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtSortCode.MaxLength = 20;
            this.txtSortCode.Name = "txtSortCode";
            this.txtSortCode.Size = new System.Drawing.Size(529, 22);
            this.txtSortCode.TabIndex = 14;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccountNumber.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtAccountNumber.Location = new System.Drawing.Point(257, 186);
            this.txtAccountNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtAccountNumber.MaxLength = 20;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(529, 22);
            this.txtAccountNumber.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 222);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Sort Code";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 190);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 16);
            this.label7.TabIndex = 11;
            this.label7.Text = "Account Number";
            // 
            // txtAccountName
            // 
            this.txtAccountName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccountName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtAccountName.Location = new System.Drawing.Point(257, 154);
            this.txtAccountName.Margin = new System.Windows.Forms.Padding(4);
            this.txtAccountName.MaxLength = 200;
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(609, 22);
            this.txtAccountName.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 158);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Account Name";
            // 
            // txtCompanyAddress
            // 
            this.txtCompanyAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompanyAddress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtCompanyAddress.Location = new System.Drawing.Point(257, 59);
            this.txtCompanyAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtCompanyAddress.MaxLength = 2000;
            this.txtCompanyAddress.Multiline = true;
            this.txtCompanyAddress.Name = "txtCompanyAddress";
            this.txtCompanyAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCompanyAddress.Size = new System.Drawing.Size(609, 89);
            this.txtCompanyAddress.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Company Address";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtCompanyName.Location = new System.Drawing.Point(257, 27);
            this.txtCompanyName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCompanyName.MaxLength = 500;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(609, 22);
            this.txtCompanyName.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Company Name";
            // 
            // gbSmtp
            // 
            this.gbSmtp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSmtp.Controls.Add(this.txtPassword);
            this.gbSmtp.Controls.Add(this.txtUserName);
            this.gbSmtp.Controls.Add(this.txtFromAddress);
            this.gbSmtp.Controls.Add(this.label11);
            this.gbSmtp.Controls.Add(this.label10);
            this.gbSmtp.Controls.Add(this.label9);
            this.gbSmtp.Location = new System.Drawing.Point(8, 410);
            this.gbSmtp.Margin = new System.Windows.Forms.Padding(4);
            this.gbSmtp.Name = "gbSmtp";
            this.gbSmtp.Padding = new System.Windows.Forms.Padding(4);
            this.gbSmtp.Size = new System.Drawing.Size(876, 94);
            this.gbSmtp.TabIndex = 4;
            this.gbSmtp.TabStop = false;
            this.gbSmtp.Text = "SMTP settings";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtPassword.Location = new System.Drawing.Point(668, 55);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(199, 22);
            this.txtPassword.TabIndex = 17;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtUserName.Location = new System.Drawing.Point(131, 55);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.MaxLength = 100;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(455, 22);
            this.txtUserName.TabIndex = 16;
            // 
            // txtFromAddress
            // 
            this.txtFromAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFromAddress.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtFromAddress.Location = new System.Drawing.Point(131, 23);
            this.txtFromAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtFromAddress.MaxLength = 100;
            this.txtFromAddress.Name = "txtFromAddress";
            this.txtFromAddress.Size = new System.Drawing.Size(736, 22);
            this.txtFromAddress.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(593, 58);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 16);
            this.label11.TabIndex = 12;
            this.label11.Text = "Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 58);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 16);
            this.label10.TabIndex = 11;
            this.label10.Text = "User name";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 27);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 16);
            this.label9.TabIndex = 10;
            this.label9.Text = "From Address";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(895, 550);
            this.Controls.Add(this.gbSmtp);
            this.Controls.Add(this.gbCompany);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Global settings";
            this.gbMain.ResumeLayout(false);
            this.gbMain.PerformLayout();
            this.gbCompany.ResumeLayout(false);
            this.gbCompany.PerformLayout();
            this.gbSmtp.ResumeLayout(false);
            this.gbSmtp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPdfExportPath;
        private System.Windows.Forms.TextBox txtCustomerExportPath;
        private System.Windows.Forms.TextBox txtInvoiceExportPath;
        private System.Windows.Forms.Button btnBrowsePdf;
        private System.Windows.Forms.Button btnBrowseCustomer;
        private System.Windows.Forms.Button btnBrowseInvoice;
        private System.Windows.Forms.GroupBox gbCompany;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCompanyAddress;
        private System.Windows.Forms.TextBox txtSortCode;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbSmtp;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtFromAddress;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
    }
}