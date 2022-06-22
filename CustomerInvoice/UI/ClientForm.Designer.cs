namespace CustomerInvoice.UI
{
    partial class ClientForm
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
            this.gbDetails = new System.Windows.Forms.GroupBox();
            this.txtNarrative = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTheirReference = new System.Windows.Forms.TextBox();
            this.txtSageReference = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTotalRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpDOA = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.chkRip = new System.Windows.Forms.CheckBox();
            this.dtpRip = new System.Windows.Forms.DateTimePicker();
            this.gbDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDetails
            // 
            this.gbDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDetails.Controls.Add(this.dtpRip);
            this.gbDetails.Controls.Add(this.chkRip);
            this.gbDetails.Controls.Add(this.label9);
            this.gbDetails.Controls.Add(this.txtNarrative);
            this.gbDetails.Controls.Add(this.label8);
            this.gbDetails.Controls.Add(this.label7);
            this.gbDetails.Controls.Add(this.txtTheirReference);
            this.gbDetails.Controls.Add(this.txtSageReference);
            this.gbDetails.Controls.Add(this.label6);
            this.gbDetails.Controls.Add(this.txtTotalRate);
            this.gbDetails.Controls.Add(this.label5);
            this.gbDetails.Controls.Add(this.dtpDOA);
            this.gbDetails.Controls.Add(this.label4);
            this.gbDetails.Controls.Add(this.dtpDOB);
            this.gbDetails.Controls.Add(this.label3);
            this.gbDetails.Controls.Add(this.txtName);
            this.gbDetails.Controls.Add(this.label2);
            this.gbDetails.Controls.Add(this.txtCode);
            this.gbDetails.Controls.Add(this.label1);
            this.gbDetails.Location = new System.Drawing.Point(7, 1);
            this.gbDetails.Margin = new System.Windows.Forms.Padding(4);
            this.gbDetails.Name = "gbDetails";
            this.gbDetails.Padding = new System.Windows.Forms.Padding(4);
            this.gbDetails.Size = new System.Drawing.Size(879, 444);
            this.gbDetails.TabIndex = 0;
            this.gbDetails.TabStop = false;
            this.gbDetails.Text = "Client Information";
            // 
            // txtNarrative
            // 
            this.txtNarrative.Location = new System.Drawing.Point(133, 272);
            this.txtNarrative.Margin = new System.Windows.Forms.Padding(4);
            this.txtNarrative.MaxLength = 500;
            this.txtNarrative.Multiline = true;
            this.txtNarrative.Name = "txtNarrative";
            this.txtNarrative.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNarrative.Size = new System.Drawing.Size(736, 129);
            this.txtNarrative.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 275);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 16);
            this.label8.TabIndex = 14;
            this.label8.Text = "Narrative";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 245);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 16);
            this.label7.TabIndex = 13;
            this.label7.Text = "Their reference";
            // 
            // txtTheirReference
            // 
            this.txtTheirReference.Location = new System.Drawing.Point(133, 240);
            this.txtTheirReference.Margin = new System.Windows.Forms.Padding(4);
            this.txtTheirReference.MaxLength = 50;
            this.txtTheirReference.Name = "txtTheirReference";
            this.txtTheirReference.Size = new System.Drawing.Size(192, 22);
            this.txtTheirReference.TabIndex = 12;
            // 
            // txtSageReference
            // 
            this.txtSageReference.Location = new System.Drawing.Point(133, 204);
            this.txtSageReference.Margin = new System.Windows.Forms.Padding(4);
            this.txtSageReference.MaxLength = 50;
            this.txtSageReference.Name = "txtSageReference";
            this.txtSageReference.Size = new System.Drawing.Size(192, 22);
            this.txtSageReference.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 209);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 16);
            this.label6.TabIndex = 10;
            this.label6.Text = "Sage reference";
            // 
            // txtTotalRate
            // 
            this.txtTotalRate.Location = new System.Drawing.Point(133, 169);
            this.txtTotalRate.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalRate.Name = "txtTotalRate";
            this.txtTotalRate.Size = new System.Drawing.Size(192, 22);
            this.txtTotalRate.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 174);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Total rate";
            // 
            // dtpDOA
            // 
            this.dtpDOA.CustomFormat = "dd/MMM/yyyy";
            this.dtpDOA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDOA.Location = new System.Drawing.Point(133, 133);
            this.dtpDOA.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDOA.Name = "dtpDOA";
            this.dtpDOA.Size = new System.Drawing.Size(192, 22);
            this.dtpDOA.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date of admission";
            // 
            // dtpDOB
            // 
            this.dtpDOB.CustomFormat = "dd/MMM/yyyy";
            this.dtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDOB.Location = new System.Drawing.Point(133, 97);
            this.dtpDOB.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDOB.Name = "dtpDOB";
            this.dtpDOB.Size = new System.Drawing.Size(192, 22);
            this.dtpDOB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Date of birth";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(133, 62);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(736, 22);
            this.txtName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 66);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(133, 26);
            this.txtCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(192, 22);
            this.txtCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Code";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(680, 453);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(99, 38);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(787, 453);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 38);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevious.Location = new System.Drawing.Point(7, 453);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(99, 38);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(114, 453);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(99, 38);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 413);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "R.I.P.";
            // 
            // chkRip
            // 
            this.chkRip.AutoSize = true;
            this.chkRip.Location = new System.Drawing.Point(13, 413);
            this.chkRip.Name = "chkRip";
            this.chkRip.Size = new System.Drawing.Size(15, 14);
            this.chkRip.TabIndex = 17;
            this.chkRip.UseVisualStyleBackColor = true;
            // 
            // dtpRip
            // 
            this.dtpRip.CustomFormat = "dd/MMM/yyyy";
            this.dtpRip.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRip.Location = new System.Drawing.Point(133, 409);
            this.dtpRip.Margin = new System.Windows.Forms.Padding(4);
            this.dtpRip.Name = "dtpRip";
            this.dtpRip.Size = new System.Drawing.Size(192, 22);
            this.dtpRip.TabIndex = 18;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(896, 495);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add / Edit Client Information";
            this.gbDetails.ResumeLayout(false);
            this.gbDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDetails;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTheirReference;
        private System.Windows.Forms.TextBox txtSageReference;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTotalRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpDOA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox txtNarrative;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpRip;
        private System.Windows.Forms.CheckBox chkRip;
        private System.Windows.Forms.Label label9;
    }
}