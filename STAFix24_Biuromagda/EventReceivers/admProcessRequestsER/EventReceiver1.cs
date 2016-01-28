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
using System.Collections;
using System.Xml;

namespace EventReceivers
{
    public class EventReceiver1 : SPItemEventReceiver
    {
        private string _OBSLUGA = "Obsługa";
        private string _ANULOWANY = "Anulowany";

        public override void ItemAdded(SPItemEventProperties properties)
        {
            Run_admProcessRequestsWF(properties);
        }

        private void Run_admProcessRequestsWF(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            try
            {
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
                    case "Import faktur za obsługę":
                        Debug.WriteLine("Event: Import faktur za obsługę");

                        string[] p = new string[2];
                        p[0] = BLL.Tools.Get_LookupId(item, "selOkres").ToString();
                        p[1] = item.ID.ToString();
                        string initParams = BLL.Tools.ConvertStringArrayToString(p);

                        BLL.Workflows.StartSiteWorkflow(item.ParentList.ParentWeb.Site, "ImportFakturSWF", initParams);
                        Debug.WriteLine("Workflow: ImportFakturSWF - started");
                        break;
                    default:
                        BLL.Workflows.StartWorkflow(item, "admProcessRequestsWF");
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
                BLL.Tools.Set_Text(properties.ListItem, "enumStatusZlecenia", _ANULOWANY);
                properties.ListItem.Update();
            }

            this.EventFiringEnabled = true;
        }
    }
}
