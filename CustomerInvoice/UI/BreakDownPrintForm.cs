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
using Microsoft.Reporting.WinForms;
using CustomerInvoice.Common;
using CustomerInvoice.Data;
using CustomerInvoice.Data.DataSets;

namespace CustomerInvoice.UI
{
    public partial class BreakDownPrintForm : Form
    {
        #region Private Field declaration

        private BreakDownDataSet _BreakDownData = null;

        #endregion

        public BreakDownPrintForm()
        {
            InitializeComponent();
        }

        public BreakDownPrintForm(BreakDownDataSet breakdownData)
        {
            InitializeComponent();
            Company tmpCompany = DataLayer.GetCompanySingle(Program.LoggedInCompanyId);
            this._BreakDownData = breakdownData;
            this.viewerReport.LocalReport.ReportPath = "~/../../../Reports/BreakDownReport.rdlc";
            this.viewerReport.LocalReport.DataSources.Add(new ReportDataSource("DataSetReport", this._BreakDownData.Tables[BreakDownDataSet.TableBreakDown]));
            this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterCompanyName", tmpCompany.Name));
            this.viewerReport.LocalReport.SetParameters(new ReportParameter("ParameterCompanyAddress", tmpCompany.Address));
            this.viewerReport.RefreshReport();
        }

        private void BreakDownPrintForm_Load(object sender, EventArgs e)
        {
            
            this.btnClose.Click += new EventHandler(OnClose);
        }

        #region Custom methods

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
