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
using System.Diagnostics;
using System.Text;
using BLL;
using EventReceivers.admProcessRequestsER;

namespace Workflows.wfGenerujFormatkiRozliczeniowe
{
    public sealed partial class wfGenerujFormatkiRozliczeniowe : SequentialWorkflowActivity
    {
        public wfGenerujFormatkiRozliczeniowe()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        private SPListItem item;
        private string _CT_GFRK = "Generowanie formatek rozliczeniowych dla klienta";
        private int okresId;
        private int klientId;
        private StringBuilder msg;
        private SPListItem klient;
        private string _CT_KPIR = "KPiR";
        private string _CT_KSH = "KSH";
        private string _CT_Firma = "Firma";
        private string _CT_FirmaZewnętrzna = "Firma zewnętrzna";
        private string _CT_OsobaFizyczna = "Osoba fizyczna";


        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void isCT_GFRK(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals(_CT_GFRK)) e.Result = true;
        }

        private void cmdCaptureParams_ExecuteCode(object sender, EventArgs e)
        {
            okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;
            klientId = new SPFieldLookupValue(item["selKlient"].ToString()).LookupId;

            logOkresId_HistoryOutcome = okresId.ToString();
            logKlientId_HistoryOutcome = klientId.ToString();

            Debug.WriteLine(string.Format("klientId={0}, okresId={1}", klientId.ToString(), okresId.ToString()));
        }

        private void isValidParams(object sender, ConditionalEventArgs e)
        {
            if (okresId > 0 && klientId > 0) e.Result = true;
        }

        private void cmdInitMsg_ExecuteCode(object sender, EventArgs e)
        {
            msg = new StringBuilder();
        }

        private void cmdGetKlientDetails_ExecuteCode(object sender, EventArgs e)
        {
            klient = tabKlienci.Get_KlientById(item.Web, klientId);
            if (klient != null)
            {
                logKlient_HistoryOutcome = BLL.Tools.Get_Text(klient, "_NazwaPrezentowana");

                // update msg
                msg.AppendFormat(@"<li>klient# {0} {1}</li>",
                    klient.ID.ToString(),
                    BLL.Tools.Get_Text(klient, "_NazwaPrezentowana"));
            }
        }

        private void isKPIR(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_KPIR)) e.Result = true;
        }

        private void isKSH(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_KSH)) e.Result = true;
        }

        private void isFirma(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_Firma)) e.Result = true;
        }

        private void isOsobaFizyczna(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_OsobaFizyczna)) e.Result = true;
        }

        private void isFirmaZewnetrzna(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_FirmaZewnętrzna)) e.Result = true;
        }

        private void Manage_ZUS_ExecuteCode(object sender, EventArgs e)
        {
            //ZUS_Forms.Create(item.Web, klientId, okresId, true);

        }

        private void Manage_PD_ExecuteCode(object sender, EventArgs e)
        {
            //PD_Forms.Create(item.Web, klientId, okresId, true);

        }

        private void Manage_VAT_ExecuteCode(object sender, EventArgs e)
        {
            //VAT_Forms.Create(item.Web, klientId, okresId, true);

        }

        private void Manage_RBR_ExecuteCode(object sender, EventArgs e)
        {
            //BR_Forms.Create(item.Web, klientId, okresId, true);

        }

        private void Manage_Reminders_ExecuteCode(object sender, EventArgs e)
        {
            //Reminder_Forms.Create(item.Web, klientId, okresId);
        }

        public String logKlientId_HistoryOutcome = default(System.String);
        public String logOkresId_HistoryOutcome = default(System.String);
        public String logKlient_HistoryOutcome = default(System.String);
        public String logErrorMessage_HistoryDescription = default(System.String);
        public String logErrorMessage_HistoryOutcome = default(System.String);

        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity faultHandlerActivity = ((Activity)sender).Parent as FaultHandlerActivity;
            if (faultHandlerActivity != null)
            {
                logErrorMessage_HistoryDescription = faultHandlerActivity.Fault.Message;
                logErrorMessage_HistoryOutcome = faultHandlerActivity.Fault.StackTrace;

                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, faultHandlerActivity.Fault.Message, faultHandlerActivity.Fault.StackTrace);
            }
        }

        private void onWorkflowActivated1_Invoked_1(object sender, ExternalDataEventArgs e)
        {
            //item = workflowProperties.Item;
        }


    }
}
