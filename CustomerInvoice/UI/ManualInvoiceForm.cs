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
    public partial class ManualInvoiceForm : Form
    {
        public ManualInvoiceForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.PopulateStaticControls();
            this.AssignEventHandlers();
        }

        #endregion

        #region Custom events

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnCancel);
            this.cboClient.SelectedIndexChanged += new EventHandler(OnClientChanged);
            this.txtDays.TextChanged += new EventHandler(OnChangeDays);
            this.cboCustomer.SelectedIndexChanged += new EventHandler(OnCustomerChanged);
            this.txtExtraAmount.TextChanged += new EventHandler(OnExtraLessEntered);
            this.txtLessAmount.TextChanged += new EventHandler(OnExtraLessEntered);
            this.txtAmount.TextChanged += new EventHandler(OnExtraLessEntered);
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (!CheckData()) return;

            Invoice invoice = new Invoice();
            InvoiceDetail detailItem = null;
            List<InvoiceDetail> detailList = new List<InvoiceDetail>();

            invoice.ClientID = Convert.ToInt32(this.cboClient.SelectedValue);
            invoice.CompanyId = Program.LoggedInCompanyId;
            invoice.EndDate = this.dtpEndDate.Value;
            invoice.InvoiceDate = this.dtpInvoiceDate.Value;
            invoice.InvoiceNumber = this.txtInvoiceNumber.Text;
            invoice.Narration = this.txtNarration.Text;
            invoice.NetAmount = Convert.ToDecimal(this.txtTotalAmount.Text);
            invoice.StartDate = this.dtpStart.Value;
            invoice.MultiMonth = false;

            if (this.cboCustomer.SelectedIndex > 0)
            {
                detailItem = new InvoiceDetail();
                detailItem.ChargeHeadID = DataLayer.GetChargeId(this.txtChargeHead.Text);
                detailItem.CustomerID = Convert.ToInt32(this.cboCustomer.SelectedValue);
                detailItem.Days = Convert.ToInt32(this.txtDays.Text);
                detailItem.ExtraAmount = Convert.ToDecimal(string.IsNullOrEmpty(this.txtExtraAmount.Text) ? "0" : this.txtExtraAmount.Text);
                detailItem.ExtraHead = this.txtExtraHead.Text;
                detailItem.LessAmount = Convert.ToDecimal(string.IsNullOrEmpty(this.txtLessAmount.Text) ? "0" : this.txtLessAmount.Text);
                detailItem.LessHead = this.txtLessHead.Text;
                detailItem.SubTotal = Convert.ToDecimal(this.txtAmount.Text);
                detailItem.TotalAmount = Convert.ToDecimal(this.txtTotalAmount.Text);
                detailItem.WeeklyRate = DataLayer.GetWeeklyRate(Program.LoggedInCompanyId, invoice.ClientID, detailItem.CustomerID);
                detailList.Add(detailItem);
            }
            
            if (DataLayer.SaveManualInvoice(invoice, detailList))
            {
                MessageBox.Show(this, "Manual invoice saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Manual invoice could not be saved please check log file for detail", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool CheckData()
        {
            bool success = true;

            if (string.IsNullOrEmpty(this.txtInvoiceNumber.Text))
            {
                MessageBox.Show(this, "Invoice number cannot be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtInvoiceNumber.Focus();
                return false;
            }

            if (Convert.ToInt32(this.cboClient.SelectedValue) <= 0)
            {
                MessageBox.Show(this, "Client cannot be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboClient.Focus();
                return false;
            }

            if (Convert.ToInt32(this.cboCustomer.SelectedValue) <= 0)
            {
                MessageBox.Show(this, "Customer cannot be left blank", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.cboCustomer.Focus();
                return false;
            }

            if (DataLayer.InvoiceExists(Program.LoggedInCompanyId,this.txtInvoiceNumber.Text))
            {
                MessageBox.Show(this, "Invoice number already exists", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtInvoiceNumber.Focus();
                this.txtInvoiceNumber.SelectAll();
                return false;
            }

            var clientDetail = DataLayer.GetSingleClient(Convert.ToInt32(this.cboClient.SelectedValue));
            if(clientDetail!=null && clientDetail.Rip.HasValue && clientDetail.Rip.Value<this.dtpEndDate.Value)
            {
                if(MessageBox.Show(this, "Selected client is marked as RIP, are you sure to continue?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No)
                {
                    return false;
                }
            }

            return success;
        }

        private void PopulateStaticControls()
        {
            this.txtInvoiceNumber.Text = DataLayer.GenerateInvoiceNumber(Program.LoggedInCompanyId);

            ClientDataSet clientList = DataLayer.PopulateClients(Program.LoggedInCompanyId, false);
            AutoCompleteStringCollection clientSuggest = new AutoCompleteStringCollection();
            foreach (DataRow rowItem in clientList.Tables[ClientDataSet.TableClient].Rows)
            {
                clientSuggest.Add(Convert.ToString(rowItem[ClientDataSet.NameColumn]));
            }
            this.cboClient.DataSource = clientList.Tables[ClientDataSet.TableClient];
            this.cboClient.DisplayMember = ClientDataSet.NameColumn;
            this.cboClient.ValueMember = ClientDataSet.IdColumn;
            this.cboClient.AutoCompleteMode = AutoCompleteMode.None;
            this.cboClient.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.cboClient.AutoCompleteCustomSource = clientSuggest;
            this.cboClient.SelectedIndex = -1;
        }

        private void OnClientChanged(object sender, EventArgs e)
        {
            if (this.cboClient.SelectedValue == null) return;
            int selectedClientId = Convert.ToInt32(this.cboClient.SelectedValue);
            if (selectedClientId > 0)
            {
                CustomerDataSet customers = DataLayer.PopulateCustomerForClient(Program.LoggedInCompanyId, selectedClientId);
                this.cboCustomer.DataSource = customers.Tables[CustomerDataSet.TableCustomer];
                this.cboCustomer.DisplayMember = CustomerDataSet.NameColumn;
                this.cboCustomer.ValueMember = CustomerDataSet.IdColumn;

                if (this.cboCustomer.Items.Count > 0)
                {
                    this.cboCustomer.SelectedIndex = 0;
                }

                if (!string.IsNullOrEmpty(this.txtDays.Text))
                {
                    this.CalculateInvoiceAmount(Convert.ToInt32(this.txtDays.Text));
                }
            }
            else
            {
                this.cboCustomer.DataSource = null;
            }
        }

        private void OnCustomerChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtDays.Text))
            {
                this.CalculateInvoiceAmount(Convert.ToInt32(this.txtDays.Text));
            }
        }

        private void OnExtraLessEntered(object sender, EventArgs e)
        {
            decimal inputValue;
            bool parsed = false;

            parsed = decimal.TryParse((sender as TextBox).Text, out inputValue);

            if (!parsed)
            {
                MessageBox.Show(this, "Non-numeric value not allowed", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                (sender as TextBox).Focus();
                return;
            }

            else
            {
                if (!string.IsNullOrEmpty(this.txtDays.Text))
                {
                    this.CalculateInvoiceAmount(Convert.ToInt32(this.txtDays.Text));
                }
            }
        }

        private void OnChangeDays(object sender, EventArgs e)
        {
            int days;

            bool success = int.TryParse(this.txtDays.Text, out days);
            if (!success)
            {
                MessageBox.Show(this, "Please mention numeric figure here", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtDays.Focus();
                this.txtDays.SelectAll();
                return;
            }
            else
            {
                this.dtpEndDate.Value = this.dtpStart.Value.AddDays(days-1);
                this.CalculateInvoiceAmount(days);
            }
        }

        private void CalculateInvoiceAmount(int invoiceDays)
        {
            int clientId = Convert.ToInt32(this.cboClient.SelectedValue);
            int selectedCustomerId = 0;
            int customerId = 0;
            decimal invoiceAmount = 0;
            decimal extraAmount = 0;
            decimal lessAmount = 0;

            if (clientId <= 0) return;

            if (this.cboCustomer.SelectedIndex > 0)
            {
                selectedCustomerId = Convert.ToInt32(this.cboCustomer.SelectedValue);
            }

            BreakdownDetailDataSet breakdownData = null;
            breakdownData = DataLayer.GetBreakdownForClient(clientId);

            foreach (DataRow rowItem in breakdownData.Tables[BreakdownDetailDataSet.TableBreakdownDetail].Rows)
            {
                this.txtChargeHead.Text = Convert.ToString(rowItem[BreakdownDetailDataSet.ChargeHeadNameColumn]);
                customerId = Convert.ToInt32(rowItem[BreakdownDetailDataSet.CustomerIdColumn]);
                if (selectedCustomerId > 0)
                {
                    if (customerId == selectedCustomerId)
                    {
                        invoiceAmount += (Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn]) / Convert.ToDecimal("7.00")) * invoiceDays;
                    }
                }
                else
                {
                    invoiceAmount += (Convert.ToDecimal(rowItem[BreakdownDetailDataSet.RateColumn])/Convert.ToDecimal("7.00"))*invoiceDays;
                }
            }

            if (string.IsNullOrEmpty(this.txtAmount.Text) || Convert.ToDecimal(this.txtAmount.Text)==0)
            {
                this.txtAmount.Text = invoiceAmount.ToString("N2");
            }
            else
            {
                invoiceAmount = Convert.ToDecimal(this.txtAmount.Text);
            }

            if (!string.IsNullOrEmpty(this.txtExtraAmount.Text)) extraAmount = Convert.ToDecimal(this.txtExtraAmount.Text);
            if (!string.IsNullOrEmpty(this.txtLessAmount.Text)) lessAmount = Convert.ToDecimal(this.txtLessAmount.Text);

            this.txtTotalAmount.Text = ((invoiceAmount + extraAmount) - lessAmount).ToString("N2");
        }

        #endregion
    }
}
