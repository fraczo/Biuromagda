﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class dicOperatorzy
    {
        const string targetList = "Operatorzy";


        internal static SPListItem GetItemById(Microsoft.SharePoint.SPWeb web, int operatorId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            return list.Items.GetItemById(operatorId);
        }

        internal static int Get_IdByName(SPWeb web, string name)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i.Title == name)
                .FirstOrDefault();
            return item != null ? item.ID : 0;
        }

        internal static int GetID(SPWeb web, string name, bool createIfNotFound)
        {
            int operatorId = Get_IdByName(web, name);
            if (operatorId > 0) return operatorId;
            else
            {
                SPList list = web.Lists.TryGetList(targetList);
                SPListItem newItem = list.AddItem();
                newItem["Title"] = name;
                newItem["colTelefon"] = BLL.admSetup.GetValue(web, "TELEFON_BIURA");
                newItem["colEmail"] = BLL.admSetup.GetValue(web, "EMAIL_BIURA");

                newItem.SystemUpdate();
                return newItem.ID;
            }
        }

        public static int Get_UserIdById(SPWeb web, int operatorId)
        {
            SPListItem item = Get_OperatorById(web, operatorId);
            return item["colKontoOperatora"] != null ? new SPFieldUserValue(item.Web, item["colKontoOperatora"].ToString()).User.ID : 0;
        }

        private static SPListItem Get_OperatorById(SPWeb web, int operatorId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            return list.GetItemById(operatorId);
        }

        public static int Get_OperatorIdByLoginName(SPWeb web, string loginName)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i["colKontoOperatora"]!=null)
                .Where(i => new SPFieldUserValue(web, i["colKontoOperatora"].ToString()).User.LoginName == loginName)
                .FirstOrDefault();
            if (item != null) return item.ID;
            else return 0;
        }
    }
}
