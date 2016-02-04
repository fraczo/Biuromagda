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

        public static string Define_KEY(int klientId, int okresId)
        {
            string result;

            result = String.Format(@"{0}:{1}",
                klientId.ToString(),
                okresId.ToString());

            return result;
        }


        /// <summary>
        /// Inicjuje rekordy rozliczenia wspólników spółek na podstawia aktualnej konfiguracji klienta w kartotece.
        /// </summary>
        public static void Ensure_RecordInitiated(Microsoft.SharePoint.SPWeb web, Microsoft.SharePoint.SPListItem klientItem, int klientId, int okresId)
        {
            Debug.WriteLine("tab.DochowyWspolnikow.Ensure_RecordInitiated, powiązanieId=" + klientItem.ID.ToString());

            int result = 0;

            SPList list = web.Lists.TryGetList(targetList);

            string key = Define_KEY(klientItem.ID, okresId);

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
                newItem["selKlient"] = klientItem.ID;
                newItem["selOkres"] = okresId;

                newItem["selKlient_NazwaSkrocona"] = klientId;
                newItem["colPD_UdzialWZysku"] = BLL.Tools.Get_Value(klientItem, "colPD_UdzialWZysku");

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
    }
}
