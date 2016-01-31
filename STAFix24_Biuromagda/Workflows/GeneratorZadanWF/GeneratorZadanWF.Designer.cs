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

namespace Workflows.GeneratorZadanWF
{
    public sealed partial class GeneratorZadanWF
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
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.logTask = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Create_Task = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.sendEmail2 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
            this.Set_TaskEnumerator = new System.Workflow.Activities.CodeActivity();
            this.sendEmail1 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.logKlienci = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_Klienci = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "GeneratorZadanWF";
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
            // logTask
            // 
            this.logTask.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logTask.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind2.Name = "GeneratorZadanWF";
            activitybind2.Path = "logTask_HistoryDescription";
            activitybind3.Name = "GeneratorZadanWF";
            activitybind3.Path = "logTask_HistoryOutcome";
            this.logTask.Name = "logTask";
            this.logTask.OtherData = "";
            this.logTask.UserId = -1;
            this.logTask.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.logTask.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // Create_Task
            // 
            this.Create_Task.Name = "Create_Task";
            this.Create_Task.ExecuteCode += new System.EventHandler(this.Create_Task_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.Create_Task);
            this.sequenceActivity1.Activities.Add(this.logTask);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // sendEmail2
            // 
            this.sendEmail2.BCC = null;
            this.sendEmail2.Body = null;
            this.sendEmail2.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "GeneratorZadanWF";
            this.sendEmail2.CorrelationToken = correlationtoken1;
            this.sendEmail2.From = "";
            this.sendEmail2.Headers = null;
            this.sendEmail2.IncludeStatus = false;
            this.sendEmail2.Name = "sendEmail2";
            activitybind4.Name = "GeneratorZadanWF";
            activitybind4.Path = "msgSubject";
            activitybind5.Name = "GeneratorZadanWF";
            activitybind5.Path = "msgTo";
            this.sendEmail2.MethodInvoking += new System.EventHandler(this.sendEmail2_MethodInvoking);
            this.sendEmail2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.sendEmail2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // whileActivity1
            // 
            this.whileActivity1.Activities.Add(this.sequenceActivity1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileTaskExist);
            this.whileActivity1.Condition = codecondition1;
            this.whileActivity1.Name = "whileActivity1";
            // 
            // Set_TaskEnumerator
            // 
            this.Set_TaskEnumerator.Name = "Set_TaskEnumerator";
            this.Set_TaskEnumerator.ExecuteCode += new System.EventHandler(this.Set_TaskEnumerator_ExecuteCode);
            // 
            // sendEmail1
            // 
            this.sendEmail1.BCC = null;
            this.sendEmail1.Body = null;
            this.sendEmail1.CC = null;
            this.sendEmail1.CorrelationToken = correlationtoken1;
            this.sendEmail1.From = "";
            this.sendEmail1.Headers = null;
            this.sendEmail1.IncludeStatus = false;
            this.sendEmail1.Name = "sendEmail1";
            activitybind6.Name = "GeneratorZadanWF";
            activitybind6.Path = "msgSubject";
            activitybind7.Name = "GeneratorZadanWF";
            activitybind7.Path = "msgTo";
            this.sendEmail1.MethodInvoking += new System.EventHandler(this.sendEmail1_MethodInvoking);
            this.sendEmail1.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.sendEmail1.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // logKlienci
            // 
            this.logKlienci.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKlienci.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKlienci.HistoryDescription = "Liczba wybranych klientów";
            activitybind8.Name = "GeneratorZadanWF";
            activitybind8.Path = "logKlienci_HistoryOutcome";
            this.logKlienci.Name = "logKlienci";
            this.logKlienci.OtherData = "";
            this.logKlienci.UserId = -1;
            this.logKlienci.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // Select_Klienci
            // 
            this.Select_Klienci.Name = "Select_Klienci";
            this.Select_Klienci.ExecuteCode += new System.EventHandler(this.Select_Klienci_ExecuteCode);
            activitybind10.Name = "GeneratorZadanWF";
            activitybind10.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind9.Name = "GeneratorZadanWF";
            activitybind9.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // GeneratorZadanWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Select_Klienci);
            this.Activities.Add(this.logKlienci);
            this.Activities.Add(this.sendEmail1);
            this.Activities.Add(this.Set_TaskEnumerator);
            this.Activities.Add(this.whileActivity1);
            this.Activities.Add(this.sendEmail2);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "GeneratorZadanWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Set_TaskEnumerator;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail2;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logTask;

        private CodeActivity Create_Task;

        private SequenceActivity sequenceActivity1;

        private WhileActivity whileActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKlienci;

        private CodeActivity Select_Klienci;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;





















    }
}
