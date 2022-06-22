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
    public partial class ExportListPrintForm : Form
    {
        #region Private declaration

        private InvoiceCsvDataSet _InvoiceData = null;

        #endregion

        public ExportListPrintForm()
        {
            InitializeComponent();
        }

        public ExportListPrintForm(InvoiceCsvDataSet csvData)
        {
            InitializeComponent();
            this._InvoiceData = csvData;
        }

        #region Form Events
        
        protected override void OnLoad(EventArgs e)
        {
            Company tmpCompany = DataLayer.GetCompanySingle(Program.LoggedInCompanyId);
 	        base.OnLoad(e);
            this.AssignEventHandlers();
            this.viewerList.LocalReport.ReportPath = "~/../../../Reports/InvoiceExportList.rdlc";
            this.viewerList.LocalReport.DataSources.Add(new ReportDataSource("DataSetReport", this._InvoiceData.Tables[InvoiceCsvDataSet.TableInvoiceExport]));
            this.viewerList.LocalReport.SetParameters(new ReportParameter("ParameterCompanyName", tmpCompany.Name));
            this.viewerList.LocalReport.SetParameters(new ReportParameter("ParameterAddress", tmpCompany.Address));
            this.viewerList.RefreshReport();
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
    }
}
