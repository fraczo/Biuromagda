using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;

namespace admProcessRequests_EventReceiver
{
    internal class PDS_Forms
    {

        const string ctPDS = @"Rozliczenie podatku dochodowego spółki";

        /// <summary>
        /// Wywołuje procedurę generowania kart kontrolnych PDS dla listy klientów
        /// </summary>
        internal static void Create(SPWeb web, Array aKlienci, int okresId, bool createKK)
        {
            foreach (SPListItem item in aKlienci)
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
                        case @"PDS-M":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_PDS_M_Form(web, item.ID, okresId);
                            break;
                        case @"PDS-KW":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_PDS_KW_Form(web, item.ID, okresId);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Wywołuje procedurę generowania kart kontrolnych PDS dla pojedyńczego klienta
        /// </summary>
        internal static void Create(SPWeb web, int klientId, int okresId, bool createKK)
        {
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
                        case @"PDS-M":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_PDS_M_Form(web, item.ID, okresId);
                            break;
                        case @"PDS-KW":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_PDS_KW_Form(web, item.ID, okresId);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Zlecenia generowania rozliczenia kwartalnego PDS
        /// </summary>
        private static void Create_PDS_KW_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctPDS, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    //zainicjowanie formatki PDS

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_KW(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDS_Form(web, ctPDS, klientId, okresId, key, terminPlatnosci, terminPrzekazania, true);

                    //zainicjowanie danych NKUP, WS NP
                    bool trybKwartalny = true;
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny); //tryp kwartalny

                    //zainicjowanie sumy strat z lat ubiegłych
                    Copy_SumyStratZLatUbieglych(web, klientId, okresId);

                    //zainicjowanie kart w tabeli dochody wspólników
                    Create_DochodyWspolnikow(web, klientId, okresId);
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


        /// <summary>
        /// Zlecenia generowania rozliczenia miesięcznego PDS
        /// </summary>
        private static void Create_PDS_M_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctPDS, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_M(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDS_Form(web, ctPDS, klientId, okresId, key, terminPlatnosci, terminPrzekazania, false);

                    //zainicjowanie danych NKUP, WS NP
                    bool trybKwartalny = false;
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny); //tryp kwartalny

                    //zainicjowanie sumy strat z lat ubiegłych
                    Copy_SumyStratZLatUbieglych(web, klientId, okresId);

                    //zainicjowanie kart w tabeli dochody wspólników
                    Create_DochodyWspolnikow(web, klientId, okresId);
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

        private static void Copy_DaneRozszerzone(SPWeb web, int klientId, int okresId, bool p)
        {
            //throw new NotImplementedException();
        }

        private static void Copy_SumyStratZLatUbieglych(SPWeb web, int klientId, int okresId)
        {
            DateTime d = BLL.tabOkresy.Get_StartDate(web, okresId);

            double sumaStrat = 0;
            double sumaOdliczen = 0;

            //sprawdź 5 ostatnich lat
            int currentYear = d.Year;
            for (int i = 0; i < 5; i++)
            {
                int targetYear = currentYear - 1 - i;
                int itemId = BLL.tabStratyZLatUbieglych.Ensure_RecordExist(web, klientId, targetYear);
                
                //dodaje wartości strat i odliczeń dla bieżącego rekordu
                BLL.tabStratyZLatUbieglych.Add_StratyIOdliczenia(web, itemId, ref sumaStrat, ref sumaOdliczen);
            }

            //ToDo: zapisz wartości sumaStrat i sumaOdliczeń na formatce PDS
        }

        private static void Create_DochodyWspolnikow(SPWeb web, int klientId, int okresId)
        {
            Array wspolnicy = tabKlienci.Get_WspolnicyByKlientId(web, klientId);

            //zainicjuj rekord na bieżący okres w tabeli wspólnicy dla każdego wspólnika niezależnie.

            foreach (SPListItem wItem in wspolnicy)
            {
                BLL.tabDochodyWspolnikow.Ensure_RecordInitiated(web, wItem, klientId, okresId);
            }
        }
    }
}
