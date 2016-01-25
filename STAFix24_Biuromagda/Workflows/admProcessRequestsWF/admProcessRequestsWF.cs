using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.Diagnostics;

namespace Workflows.admProcessRequestsWF
{
    public sealed partial class admProcessRequestsWF : SequentialWorkflowActivity
    {
        public admProcessRequestsWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        public String logErrorMessage_HistoryDescription = default(System.String);
        private string ZAKONCZONY = "Zakończony";
        private string ANULOWANY = "Anulowany";
        private IEnumerator myEnum;
        private int okresId;
        DateTime startTime;

        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fa = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fa != null)
            {
                Debug.WriteLine(fa.Fault.Source);
                Debug.WriteLine(fa.Fault.Message);
                Debug.WriteLine(fa.Fault.StackTrace);

                logErrorMessage_HistoryDescription = string.Format("{0}::{1}",
                    fa.Fault.Message,
                    fa.Fault.StackTrace);

                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fa.Fault.Message, fa.Fault.StackTrace);
            }
        }

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("admProcessRequestsWF:{" + workflowProperties.WorkflowId + "} initiated");
            item = workflowProperties.Item;

            startTime = DateTime.Now;
        }

        private void Preset_ExecuteCode(object sender, EventArgs e)
        {
            item["enumStatusZlecenia"] = "Obsługa";
        }

        private void Set_StatusAnulowany_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Tools.Set_Text(item, "enumStatusZlecenia", ANULOWANY);
        }

        private void UpdateItem_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Tools.Set_Text(item, "enumStatusZlecenia", ZAKONCZONY);
            item.Update();
        }

        #region Helpers
        private static void PotwierdzMailemZakonczenieZlecenia(SPListItem item, SPWeb web, string ct)
        {
            string bodyHtml = String.Format("zlecono {0}", item["Created"].ToString());
            PotwierdzMailemZakonczenieZlecenia(item, web, ct, bodyHtml);
        }

        private static void PotwierdzMailemZakonczenieZlecenia(SPListItem item, SPWeb web, string ct, string bodyHtml)
        {
            string subject = ct.ToString();
#if DEBUG
            //send directly via ElasticEmail
            ElasticEmail.EmailGenerator.SendProcessEndConfirmationMail(
                subject,
                bodyHtml,
                web,
                item);

#else
                                    //send via SPUtility
                                    SPEmail.EmailGenerator.SendProcessEndConfirmationMail(
                                        subject,
                                        bodyHtml,
                                        web,
                                        item);
#endif
        }
        #endregion

        private void isObslugaADO(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Obsługa ADO")) e.Result = true;
        }

        private void isImportFakturElektronicznych(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Import faktur elektronicznych")) e.Result = true;
        }

        private void isUsunPrzetworzoneFaktury(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Usuń przetworzone faktury")) e.Result = true;
        }

        private void isImportFakturZaObsluge(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Import faktur za obsługę")) e.Result = true;
        }

        private void isImportPrzeterminowanychNaleznosci(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Import przeterminowanych należności")) e.Result = true;
        }

        private void isObslugaZadan(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Obsługa zadań")) e.Result = true;
        }

        private void Manage_ADO_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ObslugaADO.Execute(item, item.Web);
        }

        private void Manage_ImportFakturElektronicznych_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ImportFakturElektronicznych.Execute(item, item.Web);
            PotwierdzMailemZakonczenieZlecenia(item, item.Web, item.ContentType.Name);
        }

        private void Manage_UsunPrzetworzoneFaktury_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ImportFakturElektronicznych.Remove_Completed(item, item.Web);
            PotwierdzMailemZakonczenieZlecenia(item, item.Web, item.ContentType.Name);
        }

        private void Manage_ImportFakturZaObsluge_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ImportFakturZaObsluge.Execute(item, item.Web);
            PotwierdzMailemZakonczenieZlecenia(item, item.Web, item.ContentType.Name);
        }

        private void Manage_ImportPrzeterminowanychNaleznosci_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ImportPrzeterminowanychNaleznosci.Execute(item, item.Web);
            PotwierdzMailemZakonczenieZlecenia(item, item.Web, item.ContentType.Name);
        }

        private void Manage_ObslugaZadan_ExecuteCode(object sender, EventArgs e)
        {
            EventReceivers.admProcessRequestsER.ObslugaZadan.Execute(item, item.Web);
            PotwierdzMailemZakonczenieZlecenia(item, item.Web, item.ContentType.Name);
        }

        private void Select_IFE_ExecuteCode(object sender, EventArgs e)
        {
            okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;
            string targetList = @"Faktury elektroniczne - import";

            SPList list = item.Web.Lists.TryGetList(targetList);

            Array results = list.Items.Cast<SPListItem>().ToArray();

            myEnum = results.GetEnumerator();

        }

        private void whileIFEExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;

        }

        private void Manage_IFE_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = myEnum.Current as SPListItem;
            Workflows.admProcessRequestsWF.ImportFaktur.Import_Faktura(item, okresId);
        }

        private void Reporting_ExecuteCode(object sender, EventArgs e)
        {
            Debug.WriteLine("admProcessRequestsWF processing time: " + new TimeSpan(DateTime.Now.Ticks - startTime.Ticks).ToString());
        }
    }
}
