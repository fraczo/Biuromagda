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

namespace Workflows.ImportFakturSWF
{
    public sealed partial class ImportFakturSWF
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
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Runtime.CorrelationToken correlationtoken3 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition5 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition6 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition7 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind12 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind13 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind14 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind15 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind16 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind17 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition8 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind18 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind19 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind20 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind21 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind22 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind23 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind24 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind26 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind25 = new System.Workflow.ComponentModel.ActivityBind();
            this.Delete_SourceData = new System.Workflow.Activities.CodeActivity();
            this.sendEmail7 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Report_InvSent_UpdateKKIssue = new System.Workflow.Activities.CodeActivity();
            this.ifUpdateKKOK = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifUpdateKKIssue = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifElseActivity3 = new System.Workflow.Activities.IfElseActivity();
            this.Update_KK = new System.Workflow.Activities.CodeActivity();
            this.ifSent = new System.Workflow.Activities.IfElseBranchActivity();
            this.sendEmail4 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Report_NoRelatedPDF = new System.Workflow.Activities.CodeActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.Setup_Faktura = new System.Workflow.Activities.CodeActivity();
            this.logPowiazanyDokument = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifHasNoRelatedPDF = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifHasRelatedPDF = new System.Workflow.Activities.IfElseBranchActivity();
            this.sendEmail3 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.Report_KKNieIstnieje = new System.Workflow.Activities.CodeActivity();
            this.PowiązanieZPlikiemPDF = new System.Workflow.Activities.IfElseActivity();
            this.Find_RelatedPDF = new System.Workflow.Activities.CodeActivity();
            this.ifNIeIstniejeKK2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifIstniejeKK2 = new System.Workflow.Activities.IfElseBranchActivity();
            this.Manage_LFE = new System.Workflow.Activities.CodeActivity();
            this.Manage_LFZO = new System.Workflow.Activities.CodeActivity();
            this.ifElseActivity2 = new System.Workflow.Activities.IfElseActivity();
            this.Find_KK = new System.Workflow.Activities.CodeActivity();
            this.CopySrcData = new System.Workflow.Activities.CodeActivity();
            this.whileLFE = new System.Workflow.Activities.WhileActivity();
            this.Select_LFE = new System.Workflow.Activities.CodeActivity();
            this.whileLFZO = new System.Workflow.Activities.WhileActivity();
            this.Select_LFZO = new System.Workflow.Activities.CodeActivity();
            this.ObsługaFaktury = new System.Workflow.Activities.SequenceActivity();
            this.ListaFakturElektronicznych = new System.Workflow.Activities.SequenceActivity();
            this.ListaFakturZaObslugę = new System.Workflow.Activities.SequenceActivity();
            this.whileRecord = new System.Workflow.Activities.WhileActivity();
            this.Select_LFDO = new System.Workflow.Activities.CodeActivity();
            this.Preset_kkList = new System.Workflow.Activities.CodeActivity();
            this.Preset_BiuroRachunkowe = new System.Workflow.Activities.CodeActivity();
            this.sequenceActivity2 = new System.Workflow.Activities.SequenceActivity();
            this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem_Anulowany = new System.Workflow.Activities.CodeActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.sendEmail5 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.ListaFakturDoObsługi = new System.Workflow.Activities.SequenceActivity();
            this.UpdateItem_ObslugaFaza2 = new System.Workflow.Activities.CodeActivity();
            this.sendEmail1 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.parallelActivity1 = new System.Workflow.Activities.ParallelActivity();
            this.Init_Lists = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.ifElse = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifValidParams = new System.Workflow.Activities.IfElseBranchActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.sendEmail2 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.logZakonczony = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.UpdateItem_Zakonczony = new System.Workflow.Activities.CodeActivity();
            this.ValidacjaParametrów = new System.Workflow.Activities.IfElseActivity();
            this.logParameters = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.sendEmail6 = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.UpdateItem_ObslugaFaza1 = new System.Workflow.Activities.CodeActivity();
            this.Get_Parameteres = new System.Workflow.Activities.CodeActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // Delete_SourceData
            // 
            this.Delete_SourceData.Name = "Delete_SourceData";
            this.Delete_SourceData.ExecuteCode += new System.EventHandler(this.Delete_SourceData_ExecuteCode);
            // 
            // sendEmail7
            // 
            this.sendEmail7.BCC = null;
            this.sendEmail7.Body = null;
            this.sendEmail7.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "ImportFakturSWF";
            this.sendEmail7.CorrelationToken = correlationtoken1;
            activitybind1.Name = "ImportFakturSWF";
            activitybind1.Path = "msgFrom";
            this.sendEmail7.Headers = null;
            this.sendEmail7.IncludeStatus = false;
            this.sendEmail7.Name = "sendEmail7";
            activitybind2.Name = "ImportFakturSWF";
            activitybind2.Path = "msgSubject";
            activitybind3.Name = "ImportFakturSWF";
            activitybind3.Path = "msgTo";
            this.sendEmail7.MethodInvoking += new System.EventHandler(this.sendEmail7_MethodInvoking);
            this.sendEmail7.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.sendEmail7.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.sendEmail7.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // Report_InvSent_UpdateKKIssue
            // 
            this.Report_InvSent_UpdateKKIssue.Name = "Report_InvSent_UpdateKKIssue";
            this.Report_InvSent_UpdateKKIssue.ExecuteCode += new System.EventHandler(this.Report_InvSent_UpdateKKIssue_ExecuteCode);
            // 
            // ifUpdateKKOK
            // 
            this.ifUpdateKKOK.Activities.Add(this.Delete_SourceData);
            this.ifUpdateKKOK.Name = "ifUpdateKKOK";
            // 
            // ifUpdateKKIssue
            // 
            this.ifUpdateKKIssue.Activities.Add(this.Report_InvSent_UpdateKKIssue);
            this.ifUpdateKKIssue.Activities.Add(this.sendEmail7);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isUpdateKKIssue);
            this.ifUpdateKKIssue.Condition = codecondition1;
            this.ifUpdateKKIssue.Name = "ifUpdateKKIssue";
            // 
            // ifElseActivity3
            // 
            this.ifElseActivity3.Activities.Add(this.ifUpdateKKIssue);
            this.ifElseActivity3.Activities.Add(this.ifUpdateKKOK);
            this.ifElseActivity3.Name = "ifElseActivity3";
            // 
            // Update_KK
            // 
            this.Update_KK.Name = "Update_KK";
            this.Update_KK.ExecuteCode += new System.EventHandler(this.Update_KK_ExecuteCode);
            // 
            // ifSent
            // 
            this.ifSent.Activities.Add(this.Update_KK);
            this.ifSent.Activities.Add(this.ifElseActivity3);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isSent);
            this.ifSent.Condition = codecondition2;
            this.ifSent.Name = "ifSent";
            // 
            // sendEmail4
            // 
            this.sendEmail4.BCC = null;
            this.sendEmail4.Body = null;
            this.sendEmail4.CC = null;
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "ImportFakturSWF";
            this.sendEmail4.CorrelationToken = correlationtoken2;
            activitybind4.Name = "ImportFakturSWF";
            activitybind4.Path = "msgFrom";
            this.sendEmail4.Headers = null;
            this.sendEmail4.IncludeStatus = false;
            this.sendEmail4.Name = "sendEmail4";
            activitybind5.Name = "ImportFakturSWF";
            activitybind5.Path = "msgSubject";
            activitybind6.Name = "ImportFakturSWF";
            activitybind6.Path = "msgTo";
            this.sendEmail4.MethodInvoking += new System.EventHandler(this.sendEmail4_MethodInvoking);
            this.sendEmail4.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.sendEmail4.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.sendEmail4.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // Report_NoRelatedPDF
            // 
            this.Report_NoRelatedPDF.Name = "Report_NoRelatedPDF";
            this.Report_NoRelatedPDF.ExecuteCode += new System.EventHandler(this.Report_NoRelatedPDF_ExecuteCode);
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifSent);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // Setup_Faktura
            // 
            this.Setup_Faktura.Name = "Setup_Faktura";
            this.Setup_Faktura.ExecuteCode += new System.EventHandler(this.Setup_Faktura_ExecuteCode);
            // 
            // logPowiazanyDokument
            // 
            this.logPowiazanyDokument.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logPowiazanyDokument.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logPowiazanyDokument.HistoryDescription = "Powiązany dokument";
            activitybind7.Name = "ImportFakturSWF";
            activitybind7.Path = "logPowiazanyDokument_HistoryOutcome";
            this.logPowiazanyDokument.Name = "logPowiazanyDokument";
            this.logPowiazanyDokument.OtherData = "";
            this.logPowiazanyDokument.UserId = -1;
            this.logPowiazanyDokument.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // ifHasNoRelatedPDF
            // 
            this.ifHasNoRelatedPDF.Activities.Add(this.Report_NoRelatedPDF);
            this.ifHasNoRelatedPDF.Activities.Add(this.sendEmail4);
            this.ifHasNoRelatedPDF.Name = "ifHasNoRelatedPDF";
            // 
            // ifHasRelatedPDF
            // 
            this.ifHasRelatedPDF.Activities.Add(this.logPowiazanyDokument);
            this.ifHasRelatedPDF.Activities.Add(this.Setup_Faktura);
            this.ifHasRelatedPDF.Activities.Add(this.ifElseActivity1);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.hasRelatedPDF);
            this.ifHasRelatedPDF.Condition = codecondition3;
            this.ifHasRelatedPDF.Name = "ifHasRelatedPDF";
            // 
            // sendEmail3
            // 
            this.sendEmail3.BCC = null;
            this.sendEmail3.Body = null;
            this.sendEmail3.CC = null;
            correlationtoken3.Name = "workflowToken";
            correlationtoken3.OwnerActivityName = "ImportFakturSWF";
            this.sendEmail3.CorrelationToken = correlationtoken3;
            activitybind8.Name = "ImportFakturSWF";
            activitybind8.Path = "msgFrom";
            this.sendEmail3.Headers = null;
            this.sendEmail3.IncludeStatus = false;
            this.sendEmail3.Name = "sendEmail3";
            activitybind9.Name = "ImportFakturSWF";
            activitybind9.Path = "msgSubject";
            activitybind10.Name = "ImportFakturSWF";
            activitybind10.Path = "msgTo";
            this.sendEmail3.MethodInvoking += new System.EventHandler(this.sendEmail3_MethodInvoking);
            this.sendEmail3.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            this.sendEmail3.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            this.sendEmail3.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // Report_KKNieIstnieje
            // 
            this.Report_KKNieIstnieje.Name = "Report_KKNieIstnieje";
            this.Report_KKNieIstnieje.ExecuteCode += new System.EventHandler(this.Report_KKNieIstnieje_ExecuteCode);
            // 
            // PowiązanieZPlikiemPDF
            // 
            this.PowiązanieZPlikiemPDF.Activities.Add(this.ifHasRelatedPDF);
            this.PowiązanieZPlikiemPDF.Activities.Add(this.ifHasNoRelatedPDF);
            this.PowiązanieZPlikiemPDF.Name = "PowiązanieZPlikiemPDF";
            // 
            // Find_RelatedPDF
            // 
            this.Find_RelatedPDF.Description = "wyszukuje plik PDF w/g numeru faktury i kodu klienta";
            this.Find_RelatedPDF.Name = "Find_RelatedPDF";
            this.Find_RelatedPDF.ExecuteCode += new System.EventHandler(this.Find_RelatedPDF_ExecuteCode);
            // 
            // ifNIeIstniejeKK2
            // 
            this.ifNIeIstniejeKK2.Activities.Add(this.Report_KKNieIstnieje);
            this.ifNIeIstniejeKK2.Activities.Add(this.sendEmail3);
            this.ifNIeIstniejeKK2.Name = "ifNIeIstniejeKK2";
            // 
            // ifIstniejeKK2
            // 
            this.ifIstniejeKK2.Activities.Add(this.Find_RelatedPDF);
            this.ifIstniejeKK2.Activities.Add(this.PowiązanieZPlikiemPDF);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isIstniejeKK);
            this.ifIstniejeKK2.Condition = codecondition4;
            this.ifIstniejeKK2.Name = "ifIstniejeKK2";
            // 
            // Manage_LFE
            // 
            this.Manage_LFE.Name = "Manage_LFE";
            this.Manage_LFE.ExecuteCode += new System.EventHandler(this.Manage_LFE_ExecuteCode);
            // 
            // Manage_LFZO
            // 
            this.Manage_LFZO.Name = "Manage_LFZO";
            this.Manage_LFZO.ExecuteCode += new System.EventHandler(this.Manage_LFZO_ExecuteCode);
            // 
            // ifElseActivity2
            // 
            this.ifElseActivity2.Activities.Add(this.ifIstniejeKK2);
            this.ifElseActivity2.Activities.Add(this.ifNIeIstniejeKK2);
            this.ifElseActivity2.Name = "ifElseActivity2";
            // 
            // Find_KK
            // 
            this.Find_KK.Name = "Find_KK";
            this.Find_KK.ExecuteCode += new System.EventHandler(this.Find_KK_ExecuteCode);
            // 
            // CopySrcData
            // 
            this.CopySrcData.Description = "sprawdza czy wszystkie dane są potrzebe";
            this.CopySrcData.Name = "CopySrcData";
            this.CopySrcData.ExecuteCode += new System.EventHandler(this.CopySrcData_ExecuteCode);
            // 
            // whileLFE
            // 
            this.whileLFE.Activities.Add(this.Manage_LFE);
            codecondition5.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileLFEExist);
            this.whileLFE.Condition = codecondition5;
            this.whileLFE.Name = "whileLFE";
            // 
            // Select_LFE
            // 
            this.Select_LFE.Name = "Select_LFE";
            this.Select_LFE.ExecuteCode += new System.EventHandler(this.Select_LFE_ExecuteCode);
            // 
            // whileLFZO
            // 
            this.whileLFZO.Activities.Add(this.Manage_LFZO);
            codecondition6.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileLFZOExist);
            this.whileLFZO.Condition = codecondition6;
            this.whileLFZO.Name = "whileLFZO";
            // 
            // Select_LFZO
            // 
            this.Select_LFZO.Name = "Select_LFZO";
            this.Select_LFZO.ExecuteCode += new System.EventHandler(this.Select_LFZO_ExecuteCode);
            // 
            // ObsługaFaktury
            // 
            this.ObsługaFaktury.Activities.Add(this.CopySrcData);
            this.ObsługaFaktury.Activities.Add(this.Find_KK);
            this.ObsługaFaktury.Activities.Add(this.ifElseActivity2);
            this.ObsługaFaktury.Name = "ObsługaFaktury";
            // 
            // ListaFakturElektronicznych
            // 
            this.ListaFakturElektronicznych.Activities.Add(this.Select_LFE);
            this.ListaFakturElektronicznych.Activities.Add(this.whileLFE);
            this.ListaFakturElektronicznych.Name = "ListaFakturElektronicznych";
            // 
            // ListaFakturZaObslugę
            // 
            this.ListaFakturZaObslugę.Activities.Add(this.Select_LFZO);
            this.ListaFakturZaObslugę.Activities.Add(this.whileLFZO);
            this.ListaFakturZaObslugę.Name = "ListaFakturZaObslugę";
            // 
            // whileRecord
            // 
            this.whileRecord.Activities.Add(this.ObsługaFaktury);
            codecondition7.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileRecordExist);
            this.whileRecord.Condition = codecondition7;
            this.whileRecord.Name = "whileRecord";
            // 
            // Select_LFDO
            // 
            this.Select_LFDO.Name = "Select_LFDO";
            this.Select_LFDO.ExecuteCode += new System.EventHandler(this.Select_LFDO_ExecuteCode);
            // 
            // Preset_kkList
            // 
            this.Preset_kkList.Name = "Preset_kkList";
            this.Preset_kkList.ExecuteCode += new System.EventHandler(this.Preset_kkList_ExecuteCode);
            // 
            // Preset_BiuroRachunkowe
            // 
            this.Preset_BiuroRachunkowe.Name = "Preset_BiuroRachunkowe";
            this.Preset_BiuroRachunkowe.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode);
            // 
            // sequenceActivity2
            // 
            this.sequenceActivity2.Activities.Add(this.ListaFakturElektronicznych);
            this.sequenceActivity2.Name = "sequenceActivity2";
            // 
            // sequenceActivity1
            // 
            this.sequenceActivity1.Activities.Add(this.ListaFakturZaObslugę);
            this.sequenceActivity1.Name = "sequenceActivity1";
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "CANCELED";
            this.logToHistoryListActivity1.HistoryOutcome = "";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            // 
            // UpdateItem_Anulowany
            // 
            this.UpdateItem_Anulowany.Name = "UpdateItem_Anulowany";
            this.UpdateItem_Anulowany.ExecuteCode += new System.EventHandler(this.UpdateItem_Anulowany_ExecuteCode);
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind11.Name = "ImportFakturSWF";
            activitybind11.Path = "logErrorMessage_HistoryDescription";
            this.logErrorMessage.HistoryOutcome = "";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // sendEmail5
            // 
            this.sendEmail5.BCC = null;
            this.sendEmail5.Body = null;
            this.sendEmail5.CC = null;
            this.sendEmail5.CorrelationToken = correlationtoken1;
            activitybind12.Name = "ImportFakturSWF";
            activitybind12.Path = "msgFrom";
            this.sendEmail5.Headers = null;
            this.sendEmail5.IncludeStatus = false;
            this.sendEmail5.Name = "sendEmail5";
            activitybind13.Name = "ImportFakturSWF";
            activitybind13.Path = "msgSubject";
            activitybind14.Name = "ImportFakturSWF";
            activitybind14.Path = "msgTo";
            this.sendEmail5.MethodInvoking += new System.EventHandler(this.sendEmail5_MethodInvoking);
            this.sendEmail5.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind14)));
            this.sendEmail5.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind13)));
            this.sendEmail5.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind12)));
            // 
            // ListaFakturDoObsługi
            // 
            this.ListaFakturDoObsługi.Activities.Add(this.Preset_BiuroRachunkowe);
            this.ListaFakturDoObsługi.Activities.Add(this.Preset_kkList);
            this.ListaFakturDoObsługi.Activities.Add(this.Select_LFDO);
            this.ListaFakturDoObsługi.Activities.Add(this.whileRecord);
            this.ListaFakturDoObsługi.Name = "ListaFakturDoObsługi";
            // 
            // UpdateItem_ObslugaFaza2
            // 
            this.UpdateItem_ObslugaFaza2.Name = "UpdateItem_ObslugaFaza2";
            this.UpdateItem_ObslugaFaza2.ExecuteCode += new System.EventHandler(this.UpdateItem_ObslugaFaza2_ExecuteCode);
            // 
            // sendEmail1
            // 
            this.sendEmail1.BCC = null;
            this.sendEmail1.Body = null;
            this.sendEmail1.CC = null;
            this.sendEmail1.CorrelationToken = correlationtoken1;
            activitybind15.Name = "ImportFakturSWF";
            activitybind15.Path = "msgFrom";
            this.sendEmail1.Headers = null;
            this.sendEmail1.IncludeStatus = false;
            this.sendEmail1.Name = "sendEmail1";
            activitybind16.Name = "ImportFakturSWF";
            activitybind16.Path = "msgSubject";
            activitybind17.Name = "ImportFakturSWF";
            activitybind17.Path = "msgTo";
            this.sendEmail1.MethodInvoking += new System.EventHandler(this.sendEmail1_MethodInvoking);
            this.sendEmail1.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind17)));
            this.sendEmail1.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind16)));
            this.sendEmail1.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind15)));
            // 
            // parallelActivity1
            // 
            this.parallelActivity1.Activities.Add(this.sequenceActivity1);
            this.parallelActivity1.Activities.Add(this.sequenceActivity2);
            this.parallelActivity1.Name = "parallelActivity1";
            // 
            // Init_Lists
            // 
            this.Init_Lists.Name = "Init_Lists";
            this.Init_Lists.ExecuteCode += new System.EventHandler(this.Init_Lists_ExecuteCode);
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.Activities.Add(this.UpdateItem_Anulowany);
            this.faultHandlerActivity1.Activities.Add(this.logToHistoryListActivity1);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // ifElse
            // 
            this.ifElse.Activities.Add(this.sendEmail5);
            this.ifElse.Name = "ifElse";
            // 
            // ifValidParams
            // 
            this.ifValidParams.Activities.Add(this.Init_Lists);
            this.ifValidParams.Activities.Add(this.parallelActivity1);
            this.ifValidParams.Activities.Add(this.sendEmail1);
            this.ifValidParams.Activities.Add(this.UpdateItem_ObslugaFaza2);
            this.ifValidParams.Activities.Add(this.ListaFakturDoObsługi);
            codecondition8.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.hasValidParams);
            this.ifValidParams.Condition = codecondition8;
            this.ifValidParams.Name = "ifValidParams";
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
            this.sendEmail2.CorrelationToken = correlationtoken1;
            activitybind18.Name = "ImportFakturSWF";
            activitybind18.Path = "msgFrom";
            this.sendEmail2.Headers = null;
            this.sendEmail2.IncludeStatus = false;
            this.sendEmail2.Name = "sendEmail2";
            activitybind19.Name = "ImportFakturSWF";
            activitybind19.Path = "msgTo";
            activitybind20.Name = "ImportFakturSWF";
            activitybind20.Path = "msgSubject";
            this.sendEmail2.MethodInvoking += new System.EventHandler(this.sendEmail2_MethodInvoking);
            this.sendEmail2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind19)));
            this.sendEmail2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind20)));
            this.sendEmail2.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind18)));
            // 
            // logZakonczony
            // 
            this.logZakonczony.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logZakonczony.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logZakonczony.HistoryDescription = "END";
            this.logZakonczony.HistoryOutcome = "";
            this.logZakonczony.Name = "logZakonczony";
            this.logZakonczony.OtherData = "";
            this.logZakonczony.UserId = -1;
            // 
            // UpdateItem_Zakonczony
            // 
            this.UpdateItem_Zakonczony.Name = "UpdateItem_Zakonczony";
            this.UpdateItem_Zakonczony.ExecuteCode += new System.EventHandler(this.UpdateItem_Zakonczony_ExecuteCode);
            // 
            // ValidacjaParametrów
            // 
            this.ValidacjaParametrów.Activities.Add(this.ifValidParams);
            this.ValidacjaParametrów.Activities.Add(this.ifElse);
            this.ValidacjaParametrów.Name = "ValidacjaParametrów";
            // 
            // logParameters
            // 
            this.logParameters.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logParameters.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logParameters.HistoryDescription = "Parameters";
            activitybind21.Name = "ImportFakturSWF";
            activitybind21.Path = "logParameters_HistoryOutcome";
            this.logParameters.Name = "logParameters";
            this.logParameters.OtherData = "";
            this.logParameters.UserId = -1;
            this.logParameters.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind21)));
            // 
            // sendEmail6
            // 
            this.sendEmail6.BCC = null;
            this.sendEmail6.Body = null;
            this.sendEmail6.CC = null;
            this.sendEmail6.CorrelationToken = correlationtoken1;
            activitybind22.Name = "ImportFakturSWF";
            activitybind22.Path = "msgFrom";
            this.sendEmail6.Headers = null;
            this.sendEmail6.IncludeStatus = false;
            this.sendEmail6.Name = "sendEmail6";
            activitybind23.Name = "ImportFakturSWF";
            activitybind23.Path = "msgSubject";
            activitybind24.Name = "ImportFakturSWF";
            activitybind24.Path = "msgTo";
            this.sendEmail6.MethodInvoking += new System.EventHandler(this.sendEmail6_MethodInvoking);
            this.sendEmail6.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind23)));
            this.sendEmail6.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind24)));
            this.sendEmail6.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.FromProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind22)));
            // 
            // UpdateItem_ObslugaFaza1
            // 
            this.UpdateItem_ObslugaFaza1.Name = "UpdateItem_ObslugaFaza1";
            this.UpdateItem_ObslugaFaza1.ExecuteCode += new System.EventHandler(this.UpdateItem_ObslugaFaza1_ExecuteCode);
            // 
            // Get_Parameteres
            // 
            this.Get_Parameteres.Name = "Get_Parameteres";
            this.Get_Parameteres.ExecuteCode += new System.EventHandler(this.Get_Parameteres_ExecuteCode);
            activitybind26.Name = "ImportFakturSWF";
            activitybind26.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind25.Name = "ImportFakturSWF";
            activitybind25.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind26)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind25)));
            // 
            // ImportFakturSWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Get_Parameteres);
            this.Activities.Add(this.UpdateItem_ObslugaFaza1);
            this.Activities.Add(this.sendEmail6);
            this.Activities.Add(this.logParameters);
            this.Activities.Add(this.ValidacjaParametrów);
            this.Activities.Add(this.UpdateItem_Zakonczony);
            this.Activities.Add(this.logZakonczony);
            this.Activities.Add(this.sendEmail2);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "ImportFakturSWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail7;

        private IfElseBranchActivity ifUpdateKKOK;

        private IfElseBranchActivity ifUpdateKKIssue;

        private CodeActivity Report_InvSent_UpdateKKIssue;

        private IfElseActivity ifElseActivity3;

        private CodeActivity UpdateItem_ObslugaFaza2;

        private CodeActivity UpdateItem_ObslugaFaza1;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail6;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail5;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail4;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail3;

        private CodeActivity Delete_SourceData;

        private CodeActivity Update_KK;

        private IfElseBranchActivity ifSent;

        private CodeActivity Report_NoRelatedPDF;

        private IfElseActivity ifElseActivity1;

        private CodeActivity Setup_Faktura;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logPowiazanyDokument;

        private IfElseBranchActivity ifHasNoRelatedPDF;

        private IfElseBranchActivity ifHasRelatedPDF;

        private CodeActivity Report_KKNieIstnieje;

        private CodeActivity Find_KK;

        private IfElseActivity PowiązanieZPlikiemPDF;

        private IfElseBranchActivity ifNIeIstniejeKK2;

        private IfElseBranchActivity ifIstniejeKK2;

        private CodeActivity Find_RelatedPDF;

        private IfElseActivity ifElseActivity2;

        private CodeActivity CopySrcData;

        private SequenceActivity ObsługaFaktury;

        private WhileActivity whileRecord;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail1;

        private Microsoft.SharePoint.WorkflowActions.SendEmail sendEmail2;

        private CodeActivity Init_Lists;

        private CodeActivity Preset_kkList;

        private CodeActivity Preset_BiuroRachunkowe;

        private CodeActivity Manage_LFE;

        private CodeActivity Manage_LFZO;

        private WhileActivity whileLFE;

        private CodeActivity Select_LFE;

        private WhileActivity whileLFZO;

        private CodeActivity Select_LFZO;

        private SequenceActivity ListaFakturElektronicznych;

        private SequenceActivity ListaFakturZaObslugę;

        private CodeActivity Select_LFDO;

        private SequenceActivity sequenceActivity2;

        private SequenceActivity sequenceActivity1;

        private SequenceActivity ListaFakturDoObsługi;

        private ParallelActivity parallelActivity1;

        private IfElseBranchActivity ifElse;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private IfElseBranchActivity ifValidParams;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logZakonczony;

        private IfElseActivity ValidacjaParametrów;

        private CodeActivity UpdateItem_Zakonczony;

        private CodeActivity UpdateItem_Anulowany;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logParameters;

        private CodeActivity Get_Parameteres;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;




































































































    }
}
