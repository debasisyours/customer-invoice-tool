namespace CustomerInvoice.UI
{
    partial class InvoiceResetForm
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
            this.gbInvoice = new System.Windows.Forms.GroupBox();
            this.txtInvoice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDeleteAdjust = new System.Windows.Forms.Button();
            this.gbInvoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInvoice
            // 
            this.gbInvoice.Controls.Add(this.txtInvoice);
            this.gbInvoice.Controls.Add(this.label1);
            this.gbInvoice.Location = new System.Drawing.Point(7, 1);
            this.gbInvoice.Name = "gbInvoice";
            this.gbInvoice.Size = new System.Drawing.Size(351, 69);
            this.gbInvoice.TabIndex = 0;
            this.gbInvoice.TabStop = false;
            this.gbInvoice.Text = "Delete Invoice";
            // 
            // txtInvoice
            // 
            this.txtInvoice.Location = new System.Drawing.Point(215, 25);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(130, 20);
            this.txtInvoice.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter last valid invoice number";
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(222, 73);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(65, 32);
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(293, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAdjust
            // 
            this.btnDeleteAdjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAdjust.Location = new System.Drawing.Point(7, 73);
            this.btnDeleteAdjust.Name = "btnDeleteAdjust";
            this.btnDeleteAdjust.Size = new System.Drawing.Size(65, 32);
            this.btnDeleteAdjust.TabIndex = 3;
            this.btnDeleteAdjust.Text = "Reset";
            this.btnDeleteAdjust.UseVisualStyleBackColor = true;
            // 
            // InvoiceResetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 111);
            this.Controls.Add(this.btnDeleteAdjust);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.gbInvoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InvoiceResetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset Invoice Number";
            this.gbInvoice.ResumeLayout(false);
            this.gbInvoice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbInvoice;
        private System.Windows.Forms.TextBox txtInvoice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDeleteAdjust;
    }
}