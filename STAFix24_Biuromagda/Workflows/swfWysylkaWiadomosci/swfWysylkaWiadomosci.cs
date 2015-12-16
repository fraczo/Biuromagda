﻿using System;
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
using System.Text;

namespace Workflows.swfWysylkaWiadomosci
{
    public sealed partial class swfWysylkaWiadomosci : SequentialWorkflowActivity
    {
        public swfWysylkaWiadomosci()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String logSelected_HistoryDescription = default(System.String);
        public String logCurrentMessage_HistoryDescription = default(System.String);
        private Array results;
        private IEnumerator myEnum;
        private StringBuilder sb = new StringBuilder();
        public String adminEmail = "stafix24@hotmail.com";

        private void Select_ListaWiadomosciOczekujacych_ExecuteCode(object sender, EventArgs e)
        {
            results = BLL.tabWiadomosci.Select_Batch(workflowProperties.Web);
            myEnum = results.GetEnumerator();

            logSelected.HistoryDescription = "";
        }

        private void whileRecordExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Initialize_ChildWorkflow_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = myEnum.Current as SPListItem;

            BLL.Workflows.StartWorkflow(item, "Obsługa wiadomości");
            Debug.WriteLine("Workflow initiated for message #" + item.ID.ToString());

            //aktualizacja informacji do raportu
            sb.AppendFormat(@"<li>{0} :: {1}</li>",
                BLL.Tools.Get_LookupValue(item, "selKlient"),
                item.Title);

        }

        public String msgSubject = default(System.String);
        public String msgBody = default(System.String);
        private void sendAdminConfirmation_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = string.Format(@"Biuromagda::Wysyłka wiadomości zakończona ({0})", results.Length.ToString());
            msgBody = string.Format(@"Lista przetworzonych wiadomości<br><ol>{0}</ol>", sb.ToString());
        }

    }
}
