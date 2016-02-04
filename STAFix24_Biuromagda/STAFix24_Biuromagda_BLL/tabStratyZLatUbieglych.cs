using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class tabStratyZLatUbieglych
    {

        const string targetList = "Straty z lat ubiegłych";

        public static int Ensure_RecordExist(Microsoft.SharePoint.SPWeb web, int klientId, int targetYear)
        {
            int result = 0;
            
            SPList list = web.Lists.TryGetList(targetList);

            string key = Define_KEY(klientId, targetYear);

            SPListItem item = list.Items.Cast<SPListItem>()
                            .Where(i => BLL.Tools.Get_Text(i,"KEY").Equals(key))
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
                newItem["selKlient"] = klientId;
                newItem["colRokObrachunkowy"] = targetYear;

                newItem.SystemUpdate();

                result = newItem.ID;
            }

            return result;
        }

        public static string Define_KEY(int klientId, int year)
        {
            string result;

            result = String.Format(@"{0}:{1}",
                klientId.ToString(),
                year.ToString());

            return result;
        }

        /// <summary>
        /// powiększa wartości strat i odliczeń o wartości z bieżącego rekordu
        /// </summary>
        private static void Add_StratyIOdliczenia(SPWeb web, int itemId, ref double sumaStrat, ref double sumaOdliczen, ref double sumaDoDoliczenia)
        {
            SPListItem item = web.Lists.TryGetList(targetList).GetItemById(itemId);
            double sStrat = BLL.Tools.Get_Value(item, "colWysokoscStraty");
            double sOdliczen = BLL.Tools.Get_Value(item, "colOdliczono");

            double maxOdliczenie = sStrat / 2;
            double sDoDoliczenia = sStrat - sOdliczen;
            if (sDoDoliczenia > maxOdliczenie) sDoDoliczenia = maxOdliczenie;

            sumaDoDoliczenia = sumaDoDoliczenia + sDoDoliczenia;
            sumaStrat = sumaStrat + sStrat;
            sumaOdliczen = sumaOdliczen + sOdliczen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="web"></param>
        /// <param name="klientId"></param>
        /// <param name="okresId">wskazuje na biżący okres na podstawie którego ustalany jest bieżący rok</param>
        /// <returns></returns>
        public static double Get_SumaDoOdliczenia(SPWeb web, int klientId, int okresId)
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
                int itemId = Ensure_RecordExist(web, klientId, targetYear);

                //dodaje wartości strat i odliczeń dla bieżącego rekordu
                Add_StratyIOdliczenia(web, itemId, ref sumaStrat, ref sumaOdliczen, ref sumaDoOdliczenia);
            }
            return sumaDoOdliczenia;
        }
    }
}
