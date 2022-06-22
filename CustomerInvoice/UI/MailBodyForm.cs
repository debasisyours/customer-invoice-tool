using CustomerInvoice.Common;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace CustomerInvoice.UI
{
    public partial class MailBodyForm : Form
    {

        #region Constructor
        public MailBodyForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AssignEventHandlers();
            this.InitializeControls();
        }

        #endregion

        #region Custom functions

        private void InitializeControls()
        {
            string mailBodyFileName = "MailBody.txt";
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Application.ProductName);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, mailBodyFileName);
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.txtMailBody.Text = reader.ReadToEnd();
                    }
                }

                if (string.IsNullOrEmpty(this.txtMailBody.Text))
                {
                    this.txtMailBody.Text = ConfigurationManager.AppSettings["MailBody"].Replace("&lt;","<").Replace("&gt;",">");
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        private void AssignEventHandlers()
        {
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel.Click += new EventHandler(OnClose);
        }

        private void OnSave(object sender, EventArgs e)
        {
            string mailBodyFileName = "MailBody.txt";
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Application.ProductName);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, mailBodyFileName);
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(this.txtMailBody.Text);
                    }
                }

                MessageBox.Show(this, "Mail body content saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Mail body content could not be saved.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.WriteLog(ex);
            }
        }

        private void OnClose(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
