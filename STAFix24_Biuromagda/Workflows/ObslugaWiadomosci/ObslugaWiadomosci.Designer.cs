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
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Runtime.CorrelationToken correlationtoken3 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Runtime.CorrelationToken correlationtoken4 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Runtime.CorrelationToken correlationtoken5 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Update_tabKartyKlientów = new System.Workflow.Activities.CodeActivity();
            this.AktualizacjaPowiązanychKrtotek = new System.Workflow.Activities.SequenceActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.setState_Wysłana = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.Mail_Send = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.setState_Wysylka = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.Mail_Setup = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.setState_PrzygotowanieWysyłki = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.setState_Anulowana = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.Else = new System.Workflow.Activities.IfElseBranchActivity();
            this.isMailSent = new System.Workflow.Activities.IfElseBranchActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.CzyWiadomośćWysłana = new System.Workflow.Activities.IfElseActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.setState_Obsługa = new Microsoft.SharePoint.WorkflowActions.SetState();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "Aktualizacja kartotek zakończona";
            this.logToHistoryListActivity6.HistoryOutcome = "";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            // 
            // Update_tabKartyKlientów
            // 
            this.Update_tabKartyKlientów.Name = "Update_tabKartyKlientów";
            this.Update_tabKartyKlientów.ExecuteCode += new System.EventHandler(this.Update_tabKartyKlientów_ExecuteCode);
            // 
            // AktualizacjaPowiązanychKrtotek
            // 
            this.AktualizacjaPowiązanychKrtotek.Activities.Add(this.Update_tabKartyKlientów);
            this.AktualizacjaPowiązanychKrtotek.Activities.Add(this.logToHistoryListActivity6);
            this.AktualizacjaPowiązanychKrtotek.Name = "AktualizacjaPowiązanychKrtotek";
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "Wysyłka zakończona";
            this.logToHistoryListActivity5.HistoryOutcome = "";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            // 
            // setState_Wysłana
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "ObslugaWiadomosci";
            this.setState_Wysłana.CorrelationToken = correlationtoken1;
            this.setState_Wysłana.Name = "setState_Wysłana";
            this.setState_Wysłana.State = 21;
            this.setState_Wysłana.MethodInvoking += new System.EventHandler(this.setState_Wysłana_MethodInvoking);
            // 
            // Mail_Send
            // 
            this.Mail_Send.Name = "Mail_Send";
            this.Mail_Send.ExecuteCode += new System.EventHandler(this.Mail_Send_ExecuteCode);
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "Zlecenie wysyłki";
            this.logToHistoryListActivity4.HistoryOutcome = "";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // setState_Wysylka
            // 
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "ObslugaWiadomosci";
            this.setState_Wysylka.CorrelationToken = correlationtoken2;
            this.setState_Wysylka.Name = "setState_Wysylka";
            this.setState_Wysylka.State = 20;
            // 
            // Mail_Setup
            // 
            this.Mail_Setup.Name = "Mail_Setup";
            this.Mail_Setup.ExecuteCode += new System.EventHandler(this.Mail_Setup_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "Przygotowanie";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // setState_PrzygotowanieWysyłki
            // 
            correlationtoken3.Name = "workflowToken";
            correlationtoken3.OwnerActivityName = "ObslugaWiadomosci";
            this.setState_PrzygotowanieWysyłki.CorrelationToken = correlationtoken3;
            this.setState_PrzygotowanieWysyłki.Name = "setState_PrzygotowanieWysyłki";
            this.setState_PrzygotowanieWysyłki.State = 19;
            this.setState_PrzygotowanieWysyłki.MethodInvoking += new System.EventHandler(this.setState_PrzygotowanieWysyłki_MethodInvoking);
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "STOP: wiadomość oznaczona jako wysłana";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // setState_Anulowana
            // 
            correlationtoken4.Name = "workflowToken";
            correlationtoken4.OwnerActivityName = "ObslugaWiadomosci";
            this.setState_Anulowana.CorrelationToken = correlationtoken4;
            this.setState_Anulowana.Name = "setState_Anulowana";
            this.setState_Anulowana.State = 16;
            this.setState_Anulowana.MethodInvoking += new System.EventHandler(this.setState_Anulowana_MethodInvoking);
            // 
            // Else
            // 
            this.Else.Activities.Add(this.setState_PrzygotowanieWysyłki);
            this.Else.Activities.Add(this.logToHistoryListActivity3);
            this.Else.Activities.Add(this.Mail_Setup);
            this.Else.Activities.Add(this.setState_Wysylka);
            this.Else.Activities.Add(this.logToHistoryListActivity4);
            this.Else.Activities.Add(this.Mail_Send);
            this.Else.Activities.Add(this.setState_Wysłana);
            this.Else.Activities.Add(this.logToHistoryListActivity5);
            this.Else.Activities.Add(this.AktualizacjaPowiązanychKrtotek);
            this.Else.Name = "Else";
            // 
            // isMailSent
            // 
            this.isMailSent.Activities.Add(this.setState_Anulowana);
            this.isMailSent.Activities.Add(this.logToHistoryListActivity1);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFlagaWysłanoUstawiona);
            this.isMailSent.Condition = codecondition1;
            this.isMailSent.Name = "isMailSent";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // CzyWiadomośćWysłana
            // 
            this.CzyWiadomośćWysłana.Activities.Add(this.isMailSent);
            this.CzyWiadomośćWysłana.Activities.Add(this.Else);
            this.CzyWiadomośćWysłana.Name = "CzyWiadomośćWysłana";
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "Obsługa";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // setState_Obsługa
            // 
            correlationtoken5.Name = "workflowToken";
            correlationtoken5.OwnerActivityName = "ObslugaWiadomosci";
            this.setState_Obsługa.CorrelationToken = correlationtoken5;
            this.setState_Obsługa.Name = "setState_Obsługa";
            this.setState_Obsługa.State = 17;
            activitybind2.Name = "ObslugaWiadomosci";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken5;
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
            this.Activities.Add(this.setState_Obsługa);
            this.Activities.Add(this.logToHistoryListActivity2);
            this.Activities.Add(this.CzyWiadomośćWysłana);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Name = "ObslugaWiadomosci";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private CodeActivity Mail_Setup;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.SetState setState_PrzygotowanieWysyłki;

        private Microsoft.SharePoint.WorkflowActions.SetState setState_Anulowana;

        private IfElseBranchActivity Else;

        private Microsoft.SharePoint.WorkflowActions.SetState setState_Wysłana;

        private Microsoft.SharePoint.WorkflowActions.SetState setState_Wysylka;

        private CancellationHandlerActivity cancellationHandlerActivity1;

        private Microsoft.SharePoint.WorkflowActions.SetState setState_Obsługa;

        private CodeActivity Update_tabKartyKlientów;

        private SequenceActivity AktualizacjaPowiązanychKrtotek;

        private CodeActivity Mail_Send;

        private IfElseBranchActivity isMailSent;

        private IfElseActivity CzyWiadomośćWysłana;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;




































































    }
}
