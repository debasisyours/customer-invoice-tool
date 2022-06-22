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
    public partial class CreditNoteSearchForm : Form
    {
        #region Private fields

        private CreditNoteDataSet _CreditData = null;

        #endregion

        public CreditNoteSearchForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.FormatGrid();
            this.PopulateClients();
        }

        #endregion

        #region Custom functions

        private void AssignEventHandlers()
        {
            this.btnExit.Click += new EventHandler(OnExit);
            this.cboClient.SelectedIndexChanged += new EventHandler(OnSelectClient);
            this.btnAdd.Click += new EventHandler(OnAddCreditNote);
            this.btnEdit.Click += new EventHandler(OnEditCreditNote);
            this.btnDelete.Click += new EventHandler(OnDeleteCreditNote);
            this.btnPrint.Click += new EventHandler(OnExportCreditNote);
            this.dgvCreditNote.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnGridDoubleClick);
            this.dgvCreditNote.KeyDown += new KeyEventHandler(OnGridKeyPress);
            this.btnExportExcel.Click += new EventHandler(OnExportToExcel);
        }

        private void OnExportToExcel(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvCreditNote, "CreditNoteList", "Amount");
            MessageBox.Show(this, "Credit note list exported successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnAddCreditNote(object sender, EventArgs e)
        {
            using (CreditNoteForm creditForm = new CreditNoteForm())
            {
                creditForm.SetClientId(Convert.ToInt32(this.cboClient.SelectedValue));
                DialogResult result = creditForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnExportCreditNote(object sender, EventArgs e)
        {
            int noteId = 0;
            if (this.dgvCreditNote.SelectedRows.Count == 0) return;
            noteId = Convert.ToInt32(this.dgvCreditNote.SelectedRows[0].Cells[CreditNoteDataSet.IdColumn].Value);
            using (CreditNotePrintForm notePrint = new CreditNotePrintForm(noteId,false))
            {
                notePrint.ShowDialog();
            }
            DataLayer.UpdateCreditNotePrintedByUser(noteId);
        }

        private void OnEditCreditNote(object sender, EventArgs e)
        {
            int noteId = 0;
            if (this.dgvCreditNote.SelectedRows.Count == 0) return;
            noteId = Convert.ToInt32(this.dgvCreditNote.SelectedRows[0].Cells[CreditNoteDataSet.IdColumn].Value);
            CreditNote tmpNote = DataLayer.GetSingleCreditNote(noteId);

            if (tmpNote.Printed)
            {
                MessageBox.Show(this, "Credit note already posted, modification not possible",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            using (CreditNoteForm creditForm = new CreditNoteForm(noteId))
            {
                DialogResult result = creditForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void OnSelectClient(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cboClient.Text)) return;
            CreditNoteDataSet tmpData = new CreditNoteDataSet();
            DataRowView rowValue = (DataRowView)this.cboClient.SelectedItem;

            string filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} = {1}", CreditNoteDataSet.ClientIdColumn, rowValue[ClientDataSet.IdColumn]);
            DataRow[] selectedRow = this._CreditData.Tables[CreditNoteDataSet.TableCreditNote].Select(filterCondition);

            foreach (DataRow row in selectedRow)
            {
                tmpData.Tables[CreditNoteDataSet.TableCreditNote].ImportRow(row);
            }
            this.dgvCreditNote.DataSource = tmpData.Tables[CreditNoteDataSet.TableCreditNote];
        }

        private void OnDeleteCreditNote(object sender, EventArgs e)
        {
            int noteId = 0;
            if (this.dgvCreditNote.SelectedRows.Count == 0) return;
            noteId = Convert.ToInt32(this.dgvCreditNote.SelectedRows[0].Cells[CreditNoteDataSet.IdColumn].Value);

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
                        if (DataLayer.DeleteCreditNote(noteId))
                        {
                            MessageBox.Show(this, "Credit note deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.FormatGrid();
                            return;
                        }
                        else
                        {
                            MessageBox.Show(this, "Credit note could not be deleted please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
        }

        private void PopulateClients()
        {
            this.cboClient.DataSource = DataLayer.PopulateClients(Program.LoggedInCompanyId, false).Tables[ClientDataSet.TableClient];
            this.cboClient.DisplayMember = ClientDataSet.NameColumn;
            this.cboClient.ValueMember = ClientDataSet.IdColumn;
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvCreditNote.AllowUserToAddRows = false;
            this.dgvCreditNote.AllowUserToDeleteRows = false;
            this.dgvCreditNote.AllowUserToResizeRows = false;
            this.dgvCreditNote.AutoGenerateColumns = false;
            this.dgvCreditNote.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCreditNote.BackgroundColor = Color.White;
            this.dgvCreditNote.MultiSelect = false;
            this.dgvCreditNote.ReadOnly = true;
            this.dgvCreditNote.RowHeadersVisible = false;
            this.dgvCreditNote.ScrollBars = ScrollBars.Vertical;
            this.dgvCreditNote.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvCreditNote.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.IdColumn;
            column.FillWeight = 20;
            column.HeaderText = "ID";
            column.Name = CreditNoteDataSet.IdColumn;
            column.ReadOnly = true;
            column.Visible = false;
            column.Width = 20;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.ClientIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Client ID";
            column.Name = CreditNoteDataSet.ClientIdColumn;
            column.ReadOnly = true;
            column.Visible = false;
            column.Width = 20;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.ClientNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Client Name";
            column.Name = CreditNoteDataSet.ClientNameColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.CustomerIdColumn;
            column.FillWeight = 20;
            column.HeaderText = "Customer ID";
            column.Name = CreditNoteDataSet.CustomerIdColumn;
            column.ReadOnly = true;
            column.Visible = false;
            column.Width = 20;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.CustomerNameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Customer Name";
            column.Name = CreditNoteDataSet.CustomerNameColumn;
            column.ReadOnly = true;
            column.Width = 100;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.TransactionNumberColumn;
            column.FillWeight = 80;
            column.HeaderText = "Credit Note No";
            column.Name = CreditNoteDataSet.TransactionNumberColumn;
            column.ReadOnly = true;
            column.Width = 80;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.TransactionDateColumn;
            column.FillWeight = 60;
            column.HeaderText = "Date";
            column.Name = CreditNoteDataSet.TransactionDateColumn;
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.ReadOnly = true;
            column.Width = 60;
            this.dgvCreditNote.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = CreditNoteDataSet.AmountColumn;
            column.FillWeight = 80;
            column.HeaderText = "Amount";
            column.Name = CreditNoteDataSet.AmountColumn;
            column.ReadOnly = true;
            column.Width = 80;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvCreditNote.Columns.Add(column);

            this._CreditData = DataLayer.PopulateCreditNoteData(Program.LoggedInCompanyId);

            this.dgvCreditNote.Columns[CreditNoteDataSet.IdColumn].Visible = false;
            this.dgvCreditNote.Columns[CreditNoteDataSet.ClientIdColumn].Visible = false;
            this.dgvCreditNote.Columns[CreditNoteDataSet.CustomerIdColumn].Visible = false;

            this.dgvCreditNote.DataSource = this._CreditData.Tables[CreditNoteDataSet.TableCreditNote];
        }

        #endregion

        #region Datagridview events

        private void OnGridDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        private void OnGridKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.dgvCreditNote.SelectedRows.Count > 0)
            {
                this.btnEdit.PerformClick();
            }
        }

        #endregion
    }
}
