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

namespace Workflows.ZatwierdzenieZadania
{
    public sealed partial class ZatwierdzenieZadania : SequentialWorkflowActivity
    {
        public ZatwierdzenieZadania()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        private SPListItem item;
        DateTime startTime;

        #region Error Handler
        public String logErrorMessage_HistoryDescription = default(System.String);

        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
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
        #endregion

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("ZatwierdzenieZadaniaWF:{" + workflowProperties.WorkflowId + "} initiated");
            item = workflowProperties.Item;

            Debug.WriteLine("Workflow created:" + workflowProperties.Workflow.Created.ToString());
            startTime = DateTime.Now;
        }

        private void Main_ExecuteCode(object sender, EventArgs e)
        {
            string status = BLL.Tools.Get_Text(item, "enumStatusZadania");
            switch (status)
            {
                case "Nowe":
                case "Obsługa":
                    if (item.ContentType.Name == "Prośba o dokumenty"
                        || item.ContentType.Name == "Prośba o przesłanie wyciągu bankowego"
                        || item.ContentType.Name == "Rozliczenie z biurem rachunkowym")
                        Zatwierdz_Zadanie(item);
                    break;
                case "Gotowe":
                    if (item.ContentType.Name == "Rozliczenie ZUS"
                        || item.ContentType.Name == "Rozliczenie podatku VAT"
                        || item.ContentType.Name == "Rozliczenie podatku dochodowego"
                        || item.ContentType.Name == "Rozliczenie podatku dochodowego spółki"
                        || item.ContentType.Name == "Rozliczenie podatku dochodowego wspólnika")
                        Zatwierdz_Zadanie(item);
                    break;
                default:
                    break;
            }
        }

        private void UpdateItem_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Tools.DoWithRetry(() => item.SystemUpdate());
        }

        private void InitWorkflow_ExecuteCode(object sender, EventArgs e)
        {
            //BLL.Workflows.StartWorkflow(item, "tabZadaniaWF", SPWorkflowRunOptions.SynchronousAllowPostpone);
        }


        #region Helpers
        private void Zatwierdz_Zadanie(SPListItem item)
        {
            string cmd = BLL.Tools.Get_Text(item, "cmdFormatka");
            if (string.IsNullOrEmpty(cmd))
            {
                item["cmdFormatka"] = "Zatwierdź";
            }
        }
        #endregion
    }
}
