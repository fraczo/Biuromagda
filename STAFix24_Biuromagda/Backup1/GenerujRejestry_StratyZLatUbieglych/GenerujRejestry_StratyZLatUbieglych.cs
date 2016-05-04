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

namespace Workflows.GenerujRejestry_StratyZLatUbieglych
{
    public sealed partial class GenerujRejestry_StratyZLatUbieglych : SequentialWorkflowActivity
    {
        public GenerujRejestry_StratyZLatUbieglych()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public int klientIndex = -1;
        public Array aKlienci;
        public SPWeb web;
        public int currentYear = DateTime.Now.Year;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            web = workflowProperties.Web;
        }

        private void Select_ListaKlientow_ExecuteCode(object sender, EventArgs e)
        {
            aKlienci = BLL.tabKlienci.Get_AktywniKlienci(workflowProperties.Web);
        }

        private void Set_KlientIndex_ExecuteCode(object sender, EventArgs e)
        {
            if (aKlienci.Length > 0) klientIndex = 0;
        }

        private void isKlientExist(object sender, ConditionalEventArgs e)
        {
            if (klientIndex < aKlienci.Length)
                e.Result = true;
            else
                e.Result = false;
        }

        private void Create_RekordyStrat_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = (SPListItem)aKlienci.GetValue(klientIndex);
            int klientId = item.ID;
            for (int i = 1; i <= 5; i++)
            {
                int targetYear = currentYear - i;
                BLL.tabStratyZLatUbieglych.Ensure_RecordExist(web, klientId, targetYear);
            }

        }

        private void Increment_KlientIndex_ExecuteCode(object sender, EventArgs e)
        {
            klientIndex++;
        }

    }
}
