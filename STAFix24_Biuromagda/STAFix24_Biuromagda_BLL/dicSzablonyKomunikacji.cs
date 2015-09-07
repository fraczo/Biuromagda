using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class dicSzablonyKomunikacji
    {
        const string targetList = "Szablony komunikacji";

        public static void Get_TemplateByKod(Microsoft.SharePoint.SPWeb web, string kod, out string temat, out string trescHTML)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i.Title == kod)
                .FirstOrDefault();

            temat = item["colTematWiadomosci"] != null ? item["colTematWiadomosci"].ToString() : string.Empty;
            trescHTML = item["colHTML"] != null ? item["colHTML"].ToString() : string.Empty;

            //zapakuj treść do szablonu
            if (kod.EndsWith(".Include"))
            {
                string sTemat = string.Empty;
                string sTrescHTML = string.Empty;
                Get_TemplateByKod(web, "EMAIL_DEFAULT_BODY", out sTemat, out sTrescHTML);
                StringBuilder sb = new StringBuilder(sTrescHTML);
                sb.Replace("___BODY___", trescHTML);
                trescHTML = sb.ToString();
            }
        }
    }
}
