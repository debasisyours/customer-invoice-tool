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
    public partial class DeletedClientForm : Form
    {
        #region Private members

        private ClientDataSet _ClientData = null;

        #endregion

        public DeletedClientForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.FormatGrid();
        }

        #endregion

        #region Private methods

        private void AssignEventHandlers()
        {
            this.btnClose.Click += new EventHandler(OnClose);
            this.btnView.Click += new EventHandler(OnView);
            this.txtSearch.TextChanged += new EventHandler(OnSearchTextEntered);
            this.dgvResult.CellMouseClick += new DataGridViewCellMouseEventHandler(OnGridColumnSelected);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnView(object sender, EventArgs e)
        {
            int selectedId = 0;
            if (this.dgvResult.SelectedRows.Count == 0) return;
            selectedId = Convert.ToInt32(this.dgvResult.SelectedRows[0].Cells[ClientDataSet.IdColumn].Value);
            Client selectedClient = DataLayer.GetSingleClient(selectedId);
            DialogResult result;

            using (ClientForm clientDetails = new ClientForm(selectedClient,true))
            {
                result = clientDetails.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.FormatGrid();
                }
            }
        }

        private void FormatGrid()
        {
            DataGridViewColumn column = null;

            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AllowUserToResizeRows = false;
            this.dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResult.AutoGenerateColumns = false;
            this.dgvResult.BackgroundColor = Color.White;
            this.dgvResult.MultiSelect = false;
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvResult.Columns.Clear();

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.IdColumn;
            column.FillWeight = 10;
            column.HeaderText = "Id";
            column.Name = ClientDataSet.IdColumn;
            column.Width = 10;
            this.dgvResult.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.CodeColumn;
            column.FillWeight = 40;
            column.HeaderText = "Code";
            column.Name = ClientDataSet.CodeColumn;
            column.Width = 40;
            this.dgvResult.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.NameColumn;
            column.FillWeight = 100;
            column.HeaderText = "Name";
            column.Name = ClientDataSet.NameColumn;
            column.Width = 100;
            this.dgvResult.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.SageReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Sage Reference";
            column.Name = ClientDataSet.SageReferenceColumn;
            column.Width = 80;
            this.dgvResult.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = ClientDataSet.TheirReferenceColumn;
            column.FillWeight = 80;
            column.HeaderText = "Their Ref";
            column.Name = ClientDataSet.TheirReferenceColumn;
            column.Width = 80;
            this.dgvResult.Columns.Add(column);

            this._ClientData = DataLayer.PopulateDeletedClient(Program.LoggedInCompanyId);
            this.dgvResult.DataSource = this._ClientData.Tables[ClientDataSet.TableClient];
            this.dgvResult.Columns[ClientDataSet.IdColumn].Visible = false;
        }

        private void OnGridColumnSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (this.dgvResult.Columns[e.ColumnIndex].Name)
            {
                case ClientDataSet.CodeColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Code");
                        break;
                    }
                case ClientDataSet.NameColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Name");
                        break;
                    }
                case ClientDataSet.SageReferenceColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Sage Reference");
                        break;
                    }
                case ClientDataSet.TheirReferenceColumn:
                    {
                        this.lblSearch.Text = string.Format(CultureInfo.CurrentCulture, "Search on {0}", "Their Ref");
                        break;
                    }
            }
            this.txtSearch.Focus();
        }

        private void OnSearchTextEntered(object sender, EventArgs e)
        {
            string filterCondition = string.Empty;
            if(this.lblSearch.Text.Trim().Length>9)
            {
                switch (this.lblSearch.Text.Substring(9).Trim())
                {
                    case "Code":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.CodeColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Name":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.NameColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Sage Reference":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.SageReferenceColumn, this.txtSearch.Text);
                            break;
                        }
                    case "Their Ref":
                        {
                            filterCondition = string.Format(CultureInfo.CurrentCulture, "{0} LIKE '%{1}%'", ClientDataSet.TheirReferenceColumn, this.txtSearch.Text);
                            break;
                        }
                }
            }

            ClientDataSet tmpData = new ClientDataSet();
            DataRow[] selectedRows = this._ClientData.Tables[ClientDataSet.TableClient].Select(filterCondition);

            foreach (DataRow rowItem in selectedRows)
            {
                tmpData.Tables[ClientDataSet.TableClient].ImportRow(rowItem);
            }

            this.dgvResult.DataSource = tmpData.Tables[ClientDataSet.TableClient];
            this.dgvResult.Columns[ClientDataSet.IdColumn].Visible = false;
        }

        #endregion
    }
}
