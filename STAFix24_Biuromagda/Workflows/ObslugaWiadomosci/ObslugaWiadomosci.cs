using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.Net.Mail;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace Workflows.ObslugaWiadomosci
{
    public sealed partial class ObslugaWiadomosci : SequentialWorkflowActivity
    {
        public ObslugaWiadomosci()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        public MailMessage mail;
        private bool isMailReadyToSend;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void Mail_Setup_ExecuteCode(object sender, EventArgs e)
        {
            mail = new MailMessage();
            isMailReadyToSend = false;

            //From
            if (item["colNadawca"] != null)
            {
                mail.From = new MailAddress(item["colNadawca"].ToString());
            }
            else
            {
                mail.From = new MailAddress(BLL.admSetup.GetValue(item.ParentList.ParentWeb, "EMAIL_BIURA"));
            }

            //To
            if (item["colOdbiorca"] != null && !string.IsNullOrEmpty(item.Title))
            {
                mail.To.Add(new MailAddress(item["colOdbiorca"].ToString()));

                //CC
                bool isKopiaDoNadawcy = item["colKopiaDoNadawcy"] != null ? (bool)item["colKopiaDoNadawcy"] : false;
                if (isKopiaDoNadawcy)
                {
                    mail.CC.Add(new MailAddress(item["colNadawca"].ToString()));
                }

                //BCC
                bool isKopiaDoBiura = item["colKopiaDoBiura"] != null ? (bool)item["colKopiaDoBiura"] : false;
                if (isKopiaDoBiura)
                {
                    mail.Bcc.Add(new MailAddress(BLL.admSetup.GetValue(item.ParentList.ParentWeb, "EMAIL_BIURA")));
                }

                //Subject
                mail.Subject = item.Title;

                //Body
                if (item["colTrescHTML"] != null)
                {
                    string bodyHTML = item["colTrescHTML"].ToString();
                    mail.Body = bodyHTML;
                    mail.IsBodyHtml = true;
                }
                else
                {
                    if (item["colTresc"] != null)
                    {
                        string body = item["colTresc"].ToString();

                        StringBuilder sb = new StringBuilder(BLL.admSetup.GetText(item.Web, "MAIL_TEMPLATE"));
                        sb.Replace(@"___BODY___", body);
                        mail.Body = sb.ToString();
                        mail.IsBodyHtml = true;
                    }
                }

                isMailReadyToSend = true;
            }
        }

        private void Mail_Send_ExecuteCode(object sender, EventArgs e)
        {
            if (isMailReadyToSend)
            {
                bool testMode = true;
                SPEmail.EmailGenerator.SendMailFromMessageQueue(item, mail, testMode);

                //ustaw flagę wysyłki
                item["colCzyWyslana"] = true;
                item["colDataNadania"] = DateTime.Now.ToString();
                item.Update();

            }
        }

        private void isWiadomoscWyslana(object sender, ConditionalEventArgs e)
        {
            e.Result = false;
        }

        #region Helpers

        //public static void SendMailWithAttachment(SPListItem item, string from, string to, string subject, string body, bool isBodyHtml, string cc, string bcc)
        //{

        //    SmtpClient client = new SmtpClient();
        //    client.Host = item.Web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;

        //    //nazwa witryny
        //    from = item.Web.Title != null ? String.Format(@"{0}<{1}>",
        //        item.Web.Title,
        //        from) : from;

        //    MailMessage message = new MailMessage();
        //    SPList list = item.ParentList;
        //    message.From = new MailAddress(from);
        //    message.To.Add(new MailAddress(to));
        //    if (!string.IsNullOrEmpty(cc))
        //    {
        //        message.CC.Add(new MailAddress(cc));
        //    }
        //    if (!string.IsNullOrEmpty(bcc))
        //    {
        //        message.Bcc.Add(new MailAddress(bcc));
        //    }
        //    message.IsBodyHtml = isBodyHtml;
        //    message.Body = body;
        //    message.Subject = subject;

        //    for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
        //    {
        //        string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
        //        SPFile file = list.ParentWeb.GetFile(url);
        //        message.Attachments.Add(new Attachment(file.OpenBinaryStream(), file.Name));
        //    }

        //    client.Send(message);

        //}

        //private void sendEmailToAssignee_MethodInvoking(object sender, EventArgs e)
        //{
        //SPListItem wfItem = onWorkflowActivated1.WorkflowProperties.Item;
        //SPFieldUser assignedTo = (SPFieldUser)wfItem.Fields["Assigned To"];
        //SPFieldUserValue user = (SPFieldUserValue)assignedTo.GetFieldValue(
        //wfItem["Assigned To"].ToString());
        //string assigneeEmail = user.User.Email;
        //sendEmailToAssignee.To = new SPFieldUserValue(item.ParentList.ParentWeb, item["Author"].ToString()).User.Email;
        //sendEmailToAssignee.Subject = "New work order has been created.";
        //sendEmailToAssignee.Body = "Work order number " +
        //onWorkflowActivated1.WorkflowProperties.Item.ID +
        //" has just been created and assigned to you.";
        //sendEmailToAssignee.From = "Maszynka<noreply@stafix24.pl>";

        //SendMailWithAttachment(item, "noreply@stafix24.pl", sendEmailToAssignee.To, "nowy temat", sendEmailToAssignee.Body, true, string.Empty, string.Empty);
        //}


        #endregion

        private void Update_tabKartyKlientów_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void isFlagaWysłanoUstawiona(object sender, ConditionalEventArgs e)
        {
            e.Result = item["colCzyWyslana"] != null ? (bool)item["colCzyWyslana"] : false;
        }

        private void setState_Anulowana_MethodInvoking(object sender, EventArgs e)
        {
            //SetStatusWysylki(enumStatusWysylki.Anulowana);
        }

        private void setState_Wysłana_MethodInvoking(object sender, EventArgs e)
        {
            SetStatusWysylki(enumStatusWysylki.Wysłana);
        }

        private void setState_PrzygotowanieWysyłki_MethodInvoking(object sender, EventArgs e)
        {
            SetStatusWysylki(enumStatusWysylki.Obsługa);
        }

        private void SetStatusWysylki(enumStatusWysylki status)
        {
            item["enumStatusWysylki"] = status;
            item["colDataNadania"] = DateTime.Now;
            item.Update();
        }






    }
}
