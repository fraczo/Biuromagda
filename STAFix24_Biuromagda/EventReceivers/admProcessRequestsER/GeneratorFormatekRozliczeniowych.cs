﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using admProcessRequests_EventReceiver;
using admProcessRequests_EventReceiver.admProcessRequestsER;

namespace admProcessRequests_EventReceiver
{
    internal class GeneratorFormatekRozliczeniowych
    {
        /// <summary>
        /// Generowanie formatek rozliczeniowych dla klienta
        /// </summary>
        /// <param name="properties"></param>
        internal static void Execute_GenFormRozlK(SPItemEventProperties properties, SPWeb web)
        {
            int okresId = new SPFieldLookupValue(properties.ListItem["selOkres"].ToString()).LookupId;
            int klientId = new SPFieldLookupValue(properties.ListItem["selKlient"].ToString()).LookupId;

            if (okresId > 0 && klientId > 0)
            {
                SPListItem klient = tabKlienci.Get_KlientById(web, klientId);
                if (klient != null && klient["enumStatus"] != null && klient["enumStatus"].ToString() == "Aktywny")
                {
                    switch (klient.ContentType.Name)
                    {
                        case "KPiR":
                            ZUS_Forms.Create(web, klientId, okresId);
                            PD_Forms.Create(web, klientId, okresId);
                            VAT_Forms.Create(web, klientId, okresId);
                            BR_Forms.Create(web, klientId, okresId);
                            Reminder_Forms.Create(web, klientId, okresId);
                            break;
                        case "KSH":
                            ZUS_Forms.Create(web, klientId, okresId);
                            PDS_Forms.Create(web, klientId, okresId);
                            VAT_Forms.Create(web, klientId, okresId);
                            BR_Forms.Create(web, klientId, okresId);
                            Reminder_Forms.Create(web, klientId, okresId);
                            break;
                        case "Firma":
                            PDS_Forms.Create(web, klientId, okresId);
                            break;
                        case "Osoba fizyczna":
                            ZUS_Forms.Create(web, klientId, okresId);
                            PD_Forms.Create(web, klientId, okresId);
                            VAT_Forms.Create(web, klientId, okresId);
                            break;

                        default:
                            break;
                    }
                }
            }

        }


        /// <summary>
        /// Generowanie formatek rozliczeniowych dla wszystkich aktywnych klientów
        /// </summary>
        /// <param name="properties"></param>
        internal static void Execute_GenFormRozl(SPItemEventProperties properties, SPWeb web)
        {
            StringBuilder sb = new StringBuilder();

            //sprawdź parametry wywołania
            SPFieldMultiChoiceValue wt = new SPFieldMultiChoiceValue(properties.ListItem["enumTypKlienta"].ToString());
            int okresId = new SPFieldLookupValue(properties.ListItem["selOkres"].ToString()).LookupId;

            for (int i = 0; i < wt.Count; i++)
            {
                if (okresId > 0)
                {
                    string typKlienta = wt[i];

                    Array klienci = tabKlienci.Get_AktywniKlienci_Serwis(web, typKlienta);

                    switch (typKlienta)
                    {
                        case "KPiR":
                            ZUS_Forms.Create(web, klienci, okresId);
                            PD_Forms.Create(web, klienci, okresId);
                            VAT_Forms.Create(web, klienci, okresId);
                            BR_Forms.Create(web, klienci, okresId);
                            Reminder_Forms.Create(web, klienci, okresId);
                            break;
                        case "KSH":
                            ZUS_Forms.Create(web, klienci, okresId);
                            PDS_Forms.Create(web, klienci, okresId);
                            VAT_Forms.Create(web, klienci, okresId);
                            BR_Forms.Create(web, klienci, okresId);
                            Reminder_Forms.Create(web, klienci, okresId);
                            break;
                        case "Firma":
                            PDS_Forms.Create(web, klienci, okresId);
                            break;
                        case "Osoba fizyczna":
                            ZUS_Forms.Create(web, klienci, okresId);
                            PD_Forms.Create(web, klienci, okresId);
                            VAT_Forms.Create(web, klienci, okresId);
                            break;

                        default:
                            break;
                    }


                }
            }
        }

    }
}
