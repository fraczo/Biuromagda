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

namespace Workflows.tabZadaniaWF
{
    public sealed partial class tabZadaniaWF
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
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.Reporting = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.SetOperator = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.SetTitle = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ManageCT = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Preset = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "tabZadaniaWF";
            activitybind1.Path = "logErrorMessage_HistoryDescription";
            this.logErrorMessage.HistoryOutcome = "";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // Reporting
            // 
            this.Reporting.Name = "Reporting";
            this.Reporting.ExecuteCode += new System.EventHandler(this.Reporting_ExecuteCode);
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "ItemUpdate";
            this.logToHistoryListActivity5.HistoryOutcome = "";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            // 
            // UpdateItem
            // 
            this.UpdateItem.Name = "UpdateItem";
            this.UpdateItem.ExecuteCode += new System.EventHandler(this.UpdateItem_ExecuteCode);
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "SetOperator";
            this.logToHistoryListActivity4.HistoryOutcome = "";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // SetOperator
            // 
            this.SetOperator.Name = "SetOperator";
            this.SetOperator.ExecuteCode += new System.EventHandler(this.SetOperator_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "SetTitle";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // SetTitle
            // 
            this.SetTitle.Name = "SetTitle";
            this.SetTitle.ExecuteCode += new System.EventHandler(this.SetTitle_ExecuteCode);
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "ManageCT";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // ManageCT
            // 
            this.ManageCT.Name = "ManageCT";
            this.ManageCT.ExecuteCode += new System.EventHandler(this.ManageCT_ExecuteCode);
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "Preset";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // Preset
            // 
            this.Preset.Name = "Preset";
            this.Preset.ExecuteCode += new System.EventHandler(this.Preset_ExecuteCode);
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "AAAAAAAAAAAAAAA";
            this.logToHistoryListActivity6.HistoryOutcome = "";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            activitybind3.Name = "tabZadaniaWF";
            activitybind3.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "tabZadaniaWF";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind2.Name = "tabZadaniaWF";
            activitybind2.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // tabZadaniaWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity6);
            this.Activities.Add(this.Preset);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.ManageCT);
            this.Activities.Add(this.logToHistoryListActivity2);
            this.Activities.Add(this.SetTitle);
            this.Activities.Add(this.logToHistoryListActivity3);
            this.Activities.Add(this.SetOperator);
            this.Activities.Add(this.logToHistoryListActivity4);
            this.Activities.Add(this.UpdateItem);
            this.Activities.Add(this.logToHistoryListActivity5);
            this.Activities.Add(this.Reporting);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "tabZadaniaWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private CodeActivity Reporting;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private CodeActivity ManageCT;

        private CodeActivity Preset;

        private CodeActivity UpdateItem;

        private CodeActivity SetOperator;

        private CodeActivity SetTitle;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private CancellationHandlerActivity cancellationHandlerActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;





















    }
}