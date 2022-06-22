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
using CustomerInvoice.Common;
using CustomerInvoice.Data.DataSets;
using System.Globalization;

namespace CustomerInvoice.UI
{
    public partial class ClientSearchForm : Form
    {

        #region Field declaration

        private ClientDataSet _ClientData = null;
        private ClientDataSet _DeletedClientData = null;

        #endregion

        public ClientSearchForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitializeControls();
            this.AssignEventHandlers();
            this.chkRip.Checked = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.lblTotalRecords.Top = this.Height - 70;
            this.lblTotalRecords.Left = 195;
        }

        #endregion

        #region Private functions

        private void AssignEventHandlers()
        {
            this.btnExit.Click += new EventHandler(OnExit);
            this.btnAdd.Click += new EventHandler(OnAddNewClient);
            this.btnEdit.Click += new EventHandler(OnEditClient);
            this.btnDelete.Click += new EventHandler(OnDeleteClient);
            this.txtSearch.TextChanged += new EventHandler(OnSearchChanged);
            this.btnBreakdown.Click += new EventHandler(OnBreakdownSelected);
            this.dgvClients.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnColumnHeaderClicked);
            this.txtSearch.KeyDown += new KeyEventHandler(OnEnterKeyPress);
            this.dgvClients.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(OnGridDoubleClick);
            this.dgvClients.KeyDown += new KeyEventHandler(OnEnterKeyPress);
            this.btnViewDeleted.Click += new EventHandler(OnDeletedClient);
            this.dgvClients.CellFormatting += new DataGridViewCellFormattingEventHandler(OnFormattingRip);
            this.chkRip.CheckedChanged += new EventHandler(OnShowHideRip);
            this.btnExportExcel.Click += new EventHandler(OnGenerateExcelExport);
            this.btnSendLetter.Click += new EventHandler(OnSendLetter);
        }

        private void OnGenerateExcelExport(object sender, EventArgs e)
        {
            Program.GenerateExcelReport(this.dgvClients, "ClientList", "Total Rate", true);
            MessageBox.Show(this, "Client list exported successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnSendLetter(object sender, EventArgs e)
        {
            var globalSettings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);

            if (globalSettings == null || string.IsNullOrWhiteSpace(globalSettings.LetterContent))
            {
                MessageBox.Show(this, "Letter format is not created for selected Company, cannot proceed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> customerList = new List<string>();

            foreach(DataGridViewRow gridRow in this.dgvClients.Rows)
            {
                if(!string.IsNullOrWhiteSpace(gridRow.Cells[ClientDataSet.CustomerEmailColumn].Value.ToString()))
                {
                    string customerEmail = gridRow.Cells[ClientDataSet.CustomerEmailColumn].Value.ToString();
                    if (customerEmail.Contains(","))
                    {
                        string[] emails = customerEmail.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        emails.ToList().ForEach(s =>
                        {
                            if (!customerList.Contains(s)) customerList.Add(s);
                        });
                    }
                    else
                    {
                        if (!customerList.Contains(customerEmail)) customerList.Add(customerEmail);
                    }
                }
            }

            using (LetterFormatSelectorForm selector = new LetterFormatSelectorForm(customerList))
            {
                selector.ShowDialog();
            }
        }

        private void OnFormattingRip(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex == this.dgvClients.NewRowIndex) return;

            if (string.Compare(this.dgvClients.Columns[e.ColumnIndex].Name, ClientDataSet.NameColumn, true) == 0)
            {
                DateTime ripValue = Convert.ToDateTime(this.dgvClients.Rows[e.RowIndex].Cells[ClientDataSet.RipColumn].FormattedValue.ToString());
                if (ripValue.Year != 1901)
                {
                    for (int loop = 0; loop < this.dgvClients.Columns.Count; loop++)
                    {
                        this.dgvClients.Rows[e.RowIndex].Cells[loop].Style.BackColor = Color.Yellow;
                    }
                }
            }
        }

        private void InitializeControls()
        {
            this.FormatGrid();
            this.FormatRipGrid();
            this._ClientData = DataLayer.PopulateClients(Program.LoggedInCompanyId, true);
            this.dgvClients.DataSource = this._ClientData.Tables[ClientDataSet.TableClient];
            this.lblTotalRecords.Text = string.Format(CultureInfo.CurrentCulture, "Total Clients: {0}", this._ClientData.Tables[ClientDataSet.TableClient].Rows.Count);

            this._DeletedClientData = DataLayer.PopulateDeletedClients(Program.LoggedInCompanyId);
            this.dgvRip.DataSource = this._DeletedClientData.Tables[ClientDataSet.TableClient];
        }

        private void OnShowHideRip(object sender, EventArgs e)
        {
            this.PopulateClientList();
        }

        private void PopulateClientList()
        {
            if (this.chkRip.Checked)
            {
                this._ClientData = DataLayer.PopulateClientsWithoutRip(Program.LoggedInCompanyId);
                this.dgvClients.DataSource = this._ClientData.Tables[ClientDataSet.TableClient];
                this.lblTotalRecords.Text = string.Format(CultureInfo.CurrentCulture, "Total Clients: {0}", this._ClientData.Tables[ClientDataSet.TableClient].Rows.Count);
            }
            else
            {
                this._ClientData = DataLayer.PopulateClients(Program.LoggedInCompanyId, true);
                this.dgvClients.DataSource = this._ClientData.Tables[ClientDataSet.TableClient];
                this.lblTotalRecords.Text = string.Format(CultureInfo.CurrentCulture, "Total Clients: {0}", this._ClientData.Tables[ClientDataSet.TableClient].Rows.Count);
            }
        }

        private void OnDeletedClient(object sender, EventArgs e)
        {
            using (DeletedClientForm deletedForm = new DeletedClientForm())
            {
                deletedForm.ShowDialog();
            }
        }

        protected void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.AutoGenerateColumns = false;
            this.dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClients.BackgroundColor = Color.White;
            this.dgvClients.MultiSelect = true;
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersVisible = false;
            this.dgvClients.ScrollBars = ScrollBars.Vertical;
            this.dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            this.dgvClients.Columns.Clear();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.IdColumn;
            column.FillWeight = 50;
            column.HeaderText = "ID";
            column.Name = ClientDataSet.IdColumn;
            column.Width = 50;
            column.Visible = false;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.CodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Code";
            column.Name = ClientDataSet.CodeColumn;
            column.Width = 80;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = ClientDataSet.NameColumn;
            column.Width = 100;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.DateOfBirthColumn;
            column.FillWeight = 80;
            column.HeaderText = "Date of Birth";
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.Name = ClientDataSet.DateOfBirthColumn;
            column.Width = 80;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.DateOfAdmissionColumn;
            column.FillWeight = 100;
            column.HeaderText = "Date of Admission";
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.Name = ClientDataSet.DateOfAdmissionColumn;
            column.Width = 100;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.TotalRateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Total Rate";
            column.Name = ClientDataSet.TotalRateColumn;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.SageReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sage Ref";
            column.Name = ClientDataSet.SageReferenceColumn;
            column.Width = 80;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.CustomerCodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Customer(s)";
            column.Name = ClientDataSet.CustomerCodeColumn;
            column.Width = 80;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.RipColumn;
            column.FillWeight = 10;
            column.HeaderText = "RIP";
            column.Name = ClientDataSet.RipColumn;
            column.Width = 10;
            this.dgvClients.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.CustomerEmailColumn;
            column.FillWeight = 120;
            column.HeaderText = "Customer Email(s)";
            column.Name = ClientDataSet.CustomerEmailColumn;
            column.Width = 120;
            this.dgvClients.Columns.Add(column);

            this.dgvClients.Columns[ClientDataSet.IdColumn].Visible = false;
            this.dgvClients.Columns[ClientDataSet.RipColumn].Visible = false;
        }

        protected void FormatRipGrid()
        {
            DataGridViewColumn column = null;

            this.dgvRip.AllowUserToAddRows = false;
            this.dgvRip.AllowUserToDeleteRows = false;
            this.dgvRip.AllowUserToResizeRows = false;
            this.dgvRip.AutoGenerateColumns = false;
            this.dgvRip.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRip.BackgroundColor = Color.White;
            this.dgvRip.MultiSelect = true;
            this.dgvRip.ReadOnly = true;
            this.dgvRip.RowHeadersVisible = false;
            this.dgvRip.ScrollBars = ScrollBars.Vertical;
            this.dgvRip.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dgvRip.Columns.Clear();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.IdColumn;
            column.FillWeight = 50;
            column.HeaderText = "ID";
            column.Name = ClientDataSet.IdColumn;
            column.Width = 50;
            column.Visible = false;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.CodeColumn;
            column.FillWeight = 80;
            column.HeaderText = "Code";
            column.Name = ClientDataSet.CodeColumn;
            column.Width = 80;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = ClientDataSet.NameColumn;
            column.Width = 100;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.DateOfBirthColumn;
            column.FillWeight = 80;
            column.HeaderText = "Date of Birth";
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.Name = ClientDataSet.DateOfBirthColumn;
            column.Width = 80;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.DateOfAdmissionColumn;
            column.FillWeight = 100;
            column.HeaderText = "Date of Admission";
            column.DefaultCellStyle.Format = "dd/MMM/yyyy";
            column.Name = ClientDataSet.DateOfAdmissionColumn;
            column.Width = 100;
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.TotalRateColumn;
            column.FillWeight = 80;
            column.HeaderText = "Total Rate";
            column.Name = ClientDataSet.TotalRateColumn;
            column.Width = 80;
            column.DefaultCellStyle.Format = "#0.00";
            this.dgvRip.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.SageReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sage Ref";
            column.Name = ClientDataSet.SageReferenceColumn;
            column.Width = 80;
            this.dgvRip.Columns.Add(column);

            this.dgvRip.Columns[ClientDataSet.IdColumn].Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnAddNewClient(object sender, EventArgs e)
        {
            DialogResult result;
            using (ClientForm client = new ClientForm())
            {
                result = client.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeControls();
                }
            }
        }

        private void OnEditClient(object sender, EventArgs e)
        {
            DialogResult result;
            Client currentClient = null;
            if(this.dgvClients.SelectedRows.Count==0) return;
            DataGridViewRow selectedRow = this.dgvClients.SelectedRows[0];
            int clientId = Convert.ToInt32(selectedRow.Cells[ClientDataSet.IdColumn].Value);
            currentClient = DataLayer.GetSingleClient(clientId);
            using (ClientForm client = new ClientForm(currentClient,false))
            {
                result = client.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InitializeControls();
                }

                this.dgvClients.ClearSelection();
                DataGridViewRow tmpRow = null;
                foreach (DataGridViewRow rowItem in this.dgvClients.Rows)
                {
                    if (Convert.ToInt32(rowItem.Cells[ClientDataSet.IdColumn].Value) == clientId)
                    {
                        tmpRow = rowItem;
                        break;
                    }
                }

                if (tmpRow != null)
                {
                    this.dgvClients.Rows[tmpRow.Index].Selected = true;
                    this.dgvClients.CurrentCell = tmpRow.Cells[ClientDataSet.NameColumn];
                }
            }
        }

        private void OnDeleteClient(object sender, EventArgs e)
        {
            int clientId = 0;
            bool success = false;
            if (this.dgvClients.SelectedRows.Count == 0) return;
            
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
                        foreach (DataGridViewRow rowItem in this.dgvClients.SelectedRows)
                        {
                            clientId = Convert.ToInt32(rowItem.Cells[ClientDataSet.IdColumn].Value);
                            success = DataLayer.DeleteClient(clientId,Program.LoggedInCompanyId);
                            
                            if(!success)
                            {
                                MessageBox.Show(this, "One or more client(s) could not be deleted please check log file", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        MessageBox.Show(this, "Selected client(s) deleted successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.InitializeControls();
                        return;
                    }
                }
            }
        }

        protected void OnBreakdownSelected(object sender, EventArgs e)
        {
            if (this.dgvClients.SelectedRows.Count == 0) return;
            Client selectedClient = null;
            int clientId = Convert.ToInt32(this.dgvClients.SelectedRows[0].Cells[ClientDataSet.IdColumn].Value);
            selectedClient = DataLayer.GetSingleClient(clientId);
            using (BreakdownForm breakdown = new BreakdownForm(selectedClient))
            {
                DialogResult dialogResult = breakdown.ShowDialog();
                if(dialogResult== DialogResult.OK)
                {
                    this.PopulateClientList();
                }
            }
        }

        #endregion

        #region Grid events

        protected void OnSearchChanged(object sender, EventArgs e)
        {
            DataRow[] selectedRows = null;
            ClientDataSet tmpData = new ClientDataSet();
            string filterCondition = string.Empty;
            
            if (string.Compare(this.lblSearch.Text, "Search on", false) == 0) return;

            if (this.tabMain.SelectedIndex == 0)
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.CodeColumn, this.txtSearch.Text);
                            selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvClients.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.NameColumn, this.txtSearch.Text);
                            selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvClients.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Sage Ref":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.SageReferenceColumn, this.txtSearch.Text);
                            selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvClients.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Customer(s)":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.CustomerCodeColumn, this.txtSearch.Text);
                            selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvClients.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Customer Email(s)":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.CustomerEmailColumn, this.txtSearch.Text);
                            selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvClients.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                }
            }
            else
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.CodeColumn, this.txtSearch.Text);
                            selectedRows = this._DeletedClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.NameColumn, this.txtSearch.Text);
                            selectedRows = this._DeletedClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                    case "Sage Ref":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.SageReferenceColumn, this.txtSearch.Text);
                            selectedRows = this._DeletedClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);
                            foreach (DataRow rowItem in selectedRows)
                            {
                                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
                            }
                            this.dgvRip.DataSource = tmpData.Tables[ClientDataSet.TableClient];
                            break;
                        }
                }
            }
        }

        protected void OnColumnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvClients.Columns[e.ColumnIndex].HeaderText)
            {
                case "Code":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Code");
                        break;
                    }
                case "Name":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Name");
                        break;
                    }
                case "Date of Birth":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Date of Birth");
                        break;
                    }
                case "Date of Admission":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Date of Admission");
                        break;
                    }
                case "Total Rate":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Total Rate");
                        break;
                    }
                case "Sage Ref":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Sage Ref");
                        break;
                    }
                case "Customer(s)":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Customer(s)");
                        break;
                    }
                case "Customer Email(s)":
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Customer Email(s)");
                        break;
                    }
            }
            this.txtSearch.Focus();
        }

        private void OnEnterKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.dgvClients.SelectedRows.Count > 0)
            {
                this.btnEdit.PerformClick();
            }
        }

        private void OnGridDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.btnEdit.PerformClick();
        }

        #endregion
    }
}
