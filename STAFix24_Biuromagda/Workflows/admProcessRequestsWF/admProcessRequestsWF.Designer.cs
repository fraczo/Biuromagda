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

namespace Workflows.admProcessRequestsWF
{
    public sealed partial class admProcessRequestsWF
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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition5 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            this.Manage_IFE = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity3 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ObslugaZadan = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity11 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ImportPrzeterminowanychNaleznosci = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity10 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ImportFakturZaObsluge = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity9 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_UsunPrzetworzoneFaktury = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity8 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileIFE = new System.Workflow.Activities.WhileActivity();
            this.Select_IFE = new System.Workflow.Activities.CodeActivity();
            this.Manage_ImportFakturElektronicznych = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity7 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Manage_ADO = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity6 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem2 = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Set_StatusAnulowany = new System.Workflow.Activities.CodeActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.ifElseBranchActivity1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifObslugaZadan = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifImportPrzeterminowanychNaleznosci = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifImportFakturZaObslugę = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifUsunPrzetworzoneFaktury = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifImportFakturElektroniczny = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifObsługaADO = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Reporting = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem = new System.Workflow.Activities.CodeActivity();
            this.Case = new System.Workflow.Activities.SequenceActivity();
            this.Preset = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Manage_IFE
            // 
            this.Manage_IFE.Name = "Manage_IFE";
            this.Manage_IFE.ExecuteCode += new System.EventHandler(this.Manage_IFE_ExecuteCode);
            // 
            // logToHistoryListActivity3
            // 
            this.logToHistoryListActivity3.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity3.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity3.HistoryDescription = "Case";
            this.logToHistoryListActivity3.HistoryOutcome = "Else";
            this.logToHistoryListActivity3.Name = "logToHistoryListActivity3";
            this.logToHistoryListActivity3.OtherData = "";
            this.logToHistoryListActivity3.UserId = -1;
            // 
            // Manage_ObslugaZadan
            // 
            this.Manage_ObslugaZadan.Name = "Manage_ObslugaZadan";
            this.Manage_ObslugaZadan.ExecuteCode += new System.EventHandler(this.Manage_ObslugaZadan_ExecuteCode);
            // 
            // logToHistoryListActivity11
            // 
            this.logToHistoryListActivity11.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity11.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity11.HistoryDescription = "Case";
            this.logToHistoryListActivity11.HistoryOutcome = "Obsługa zadań";
            this.logToHistoryListActivity11.Name = "logToHistoryListActivity11";
            this.logToHistoryListActivity11.OtherData = "";
            this.logToHistoryListActivity11.UserId = -1;
            // 
            // Manage_ImportPrzeterminowanychNaleznosci
            // 
            this.Manage_ImportPrzeterminowanychNaleznosci.Name = "Manage_ImportPrzeterminowanychNaleznosci";
            this.Manage_ImportPrzeterminowanychNaleznosci.ExecuteCode += new System.EventHandler(this.Manage_ImportPrzeterminowanychNaleznosci_ExecuteCode);
            // 
            // logToHistoryListActivity10
            // 
            this.logToHistoryListActivity10.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity10.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity10.HistoryDescription = "Case";
            this.logToHistoryListActivity10.HistoryOutcome = "Import przeterminowanych należności";
            this.logToHistoryListActivity10.Name = "logToHistoryListActivity10";
            this.logToHistoryListActivity10.OtherData = "";
            this.logToHistoryListActivity10.UserId = -1;
            // 
            // Manage_ImportFakturZaObsluge
            // 
            this.Manage_ImportFakturZaObsluge.Name = "Manage_ImportFakturZaObsluge";
            this.Manage_ImportFakturZaObsluge.ExecuteCode += new System.EventHandler(this.Manage_ImportFakturZaObsluge_ExecuteCode);
            // 
            // logToHistoryListActivity9
            // 
            this.logToHistoryListActivity9.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity9.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity9.HistoryDescription = "Case";
            this.logToHistoryListActivity9.HistoryOutcome = "Import faktur za obsługę";
            this.logToHistoryListActivity9.Name = "logToHistoryListActivity9";
            this.logToHistoryListActivity9.OtherData = "";
            this.logToHistoryListActivity9.UserId = -1;
            // 
            // Manage_UsunPrzetworzoneFaktury
            // 
            this.Manage_UsunPrzetworzoneFaktury.Name = "Manage_UsunPrzetworzoneFaktury";
            this.Manage_UsunPrzetworzoneFaktury.ExecuteCode += new System.EventHandler(this.Manage_UsunPrzetworzoneFaktury_ExecuteCode);
            // 
            // logToHistoryListActivity8
            // 
            this.logToHistoryListActivity8.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity8.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity8.HistoryDescription = "Case";
            this.logToHistoryListActivity8.HistoryOutcome = "Usuń przetworzone faktury";
            this.logToHistoryListActivity8.Name = "logToHistoryListActivity8";
            this.logToHistoryListActivity8.OtherData = "";
            this.logToHistoryListActivity8.UserId = -1;
            // 
            // whileIFE
            // 
            this.whileIFE.Activities.Add(this.Manage_IFE);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileIFEExist);
            this.whileIFE.Condition = codecondition1;
            this.whileIFE.Name = "whileIFE";
            // 
            // Select_IFE
            // 
            this.Select_IFE.Name = "Select_IFE";
            this.Select_IFE.ExecuteCode += new System.EventHandler(this.Select_IFE_ExecuteCode);
            // 
            // Manage_ImportFakturElektronicznych
            // 
            this.Manage_ImportFakturElektronicznych.Enabled = false;
            this.Manage_ImportFakturElektronicznych.Name = "Manage_ImportFakturElektronicznych";
            this.Manage_ImportFakturElektronicznych.ExecuteCode += new System.EventHandler(this.Manage_ImportFakturElektronicznych_ExecuteCode);
            // 
            // logToHistoryListActivity7
            // 
            this.logToHistoryListActivity7.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity7.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity7.HistoryDescription = "Case";
            this.logToHistoryListActivity7.HistoryOutcome = "Import faktur elektronicznych";
            this.logToHistoryListActivity7.Name = "logToHistoryListActivity7";
            this.logToHistoryListActivity7.OtherData = "";
            this.logToHistoryListActivity7.UserId = -1;
            // 
            // Manage_ADO
            // 
            this.Manage_ADO.Name = "Manage_ADO";
            this.Manage_ADO.ExecuteCode += new System.EventHandler(this.Manage_ADO_ExecuteCode);
            // 
            // logToHistoryListActivity6
            // 
            this.logToHistoryListActivity6.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity6.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity6.HistoryDescription = "Case";
            this.logToHistoryListActivity6.HistoryOutcome = "Obsługa ADO";
            this.logToHistoryListActivity6.Name = "logToHistoryListActivity6";
            this.logToHistoryListActivity6.OtherData = "";
            this.logToHistoryListActivity6.UserId = -1;
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "UpdateItem";
            this.logToHistoryListActivity2.HistoryOutcome = "";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // UpdateItem2
            // 
            this.UpdateItem2.Name = "UpdateItem2";
            this.UpdateItem2.ExecuteCode += new System.EventHandler(this.UpdateItem_ExecuteCode);
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "Status";
            this.logToHistoryListActivity1.HistoryOutcome = "Anulowany";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // Set_StatusAnulowany
            // 
            this.Set_StatusAnulowany.Name = "Set_StatusAnulowany";
            this.Set_StatusAnulowany.ExecuteCode += new System.EventHandler(this.Set_StatusAnulowany_ExecuteCode);
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind1.Name = "admProcessRequestsWF";
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
            // ifElseBranchActivity1
            // 
            this.ifElseBranchActivity1.Activities.Add(this.logToHistoryListActivity3);
            this.ifElseBranchActivity1.Name = "ifElseBranchActivity1";
            // 
            // ifObslugaZadan
            // 
            this.ifObslugaZadan.Activities.Add(this.logToHistoryListActivity11);
            this.ifObslugaZadan.Activities.Add(this.Manage_ObslugaZadan);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isObslugaZadan);
            this.ifObslugaZadan.Condition = codecondition2;
            this.ifObslugaZadan.Name = "ifObslugaZadan";
            // 
            // ifImportPrzeterminowanychNaleznosci
            // 
            this.ifImportPrzeterminowanychNaleznosci.Activities.Add(this.logToHistoryListActivity10);
            this.ifImportPrzeterminowanychNaleznosci.Activities.Add(this.Manage_ImportPrzeterminowanychNaleznosci);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isImportPrzeterminowanychNaleznosci);
            this.ifImportPrzeterminowanychNaleznosci.Condition = codecondition3;
            this.ifImportPrzeterminowanychNaleznosci.Name = "ifImportPrzeterminowanychNaleznosci";
            // 
            // ifImportFakturZaObslugę
            // 
            this.ifImportFakturZaObslugę.Activities.Add(this.logToHistoryListActivity9);
            this.ifImportFakturZaObslugę.Activities.Add(this.Manage_ImportFakturZaObsluge);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isImportFakturZaObsluge);
            this.ifImportFakturZaObslugę.Condition = codecondition4;
            this.ifImportFakturZaObslugę.Name = "ifImportFakturZaObslugę";
            // 
            // ifUsunPrzetworzoneFaktury
            // 
            this.ifUsunPrzetworzoneFaktury.Activities.Add(this.logToHistoryListActivity8);
            this.ifUsunPrzetworzoneFaktury.Activities.Add(this.Manage_UsunPrzetworzoneFaktury);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isUsunPrzetworzoneFaktury);
            this.ifUsunPrzetworzoneFaktury.Condition = codecondition5;
            this.ifUsunPrzetworzoneFaktury.Name = "ifUsunPrzetworzoneFaktury";
            // 
            // ifImportFakturElektroniczny
            // 
            this.ifImportFakturElektroniczny.Activities.Add(this.logToHistoryListActivity7);
            this.ifImportFakturElektroniczny.Activities.Add(this.Manage_ImportFakturElektronicznych);
            this.ifImportFakturElektroniczny.Activities.Add(this.Select_IFE);
            this.ifImportFakturElektroniczny.Activities.Add(this.whileIFE);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isImportFakturElektronicznych);
            this.ifImportFakturElektroniczny.Condition = codecondition6;
            this.ifImportFakturElektroniczny.Name = "ifImportFakturElektroniczny";
            // 
            // ifObsługaADO
            // 
            this.ifObsługaADO.Activities.Add(this.logToHistoryListActivity6);
            this.ifObsługaADO.Activities.Add(this.Manage_ADO);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isObslugaADO);
            this.ifObsługaADO.Condition = codecondition7;
            this.ifObsługaADO.Name = "ifObsługaADO";
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.Activities.Add(this.Set_StatusAnulowany);
            this.faultHandlerActivity1.Activities.Add(this.logToHistoryListActivity1);
            this.faultHandlerActivity1.Activities.Add(this.UpdateItem2);
            this.faultHandlerActivity1.Activities.Add(this.logToHistoryListActivity2);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifObsługaADO);
            this.ifElseActivity1.Activities.Add(this.ifImportFakturElektroniczny);
            this.ifElseActivity1.Activities.Add(this.ifUsunPrzetworzoneFaktury);
            this.ifElseActivity1.Activities.Add(this.ifImportFakturZaObslugę);
            this.ifElseActivity1.Activities.Add(this.ifImportPrzeterminowanychNaleznosci);
            this.ifElseActivity1.Activities.Add(this.ifObslugaZadan);
            this.ifElseActivity1.Activities.Add(this.ifElseBranchActivity1);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
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
            this.logToHistoryListActivity5.HistoryDescription = "UpdateItem";
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
            // Case
            // 
            this.Case.Activities.Add(this.ifElseActivity1);
            this.Case.Name = "Case";
            // 
            // Preset
            // 
            this.Preset.Name = "Preset";
            this.Preset.ExecuteCode += new System.EventHandler(this.Preset_ExecuteCode);
            activitybind3.Name = "admProcessRequestsWF";
            activitybind3.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "admProcessRequestsWF";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind2.Name = "admProcessRequestsWF";
            activitybind2.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // admProcessRequestsWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Preset);
            this.Activities.Add(this.Case);
            this.Activities.Add(this.UpdateItem);
            this.Activities.Add(this.logToHistoryListActivity5);
            this.Activities.Add(this.Reporting);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "admProcessRequestsWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Reporting;

        private CodeActivity Manage_IFE;

        private WhileActivity whileIFE;

        private CodeActivity Select_IFE;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity3;

        private IfElseBranchActivity ifElseBranchActivity1;

        private CodeActivity Manage_ImportFakturElektronicznych;

        private CodeActivity Manage_UsunPrzetworzoneFaktury;

        private CodeActivity Manage_ImportFakturZaObsluge;

        private CodeActivity Manage_ImportPrzeterminowanychNaleznosci;

        private CodeActivity Manage_ObslugaZadan;

        private CodeActivity Manage_ADO;

        private IfElseBranchActivity ifObslugaZadan;

        private IfElseBranchActivity ifImportPrzeterminowanychNaleznosci;

        private IfElseBranchActivity ifImportFakturZaObslugę;

        private IfElseBranchActivity ifUsunPrzetworzoneFaktury;

        private IfElseBranchActivity ifImportFakturElektroniczny;

        private IfElseBranchActivity ifObsługaADO;

        private IfElseActivity ifElseActivity1;

        private SequenceActivity Case;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity11;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity10;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity9;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity8;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity7;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity6;

        private CodeActivity Preset;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private CodeActivity UpdateItem2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private CodeActivity Set_StatusAnulowany;

        private CodeActivity UpdateItem;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;









































    }
}
