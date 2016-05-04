using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using EventReceivers;
using EventReceivers.admProcessRequestsER;
using EventReceivers.admProcessRequestsER;
using System.Collections;
using System.Diagnostics;

namespace EventReceivers
{
    internal class GeneratorFormatekRozliczeniowych
    {
        /// <summary>
        /// Generowanie formatek rozliczeniowych dla klienta
        /// </summary>
        /// <param name="properties"></param>
        internal static void Execute_GenFormRozlK(SPItemEventProperties properties, SPWeb web)
        {
            Debug.WriteLine("Execute_GenFromRozl");

            StringBuilder msg = new StringBuilder();

            SPListItem item = properties.ListItem;

            int okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;
            int klientId = new SPFieldLookupValue(item["selKlient"].ToString()).LookupId;

            Debug.WriteLine(string.Format("klientId={0}, okresId={1}", klientId.ToString(), okresId.ToString()));

            if (okresId > 0 && klientId > 0)
            {
                SPListItem klient = tabKlienci.Get_KlientById(web, klientId);
                if (klient != null && klient["enumStatus"] != null && klient["enumStatus"].ToString() == "Aktywny")
                {
                    msg.AppendFormat(@"<li>klient# {0} {1}</li>",
                        klient.ID.ToString(),
                        BLL.Tools.Get_Text(klient, "_NazwaPrezentowana"));

                    bool createKK = Get_Flag(item, "colDodajKartyKontrolne");

                    Debug.WriteLine("Case: " + klient.ContentType.Name);

                    switch (klient.ContentType.Name)
                    {
                        case "KPiR":
                            ZUS_Forms.Create(web, klientId, okresId, createKK);
                            PD_Forms.Create(web, klientId, okresId, createKK);
                            VAT_Forms.Create(web, klientId, okresId, createKK);
                            BR_Forms.Create(web, klientId, okresId, createKK);
                            Reminder_Forms.Create(web, klientId, okresId);
                            break;
                        case "KSH":
                            ZUS_Forms.Create(web, klientId, okresId, createKK);
                            PDS_Forms.Create(web, klientId, okresId, createKK);
                            VAT_Forms.Create(web, klientId, okresId, createKK);
                            BR_Forms.Create(web, klientId, okresId, createKK);
                            Reminder_Forms.Create(web, klientId, okresId);
                            break;
                        case "Firma":
                            PDS_Forms.Create(web, klientId, okresId, createKK);
                            break;
                        case "Osoba fizyczna":
                            ZUS_Forms.Create(web, klientId, okresId, createKK);
                            PD_Forms.Create(web, klientId, okresId, createKK);
                            PDS_Forms.Create(web, klientId, okresId, createKK);
                            VAT_Forms.Create(web, klientId, okresId, createKK);
                            break;

                        default:
                            break;
                    }
                }
            }

            // info o zakończeniu procesu
            string bodyHTML = string.Empty;

            if (msg.Length > 0)
            {
                bodyHTML = string.Format(@"<ul>{0}</ul>", msg.ToString());
            }

            string subject = string.Format(@"Generowanie formatek rozliczeniowych dla klienta");
            SPEmail.EmailGenerator.SendProcessEndConfirmationMail(subject, bodyHTML, web, item);

        }


        /// <summary>
        /// Generowanie formatek rozliczeniowych dla wszystkich aktywnych klientów
        /// </summary>
        /// <param name="properties"></param>
        internal static void Execute_GenFormRozl(SPItemEventProperties properties, SPWeb web)
        {
            Debug.WriteLine("Execute_GenFromRozl");

            StringBuilder msg = new StringBuilder();

            StringBuilder sb = new StringBuilder();

            //sprawdź parametry wywołania
            SPFieldMultiChoiceValue wt = new SPFieldMultiChoiceValue(properties.ListItem["enumTypKlienta"].ToString());
            int okresId = new SPFieldLookupValue(properties.ListItem["selOkres"].ToString()).LookupId;
            SPListItem item = properties.ListItem;

            for (int i = 0; i < wt.Count; i++)
            {
                if (okresId > 0)
                {
                    string typKlienta = wt[i];

                    Array klienci = tabKlienci.Get_AktywniKlienci_Serwis(web, typKlienta);
                    Debug.WriteLine("Wybrano klientów: " + klienci.Length.ToString());

                    bool createKK = Get_Flag(item, "colDodajKartyKontrolne");

                    //sprawdź czy jest ograniczona lista serwisów
                    if (item["selSewisy"] != null
                        && BLL.Tools.Get_LookupValueColection(item, "selSewisy").Count > 0)
                    {
                        SPFieldLookupValueCollection serwisy = BLL.Tools.Get_LookupValueColection(item, "selSewisy");

                        klienci = Refine_Klienci(klienci, serwisy);
                        Debug.WriteLine("Ograniczono listę do: " + klienci.Length.ToString());
                        
                        foreach (SPListItem klient in klienci)
                        {
                            Debug.WriteLine("klientId=" + klient.ID.ToString());

                            foreach (SPFieldLookupValue v in serwisy)
                            {
                                switch (v.LookupValue)
                                {
                                    case "ZUS-D":
                                    case "ZUS-D+C":
                                    case "ZUS-M":
                                    case "ZUS-M+C":
                                    case "ZUS-ZD":
                                    case "ZUS-PRAC":
                                        ZUS_Forms.CreateAll(web, klienci, okresId, createKK);
                                        break;
                                    case "PDS-M":
                                    case "PDS-KW":
                                        PDS_Forms.CreateAll(web, klienci, okresId, createKK);
                                        break;
                                    case "PDW-M":
                                    case "PDW-KW":
                                        //PDW_Forms.Create(web, klienci, okresId, createKK);
                                        break;
                                    case "PD-M":
                                    case "PD-KW":
                                        PD_Forms.CreateAll(web, klienci, okresId, createKK);
                                        break;
                                    case "VAT-M":
                                    case "VAT-KW":
                                        VAT_Forms.CreateAll(web, klienci, okresId, createKK);
                                        break;
                                    case "RBR":
                                        BR_Forms.CreateAll(web, klienci, okresId, createKK);
                                        break;
                                    case "POW-Dok":
                                    case "POW-WBank":
                                        Reminder_Forms.CreateAll(web, klienci, okresId);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (typKlienta)
                        {
                            case "KPiR":
                                ZUS_Forms.CreateAll(web, klienci, okresId, createKK);
                                PD_Forms.CreateAll(web, klienci, okresId, createKK);
                                //PDW_Forms.Create(web, klienci, okresId, createKK);
                                VAT_Forms.CreateAll(web, klienci, okresId, createKK);
                                BR_Forms.CreateAll(web, klienci, okresId, createKK);
                                Reminder_Forms.CreateAll(web, klienci, okresId);
                                break;
                            case "KSH":
                                ZUS_Forms.CreateAll(web, klienci, okresId, createKK);
                                PDS_Forms.CreateAll(web, klienci, okresId, createKK);
                                VAT_Forms.CreateAll(web, klienci, okresId, createKK);
                                BR_Forms.CreateAll(web, klienci, okresId, createKK);
                                Reminder_Forms.CreateAll(web, klienci, okresId);
                                break;
                            case "Firma":
                                PDS_Forms.CreateAll(web, klienci, okresId, createKK);
                                break;
                            case "Osoba fizyczna":
                                ZUS_Forms.CreateAll(web, klienci, okresId, createKK);
                                PD_Forms.CreateAll(web, klienci, okresId, createKK);
                                //PDW_Forms.Create(web, klienci, okresId, createKK);
                                VAT_Forms.CreateAll(web, klienci, okresId, createKK);
                                break;

                            default:
                                break;
                        }
                    }

                    //informacja dla operatora
                    foreach (SPListItem klient in klienci)
                    {
                        msg.AppendFormat(@"<li>klient# {0} {1}</li>",
                            klient.ID.ToString(),
                            BLL.Tools.Get_Text(klient, "_NazwaPrezentowana"));
                    }
                }
            }

            // info o zakończeniu procesu
            string bodyHTML = string.Empty;

            if (msg.Length > 0)
            {
                bodyHTML = string.Format(@"<ul>{0}</ul>", msg.ToString());
            }

            string subject = string.Format(@"Generowanie formatek rozliczeniowych dla klientów typu {0}",
                wt.ToString());
            SPEmail.EmailGenerator.SendProcessEndConfirmationMail(subject, bodyHTML, web, item);
        }

        private static Array Refine_Klienci(Array klienci, SPFieldLookupValueCollection serwisy)
        {
            ArrayList results = new ArrayList();

            foreach (SPListItem klientItem in klienci)
            {
                foreach (SPFieldLookupValue s in serwisy)
                {
                    if (BLL.Tools.Has_Service(klientItem, s.LookupValue, "selSewisy")
                        | BLL.Tools.Has_Service(klientItem, s.LookupValue, "selSerwisyWspolnicy"))
                    {
                        results.Add(klientItem);
                        Debug.WriteLine(BLL.Tools.Get_Text(klientItem, "_NazwaPrezentowana") + " - added");
                        break;
                    }
                }

            }

            return results.ToArray();
        }

        #region Helpers
        private static bool Get_Flag(SPListItem item, string col)
        {
            return item[col] != null ? bool.Parse(item[col].ToString()) : false;
        }
        #endregion

    }
}
