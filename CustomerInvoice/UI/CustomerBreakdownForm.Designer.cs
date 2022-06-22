namespace CustomerInvoice.UI
{
    partial class CustomerBreakdownForm
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
            this.selectGroup = new System.Windows.Forms.GroupBox();
            this.GoButton = new System.Windows.Forms.Button();
            this.customerDropDown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.detailsGroup = new System.Windows.Forms.GroupBox();
            this.dgvBreakdown = new System.Windows.Forms.DataGridView();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.txtSumTotal = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.filterGroup = new System.Windows.Forms.GroupBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.selectGroup.SuspendLayout();
            this.detailsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBreakdown)).BeginInit();
            this.filterGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectGroup
            // 
            this.selectGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectGroup.Controls.Add(this.GoButton);
            this.selectGroup.Controls.Add(this.customerDropDown);
            this.selectGroup.Controls.Add(this.label1);
            this.selectGroup.Location = new System.Drawing.Point(9, 3);
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Size = new System.Drawing.Size(980, 70);
            this.selectGroup.TabIndex = 0;
            this.selectGroup.TabStop = false;
            this.selectGroup.Text = "Customer Selection";
            // 
            // GoButton
            // 
            this.GoButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.GoButton.Location = new System.Drawing.Point(872, 25);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(102, 37);
            this.GoButton.TabIndex = 3;
            this.GoButton.Text = "Go";
            this.GoButton.UseVisualStyleBackColor = true;
            // 
            // customerDropDown
            // 
            this.customerDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customerDropDown.FormattingEnabled = true;
            this.customerDropDown.Location = new System.Drawing.Point(179, 30);
            this.customerDropDown.Name = "customerDropDown";
            this.customerDropDown.Size = new System.Drawing.Size(687, 28);
            this.customerDropDown.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Customer";
            // 
            // detailsGroup
            // 
            this.detailsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsGroup.Controls.Add(this.dgvBreakdown);
            this.detailsGroup.Location = new System.Drawing.Point(9, 148);
            this.detailsGroup.Name = "detailsGroup";
            this.detailsGroup.Size = new System.Drawing.Size(980, 325);
            this.detailsGroup.TabIndex = 1;
            this.detailsGroup.TabStop = false;
            this.detailsGroup.Text = "Breakdowns for Customer";
            // 
            // dgvBreakdown
            // 
            this.dgvBreakdown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBreakdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBreakdown.Location = new System.Drawing.Point(3, 23);
            this.dgvBreakdown.Name = "dgvBreakdown";
            this.dgvBreakdown.RowHeadersWidth = 51;
            this.dgvBreakdown.RowTemplate.Height = 24;
            this.dgvBreakdown.Size = new System.Drawing.Size(974, 299);
            this.dgvBreakdown.TabIndex = 0;
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Location = new System.Drawing.Point(776, 514);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(102, 37);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(884, 513);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(102, 37);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // txtSumTotal
            // 
            this.txtSumTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSumTotal.Location = new System.Drawing.Point(776, 482);
            this.txtSumTotal.Name = "txtSumTotal";
            this.txtSumTotal.Size = new System.Drawing.Size(210, 27);
            this.txtSumTotal.TabIndex = 4;
            this.txtSumTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.ForeColor = System.Drawing.Color.Maroon;
            this.lblTotal.Location = new System.Drawing.Point(15, 485);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(95, 20);
            this.lblTotal.TabIndex = 5;
            this.lblTotal.Text = "Sum (Rate)";
            // 
            // filterGroup
            // 
            this.filterGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterGroup.Controls.Add(this.txtFilter);
            this.filterGroup.Controls.Add(this.lblFilter);
            this.filterGroup.Location = new System.Drawing.Point(9, 74);
            this.filterGroup.Name = "filterGroup";
            this.filterGroup.Size = new System.Drawing.Size(980, 72);
            this.filterGroup.TabIndex = 6;
            this.filterGroup.TabStop = false;
            this.filterGroup.Text = "Filter";
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(6, 35);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(84, 20);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Search by";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(179, 28);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(795, 27);
            this.txtFilter.TabIndex = 2;
            // 
            // CustomerBreakdownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 562);
            this.Controls.Add(this.filterGroup);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtSumTotal);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.detailsGroup);
            this.Controls.Add(this.selectGroup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CustomerBreakdownForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Breakdown (Customer wise)";
            this.selectGroup.ResumeLayout(false);
            this.selectGroup.PerformLayout();
            this.detailsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBreakdown)).EndInit();
            this.filterGroup.ResumeLayout(false);
            this.filterGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox selectGroup;
        private System.Windows.Forms.ComboBox customerDropDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox detailsGroup;
        private System.Windows.Forms.DataGridView dgvBreakdown;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.TextBox txtSumTotal;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox filterGroup;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
    }
}