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

namespace Workflows.wfGFR
{
    public sealed partial class wfGFR
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
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition5 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition8 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition9 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition10 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition11 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            this.logManagedForms = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateMessage = new System.Workflow.Activities.CodeActivity();
            this.Manage_PD5 = new System.Workflow.Activities.CodeActivity();
            this.logFirmaZewnetrzna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PD4 = new System.Workflow.Activities.CodeActivity();
            this.logFirma = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PDW = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS3 = new System.Workflow.Activities.CodeActivity();
            this.logOsobaFizyczna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_RBR2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_VAT2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_PDS = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS2 = new System.Workflow.Activities.CodeActivity();
            this.logKSH = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders = new System.Workflow.Activities.CodeActivity();
            this.Manage_RBR = new System.Workflow.Activities.CodeActivity();
            this.Manage_VAT = new System.Workflow.Activities.CodeActivity();
            this.Manage_PD = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS = new System.Workflow.Activities.CodeActivity();
            this.logKPiR = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifManagedForms = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_FirmaZewnetrzna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_Firma = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_OsobaFizyczna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KSH = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KPIR = new System.Workflow.Activities.IfElseBranchActivity();
            this.ManagedForms = new System.Workflow.Activities.IfElseActivity();
            this.Case_CT = new System.Workflow.Activities.IfElseActivity();
            this.logKlient = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Set_Klient = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_Klienci4 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logRefinedCounter1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Refine_Klienci2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_Klienci3 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_Klienci2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logRefinedCounter = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Refine_Klienci = new System.Workflow.Activities.CodeActivity();
            this.logKlientCounter = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Select_Klienci = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.None = new System.Workflow.Activities.IfElseBranchActivity();
            this.Serwis = new System.Workflow.Activities.IfElseBranchActivity();
            this.TypK = new System.Workflow.Activities.IfElseBranchActivity();
            this.TypK_Serwis = new System.Workflow.Activities.IfElseBranchActivity();
            this.logToHistoryListActivity7 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Status_Anulowany = new System.Workflow.Activities.CodeActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.whileKlient = new System.Workflow.Activities.WhileActivity();
            this.Prepare_List = new System.Workflow.Activities.CodeActivity();
            this.Case = new System.Workflow.Activities.IfElseActivity();
            this.Preset_ot = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifValidRequest = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.ReportTime = new System.Workflow.Activities.CodeActivity();
            this.send_CtrlMsg2 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.ValidateRequest = new System.Workflow.Activities.IfElseActivity();
            this.send_CtrMsg = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // logManagedForms
            // 
            this.logManagedForms.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logManagedForms.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "wfGFR";
            activitybind1.Path = "logManagedForms_HistoryDescription";
            activitybind2.Name = "wfGFR";
            activitybind2.Path = "logManagedForms_HistoryOutcome";
            this.logManagedForms.Name = "logManagedForms";
            this.logManagedForms.OtherData = "";
            this.logManagedForms.UserId = -1;
            this.logManagedForms.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.logManagedForms.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // UpdateMessage
            // 
            this.UpdateMessage.Name = "UpdateMessage";
            this.UpdateMessage.ExecuteCode += new System.EventHandler(this.UpdateMessage_ExecuteCode);
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
            // Manage_PDW
            // 
            this.Manage_PDW.Name = "Manage_PDW";
            this.Manage_PDW.ExecuteCode += new System.EventHandler(this.Manage_PDW_ExecuteCode);
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
            // Manage_Reminders2
            // 
            this.Manage_Reminders2.Name = "Manage_Reminders2";
            this.Manage_Reminders2.ExecuteCode += new System.EventHandler(this.Manage_Reminders_ExecuteCode);
            // 
            // Manage_RBR2
            // 
            this.Manage_RBR2.Name = "Manage_RBR2";
            this.Manage_RBR2.ExecuteCode += new System.EventHandler(this.Manage_RBR_ExecuteCode);
            // 
            // Manage_VAT2
            // 
            this.Manage_VAT2.Name = "Manage_VAT2";
            this.Manage_VAT2.ExecuteCode += new System.EventHandler(this.Manage_VAT_ExecuteCode);
            // 
            // Manage_PDS
            // 
            this.Manage_PDS.Name = "Manage_PDS";
            this.Manage_PDS.ExecuteCode += new System.EventHandler(this.Manage_PDS_ExecuteCode);
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
            // Manage_Reminders
            // 
            this.Manage_Reminders.Name = "Manage_Reminders";
            this.Manage_Reminders.ExecuteCode += new System.EventHandler(this.Manage_Reminders_ExecuteCode);
            // 
            // Manage_RBR
            // 
            this.Manage_RBR.Name = "Manage_RBR";
            this.Manage_RBR.ExecuteCode += new System.EventHandler(this.Manage_RBR_ExecuteCode);
            // 
            // Manage_VAT
            // 
            this.Manage_VAT.Name = "Manage_VAT";
            this.Manage_VAT.ExecuteCode += new System.EventHandler(this.Manage_VAT_ExecuteCode);
            // 
            // Manage_PD
            // 
            this.Manage_PD.Name = "Manage_PD";
            this.Manage_PD.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
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
            // ifManagedForms
            // 
            this.ifManagedForms.Activities.Add(this.UpdateMessage);
            this.ifManagedForms.Activities.Add(this.logManagedForms);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.hasManagedForms);
            this.ifManagedForms.Condition = codecondition1;
            this.ifManagedForms.Name = "ifManagedForms";
            // 
            // CT_FirmaZewnetrzna
            // 
            this.CT_FirmaZewnetrzna.Activities.Add(this.logFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Activities.Add(this.Manage_PD5);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Condition = codecondition2;
            this.CT_FirmaZewnetrzna.Name = "CT_FirmaZewnetrzna";
            // 
            // CT_Firma
            // 
            this.CT_Firma.Activities.Add(this.logFirma);
            this.CT_Firma.Activities.Add(this.Manage_PD4);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirma);
            this.CT_Firma.Condition = codecondition3;
            this.CT_Firma.Name = "CT_Firma";
            // 
            // CT_OsobaFizyczna
            // 
            this.CT_OsobaFizyczna.Activities.Add(this.logOsobaFizyczna);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_ZUS3);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_PDW);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isOsobaFizyczna);
            this.CT_OsobaFizyczna.Condition = codecondition4;
            this.CT_OsobaFizyczna.Name = "CT_OsobaFizyczna";
            // 
            // CT_KSH
            // 
            this.CT_KSH.Activities.Add(this.logKSH);
            this.CT_KSH.Activities.Add(this.Manage_ZUS2);
            this.CT_KSH.Activities.Add(this.Manage_PDS);
            this.CT_KSH.Activities.Add(this.Manage_VAT2);
            this.CT_KSH.Activities.Add(this.Manage_RBR2);
            this.CT_KSH.Activities.Add(this.Manage_Reminders2);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKSH);
            this.CT_KSH.Condition = codecondition5;
            this.CT_KSH.Name = "CT_KSH";
            // 
            // CT_KPIR
            // 
            this.CT_KPIR.Activities.Add(this.logKPiR);
            this.CT_KPIR.Activities.Add(this.Manage_ZUS);
            this.CT_KPIR.Activities.Add(this.Manage_PD);
            this.CT_KPIR.Activities.Add(this.Manage_VAT);
            this.CT_KPIR.Activities.Add(this.Manage_RBR);
            this.CT_KPIR.Activities.Add(this.Manage_Reminders);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKPIR);
            this.CT_KPIR.Condition = codecondition6;
            this.CT_KPIR.Name = "CT_KPIR";
            // 
            // ManagedForms
            // 
            this.ManagedForms.Activities.Add(this.ifManagedForms);
            this.ManagedForms.Name = "ManagedForms";
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
            activitybind3.Name = "wfGFR";
            activitybind3.Path = "logKlient_HistoryOutcome";
            this.logKlient.Name = "logKlient";
            this.logKlient.OtherData = "";
            this.logKlient.UserId = -1;
            this.logKlient.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // Set_Klient
            // 
            this.Set_Klient.Name = "Set_Klient";
            this.Set_Klient.ExecuteCode += new System.EventHandler(this.Set_Klient_ExecuteCode);
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "Aktywnych klientów";
            activitybind4.Name = "wfGFR";
            activitybind4.Path = "logKlientCounter_HistoryOutcome";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            this.logToHistoryListActivity6.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // Select_Klienci4
            // 
            this.Select_Klienci4.Name = "Select_Klienci4";
            this.Select_Klienci4.ExecuteCode += new System.EventHandler(this.Select_Klienci_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "Case";
            this.logToHistoryListActivity3.HistoryOutcome = "None";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // logRefinedCounter1
            // 
            this.logRefinedCounter1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logRefinedCounter1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logRefinedCounter1.HistoryDescription = "Spełniających kryteria";
            activitybind5.Name = "wfGFR";
            activitybind5.Path = "logKlientCounter_HistoryOutcome";
            this.logRefinedCounter1.Name = "logRefinedCounter1";
            this.logRefinedCounter1.OtherData = "";
            this.logRefinedCounter1.UserId = -1;
            this.logRefinedCounter1.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // Refine_Klienci2
            // 
            this.Refine_Klienci2.Name = "Refine_Klienci2";
            this.Refine_Klienci2.ExecuteCode += new System.EventHandler(this.Refine_Klienci_ExecuteCode);
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "Aktywnych klientów";
            activitybind6.Name = "wfGFR";
            activitybind6.Path = "logKlientCounter_HistoryOutcome";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            this.logToHistoryListActivity5.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // Select_Klienci3
            // 
            this.Select_Klienci3.Name = "Select_Klienci3";
            this.Select_Klienci3.ExecuteCode += new System.EventHandler(this.Select_Klienci_ExecuteCode);
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "Case";
            this.logToHistoryListActivity2.HistoryOutcome = "Serwis";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "Aktywnych klientów";
            activitybind7.Name = "wfGFR";
            activitybind7.Path = "logKlientCounter_HistoryOutcome";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            this.logToHistoryListActivity4.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // Select_Klienci2
            // 
            this.Select_Klienci2.Name = "Select_Klienci2";
            this.Select_Klienci2.ExecuteCode += new System.EventHandler(this.Select_Klienci_ExecuteCode);
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "Case";
            this.logToHistoryListActivity1.HistoryOutcome = "TK";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // logRefinedCounter
            // 
            this.logRefinedCounter.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logRefinedCounter.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logRefinedCounter.HistoryDescription = "Spełniających kryteria";
            activitybind8.Name = "wfGFR";
            activitybind8.Path = "logKlientCounter_HistoryOutcome";
            this.logRefinedCounter.Name = "logRefinedCounter";
            this.logRefinedCounter.OtherData = "";
            this.logRefinedCounter.UserId = -1;
            this.logRefinedCounter.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // Refine_Klienci
            // 
            this.Refine_Klienci.Name = "Refine_Klienci";
            this.Refine_Klienci.ExecuteCode += new System.EventHandler(this.Refine_Klienci_ExecuteCode);
            // 
            // logKlientCounter
            // 
            this.logKlientCounter.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKlientCounter.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKlientCounter.HistoryDescription = "Aktywnych klientów";
            activitybind9.Name = "wfGFR";
            activitybind9.Path = "logKlientCounter_HistoryOutcome";
            this.logKlientCounter.Name = "logKlientCounter";
            this.logKlientCounter.OtherData = "";
            this.logKlientCounter.UserId = -1;
            this.logKlientCounter.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // Select_Klienci
            // 
            this.Select_Klienci.Name = "Select_Klienci";
            this.Select_Klienci.ExecuteCode += new System.EventHandler(this.Select_Klienci_ExecuteCode);
            // 
            // logToHistoryListActivity
            // 
            this.logToHistoryListActivity.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity.HistoryDescription = "Case";
            this.logToHistoryListActivity.HistoryOutcome = "TK_Serwis";
            this.logToHistoryListActivity.Name = "logToHistoryListActivity";
            this.logToHistoryListActivity.OtherData = "";
            this.logToHistoryListActivity.UserId = -1;
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.Set_Klient);
            this.sequenceActivity1.Activities.Add(this.logKlient);
            this.sequenceActivity1.Activities.Add(this.Case_CT);
            this.sequenceActivity1.Activities.Add(this.ManagedForms);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // None
            // 
            this.None.Activities.Add(this.logToHistoryListActivity3);
            this.None.Activities.Add(this.Select_Klienci4);
            this.None.Activities.Add(this.logToHistoryListActivity6);
            this.None.Name = "None";
            // 
            // Serwis
            // 
            this.Serwis.Activities.Add(this.logToHistoryListActivity2);
            this.Serwis.Activities.Add(this.Select_Klienci3);
            this.Serwis.Activities.Add(this.logToHistoryListActivity5);
            this.Serwis.Activities.Add(this.Refine_Klienci2);
            this.Serwis.Activities.Add(this.logRefinedCounter1);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isSerwis);
            this.Serwis.Condition = codecondition7;
            this.Serwis.Name = "Serwis";
            // 
            // TypK
            // 
            this.TypK.Activities.Add(this.logToHistoryListActivity1);
            this.TypK.Activities.Add(this.Select_Klienci2);
            this.TypK.Activities.Add(this.logToHistoryListActivity4);
            codecondition8.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isTypK);
            this.TypK.Condition = codecondition8;
            this.TypK.Name = "TypK";
            // 
            // TypK_Serwis
            // 
            this.TypK_Serwis.Activities.Add(this.logToHistoryListActivity);
            this.TypK_Serwis.Activities.Add(this.Select_Klienci);
            this.TypK_Serwis.Activities.Add(this.logKlientCounter);
            this.TypK_Serwis.Activities.Add(this.Refine_Klienci);
            this.TypK_Serwis.Activities.Add(this.logRefinedCounter);
            codecondition9.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isTypK_Serwis);
            this.TypK_Serwis.Condition = codecondition9;
            this.TypK_Serwis.Name = "TypK_Serwis";
            // 
            // logToHistoryListActivity7
            // 
            this.logToHistoryListActivity7.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity7.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity7.HistoryDescription = "Status";
            this.logToHistoryListActivity7.HistoryOutcome = "Anulowane";
            this.logToHistoryListActivity7.Name = "logToHistoryListActivity7";
            this.logToHistoryListActivity7.OtherData = "";
            this.logToHistoryListActivity7.UserId = -1;
            // 
            // Status_Anulowany
            // 
            this.Status_Anulowany.Name = "Status_Anulowany";
            this.Status_Anulowany.ExecuteCode += new System.EventHandler(this.Status_Anulowane_ExecuteCode);
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind10.Name = "wfGFR";
            activitybind10.Path = "logErrorMessage_HistoryDescription";
            activitybind11.Name = "wfGFR";
            activitybind11.Path = "logErrorMessage_HistoryOutcome";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // whileKlient
            // 
            this.whileKlient.Activities.Add(this.sequenceActivity1);
            codecondition10.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileKlientExist);
            this.whileKlient.Condition = codecondition10;
            this.whileKlient.Name = "whileKlient";
            // 
            // Prepare_List
            // 
            this.Prepare_List.Name = "Prepare_List";
            this.Prepare_List.ExecuteCode += new System.EventHandler(this.Prepare_List_ExecuteCode);
            // 
            // Case
            // 
            this.Case.Activities.Add(this.TypK_Serwis);
            this.Case.Activities.Add(this.TypK);
            this.Case.Activities.Add(this.Serwis);
            this.Case.Activities.Add(this.None);
            this.Case.Name = "Case";
            // 
            // Preset_ot
            // 
            this.Preset_ot.Name = "Preset_ot";
            this.Preset_ot.ExecuteCode += new System.EventHandler(this.Preset_ot_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.Activities.Add(this.Status_Anulowany);
            this.faultHandlerActivity1.Activities.Add(this.logToHistoryListActivity7);
            this.faultHandlerActivity1.FaultType = typeof(System.SystemException);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // ifValidRequest
            // 
            this.ifValidRequest.Activities.Add(this.Preset_ot);
            this.ifValidRequest.Activities.Add(this.Case);
            this.ifValidRequest.Activities.Add(this.Prepare_List);
            this.ifValidRequest.Activities.Add(this.whileKlient);
            codecondition11.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isValidRequest);
            this.ifValidRequest.Condition = codecondition11;
            this.ifValidRequest.Name = "ifValidRequest";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // ReportTime
            // 
            this.ReportTime.Name = "ReportTime";
            this.ReportTime.ExecuteCode += new System.EventHandler(this.ReportTime_ExecuteCode);
            // 
            // send_CtrlMsg2
            // 
            this.send_CtrlMsg2.BCC = null;
            activitybind12.Name = "wfGFR";
            activitybind12.Path = "msgBody";
            this.send_CtrlMsg2.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "wfGFR";
            this.send_CtrlMsg2.CorrelationToken = correlationtoken1;
            this.send_CtrlMsg2.From = null;
            this.send_CtrlMsg2.Headers = null;
            this.send_CtrlMsg2.IncludeStatus = false;
            this.send_CtrlMsg2.Name = "send_CtrlMsg2";
            activitybind13.Name = "wfGFR";
            activitybind13.Path = "msgSubject";
            activitybind14.Name = "wfGFR";
            activitybind14.Path = "msgTo";
            this.send_CtrlMsg2.MethodInvoking += new System.EventHandler(this.send_CtrlMsg2_MethodInvoking);
            this.send_CtrlMsg2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.send_CtrlMsg2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.send_CtrlMsg2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // ValidateRequest
            // 
            this.ValidateRequest.Activities.Add(this.ifValidRequest);
            this.ValidateRequest.Name = "ValidateRequest";
            // 
            // send_CtrMsg
            // 
            this.send_CtrMsg.BCC = null;
            activitybind15.Name = "wfGFR";
            activitybind15.Path = "msgBody";
            this.send_CtrMsg.CC = null;
            this.send_CtrMsg.CorrelationToken = correlationtoken1;
            this.send_CtrMsg.From = null;
            this.send_CtrMsg.Headers = null;
            this.send_CtrMsg.IncludeStatus = false;
            this.send_CtrMsg.Name = "send_CtrMsg";
            activitybind16.Name = "wfGFR";
            activitybind16.Path = "msgSubject";
            activitybind17.Name = "wfGFR";
            activitybind17.Path = "msgTo";
            this.send_CtrMsg.MethodInvoking += new System.EventHandler(this.send_CtrlMsg_MethodInvoking);
            this.send_CtrMsg.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.send_CtrMsg.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            this.send_CtrMsg.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            activitybind19.Name = "wfGFR";
            activitybind19.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind18.Name = "wfGFR";
            activitybind18.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            // 
            // wfGFR
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.send_CtrMsg);
            this.Activities.Add(this.ValidateRequest);
            this.Activities.Add(this.send_CtrlMsg2);
            this.Activities.Add(this.ReportTime);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "wfGFR";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity ReportTime;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity7;

        private CodeActivity Status_Anulowany;

        private CodeActivity UpdateMessage;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logManagedForms;

        private IfElseBranchActivity ifManagedForms;

        private IfElseActivity ManagedForms;

        private CodeActivity Preset_ot;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.SendEmail send_CtrlMsg2;

        private Microsoft.SharePoint.WorkflowActions.SendEmail send_CtrMsg;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKlient;

        private CodeActivity Set_Klient;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logRefinedCounter1;

        private CodeActivity Refine_Klienci2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private CodeActivity Manage_PD5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirmaZewnetrzna;

        private CodeActivity Manage_PD4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirma;

        private CodeActivity Manage_PDW;

        private CodeActivity Manage_ZUS3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logOsobaFizyczna;

        private CodeActivity Manage_Reminders2;

        private CodeActivity Manage_RBR2;

        private CodeActivity Manage_VAT2;

        private CodeActivity Manage_PDS;

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

        private IfElseBranchActivity CT_OsobaFizyczna;

        private IfElseBranchActivity CT_KSH;

        private IfElseBranchActivity CT_KPIR;

        private IfElseActivity Case_CT;

        private SequenceActivity sequenceActivity1;

        private CodeActivity Prepare_List;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logRefinedCounter;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logKlientCounter;

        private CodeActivity Refine_Klienci;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity;

        private CodeActivity Select_Klienci4;

        private CodeActivity Select_Klienci3;

        private CodeActivity Select_Klienci2;

        private IfElseBranchActivity None;

        private IfElseBranchActivity Serwis;

        private IfElseBranchActivity TypK;

        private IfElseBranchActivity TypK_Serwis;

        private IfElseActivity Case;

        private IfElseBranchActivity ifValidRequest;

        private IfElseActivity ValidateRequest;

        private WhileActivity whileKlient;

        private CodeActivity Select_Klienci;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;





























































    }
}
