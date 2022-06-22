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
    public partial class VerificationForm : Form
    {
        #region Private field declaration

        private bool _Validated = false;

        #endregion
        
        public VerificationForm()
        {
            InitializeComponent();
            this.btnOK.Click += new EventHandler(OnVerify);
        }

        #region Custom events

        protected void OnVerify(object sender, EventArgs e)
        {
            this._Validated = DataLayer.IsUserAuthenticated(Program.LoggedUser, this.txtPassword.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool IsVerified()
        {
            return this._Validated;
        }

        #endregion
    }
}
