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
        public static void Add_StratyIOdliczenia(SPWeb web, int itemId, ref double sumaStrat, ref double sumaOdliczen)
        {
            SPListItem item = web.Lists.TryGetList(targetList).GetItemById(itemId);
            double sStrat = BLL.Tools.Get_Value(item, "colWysokoscStraty");
            double sOdliczen = BLL.Tools.Get_Value(item, "colOdliczono");
            sumaStrat = sumaStrat + sStrat;
            sumaOdliczen = sumaOdliczen + sOdliczen;
        }
    }
}
