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

namespace Workflows.swfCleanup
{
    public sealed partial class swfCleanup
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
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.Manage_Wiadomość = new System.Workflow.Activities.CodeActivity();
            this.Manage_Zadanie = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.logWiadomosciCompleted = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileWiadomoscExist = new System.Workflow.Activities.WhileActivity();
            this.Select_ListaWiadomosci = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logZadaniaCompleted = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileZadanieExist = new System.Workflow.Activities.WhileActivity();
            this.Select_ListaZadan = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.sendAdminConfirmation = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.cmdEmptyRecycleBin = new System.Workflow.Activities.CodeActivity();
            this.ObsługaListyWiadomości = new System.Workflow.Activities.SequenceActivity();
            this.ObsługaListyZadań = new System.Workflow.Activities.SequenceActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "swfCleanup";
            activitybind1.Path = "logErrorMessage_HistoryDescription";
            this.logErrorMessage.HistoryOutcome = "";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // cmdErrorHandler
            // 
            this.cmdErrorHandler.Name = "cmdErrorHandler";
            this.cmdErrorHandler.ExecuteCode += new System.EventHandler(this.cmdErrorHandler_ExecuteCode);
            // 
            // Manage_Wiadomość
            // 
            this.Manage_Wiadomość.Name = "Manage_Wiadomość";
            this.Manage_Wiadomość.ExecuteCode += new System.EventHandler(this.Manage_Wiadomosc_ExecuteCode);
            // 
            // Manage_Zadanie
            // 
            this.Manage_Zadanie.Name = "Manage_Zadanie";
            this.Manage_Zadanie.ExecuteCode += new System.EventHandler(this.Manage_Zadanie_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.cmdErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.SystemException);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // logWiadomosciCompleted
            // 
            this.logWiadomosciCompleted.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logWiadomosciCompleted.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logWiadomosciCompleted.HistoryDescription = "Obsługa listy wiadomości zakończona";
            activitybind2.Name = "swfCleanup";
            activitybind2.Path = "logWiadomosciCompleted_HistoryOutcome";
            this.logWiadomosciCompleted.Name = "logWiadomosciCompleted";
            this.logWiadomosciCompleted.OtherData = "";
            this.logWiadomosciCompleted.UserId = -1;
            this.logWiadomosciCompleted.MethodInvoking += new System.EventHandler(this.cmdUpdateCounters);
            this.logWiadomosciCompleted.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
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
            activitybind3.Name = "swfCleanup";
            activitybind3.Path = "logZadaniaCompleted_HistoryOutcome";
            this.logZadaniaCompleted.Name = "logZadaniaCompleted";
            this.logZadaniaCompleted.OtherData = "";
            this.logZadaniaCompleted.UserId = -1;
            this.logZadaniaCompleted.MethodInvoking += new System.EventHandler(this.cmdUpdateCounters);
            this.logZadaniaCompleted.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
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
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // sendAdminConfirmation
            // 
            this.sendAdminConfirmation.BCC = null;
            activitybind4.Name = "swfCleanup";
            activitybind4.Path = "msgBody";
            this.sendAdminConfirmation.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "swfCleanup";
            this.sendAdminConfirmation.CorrelationToken = correlationtoken1;
            this.sendAdminConfirmation.From = null;
            this.sendAdminConfirmation.Headers = null;
            this.sendAdminConfirmation.IncludeStatus = false;
            this.sendAdminConfirmation.Name = "sendAdminConfirmation";
            activitybind5.Name = "swfCleanup";
            activitybind5.Path = "msgSubject";
            activitybind6.Name = "swfCleanup";
            activitybind6.Path = "msgAdminEmail";
            this.sendAdminConfirmation.MethodInvoking += new System.EventHandler(this.sendAdminConfirmation_MethodInvoking);
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // cmdEmptyRecycleBin
            // 
            this.cmdEmptyRecycleBin.Name = "cmdEmptyRecycleBin";
            this.cmdEmptyRecycleBin.ExecuteCode += new System.EventHandler(this.cmdEmptyRecycleBin_ExecuteCode);
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
            activitybind8.Name = "swfCleanup";
            activitybind8.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind7.Name = "swfCleanup";
            activitybind7.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // swfCleanup
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.ObsługaListyZadań);
            this.Activities.Add(this.ObsługaListyWiadomości);
            this.Activities.Add(this.cmdEmptyRecycleBin);
            this.Activities.Add(this.sendAdminConfirmation);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "swfCleanup";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity cmdEmptyRecycleBin;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendAdminConfirmation;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity cmdErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private CodeActivity Manage_Zadanie;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logZadaniaCompleted;

        private WhileActivity whileZadanieExist;

        private CodeActivity Select_ListaZadan;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private SequenceActivity ObsługaListyZadań;

        private CodeActivity Manage_Wiadomość;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logWiadomosciCompleted;

        private WhileActivity whileWiadomoscExist;

        private CodeActivity Select_ListaWiadomosci;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private SequenceActivity ObsługaListyWiadomości;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;






















    }
}
