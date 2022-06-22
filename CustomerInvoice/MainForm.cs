using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerInvoice.UI;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using CustomerInvoice.Data;

namespace CustomerInvoice
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // Call Database update script so that prior to loading of the application, all pending
            // database changes are applied
            DataLayer.UpdateDatabase();

            //Below code is added to map network drives on startup
            GlobalSetting setting = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            Process mapDrive = new Process();
            if (!string.IsNullOrEmpty(setting.InvoiceExportPath))
            {
                mapDrive.StartInfo.FileName = "net.exe";
                mapDrive.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"use {0}", setting.InvoiceExportPath);
                mapDrive.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                mapDrive.Start();
            }

            if (!string.IsNullOrEmpty(setting.CustomerExportPath))
            {
                mapDrive.StartInfo.FileName = "net.exe";
                mapDrive.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"use {0}", setting.CustomerExportPath);
                mapDrive.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                mapDrive.Start();
            }

            if (!string.IsNullOrEmpty(setting.PdfExportPath))
            {
                mapDrive.StartInfo.FileName = "net.exe";
                mapDrive.StartInfo.Arguments = string.Format(CultureInfo.CurrentCulture, @"use {0}", setting.PdfExportPath);
                mapDrive.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                mapDrive.Start();
            }
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = string.Format(CultureInfo.CurrentCulture,"Customer Invoice Ver {0}",version);
            this.stripApplication.Text = Application.ProductName;
            this.stripCompany.Text =string.Format(CultureInfo.CurrentCulture,"Company name : {0}", DataLayer.GetCompanySingle(Program.LoggedInCompanyId).Name);
            this.stripUser.Text = string.Format(CultureInfo.CurrentCulture,"Logged In as {0}",Program.LoggedUser);
            this.stripVersion.Text = string.Format(CultureInfo.CurrentCulture,"Version {0}", Application.ProductVersion);
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion

        #region Custom methods

        private void AssignEventHandlers()
        {
            this.customerToolStripMenuItem.Click += new EventHandler(OnOpenCustomerForm);
            this.clientToolStripMenuItem.Click += new EventHandler(OnOpenClientForm);
            this.chargesToolStripMenuItem.Click += new EventHandler(OnOpenChargesForm);
            this.customizationToolStripMenuItem.Click += new EventHandler(OnOpenSettingsForm);
            this.exitToolStripMenuItem.Click += new EventHandler(OnExitApplication);
            this.invoiceToolStripMenuItem.Click += new EventHandler(OnOpenInvoiceForm);
            this.AmalgamatedInvoiceMenuItem.Click += new EventHandler(OnOpenAmalgamatedInvoiceForm);
            this.creditNoteToolStripMenuItem.Click += new EventHandler(OnOpenCreditNoteForm);
            this.invoicePrintToolStripMenuItem.Click += new EventHandler(OnInvoicePrint);
            this.invoiceHistoryToolStripMenuItem.Click += new EventHandler(OnInvoiceHistory);
            this.BreakdownToolStripMenuItem.Click += new EventHandler(OnOpenBreakdown);
            this.resetInvoiceToolStripMenuItem.Click += new EventHandler(OnResetInvoice);
            this.mailBodyToolStripMenuItem.Click += new EventHandler(OnMailBody);
            this.toolStripRestore.Click += new EventHandler(OnRestoreForm);
            this.letterContentToolStripMenuItem.Click += new EventHandler(OnLetterContentForm);
            this.windowsToolStripMenuItem.Click += new EventHandler(OnWindowMenuClick);
            this.breakdownCustomerToolStripMenuItem.Click += new EventHandler(OnOpenBreakdownCustomer);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();
        }

        #endregion

        #region Menu operations

        #region windows menu 

        private void OnWindowMenuClick(object sender, EventArgs e)
        {
            if(Application.OpenForms!=null && Application.OpenForms.Count > 0)
            {
                this.windowsToolStripMenuItem.DropDownItems.Clear();
                foreach(Form form in Application.OpenForms)
                {
                    if (form.Text == "Login" || form.IsMdiContainer) continue;
                    this.windowsToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(form.Text, this.Icon.ToBitmap(), OpenSelectedForm));
                }
                if(this.windowsToolStripMenuItem.DropDownItems.Count>1) this.windowsToolStripMenuItem.ShowDropDown();
            }
            else
            {
                this.windowsToolStripMenuItem.DropDownItems.Clear();
            }
        }

        private void OpenSelectedForm(object sender, EventArgs e)
        {
            foreach(Form form in Application.OpenForms)
            {
                if(form.Text == (sender as ToolStripMenuItem).Text)
                {
                    form.Activate();
                    form.BringToFront();
                }
            }
        }

        #endregion

        #region Master Menu

        protected void OnOpenCustomerForm(object sender, EventArgs e)
        {
            CustomerSearchForm form = new CustomerSearchForm();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        protected void OnOpenClientForm(object sender, EventArgs e)
        {
            ClientSearchForm clientForm = new ClientSearchForm();
            clientForm.MdiParent = this;
            clientForm.WindowState = FormWindowState.Maximized;
            clientForm.Show();
        }

        protected void OnOpenChargesForm(object sender, EventArgs e)
        {
            ChargeSearchForm chargeForm = new ChargeSearchForm();
            chargeForm.MdiParent=this;
            chargeForm.Show();
        }

        protected void OnOpenSettingsForm(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.MdiParent = this;
            settingsForm.Show();
        }

        private void OnMailBody(object sender, EventArgs e)
        {
            MailBodyForm mailBodyForm = new MailBodyForm();
            mailBodyForm.MdiParent = this;
            mailBodyForm.WindowState = FormWindowState.Maximized;
            mailBodyForm.Show();
        }

        private void OnRestoreForm(object sender, EventArgs e)
        {
            RestoreForm restoreForm = new RestoreForm();
            restoreForm.MdiParent = this;
            restoreForm.Show();
        }

        private void OnLetterContentForm(object sender, EventArgs e)
        {
            LetterCustomizationForm letterForm = new LetterCustomizationForm();
            letterForm.MdiParent = this;
            letterForm.WindowState = FormWindowState.Maximized;
            letterForm.Show();
        }

        protected void OnExitApplication(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure to exit the application", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        #endregion

        #region Transaction Menu

        private void OnOpenInvoiceForm(object sender, EventArgs e)
        {
            InvoiceSearchForm invoiceForm = new InvoiceSearchForm();
            invoiceForm.MdiParent = this;
            invoiceForm.WindowState = FormWindowState.Maximized;
            invoiceForm.Show();
        }

        private void OnOpenAmalgamatedInvoiceForm(object sender, EventArgs e)
        {
            AmalgamatedInvoiceSearchForm invoiceForm = new AmalgamatedInvoiceSearchForm();
            invoiceForm.MdiParent = this;
            invoiceForm.WindowState = FormWindowState.Maximized;
            invoiceForm.Show();
        }

        private void OnOpenCreditNoteForm(object sender, EventArgs e)
        {
            CreditNoteSearchForm creditForm = new CreditNoteSearchForm();
            creditForm.MdiParent = this;
            creditForm.WindowState = FormWindowState.Maximized;
            creditForm.Show();
        }

        private void OnOpenBreakdown(object sender, EventArgs e)
        {
            BreakdownEditForm breakDown = new BreakdownEditForm();
            breakDown.MdiParent = this;
            breakDown.WindowState = FormWindowState.Maximized;
            breakDown.Show();
        }

        private void OnOpenBreakdownCustomer(object sender, EventArgs e)
        {
            CustomerBreakdownForm breakDown = new CustomerBreakdownForm();
            breakDown.MdiParent = this;
            breakDown.WindowState = FormWindowState.Maximized;
            breakDown.Show();
        }

        private void OnResetInvoice(object sender, EventArgs e)
        {
            InvoiceResetForm resetInvoice = new InvoiceResetForm();
            resetInvoice.MdiParent = this;
            resetInvoice.Show();
        }

        #endregion

        #region Reports Menu

        private void OnInvoicePrint(object sender, EventArgs e)
        {
            InvoiceExportForm export = new InvoiceExportForm();
            export.MdiParent = this;
            export.WindowState = FormWindowState.Maximized;
            export.Show();
        }

        private void OnInvoiceHistory(object sender, EventArgs e)
        {
            InvoiceHistoryForm history = new InvoiceHistoryForm();
            history.MdiParent = this;
            history.WindowState = FormWindowState.Maximized;
            history.Show();
        }

        #endregion

        #endregion
    }
}
