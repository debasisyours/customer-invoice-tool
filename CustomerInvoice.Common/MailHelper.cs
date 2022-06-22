using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using Microsoft.Office.Interop.Outlook;
using System.IO;
using SelectPdf;

namespace CustomerInvoice.Common
{
    public sealed class MailHelper
    {
        #region Private constructor

        private MailHelper()
        {
            // Private constructor - left blank intentionally
        }

        #endregion

        #region Static methods

        public static bool SendMail(string fromAddress, string toAddress, string subject, string attachmentFile, string smtpUserName, string smtpPassword)
        {
            bool success = false;
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress);
                message.IsBodyHtml = true;
                message.Sender = new MailAddress(fromAddress);
                message.Subject = subject;
                message.To.Add(new MailAddress(toAddress));
                message.Attachments.Add(new System.Net.Mail.Attachment(attachmentFile, new ContentType("application/pdf")));

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Timeout = 600;
                client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                client.Send(message);
                success = true;
            }
            catch (System.Exception ex)
            {
                Logger.WriteLog(ex);
            }
            return success;
        }

        public static bool SendMailOutlook(string fromAddress, string toAddress, string subject, string attachmentFile, string smtpUserName, string smtpPassword, bool letter=false, string content="", bool browsedFile = false)
        {
            bool success = false;
            Application outlook = new Application();
            MailItem mail = (MailItem)outlook.CreateItem(OlItemType.olMailItem);
            string mailBody=ConfigurationManager.AppSettings["MailBody"];
            string mailBodyFromFile = GetMailBody();

            if (letter)
            {
                mail.HTMLBody = ConfigurationManager.AppSettings["LetterBody"];
                attachmentFile = browsedFile? attachmentFile :GenerateLetter(content);
            }
            else
            {
                mail.HTMLBody = string.IsNullOrWhiteSpace(mailBodyFromFile) ? mailBody : mailBodyFromFile;
            }

            try
            {
                string displayName = "Attachment - PDF";
                int position = (int)mail.Body.Length + 1;
                int attachType = (int)OlAttachmentType.olByValue;
                Microsoft.Office.Interop.Outlook.Attachment attachment = mail.Attachments.Add(attachmentFile, attachType, position, displayName);
                mail.Subject = subject;
                mail.BodyFormat = OlBodyFormat.olFormatHTML;
                Microsoft.Office.Interop.Outlook.Recipients recpients = (Microsoft.Office.Interop.Outlook.Recipients)mail.Recipients;
                Microsoft.Office.Interop.Outlook.Recipient recipient = null;
                if (toAddress.Contains(";"))
                {
                    toAddress.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(s =>
                     {
                         recipient = (Microsoft.Office.Interop.Outlook.Recipient)recpients.Add(s.Trim());
                         recipient.Resolve();
                         mail.Send();
                         recipient = null;
                     });
                }
                else
                {
                    recipient = (Microsoft.Office.Interop.Outlook.Recipient)recpients.Add(toAddress.Trim());
                    recipient.Resolve();
                    mail.Send();
                    recipient = null;
                }
                recpients = null;
                mail = null;
                outlook = null;
                success = true;
            }
            catch (System.Exception ex)
            {
                Logger.WriteLogDetails(ex);
            }
            return success;
        }

        private static string GetMailBody()
        {
            string mailBody = string.Empty;

            string mailBodyFileName = "MailBody.txt";
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "CustomerInvoice");

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, mailBodyFileName);
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        mailBody = reader.ReadToEnd();
                    }
                }
            }
            catch(System.Exception ex)
            {
                Logger.WriteLog(ex);
            }

            return mailBody;
        }

        private static string GenerateLetter(string letterContent)
        {
            string applicationName = "CustomerInvoice";
            
            string pdfFileName = $"LetterContent-{DateTime.Now.Year}-{DateTime.Now.Month.ToString().PadLeft(2, '0')}-{DateTime.Now.Day.ToString().PadLeft(2, '0')}-{DateTime.Now.Hour.ToString().PadLeft(2, '0')}{DateTime.Now.Minute.ToString().PadLeft(2, '0')}{DateTime.Now.Second.ToString().PadLeft(2, '0')}.pdf";
            pdfFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), applicationName, pdfFileName);

            //var pdfGenerated = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(letterContent, PdfSharp.PageSize.A4);

            HtmlToPdf converter = new HtmlToPdf();
            var pdfGenerated = converter.ConvertHtmlString(letterContent);
            pdfGenerated.Save(pdfFileName);
            return pdfFileName;
        }

        #endregion
    }
}
