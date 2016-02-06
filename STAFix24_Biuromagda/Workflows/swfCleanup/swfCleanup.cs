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

namespace Workflows.swfCleanup
{
    public sealed partial class swfCleanup : SequentialWorkflowActivity
    {
        public swfCleanup()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public System.Collections.IEnumerator myEnum;
        public int wiadomoscIdx = -1;
        public Array zadania = null;
        public Array wiadomosci = null;
        public SPListItem zadanie;
        public SPListItem wiadomosc;

        public String msgAdminEmail = "stafix24@hotmail.com";

        public int taskCounter = 0;
        public int messageCounter = 0;
        private string _ATT_TO_REMOVE_MASK = @"DRUK WPŁATY__";

        private DateTime t;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            t = DateTime.Now;
        }

        private void Select_ListaZadan_ExecuteCode(object sender, EventArgs e)
        {
            bool withAttachements = true;
            zadania = BLL.tabZadania.Get_ZakonczoneDoArchiwizacji(workflowProperties.Web, withAttachements);
            myEnum = zadania.GetEnumerator();
        }

        private void isZadanieExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        /// <summary>
        /// Usówa załączniki druków wpłaty ze wszystkich zadań w statusie Zakmnięte i Anulowane
        /// </summary>
        private void Manage_Zadanie_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem zadanie = (SPListItem)myEnum.Current;
            Debug.WriteLine(zadanie.ID.ToString());

            if (zadanie.Attachments.Count > 0)
            {
                Debug.WriteLine(zadanie.ID.ToString() + " has attachments");

                Remove_DrukiWplaty(zadanie);
                taskCounter++;

                //if (BLL.Tools.Get_Flag(zadanie, "colDrukWplaty"))
                //{
                //    BLL.Tools.Set_Flag(zadanie, "colDrukWplaty", false);

                //    zadanie.SystemUpdate();
                //}
            }
        }

        /// <summary>
        /// Usówa załączniki pasujące do wzorca z bieżącego elementu.
        /// </summary>
        private void Remove_DrukiWplaty(SPListItem item)
        {
            if (item.Attachments.Count > 0)
            {
                System.Collections.Generic.List<string> foundNames = new System.Collections.Generic.List<string>();

                foreach (string attName in item.Attachments)
                {
                    if (attName.StartsWith(_ATT_TO_REMOVE_MASK))
                    {
                        foundNames.Add((string)attName);
                        Debug.WriteLine(attName + "-to be removed");

                    }
                }

                if (foundNames.Count > 0)
                {

                    foreach (string attName in foundNames)
                    {
                        item.Attachments.Delete(attName);
                        Debug.WriteLine(attName + "-removed");
                        break;
                    }

                    item.SystemUpdate();

                }
            }
        }

        private void Select_ListaWiadomosci_ExecuteCode(object sender, EventArgs e)
        {
            wiadomosci = BLL.tabWiadomosci.Get_GotoweDoArchiwizacji(workflowProperties.Web);
            myEnum = wiadomosci.GetEnumerator();
        }

        private void isWiadomoscExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Manage_Wiadomosc_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem wiadomosc = (SPListItem)myEnum.Current;
            Debug.WriteLine(wiadomosc.ID.ToString());

            if (wiadomosc.Attachments.Count > 0)
            {
                Debug.WriteLine(wiadomosc.ID.ToString() + " has attachments");
                Remove_DrukiWplaty(wiadomosc);

                messageCounter++;
            }
        }

        private void cmdErrorHandler_ExecuteCode(object sender, EventArgs e)
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

        public String logErrorMessage_HistoryDescription = default(System.String);
        public String logZadaniaCompleted_HistoryOutcome = default(System.String);

        private void cmdUpdateCounters(object sender, EventArgs e)
        {
            logZadaniaCompleted_HistoryOutcome = taskCounter.ToString();
            logWiadomosciCompleted_HistoryOutcome = messageCounter.ToString();

            TimeSpan ts = DateTime.Now - t;
            Debug.WriteLine("Czas obsługi: " + ts.ToString());
        }

        public String logWiadomosciCompleted_HistoryOutcome = default(System.String);

        public String sendAdminConfirmation_CC1 = default(System.String);
        public String msgSubject = default(System.String);
        public String msgBody = default(System.String);
        private void sendAdminConfirmation_MethodInvoking(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - t;
            Debug.WriteLine("Czas obsługi: " + ts.ToString());

            msgSubject = string.Format(@"Biuromagda::Odchudzanie kartotek zakończone");
            msgBody = string.Format(@"Liczba przetworzonych zadań: {0}<br>Liczba przetworzonych wiadomości: {1}<br>Czas obsługi: {2}",
                taskCounter.ToString(),
                messageCounter.ToString(),
                ts.ToString());

        }

        private void cmdEmptyRecycleBin_ExecuteCode(object sender, EventArgs e)
        {
            Debug.WriteLine("Czyszczenie kosza");
            workflowProperties.Site.RecycleBin.DeleteAll();
        }

    }
}
