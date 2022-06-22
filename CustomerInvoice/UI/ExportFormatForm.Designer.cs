
namespace CustomerInvoice.UI
{
    partial class ExportFormatForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioXero = new System.Windows.Forms.RadioButton();
            this.radioExcel = new System.Windows.Forms.RadioButton();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkMark = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioXero);
            this.groupBox1.Controls.Add(this.radioExcel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 85);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format selection";
            // 
            // radioXero
            // 
            this.radioXero.AutoSize = true;
            this.radioXero.Location = new System.Drawing.Point(348, 32);
            this.radioXero.Name = "radioXero";
            this.radioXero.Size = new System.Drawing.Size(99, 20);
            this.radioXero.TabIndex = 1;
            this.radioXero.TabStop = true;
            this.radioXero.Text = "Xero Format";
            this.radioXero.UseVisualStyleBackColor = true;
            // 
            // radioExcel
            // 
            this.radioExcel.AutoSize = true;
            this.radioExcel.Location = new System.Drawing.Point(6, 33);
            this.radioExcel.Name = "radioExcel";
            this.radioExcel.Size = new System.Drawing.Size(128, 20);
            this.radioExcel.TabIndex = 0;
            this.radioExcel.TabStop = true;
            this.radioExcel.Text = "Old Excel Format";
            this.radioExcel.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(316, 101);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(86, 37);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Generate";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(408, 101);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 37);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkMark
            // 
            this.chkMark.AutoSize = true;
            this.chkMark.Location = new System.Drawing.Point(18, 112);
            this.chkMark.Name = "chkMark";
            this.chkMark.Size = new System.Drawing.Size(184, 20);
            this.chkMark.TabIndex = 3;
            this.chkMark.Text = "Mark invoices as exported";
            this.chkMark.UseVisualStyleBackColor = true;
            // 
            // ExportFormatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 148);
            this.Controls.Add(this.chkMark);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ExportFormatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export format selection";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioXero;
        private System.Windows.Forms.RadioButton radioExcel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkMark;
    }
}