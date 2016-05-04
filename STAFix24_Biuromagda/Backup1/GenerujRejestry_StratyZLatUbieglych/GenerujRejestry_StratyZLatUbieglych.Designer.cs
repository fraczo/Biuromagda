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

namespace Workflows.GenerujRejestry_StratyZLatUbieglych
{
    public sealed partial class GenerujRejestry_StratyZLatUbieglych
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
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.Increment_KlientIndex = new System.Workflow.Activities.CodeActivity();
            this.Create_RekordyStrat = new System.Workflow.Activities.CodeActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.whileKlientExist = new System.Workflow.Activities.WhileActivity();
            this.Set_KlientIndex = new System.Workflow.Activities.CodeActivity();
            this.Select_ListaKlientow = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Increment_KlientIndex
            // 
            this.Increment_KlientIndex.Name = "Increment_KlientIndex";
            this.Increment_KlientIndex.ExecuteCode += new System.EventHandler(this.Increment_KlientIndex_ExecuteCode);
            // 
            // Create_RekordyStrat
            // 
            this.Create_RekordyStrat.Name = "Create_RekordyStrat";
            this.Create_RekordyStrat.ExecuteCode += new System.EventHandler(this.Create_RekordyStrat_ExecuteCode);
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.Create_RekordyStrat);
            this.sequenceActivity1.Activities.Add(this.Increment_KlientIndex);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // whileKlientExist
            // 
            this.whileKlientExist.Activities.Add(this.sequenceActivity1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKlientExist);
            this.whileKlientExist.Condition = codecondition1;
            this.whileKlientExist.Name = "whileKlientExist";
            // 
            // Set_KlientIndex
            // 
            this.Set_KlientIndex.Name = "Set_KlientIndex";
            this.Set_KlientIndex.ExecuteCode += new System.EventHandler(this.Set_KlientIndex_ExecuteCode);
            // 
            // Select_ListaKlientow
            // 
            this.Select_ListaKlientow.Name = "Select_ListaKlientow";
            this.Select_ListaKlientow.ExecuteCode += new System.EventHandler(this.Select_ListaKlientow_ExecuteCode);
            activitybind2.Name = "GenerujRejestry_StratyZLatUbieglych";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "GenerujRejestry_StratyZLatUbieglych";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind1.Name = "GenerujRejestry_StratyZLatUbieglych";
            activitybind1.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // GenerujRejestry_StratyZLatUbieglych
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Select_ListaKlientow);
            this.Activities.Add(this.Set_KlientIndex);
            this.Activities.Add(this.whileKlientExist);
            this.Name = "GenerujRejestry_StratyZLatUbieglych";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Increment_KlientIndex;

        private SequenceActivity sequenceActivity1;

        private CodeActivity Set_KlientIndex;

        private CodeActivity Create_RekordyStrat;

        private WhileActivity whileKlientExist;

        private CodeActivity Select_ListaKlientow;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;







    }
}
