using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;
using ElasticEmail;

namespace admProcessRequests_EventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Execute(properties);
        }


        private void Execute(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            try
            {
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
                                    GeneratorFormatekRozliczeniowych.Execute_GenFormRozl(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Generowanie formatek rozliczeniowych dla klienta":
                                    GeneratorFormatekRozliczeniowych.Execute_GenFormRozlK(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
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
                                case "Import faktur za obsługę":
                                    ImportFakturZaObsluge.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;
                                case "Import przeterminowanych należności":
                                    ImportPrzeterminowanychNaleznosci.Execute(properties, web);
                                    PotwierdzMailemZakonczenieZlecenia(properties, web, ct);
                                    break;

                                default:
                                    //properties.ListItem["colStatus"] = "Zakończony";
                                    properties.ListItem.Update();
                                    break;
                            }

                        }
                    }

                });
            }
            catch (Exception ex)
            {

                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());

            }
            finally
            {
                properties.ListItem.Delete();
                this.EventFiringEnabled = true;
            }
        }



        private static void PotwierdzMailemZakonczenieZlecenia(SPItemEventProperties properties, SPWeb web, string ct)
        {
            string bodyHtml = String.Format("zlecono {0}", properties.ListItem["Created"].ToString());
            PotwierdzMailemZakonczenieZlecenia(properties, web, ct, bodyHtml);
        }

        private static void PotwierdzMailemZakonczenieZlecenia(SPItemEventProperties properties, SPWeb web, string ct, string message)
        {

            string subject = String.Format(@": Zlecenie #{0} {1} - zakończone", properties.ListItemId.ToString(), ct);
            string bodyHtml = message;

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
