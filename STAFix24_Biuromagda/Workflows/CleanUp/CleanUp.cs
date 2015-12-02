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

namespace Workflows.CleanUp
{
    public sealed partial class CleanUp : SequentialWorkflowActivity
    {
        public CleanUp()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        public System.Collections.IEnumerator myEnum;
        public int wiadomoscIdx = -1;
        public Array zadania = null;
        public Array wiadomosci = null;
        public SPListItem zadanie;
        public SPListItem wiadomosc;

        public String taskCounter = default(System.String);
        public String messageCounter = default(System.String);
        private string _STATUS_PROCESU_ZAKONCZONY = "Zakończony";
        private string _ATT_TO_REMOVE_MASK = @"DRUK WPŁATY__";

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void Select_ListaZadan_ExecuteCode(object sender, EventArgs e)
        {
            bool withAttachements = true;
            zadania = BLL.tabZadania.Get_ZakonczoneDoArchiwizacji(item.Web, withAttachements);
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
                //Remove_DrukiWplaty(zadanie);
                if (BLL.Tools.Get_Flag(zadanie, "colDrukWplaty"))
                {
                    BLL.Tools.Set_Flag(zadanie, "colDrukWplaty", false);
                    zadanie.SystemUpdate();
                }
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
                        foundNames.Add(attName);
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

        }

        private void isWiadomoscExist(object sender, ConditionalEventArgs e)
        {

        }

        private void Manage_Wiadomość_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Update_Status_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Tools.Set_Text(item, "enumStatusProcesu", _STATUS_PROCESU_ZAKONCZONY);
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            string a = "sldkfj";
        }
    }
}
