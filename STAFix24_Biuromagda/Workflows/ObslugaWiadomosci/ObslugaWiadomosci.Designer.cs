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

namespace Workflows.ObslugaWiadomosci
{
    public sealed partial class ObslugaWiadomosci
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
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.Update_tabKartyKlientów = new System.Workflow.Activities.CodeActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.AktualizacjaPowiązanychKrtotek = new System.Workflow.Activities.SequenceActivity();
            this.suspendActivity1 = new System.Workflow.ComponentModel.SuspendActivity();
            this.terminateActivity1 = new System.Workflow.ComponentModel.TerminateActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.Else3 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifWysylkaZakonczona = new System.Workflow.Activities.IfElseBranchActivity();
            this.Update_Flags = new System.Workflow.Activities.CodeActivity();
            this.Send_Mail = new System.Workflow.Activities.CodeActivity();
            this.Set_Body = new System.Workflow.Activities.CodeActivity();
            this.Set_CC = new System.Workflow.Activities.CodeActivity();
            this.Set_From = new System.Workflow.Activities.CodeActivity();
            this.ifOdroczonaWysylka = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifWiadomoscWyslana = new System.Workflow.Activities.IfElseBranchActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.CzyWysłana = new System.Workflow.Activities.IfElseActivity();
            this.WysylkaWiadomosci = new System.Workflow.Activities.SequenceActivity();
            this.FormatowanieWiadomosci = new System.Workflow.Activities.SequenceActivity();
            this.setState2 = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.delayActivity1 = new System.Workflow.Activities.DelayActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sendEmailToAssignee = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.CzyOdroczonaWysyłka = new System.Workflow.Activities.IfElseActivity();
            this.setState1 = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.CzyWiadomośćWysłana = new System.Workflow.Activities.IfElseActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Update_tabKartyKlientów
            // 
            this.Update_tabKartyKlientów.Name = "Update_tabKartyKlientów";
            this.Update_tabKartyKlientów.ExecuteCode += new System.EventHandler(this.Update_tabKartyKlientów_ExecuteCode);
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // AktualizacjaPowiązanychKrtotek
            // 
            this.AktualizacjaPowiązanychKrtotek.Activities.Add(this.Update_tabKartyKlientów);
            this.AktualizacjaPowiązanychKrtotek.Name = "AktualizacjaPowiązanychKrtotek";
            // 
            // suspendActivity1
            // 
            this.suspendActivity1.Name = "suspendActivity1";
            // 
            // terminateActivity1
            // 
            this.terminateActivity1.Name = "terminateActivity1";
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // Else3
            // 
            this.Else3.Name = "Else3";
            // 
            // ifWysylkaZakonczona
            // 
            this.ifWysylkaZakonczona.Activities.Add(this.AktualizacjaPowiązanychKrtotek);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isWysylkaZakonczona2);
            this.ifWysylkaZakonczona.Condition = codecondition1;
            this.ifWysylkaZakonczona.Name = "ifWysylkaZakonczona";
            // 
            // Update_Flags
            // 
            this.Update_Flags.Name = "Update_Flags";
            this.Update_Flags.ExecuteCode += new System.EventHandler(this.Update_Flags_ExecuteCode);
            // 
            // Send_Mail
            // 
            this.Send_Mail.Name = "Send_Mail";
            this.Send_Mail.ExecuteCode += new System.EventHandler(this.Send_Mail_ExecuteCode);
            // 
            // Set_Body
            // 
            this.Set_Body.Name = "Set_Body";
            this.Set_Body.ExecuteCode += new System.EventHandler(this.Set_Body_ExecuteCode);
            // 
            // Set_CC
            // 
            this.Set_CC.Name = "Set_CC";
            this.Set_CC.ExecuteCode += new System.EventHandler(this.Set_CC_ExecuteCode);
            // 
            // Set_From
            // 
            this.Set_From.Name = "Set_From";
            this.Set_From.ExecuteCode += new System.EventHandler(this.Set_From_ExecuteCode);
            // 
            // ifOdroczonaWysylka
            // 
            this.ifOdroczonaWysylka.Activities.Add(this.suspendActivity1);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isOdroczonaWysylka);
            this.ifOdroczonaWysylka.Condition = codecondition2;
            this.ifOdroczonaWysylka.Name = "ifOdroczonaWysylka";
            // 
            // ifWiadomoscWyslana
            // 
            this.ifWiadomoscWyslana.Activities.Add(this.terminateActivity1);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isWiadomoscWyslana);
            this.ifWiadomoscWyslana.Condition = codecondition3;
            this.ifWiadomoscWyslana.Name = "ifWiadomoscWyslana";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Enabled = false;
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // CzyWysłana
            // 
            this.CzyWysłana.Activities.Add(this.ifWysylkaZakonczona);
            this.CzyWysłana.Activities.Add(this.Else3);
            this.CzyWysłana.Name = "CzyWysłana";
            // 
            // WysylkaWiadomosci
            // 
            this.WysylkaWiadomosci.Activities.Add(this.Send_Mail);
            this.WysylkaWiadomosci.Activities.Add(this.Update_Flags);
            this.WysylkaWiadomosci.Name = "WysylkaWiadomosci";
            // 
            // FormatowanieWiadomosci
            // 
            this.FormatowanieWiadomosci.Activities.Add(this.Set_From);
            this.FormatowanieWiadomosci.Activities.Add(this.Set_CC);
            this.FormatowanieWiadomosci.Activities.Add(this.Set_Body);
            this.FormatowanieWiadomosci.Name = "FormatowanieWiadomosci";
            // 
            // setState2
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "ObslugaWiadomosci";
            this.setState2.CorrelationToken = correlationtoken1;
            this.setState2.Name = "setState2";
            this.setState2.State = 17;
            // 
            // delayActivity1
            // 
            this.delayActivity1.Enabled = false;
            this.delayActivity1.Name = "delayActivity1";
            this.delayActivity1.TimeoutDuration = System.TimeSpan.Parse("00:00:03");
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "Wiadomość wysłana";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // sendEmailToAssignee
            // 
            this.sendEmailToAssignee.BCC = null;
            this.sendEmailToAssignee.Body = null;
            this.sendEmailToAssignee.CC = null;
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "ObslugaWiadomosci";
            this.sendEmailToAssignee.CorrelationToken = correlationtoken2;
            this.sendEmailToAssignee.From = null;
            this.sendEmailToAssignee.Headers = null;
            this.sendEmailToAssignee.IncludeStatus = false;
            this.sendEmailToAssignee.Name = "sendEmailToAssignee";
            this.sendEmailToAssignee.Subject = null;
            this.sendEmailToAssignee.To = null;
            this.sendEmailToAssignee.MethodInvoking += new System.EventHandler(this.sendEmailToAssignee_MethodInvoking);
            // 
            // CzyOdroczonaWysyłka
            // 
            this.CzyOdroczonaWysyłka.Activities.Add(this.ifOdroczonaWysylka);
            this.CzyOdroczonaWysyłka.Name = "CzyOdroczonaWysyłka";
            // 
            // setState1
            // 
            this.setState1.CorrelationToken = correlationtoken2;
            this.setState1.Name = "setState1";
            this.setState1.State = 16;
            // 
            // CzyWiadomośćWysłana
            // 
            this.CzyWiadomośćWysłana.Activities.Add(this.ifWiadomoscWyslana);
            this.CzyWiadomośćWysłana.Name = "CzyWiadomośćWysłana";
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "START";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            activitybind2.Name = "ObslugaWiadomosci";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken2;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind1.Name = "ObslugaWiadomosci";
            activitybind1.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // ObslugaWiadomosci
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity2);
            this.Activities.Add(this.CzyWiadomośćWysłana);
            this.Activities.Add(this.setState1);
            this.Activities.Add(this.CzyOdroczonaWysyłka);
            this.Activities.Add(this.sendEmailToAssignee);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.delayActivity1);
            this.Activities.Add(this.setState2);
            this.Activities.Add(this.FormatowanieWiadomosci);
            this.Activities.Add(this.WysylkaWiadomosci);
            this.Activities.Add(this.CzyWysłana);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Name = "ObslugaWiadomosci";
            this.CanModifyActivities = false;

        }

        #endregion

        private CancellationHandlerActivity cancellationHandlerActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmailToAssignee;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.SetState setState1;

        private DelayActivity delayActivity1;

        private Microsoft.SharePoint.WorkflowActions.SetState setState2;

        private CodeActivity Update_tabKartyKlientów;

        private SequenceActivity AktualizacjaPowiązanychKrtotek;

        private SuspendActivity suspendActivity1;

        private TerminateActivity terminateActivity1;

        private IfElseBranchActivity Else3;

        private IfElseBranchActivity ifWysylkaZakonczona;

        private CodeActivity Update_Flags;

        private CodeActivity Send_Mail;

        private CodeActivity Set_Body;

        private CodeActivity Set_CC;

        private CodeActivity Set_From;

        private IfElseBranchActivity ifOdroczonaWysylka;

        private IfElseBranchActivity ifWiadomoscWyslana;

        private IfElseActivity CzyWysłana;

        private SequenceActivity WysylkaWiadomosci;

        private SequenceActivity FormatowanieWiadomosci;

        private IfElseActivity CzyOdroczonaWysyłka;

        private IfElseActivity CzyWiadomośćWysłana;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;







































    }
}
