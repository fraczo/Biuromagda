using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using System.Diagnostics;

namespace admProcessRequests_EventReceiver
{
    internal class PDS_Forms
    {

        const string _CT_NAME_PDS = @"Rozliczenie podatku dochodowego spółki";

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
                    case "Firma zewnętrzna":
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
                string key = tabZadania.Define_KEY(_CT_NAME_PDS, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    Debug.WriteLine("PDS_KW klient:" + klientId.ToString());

                    //zainicjowanie formatki PDS

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_KW(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDS_Form(web, _CT_NAME_PDS, klientId, okresId, key, terminPlatnosci, terminPrzekazania, true);

                    SPListItem formatka = null;

                    //zainicjowanie danych NKUP, WS NP
                    bool trybKwartalny = true;
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny, ref formatka); //tryp kwartalny

                    //zainicjowanie sumy strat z lat ubiegłych
                    Copy_SumyStratZLatUbieglych(web, klientId, okresId, ref formatka);

                    //zapisz zmiany
                    if (formatka != null) formatka.SystemUpdate();

                    //zainicjowanie kart w tabeli dochody wspólników
                    Create_DochodyWspolnikow(web, klientId, okresId);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("ERROR: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
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
                string key = tabZadania.Define_KEY(_CT_NAME_PDS, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    Debug.WriteLine("PDS_M klient:" + klientId.ToString());

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_M(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDS_Form(web, _CT_NAME_PDS, klientId, okresId, key, terminPlatnosci, terminPrzekazania, false);

                    //odniesienie do utworzonej powyżej karty rozliczenia PDS ponieważ powyższa procedura nie zwraca id karty
                    SPListItem formatka = null; 

                    //zainicjowanie danych NKUP, WS NP
                    bool trybKwartalny = false;
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny, ref formatka); //tryp kwartalny

                    //zainicjowanie sumy strat z lat ubiegłych
                    Copy_SumyStratZLatUbieglych(web, klientId, okresId, ref formatka);

                    //zapisz zmiany
                    if (formatka != null) formatka.SystemUpdate();

                    //zainicjowanie kart w tabeli dochody wspólników
                    Create_DochodyWspolnikow(web, klientId, okresId);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("ERROR: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
#else
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
#endif

            }
        }

        /// <summary>
        /// Kopiuje na bieżącą kartę informacje z poprzedzającego okresu (odpowiednio miesięcznie / kwartalnie)
        /// z uwzględnieniem zerowania od początku roku kalendarzowego.
        /// </summary>
        private static void Copy_DaneRozszerzone(SPWeb web, int klientId, int okresId, bool trybKwartalny, ref SPListItem formatka)
        {
            //jeżeli bieżący miesiąc > styczeń to kopuj dane z poprzedniej karty odpowiednio w/g trybu (miesięcznie/kwartalnie)

            SPListItem okres = BLL.tabOkresy.Get_OkresById(web, okresId);
            DateTime dataRozpoczecia = BLL.Tools.Get_Date(okres, "colDataRozpoczecia");
            if (dataRozpoczecia.Month == 1) return;

            //wyszukaj źródłową kartę kontrolną

            DateTime targetStartDate = BLL.Tools.Get_TargetStartDate(trybKwartalny, dataRozpoczecia);

            if (targetStartDate.Equals(new DateTime())) return; //dane niedostepne

            SPListItem targetOkres = BLL.tabOkresy.Get_OkresByStartDate(web, targetStartDate);

            int targetOkresId = 0;

            if (targetOkres != null) targetOkresId = targetOkres.ID;

            if (targetOkresId.Equals(0)) return; //dane niedostępne


            // znajdź kartę kontrolną

            SPListItem kk = BLL.tabKartyKontrolne.Get_KartaKontrolna(web, klientId, targetOkresId);

            if (kk == null) return; //dane niedostępne


            // skopiuj wartości z karty kontrolnej do bieżącej formatki

            // upewnij się że docelowa formatka została zainicjowana
            Ensure_CurrentPDS(web, klientId, okresId, ref formatka);

            if (formatka == null) return; // nie znaleziono formatki docelowej

            // koszty NKUP
            if (Copy(kk, formatka, "colKosztyNKUP_WynWyl")
                | Copy(kk, formatka, "colKosztyNKUP_ZUSPlatWyl")
                | Copy(kk, formatka, "colKosztyNKUP_FakWyl")
                | Copy(kk, formatka, "colKosztyNKUP_PozostaleKoszty"))
            {
                BLL.Tools.Set_Flag(formatka, "colKosztyNKUP", true);
            }

            // koszty WS
            if (Copy(kk, formatka, "colKosztyWS_WynWlaczone")
                | Copy(kk, formatka, "colKosztyWS_ZUSPlatWlaczone")
                | Copy(kk, formatka, "colKosztyWS_FakWlaczone"))
            {
                BLL.Tools.Set_Flag(formatka, "colKosztyWS", true);
            }

            // koszty NP
            if (Copy(kk, formatka, "colPrzychodyNP_DywidendySpO")
                | Copy(kk, formatka, "colPrzychodyNP_Inne"))
            {
                BLL.Tools.Set_Flag(formatka, "colPrzychodyNP", true);
            }

            var temp = Copy(kk, formatka, "colPrzychodyZwolnione")
                        | Copy(kk, formatka, "colStrataDoOdliczenia")
                        | Copy(kk, formatka, "colWplaconaSZ")
                        | Copy(kk, formatka, "colWplaconeZaliczkiOdPoczatkuRoku")
                        | Copy(kk, formatka, "colIleDoDoplaty")
                        | Copy(kk, formatka, "colZyskStrataNetto")
                        | Copy(kk, formatka, "colStronaWn")
                        | Copy(kk, formatka, "colStronaMa");



        }




        private static bool Copy(SPListItem srcItem, SPListItem dstItem, string col)
        {
            bool result = false;

            if (srcItem[col] != null)
            {
                dstItem[col] = srcItem[col];

                if (BLL.Tools.Get_Value(dstItem, col) > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        private static void Ensure_CurrentPDS(SPWeb web, int klientId, int okresId, ref SPListItem formatka)
        {
            if (formatka == null)
            {
                formatka = BLL.tabZadania.Get_Zadanie(web, klientId, okresId, _CT_NAME_PDS);
            }
        }

        private static void Copy_SumyStratZLatUbieglych(SPWeb web, int klientId, int okresId, ref SPListItem formatka)
        {
            DateTime d = BLL.tabOkresy.Get_StartDate(web, okresId);

            double sumaStrat = 0;
            double sumaOdliczen = 0;
            double sumaDoOdliczenia = 0;

            //sprawdź 5 ostatnich lat
            int currentYear = d.Year;
            for (int i = 0; i < 5; i++)
            {
                int targetYear = currentYear - 1 - i;
                int itemId = BLL.tabStratyZLatUbieglych.Ensure_RecordExist(web, klientId, targetYear);

                //dodaje wartości strat i odliczeń dla bieżącego rekordu
                BLL.tabStratyZLatUbieglych.Add_StratyIOdliczenia(web, itemId, ref sumaStrat, ref sumaOdliczen, ref sumaDoOdliczenia);
            }

            //zapisz wartości sumaStrat i sumaOdliczeń na formatce PDS

            if (sumaDoOdliczenia>0)
            {
                // upewnij się że docelowa formatka została zainicjowana
                Ensure_CurrentPDS(web, klientId, okresId, ref formatka);

                if (formatka == null) return; // nie znaleziono formatki docelowej

                BLL.Tools.Set_Value(formatka, "colStrataDoOdliczenia", sumaDoOdliczenia);
            }
        }

        private static void Create_DochodyWspolnikow(SPWeb web, int klientId, int okresId)
        {
            Debug.WriteLine("Dochody wpsólników:" + klientId.ToString());

            Array wspolnicy = tabKlienci.Get_WspolnicyByKlientId(web, klientId);

            //zainicjuj rekord na bieżący okres w tabeli wspólnicy dla każdego wspólnika niezależnie.

            foreach (SPListItem wItem in wspolnicy)
            {
                BLL.tabDochodyWspolnikow.Ensure_RecordInitiated(web, wItem, klientId, okresId);
            }
        }
    }
}
