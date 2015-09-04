using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class dicOperatorzy
    {
        const string targetList = "Operatorzy"; // "dicOperatorzy";
        const string nazwaOperatora_Default = @"STAFix24 Robot";

        public static int GetID(SPWeb web, string nazwaOperatora, bool createIfNotExist)
        {
            if (string.IsNullOrEmpty(nazwaOperatora))
            {
                nazwaOperatora = nazwaOperatora_Default;
            }

            int result = 0;

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
                SPListItem item = list.Items.Cast<SPListItem>()
                    .Where(i => i.Title == nazwaOperatora)
                    .ToList()
                    .FirstOrDefault();

                if (item != null)
                {
                    result = item.ID;
                }
            //}

            if (result == 0 && createIfNotExist)
            {
                try
                {
                     item = list.AddItem();
                    item["Title"] = nazwaOperatora;
                    item.Update();
                }
                catch (Exception)
                { }
                finally
                {
                     item = list.Items.Cast<SPListItem>()
                        .Where(i => i.Title == nazwaOperatora)
                        .ToList()
                        .FirstOrDefault();

                    result = item.ID;
                }
            }

            return result;
        }

        internal static int Get_IdByName(SPWeb web, string v)
        {
            SPList list = web.Lists.TryGetList(targetList);
            //if (list!=null)
            //{
                SPListItem item = list.Items.Cast<SPListItem>()
                    .Where(i => i.Title == v)
                    .FirstOrDefault();

                if (item != null)
                {
                    return item.ID;
                }
            //}

            SPListItem newItem = list.AddItem();
            newItem["Title"] = v;
            newItem.Update();

            return newItem.ID;
        }


        public static int Get_UserIdById(SPWeb web, int operatorId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            //if (list != null)
            //{
                SPListItem item = list.GetItemById(operatorId);
                if (item!=null && item["colKontoOperatora"]!=null)
                {
                    int kontoOperatoraId = new SPFieldUserValue(web, item["colKontoOperatora"].ToString()).LookupId;

                    return kontoOperatoraId;
                }
            //}

            return 0;
        }
    }
}
