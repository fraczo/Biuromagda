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
        public int zadanieIdx = -1;
        public int wiadomoscIdx = -1;
        public Array zadania = null;
        public Array wiadomosci = null;
        public SPListItem zadanie;
        public SPListItem wiadomosc;

        public String taskCounter = default(System.String);
        public String messageCounter = default(System.String);

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void Select_ListaZadan_ExecuteCode(object sender, EventArgs e)
        {
            zadania = BLL.tabZadania.Get_ZakonczoneDoArchiwizacji(item.Web);
            if (zadania.Length > 0) zadanieIdx = 0;
        }

        private void isZadanieExist(object sender, ConditionalEventArgs e)
        {
            bool result = false;

            if (zadania != null & (zadanieIdx <= zadania.Length))
            {
                //zadanie = zadania(zadanieIdx);
            }

            e.Result = result;

        }

        private void Manage_Zadanie_ExecuteCode(object sender, EventArgs e)
        {
            var item = zadania(zadanieIdx);
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




    }
}
