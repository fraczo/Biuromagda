using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Net;

namespace SPEmail
{
    public class EmailGenerator
    {

        public static void SendMail(SPWeb web, string from, string to, string subject, string body, bool isBodyHtml, string cc, string bcc)
        {

                StringDictionary headers = new StringDictionary();
                headers.Add("from", from);
                headers.Add("to", to);
                headers.Add("subject", subject);
                if (!String.IsNullOrEmpty(cc)) headers.Add("cc", cc);
                if (!String.IsNullOrEmpty(bcc)) headers.Add("bcc", bcc);
                headers.Add("content-type", "text/html");
                SPUtility.SendEmail(web, headers, body);
       

        }

        public static void SendMailWithAttachment(SPListItem item, string from, string fromName, string to, string toName, string subject, string body, bool isBodyHtml, string cc, string bcc)
        {

            MailMessage message = new MailMessage();
            SPList list = item.ParentList;
            message.From = new MailAddress(from, fromName);
            message.To.Add(new MailAddress(to, toName));
            message.CC.Add(new MailAddress(cc));
            message.Bcc.Add(new MailAddress(bcc));
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.Subject = subject;

            SendMailWithAttachment(item, message);
        }

        public static void SendMailWithAttachment(SPListItem item, string from, string to, string subject, string body, bool isBodyHtml, string cc, string bcc)
        {

                MailMessage message = new MailMessage();
                SPList list = item.ParentList;
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(to));
                message.CC.Add(new MailAddress(cc));
                message.Bcc.Add(new MailAddress(bcc));
                message.IsBodyHtml = isBodyHtml;
                message.Body = body;
                message.Subject = subject;

                SendMailWithAttachment(item, message);
        }

        public static void SendMailWithAttachment(SPListItem item, MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = item.Web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;

            //nazwa witryny
            if (string.IsNullOrEmpty(message.From.Address))
            {
                message.From = new MailAddress(BLL.admSetup.GetValue(item.ParentList.ParentWeb, "EMAIL_BIURA"),
                  item.Web.Title != null ? item.Web.Title : BLL.admSetup.GetValue(item.ParentList.ParentWeb, "EMAIL_NAZWA_FIRMY"));
            }

            for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
            {
                string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
                SPFile file = item.ParentList.ParentWeb.GetFile(url);
                message.Attachments.Add(new Attachment(file.OpenBinaryStream(), file.Name));
            }

            client.Send(message);
        }
        
        public static void SendProcessEndConfirmationMail(string subject, string bodyHtml, SPWeb web, SPListItem item)
        {
            string from = "STAFix24 Robot<noreply@stafix24.pl>";
            string to = new SPFieldUserValue(web, item["Author"].ToString()).User.Email;

            SendMail(web, from, to, subject, bodyHtml, true, string.Empty, string.Empty);

        }


    }
}
