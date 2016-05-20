using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Workflows.WyslijKopieWiadomosci
{
    public sealed partial class WyslijKopieWiadomosci
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.Run_ObslugaWiadomosci = new System.Workflow.Activities.CodeActivity();
            this.Create_KopiaWiadomosci = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Run_ObslugaWiadomosci
            // 
            this.Run_ObslugaWiadomosci.Name = "Run_ObslugaWiadomosci";
            this.Run_ObslugaWiadomosci.ExecuteCode += new System.EventHandler(this.Run_ObslugaWiadomosci_ExecuteCode);
            // 
            // Create_KopiaWiadomosci
            // 
            this.Create_KopiaWiadomosci.Name = "Create_KopiaWiadomosci";
            this.Create_KopiaWiadomosci.ExecuteCode += new System.EventHandler(this.Create_KopiaWiadomosci_ExecuteCode);
            activitybind2.Name = "WyslijKopieWiadomosci";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "WyslijKopieWiadomosci";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind1.Name = "WyslijKopieWiadomosci";
            activitybind1.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // WyslijKopieWiadomosci
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Create_KopiaWiadomosci);
            this.Activities.Add(this.Run_ObslugaWiadomosci);
            this.Name = "WyslijKopieWiadomosci";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Run_ObslugaWiadomosci;

        private CodeActivity Create_KopiaWiadomosci;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;



    }
}
