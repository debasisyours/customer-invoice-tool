using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;
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
    public partial class AmalgamatedInvoiceSearchForm : Form
    {
        #region Declaration

        private InvoiceDataSet _invoiceDataSet;

        #endregion

        #region Constructor
        public AmalgamatedInvoiceSearchForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.FormatGrid();
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.AddButton.Click += new EventHandler(OnAddInvoice);
            this.EditButton.Click += new EventHandler(OnEditInvoice);
            this.CloseButton.Click += new EventHandler(OnClose);
            this.ExportButton.Click += new EventHandler(OnExportInvoice);
        }

        private void OnAddInvoice(object sender, EventArgs e)
        {
            using (var invoiceEditForm = new AmalgamatedInvoiceForm())
            {
                var dialogResult = invoiceEditForm.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnExportInvoice(object sender, EventArgs e)
        {
            int selectedId = 0;

            if (this.dgvInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an entry to export.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvInvoices.Focus();
                return;
            }
            else
            {
                selectedId = Convert.ToInt32(this.dgvInvoices.SelectedRows[0].Cells[InvoiceDataSet.IdColumn].Value);
                var customerId = Convert.ToInt32(this.dgvInvoices.SelectedRows[0].Cells[InvoiceDataSet.ClientIdColumn].Value);
                using (var printForm = new PrintForm(selectedId, customerId, false, true, false, true))
                {
                    var dialogResult = printForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        this.FormatGrid();
                    }
                }
            }
        }

        private void OnEditInvoice(object sender, EventArgs e)
        {
            int selectedId = 0;

            if (this.dgvInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an entry to edit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.dgvInvoices.Focus();
                return;
            }
            else
            {
                selectedId = Convert.ToInt32(this.dgvInvoices.SelectedRows[0].Cells[InvoiceDataSet.IdColumn].Value);
                using (var invoiceEditForm = new AmalgamatedInvoiceForm(selectedId))
                {
                    var dialogResult = invoiceEditForm.ShowDialog();
                    if(dialogResult == DialogResult.OK)
                    {
                        this.FormatGrid();
                    }
                }
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormatGrid()
        {
            DataGridViewTextBoxColumn column = null;
            this.dgvInvoices.AllowUserToAddRows = false;
            this.dgvInvoices.AllowUserToDeleteRows = false;
            this.dgvInvoices.AllowUserToResizeRows = false;
            this.dgvInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInvoices.AutoGenerateColumns = false;
            this.dgvInvoices.BackgroundColor = Color.White;
            this.dgvInvoices.MultiSelect = false;
            this.dgvInvoices.RowHeadersVisible = false;
            this.dgvInvoices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvInvoices.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.IdColumn;
            column.DataPropertyName = InvoiceDataSet.IdColumn;
            column.HeaderText = InvoiceDataSet.IdColumn;
            column.FillWeight = 10;
            column.Width = 0;
            column.Visible = false;
            column.ReadOnly = true;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.InvoiceNumberColumn;
            column.DataPropertyName = InvoiceDataSet.InvoiceNumberColumn;
            column.HeaderText = "Invoice Number";
            column.FillWeight = 30;
            column.Width = 70;
            column.Visible = true;
            column.ReadOnly = true;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.InvoiceDateColumn;
            column.DataPropertyName = InvoiceDataSet.InvoiceDateColumn;
            column.HeaderText = "Invoice Date";
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.FillWeight = 20;
            column.Width = 40;
            column.Visible = true;
            column.ReadOnly = true;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.ClientNameColumn;
            column.DataPropertyName = InvoiceDataSet.ClientNameColumn;
            column.HeaderText = "Customer";
            column.FillWeight = 50;
            column.Width = 70;
            column.Visible = true;
            column.ReadOnly = true;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.ClientIdColumn;
            column.DataPropertyName = InvoiceDataSet.ClientIdColumn;
            column.HeaderText = "Customer Id";
            column.FillWeight = 10;
            column.Width = 0;
            column.Visible = false;
            column.ReadOnly = true;
            this.dgvInvoices.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = InvoiceDataSet.NetAmountColumn;
            column.DataPropertyName = InvoiceDataSet.NetAmountColumn;
            column.HeaderText = "Net Amount";
            column.FillWeight = 20;
            column.Width = 30;
            column.Visible = true;
            column.ReadOnly = true;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvInvoices.Columns.Add(column);

            this._invoiceDataSet = DataLayer.PopulateAmalgamatedInvoices(Program.LoggedInCompanyId);
            this.dgvInvoices.DataSource = this._invoiceDataSet.Tables[InvoiceDataSet.TableInvoice];
            this.dgvInvoices.Columns[InvoiceDataSet.IdColumn].Width = 0;
            this.dgvInvoices.Columns[InvoiceDataSet.IdColumn].Visible = false;
            this.dgvInvoices.Columns[InvoiceDataSet.ClientIdColumn].Width = 0;
            this.dgvInvoices.Columns[InvoiceDataSet.ClientIdColumn].Visible = false;
        }

        #endregion
    }
}
