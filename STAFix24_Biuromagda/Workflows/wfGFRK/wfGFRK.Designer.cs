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
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            this.Manage_PD5 = new System.Workflow.Activities.CodeActivity();
            this.logFirmaZewnetrzna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_PD4 = new System.Workflow.Activities.CodeActivity();
            this.logFirma = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_VAT3 = new System.Workflow.Activities.CodeActivity();
            this.Manage_PD3 = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS3 = new System.Workflow.Activities.CodeActivity();
            this.logOsobaFizyczna = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_RBR2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_VAT2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_PD2 = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS2 = new System.Workflow.Activities.CodeActivity();
            this.logKSH = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_Reminders = new System.Workflow.Activities.CodeActivity();
            this.Manage_RBR = new System.Workflow.Activities.CodeActivity();
            this.Manage_VAT = new System.Workflow.Activities.CodeActivity();
            this.Manage_PD = new System.Workflow.Activities.CodeActivity();
            this.Manage_ZUS = new System.Workflow.Activities.CodeActivity();
            this.logKPiR = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.CT_FirmaZewnetrzna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_Firma = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_OsobaFizyczna = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KSH = new System.Workflow.Activities.IfElseBranchActivity();
            this.CT_KPIR = new System.Workflow.Activities.IfElseBranchActivity();
            this.Case_CT = new System.Workflow.Activities.IfElseActivity();
            this.logKlient = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdGetKlientDetails = new System.Workflow.Activities.CodeActivity();
            this.ifValidParams = new System.Workflow.Activities.IfElseBranchActivity();
            this.Param_Validation = new System.Workflow.Activities.IfElseActivity();
            this.logOkresId = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logKlientId = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.cmdCaptureParams = new System.Workflow.Activities.CodeActivity();
            this.ifCT_GFRK = new System.Workflow.Activities.IfElseBranchActivity();
            this.Test_CT = new System.Workflow.Activities.IfElseActivity();
            this.cmdInitMsg = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
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
            this.logFirmaZewnetrzna.HistoryDescription = "case";
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
            this.logFirma.HistoryDescription = "case";
            this.logFirma.HistoryOutcome = "Firma";
            this.logFirma.Name = "logFirma";
            this.logFirma.OtherData = "";
            this.logFirma.UserId = -1;
            // 
            // Manage_VAT3
            // 
            this.Manage_VAT3.Name = "Manage_VAT3";
            this.Manage_VAT3.ExecuteCode += new System.EventHandler(this.Manage_VAT_ExecuteCode);
            // 
            // Manage_PD3
            // 
            this.Manage_PD3.Name = "Manage_PD3";
            this.Manage_PD3.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
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
            this.logOsobaFizyczna.HistoryDescription = "case";
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
            // Manage_PD2
            // 
            this.Manage_PD2.Name = "Manage_PD2";
            this.Manage_PD2.ExecuteCode += new System.EventHandler(this.Manage_PD_ExecuteCode);
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
            this.logKSH.HistoryDescription = "case";
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
            this.logKPiR.HistoryDescription = "case";
            this.logKPiR.HistoryOutcome = "KPiR";
            this.logKPiR.Name = "logKPiR";
            this.logKPiR.OtherData = "";
            this.logKPiR.UserId = -1;
            // 
            // CT_FirmaZewnetrzna
            // 
            this.CT_FirmaZewnetrzna.Activities.Add(this.logFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Activities.Add(this.Manage_PD5);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirmaZewnetrzna);
            this.CT_FirmaZewnetrzna.Condition = codecondition1;
            this.CT_FirmaZewnetrzna.Name = "CT_FirmaZewnetrzna";
            // 
            // CT_Firma
            // 
            this.CT_Firma.Activities.Add(this.logFirma);
            this.CT_Firma.Activities.Add(this.Manage_PD4);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isFirma);
            this.CT_Firma.Condition = codecondition2;
            this.CT_Firma.Name = "CT_Firma";
            // 
            // CT_OsobaFizyczna
            // 
            this.CT_OsobaFizyczna.Activities.Add(this.logOsobaFizyczna);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_ZUS3);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_PD3);
            this.CT_OsobaFizyczna.Activities.Add(this.Manage_VAT3);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isOsobaFizyczna);
            this.CT_OsobaFizyczna.Condition = codecondition3;
            this.CT_OsobaFizyczna.Name = "CT_OsobaFizyczna";
            // 
            // CT_KSH
            // 
            this.CT_KSH.Activities.Add(this.logKSH);
            this.CT_KSH.Activities.Add(this.Manage_ZUS2);
            this.CT_KSH.Activities.Add(this.Manage_PD2);
            this.CT_KSH.Activities.Add(this.Manage_VAT2);
            this.CT_KSH.Activities.Add(this.Manage_RBR2);
            this.CT_KSH.Activities.Add(this.Manage_Reminders2);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKSH);
            this.CT_KSH.Condition = codecondition4;
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
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isKPIR);
            this.CT_KPIR.Condition = codecondition5;
            this.CT_KPIR.Name = "CT_KPIR";
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
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isValidParams);
            this.ifValidParams.Condition = codecondition6;
            this.ifValidParams.Name = "ifValidParams";
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
            activitybind2.Name = "wfGFRK";
            activitybind2.Path = "logOkresId_HistoryOutcome";
            this.logOkresId.Name = "logOkresId";
            this.logOkresId.OtherData = "";
            this.logOkresId.UserId = -1;
            this.logOkresId.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // logKlientId
            // 
            this.logKlientId.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logKlientId.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logKlientId.HistoryDescription = "klientId";
            activitybind3.Name = "wfGFRK";
            activitybind3.Path = "logKlientId_HistoryOutcome";
            this.logKlientId.Name = "logKlientId";
            this.logKlientId.OtherData = "";
            this.logKlientId.UserId = -1;
            this.logKlientId.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // cmdCaptureParams
            // 
            this.cmdCaptureParams.Name = "cmdCaptureParams";
            this.cmdCaptureParams.ExecuteCode += new System.EventHandler(this.cmdCaptureParams_ExecuteCode);
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
            activitybind5.Name = "wfGFRK";
            activitybind5.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "wfGFRK";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind4.Name = "wfGFRK";
            activitybind4.Path = "workflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // wfGFRK
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.cmdInitMsg);
            this.Activities.Add(this.Test_CT);
            this.Name = "wfGFRK";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Manage_PD5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirmaZewnetrzna;

        private CodeActivity Manage_PD4;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logFirma;

        private CodeActivity Manage_VAT3;

        private CodeActivity Manage_PD3;

        private CodeActivity Manage_ZUS3;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logOsobaFizyczna;

        private CodeActivity Manage_Reminders2;

        private CodeActivity Manage_RBR2;

        private CodeActivity Manage_VAT2;

        private CodeActivity Manage_PD2;

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
