namespace CustomerInvoice
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.masterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chargesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mailBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.letterContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BreakdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakdownCustomerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoicePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoiceHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.stripApplication = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripCompany = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.AmalgamatedInvoiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.masterToolStripMenuItem,
            this.transactionToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.windowsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1183, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // masterToolStripMenuItem
            // 
            this.masterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customerToolStripMenuItem,
            this.clientToolStripMenuItem,
            this.chargesToolStripMenuItem,
            this.mailBodyToolStripMenuItem,
            this.customizationToolStripMenuItem,
            this.letterContentToolStripMenuItem,
            this.toolStripRestore,
            this.exitToolStripMenuItem});
            this.masterToolStripMenuItem.Name = "masterToolStripMenuItem";
            this.masterToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.masterToolStripMenuItem.Text = "Master";
            // 
            // customerToolStripMenuItem
            // 
            this.customerToolStripMenuItem.Name = "customerToolStripMenuItem";
            this.customerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.customerToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.customerToolStripMenuItem.Text = "Customer";
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.clientToolStripMenuItem.Text = "Client";
            // 
            // chargesToolStripMenuItem
            // 
            this.chargesToolStripMenuItem.Name = "chargesToolStripMenuItem";
            this.chargesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.chargesToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.chargesToolStripMenuItem.Text = "Charges";
            // 
            // mailBodyToolStripMenuItem
            // 
            this.mailBodyToolStripMenuItem.Name = "mailBodyToolStripMenuItem";
            this.mailBodyToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.mailBodyToolStripMenuItem.Text = "Mail Body";
            // 
            // customizationToolStripMenuItem
            // 
            this.customizationToolStripMenuItem.Name = "customizationToolStripMenuItem";
            this.customizationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.customizationToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.customizationToolStripMenuItem.Text = "Customization";
            // 
            // letterContentToolStripMenuItem
            // 
            this.letterContentToolStripMenuItem.Name = "letterContentToolStripMenuItem";
            this.letterContentToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.letterContentToolStripMenuItem.Text = "Letter Content";
            // 
            // toolStripRestore
            // 
            this.toolStripRestore.Name = "toolStripRestore";
            this.toolStripRestore.Size = new System.Drawing.Size(277, 26);
            this.toolStripRestore.Text = "Restore database";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // transactionToolStripMenuItem
            // 
            this.transactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BreakdownToolStripMenuItem,
            this.invoiceToolStripMenuItem,
            this.AmalgamatedInvoiceMenuItem,
            this.creditNoteToolStripMenuItem,
            this.resetInvoiceToolStripMenuItem,
            this.breakdownCustomerToolStripMenuItem});
            this.transactionToolStripMenuItem.Name = "transactionToolStripMenuItem";
            this.transactionToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.transactionToolStripMenuItem.Text = "Transaction";
            // 
            // BreakdownToolStripMenuItem
            // 
            this.BreakdownToolStripMenuItem.Name = "BreakdownToolStripMenuItem";
            this.BreakdownToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
            this.BreakdownToolStripMenuItem.Text = "Breakdown";
            // 
            // invoiceToolStripMenuItem
            // 
            this.invoiceToolStripMenuItem.Name = "invoiceToolStripMenuItem";
            this.invoiceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.I)));
            this.invoiceToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
            this.invoiceToolStripMenuItem.Text = "Invoice";
            // 
            // creditNoteToolStripMenuItem
            // 
            this.creditNoteToolStripMenuItem.Name = "creditNoteToolStripMenuItem";
            this.creditNoteToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
            this.creditNoteToolStripMenuItem.Text = "Credit Note";
            // 
            // resetInvoiceToolStripMenuItem
            // 
            this.resetInvoiceToolStripMenuItem.Name = "resetInvoiceToolStripMenuItem";
            this.resetInvoiceToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
            this.resetInvoiceToolStripMenuItem.Text = "Reset Invoice";
            // 
            // breakdownCustomerToolStripMenuItem
            // 
            this.breakdownCustomerToolStripMenuItem.Name = "breakdownCustomerToolStripMenuItem";
            this.breakdownCustomerToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
            this.breakdownCustomerToolStripMenuItem.Text = "Breakdown (Customer-wise)";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.invoicePrintToolStripMenuItem,
            this.invoiceHistoryToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.reportsToolStripMenuItem.Text = "Reports";
            // 
            // invoicePrintToolStripMenuItem
            // 
            this.invoicePrintToolStripMenuItem.Name = "invoicePrintToolStripMenuItem";
            this.invoicePrintToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.invoicePrintToolStripMenuItem.Size = new System.Drawing.Size(283, 26);
            this.invoicePrintToolStripMenuItem.Text = "Export";
            // 
            // invoiceHistoryToolStripMenuItem
            // 
            this.invoiceHistoryToolStripMenuItem.Name = "invoiceHistoryToolStripMenuItem";
            this.invoiceHistoryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.invoiceHistoryToolStripMenuItem.Size = new System.Drawing.Size(283, 26);
            this.invoiceHistoryToolStripMenuItem.Text = "Invoice History";
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(84, 24);
            this.windowsToolStripMenuItem.Text = "&Windows";
            // 
            // statusMain
            // 
            this.statusMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripApplication,
            this.stripVersion,
            this.stripCompany,
            this.stripUser});
            this.statusMain.Location = new System.Drawing.Point(0, 490);
            this.statusMain.Name = "statusMain";
            this.statusMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusMain.Size = new System.Drawing.Size(1183, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // stripApplication
            // 
            this.stripApplication.Name = "stripApplication";
            this.stripApplication.Size = new System.Drawing.Size(1163, 16);
            this.stripApplication.Spring = true;
            this.stripApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stripVersion
            // 
            this.stripVersion.Name = "stripVersion";
            this.stripVersion.Size = new System.Drawing.Size(0, 16);
            // 
            // stripCompany
            // 
            this.stripCompany.Name = "stripCompany";
            this.stripCompany.Size = new System.Drawing.Size(0, 16);
            // 
            // stripUser
            // 
            this.stripUser.Name = "stripUser";
            this.stripUser.Size = new System.Drawing.Size(0, 16);
            // 
            // AmalgamatedInvoiceMenuItem
            // 
            this.AmalgamatedInvoiceMenuItem.Name = "AmalgamatedInvoiceMenuItem";
            this.AmalgamatedInvoiceMenuItem.Size = new System.Drawing.Size(278, 26);
            this.AmalgamatedInvoiceMenuItem.Text = "Amalgamated Invoice";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 512);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Application";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem masterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chargesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoicePrintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invoiceHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem creditNoteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel stripApplication;
        private System.Windows.Forms.ToolStripStatusLabel stripVersion;
        private System.Windows.Forms.ToolStripStatusLabel stripCompany;
        private System.Windows.Forms.ToolStripStatusLabel stripUser;
        private System.Windows.Forms.ToolStripMenuItem BreakdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetInvoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailBodyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripRestore;
        private System.Windows.Forms.ToolStripMenuItem letterContentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breakdownCustomerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AmalgamatedInvoiceMenuItem;
    }
}

