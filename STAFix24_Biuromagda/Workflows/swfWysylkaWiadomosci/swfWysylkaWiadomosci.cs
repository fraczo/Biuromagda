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
using System.Threading;

namespace Workflows.swfWysylkaWiadomosci
{
    public sealed partial class swfWysylkaWiadomosci : SequentialWorkflowActivity
    {
        public swfWysylkaWiadomosci()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String logSelected_HistoryDescription = default(System.String);
        public String logCurrentMessage_HistoryDescription = default(System.String);
        private Array results;
        private IEnumerator myEnum;
        private StringBuilder sb = new StringBuilder();
        public String adminEmail = "stafix24@hotmail.com";
        private string _ZAKONCZONY = "Zakończony";
        private string _ANULOWANY = "Anulowany";


        private void Select_ListaWiadomosciOczekujacych_ExecuteCode(object sender, EventArgs e)
        {
            results = BLL.tabWiadomosci.Select_Batch(workflowProperties.Web);
            myEnum = results.GetEnumerator();

            logSelected_HistoryOutcome = results.Length.ToString();
        }

        private void whileRecordExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Initialize_ChildWorkflow_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = myEnum.Current as SPListItem;

            BLL.Workflows.StartWorkflow(item, "Obsługa wiadomości");
            Debug.WriteLine("Workflow initiated for message #" + item.ID.ToString());

            //aktualizacja informacji do raportu
            sb.AppendFormat(@"<li>{0} :: {1}</li>",
                BLL.Tools.Get_LookupValue(item, "selKlient_NazwaSkrocona"),
                item.Title);

            logCurrentMessage_HistoryOutcome = item.ID.ToString();

        }

        public String sendAdminConfirmation_CC1 = default(System.String);
        public String msgSubject = default(System.String);
        public String msgBody = default(System.String);
        private void sendAdminConfirmation_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = string.Format(@"Wysyłka wiadomości zakończona ({0})", results.Length.ToString());
            msgBody = string.Format(@"Lista przetworzonych wiadomości<br><ol>{0}</ol>", sb.ToString());
        }



        private void cmdErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fa = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fa != null)
            {
                Debug.WriteLine(fa.Fault.Source);
                Debug.WriteLine(fa.Fault.Message);
                Debug.WriteLine(fa.Fault.StackTrace);

                logErrorMessage_HistoryDescription = string.Format("{0}::{1}",
                    fa.Fault.Message,
                    fa.Fault.StackTrace);

                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fa.Fault.Message, fa.Fault.StackTrace);
            }
        }

        public String logErrorMessage_HistoryDescription = default(System.String);

        private void cmdObslugaBledu_ExecuteCode(object sender, EventArgs e)
        {

        }

        public String logCurrentMessage_HistoryOutcome = default(System.String);

        private void cmdDelay_ExecuteCode(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
        }

        public String logSelected_HistoryOutcome = default(System.String);



        private void Update_Request_Completed_ExecuteCode(object sender, EventArgs e)
        {
            Update_Request(_ZAKONCZONY);
        }

        private void Update_Request(string status)
        {
            if (!string.IsNullOrEmpty(workflowProperties.InitiationData))
            {
                if (workflowProperties.InitiationData.Length > 0)
                {
                    string[] param = workflowProperties.InitiationData.Split(new string[] { ";" }, StringSplitOptions.None);

                    int sourceItemId = int.Parse(param.GetValue(0).ToString());

                    if (sourceItemId > 0)
                    {
                        SPListItem rItem = BLL.admProcessRequests.GetItemById(workflowProperties.Web, sourceItemId);
                        if (rItem != null)
                        {
                            BLL.Tools.Set_Text(rItem, "enumStatusZlecenia", status);
                            rItem.SystemUpdate();
                        }
                    }
                }
            }
        }

        private void Update_Request_Canceled_ExecuteCode(object sender, EventArgs e)
        {
            Update_Request(_ANULOWANY);
        }

    }
}
