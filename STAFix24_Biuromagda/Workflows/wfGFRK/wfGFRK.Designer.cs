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

namespace Workflows.wfGFRK
{
    public sealed partial class wfGFRK
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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            this.logToHistoryListActivity9 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PD5 = new System.Workflow.Activities.CodeActivity();
            this.logFirmaZewnetrzna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity8 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PD4 = new System.Workflow.Activities.CodeActivity();
            this.logFirma = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity14 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PDW = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ZUS3 = new System.Workflow.Activities.CodeActivity();
            this.logOsobaFizyczna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity18 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity16 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_RBR2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity11 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_VAT2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PDS = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ZUS2 = new System.Workflow.Activities.CodeActivity();
            this.logKSH = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity17 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity15 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_RBR = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity10 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_VAT = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PD = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ZUS = new System.Workflow.Activities.CodeActivity();
            this.logKPiR = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.CT_FirmaZewnetrzna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_Firma = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_OsobaFizyczna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KSH = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KPIR = new System.Workflow.Activities.IfElseBranchActivity();
            this.Manage_KK = new System.Workflow.Activities.CodeActivity();
            this.Case_CT = new System.Workflow.Activities.IfElseActivity();
            this.logKlient = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdGetKlientDetails = new System.Workflow.Activities.CodeActivity();
            this.ifValidParams = new System.Workflow.Activities.IfElseBranchActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.Param_Validation = new System.Workflow.Activities.IfElseActivity();
            this.logOkresId = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logKlientId = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdCaptureParams = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifCT_GFRK = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Update_Status = new System.Workflow.Activities.CodeActivity();
            this.Test_CT = new System.Workflow.Activities.IfElseActivity();
            this.cmdInitMsg = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logToHistoryListActivity9
            // 
            this.logToHistoryListActivity9.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity9.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity9.HistoryDescription = "PD";
            this.logToHistoryListActivity9.HistoryOutcome = "";
            this.logToHistoryListActivity9.Name = "logToHistoryListActivity9";
            this.logToHistoryListActivity9.OtherData = "";
            this.logToHistoryListActivity9.UserId = -1;
            // 
            // Manage_PD5
            // 
            this.Manage_PD5.Name = "Manage_PD5";
            this.Manage_PD5.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
            // 
            // logFirmaZewnetrzna
            // 
            this.logFirmaZewnetrzna.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logFirmaZewnetrzna.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logFirmaZewnetrzna.HistoryDescription = "CT";
            this.logFirmaZewnetrzna.HistoryOutcome = "Firma zewnętrzna";
            this.logFirmaZewnetrzna.Name = "logFirmaZewnetrzna";
            this.logFirmaZewnetrzna.OtherData = "";
            this.logFirmaZewnetrzna.UserId = -1;
            // 
            // logToHistoryListActivity8
            // 
            this.logToHistoryListActivity8.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity8.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity8.HistoryDescription = "PD";
            this.logToHistoryListActivity8.HistoryOutcome = "";
            this.logToHistoryListActivity8.Name = "logToHistoryListActivity8";
            this.logToHistoryListActivity8.OtherData = "";
            this.logToHistoryListActivity8.UserId = -1;
            // 
            // Manage_PD4
            // 
            this.Manage_PD4.Name = "Manage_PD4";
            this.Manage_PD4.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
            // 
            // logFirma
            // 
            this.logFirma.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logFirma.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logFirma.HistoryDescription = "CT";
            this.logFirma.HistoryOutcome = "Firma";
            this.logFirma.Name = "logFirma";
            this.logFirma.OtherData = "";
            this.logFirma.UserId = -1;
            // 
            // logToHistoryListActivity14
            // 
            this.logToHistoryListActivity14.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity14.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity14.HistoryDescription = "PDW";
            this.logToHistoryListActivity14.HistoryOutcome = "";
            this.logToHistoryListActivity14.Name = "logToHistoryListActivity14";
            this.logToHistoryListActivity14.OtherData = "";
            this.logToHistoryListActivity14.UserId = -1;
            // 
            // Manage_PDW
            // 
            this.Manage_PDW.Name = "Manage_PDW";
            this.Manage_PDW.ExecuteCode += new System.EventHandler(this.Manage_PDW_ExecuteCode);
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "ZUS";
            this.logToHistoryListActivity4.HistoryOutcome = "";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // Manage_ZUS3
            // 
            this.Manage_ZUS3.Name = "Manage_ZUS3";
            this.Manage_ZUS3.ExecuteCode += new System.EventHandler(this.Manage_ZUS_ExecuteCode);
            // 
            // logOsobaFizyczna
            // 
            this.logOsobaFizyczna.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logOsobaFizyczna.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logOsobaFizyczna.HistoryDescription = "CT";
            this.logOsobaFizyczna.HistoryOutcome = "Osoba fizyczna";
            this.logOsobaFizyczna.Name = "logOsobaFizyczna";
            this.logOsobaFizyczna.OtherData = "";
            this.logOsobaFizyczna.UserId = -1;
            // 
            // logToHistoryListActivity18
            // 
            this.logToHistoryListActivity18.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity18.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity18.HistoryDescription = "Reminders";
            this.logToHistoryListActivity18.HistoryOutcome = "";
            this.logToHistoryListActivity18.Name = "logToHistoryListActivity18";
            this.logToHistoryListActivity18.OtherData = "";
            this.logToHistoryListActivity18.UserId = -1;
            // 
            // Manage_Reminders2
            // 
            this.Manage_Reminders2.Name = "Manage_Reminders2";
            this.Manage_Reminders2.ExecuteCode += new System.EventHandler(this.Manage_Reminders_ExecuteCode);
            // 
            // logToHistoryListActivity16
            // 
            this.logToHistoryListActivity16.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity16.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity16.HistoryDescription = "RBR";
            this.logToHistoryListActivity16.HistoryOutcome = "";
            this.logToHistoryListActivity16.Name = "logToHistoryListActivity16";
            this.logToHistoryListActivity16.OtherData = "";
            this.logToHistoryListActivity16.UserId = -1;
            // 
            // Manage_RBR2
            // 
            this.Manage_RBR2.Name = "Manage_RBR2";
            this.Manage_RBR2.ExecuteCode += new System.EventHandler(this.Manage_RBR_ExecuteCode);
            // 
            // logToHistoryListActivity11
            // 
            this.logToHistoryListActivity11.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity11.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity11.HistoryDescription = "VAT";
            this.logToHistoryListActivity11.HistoryOutcome = "";
            this.logToHistoryListActivity11.Name = "logToHistoryListActivity11";
            this.logToHistoryListActivity11.OtherData = "";
            this.logToHistoryListActivity11.UserId = -1;
            // 
            // Manage_VAT2
            // 
            this.Manage_VAT2.Name = "Manage_VAT2";
            this.Manage_VAT2.ExecuteCode += new System.EventHandler(this.Manage_VAT_ExecuteCode);
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "PDS";
            this.logToHistoryListActivity6.HistoryOutcome = "";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            // 
            // Manage_PDS
            // 
            this.Manage_PDS.Name = "Manage_PDS";
            this.Manage_PDS.ExecuteCode += new System.EventHandler(this.Manage_PDS_ExecuteCode);
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "ZUS";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // Manage_ZUS2
            // 
            this.Manage_ZUS2.Name = "Manage_ZUS2";
            this.Manage_ZUS2.ExecuteCode += new System.EventHandler(this.Manage_ZUS_ExecuteCode);
            // 
            // logKSH
            // 
            this.logKSH.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKSH.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKSH.HistoryDescription = "CT";
            this.logKSH.HistoryOutcome = "KSH";
            this.logKSH.Name = "logKSH";
            this.logKSH.OtherData = "";
            this.logKSH.UserId = -1;
            // 
            // logToHistoryListActivity17
            // 
            this.logToHistoryListActivity17.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity17.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity17.HistoryDescription = "Reminders";
            this.logToHistoryListActivity17.HistoryOutcome = "";
            this.logToHistoryListActivity17.Name = "logToHistoryListActivity17";
            this.logToHistoryListActivity17.OtherData = "";
            this.logToHistoryListActivity17.UserId = -1;
            // 
            // Manage_Reminders
            // 
            this.Manage_Reminders.Name = "Manage_Reminders";
            this.Manage_Reminders.ExecuteCode += new System.EventHandler(this.Manage_Reminders_ExecuteCode);
            // 
            // logToHistoryListActivity15
            // 
            this.logToHistoryListActivity15.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity15.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity15.HistoryDescription = "RBR";
            this.logToHistoryListActivity15.HistoryOutcome = "";
            this.logToHistoryListActivity15.Name = "logToHistoryListActivity15";
            this.logToHistoryListActivity15.OtherData = "";
            this.logToHistoryListActivity15.UserId = -1;
            // 
            // Manage_RBR
            // 
            this.Manage_RBR.Name = "Manage_RBR";
            this.Manage_RBR.ExecuteCode += new System.EventHandler(this.Manage_RBR_ExecuteCode);
            // 
            // logToHistoryListActivity10
            // 
            this.logToHistoryListActivity10.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity10.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity10.HistoryDescription = "VAT";
            this.logToHistoryListActivity10.HistoryOutcome = "";
            this.logToHistoryListActivity10.Name = "logToHistoryListActivity10";
            this.logToHistoryListActivity10.OtherData = "";
            this.logToHistoryListActivity10.UserId = -1;
            // 
            // Manage_VAT
            // 
            this.Manage_VAT.Name = "Manage_VAT";
            this.Manage_VAT.ExecuteCode += new System.EventHandler(this.Manage_VAT_ExecuteCode);
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "PD";
            this.logToHistoryListActivity5.HistoryOutcome = "";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            // 
            // Manage_PD
            // 
            this.Manage_PD.Name = "Manage_PD";
            this.Manage_PD.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "ZUS";
            this.logToHistoryListActivity3.HistoryOutcome = "";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // Manage_ZUS
            // 
            this.Manage_ZUS.Name = "Manage_ZUS";
            this.Manage_ZUS.ExecuteCode += new System.EventHandler(this.Manage_ZUS_ExecuteCode);
            // 
            // logKPiR
            // 
            this.logKPiR.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKPiR.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKPiR.HistoryDescription = "CT";
            this.logKPiR.HistoryOutcome = "KPiR";
            this.logKPiR.Name = "logKPiR";
            this.logKPiR.OtherData = "";
            this.logKPiR.UserId = -1;
            // 
            // CT_FirmaZewnetrzna
            // 
            this.CT_FirmaZewnetrzna.Activities.Add(this.logFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Activities.Add(this.Manage_PD5);
            this.CT_FirmaZewnetrzna.Activities.Add(this.logToHistoryListActivity9);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Condition = codecondition1;
            this.CT_FirmaZewnetrzna.Name = "CT_FirmaZewnetrzna";
            // 
            // CT_Firma
            // 
            this.CT_Firma.Activities.Add(this.logFirma);
            this.CT_Firma.Activities.Add(this.Manage_PD4);
            this.CT_Firma.Activities.Add(this.logToHistoryListActivity8);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirma);
            this.CT_Firma.Condition = codecondition2;
            this.CT_Firma.Name = "CT_Firma";
            // 
            // CT_OsobaFizyczna
            // 
            this.CT_OsobaFizyczna.Activities.Add(this.logOsobaFizyczna);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_ZUS3);
            this.CT_OsobaFizyczna.Activities.Add(this.logToHistoryListActivity4);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_PDW);
            this.CT_OsobaFizyczna.Activities.Add(this.logToHistoryListActivity14);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isOsobaFizyczna);
            this.CT_OsobaFizyczna.Condition = codecondition3;
            this.CT_OsobaFizyczna.Name = "CT_OsobaFizyczna";
            // 
            // CT_KSH
            // 
            this.CT_KSH.Activities.Add(this.logKSH);
            this.CT_KSH.Activities.Add(this.Manage_ZUS2);
            this.CT_KSH.Activities.Add(this.logToHistoryListActivity2);
            this.CT_KSH.Activities.Add(this.Manage_PDS);
            this.CT_KSH.Activities.Add(this.logToHistoryListActivity6);
            this.CT_KSH.Activities.Add(this.Manage_VAT2);
            this.CT_KSH.Activities.Add(this.logToHistoryListActivity11);
            this.CT_KSH.Activities.Add(this.Manage_RBR2);
            this.CT_KSH.Activities.Add(this.logToHistoryListActivity16);
            this.CT_KSH.Activities.Add(this.Manage_Reminders2);
            this.CT_KSH.Activities.Add(this.logToHistoryListActivity18);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKSH);
            this.CT_KSH.Condition = codecondition4;
            this.CT_KSH.Name = "CT_KSH";
            // 
            // CT_KPIR
            // 
            this.CT_KPIR.Activities.Add(this.logKPiR);
            this.CT_KPIR.Activities.Add(this.Manage_ZUS);
            this.CT_KPIR.Activities.Add(this.logToHistoryListActivity3);
            this.CT_KPIR.Activities.Add(this.Manage_PD);
            this.CT_KPIR.Activities.Add(this.logToHistoryListActivity5);
            this.CT_KPIR.Activities.Add(this.Manage_VAT);
            this.CT_KPIR.Activities.Add(this.logToHistoryListActivity10);
            this.CT_KPIR.Activities.Add(this.Manage_RBR);
            this.CT_KPIR.Activities.Add(this.logToHistoryListActivity15);
            this.CT_KPIR.Activities.Add(this.Manage_Reminders);
            this.CT_KPIR.Activities.Add(this.logToHistoryListActivity17);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKPIR);
            this.CT_KPIR.Condition = codecondition5;
            this.CT_KPIR.Name = "CT_KPIR";
            // 
            // Manage_KK
            // 
            this.Manage_KK.Name = "Manage_KK";
            this.Manage_KK.ExecuteCode += new System.EventHandler(this.Manage_KK_ExecuteCode);
            // 
            // Case_CT
            // 
            this.Case_CT.Activities.Add(this.CT_KPIR);
            this.Case_CT.Activities.Add(this.CT_KSH);
            this.Case_CT.Activities.Add(this.CT_OsobaFizyczna);
            this.Case_CT.Activities.Add(this.CT_Firma);
            this.Case_CT.Activities.Add(this.CT_FirmaZewnetrzna);
            this.Case_CT.Name = "Case_CT";
            // 
            // logKlient
            // 
            this.logKlient.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKlient.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKlient.HistoryDescription = "Klient";
            activitybind1.Name = "wfGFRK";
            activitybind1.Path = "logKlient_HistoryOutcome";
            this.logKlient.Name = "logKlient";
            this.logKlient.OtherData = "";
            this.logKlient.UserId = -1;
            this.logKlient.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // cmdGetKlientDetails
            // 
            this.cmdGetKlientDetails.Name = "cmdGetKlientDetails";
            this.cmdGetKlientDetails.ExecuteCode += new System.EventHandler(this.cmdGetKlientDetails_ExecuteCode);
            // 
            // ifValidParams
            // 
            this.ifValidParams.Activities.Add(this.cmdGetKlientDetails);
            this.ifValidParams.Activities.Add(this.logKlient);
            this.ifValidParams.Activities.Add(this.Case_CT);
            this.ifValidParams.Activities.Add(this.Manage_KK);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isValidParams);
            this.ifValidParams.Condition = codecondition6;
            this.ifValidParams.Name = "ifValidParams";
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind2.Name = "wfGFRK";
            activitybind2.Path = "logErrorMessage_HistoryDescription";
            activitybind3.Name = "wfGFRK";
            activitybind3.Path = "logErrorMessage_HistoryOutcome";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // Param_Validation
            // 
            this.Param_Validation.Activities.Add(this.ifValidParams);
            this.Param_Validation.Name = "Param_Validation";
            // 
            // logOkresId
            // 
            this.logOkresId.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logOkresId.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logOkresId.HistoryDescription = "okresId";
            activitybind4.Name = "wfGFRK";
            activitybind4.Path = "logOkresId_HistoryOutcome";
            this.logOkresId.Name = "logOkresId";
            this.logOkresId.OtherData = "";
            this.logOkresId.UserId = -1;
            this.logOkresId.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // logKlientId
            // 
            this.logKlientId.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKlientId.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKlientId.HistoryDescription = "klientId";
            activitybind5.Name = "wfGFRK";
            activitybind5.Path = "logKlientId_HistoryOutcome";
            this.logKlientId.Name = "logKlientId";
            this.logKlientId.OtherData = "";
            this.logKlientId.UserId = -1;
            this.logKlientId.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // cmdCaptureParams
            // 
            this.cmdCaptureParams.Name = "cmdCaptureParams";
            this.cmdCaptureParams.ExecuteCode += new System.EventHandler(this.cmdCaptureParams_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.SystemException);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // ifCT_GFRK
            // 
            this.ifCT_GFRK.Activities.Add(this.cmdCaptureParams);
            this.ifCT_GFRK.Activities.Add(this.logKlientId);
            this.ifCT_GFRK.Activities.Add(this.logOkresId);
            this.ifCT_GFRK.Activities.Add(this.Param_Validation);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isCT_GFRK);
            this.ifCT_GFRK.Condition = codecondition7;
            this.ifCT_GFRK.Name = "ifCT_GFRK";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // Update_Status
            // 
            this.Update_Status.Name = "Update_Status";
            this.Update_Status.ExecuteCode += new System.EventHandler(this.Update_Status_ExecuteCode);
            // 
            // Test_CT
            // 
            this.Test_CT.Activities.Add(this.ifCT_GFRK);
            this.Test_CT.Name = "Test_CT";
            // 
            // cmdInitMsg
            // 
            this.cmdInitMsg.Name = "cmdInitMsg";
            this.cmdInitMsg.ExecuteCode += new System.EventHandler(this.cmdInitMsg_ExecuteCode);
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
            activitybind7.Name = "wfGFRK";
            activitybind7.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "wfGFRK";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind6.Name = "wfGFRK";
            activitybind6.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked_2);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // wfGFRK
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.cmdInitMsg);
            this.Activities.Add(this.Test_CT);
            this.Activities.Add(this.Update_Status);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "wfGFRK";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Manage_KK;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private CodeActivity Update_Status;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity14;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity9;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity8;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity18;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity16;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity11;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity17;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity15;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity10;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private CodeActivity Manage_PDW;

        private CodeActivity Manage_PDS;

        private CodeActivity Manage_ZUS3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logOsobaFizyczna;

        private IfElseBranchActivity CT_OsobaFizyczna;

        private CodeActivity Manage_PD5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirmaZewnetrzna;

        private CodeActivity Manage_PD4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirma;

        private CodeActivity Manage_Reminders2;

        private CodeActivity Manage_RBR2;

        private CodeActivity Manage_VAT2;

        private CodeActivity Manage_ZUS2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKSH;

        private CodeActivity Manage_Reminders;

        private CodeActivity Manage_RBR;

        private CodeActivity Manage_VAT;

        private CodeActivity Manage_PD;

        private CodeActivity Manage_ZUS;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKPiR;

        private IfElseBranchActivity CT_FirmaZewnetrzna;

        private IfElseBranchActivity CT_Firma;

        private IfElseBranchActivity CT_KSH;

        private IfElseBranchActivity CT_KPIR;

        private IfElseActivity Case_CT;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKlient;

        private CodeActivity cmdGetKlientDetails;

        private IfElseBranchActivity ifValidParams;

        private IfElseActivity Param_Validation;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logOkresId;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKlientId;

        private CodeActivity cmdCaptureParams;

        private IfElseBranchActivity ifCT_GFRK;

        private IfElseActivity Test_CT;

        private CodeActivity cmdInitMsg;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;


































    }
}
