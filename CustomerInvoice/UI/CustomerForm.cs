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
    public partial class CustomerForm : Form
    {
        #region Field declaration

        private Customer _Customer;
        private bool _InvokeFromPreviousNext = false;

        #endregion

        public CustomerForm()
        {
            InitializeComponent();
        }

        public CustomerForm(Customer customer)
        {
            InitializeComponent();
            this._Customer = customer;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);
            this.PopulateFormControls();
            this.AssignEventHandlers();

            if (this._Customer == null)
            {
                this.btnPrevious.Enabled = false;
                this.btnNext.Enabled = false;
            }
            else
            {
                this.btnPrevious.Enabled = true;
                this.btnNext.Enabled = true;
            }
        }

        #endregion

        #region Custom methods

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.btnPrevious.Click += new EventHandler(OnPrevious);
            this.btnNext.Click+=new EventHandler(OnNext);
        }

        private void PopulateFormControls()
        {
            if (this._Customer != null)
            {
                this.txtCode.Text = this._Customer.Code;
                this.txtName.Text = this._Customer.Name;
                this.txtAddress.Text = this._Customer.Address;
                this.txtEmail.Text = this._Customer.Email;
                this.txtPhone.Text = this._Customer.Phone;
                this.txtSageRef.Text = this._Customer.SageReference;
                this.chkIsFamily.Checked = this._Customer.IsFamily.Value;
                this.chkShowName.Checked = this._Customer.ShowName.Value;
                this.chkExported.Checked = this._Customer.Exported.Value;
                this.chkChanged.Checked = this._Customer.Changed.Value;
                this.chkPrintRequired.Checked = this._Customer.PhysicalPrintRequired;
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCode.Text))
            {
                MessageBox.Show(this, "Customer code can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtName.Text))
            {
                MessageBox.Show(this, "Customer name can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtName.Focus();
                return;
            }

            if (this._Customer == null)
            {
                this._Customer = new Customer();
            }
            this._Customer.Address = this.txtAddress.Text;
            this._Customer.Changed = this.chkChanged.Checked;
            this._Customer.Code = this.txtCode.Text;
            this._Customer.Email = this.txtEmail.Text;
            this._Customer.Phone = this.txtPhone.Text;
            this._Customer.Exported = this.chkExported.Checked;
            this._Customer.IsFamily = this.chkIsFamily.Checked;
            this._Customer.Name = this.txtName.Text;
            this._Customer.SageReference = this.txtName.Text;
            this._Customer.ShowName = this.chkShowName.Checked;
            this._Customer.PhysicalPrintRequired = this.chkPrintRequired.Checked;

            if (DataLayer.SaveCustomer(this._Customer, Program.LoggedInCompanyId))
            {
                if (!this._InvokeFromPreviousNext)
                {
                    MessageBox.Show(this, "Customer saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(this, "Customer could not be saved, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Previous and Next functionality

        private void OnPrevious(object sender, EventArgs e)
        {
            Customer newCustomer = null;
            this._InvokeFromPreviousNext = true;
            this.btnOK.PerformClick();
            this._InvokeFromPreviousNext = false;
            if (this._Customer != null)
            {
                int currentId = this._Customer.ID;
                newCustomer = DataLayer.GetPreviousCustomer(this._Customer.ID);
                if (newCustomer != null)
                {
                    this._Customer = newCustomer;
                    this.PopulateFormControls();
                    this.btnNext.Enabled = true;
                }
                else
                {
                    this.btnPrevious.Enabled = false;
                }
            }
        }

        private void OnNext(object sender, EventArgs e)
        {
            Customer newCustomer = null;
            this._InvokeFromPreviousNext = true;
            this.btnOK.PerformClick();
            this._InvokeFromPreviousNext = false;
            if (this._Customer != null)
            {
                int currentId = this._Customer.ID;
                newCustomer = DataLayer.GetNextCustomer(this._Customer.ID);
                if (newCustomer != null)
                {
                    this._Customer = newCustomer;
                    this.PopulateFormControls();
                    this.btnPrevious.Enabled = true;
                }
                else
                {
                    this.btnNext.Enabled = false;
                }
            }
        }

        #endregion
    }
}
