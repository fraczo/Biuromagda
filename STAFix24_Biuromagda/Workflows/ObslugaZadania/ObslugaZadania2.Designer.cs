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

namespace Workflows.ObslugaZadania2
{
    public sealed partial class ObslugaZadania2
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
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListActivity10 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ProsbaOWyciagBankowy = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity9 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ProsbaODokumenty = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity8 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity7 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Else = new System.Workflow.Activities.IfElseBranchActivity();
            this.ProsbaOWyciagBankowy = new System.Workflow.Activities.IfElseBranchActivity();
            this.ProsbaODokumenty = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczenieRBR = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczenieZUS = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczenieVAT = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePDS = new System.Workflow.Activities.IfElseBranchActivity();
            this.RozliczeniePD = new System.Workflow.Activities.IfElseBranchActivity();
            this.Zadanie = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logToHistoryListActivity10
            // 
            this.logToHistoryListActivity10.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity10.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity10.HistoryDescription = "Nieobsługiwany CT";
            this.logToHistoryListActivity10.HistoryOutcome = "";
            this.logToHistoryListActivity10.Name = "logToHistoryListActivity10";
            this.logToHistoryListActivity10.OtherData = "";
            this.logToHistoryListActivity10.UserId = -1;
            // 
            // Manage_ProsbaOWyciagBankowy
            // 
            this.Manage_ProsbaOWyciagBankowy.Name = "Manage_ProsbaOWyciagBankowy";
            this.Manage_ProsbaOWyciagBankowy.ExecuteCode += new System.EventHandler(this.Manage_ProsbaOWyciagBankowy_ExecuteCode);
            // 
            // logToHistoryListActivity9
            // 
            this.logToHistoryListActivity9.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity9.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity9.HistoryDescription = "Prośba o wyciąg bankowy";
            this.logToHistoryListActivity9.HistoryOutcome = "";
            this.logToHistoryListActivity9.Name = "logToHistoryListActivity9";
            this.logToHistoryListActivity9.OtherData = "";
            this.logToHistoryListActivity9.UserId = -1;
            // 
            // Manage_ProsbaODokumenty
            // 
            this.Manage_ProsbaODokumenty.Name = "Manage_ProsbaODokumenty";
            this.Manage_ProsbaODokumenty.ExecuteCode += new System.EventHandler(this.Manage_ProsbaODokumenty_ExecuteCode);
            // 
            // logToHistoryListActivity8
            // 
            this.logToHistoryListActivity8.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity8.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity8.HistoryDescription = "Prośba o dokumenty";
            this.logToHistoryListActivity8.HistoryOutcome = "";
            this.logToHistoryListActivity8.Name = "logToHistoryListActivity8";
            this.logToHistoryListActivity8.OtherData = "";
            this.logToHistoryListActivity8.UserId = -1;
            // 
            // logToHistoryListActivity7
            // 
            this.logToHistoryListActivity7.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity7.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity7.HistoryDescription = "Rozliczenie RBR";
            this.logToHistoryListActivity7.HistoryOutcome = "";
            this.logToHistoryListActivity7.Name = "logToHistoryListActivity7";
            this.logToHistoryListActivity7.OtherData = "";
            this.logToHistoryListActivity7.UserId = -1;
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "Rozliczenie ZUS";
            this.logToHistoryListActivity6.HistoryOutcome = "";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "Rozliczenie VAT";
            this.logToHistoryListActivity5.HistoryOutcome = "";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "Rozliczenie PDS";
            this.logToHistoryListActivity4.HistoryOutcome = "";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "Rozliczenie PD";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "Zadanie";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // Else
            // 
            this.Else.Activities.Add(this.logToHistoryListActivity10);
            this.Else.Name = "Else";
            // 
            // ProsbaOWyciagBankowy
            // 
            this.ProsbaOWyciagBankowy.Activities.Add(this.logToHistoryListActivity9);
            this.ProsbaOWyciagBankowy.Activities.Add(this.Manage_ProsbaOWyciagBankowy);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isProsbaOWyciagBankowy);
            this.ProsbaOWyciagBankowy.Condition = codecondition1;
            this.ProsbaOWyciagBankowy.Name = "ProsbaOWyciagBankowy";
            // 
            // ProsbaODokumenty
            // 
            this.ProsbaODokumenty.Activities.Add(this.logToHistoryListActivity8);
            this.ProsbaODokumenty.Activities.Add(this.Manage_ProsbaODokumenty);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isProsbaODokumenty);
            this.ProsbaODokumenty.Condition = codecondition2;
            this.ProsbaODokumenty.Name = "ProsbaODokumenty";
            // 
            // RozliczenieRBR
            // 
            this.RozliczenieRBR.Activities.Add(this.logToHistoryListActivity7);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isRozliczenieRBR);
            this.RozliczenieRBR.Condition = codecondition3;
            this.RozliczenieRBR.Name = "RozliczenieRBR";
            // 
            // RozliczenieZUS
            // 
            this.RozliczenieZUS.Activities.Add(this.logToHistoryListActivity6);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isRozliczenieZUS);
            this.RozliczenieZUS.Condition = codecondition4;
            this.RozliczenieZUS.Name = "RozliczenieZUS";
            // 
            // RozliczenieVAT
            // 
            this.RozliczenieVAT.Activities.Add(this.logToHistoryListActivity5);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isRozliczenieVAT);
            this.RozliczenieVAT.Condition = codecondition5;
            this.RozliczenieVAT.Name = "RozliczenieVAT";
            // 
            // RozliczeniePDS
            // 
            this.RozliczeniePDS.Activities.Add(this.logToHistoryListActivity4);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isRozliczeniePDS);
            this.RozliczeniePDS.Condition = codecondition6;
            this.RozliczeniePDS.Name = "RozliczeniePDS";
            // 
            // RozliczeniePD
            // 
            this.RozliczeniePD.Activities.Add(this.logToHistoryListActivity3);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isRozliczeniePD);
            this.RozliczeniePD.Condition = codecondition7;
            this.RozliczeniePD.Name = "RozliczeniePD";
            // 
            // Zadanie
            // 
            this.Zadanie.Activities.Add(this.logToHistoryListActivity2);
            codecondition8.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isZadanie);
            this.Zadanie.Condition = codecondition8;
            this.Zadanie.Name = "Zadanie";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.Zadanie);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePD);
            this.ifElseActivity1.Activities.Add(this.RozliczeniePDS);
            this.ifElseActivity1.Activities.Add(this.RozliczenieVAT);
            this.ifElseActivity1.Activities.Add(this.RozliczenieZUS);
            this.ifElseActivity1.Activities.Add(this.RozliczenieRBR);
            this.ifElseActivity1.Activities.Add(this.ProsbaODokumenty);
            this.ifElseActivity1.Activities.Add(this.ProsbaOWyciagBankowy);
            this.ifElseActivity1.Activities.Add(this.Else);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "Start";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            activitybind2.Name = "ObslugaZadania2";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "ObslugaZadania2";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind1.Name = "ObslugaZadania2";
            activitybind1.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // ObslugaZadania2
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.ifElseActivity1);
            this.Name = "ObslugaZadania2";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Manage_ProsbaOWyciagBankowy;

        private CodeActivity Manage_ProsbaODokumenty;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity10;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity9;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity8;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity7;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private IfElseBranchActivity Else;

        private IfElseBranchActivity ProsbaOWyciagBankowy;

        private IfElseBranchActivity ProsbaODokumenty;

        private IfElseBranchActivity RozliczenieRBR;

        private IfElseBranchActivity RozliczenieZUS;

        private IfElseBranchActivity RozliczenieVAT;

        private IfElseBranchActivity RozliczeniePDS;

        private IfElseBranchActivity RozliczeniePD;

        private IfElseBranchActivity Zadanie;

        private IfElseActivity ifElseActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;






    }
}
