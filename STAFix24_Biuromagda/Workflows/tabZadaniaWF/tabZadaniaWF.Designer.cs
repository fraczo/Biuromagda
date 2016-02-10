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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition22 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            this.Manage_Cmd_Anuluj1 = new System.Workflow.Activities.CodeActivity();
            this.logCase4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Cmd_WyslijInfoIZakoncz = new System.Workflow.Activities.CodeActivity();
            this.logCase3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Mange_Cmd_WyslijInfo1 = new System.Workflow.Activities.CodeActivity();
            this.logCase2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Cmd_Zatwierdz1 = new System.Workflow.Activities.CodeActivity();
            this.logCase = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this._Anuluj = new System.Workflow.Activities.IfElseBranchActivity();
            this._WyślijInfoIZakoncz = new System.Workflow.Activities.IfElseBranchActivity();
            this._WyslijInfo = new System.Workflow.Activities.IfElseBranchActivity();
            this._Zatwierdz = new System.Workflow.Activities.IfElseBranchActivity();
            this.Set_Status_Obsluga4 = new System.Workflow.Activities.CodeActivity();
            this.Set_Status_Obsluga3 = new System.Workflow.Activities.CodeActivity();
            this.Manage_POD3 = new System.Workflow.Activities.CodeActivity();
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
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.Set_KontoOperatora = new System.Workflow.Activities.CodeActivity();
            this.SetTitle = new System.Workflow.Activities.CodeActivity();
            this.TestStatus = new System.Workflow.Activities.IfElseActivity();
            this.Get_Status = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Manage_Cmd_Anuluj1
            // 
            this.Manage_Cmd_Anuluj1.Name = "Manage_Cmd_Anuluj1";
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
            // Manage_Cmd_Zatwierdz1
            // 
            this.Manage_Cmd_Zatwierdz1.Name = "Manage_Cmd_Zatwierdz1";
            this.Manage_Cmd_Zatwierdz1.ExecuteCode += new System.EventHandler(this.Manage_Cmd_Zatwierdz1_ExecuteCode);
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
            this._Anuluj.Condition = codecondition1;
            this._Anuluj.Name = "_Anuluj";
            // 
            // _WyślijInfoIZakoncz
            // 
            this._WyślijInfoIZakoncz.Activities.Add(this.logCase3);
            this._WyślijInfoIZakoncz.Activities.Add(this.Manage_Cmd_WyslijInfoIZakoncz);
            this._WyślijInfoIZakoncz.Condition = codecondition2;
            this._WyślijInfoIZakoncz.Name = "_WyślijInfoIZakoncz";
            // 
            // _WyslijInfo
            // 
            this._WyslijInfo.Activities.Add(this.logCase2);
            this._WyslijInfo.Activities.Add(this.Mange_Cmd_WyslijInfo1);
            this._WyslijInfo.Condition = codecondition3;
            this._WyslijInfo.Name = "_WyslijInfo";
            // 
            // _Zatwierdz
            // 
            this._Zatwierdz.Activities.Add(this.logCase);
            this._Zatwierdz.Activities.Add(this.Manage_Cmd_Zatwierdz1);
            this._Zatwierdz.Condition = codecondition4;
            this._Zatwierdz.Name = "_Zatwierdz";
            // 
            // Set_Status_Obsluga4
            // 
            this.Set_Status_Obsluga4.Name = "Set_Status_Obsluga4";
            // 
            // Set_Status_Obsluga3
            // 
            this.Set_Status_Obsluga3.Name = "Set_Status_Obsluga3";
            // 
            // Manage_POD3
            // 
            this.Manage_POD3.Name = "Manage_POD3";
            // 
            // Manage_POD2
            // 
            this.Manage_POD2.Name = "Manage_POD2";
            // 
            // Manage_POD
            // 
            this.Manage_POD.Name = "Manage_POD";
            // 
            // Set_Status_Obsluga2
            // 
            this.Set_Status_Obsluga2.Name = "Set_Status_Obsluga2";
            // 
            // Set_Operator2
            // 
            this.Set_Operator2.Name = "Set_Operator2";
            // 
            // Set_Status_Obsluga
            // 
            this.Set_Status_Obsluga.Name = "Set_Status_Obsluga";
            // 
            // Set_Operator
            // 
            this.Set_Operator.Name = "Set_Operator";
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
            this.ifStatusNowe4.Condition = codecondition5;
            this.ifStatusNowe4.Name = "ifStatusNowe4";
            // 
            // ifStatusNowe3
            // 
            this.ifStatusNowe3.Activities.Add(this.Set_Status_Obsluga3);
            this.ifStatusNowe3.Condition = codecondition6;
            this.ifStatusNowe3.Name = "ifStatusNowe3";
            // 
            // RozliczenieZBiuremRachunkowym
            // 
            this.RozliczenieZBiuremRachunkowym.Condition = codecondition7;
            this.RozliczenieZBiuremRachunkowym.Name = "RozliczenieZBiuremRachunkowym";
            // 
            // RozliczeniePodatkuVAT
            // 
            this.RozliczeniePodatkuVAT.Condition = codecondition8;
            this.RozliczeniePodatkuVAT.Name = "RozliczeniePodatkuVAT";
            // 
            // RozliczeniePodatkuDochodowegWspólnika
            // 
            this.RozliczeniePodatkuDochodowegWspólnika.Activities.Add(this.Manage_POD3);
            this.RozliczeniePodatkuDochodowegWspólnika.Condition = codecondition9;
            this.RozliczeniePodatkuDochodowegWspólnika.Name = "RozliczeniePodatkuDochodowegWspólnika";
            // 
            // RozliczeniePodatkuDochodowegoSpółki
            // 
            this.RozliczeniePodatkuDochodowegoSpółki.Activities.Add(this.Manage_POD2);
            this.RozliczeniePodatkuDochodowegoSpółki.Condition = codecondition10;
            this.RozliczeniePodatkuDochodowegoSpółki.Name = "RozliczeniePodatkuDochodowegoSpółki";
            // 
            // RozliczeniePodatkuDochodowego
            // 
            this.RozliczeniePodatkuDochodowego.Activities.Add(this.Manage_POD);
            this.RozliczeniePodatkuDochodowego.Condition = codecondition11;
            this.RozliczeniePodatkuDochodowego.Name = "RozliczeniePodatkuDochodowego";
            // 
            // RozliczenieZUS
            // 
            this.RozliczenieZUS.Condition = codecondition12;
            this.RozliczenieZUS.Name = "RozliczenieZUS";
            // 
            // ifStatusNowe2
            // 
            this.ifStatusNowe2.Activities.Add(this.Set_Operator2);
            this.ifStatusNowe2.Activities.Add(this.Set_Status_Obsluga2);
            this.ifStatusNowe2.Condition = codecondition13;
            this.ifStatusNowe2.Name = "ifStatusNowe2";
            // 
            // ProśbaOPrzedslanieWyciaguBankowego
            // 
            this.ProśbaOPrzedslanieWyciaguBankowego.Condition = codecondition14;
            this.ProśbaOPrzedslanieWyciaguBankowego.Name = "ProśbaOPrzedslanieWyciaguBankowego";
            // 
            // ProśbaODokumenty
            // 
            this.ProśbaODokumenty.Condition = codecondition15;
            this.ProśbaODokumenty.Name = "ProśbaODokumenty";
            // 
            // ifStatusNowe
            // 
            this.ifStatusNowe.Activities.Add(this.Set_Operator);
            this.ifStatusNowe.Activities.Add(this.Set_Status_Obsluga);
            this.ifStatusNowe.Condition = codecondition16;
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
            this.ifCommandExist.Condition = codecondition17;
            this.ifCommandExist.Name = "ifCommandExist";
            // 
            // Create_Message
            // 
            this.Create_Message.Name = "Create_Message";
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
            // 
            // Wiadomości
            // 
            this.Wiadomości.Activities.Add(this.logWiadomość);
            this.Wiadomości.Activities.Add(this.ifStatus_Nowe4);
            this.Wiadomości.Activities.Add(this.Create_Message);
            this.Wiadomości.Condition = codecondition18;
            this.Wiadomości.Name = "Wiadomości";
            // 
            // Formatki
            // 
            this.Formatki.Activities.Add(this.logFormatka);
            this.Formatki.Activities.Add(this.Set_KEY2);
            this.Formatki.Activities.Add(this.ifElseActivity1);
            this.Formatki.Activities.Add(this.ifStatus_Nowe3);
            this.Formatki.Condition = codecondition19;
            this.Formatki.Name = "Formatki";
            // 
            // Komunikaty
            // 
            this.Komunikaty.Activities.Add(this.logKomunikat);
            this.Komunikaty.Activities.Add(this.Set_KEY1);
            this.Komunikaty.Activities.Add(this.ifElseActivity2);
            this.Komunikaty.Activities.Add(this.ifStatus_Nowe2);
            this.Komunikaty.Condition = codecondition20;
            this.Komunikaty.Name = "Komunikaty";
            // 
            // Zadanie
            // 
            this.Zadanie.Activities.Add(this.logZadanie);
            this.Zadanie.Activities.Add(this.Set_Zadanie1);
            this.Zadanie.Activities.Add(this.ifStatus_Nowe);
            this.Zadanie.Condition = codecondition21;
            this.Zadanie.Name = "Zadanie";
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
            this.Active.Condition = codecondition22;
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
            this.Activities.Add(this.Get_Status);
            this.Activities.Add(this.TestStatus);
            this.Activities.Add(this.SetTitle);
            this.Activities.Add(this.Set_KontoOperatora);
            this.Activities.Add(this.UpdateItem);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "tabZadaniaWF";
            this.CanModifyActivities = false;

        }

        #endregion

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

        private CodeActivity Manage_POD3;

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
