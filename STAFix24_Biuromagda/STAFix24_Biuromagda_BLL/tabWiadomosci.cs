using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class tabWiadomosci
    {
        const string targetList = "Wiadomości";

        public static void AddNew(SPWeb web, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem newItem = list.AddItem();
            newItem["Title"] = temat;
            newItem["colNadawca"] = nadawca;
            newItem["colOdbiorca"] = odbiorca;
            newItem["colKopiaDla"] = kopiaDla;
            newItem["colTresc"] = tresc;
            newItem["colTrescHTML"] = trescHTML;
            if (!string.IsNullOrEmpty(planowanaDataNadania.ToString()) && planowanaDataNadania != new DateTime())
            {
                newItem["colPlanowanaDataNadania"] = planowanaDataNadania.ToString();
            }
            newItem["colKopiaDoNadawcy"] = KopiaDoNadawcy;
            newItem["colKopiaDoBiura"] = KopiaDoBiura;

            newItem.Update();
        }
    }
}
