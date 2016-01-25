using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;

namespace EventReceivers.admProcessRequestsER
{
    public class ImportFakturElektronicznych
    {
        const string targetList = @"Faktury elektroniczne - import"; //"intFakturyElektroniczne";

        public static void Execute(SPListItem item, SPWeb web)
        {
            int okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;

            SPList list = web.Lists.TryGetList(targetList);

            list.Items.Cast<SPListItem>()
                .ToList()
                .ForEach(oItem =>
                {
                    Import_Faktura(oItem, okresId);
                });
        }

        #region Helpers

        private static void Import_Faktura(SPListItem item, int okresId)
        {
            int klientId = 0;
            string fileName = item.File.Name;

            string nazwaSkrocona = Extract_NazwaSkrocona(fileName);

            if (!String.IsNullOrEmpty(nazwaSkrocona))
            {
                klientId = tabKlienci.Get_KlientId(item.Web, nazwaSkrocona);
            }

            if (klientId > 0)
            {
                //item["Title"] = tabKlienci.Get_KlientById(item.Web, klientId).Title;
                item["Title"] = String.Empty;
                item["selKlient"] = klientId;
                item["selOkres"] = okresId;

                int zadanieId = tabZadania.Get_NumerZadaniaBR(item.Web, klientId, okresId);

                if (zadanieId > 0)
                {
                    item["selZadanie"] = zadanieId;
                    bool attResult = tabZadania.Add_FileFromURL(item.Web, zadanieId, item.File);
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

        #endregion

        public static void Remove_Completed(SPListItem item, SPWeb web)
        {
            SPList list = web.Lists.TryGetList(targetList);

            list.Items.Cast<SPListItem>()
                .Where(i => (i["selZadanie"] != null ? new SPFieldLookupValue(i["selZadanie"].ToString()).LookupId : 0) > 0)
                .Where(i => (i["selOkres"] != null ? new SPFieldLookupValue(i["selOkres"].ToString()).LookupId : 0) > 0)
                .Where(i => (i["selKlient"]!=null?new SPFieldLookupValue(i["selKlient"].ToString()).LookupId:0) > 0)
                .ToList()
                .ForEach(oItem =>
                {
                    oItem.Delete();
                });
        }
    }
}
