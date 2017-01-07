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
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition5 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition8 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition9 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition10 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition11 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition12 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition13 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition14 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition15 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition16 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition17 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition18 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition19 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition20 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition21 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition22 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition23 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition24 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            this.logUpdateIssueMessageSent = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sendUpdateIssueResults = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Setup_UpdateIssueMessage = new System.Workflow.Activities.CodeActivity();
            this.LogValidationMessageSent = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sendValidationResults = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Setup_ValidationMessage = new System.Workflow.Activities.CodeActivity();
            this.ifVM1MessageExist = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifVMMessageExist = new System.Workflow.Activities.IfElseBranchActivity();
            this.Manage_Cmd_Anuluj1 = new System.Workflow.Activities.CodeActivity();
            this.logCase4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Cmd_WyslijInfoIZakoncz = new System.Workflow.Activities.CodeActivity();
            this.logCase3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Mange_Cmd_WyslijInfo1 = new System.Workflow.Activities.CodeActivity();
            this.logCase2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifElseActivity3 = new System.Workflow.Activities.IfElseActivity();
            this.ReportValidationResults = new System.Workflow.Activities.IfElseActivity();
            this.Preset_Message = new System.Workflow.Activities.CodeActivity();
            this.Manage_Cmd_Zatwierdz1 = new System.Workflow.Activities.CodeActivity();
            this.Reset_ValidationMessage = new System.Workflow.Activities.CodeActivity();
            this.logCase = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this._Anuluj = new System.Workflow.Activities.IfElseBranchActivity();
            this._WyślijInfoIZakoncz = new System.Workflow.Activities.IfElseBranchActivity();
            this._WyslijInfo = new System.Workflow.Activities.IfElseBranchActivity();
            this._Zatwierdz = new System.Workflow.Activities.IfElseBranchActivity();
            this.Set_Status_Obsluga4 = new System.Workflow.Activities.CodeActivity();
            this.Set_Status_Obsluga3 = new System.Workflow.Activities.CodeActivity();
            this.Manage_POD2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_POD = new System.Workflow.Activities.CodeActivity();
            this.Set_Status_Obsluga2 = new System.Workflow.Activities.CodeActivity();
            this.Set_Operator2 = new System.Workflow.Activities.CodeActivity();
            this.Set_Status_Obsluga = new System.Workflow.Activities.CodeActivity();
            this.Set_Operator = new System.Workflow.Activities.CodeActivity();
            this.locCommandInactive = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.SelectCommand = new System.Workflow.Activities.IfElseActivity();
            this.logCommandActive = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifStatusNowe4 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifStatusNowe3 = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczenieZBiuremRachunkowym = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePodatkuVAT = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePodatkuDochodowegWspólnika = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePodatkuDochodowegoSpółki = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePodatkuDochodowego = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczenieZUS = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifStatusNowe2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ProśbaOPrzedslanieWyciaguBankowego = new System.Workflow.Activities.IfElseBranchActivity();
            this.ProśbaODokumenty = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifStatusNowe = new System.Workflow.Activities.IfElseBranchActivity();
            this.Else = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifCommandExist = new System.Workflow.Activities.IfElseBranchActivity();
            this.Create_Message = new System.Workflow.Activities.CodeActivity();
            this.ifStatus_Nowe4 = new System.Workflow.Activities.IfElseActivity();
            this.logWiadomość = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifStatus_Nowe3 = new System.Workflow.Activities.IfElseActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.Set_KEY2 = new System.Workflow.Activities.CodeActivity();
            this.logFormatka = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifStatus_Nowe2 = new System.Workflow.Activities.IfElseActivity();
            this.ifElseActivity2 = new System.Workflow.Activities.IfElseActivity();
            this.Set_KEY1 = new System.Workflow.Activities.CodeActivity();
            this.logKomunikat = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifStatus_Nowe = new System.Workflow.Activities.IfElseActivity();
            this.Set_Zadanie1 = new System.Workflow.Activities.CodeActivity();
            this.logZadanie = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.TestCommand = new System.Workflow.Activities.IfElseActivity();
            this.Get_Command = new System.Workflow.Activities.CodeActivity();
            this.Wiadomości = new System.Workflow.Activities.IfElseBranchActivity();
            this.Formatki = new System.Workflow.Activities.IfElseBranchActivity();
            this.Komunikaty = new System.Workflow.Activities.IfElseBranchActivity();
            this.Zadanie = new System.Workflow.Activities.IfElseBranchActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.logTaskStatus2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ManageCommand = new System.Workflow.Activities.SequenceActivity();
            this.SelectCTGroup = new System.Workflow.Activities.IfElseActivity();
            this.Get_CT = new System.Workflow.Activities.CodeActivity();
            this.logTaskStatus = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.Inactive = new System.Workflow.Activities.IfElseBranchActivity();
            this.Active = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.logEND = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.Set_KontoOperatora = new System.Workflow.Activities.CodeActivity();
            this.SetTitle = new System.Workflow.Activities.CodeActivity();
            this.TestStatus = new System.Workflow.Activities.IfElseActivity();
            this.Get_Status = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logUpdateIssueMessageSent
            // 
            this.logUpdateIssueMessageSent.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logUpdateIssueMessageSent.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logUpdateIssueMessageSent.HistoryDescription = "Update Issue Message";
            this.logUpdateIssueMessageSent.HistoryOutcome = "Wysłana";
            this.logUpdateIssueMessageSent.Name = "logUpdateIssueMessageSent";
            this.logUpdateIssueMessageSent.OtherData = "";
            this.logUpdateIssueMessageSent.UserId = -1;
            // 
            // sendUpdateIssueResults
            // 
            this.sendUpdateIssueResults.BCC = null;
            activitybind1.Name = "tabZadaniaWF";
            activitybind1.Path = "msgBody1";
            this.sendUpdateIssueResults.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "tabZadaniaWF";
            this.sendUpdateIssueResults.CorrelationToken = correlationtoken1;
            this.sendUpdateIssueResults.From = null;
            activitybind2.Name = "tabZadaniaWF";
            activitybind2.Path = "msgHeaders";
            this.sendUpdateIssueResults.IncludeStatus = false;
            this.sendUpdateIssueResults.Name = "sendUpdateIssueResults";
            activitybind3.Name = "tabZadaniaWF";
            activitybind3.Path = "msgSubject1";
            activitybind4.Name = "tabZadaniaWF";
            activitybind4.Path = "msgTo";
            this.sendUpdateIssueResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.HeadersProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.sendUpdateIssueResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.sendUpdateIssueResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            this.sendUpdateIssueResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // Setup_UpdateIssueMessage
            // 
            this.Setup_UpdateIssueMessage.Name = "Setup_UpdateIssueMessage";
            this.Setup_UpdateIssueMessage.ExecuteCode += new System.EventHandler(this.Setup_UpdateIssueMessage_ExecuteCode);
            // 
            // LogValidationMessageSent
            // 
            this.LogValidationMessageSent.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.LogValidationMessageSent.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.LogValidationMessageSent.HistoryDescription = "Validation message";
            this.LogValidationMessageSent.HistoryOutcome = "Wysłana";
            this.LogValidationMessageSent.Name = "LogValidationMessageSent";
            this.LogValidationMessageSent.OtherData = "";
            this.LogValidationMessageSent.UserId = -1;
            // 
            // sendValidationResults
            // 
            this.sendValidationResults.BCC = null;
            activitybind5.Name = "tabZadaniaWF";
            activitybind5.Path = "msgBody";
            this.sendValidationResults.CC = null;
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "tabZadaniaWF";
            this.sendValidationResults.CorrelationToken = correlationtoken2;
            this.sendValidationResults.From = null;
            activitybind6.Name = "tabZadaniaWF";
            activitybind6.Path = "msgHeaders";
            this.sendValidationResults.IncludeStatus = false;
            this.sendValidationResults.Name = "sendValidationResults";
            activitybind7.Name = "tabZadaniaWF";
            activitybind7.Path = "msgSubject";
            activitybind8.Name = "tabZadaniaWF";
            activitybind8.Path = "msgTo";
            this.sendValidationResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.sendValidationResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.sendValidationResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.sendValidationResults.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.HeadersProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // Setup_ValidationMessage
            // 
            this.Setup_ValidationMessage.Name = "Setup_ValidationMessage";
            this.Setup_ValidationMessage.ExecuteCode += new System.EventHandler(this.Setup_ValidationMessage_ExecuteCode);
            // 
            // ifVM1MessageExist
            // 
            this.ifVM1MessageExist.Activities.Add(this.Setup_UpdateIssueMessage);
            this.ifVM1MessageExist.Activities.Add(this.sendUpdateIssueResults);
            this.ifVM1MessageExist.Activities.Add(this.logUpdateIssueMessageSent);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isUpdateIssueMessageExist);
            this.ifVM1MessageExist.Condition = codecondition1;
            this.ifVM1MessageExist.Name = "ifVM1MessageExist";
            // 
            // ifVMMessageExist
            // 
            this.ifVMMessageExist.Activities.Add(this.Setup_ValidationMessage);
            this.ifVMMessageExist.Activities.Add(this.sendValidationResults);
            this.ifVMMessageExist.Activities.Add(this.LogValidationMessageSent);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isValidationMessageExist);
            this.ifVMMessageExist.Condition = codecondition2;
            this.ifVMMessageExist.Name = "ifVMMessageExist";
            // 
            // Manage_Cmd_Anuluj1
            // 
            this.Manage_Cmd_Anuluj1.Name = "Manage_Cmd_Anuluj1";
            this.Manage_Cmd_Anuluj1.ExecuteCode += new System.EventHandler(this.Manage_Cmd_Anuluj_ExecuteCode);
            // 
            // logCase4
            // 
            this.logCase4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCase4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCase4.HistoryDescription = "Case";
            this.logCase4.HistoryOutcome = "Anuluj";
            this.logCase4.Name = "logCase4";
            this.logCase4.OtherData = "";
            this.logCase4.UserId = -1;
            // 
            // Manage_Cmd_WyslijInfoIZakoncz
            // 
            this.Manage_Cmd_WyslijInfoIZakoncz.Name = "Manage_Cmd_WyslijInfoIZakoncz";
            this.Manage_Cmd_WyslijInfoIZakoncz.ExecuteCode += new System.EventHandler(this.Manage_Cmd_WyslijInfoIZakoncz_ExecuteCode);
            // 
            // logCase3
            // 
            this.logCase3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCase3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCase3.HistoryDescription = "Case";
            this.logCase3.HistoryOutcome = "Wyślij info i zakończ";
            this.logCase3.Name = "logCase3";
            this.logCase3.OtherData = "";
            this.logCase3.UserId = -1;
            // 
            // Mange_Cmd_WyslijInfo1
            // 
            this.Mange_Cmd_WyslijInfo1.Name = "Mange_Cmd_WyslijInfo1";
            this.Mange_Cmd_WyslijInfo1.ExecuteCode += new System.EventHandler(this.Mange_Cmd_WyslijInfo_ExecuteCode);
            // 
            // logCase2
            // 
            this.logCase2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCase2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCase2.HistoryDescription = "Case";
            this.logCase2.HistoryOutcome = "Wyślij Info";
            this.logCase2.Name = "logCase2";
            this.logCase2.OtherData = "";
            this.logCase2.UserId = -1;
            // 
            // ifElseActivity3
            // 
            this.ifElseActivity3.Activities.Add(this.ifVM1MessageExist);
            this.ifElseActivity3.Name = "ifElseActivity3";
            // 
            // ReportValidationResults
            // 
            this.ReportValidationResults.Activities.Add(this.ifVMMessageExist);
            this.ReportValidationResults.Name = "ReportValidationResults";
            // 
            // Preset_Message
            // 
            this.Preset_Message.Name = "Preset_Message";
            this.Preset_Message.ExecuteCode += new System.EventHandler(this.Preset_Message_ExecuteCode);
            // 
            // Manage_Cmd_Zatwierdz1
            // 
            this.Manage_Cmd_Zatwierdz1.Name = "Manage_Cmd_Zatwierdz1";
            this.Manage_Cmd_Zatwierdz1.ExecuteCode += new System.EventHandler(this.Manage_Cmd_Zatwierdz_ExecuteCode);
            // 
            // Reset_ValidationMessage
            // 
            this.Reset_ValidationMessage.Name = "Reset_ValidationMessage";
            this.Reset_ValidationMessage.ExecuteCode += new System.EventHandler(this.Reset_ValidationMessage_ExecuteCode);
            // 
            // logCase
            // 
            this.logCase.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCase.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCase.HistoryDescription = "Case";
            this.logCase.HistoryOutcome = "Zatwierdź";
            this.logCase.Name = "logCase";
            this.logCase.OtherData = "";
            this.logCase.UserId = -1;
            // 
            // _Anuluj
            // 
            this._Anuluj.Activities.Add(this.logCase4);
            this._Anuluj.Activities.Add(this.Manage_Cmd_Anuluj1);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCmd_Anuluj);
            this._Anuluj.Condition = codecondition3;
            this._Anuluj.Name = "_Anuluj";
            // 
            // _WyślijInfoIZakoncz
            // 
            this._WyślijInfoIZakoncz.Activities.Add(this.logCase3);
            this._WyślijInfoIZakoncz.Activities.Add(this.Manage_Cmd_WyslijInfoIZakoncz);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCmd_WyslijInfoIZakoncz);
            this._WyślijInfoIZakoncz.Condition = codecondition4;
            this._WyślijInfoIZakoncz.Name = "_WyślijInfoIZakoncz";
            // 
            // _WyslijInfo
            // 
            this._WyslijInfo.Activities.Add(this.logCase2);
            this._WyslijInfo.Activities.Add(this.Mange_Cmd_WyslijInfo1);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCmd_WyslijInfo);
            this._WyslijInfo.Condition = codecondition5;
            this._WyslijInfo.Name = "_WyslijInfo";
            // 
            // _Zatwierdz
            // 
            this._Zatwierdz.Activities.Add(this.logCase);
            this._Zatwierdz.Activities.Add(this.Reset_ValidationMessage);
            this._Zatwierdz.Activities.Add(this.Manage_Cmd_Zatwierdz1);
            this._Zatwierdz.Activities.Add(this.Preset_Message);
            this._Zatwierdz.Activities.Add(this.ReportValidationResults);
            this._Zatwierdz.Activities.Add(this.ifElseActivity3);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCmd_Zatwierdz);
            this._Zatwierdz.Condition = codecondition6;
            this._Zatwierdz.Name = "_Zatwierdz";
            // 
            // Set_Status_Obsluga4
            // 
            this.Set_Status_Obsluga4.Name = "Set_Status_Obsluga4";
            this.Set_Status_Obsluga4.ExecuteCode += new System.EventHandler(this.Set_Status_Obsluga_ExecuteCode);
            // 
            // Set_Status_Obsluga3
            // 
            this.Set_Status_Obsluga3.Name = "Set_Status_Obsluga3";
            this.Set_Status_Obsluga3.ExecuteCode += new System.EventHandler(this.Set_Status_Obsluga_ExecuteCode);
            // 
            // Manage_POD2
            // 
            this.Manage_POD2.Name = "Manage_POD2";
            this.Manage_POD2.ExecuteCode += new System.EventHandler(this.Manage_POD_ExecuteCode);
            // 
            // Manage_POD
            // 
            this.Manage_POD.Name = "Manage_POD";
            this.Manage_POD.ExecuteCode += new System.EventHandler(this.Manage_POD_ExecuteCode);
            // 
            // Set_Status_Obsluga2
            // 
            this.Set_Status_Obsluga2.Name = "Set_Status_Obsluga2";
            this.Set_Status_Obsluga2.ExecuteCode += new System.EventHandler(this.Set_Status_Obsluga_ExecuteCode);
            // 
            // Set_Operator2
            // 
            this.Set_Operator2.Name = "Set_Operator2";
            this.Set_Operator2.ExecuteCode += new System.EventHandler(this.Set_Operator_ExecuteCode);
            // 
            // Set_Status_Obsluga
            // 
            this.Set_Status_Obsluga.Name = "Set_Status_Obsluga";
            this.Set_Status_Obsluga.ExecuteCode += new System.EventHandler(this.Set_Status_Obsluga_ExecuteCode);
            // 
            // Set_Operator
            // 
            this.Set_Operator.Name = "Set_Operator";
            this.Set_Operator.ExecuteCode += new System.EventHandler(this.Set_Operator_ExecuteCode);
            // 
            // locCommandInactive
            // 
            this.locCommandInactive.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.locCommandInactive.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.locCommandInactive.HistoryDescription = "Command";
            this.locCommandInactive.HistoryOutcome = "Inactive";
            this.locCommandInactive.Name = "locCommandInactive";
            this.locCommandInactive.OtherData = "";
            this.locCommandInactive.UserId = -1;
            // 
            // SelectCommand
            // 
            this.SelectCommand.Activities.Add(this._Zatwierdz);
            this.SelectCommand.Activities.Add(this._WyslijInfo);
            this.SelectCommand.Activities.Add(this._WyślijInfoIZakoncz);
            this.SelectCommand.Activities.Add(this._Anuluj);
            this.SelectCommand.Name = "SelectCommand";
            // 
            // logCommandActive
            // 
            this.logCommandActive.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCommandActive.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCommandActive.HistoryDescription = "Command";
            this.logCommandActive.HistoryOutcome = "Active";
            this.logCommandActive.Name = "logCommandActive";
            this.logCommandActive.OtherData = "";
            this.logCommandActive.UserId = -1;
            // 
            // ifStatusNowe4
            // 
            this.ifStatusNowe4.Activities.Add(this.Set_Status_Obsluga4);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isStatus_Nowe);
            this.ifStatusNowe4.Condition = codecondition7;
            this.ifStatusNowe4.Name = "ifStatusNowe4";
            // 
            // ifStatusNowe3
            // 
            this.ifStatusNowe3.Activities.Add(this.Set_Status_Obsluga3);
            codecondition8.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isStatus_Nowe);
            this.ifStatusNowe3.Condition = codecondition8;
            this.ifStatusNowe3.Name = "ifStatusNowe3";
            // 
            // RozliczenieZBiuremRachunkowym
            // 
            codecondition9.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRZBR);
            this.RozliczenieZBiuremRachunkowym.Condition = codecondition9;
            this.RozliczenieZBiuremRachunkowym.Name = "RozliczenieZBiuremRachunkowym";
            // 
            // RozliczeniePodatkuVAT
            // 
            codecondition10.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRPV);
            this.RozliczeniePodatkuVAT.Condition = codecondition10;
            this.RozliczeniePodatkuVAT.Name = "RozliczeniePodatkuVAT";
            // 
            // RozliczeniePodatkuDochodowegWspólnika
            // 
            codecondition11.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRPDW);
            this.RozliczeniePodatkuDochodowegWspólnika.Condition = codecondition11;
            this.RozliczeniePodatkuDochodowegWspólnika.Name = "RozliczeniePodatkuDochodowegWspólnika";
            // 
            // RozliczeniePodatkuDochodowegoSpółki
            // 
            this.RozliczeniePodatkuDochodowegoSpółki.Activities.Add(this.Manage_POD2);
            codecondition12.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRPDS);
            this.RozliczeniePodatkuDochodowegoSpółki.Condition = codecondition12;
            this.RozliczeniePodatkuDochodowegoSpółki.Name = "RozliczeniePodatkuDochodowegoSpółki";
            // 
            // RozliczeniePodatkuDochodowego
            // 
            this.RozliczeniePodatkuDochodowego.Activities.Add(this.Manage_POD);
            codecondition13.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRPD);
            this.RozliczeniePodatkuDochodowego.Condition = codecondition13;
            this.RozliczeniePodatkuDochodowego.Name = "RozliczeniePodatkuDochodowego";
            // 
            // RozliczenieZUS
            // 
            codecondition14.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifRZ);
            this.RozliczenieZUS.Condition = codecondition14;
            this.RozliczenieZUS.Name = "RozliczenieZUS";
            // 
            // ifStatusNowe2
            // 
            this.ifStatusNowe2.Activities.Add(this.Set_Operator2);
            this.ifStatusNowe2.Activities.Add(this.Set_Status_Obsluga2);
            codecondition15.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isStatus_Nowe);
            this.ifStatusNowe2.Condition = codecondition15;
            this.ifStatusNowe2.Name = "ifStatusNowe2";
            // 
            // ProśbaOPrzedslanieWyciaguBankowego
            // 
            this.ProśbaOPrzedslanieWyciaguBankowego.Condition = codecondition16;
            this.ProśbaOPrzedslanieWyciaguBankowego.Name = "ProśbaOPrzedslanieWyciaguBankowego";
            // 
            // ProśbaODokumenty
            // 
            this.ProśbaODokumenty.Condition = codecondition17;
            this.ProśbaODokumenty.Name = "ProśbaODokumenty";
            // 
            // ifStatusNowe
            // 
            this.ifStatusNowe.Activities.Add(this.Set_Operator);
            this.ifStatusNowe.Activities.Add(this.Set_Status_Obsluga);
            codecondition18.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isStatus_Nowe);
            this.ifStatusNowe.Condition = codecondition18;
            this.ifStatusNowe.Name = "ifStatusNowe";
            // 
            // Else
            // 
            this.Else.Activities.Add(this.locCommandInactive);
            this.Else.Name = "Else";
            // 
            // ifCommandExist
            // 
            this.ifCommandExist.Activities.Add(this.logCommandActive);
            this.ifCommandExist.Activities.Add(this.SelectCommand);
            codecondition19.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCommandExist);
            this.ifCommandExist.Condition = codecondition19;
            this.ifCommandExist.Name = "ifCommandExist";
            // 
            // Create_Message
            // 
            this.Create_Message.Name = "Create_Message";
            this.Create_Message.ExecuteCode += new System.EventHandler(this.Create_Message_ExecuteCode);
            // 
            // ifStatus_Nowe4
            // 
            this.ifStatus_Nowe4.Activities.Add(this.ifStatusNowe4);
            this.ifStatus_Nowe4.Name = "ifStatus_Nowe4";
            // 
            // logWiadomość
            // 
            this.logWiadomość.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logWiadomość.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logWiadomość.HistoryDescription = "Case";
            this.logWiadomość.HistoryOutcome = "Wiadomość";
            this.logWiadomość.Name = "logWiadomość";
            this.logWiadomość.OtherData = "";
            this.logWiadomość.UserId = -1;
            // 
            // ifStatus_Nowe3
            // 
            this.ifStatus_Nowe3.Activities.Add(this.ifStatusNowe3);
            this.ifStatus_Nowe3.Name = "ifStatus_Nowe3";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.RozliczenieZUS);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePodatkuDochodowego);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePodatkuDochodowegoSpółki);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePodatkuDochodowegWspólnika);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePodatkuVAT);
            this.ifElseActivity1.Activities.Add(this.RozliczenieZBiuremRachunkowym);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // Set_KEY2
            // 
            this.Set_KEY2.Name = "Set_KEY2";
            this.Set_KEY2.ExecuteCode += new System.EventHandler(this.Set_KEY_ExecuteCode);
            // 
            // logFormatka
            // 
            this.logFormatka.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logFormatka.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logFormatka.HistoryDescription = "Case";
            this.logFormatka.HistoryOutcome = "Formatka";
            this.logFormatka.Name = "logFormatka";
            this.logFormatka.OtherData = "";
            this.logFormatka.UserId = -1;
            // 
            // ifStatus_Nowe2
            // 
            this.ifStatus_Nowe2.Activities.Add(this.ifStatusNowe2);
            this.ifStatus_Nowe2.Name = "ifStatus_Nowe2";
            // 
            // ifElseActivity2
            // 
            this.ifElseActivity2.Activities.Add(this.ProśbaODokumenty);
            this.ifElseActivity2.Activities.Add(this.ProśbaOPrzedslanieWyciaguBankowego);
            this.ifElseActivity2.Enabled = false;
            this.ifElseActivity2.Name = "ifElseActivity2";
            // 
            // Set_KEY1
            // 
            this.Set_KEY1.Name = "Set_KEY1";
            this.Set_KEY1.ExecuteCode += new System.EventHandler(this.Set_KEY_ExecuteCode);
            // 
            // logKomunikat
            // 
            this.logKomunikat.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKomunikat.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKomunikat.HistoryDescription = "Case";
            this.logKomunikat.HistoryOutcome = "Komunikat";
            this.logKomunikat.Name = "logKomunikat";
            this.logKomunikat.OtherData = "";
            this.logKomunikat.UserId = -1;
            // 
            // ifStatus_Nowe
            // 
            this.ifStatus_Nowe.Activities.Add(this.ifStatusNowe);
            this.ifStatus_Nowe.Name = "ifStatus_Nowe";
            // 
            // Set_Zadanie1
            // 
            this.Set_Zadanie1.Name = "Set_Zadanie1";
            this.Set_Zadanie1.ExecuteCode += new System.EventHandler(this.Set_Zadanie_ExecuteCode);
            // 
            // logZadanie
            // 
            this.logZadanie.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logZadanie.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logZadanie.HistoryDescription = "Case";
            this.logZadanie.HistoryOutcome = "Zadanie";
            this.logZadanie.Name = "logZadanie";
            this.logZadanie.OtherData = "";
            this.logZadanie.UserId = -1;
            // 
            // TestCommand
            // 
            this.TestCommand.Activities.Add(this.ifCommandExist);
            this.TestCommand.Activities.Add(this.Else);
            this.TestCommand.Name = "TestCommand";
            // 
            // Get_Command
            // 
            this.Get_Command.Name = "Get_Command";
            this.Get_Command.ExecuteCode += new System.EventHandler(this.Get_Command_ExecuteCode);
            // 
            // Wiadomości
            // 
            this.Wiadomości.Activities.Add(this.logWiadomość);
            this.Wiadomości.Activities.Add(this.ifStatus_Nowe4);
            this.Wiadomości.Activities.Add(this.Create_Message);
            codecondition20.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifWiadomosci);
            this.Wiadomości.Condition = codecondition20;
            this.Wiadomości.Name = "Wiadomości";
            // 
            // Formatki
            // 
            this.Formatki.Activities.Add(this.logFormatka);
            this.Formatki.Activities.Add(this.Set_KEY2);
            this.Formatki.Activities.Add(this.ifElseActivity1);
            this.Formatki.Activities.Add(this.ifStatus_Nowe3);
            codecondition21.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifFormatki);
            this.Formatki.Condition = codecondition21;
            this.Formatki.Name = "Formatki";
            // 
            // Komunikaty
            // 
            this.Komunikaty.Activities.Add(this.logKomunikat);
            this.Komunikaty.Activities.Add(this.Set_KEY1);
            this.Komunikaty.Activities.Add(this.ifElseActivity2);
            this.Komunikaty.Activities.Add(this.ifStatus_Nowe2);
            codecondition22.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifKomunikat);
            this.Komunikaty.Condition = codecondition22;
            this.Komunikaty.Name = "Komunikaty";
            // 
            // Zadanie
            // 
            this.Zadanie.Activities.Add(this.logZadanie);
            this.Zadanie.Activities.Add(this.Set_Zadanie1);
            this.Zadanie.Activities.Add(this.ifStatus_Nowe);
            codecondition23.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.ifZ);
            this.Zadanie.Condition = codecondition23;
            this.Zadanie.Name = "Zadanie";
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind9.Name = "tabZadaniaWF";
            activitybind9.Path = "logErrorMessage_HistoryDescription";
            this.logErrorMessage.HistoryOutcome = "";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // logTaskStatus2
            // 
            this.logTaskStatus2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logTaskStatus2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logTaskStatus2.HistoryDescription = "Status";
            this.logTaskStatus2.HistoryOutcome = "Inactive";
            this.logTaskStatus2.Name = "logTaskStatus2";
            this.logTaskStatus2.OtherData = "";
            this.logTaskStatus2.UserId = -1;
            // 
            // ManageCommand
            // 
            this.ManageCommand.Activities.Add(this.Get_Command);
            this.ManageCommand.Activities.Add(this.TestCommand);
            this.ManageCommand.Name = "ManageCommand";
            // 
            // SelectCTGroup
            // 
            this.SelectCTGroup.Activities.Add(this.Zadanie);
            this.SelectCTGroup.Activities.Add(this.Komunikaty);
            this.SelectCTGroup.Activities.Add(this.Formatki);
            this.SelectCTGroup.Activities.Add(this.Wiadomości);
            this.SelectCTGroup.Name = "SelectCTGroup";
            // 
            // Get_CT
            // 
            this.Get_CT.Name = "Get_CT";
            this.Get_CT.ExecuteCode += new System.EventHandler(this.Get_CT_ExecuteCode);
            // 
            // logTaskStatus
            // 
            this.logTaskStatus.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logTaskStatus.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logTaskStatus.HistoryDescription = "Status";
            this.logTaskStatus.HistoryOutcome = "Active";
            this.logTaskStatus.Name = "logTaskStatus";
            this.logTaskStatus.OtherData = "";
            this.logTaskStatus.UserId = -1;
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // Inactive
            // 
            this.Inactive.Activities.Add(this.logTaskStatus2);
            this.Inactive.Name = "Inactive";
            // 
            // Active
            // 
            this.Active.Activities.Add(this.logTaskStatus);
            this.Active.Activities.Add(this.Get_CT);
            this.Active.Activities.Add(this.SelectCTGroup);
            this.Active.Activities.Add(this.ManageCommand);
            codecondition24.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isActive);
            this.Active.Condition = codecondition24;
            this.Active.Name = "Active";
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
            // logEND
            // 
            this.logEND.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logEND.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logEND.HistoryDescription = "END";
            this.logEND.HistoryOutcome = "";
            this.logEND.Name = "logEND";
            this.logEND.OtherData = "";
            this.logEND.UserId = -1;
            // 
            // UpdateItem
            // 
            this.UpdateItem.Name = "UpdateItem";
            this.UpdateItem.ExecuteCode += new System.EventHandler(this.UpdateItem_ExecuteCode);
            // 
            // Set_KontoOperatora
            // 
            this.Set_KontoOperatora.Name = "Set_KontoOperatora";
            this.Set_KontoOperatora.ExecuteCode += new System.EventHandler(this.Set_KontoOperatora_ExecuteCode);
            // 
            // SetTitle
            // 
            this.SetTitle.Name = "SetTitle";
            this.SetTitle.ExecuteCode += new System.EventHandler(this.SetTitle_ExecuteCode);
            // 
            // TestStatus
            // 
            this.TestStatus.Activities.Add(this.Active);
            this.TestStatus.Activities.Add(this.Inactive);
            this.TestStatus.Name = "TestStatus";
            // 
            // Get_Status
            // 
            this.Get_Status.Name = "Get_Status";
            this.Get_Status.ExecuteCode += new System.EventHandler(this.Get_Status_ExecuteCode);
            activitybind11.Name = "tabZadaniaWF";
            activitybind11.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken2;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind10.Name = "tabZadaniaWF";
            activitybind10.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // tabZadaniaWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Get_Status);
            this.Activities.Add(this.TestStatus);
            this.Activities.Add(this.SetTitle);
            this.Activities.Add(this.Set_KontoOperatora);
            this.Activities.Add(this.UpdateItem);
            this.Activities.Add(this.logEND);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "tabZadaniaWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logUpdateIssueMessageSent;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendUpdateIssueResults;

        private CodeActivity Setup_UpdateIssueMessage;

        private IfElseBranchActivity ifVM1MessageExist;

        private IfElseActivity ifElseActivity3;

        private CodeActivity Preset_Message;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogValidationMessageSent;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logEND;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendValidationResults;

        private CodeActivity Setup_ValidationMessage;

        private IfElseBranchActivity ifVMMessageExist;

        private IfElseActivity ReportValidationResults;

        private CodeActivity Reset_ValidationMessage;

        private CodeActivity Manage_Cmd_Anuluj1;

        private CodeActivity Manage_Cmd_WyslijInfoIZakoncz;

        private CodeActivity Mange_Cmd_WyslijInfo1;

        private CodeActivity Manage_Cmd_Zatwierdz1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCase4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCase3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCase2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCase;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity locCommandInactive;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCommandActive;

        private IfElseBranchActivity _WyslijInfo;

        private IfElseBranchActivity _Zatwierdz;

        private IfElseActivity SelectCommand;

        private IfElseBranchActivity _Anuluj;

        private IfElseBranchActivity _WyślijInfoIZakoncz;

        private SequenceActivity ManageCommand;

        private IfElseBranchActivity Else;

        private IfElseBranchActivity ifCommandExist;

        private IfElseActivity TestCommand;

        private CodeActivity Get_Command;

        private CodeActivity Manage_POD2;

        private CodeActivity Create_Message;

        private CodeActivity Set_Status_Obsluga4;

        private CodeActivity Set_Status_Obsluga3;

        private CodeActivity Set_Status_Obsluga2;

        private CodeActivity Set_Operator2;

        private IfElseBranchActivity ifStatusNowe4;

        private IfElseBranchActivity ifStatusNowe3;

        private IfElseBranchActivity ifStatusNowe2;

        private IfElseActivity ifStatus_Nowe4;

        private IfElseActivity ifStatus_Nowe3;

        private IfElseActivity ifStatus_Nowe2;

        private CodeActivity Manage_POD;

        private CodeActivity Set_Status_Obsluga;

        private CodeActivity Set_Operator;

        private IfElseBranchActivity ifStatusNowe;

        private IfElseActivity ifStatus_Nowe;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logTaskStatus2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logTaskStatus;

        private IfElseBranchActivity Inactive;

        private IfElseBranchActivity Active;

        private IfElseActivity TestStatus;

        private CodeActivity Get_Status;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logWiadomość;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFormatka;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKomunikat;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logZadanie;

        private CodeActivity Set_KEY2;

        private CodeActivity Set_Zadanie1;

        private CodeActivity Set_KEY1;

        private IfElseBranchActivity ProśbaODokumenty;

        private IfElseBranchActivity Zadanie;

        private IfElseBranchActivity RozliczenieZUS;

        private IfElseBranchActivity RozliczeniePodatkuVAT;

        private IfElseBranchActivity RozliczeniePodatkuDochodowegWspólnika;

        private IfElseBranchActivity RozliczeniePodatkuDochodowegoSpółki;

        private IfElseBranchActivity RozliczeniePodatkuDochodowego;

        private IfElseBranchActivity RozliczenieZBiuremRachunkowym;

        private IfElseBranchActivity ProśbaOPrzedslanieWyciaguBankowego;

        private IfElseActivity ifElseActivity1;

        private IfElseActivity ifElseActivity2;

        private IfElseBranchActivity Wiadomości;

        private IfElseBranchActivity Formatki;

        private IfElseBranchActivity Komunikaty;

        private IfElseActivity SelectCTGroup;

        private CodeActivity Get_CT;

        private CodeActivity UpdateItem;

        private CodeActivity Set_KontoOperatora;

        private CodeActivity SetTitle;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private CancellationHandlerActivity cancellationHandlerActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;































































































































    }
}
