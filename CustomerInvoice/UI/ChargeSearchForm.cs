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
    public partial class ChargeSearchForm : Form
    {

        #region Field declaration

        private ChargeDataSet _Charges = null;

        #endregion

        public ChargeSearchForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeFormControls();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom functions

        private void InitializeFormControls()
        {
            this.FormatGrid();
            this._Charges = DataLayer.PopulateCharges(Program.LoggedInCompanyId);
            this.dgvCharge.DataSource = this._Charges.Tables[ChargeDataSet.TableChargeHead];
        }

        private void AssignEventHandlers()
        {
            this.btnExit.Click += new EventHandler(OnExit);
            this.btnAdd.Click += new EventHandler(OnAddNewCharge);
            this.btnEdit.Click += new EventHandler(OnEditCharge);
            this.btnDelete.Click += new EventHandler(OnDeleteCharge);
            this.dgvCharge.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnGridDoubleClick);
            this.btnExportExcel.Click += new EventHandler(OnExcelExport);
        }

        private void OnExcelExport(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvCharge, "ChargeList", "");
            MessageBox.Show(this, "Charge list exported successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvCharge.AllowUserToAddRows = false;
            this.dgvCharge.AllowUserToDeleteRows = false;
            this.dgvCharge.AllowUserToResizeRows = false;
            this.dgvCharge.AutoGenerateColumns = false;
            this.dgvCharge.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCharge.BackgroundColor = Color.White;
            this.dgvCharge.MultiSelect = false;
            this.dgvCharge.ReadOnly = false;
            this.dgvCharge.RowHeadersVisible = false;
            this.dgvCharge.ScrollBars = ScrollBars.Vertical;
            this.dgvCharge.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            this.dgvCharge.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ChargeDataSet.IdColumn;
            column.FillWeight = 50;
            column.HeaderText = "ID";
            column.Name = ChargeDataSet.IdColumn;
            column.Width = 0;
            column.Visible = false;
            this.dgvCharge.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ChargeDataSet.NameColumn;
            column.FillWeight = 200;
            column.HeaderText = "Name";
            column.Name = ChargeDataSet.NameColumn;
            column.Width = 200;
            this.dgvCharge.Columns.Add(column);

            this.dgvCharge.Columns[ChargeDataSet.IdColumn].Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnAddNewCharge(object sender, EventArgs e)
        {
            DialogResult result;
            using (ChargeForm charge = new ChargeForm())
            {
                result = charge.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeFormControls();
                }
            }
        }

        private void OnEditCharge(object sender, EventArgs e)
        {
            DialogResult result;
            if (this.dgvCharge.SelectedRows.Count == 0)
                return;

            DataGridViewRow selectedRow = this.dgvCharge.SelectedRows[0];
            int chargeId = Convert.ToInt32(selectedRow.Cells[ChargeDataSet.IdColumn].Value);
            ChargeHead chargeEntry = DataLayer.GetSingleCharge(chargeId);
            using (ChargeForm charge = new ChargeForm(chargeEntry))
            {
                result = charge.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeFormControls();
                }
            }
        }

        private void OnDeleteCharge(object sender, EventArgs e)
        {
            int chargeId = 0;
            if (this.dgvCharge.SelectedRows.Count == 0) return;
            chargeId = Convert.ToInt32(this.dgvCharge.SelectedRows[0].Cells[ChargeDataSet.IdColumn].Value);

            bool verified = false;
            using (VerificationForm verify = new VerificationForm())
            {
                DialogResult result = verify.ShowDialog();
                if (result == DialogResult.OK)
                {
                    verified = verify.IsVerified();
                    if (!verified)
                    {
                        MessageBox.Show(this, "Password verification failed", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (DataLayer.DeleteCharge(chargeId))
                        {
                            MessageBox.Show(this, "Charge deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.FormatGrid();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(this, "Charge could not be deleted please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
        }

        private void OnGridDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        #endregion

    }
}
