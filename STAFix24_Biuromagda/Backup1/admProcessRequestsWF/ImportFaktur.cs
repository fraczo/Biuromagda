using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace Workflows.admProcessRequestsWF
{
    class ImportFaktur
    {
        internal static void Import_Faktura(SPListItem item, int okresId)
        {
            int klientId = 0;
            string fileName = item.File.Name;

            string nazwaSkrocona = Extract_NazwaSkrocona(fileName);

            if (!String.IsNullOrEmpty(nazwaSkrocona))
            {
                klientId = BLL.tabKlienci.Get_KlientId(item.Web, nazwaSkrocona);
            }

            if (klientId > 0)
            {
                //item["Title"] = tabKlienci.Get_KlientById(item.Web, klientId).Title;
                item["Title"] = String.Empty;
                item["selKlient"] = klientId;
                item["selOkres"] = okresId;

                int zadanieId = BLL.tabZadania.Get_NumerZadaniaBR(item.Web, klientId, okresId);

                if (zadanieId > 0)
                {
                    item["selZadanie"] = zadanieId;
                    bool attResult = BLL.tabZadania.Add_FileFromURL(item.Web, zadanieId, item.File);
                }
            }
            else
            {
                item["Title"] = "niezgodna nazwa pliku";
                item["selKlient"] = 0;
                item["selOkres"] = 0;
                item["selZadanie"] = 0;
            }

            item.Update();
        }

        private static string Extract_NazwaSkrocona(string fileName)
        {
            string result = string.Empty;

            var startIndex = 4;
            var endIndex = fileName.IndexOf(@" -");
            var len = endIndex - startIndex + 1;

            if (len > 0)
            {
                result = fileName.Substring(startIndex, len)
                    .Trim()
                    .ToUpper();
            }

            return result;

        }
    }
}
