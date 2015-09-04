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

namespace tabKlienci_GenerujFolderWF.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void GenerujFolder_ExecuteCode(object sender, EventArgs e)
        {
            try
            {
                SPListItem item = workflowProperties.Item;
                SPWeb web = workflowProperties.Web;

                string typKlienta = item["ContentType"].ToString();
                switch (typKlienta)
                {
                    //case "KPiR":
                    //case "KSH":
                    //    string folderName = item["colNazwaSkrocona"] != null ? item["colNazwaSkrocona"].ToString() : string.Empty;
                    //    string status = item["enumStatus"]!=null ? item["enumStatus"].ToString() : string.Empty;

                    //    if (status == "Aktywny" && !String.IsNullOrEmpty(folderName))
                    //    {
                    //        BLL.libDokumenty.Ensure_FolderExist(web, folderName);
                    //    }
                    //    break;

                    //default:
                    //    break;
                }
            }
            catch (Exception ex)
            {
                var result = ElasticEmail.MailHandler.ReportError(ex, workflowProperties.WebUrl.ToString());
            }
        }
    }
}
