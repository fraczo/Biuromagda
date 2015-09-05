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

namespace Workflows.ObslugaWiadomosci
{
    public sealed partial class ObslugaWiadomosci : SequentialWorkflowActivity
    {
        public ObslugaWiadomosci()
        {
            InitializeComponent();
        }

        enum StatusWysylki : int
        {
            Zarejestrowana = 15,
            Oczekuje,
            Wysłana,
            Anulowana
        }

        //public Int32 StatusWF = StatusWysylki.Zarejestrowana.ToString();


        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;

        private void Set_From_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Set_CC_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Set_Body_ExecuteCode(object sender, EventArgs e)
        {
            item["colTresc"] = DateTime.Now.ToString();
            item.Update();
        }

        private void Send_Mail_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void Update_Flags_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void isWiadomoscWyslana(object sender, ConditionalEventArgs e)
        {
            e.Result = false;
        }

        private void isOdroczonaWysylka(object sender, ConditionalEventArgs e)
        {
            e.Result = false;
        }

        private void isWysylkaZakonczona2(object sender, ConditionalEventArgs e)
        {
            e.Result = true;
        }

        private void Update_tabKartyKlientów_ExecuteCode(object sender, EventArgs e)
        {

        }


        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }




    }
}
