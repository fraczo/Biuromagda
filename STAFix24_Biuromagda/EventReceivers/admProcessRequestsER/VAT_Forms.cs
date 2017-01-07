using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Diagnostics;
using BLL;
using BLL.Models;


namespace EventReceivers.admProcessRequestsER
{
    public class VAT_Forms
    {
        const string ctVAT = "Rozliczenie podatku VAT";

        public static void CreateAll(SPWeb web, Array aKlienci, int okresId, bool createKK)
        {
            foreach (SPListItem item in aKlienci)
            {
                Debug.WriteLine("klientId=" + item.ID.ToString());

                SPFieldLookupValueCollection kody;

                switch (item.ContentType.Name)
                {
                    case "Osoba fizyczna":
                    case "Firma":
                        kody = new SPFieldLookupValueCollection(item["selSerwisyWspolnicy"].ToString());
                        break;
                    default:
                        kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                        break;
                }

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"VAT-M":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_VAT_M_Form(web, item.ID, okresId,null);
                            break;
                        case @"VAT-KW":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_VAT_KW_Form(web, item.ID, okresId,null);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static void Create(SPWeb web, int klientId, int okresId, bool createKK)
        {
            Debug.WriteLine("Create VAT Form");

            SPListItem item = tabKlienci.Get_KlientById(web, klientId);

            if (item != null)
            {
                SPFieldLookupValueCollection kody;

                switch (item.ContentType.Name)
                {
                    case "Osoba fizyczna":
                    case "Firma":
                        kody = new SPFieldLookupValueCollection(item["selSerwisyWspolnicy"].ToString());
                        break;
                    default:
                        kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                        break;
                }

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"VAT-M":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_VAT_M_Form(web, item.ID, okresId, null);
                            break;
                        case @"VAT-KW":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_VAT_KW_Form(web, item.ID, okresId, null);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Formatka rozliczenia kwartalnego VAT
        /// </summary>
        /// <param name="web"></param>
        /// <param name="klientId"></param>
        /// <param name="okresId"></param>
        private static void Create_VAT_KW_Form(SPWeb web, int klientId, int okresId, Array zadania)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctVAT, klientId, okresId);

                bool taskFound = false;
                foreach (SPListItem z in zadania)
                {
                    if (z["KEY"].Equals(key))
                    {
                        taskFound = true;
                        break;
                    }
                }

                if (taskFound) return;

                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    tabOkresy.Get_VAT_KW(web, okresId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctVAT_Form(web, ctVAT, klientId, okresId, key, terminPlatnosci, terminPrzekazania, true);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
#endif

            }
        }

        //formatka rozliczenia miesięcznego VAT
        private static void Create_VAT_M_Form(SPWeb web, int klientId, int okresId, Array zadania)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctVAT, klientId, okresId);

                bool taskFound = false;
                foreach (SPListItem z in zadania)
                {
                    if (z["KEY"].Equals(key))
                    {
                        taskFound = true;
                        break;
                    }
                }

                if (taskFound) return;

                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    tabOkresy.Get_VAT_M(web, okresId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctVAT_Form(web, ctVAT, klientId, okresId, key, terminPlatnosci, terminPrzekazania, false);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
#endif

            }
        }



        public static void CreateNew(SPWeb web, SPListItem item, int okresId, Array zadania)
        {
            Debug.WriteLine("Create VAT Form");

            if (item != null)
            {
                SPFieldLookupValueCollection kody;

                kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"VAT-M":
                            Create_VAT_M_Form(web, item.ID, okresId, zadania);
                            break;
                        case @"VAT-KW":
                            Create_VAT_KW_Form(web, item.ID, okresId, zadania);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
