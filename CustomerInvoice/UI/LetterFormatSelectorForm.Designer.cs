
namespace CustomerInvoice.UI
{
    partial class LetterFormatSelectorForm
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
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblBrowse = new System.Windows.Forms.Label();
            this.radioSettings = new System.Windows.Forms.RadioButton();
            this.radioBrowse = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.gbSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSelection
            // 
            this.gbSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSelection.Controls.Add(this.txtSubject);
            this.gbSelection.Controls.Add(this.label1);
            this.gbSelection.Controls.Add(this.btnBrowse);
            this.gbSelection.Controls.Add(this.txtFileName);
            this.gbSelection.Controls.Add(this.lblBrowse);
            this.gbSelection.Controls.Add(this.radioSettings);
            this.gbSelection.Controls.Add(this.radioBrowse);
            this.gbSelection.Location = new System.Drawing.Point(7, 1);
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.Size = new System.Drawing.Size(638, 152);
            this.gbSelection.TabIndex = 0;
            this.gbSelection.TabStop = false;
            this.gbSelection.Text = "Consent Letter Attachment";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(545, 55);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 34);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtFileName.Location = new System.Drawing.Point(189, 61);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(354, 22);
            this.txtFileName.TabIndex = 3;
            // 
            // lblBrowse
            // 
            this.lblBrowse.AutoSize = true;
            this.lblBrowse.Location = new System.Drawing.Point(29, 64);
            this.lblBrowse.Name = "lblBrowse";
            this.lblBrowse.Size = new System.Drawing.Size(154, 16);
            this.lblBrowse.TabIndex = 2;
            this.lblBrowse.Text = "Select file to be attached";
            // 
            // radioSettings
            // 
            this.radioSettings.AutoSize = true;
            this.radioSettings.Location = new System.Drawing.Point(13, 116);
            this.radioSettings.Name = "radioSettings";
            this.radioSettings.Size = new System.Drawing.Size(169, 20);
            this.radioSettings.TabIndex = 1;
            this.radioSettings.TabStop = true;
            this.radioSettings.Text = "Use format from settings";
            this.radioSettings.UseVisualStyleBackColor = true;
            // 
            // radioBrowse
            // 
            this.radioBrowse.AutoSize = true;
            this.radioBrowse.Location = new System.Drawing.Point(13, 35);
            this.radioBrowse.Name = "radioBrowse";
            this.radioBrowse.Size = new System.Drawing.Size(174, 20);
            this.radioBrowse.TabIndex = 0;
            this.radioBrowse.TabStop = true;
            this.radioBrowse.Text = "Browse to select PDF file";
            this.radioBrowse.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(460, 158);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 34);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(553, 158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 34);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mail subject ref";
            // 
            // txtSubject
            // 
            this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubject.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtSubject.Location = new System.Drawing.Point(189, 87);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(354, 22);
            this.txtSubject.TabIndex = 6;
            // 
            // LetterFormatSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 204);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbSelection);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LetterFormatSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consent Letter Format Selection";
            this.gbSelection.ResumeLayout(false);
            this.gbSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.RadioButton radioSettings;
        private System.Windows.Forms.RadioButton radioBrowse;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblBrowse;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label1;
    }
}