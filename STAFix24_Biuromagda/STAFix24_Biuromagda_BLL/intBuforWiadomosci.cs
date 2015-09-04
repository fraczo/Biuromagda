using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class intBuforWiadomosci
    {
        const string targetList = "intBuforWiadomosci";

        public static int AddNewItem(SPWeb web, int klientId, string komunikat, int szablonId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            if (list!=null)
            {
                SPListItem item = list.AddItem();

                item["selKlient_NazwaSkrocona"] = klientId;
                item["strKomunikat"] = komunikat;
                item["selSzablonWiadomosci"] = szablonId;

                item.Update();

                return item.ID;
            }

            return 0;
        }


    }
}
