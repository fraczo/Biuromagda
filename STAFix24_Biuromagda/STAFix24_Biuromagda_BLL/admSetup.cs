using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL.Models;

namespace BLL
{
    public class admSetup
    {
        const string targetList = @"admSetup";

        internal static Models.KontaZUS GetKontaZUS(Microsoft.SharePoint.SPWeb web)
        {
            SPList list  = web.Lists.TryGetList(targetList);

            KontaZUS obj = new Models.KontaZUS();

            if (list != null)
            {
                //SP
                SPListItem item = list.Items.Cast<SPListItem>()
                    .Where(i => i["KEY"].ToString() == @"ZUS_SP_KONTO")
                    .FirstOrDefault();

                if (item!=null)
                {
                    obj.KontoSP = item["VALUE"].ToString();
                }

                //ZD
                item = list.Items.Cast<SPListItem>()
                    .Where(i => i["KEY"].ToString() == @"ZUS_ZD_KONTO")
                    .FirstOrDefault();

                if (item != null)
                {
                    obj.KontoZD = item["VALUE"].ToString();
                }

                //FP
                item = list.Items.Cast<SPListItem>()
                    .Where(i => i["KEY"].ToString() == @"ZUS_FP_KONTO")
                    .FirstOrDefault();

                if (item != null)
                {
                    obj.KontoFP = item["VALUE"].ToString();
                }
            }

            return obj;

        }

        public static string GetValue(SPWeb web, string key)
        {
            SPList list = web.Lists.TryGetList(targetList);
            //if (list!=null)
            //{
                SPListItem item = list.Items.Cast<SPListItem>()
                    .Where(i => i["KEY"].ToString() == key)
                    .FirstOrDefault();

                if (item!=null)
                {
                    return item["VALUE"].ToString();
                }
            //}

            return string.Empty;
        }

        public static string GetText(SPWeb web, string key)
        {
            SPList list = web.Lists.TryGetList(targetList);
            //if (list != null)
            //{
                SPListItem item = list.Items.Cast<SPListItem>()
                    .Where(i => i["KEY"].ToString() == key)
                    .FirstOrDefault();

                if (item != null)
                {
                    return item["TEXT"].ToString();
                }
            //}

            return string.Empty;
        }
    }
}
