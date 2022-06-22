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
using Microsoft.Reporting.WinForms;

namespace CustomerInvoice.UI
{
    public partial class InvoicePrintForm : Form
    {
        #region Private member declaration

        private InvoiceDataSet _InvoiceData=null;

        #endregion

        public InvoicePrintForm()
        {
            InitializeComponent();
        }

        public InvoicePrintForm(InvoiceDataSet invoiceData)
        {
            InitializeComponent();
            this._InvoiceData = invoiceData;
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            Company companyData = DataLayer.GetCompanySingle(Program.LoggedInCompanyId);
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.viewerInvoice.LocalReport.ReportPath = "~/../../../Reports/InvoiceListReport.rdlc";
            this.viewerInvoice.LocalReport.DataSources.Add(new ReportDataSource("DataSetReport", this._InvoiceData.Tables[InvoiceDataSet.TableInvoice]));
            this.viewerInvoice.LocalReport.SetParameters(new ReportParameter("ParameterCompanyName", companyData.Name));
            this.viewerInvoice.LocalReport.SetParameters(new ReportParameter("ParameterCompanyAddress", companyData.Address));
            this.viewerInvoice.RefreshReport();
        }

        #endregion

        #region Custom events

        private void AssignEventHandlers()
        {
            this.btnClose.Click += new EventHandler(OnClose);
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        private void InvoicePrintForm_Load(object sender, EventArgs e)
        {

            this.viewerInvoice.RefreshReport();
        }
    }
}
