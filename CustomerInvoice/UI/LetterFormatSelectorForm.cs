using CustomerInvoice.Common;
using CustomerInvoice.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class LetterFormatSelectorForm : Form
    {
        #region Variables

        private List<string> _customerEmailList = null;

        #endregion

        #region Constructor
        public LetterFormatSelectorForm()
        {
            InitializeComponent();
        }

        public LetterFormatSelectorForm(List<string> emailList)
        {
            InitializeComponent();
            this._customerEmailList = emailList;
        }

        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.InitializeControls();
        }

        #endregion

        #region Custom events

        private void InitializeControls()
        {
            this.radioBrowse.Checked = true;
            this.radioSettings.Checked = false;

            this.lblBrowse.Enabled = true;
            this.txtFileName.Enabled = true;
            this.btnBrowse.Enabled = true;
        }

        private void OnSettingsSelect(object sender, EventArgs e)
        {
            if (this.radioSettings.Checked)
            {
                this.lblBrowse.Enabled = false;
                this.txtFileName.Enabled = false;
                this.btnBrowse.Enabled = false;
            }
            else
            {
                this.lblBrowse.Enabled = true;
                this.txtFileName.Enabled = true;
                this.btnBrowse.Enabled = true;
            }
        }

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSendLetter);
            this.btnCancel.Click += new EventHandler(OnCancel);
            this.btnBrowse.Click += new EventHandler(OnBrowse);
            this.radioSettings.CheckedChanged += new EventHandler(OnSettingsSelect);
            this.radioBrowse.CheckedChanged += new EventHandler(OnSettingsSelect);
        }

        private void OnBrowse(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select PDF file to be sent",
                Filter = "*.pdf|*.pdf|*.*|*.*",
                FilterIndex = 0,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            }) 
            {
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.txtFileName.Text = dialog.FileName;
                } 
            }
        }

        private void OnSendLetter(object sender, EventArgs e)
        {
            var globalSettings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            bool success = true;

            try
            {
                if (this.radioBrowse.Checked)
                {
                    if (string.IsNullOrWhiteSpace(this.txtFileName.Text))
                    {
                        MessageBox.Show(this, "Please select the file to be sent as attachment.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.btnBrowse.Focus();
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(this.txtSubject.Text))
                    {
                        if (MessageBox.Show(this, "Mail subject is blank, do you want to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                    }

                    for (int loop = 0; loop < this._customerEmailList.Count; loop++)
                    {
                        if (DataLayer.IsCustomerLinkedToActiveClient(this._customerEmailList[loop], Program.LoggedInCompanyId))
                        {
                            Logger.WriteInformation($"SmtpFromAddress: {globalSettings.SmtpFromAddress}, ToAddress: {this._customerEmailList[loop]}, Subject: {this.txtSubject.Text}, Attachment: {this.txtFileName.Text}");
                            success = MailHelper.SendMailOutlook(globalSettings.SmtpFromAddress, this._customerEmailList[loop], this.txtSubject.Text, this.txtFileName.Text, globalSettings.SmtpUser, globalSettings.SmtpPassword, true, string.Empty, true);
                        }
                        if (!success) break;
                    }
                    if (success)
                    {
                        MessageBox.Show(this, "Mails sent successfully to selected customer(s) excluding RIP.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, "Mail could not be sent to all customer(s).", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (this.radioSettings.Checked)
                {
                    if (string.IsNullOrWhiteSpace(globalSettings.LetterContent))
                    {
                        MessageBox.Show(this, "Mail format is not configured, please specify the format first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(this.txtSubject.Text))
                        {
                            if (MessageBox.Show(this, "Mail subject is blank, do you want to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
                        }

                        for (int loop = 0; loop < this._customerEmailList.Count; loop++)
                        {
                            if (DataLayer.IsCustomerLinkedToActiveClient(this._customerEmailList[loop], Program.LoggedInCompanyId))
                            {
                                success = MailHelper.SendMailOutlook(globalSettings.SmtpFromAddress, this._customerEmailList[loop], this.txtSubject.Text, this.txtFileName.Text, globalSettings.SmtpUser, globalSettings.SmtpPassword, true, string.Empty, false);
                            }
                            if (!success) break;
                        }
                        if (success)
                        {
                            MessageBox.Show(this, "Mails sent successfully to selected customer(s) excluding RIP.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, "Mail could not be sent to all customer(s).", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
