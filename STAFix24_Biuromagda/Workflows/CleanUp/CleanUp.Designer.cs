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

namespace Workflows.CleanUp
{
    public sealed partial class CleanUp
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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            this.Manage_Wiadomość = new System.Workflow.Activities.CodeActivity();
            this.Manage_Zadanie = new System.Workflow.Activities.CodeActivity();
            this.logWiadomosciCompleted = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileWiadomoscExist = new System.Workflow.Activities.WhileActivity();
            this.Select_ListaWiadomosci = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logZadaniaCompleted = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileZadanieExist = new System.Workflow.Activities.WhileActivity();
            this.Select_ListaZadan = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Update_Status = new System.Workflow.Activities.CodeActivity();
            this.ObsługaListyWiadomości = new System.Workflow.Activities.SequenceActivity();
            this.ObsługaListyZadań = new System.Workflow.Activities.SequenceActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Manage_Wiadomość
            // 
            this.Manage_Wiadomość.Name = "Manage_Wiadomość";
            this.Manage_Wiadomość.ExecuteCode += new System.EventHandler(this.Manage_Wiadomość_ExecuteCode);
            // 
            // Manage_Zadanie
            // 
            this.Manage_Zadanie.Name = "Manage_Zadanie";
            this.Manage_Zadanie.ExecuteCode += new System.EventHandler(this.Manage_Zadanie_ExecuteCode);
            // 
            // logWiadomosciCompleted
            // 
            this.logWiadomosciCompleted.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logWiadomosciCompleted.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logWiadomosciCompleted.HistoryDescription = "Obsługa listy wiadomości zakończona";
            activitybind1.Name = "CleanUp";
            activitybind1.Path = "messageCounter";
            this.logWiadomosciCompleted.Name = "logWiadomosciCompleted";
            this.logWiadomosciCompleted.OtherData = "";
            this.logWiadomosciCompleted.UserId = -1;
            this.logWiadomosciCompleted.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // whileWiadomoscExist
            // 
            this.whileWiadomoscExist.Activities.Add(this.Manage_Wiadomość);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isWiadomoscExist);
            this.whileWiadomoscExist.Condition = codecondition1;
            this.whileWiadomoscExist.Name = "whileWiadomoscExist";
            // 
            // Select_ListaWiadomosci
            // 
            this.Select_ListaWiadomosci.Name = "Select_ListaWiadomosci";
            this.Select_ListaWiadomosci.ExecuteCode += new System.EventHandler(this.Select_ListaWiadomosci_ExecuteCode);
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "Obsługa listy wiadomości";
            this.logToHistoryListActivity4.HistoryOutcome = "";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // logZadaniaCompleted
            // 
            this.logZadaniaCompleted.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logZadaniaCompleted.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logZadaniaCompleted.HistoryDescription = "Obsługa listy zadań zakończona";
            activitybind2.Name = "CleanUp";
            activitybind2.Path = "taskCounter";
            this.logZadaniaCompleted.Name = "logZadaniaCompleted";
            this.logZadaniaCompleted.OtherData = "";
            this.logZadaniaCompleted.UserId = -1;
            this.logZadaniaCompleted.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // whileZadanieExist
            // 
            this.whileZadanieExist.Activities.Add(this.Manage_Zadanie);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isZadanieExist);
            this.whileZadanieExist.Condition = codecondition2;
            this.whileZadanieExist.Name = "whileZadanieExist";
            // 
            // Select_ListaZadan
            // 
            this.Select_ListaZadan.Name = "Select_ListaZadan";
            this.Select_ListaZadan.ExecuteCode += new System.EventHandler(this.Select_ListaZadan_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "Obsługa listy zadań";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // Update_Status
            // 
            this.Update_Status.Name = "Update_Status";
            this.Update_Status.ExecuteCode += new System.EventHandler(this.Update_Status_ExecuteCode);
            // 
            // ObsługaListyWiadomości
            // 
            this.ObsługaListyWiadomości.Activities.Add(this.logToHistoryListActivity4);
            this.ObsługaListyWiadomości.Activities.Add(this.Select_ListaWiadomosci);
            this.ObsługaListyWiadomości.Activities.Add(this.whileWiadomoscExist);
            this.ObsługaListyWiadomości.Activities.Add(this.logWiadomosciCompleted);
            this.ObsługaListyWiadomości.Name = "ObsługaListyWiadomości";
            // 
            // ObsługaListyZadań
            // 
            this.ObsługaListyZadań.Activities.Add(this.logToHistoryListActivity3);
            this.ObsługaListyZadań.Activities.Add(this.Select_ListaZadan);
            this.ObsługaListyZadań.Activities.Add(this.whileZadanieExist);
            this.ObsługaListyZadań.Activities.Add(this.logZadaniaCompleted);
            this.ObsługaListyZadań.Name = "ObsługaListyZadań";
            activitybind4.Name = "CleanUp";
            activitybind4.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "CleanUp";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind3.Name = "CleanUp";
            activitybind3.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // CleanUp
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.ObsługaListyZadań);
            this.Activities.Add(this.ObsługaListyWiadomości);
            this.Activities.Add(this.Update_Status);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "CleanUp";
            this.CanModifyActivities = false;

        }

        #endregion

        private FaultHandlersActivity faultHandlersActivity1;

        private CodeActivity Update_Status;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private CodeActivity Manage_Wiadomość;

        private CodeActivity Manage_Zadanie;

        private WhileActivity whileWiadomoscExist;

        private CodeActivity Select_ListaWiadomosci;

        private WhileActivity whileZadanieExist;

        private CodeActivity Select_ListaZadan;

        private SequenceActivity ObsługaListyWiadomości;

        private SequenceActivity ObsługaListyZadań;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logWiadomosciCompleted;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logZadaniaCompleted;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;













    }
}
