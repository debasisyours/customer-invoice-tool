using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.UI
{
    public partial class ChargeForm : Form
    {
        #region Field declaration

        private ChargeHead _ChargeHead;

        #endregion

        public ChargeForm()
        {
            InitializeComponent();
        }

        public ChargeForm(ChargeHead charge)
        {
            InitializeComponent();
            this._ChargeHead = charge;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            if (this._ChargeHead != null)
            {
                this.PopulateFormControls();
            }
        }

        #endregion

        #region Custom events

        private void PopulateFormControls()
        {
            this.txtChargeHead.Text = this._ChargeHead.Name;
        }

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnExit);
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtChargeHead.Text))
            {
                MessageBox.Show(this, "Charge head name can not be empty", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtChargeHead.Focus();
                return;
            }

            if (this._ChargeHead == null)
            {
                this._ChargeHead = new ChargeHead();
                this._ChargeHead.ID = 0;
                this._ChargeHead.Name = this.txtChargeHead.Text;
            }
            else
            {
                this._ChargeHead.Name = this.txtChargeHead.Text;
            }

            if (DataLayer.SaveCharges(this._ChargeHead, Program.LoggedInCompanyId))
            {
                MessageBox.Show(this, "Charge head saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Charge head could not be saved, please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
