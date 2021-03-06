﻿using System;
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

namespace Workflows.swfWysylkaWiadomosci
{
    public sealed partial class swfWysylkaWiadomosci
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
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.logRequestCanceled = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Update_Request_Canceled = new System.Workflow.Activities.CodeActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.logReport = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sendAdminConfirmation = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Initialize_ChildWorkflow = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifResultsExists = new System.Workflow.Activities.IfElseBranchActivity();
            this.ObsługaPojedynczejWiadomości = new System.Workflow.Activities.SequenceActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.logRequestCompleted = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Update_Request_Completed = new System.Workflow.Activities.CodeActivity();
            this.ReportResults = new System.Workflow.Activities.IfElseActivity();
            this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
            this.logSelected = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_ListaWiadomosciOczekujacych = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logRequestCanceled
            // 
            this.logRequestCanceled.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logRequestCanceled.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logRequestCanceled.HistoryDescription = "Request";
            this.logRequestCanceled.HistoryOutcome = "Canceled";
            this.logRequestCanceled.Name = "logRequestCanceled";
            this.logRequestCanceled.OtherData = "";
            this.logRequestCanceled.UserId = -1;
            // 
            // Update_Request_Canceled
            // 
            this.Update_Request_Canceled.Name = "Update_Request_Canceled";
            this.Update_Request_Canceled.ExecuteCode += new System.EventHandler(this.Update_Request_Canceled_ExecuteCode);
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "swfWysylkaWiadomosci";
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
            // logReport
            // 
            this.logReport.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logReport.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logReport.HistoryDescription = "Report";
            this.logReport.HistoryOutcome = "Sent";
            this.logReport.Name = "logReport";
            this.logReport.OtherData = "";
            this.logReport.UserId = -1;
            // 
            // sendAdminConfirmation
            // 
            this.sendAdminConfirmation.BCC = null;
            activitybind2.Name = "swfWysylkaWiadomosci";
            activitybind2.Path = "msgBody";
            this.sendAdminConfirmation.CC = "";
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "swfWysylkaWiadomosci";
            this.sendAdminConfirmation.CorrelationToken = correlationtoken1;
            this.sendAdminConfirmation.From = null;
            this.sendAdminConfirmation.Headers = null;
            this.sendAdminConfirmation.IncludeStatus = false;
            this.sendAdminConfirmation.Name = "sendAdminConfirmation";
            activitybind3.Name = "swfWysylkaWiadomosci";
            activitybind3.Path = "msgSubject";
            activitybind4.Name = "swfWysylkaWiadomosci";
            activitybind4.Path = "adminEmail";
            this.sendAdminConfirmation.MethodInvoking += new System.EventHandler(this.sendAdminConfirmation_MethodInvoking);
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.sendAdminConfirmation.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // Initialize_ChildWorkflow
            // 
            this.Initialize_ChildWorkflow.Name = "Initialize_ChildWorkflow";
            this.Initialize_ChildWorkflow.ExecuteCode += new System.EventHandler(this.Initialize_ChildWorkflow_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.cmdErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.Activities.Add(this.Update_Request_Canceled);
            this.faultHandlerActivity1.Activities.Add(this.logRequestCanceled);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // ifResultsExists
            // 
            this.ifResultsExists.Activities.Add(this.sendAdminConfirmation);
            this.ifResultsExists.Activities.Add(this.logReport);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.hasResults);
            this.ifResultsExists.Condition = codecondition1;
            this.ifResultsExists.Name = "ifResultsExists";
            // 
            // ObsługaPojedynczejWiadomości
            // 
            this.ObsługaPojedynczejWiadomości.Activities.Add(this.Initialize_ChildWorkflow);
            this.ObsługaPojedynczejWiadomości.Name = "ObsługaPojedynczejWiadomości";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // logRequestCompleted
            // 
            this.logRequestCompleted.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logRequestCompleted.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logRequestCompleted.HistoryDescription = "Request";
            this.logRequestCompleted.HistoryOutcome = "Completed";
            this.logRequestCompleted.Name = "logRequestCompleted";
            this.logRequestCompleted.OtherData = "";
            this.logRequestCompleted.UserId = -1;
            // 
            // Update_Request_Completed
            // 
            this.Update_Request_Completed.Name = "Update_Request_Completed";
            this.Update_Request_Completed.ExecuteCode += new System.EventHandler(this.Update_Request_Completed_ExecuteCode);
            // 
            // ReportResults
            // 
            this.ReportResults.Activities.Add(this.ifResultsExists);
            this.ReportResults.Name = "ReportResults";
            // 
            // whileActivity1
            // 
            this.whileActivity1.Activities.Add(this.ObsługaPojedynczejWiadomości);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileRecordExist);
            this.whileActivity1.Condition = codecondition2;
            this.whileActivity1.Name = "whileActivity1";
            // 
            // logSelected
            // 
            this.logSelected.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logSelected.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logSelected.HistoryDescription = "Liczba wiadomości do obsługi";
            activitybind5.Name = "swfWysylkaWiadomosci";
            activitybind5.Path = "logSelected_HistoryOutcome";
            this.logSelected.Name = "logSelected";
            this.logSelected.OtherData = "";
            this.logSelected.UserId = -1;
            this.logSelected.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // Select_ListaWiadomosciOczekujacych
            // 
            this.Select_ListaWiadomosciOczekujacych.Name = "Select_ListaWiadomosciOczekujacych";
            this.Select_ListaWiadomosciOczekujacych.ExecuteCode += new System.EventHandler(this.Select_ListaWiadomosciOczekujacych_ExecuteCode);
            activitybind7.Name = "swfWysylkaWiadomosci";
            activitybind7.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind6.Name = "swfWysylkaWiadomosci";
            activitybind6.Path = "workflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // swfWysylkaWiadomosci
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Select_ListaWiadomosciOczekujacych);
            this.Activities.Add(this.logSelected);
            this.Activities.Add(this.whileActivity1);
            this.Activities.Add(this.ReportResults);
            this.Activities.Add(this.Update_Request_Completed);
            this.Activities.Add(this.logRequestCompleted);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "swfWysylkaWiadomosci";
            this.CanModifyActivities = false;

        }

        #endregion

        private IfElseBranchActivity ifResultsExists;

        private IfElseActivity ReportResults;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logRequestCanceled;

        private CodeActivity Update_Request_Canceled;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logRequestCompleted;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logReport;

        private CodeActivity Update_Request_Completed;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity cmdErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendAdminConfirmation;

        private CodeActivity Initialize_ChildWorkflow;

        private SequenceActivity ObsługaPojedynczejWiadomości;

        private WhileActivity whileActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logSelected;

        private CodeActivity Select_ListaWiadomosciOczekujacych;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;










































    }
}
