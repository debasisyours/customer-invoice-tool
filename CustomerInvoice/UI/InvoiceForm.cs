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
using System.Globalization;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.UI
{
    public partial class InvoiceForm : Form
    {
        public InvoiceForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PopulateClients();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom events

        private void AssignEventHandlers()
        {
            this.chkAll.CheckedChanged += new EventHandler(OnSelectOrDeselect);
            this.btnGenerate.Click += new EventHandler(OnGenerateInvoice);
            this.btnCancel.Click += new EventHandler(OnExit);
            this.txtSearch.TextChanged += new EventHandler(OnTextChange);
        }

        private void PopulateClients()
        {
            ClientDataSet clients = DataLayer.PopulateClients(Program.LoggedInCompanyId, false, false);
            if (clients.Tables[ClientDataSet.TableClient].Rows.Count > 0)
            {
                foreach (DataRow row in clients.Tables[ClientDataSet.TableClient].Rows)
                {
                    this.lstClients.Items.Add(row[ClientDataSet.NameColumn], false);
                }
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            ClientDataSet clients = null;
            if (string.IsNullOrWhiteSpace(this.txtSearch.Text))
            {
                clients = DataLayer.PopulateClients(Program.LoggedInCompanyId, false, false);
            }
            else
            {
                clients = DataLayer.PopulateClients(Program.LoggedInCompanyId, this.txtSearch.Text);
            }
            this.lstClients.Items.Clear();
            if (clients.Tables[ClientDataSet.TableClient].Rows.Count > 0)
            {
                foreach (DataRow row in clients.Tables[ClientDataSet.TableClient].Rows)
                {
                    this.lstClients.Items.Add(row[ClientDataSet.NameColumn], false);
                }
            }
        }

        private void OnSelectOrDeselect(object sender, EventArgs e)
        {
            if (string.Compare(this.chkAll.Text, "Select All", false) == 0)
            {
                for (int loop = 0; loop < this.lstClients.Items.Count; loop++)
                {
                    this.lstClients.SetItemChecked(loop, true);
                }
                this.chkAll.Text = "Clear All";
            }
            else
            {
                for (int loop = 0; loop < this.lstClients.Items.Count; loop++)
                {
                    this.lstClients.SetItemChecked(loop, false);
                }
                this.chkAll.Text = "Select All";
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnGenerateInvoice(object sender, EventArgs e)
        {
            if (!this.IsDataValidated()) return;
            int clientId = 0;
            string clientName = string.Empty;
            int days;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool parseDay = int.TryParse(this.txtDays.Text, out days);
                for (int loop = 0; loop < this.lstClients.Items.Count; loop++)
                {
                    if (this.lstClients.GetItemChecked(loop))
                    {
                        clientName = this.lstClients.Items[loop].ToString();
                        clientId = DataLayer.GetSingleClientFromName(clientName, Program.LoggedInCompanyId).ID;

                        var clientDetail = DataLayer.GetSingleClient(clientId);
                        if (clientDetail != null)
                        {
                            Logger.WriteInformation(string.Format(CultureInfo.CurrentCulture, "Generating invoice for client {0}", clientName));
                            if (DataLayer.GenerateInvoices(clientId, this.dtpInvoiceDate.Value, days, Program.LoggedInCompanyId, string.Empty, this.dtpFrom.Value, this.dtpTo.Value))
                            {
                                Logger.WriteInformation(string.Format(CultureInfo.CurrentCulture, "Invoice generated for client {0}", this.lstClients.Items[loop].ToString()));
                            }
                            else
                            {
                                Logger.WriteInformation(string.Format(CultureInfo.CurrentCulture, "Invoice generation failed for client {0}", this.lstClients.Items[loop].ToString()));
                            }
                        }
                    }
                }
                MessageBox.Show(this,"Invoice generation process completed",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool IsDataValidated()
        {
            bool validated = true;
            StringBuilder ripClientBuilder = new StringBuilder();
            
            int days;
            int selectedClients = 0;
            bool parseDay = int.TryParse(this.txtDays.Text,out days);
            if (!parseDay)
            {
                MessageBox.Show(this, "No of days should be numeric", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtDays.Focus();
                this.txtDays.SelectAll();
                validated = false;
            }
            else
            {
                this.dtpTo.Value = this.dtpFrom.Value.AddDays(days-1);
            }

            List<Client> ripClients = DataLayer.CheckClientWithRip(Program.LoggedInCompanyId, this.dtpTo.Value);
            Client matchingClient = null;

            if (this.lstClients.Items.Count > 0)
            {
                for (int loop = 0; loop < lstClients.Items.Count; loop++)
                {
                    if (this.lstClients.GetItemChecked(loop))
                    {
                        var clientDescription = this.lstClients.Items[loop].ToString();
                        var clientDetail = DataLayer.GetSingleClientFromName(clientDescription, Program.LoggedInCompanyId);

                        if(clientDetail==null)
                        {
                            var test = clientDescription;
                        }

                        matchingClient = ripClients.FirstOrDefault(s => s.ID == clientDetail.ID);

                        if (clientDetail != null && matchingClient==null)
                        {
                            selectedClients += 1;
                        }
                        else if(clientDetail != null && matchingClient!=null)
                        {
                            ripClientBuilder.AppendLine($"{clientDetail.Name}({clientDetail.Code})");
                            this.lstClients.SetItemChecked(loop,false);
                        }
                    }
                }
            }

            if (selectedClients == 0)
            {
                MessageBox.Show(this, "Either no client is selected or selected client(s) are saved as RIP", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.lstClients.Focus();
                validated = false;
            }
            else if (ripClientBuilder.Length > 0)
            {
                if(MessageBox.Show(this, $"The following client(s) are marked as RIP:{Environment.NewLine}{ripClientBuilder.ToString()}{Environment.NewLine} Are you sure to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No)
                {
                    validated = false;
                }
            }

            return validated;
        }

        #endregion
    }
}
