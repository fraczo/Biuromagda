using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Biuromagda.Features.SiteWorkflows
{
    [Guid("91de5e23-9f16-4f68-b82c-0f906cd36496")]
    public class SiteWorkflowsEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                var site = properties.Feature.Parent as SPSite;

                // swfWysylkaWiadomosci
                string workflowTemplateBaseGuid = "40c09700-4459-415c-88f4-8ba8a7dd5f5d";
                string workflowAssociationName = "Wysyłka wiadomości oczekujących";
                BLL.Workflows.AssociateWorflow(site.RootWeb, workflowTemplateBaseGuid, workflowAssociationName);

                // swfCleanup
                workflowTemplateBaseGuid = "0b5d7c6b-2764-45dc-8fc1-33fa98145d1c";
                workflowAssociationName = "Odchudzanie bazy danych";
                BLL.Workflows.AssociateWorflow(site.RootWeb, workflowTemplateBaseGuid, workflowAssociationName);

                // ImportFakturSWF
                workflowTemplateBaseGuid = "C6916B84-C75A-4FC9-9A6F-0F06E6F54FFF";
                workflowAssociationName = "ImportFakturSWF";
                BLL.Workflows.AssociateWorflow(site.RootWeb, workflowTemplateBaseGuid, workflowAssociationName);
            }
            catch (Exception ex)
            {
                ElasticEmail.EmailGenerator.ReportError(ex, (properties.Feature.Parent as SPSite).Url);
            }
        }
    }
}
