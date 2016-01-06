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
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
        private bool suppresItemDeletion = false;

        public override void ItemAdded(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        private void Execute(SPItemEventProperties properties)
        {
            Debug.WriteLine("admProcessRequest_EventReceiver_Execute");

            this.EventFiringEnabled = false;

            properties.ListItem["enumStatusZlecenia"] = "Obsługa";
            properties.ListItem.SystemUpdate();

            try
            {
                // na uprawnieniach operatora

                SPListItem item = properties.ListItem;
                switch (item.ContentType.Name)
                {
                    case "Obsługa ADO":
                        ObslugaADO.Execute(properties, item.Web);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                properties.ListItem["enumStatusZlecenia"] = "Anulowany";
                properties.ListItem.SystemUpdate();

                BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
            }

            bool allowDelete = true;

            try
            {
                // na podwyższonych uprawnieniach

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.AllWebs[properties.Web.ID])
                        {
                            //określ rodzaj raportu
                            string ct = properties.ListItem["ContentType"].ToString();
                            switch (ct)
                            {
                                case "Generowanie formatek rozliczeniowych":
                                    //GeneratorFormatekRozliczeniowych.Execute_GenFormRozl(properties, web);
                                    //PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    //obsługa wewnętrz porcedury

                                    BLL.Workflows.StartWorkflow(properties.ListItem, "Generuj zbiorczo formatki rozliczeniowe");
                                    allowDelete = false;

                                    break;
                                case "Generowanie formatek rozliczeniowych dla klienta":
                                    //Todo: zamienić na workflow
                                    //GeneratorFormatekRozliczeniowych.Execute_GenFormRozlK(properties, web);
                                    //PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    //obsługa wewnętrz porcedury

                                    BLL.Workflows.StartWorkflow(properties.ListItem, "Generuj formatki rozliczeniowe");
                                    allowDelete = false;

                                    break;
                                case "Import Klientów":
                                    string message;
                                    ImportKlientow.Execute(properties, web, out message);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct, message);
                                    break;
                                case "Import bufora wiadomości":
                                    ImportBuforaWiadomosci.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Import faktur elektronicznych":
                                    ImportFakturElektronicznych.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Usuń przetworzone faktury":
                                    ImportFakturElektronicznych.Remove_Completed(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Import faktur za obsługę":
                                    ImportFakturZaObsluge.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Import przeterminowanych należności":
                                    ImportPrzeterminowanychNaleznosci.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Obsługa wiadomości":
                                    BLL.Workflows.StartSiteWorkflow(web.Site, "Wysyłka wiadomości oczekujących");
                                    break;
                                case "Obsługa zadań":
                                    ObslugaZadan.Execute(properties, web);
                                    break;
                                case "CleanUp":
                                    BLL.Workflows.StartSiteWorkflow(web.Site, "Odchudzanie bazy danych");
                                    break;

                                default:
                                    //properties.ListItem["colStatus"] = "Zakończony";
                                    //properties.ListItem.SystemUpdate();
                                    break;
                            }

                        }
                    }

                });
            }
            catch (Exception ex)
            {
                properties.ListItem["enumStatusZlecenia"] = "Anulowany";
                properties.ListItem.SystemUpdate();

                BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());

            }
            finally
            {
                this.EventFiringEnabled = true;
                if (allowDelete) properties.ListItem.Delete();
            }
        }

        private static void PotwierdzMailemZakonczenieZlecenia(SPItemEventProperties properties, SPWeb web, string ct)
        {
            string bodyHtml = String.Format("zlecono {0}", properties.ListItem["Created"].ToString());
            PotwierdzMailemZakonczenieZlecenia(properties, web, ct, bodyHtml);
        }

        private static void PotwierdzMailemZakonczenieZlecenia(SPItemEventProperties properties, SPWeb web, string ct, string bodyHtml)
        {
            string subject = ct.ToString();
#if DEBUG
            //send directly via ElasticEmail
            ElasticEmail.EmailGenerator.SendProcessEndConfirmationMail(
                subject,
                bodyHtml,
                web,
                properties.ListItem);

#else
                                    //send via SPUtility
                                    SPEmail.EmailGenerator.SendProcessEndConfirmationMail(
                                        subject,
                                        bodyHtml,
                                        web,
                                        properties.ListItem);
#endif
        }


    }
}
