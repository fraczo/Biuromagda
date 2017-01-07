using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using System.Diagnostics;

namespace EventReceivers.admProcessRequestsER
{
    public class PDW_Forms
    {

        const string _CT_NAME_PDW = @"Rozliczenie podatku dochodowego wspólnika";

        public static void CreateNew(SPWeb web, SPListItem item, int okresId, Array zadania)
        {
            if (item != null)
            {
                SPFieldLookupValueCollection kody;

                kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"PDW-M":
                            Create_PDW_M_Form(web, item.ID, okresId, zadania);
                            break;
                        case @"PDW-KW":
                            Create_PDW_KW_Form(web, item.ID, okresId, zadania);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void Create_PDW_KW_Form(SPWeb web, int klientId, int okresId, Array zadania)
        {
            try
            {
                string key = tabZadania.Define_KEY(_CT_NAME_PDW, klientId, okresId);


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
                    Debug.WriteLine("PDW_KW klient:" + klientId.ToString());

                    //zainicjowanie formatki PDS

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_KW(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDW_Form(web, _CT_NAME_PDW, klientId, okresId, key, terminPlatnosci, terminPrzekazania, true);

                    SPListItem formatka = null;

                    bool trybKwartalny = true; //tryp kwartalny
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny, ref formatka);

                    //zapisz zmiany
                    if (formatka != null) formatka.SystemUpdate();

                }
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());

            }
        }

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
            Ensure_CurrentPDW(web, klientId, okresId, ref formatka);

            if (formatka == null) return; // nie znaleziono formatki docelowej

            // kopiowanie zawartości

            // Nieuzwględniona w kosztach składka społeczna

            Copy(kk, formatka, "colNieuwzglednionaSkladkaSpolecz");

            // wpłacona składka zdrowotna

            Copy(kk, formatka, "colWplaconaSZ");

            // wpłacone zeliczki od początku roku

            Copy(kk, formatka, "colWplaconeZaliczkiOdPoczatkuRok");

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

        private static void Create_PDW_M_Form(SPWeb web, int klientId, int okresId, Array zadania)
        {
            try
            {
                string key = tabZadania.Define_KEY(_CT_NAME_PDW, klientId, okresId);

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
                    Debug.WriteLine("PDW_M klient:" + klientId.ToString());

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności PD Wspólnika jak dla KPiR
                    tabOkresy.Get_PD_M(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDW_Form(web, _CT_NAME_PDW, klientId, okresId, key, terminPlatnosci, terminPrzekazania, false);
                    
                    SPListItem formatka = null;

                    bool trybKwartalny = false; //tryp miesięczny
                    Copy_DaneRozszerzone(web, klientId, okresId, trybKwartalny, ref formatka);

                    //zapisz zmiany
                    if (formatka != null) formatka.SystemUpdate();
                
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
            }
        }

        private static void Ensure_CurrentPDW(SPWeb web, int klientId, int okresId, ref SPListItem formatka)
        {
            if (formatka == null)
            {
                formatka = BLL.tabZadania.Get_Zadanie(web, klientId, okresId, _CT_NAME_PDW);
            }
        }
    }
}
