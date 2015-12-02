using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

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
            int result = 0;

            SPList list = web.Lists.TryGetList(targetList);

            string key = Define_KEY(klientItem.ID, okresId);

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
                newItem["selKlient"] = klientItem.ID;
                newItem["selOkres"] = okresId;

                newItem["selKlient_NazwaSkrocona"] = klientId;
                //newItem["colPD_OcenaWyniku"] =
                BLL.Models.Klient iok = new Models.Klient(web, klientItem.ID);
                newItem["colFormaOpodatkowaniaPD"] = iok.FormaOpodatkowaniaKSH;
                //newItem["colPodatekNaliczony"] =
                //newItem["colWplaconaSkladkaZdrowotna"] =
                //newItem["colWplaconeZaliczki"] =
                //newItem["colPodatekWspolnikaDoZaplaty"] = 

                newItem.SystemUpdate();

                result = newItem.ID;
            }
        }
    }
}
