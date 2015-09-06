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

namespace Workflows.ObslugaWiadomosci
{
    public sealed partial class ObslugaWiadomosci : SequentialWorkflowActivity
    {
        public ObslugaWiadomosci()
        {
            InitializeComponent();
        }

        enum StatusWysylki : int
        {
            Zarejestrowana = 15,
            Oczekuje,
            Wysłana,
            Anulowana
        }

        //public Int32 StatusWF = StatusWysylki.Zarejestrowana.ToString();


        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;

        private void Set_From_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Set_CC_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Set_Body_ExecuteCode(object sender, EventArgs e)
        {
            item["colTresc"] = DateTime.Now.ToString();
            item.Update();
        }

        private void Send_Mail_ExecuteCode(object sender, EventArgs e)
        {
            SPFieldUserValue user = new SPFieldUserValue(item.ParentList.ParentWeb, item["Author"].ToString());
            SendMailWithAttachment(item, "noreply@stafix24.pl", user.User.Email, "to jest temat", "", true, "", "");
        }

        private void Update_Flags_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void isWiadomoscWyslana(object sender, ConditionalEventArgs e)
        {
            e.Result = false;
        }

        private void isOdroczonaWysylka(object sender, ConditionalEventArgs e)
        {
            e.Result = false;
        }

        private void isWysylkaZakonczona2(object sender, ConditionalEventArgs e)
        {
            e.Result = true;
        }

        private void Update_tabKartyKlientów_ExecuteCode(object sender, EventArgs e)
        {

        }


        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {

        }


        #region Helpers



        public static void SendMailWithAttachment(SPListItem item, string from, string to, string subject, string body, bool isBodyHtml, string cc, string bcc)
        {

            SmtpClient client = new SmtpClient();
            client.Host = item.Web.Site.WebApplication.OutboundMailServiceInstance.Server.Address;

            //nazwa witryny
            from = item.Web.Title != null ? String.Format(@"{0}<{1}>",
                item.Web.Title,
                from) : from;

            MailMessage message = new MailMessage();
            SPList list = item.ParentList;
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(cc))
            {
                message.CC.Add(new MailAddress(cc));
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                message.Bcc.Add(new MailAddress(bcc));
            }
            message.IsBodyHtml = isBodyHtml;
            message.Body = body;
            message.Subject = subject;

            for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
            {
                string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
                SPFile file = list.ParentWeb.GetFile(url);
                message.Attachments.Add(new Attachment(file.OpenBinaryStream(), file.Name));
            }

            client.Send(message);

        }

        private void sendEmailToAssignee_MethodInvoking(object sender, EventArgs e)
        {
            //SPListItem wfItem = onWorkflowActivated1.WorkflowProperties.Item;
            //SPFieldUser assignedTo = (SPFieldUser)wfItem.Fields["Assigned To"];
            //SPFieldUserValue user = (SPFieldUserValue)assignedTo.GetFieldValue(
            //wfItem["Assigned To"].ToString());
            //string assigneeEmail = user.User.Email;
            sendEmailToAssignee.To = new SPFieldUserValue(item.ParentList.ParentWeb, item["Author"].ToString()).User.Email;
            sendEmailToAssignee.Subject = "New work order has been created.";
            sendEmailToAssignee.Body = "Work order number " +
            onWorkflowActivated1.WorkflowProperties.Item.ID +
            " has just been created and assigned to you.";
            sendEmailToAssignee.From = "Maszynka<noreply@stafix24.pl>";

            SendMailWithAttachment(item, "noreply@stafix24.pl", sendEmailToAssignee.To,"nowy temat", sendEmailToAssignee.Body, true, string.Empty, string.Empty);
        }


        #endregion


    }
}
