using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.UI
{
    public partial class CreditNoteForm : Form
    {
        #region Private Field declaration

        private int _CreditNoteId = 0;
        private int _ClientId = 0;

        #endregion

        public CreditNoteForm()
        {
            InitializeComponent();
        }

        public CreditNoteForm(int creditNoteId)
        {
            InitializeComponent();
            this._CreditNoteId = creditNoteId;
        }

        public void SetClientId(int clientId)
        {
            this._ClientId = clientId;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHanders();
            this.PopulateClients();

            if (this._ClientId > 0)
            {
                this.cboClient.SelectedValue = this._ClientId;
            }

            if (this._CreditNoteId > 0)
                this.PopulateDetails();
            else
                this.txtNoteNumber.Text = DataLayer.GenerateCreditNoteNumber(Program.LoggedInCompanyId);
        }

        #endregion

        #region Custom events

        private void PopulateDetails()
        {
            CreditNote noteDetail = DataLayer.GetSingleCreditNote(this._CreditNoteId);
            if (noteDetail != null)
            {
                this.txtNoteNumber.Text = noteDetail.TransactionNumber;
                this.dtpCreditNote.Value = noteDetail.TransactionDate;
                this.txtAmount.Text = noteDetail.Amount.ToString("N2");
                this.txtDescription.Text = noteDetail.Description;
                this.cboClient.SelectedValue = noteDetail.ClientId;
                if(noteDetail.CustomerId>0) this.cboCustomer.SelectedValue = noteDetail.CustomerId;
            }
        }

        private void AssignEventHanders()
        {
            this.btnCancel.Click += new EventHandler(OnExit);
            this.btnSave.Click += new EventHandler(OnSave);
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnSave(object sender, EventArgs e)
        {
            CreditNote note = null;
            if (string.IsNullOrEmpty(this.txtNoteNumber.Text))
            {
                MessageBox.Show(this, "Credit Note no can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtNoteNumber.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtAmount.Text))
            {
                MessageBox.Show(this, "Credit Note amount can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtAmount.Focus();
                return;
            }
            decimal amount = 0;
            bool success = Decimal.TryParse(this.txtAmount.Text, out amount);
            if (!success)
            {
                MessageBox.Show(this, "Credit Note amount is not valid", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtAmount.Focus();
                return;
            }
            DataRowView selectedClient = (DataRowView)this.cboClient.SelectedItem;
            if (selectedClient == null)
            {
                MessageBox.Show(this, "Client can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboClient.Focus();
                return;
            }
            if (Convert.ToInt32(selectedClient[ClientDataSet.IdColumn]) <= 0)
            {
                MessageBox.Show(this, "Client can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboClient.Focus();
                return;
            }

            DataRowView selectedCustomer = (DataRowView)this.cboCustomer.SelectedItem;
            if (selectedCustomer == null)
            {
                MessageBox.Show(this, "Customer can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboCustomer.Focus();
                return;
            }
            if (Convert.ToInt32(selectedClient[CustomerDataSet.IdColumn]) <= 0)
            {
                MessageBox.Show(this, "Customer can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboCustomer.Focus();
                return;
            }

            if (this._CreditNoteId == 0)
            {
                note = new CreditNote();
                note.Amount = amount;
                note.ClientId = Convert.ToInt32(selectedClient[ClientDataSet.IdColumn]);
                note.CustomerId = Convert.ToInt32(selectedCustomer[CustomerDataSet.IdColumn]);
                note.Description = this.txtDescription.Text;
                note.TransactionNumber = this.txtNoteNumber.Text;
                note.TransactionDate = this.dtpCreditNote.Value;
            }
            else
            {
                note = DataLayer.GetSingleCreditNote(this._CreditNoteId);
                note.TransactionNumber = this.txtNoteNumber.Text;
                note.TransactionDate = this.dtpCreditNote.Value;
                note.Amount = amount;
                note.ClientId = Convert.ToInt32(selectedClient[ClientDataSet.IdColumn]);
                note.CustomerId = Convert.ToInt32(selectedCustomer[CustomerDataSet.IdColumn]);
                note.Description = this.txtDescription.Text;
            }
            if (DataLayer.SaveCreditNote(note, Program.LoggedInCompanyId))
            {
                MessageBox.Show(this, "Credit note saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Credit note could not be saved please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PopulateClients()
        {
            this.cboClient.DataSource = DataLayer.PopulateClients(Program.LoggedInCompanyId, false).Tables[ClientDataSet.TableClient];
            this.cboClient.DisplayMember = ClientDataSet.NameColumn;
            this.cboClient.ValueMember = ClientDataSet.IdColumn;

            this.cboCustomer.DataSource = DataLayer.PopulateCustomers(Program.LoggedInCompanyId).Tables[CustomerDataSet.TableCustomer];
            this.cboCustomer.DisplayMember = CustomerDataSet.NameColumn;
            this.cboCustomer.ValueMember = CustomerDataSet.IdColumn;
        }

        #endregion
    }
}
