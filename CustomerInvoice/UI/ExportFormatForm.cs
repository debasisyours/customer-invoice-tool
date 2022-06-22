using System;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public enum ExportFormat
    {
        None,
        OldExcel,
        Xero
    }

    public partial class ExportFormatForm : Form
    {
        #region Declaration

        private ExportFormat _selectedFormat = ExportFormat.None;
        private bool _markAsExported = true;

        #endregion

        #region Constructor
        public ExportFormatForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.radioXero.Checked = true;
            this._selectedFormat = ExportFormat.Xero;
            this.chkMark.Checked = true;
            this.AssignEventHandlers();
        }

        #endregion

        #region Methods / Function

        private void AssignEventHandlers()
        {
            this.btnExport.Click += new EventHandler(OnGenerateClick);
            this.btnCancel.Click += new EventHandler(OnCloseClick);
            this.chkMark.CheckedChanged += new EventHandler(OnMarkExported);
        }

        private void OnMarkExported(object sender, EventArgs e)
        {
            this._markAsExported = this.chkMark.Checked;
        }

        private void OnRadioSelection(object sender, EventArgs e)
        {
            if (this.radioExcel.Checked)
            {
                this._selectedFormat = ExportFormat.OldExcel;
            }
            else if (this.radioXero.Checked)
            {
                this._selectedFormat = ExportFormat.Xero;
            }
        }

        private void OnGenerateClick(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public ExportFormat GetFormatSelected { get
            {
                return _selectedFormat;
            } 
        }

        public bool MarkAsExported
        {
            get
            {
                return _markAsExported;
            }
        }

        #endregion
    }
}
