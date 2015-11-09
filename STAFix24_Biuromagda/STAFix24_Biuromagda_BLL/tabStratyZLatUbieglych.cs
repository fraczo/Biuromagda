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
                            .Where(i => i["KEY"] == key)
                            .ToList()
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
                                
    }
}
