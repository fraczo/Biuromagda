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

namespace Workflows.ObslugaZadania
{
    public sealed partial class ObslugaZadania : SequentialWorkflowActivity
    {
        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public string ct;
        public SPListItem item;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
            ct = item.ContentType.ToString();
        }


        private void isZadanie(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Zadanie" ? true : false;
        }

        private void isProsbaODokumenty(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Prośba o dokumenty" ? true : false;
        }

        private void isProsbaOWyciagBankowy(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Prośba o przesłanie wyciągu bankowego" ? true : false;
        }

        private void isRozliczeniePD(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku dochodowego" ? true : false;
        }

        private void isRozliczeniePDS(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku dochodowego spółki" ? true : false;
        }

        private void isRozliczenieVAT(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku VAT" ? true : false;
        }

        private void isRozliczenieZUS(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie ZUS" ? true : false;
        }

        private void isRozliczenieRBR(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie z biurem rachunkowym" ? true : false;
        }

    }
}

