using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Diagnostics;

namespace BLL
{
    public class tabDochodyWspolnikow
    {
        private const string targetList = @"Dochody wspólników";

        public static string Define_KEY(int klientId, int wspolnikId, int okresId)
        {
            string result;

            result = String.Format(@"{0}:{1}:{2}",
                klientId.ToString(),
                wspolnikId.ToString(),
                okresId.ToString());

            return result;
        }


        /// <summary>
        /// Inicjuje rekordy rozliczenia wspólników spółek na podstawia aktualnej konfiguracji klienta w kartotece.
        /// </summary>
        public static void Ensure_RecordInitiated(Microsoft.SharePoint.SPWeb web, Microsoft.SharePoint.SPListItem powiazanieItem, int klientId, int okresId)
        {
            int wspolnikId = BLL.Tools.Get_LookupId(powiazanieItem, "selKlient");

            Debug.WriteLine("tab.DochowyWspolnikow.Ensure_RecordInitiated, powiązanieId=" + powiazanieItem.ID.ToString());

            int result = 0;

            SPList list = web.Lists.TryGetList(targetList);

            string key = Define_KEY(klientId, wspolnikId, okresId);

            SPListItem item = list.Items.Cast<SPListItem>()
                            .Where(i => BLL.Tools.Get_Text(i, "KEY").Equals(key))
                            .FirstOrDefault();
            if (item != null)
            {
                result = item.ID;
            }
            else
            {
                //create new record
                SPListItem newItem = list.AddItem();
                newItem["KEY"] = key;
                newItem["selKlient"] = wspolnikId;
                newItem["selOkres"] = okresId;

                newItem["selKlient_NazwaSkrocona"] = klientId;
                newItem["colPD_UdzialWZysku"] = BLL.Tools.Get_Value(powiazanieItem, "colPD_UdzialWZysku");

                newItem.Update();

                result = newItem.ID;
            }
        }

        public static double Sum_UdzalyWspolnikow(SPWeb web, int klientId, int okresId)
        {
            Array results = web.Lists.TryGetList(targetList).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_LookupId(i, "selKlient_NazwaSkrocona").Equals(klientId))
                .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId))
                .ToArray();

            double s = 0;
            foreach (SPListItem item in results)
            {
                s = s + BLL.Tools.Get_Value(item, "colPD_UdzialWZysku");
            }

            return s;
        }

        public static double Update_DochodyWspolnikow(SPWeb web, int klientId, int okresId, double colZyskStrataNetto, out string validationMessage)
        {
            validationMessage = string.Empty;

            Debug.WriteLine("Update_OcenaWyniku");

            double sumZysk = 0;
            double sumStrata = 0;

            Array results = web.Lists.TryGetList(targetList).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_LookupId(i, "selKlient_NazwaSkrocona").Equals(klientId))
                .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId))
                .ToArray();

            if (results != null)
            {
                foreach (SPListItem item in results)
                {
                    Debug.WriteLine("Klient: " + BLL.Tools.Get_LookupValue(item, "selKlient"));

                    double colPD_UdzialWZysku = BLL.Tools.Get_Value(item, "colPD_UdzialWZysku");

                    Debug.WriteLine("Udział w zysku: " + colPD_UdzialWZysku.ToString());

                    if (colZyskStrataNetto >= 0 && colPD_UdzialWZysku > 0)
                    {
                        BLL.Tools.Set_Text(item, "colPD_OcenaWyniku", "Dochód");

                        //zaokrąglij wynik
                        double colPD_WartoscDochodu = colZyskStrataNetto * colPD_UdzialWZysku;
                        sumZysk = sumZysk + colPD_WartoscDochodu;
                        BLL.Tools.Set_Value(item, "colPD_WartoscDochodu", colPD_WartoscDochodu);

                        BLL.Tools.Clear_Value(item, "colPD_WartoscStraty");
                    }
                    else
                    {
                        BLL.Tools.Set_Text(item, "colPD_OcenaWyniku", "Dochód");

                        BLL.Tools.Clear_Value(item, "colPD_WartoscDochodu");

                        //zaokrąglij wynik
                        double colPD_WartoscStraty = -1 * colZyskStrataNetto * colPD_UdzialWZysku;
                        sumStrata = sumStrata + colPD_WartoscStraty;
                        BLL.Tools.Set_Value(item, "colPD_WartoscStraty", colPD_WartoscStraty);
                    }

                    item.SystemUpdate();


                    // zaktualizuj zadanie rozliczenia wspólnika jeżeli istnieje
                    string comments;
                    Execute_UpdateRequest(web, BLL.Tools.Get_LookupId(item, "selKlient"), okresId, out comments);

                    if (!string.IsNullOrEmpty(comments))
                    {
                        validationMessage = validationMessage + string.Format("<li>{0}</li>", comments);
                    }
                }

            }

            double variance = (sumZysk - sumStrata) - colZyskStrataNetto;

            Debug.WriteLine("sumZysk =" + sumZysk.ToString());
            Debug.WriteLine("sumStrata =" + sumStrata.ToString());
            Debug.WriteLine("colZyskStrataNetto =" + colZyskStrataNetto.ToString());
            Debug.WriteLine("variance =" + variance.ToString());

            return variance;

        }

        public static void Get_PrzychodyWspolnika(SPWeb web, int wspolnikId, int okresId, out double przychod, out string specyfikacja)
        {
            double sumD = 0;
            double sumS = 0;
            StringBuilder sb = new StringBuilder();

            string TABLE_TEMPLATE = @"<table cellpadding=""5"" style=""FONT-SIZE: x-small; FONT-FAMILY: Arial, Helvetica, sans-serif; WIDTH: 100%;""><tr><th style=""BACKGROUND-COLOR: #e4e4e4; height: 28px;"">Firma</th><th style=""BACKGROUND-COLOR: #e4e4e4; height: 28px;"">Ocena wyniku</th><th style=""BACKGROUND-COLOR: #e4e4e4; height: 28px;"">Dochód</th><th style=""BACKGROUND-COLOR: #e4e4e4; height: 28px;"">Strata</th></tr>[[TableRow]] </table>";
            string TABLEROW_TEMPLATE = @"<tr><th style=""FONT-SIZE: x-small; HEIGHT: 16px; FONT-FAMILY: Arial, Helvetica, sans-serif; BACKGROUND-COLOR: #e4e4e4"">[[Firma]]</th> <td style=""FONT-SIZE: x-small; HEIGHT: 16px; FONT-FAMILY: Arial, Helvetica, sans-serif; WHITE-SPACE: nowrap; TEXT-ALIGN: center"">[[OcenaWyniku]]</td> <td style=""FONT-SIZE: x-small; HEIGHT: 16px; FONT-FAMILY: Arial, Helvetica, sans-serif; WHITE-SPACE: nowrap; TEXT-ALIGN: center"">[[Dochod]]</td> <td style=""FONT-SIZE: x-small; HEIGHT: 16px; FONT-FAMILY: Arial, Helvetica, sans-serif; WHITE-SPACE: nowrap; TEXT-ALIGN: center"">[[Strata]]</td> </tr>";

            Array results = web.Lists.TryGetList(targetList).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_LookupId(i, "selKlient").Equals(wspolnikId))
                .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId))
                .ToArray();

            if (results != null & results.Length > 0)
            {
                foreach (SPListItem item in results)
                {
                    StringBuilder row = new StringBuilder(TABLEROW_TEMPLATE);
                    row.Replace("[[Firma]]", BLL.Tools.Get_LookupValue(item, "selKlient_NazwaSkrocona"));

                    string ow = BLL.Tools.Get_Text(item, "colPD_OcenaWyniku");
                    row.Replace("[[OcenaWyniku]]", ow);

                    if (ow.Equals("Dochód"))
                    {
                        double dochod = BLL.Tools.Get_Value(item, "colPD_WartoscDochodu");
                        row.Replace("[[Dochod]]", BLL.Tools.Format_Currency(dochod));
                        row.Replace("[[Strata]]", string.Empty);
                        sumD = sumD + dochod;
                    }
                    if (ow.Equals("Strata"))
                    {
                        double strata = BLL.Tools.Get_Value(item, "colPD_WartoscStraty");
                        row.Replace("[[Strata]]", BLL.Tools.Format_Currency(strata));
                        row.Replace("[[Dochod]]", string.Empty);
                        sumS = sumS + strata;
                    }

                    sb.Append(row);
                }
            }

            przychod = sumD - sumS;

            StringBuilder sb0 = new StringBuilder(TABLE_TEMPLATE);
            sb0.Replace("[[TableRow]]", sb.ToString());
            specyfikacja = sb0.ToString();
        }

        /// <summary>
        /// Sprawdza czy dla danego wspólnika w danym okresie jest zdefiniowana karta rozliczeniowa PDS lub PDW
        /// Jeżeli jest to inicjuje procedurę aktualizacji karty
        /// Jeżeli nie to zwraca komunikat że karta rozliczeniowa nie zostałą zainicjowana
        /// </summary>
        public static void Execute_UpdateRequest(SPWeb web, int wspolnikId, int okresId, out string comments)
        {
            comments = string.Empty;

            int zadanieId = 0;
            //sprawdź czy istnieje zadanie typu PDS

            zadanieId = BLL.tabZadania.Get_ZadanieByKEY(web,
                 BLL.tabZadania.Define_KEY("Rozliczenie podatku dochodowego spółki", wspolnikId, okresId));

            if (zadanieId <= 0)
            {
                //sprawdź czy istnieje zadanie typu PDW

                zadanieId = BLL.tabZadania.Get_ZadanieByKEY(web,
                    BLL.tabZadania.Define_KEY("Rozliczenie podatku dochodowego wspólnika", wspolnikId, okresId));
            }

            if (zadanieId > 0)
            {
                // zainicjuj procedurę rozliczenia
                BLL.tabZadania.Execute_Update_DochodyZInnychSpolek(web, wspolnikId, okresId, zadanieId, out comments);
            }
            else
            {
                BLL.Models.Klient iok = new Models.Klient(web, wspolnikId);
                SPListItem okres = BLL.tabOkresy.Get_OkresById(web, okresId);
                
                // karta rozliczeniowa PDS/PDW nie została znaleziona >>> wyślij komunikat
                StringBuilder sb = new StringBuilder(comments);
                sb.AppendFormat("<div>Brak zadania rozliczenia podatku dochodowego wspólnika (PDS|PDW) dla klienta {0} w okresie {1}</div>",
                    iok.PelnaNazwaFirmy, okres.Title);

                comments = sb.ToString();
            }

        }
    }
}
