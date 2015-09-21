﻿using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers.tabKlienciER
{

    public class tabKlienciER : SPItemEventReceiver
    {

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            try
            {
                BLL.Logger.LogEvent(properties.WebUrl, properties.ListItem.Title + ".OnChange");

                SPListItem item = properties.ListItem;
                SPWeb web = properties.Web;

                Update_LookupRefFields(item);

                Update_FolderInLibrary(item, web);
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
            }
            finally
            {
                this.EventFiringEnabled = true;
            }
        }

        private static void Update_LookupRefFields(SPListItem item)
        {
            // aktualizacja odwołań do lookupów
            item["_TypZawartosci"] = item["ContentType"].ToString();
            item["_Biuro"] = item["selBiuro"] != null ? new SPFieldLookupValue(item["selBiuro"].ToString()).LookupValue : string.Empty;
            item["_ZatrudniaPracownikow"] = item["colZatrudniaPracownikow"] != null && (bool)item["colZatrudniaPracownikow"] ? "TAK" : string.Empty;

            if (item["selDedykowanyOperator_Podatki"] != null)
            {
                item["_DedykowanyOperator_Podatki"] = new SPFieldLookupValue(item["selDedykowanyOperator_Podatki"].ToString()).LookupValue;
            }
            if (item["selDedykowanyOperator_Kadry"] != null)
            {
                item["_DedykowanyOperator_Kadry"] = new SPFieldLookupValue(item["selDedykowanyOperator_Kadry"].ToString()).LookupValue;
            }
            if (item["selDedykowanyOperator_Audyt"] != null)
            {
                item["_DedykowanyOperator_Audyt"] = new SPFieldLookupValue(item["selDedykowanyOperator_Audyt"].ToString()).LookupValue;
            }

            string np = string.Empty;
            switch (item.ContentType.Name)
            {
                case "KPiR":
                case "KSH":
                case "Firma":
                    np = string.Format("{0} NIP:{1}", item.Title, item["colNIP"] != null ? item["colNIP"].ToString() : string.Empty);
                    break;
                case "Klient":
                    np = item["colNazwaSkrocona"].ToString();
                    break;
                case "Osoba fizyczna":
                    string npNazwsko = item["colNazwisko"] != null ? item["colNazwisko"].ToString().Trim() : string.Empty;
                    string npImie = item["colImie"] != null ? item["colImie"].ToString().Trim() : string.Empty;
                    string npPESEL = item["colPESEL"] != null ? item["colPESEL"].ToString().Trim() : string.Empty;
                    np = string.Format("{0} {1} PESEL:{2}", npNazwsko, npImie, npPESEL);
                    break;
                default:
                    break;
            }
            item["_NazwaPrezentowana"] = np;
            item.SystemUpdate();
        }

        private static void Update_FolderInLibrary(SPListItem item, SPWeb web)
        {
            string typKlienta = item["ContentType"].ToString();
            switch (typKlienta)
            {
                case "KPiR":
                case "KSH":
                    string folderName = item["colNazwaSkrocona"] != null ? item["colNazwaSkrocona"].ToString() : string.Empty;
                    string status = item["enumStatus"] != null ? item["enumStatus"].ToString() : string.Empty;

                    if (status == "Aktywny" && !String.IsNullOrEmpty(folderName))
                    {
                        int docId = BLL.libDokumenty.Ensure_FolderExist(web, folderName);
                        int currDocId = item["_DocumentId"] != null ? int.Parse(item["_DocumentId"].ToString()) : 0;

                        if (docId > 0 && currDocId != docId)
                        {
                            item["_DocumentId"] = docId.ToString();
                            item.SystemUpdate();
                        }
                    }
                    break;

                default:
                    break;
            }
        }


    }
}
