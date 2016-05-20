using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Diagnostics;

namespace Biuromagda.Features.Workflows
{
    [Guid("390fb2ec-dddc-41a1-b53b-abf9de526ed8")]
    public class WorkflowsEventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                var site = properties.Feature.Parent as SPSite;
                string workflowTemplateName;
                string associationName;


                #region tabZadania
                SPList list = BLL.tabZadania.Get_List(site.RootWeb);

                // tabZadaniaWF
                workflowTemplateName = "tabZadaniaWF";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, false, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");

                // Zatwierdzenie zadania
                workflowTemplateName = "Zatwierdzenie zadania";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, true, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated"); 
                #endregion


                #region tabWiadomości
                list = BLL.tabWiadomosci.Get_List(site.RootWeb);

                // Obsługa wiadomości
                workflowTemplateName = "Obsługa wiadomości";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, true, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");

                // Wyślij kopię wiadomości
                workflowTemplateName = "Wyślij kopię wiadomości";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, true, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");
                #endregion

                #region admProcessRequests
                list = BLL.admProcessRequests.Get_List(site.RootWeb);

                // admProcessRequestsWF
                workflowTemplateName = "admProcessRequestsWF";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, false, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");

                // GeneratorZadanWF
                workflowTemplateName = "GeneratorZadanWF";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, false, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");

                // Generuj formatki rozliczeniowe
                workflowTemplateName = "Generuj formatki rozliczeniowe";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, false, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");

                // Generuj formatki rozliczeniowe dla klienta
                workflowTemplateName = "Generuj formatki rozliczeniowe dla klienta";
                associationName = workflowTemplateName;
                if (list != null) BLL.Workflows.EnsureWorkflowAssociation(list, workflowTemplateName, associationName, false, false, false);
                Debug.WriteLine("Workflow: " + workflowTemplateName + " - associated");
                #endregion



            }
            catch (Exception ex)
            {
                ElasticEmail.EmailGenerator.ReportError(ex, (properties.Feature.Parent as SPSite).Url);
            }
        }

    }
}
