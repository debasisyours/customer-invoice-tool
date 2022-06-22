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
    public partial class InvoiceResetForm : Form
    {
        public InvoiceResetForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
        }

        #endregion

        #region Private methods

        private void AssignEventHandlers()
        {
            this.btnReset.Click += new EventHandler(OnDelete);
            this.btnCancel.Click += new EventHandler(OnClose);
            this.btnDeleteAdjust.Click += new EventHandler(OnDeleteAndAdjust);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnDeleteAndAdjust(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This operation will shuffle invoice number(s) to fill the deleted invoice number(s) to adjust the sequences, Are you sure you want to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DataLayer.DeleteInvoiceWithAdjust())
                {
                    MessageBox.Show(this, "Invoice number(s) readjusted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Invoice number(s) adjustment failed please check log file for details", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtInvoice.Text))
            {
                MessageBox.Show(this, "Please specify invoice number to reset", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtInvoice.Focus();
                return;
            }

            int invoiceNumber = 0;
            bool success = int.TryParse(this.txtInvoice.Text, out invoiceNumber);

            if (!success)
            {
                MessageBox.Show(this, "Invoice number should be numeric", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtInvoice.Focus();
                this.txtInvoice.SelectAll();
                return;
            }
            else
            {
                if (DataLayer.ResetInvoice(invoiceNumber))
                {
                    MessageBox.Show(this, "Invoice number reset successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Invoice number could not be reset please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        #endregion
    }
}
