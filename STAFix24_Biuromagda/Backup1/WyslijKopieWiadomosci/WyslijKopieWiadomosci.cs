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

namespace Workflows.WyslijKopieWiadomosci
{
    public sealed partial class WyslijKopieWiadomosci : SequentialWorkflowActivity
    {
        public WyslijKopieWiadomosci()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        public SPListItem newItem;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
        }

        private void Create_KopiaWiadomosci_ExecuteCode(object sender, EventArgs e)
        {
            newItem = item.ParentList.AddItem();
            Copy_Field(item, newItem, "colNadawca");
            Copy_Field(item, newItem, "colOdbiorca");
            Copy_Field(item, newItem, "colKopiaDla");
            Copy_Field(item, newItem, "colKopiaDoNadawcy");
            Copy_Field(item, newItem, "colKopiaDoBiura");
            Copy_Field(item, newItem, "Title");
            Copy_Field(item, newItem, "colTresc");
            Copy_Field(item, newItem, "colTrescHTML");
            Copy_Field(item, newItem, "colPlanowanaDataNadania");
            Copy_Field(item, newItem, "_ZadanieId");
            Copy_Field(item, newItem, "selKlient_NazwaSkrocona");

            Copy_Attachements(item, newItem);

            newItem.SystemUpdate();
        }

        private void Copy_Attachements(SPListItem item, SPListItem newItem)
        {
            for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
            {
                string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
                SPFile file = item.ParentList.ParentWeb.GetFile(url);

                if (file.Exists)
                {
                    Copy_Attachement(newItem, file);
                }
            }
        }

        private void Copy_Attachement(SPListItem newItem, SPFile file)
        {
            int bufferSize = 20480;
            byte[] byteBuffer = new byte[bufferSize];
            byteBuffer = file.OpenBinary();
            newItem.Attachments.Add(file.Name, byteBuffer);
        }

        private void Copy_Field(SPListItem item, SPListItem newItem, string col)
        {
            newItem[col] = item[col];
        }

        private void Run_ObslugaWiadomosci_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Workflows.StartWorkflow(newItem, "Obsługa wiadomości");
        }
    }
}
