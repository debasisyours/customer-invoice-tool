using CustomerInvoice.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class LetterCustomizationForm : Form
    {
        #region Constructor
        public LetterCustomizationForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.LoadLetterContent();
        }

        #endregion

        #region custom methods

        private void AssignEventHandlers()
        {
            this.btnOK.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnCancel);
        }

        private void LoadLetterContent()
        {
            var globalSettings = DataLayer.PopulateGlobalSettings(Program.LoggedInCompanyId);
            if (globalSettings != null)
            {
                this.txtLetterContent.Text = globalSettings.LetterContent;
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtLetterContent.Text))
            {
                bool success = DataLayer.SaveLetterContent(Program.LoggedInCompanyId, this.txtLetterContent.Text);
                if (success)
                {
                    MessageBox.Show(this, "Letter content has been updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
