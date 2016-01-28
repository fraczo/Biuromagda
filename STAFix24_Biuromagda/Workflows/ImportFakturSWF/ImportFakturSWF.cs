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

namespace Workflows.ImportFakturSWF
{
    public sealed partial class ImportFakturSWF : SequentialWorkflowActivity
    {
        public ImportFakturSWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String logParameters_HistoryOutcome = default(System.String);
        int sourceItemId;
        int okresId;
        string _ZAKONCZONY = "Zakończony";
        string _ANULOWANY = "Anulowany";

        #region Error Handler
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

        public String logErrorMessage_HistoryDescription = default(System.String);
        #endregion

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("ImportFakturSWF:{" + workflowProperties.WorkflowId + "} initiated");
        }

        private void Get_Parameteres_ExecuteCode(object sender, EventArgs e)
        {
            if (workflowProperties.InitiationData.Length > 0)
            {
                string[] param = workflowProperties.InitiationData.Split(new string[] { ";" }, StringSplitOptions.None);

                okresId = int.Parse(param.GetValue(0).ToString());
                sourceItemId = int.Parse(param.GetValue(1).ToString());
                
                logParameters_HistoryOutcome = string.Format("OkresId={0}, SourceItemId={1}",
                                                              okresId.ToString(),
                                                              sourceItemId.ToString());


            }
            else
            {
                logParameters_HistoryOutcome = "brak";
            }
        }

        private void UpdateItem_Anulowany_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem(_ANULOWANY);
        }

        private void UpdateItem_Zakonczony_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem(_ZAKONCZONY);
        }

        private void Update_SourceItem(string statusZlecenia)
        {
            SPListItem sourceItem = BLL.admProcessRequests.GetItemById(workflowProperties.Web, sourceItemId);
            if (sourceItem != null)
            {
                BLL.Tools.Set_Text(sourceItem, "enumStatusZlecenia", statusZlecenia);
                sourceItem.Update();
            }
        }


    }
}
