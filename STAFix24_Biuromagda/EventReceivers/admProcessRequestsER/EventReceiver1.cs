using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;
using ElasticEmail;
using EventReceivers.admProcessRequestsER;
using System.Diagnostics;

namespace EventReceivers
{
    public class EventReceiver1 : SPItemEventReceiver
    {
        private string _OBSLUGA = "Obsługa";

        public override void ItemAdded(SPItemEventProperties properties)
        {
            Run_admProcessRequestsWF(properties);
        }

        private void Run_admProcessRequestsWF(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            SPListItem item = properties.ListItem;

            switch (properties.ListItem.ContentType.Name)
            {
                case "Generowanie formatek rozliczeniowych":
                    BLL.Workflows.StartWorkflow(item, "Generuj zbiorczo formatki rozliczeniowe");
                    break;
                case "Generowanie formatek rozliczeniowych dla klienta":
                    BLL.Workflows.StartWorkflow(item, "Generuj formatki rozliczeniowe");
                    break;
                case "Obsługa wiadomości":
                    BLL.Workflows.StartSiteWorkflow(item.ParentList.ParentWeb.Site, "Wysyłka wiadomości oczekujących");
                    break;
                case "CleanUp":
                    BLL.Workflows.StartSiteWorkflow(item.ParentList.ParentWeb.Site, "Odchudzanie bazy danych");
                    break;
                default:
                    BLL.Workflows.StartWorkflow(item, "admProcessRequestsWF");
                    break;
            }

            this.EventFiringEnabled = true;
        }
    }
}
