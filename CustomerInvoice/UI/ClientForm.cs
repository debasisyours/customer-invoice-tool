using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
using CustomerInvoice.Common;

namespace CustomerInvoice.UI
{
    public partial class ClientForm : Form
    {
        #region Field declaration

        private Client _Client;
        private bool _InvokedFromPreviousNext = false;
        private bool _DeletedMode = false;

        #endregion

        public ClientForm()
        {
            InitializeComponent();
        }

        public ClientForm(Client client, bool deletedMode)
        {
            InitializeComponent();
            this._Client = client;
            this._DeletedMode = deletedMode;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);
            this.AssignEventHandlers();
            this.PopulateFormControls();

            if (this._Client == null)
            {
                this.btnPrevious.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else
            {
                this.btnPrevious.Enabled = true;
                this.btnNext.Enabled = true;
                this.btnPrevious.Click += new EventHandler(OnPrevious);
                this.btnNext.Click += new EventHandler(OnNext);
            }

            if (this._DeletedMode)
            {
                this.btnNext.Visible = false;
                this.btnPrevious.Visible = false;
            }
            this.dtpRip.Enabled = this.chkRip.Checked;
        }

        #endregion

        #region Custom methods

        private void PopulateFormControls()
        {
            if (this._Client != null)
            {
                this.txtCode.Text = this._Client.Code;
                this.txtName.Text = this._Client.Name;
                if (this._Client.DateOfBirth.HasValue) this.dtpDOB.Value = this._Client.DateOfBirth.Value;
                if (this._Client.DateOfAdmission.HasValue) this.dtpDOA.Value = this._Client.DateOfAdmission.Value;
                this.txtTotalRate.Text = this._Client.TotalRate.ToString("N2");
                this.txtSageReference.Text = this._Client.SageReference;
                this.txtTheirReference.Text = this._Client.TheirReference;
                this.txtNarrative.Text = this._Client.Narrative;
                this.dtpRip.Value = this._Client.Rip.HasValue? this._Client.Rip.Value: DateTime.Now;
                this.chkRip.Checked = this._Client.Rip.HasValue;
                this.chkNursing.Checked = this._Client.Nursing.HasValue ? this._Client.Nursing.Value : false;
                this.chkSelfFunding.Checked = this._Client.SelfFunding.HasValue ? this._Client.SelfFunding.Value : false;
                this.chkResidential.Checked = this._Client.Residential.HasValue ? this._Client.Residential.Value : false;
            }
        }

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.chkRip.CheckedChanged += new EventHandler(OnCheckRip);
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnCheckRip(object sender, EventArgs e)
        {
            this.dtpRip.Enabled = this.chkRip.Checked;
        }

        private void OnSave(object sender, EventArgs e)
        {
            int clientCode = 0;
            if (string.IsNullOrEmpty(this.txtCode.Text))
            {
                MessageBox.Show(this, "Client code can not be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show(this, "Client name can not be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtName.Focus();
                return;
            }

            bool success = int.TryParse(this.txtCode.Text, out clientCode);
            if (!success)
            {
                MessageBox.Show(this, "Client code must be numeric", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtCode.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.txtTotalRate.Text))
            {
                MessageBox.Show(this, "Total rate can not be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtTotalRate.Focus();
                return;
            }

            decimal rateValue;
            bool parseSucess = Decimal.TryParse(this.txtTotalRate.Text, out rateValue);
            if (!parseSucess)
            {
                MessageBox.Show(this, "Total rate should be numeric", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtTotalRate.Focus();
                return;
            }

            if (this._Client == null) this._Client = new Client();
            this._Client.Code = this.txtCode.Text;
            this._Client.Name = this.txtName.Text;
            this._Client.SageReference = this.txtSageReference.Text;
            this._Client.TheirReference = this.txtTheirReference.Text;
            this._Client.TotalRate = Convert.ToDecimal(this.txtTotalRate.Text);
            this._Client.DateOfAdmission = this.dtpDOA.Value;
            this._Client.DateOfBirth = this.dtpDOB.Value;
            this._Client.Narrative = this.txtNarrative.Text;
            this._Client.IsDeleted = this._DeletedMode;
            this._Client.Rip = this.chkRip.Checked ? this.dtpRip.Value : (DateTime?)null;
            this._Client.Nursing = this.chkNursing.Checked;
            this._Client.SelfFunding = this.chkSelfFunding.Checked;
            this._Client.Residential = this.chkResidential.Checked;
            if (DataLayer.SaveClient(this._Client, Program.LoggedInCompanyId))
            {
                if (!this._InvokedFromPreviousNext)
                {
                    MessageBox.Show(this, "Client saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(this, "Client could not be saved, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Previous and Next functionality

        private void OnPrevious(object sender, EventArgs e)
        {
            Client tmpClient = null;
            this._InvokedFromPreviousNext = true;
            this.btnOK.PerformClick();
            this._InvokedFromPreviousNext = false;
            if (this._Client != null)
            {
                tmpClient = DataLayer.GetPreviousClient(this._Client.ID);
                if (tmpClient == null)
                {
                    this.btnPrevious.Enabled = false;
                }
                else
                {
                    this._Client = tmpClient;
                    this.PopulateFormControls();
                    this.btnNext.Enabled = true;
                }
            }
        }

        private void OnNext(object sender, EventArgs e)
        {
            Client tmpClient = null;
            this._InvokedFromPreviousNext = true;
            this.btnOK.PerformClick();
            this._InvokedFromPreviousNext = false;
            if (this._Client != null)
            {
                tmpClient = DataLayer.GetNextClient(this._Client.ID);
                if (tmpClient == null)
                {
                    this.btnNext.Enabled = false;
                }
                else
                {
                    this._Client = tmpClient;
                    this.PopulateFormControls();
                    this.btnPrevious.Enabled = true;
                }
            }
        }

        #endregion
    }
}
