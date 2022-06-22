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

namespace CustomerInvoice
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.PopulateCompanies();
            this.cboCompany.Enabled = false;
            this.lblCompany.Enabled = false;
        }

        #endregion

        #region Custom methods

        private void PopulateCompanies()
        {
            this.cboCompany.DataSource = DataLayer.PopulateCompanies().Tables[CompanyDataSet.TableCompany];
            this.cboCompany.DisplayMember = CompanyDataSet.NameColumn;
            this.cboCompany.ValueMember = CompanyDataSet.IdColumn;
        }

        private void AssignEventHandlers()
        {
            this.btnLogin.Click += new EventHandler(OnLogin);
            this.btnClose.Click += new EventHandler(OnCancel);
            this.txtUser.Leave += new EventHandler(OnLeaveUser);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnLogin(object sender, EventArgs e)
        {
            int selectedCompanyId = 0;
            if (string.IsNullOrEmpty(this.txtUser.Text))
            {
                MessageBox.Show(this, "Please enter user name first", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.txtUser.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter password first", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtPassword.Focus();
                return;
            }
            if (this.cboCompany.Items.Count == 0)
            {
                MessageBox.Show(this, "Please create company first", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            selectedCompanyId = Convert.ToInt32(this.cboCompany.SelectedValue);

            if (!DataLayer.IsUserAuthenticated(this.txtUser.Text, this.txtPassword.Text))
            {
                MessageBox.Show(this, "Either user name / password is not valid OR user is not active", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtUser.Focus();
                return;
            }

            if (!DataLayer.IsUserAuthorized(this.txtUser.Text, this.txtPassword.Text,selectedCompanyId))
            {
                MessageBox.Show(this, "User is not authorized for selected Company", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtUser.Focus();
                return;
            }

            Program.LoggedInCompanyId = selectedCompanyId;
            Program.LoggedUser = this.txtUser.Text;
            using (MainForm mainForm = new MainForm())
            {
                this.Hide();
                mainForm.ShowDialog();
            }
        }

        #endregion

        #region Control events

        private void OnLeaveUser(object sender, EventArgs e)
        {
            User currentUser = DataLayer.GetUserSingleFromName(this.txtUser.Text);
            if (currentUser != null)
            {
                List<int> companyIds = DataLayer.GetAssociatedCompanies(currentUser.ID);
                if (companyIds != null && companyIds.Count > 0)
                {
                    this.cboCompany.SelectedValue = companyIds[0];
                    if (companyIds.Count > 1)
                        this.cboCompany.Enabled = true;
                    else
                        this.cboCompany.Enabled = false;
                }
            }
        }

        #endregion
    }
}
