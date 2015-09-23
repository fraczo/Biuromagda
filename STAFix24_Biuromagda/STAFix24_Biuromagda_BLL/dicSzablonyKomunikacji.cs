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

        public static void Get_TemplateByKod(SPListItem item, string kod, out string temat, out string trescHTML)
        {
            switch (item.ParentList.Title)
            {
                case "Zadania":
                    //zobacz czy operator jest przypisany do zadania

                    string temp = string.Empty;
                    string footerTR = string.Empty;
                    Get_TemplateByKod(item.Web, "EMAIL_FOOTER_TR", out temp, out footerTR, false);

                    int operatorId = Get_LookupId(item, "selOperator");
                    if (operatorId > 0)
                    {
                        //użyj stopki konkretnego operatora
                        BLL.Models.Operator op = new Models.Operator(item.Web, operatorId);

                        footerTR = footerTR.Replace("___NAME___", op.Name);
                        footerTR = footerTR.Replace("___CONTACT___", string.Format(@"email: {0}<br>tel.: {1}", op.Email, op.Telefon));
                    }
                    else
                    {
                        //użyj stopki biura
                        string name = BLL.admSetup.GetValue(item.Web, "NAZWA_BIURA");
                        string email = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                        string tel = BLL.admSetup.GetValue(item.Web, "TELEFON_BIURA");

                        footerTR = footerTR.Replace("___NAME___", name);
                        footerTR = footerTR.Replace("___CONTACT___", string.Format(@"email: {0}<br>tel.: {1}", email, tel));
                    }
                    Get_TemplateByKod(item.Web, kod, out temat, out trescHTML, true);
                    trescHTML = trescHTML.Replace("___FOOTER___", footerTR);
                    break;

                default:
                    Get_TemplateByKod(item.Web, kod, out temat, out trescHTML, true);
                    break;
            }



        }

        /// <summary>
        /// pobiera odpowiedni szablon wiadomości i ukrywa sekcję footer jeżeli flaga nie jest ustawiona
        /// </summary>
        public static void Get_TemplateByKod(SPWeb web, string kod, out string temat, out string trescHTML, bool hasFooter)
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
                Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY", out sTemat, out sTrescHTML);
                StringBuilder sb = new StringBuilder(sTrescHTML);
                sb.Replace("___BODY___", trescHTML);

                //wyczyść stopkę jeżeli nie jest potrzebna
                if (!hasFooter) sb.Replace("___FOOTER___", string.Empty);

                trescHTML = sb.ToString();
            }
        }

        public static string Get_TemplateByKod(SPListItem item, string kod, bool hasFooter)
        {
            string temp;
            string trescHTML = string.Empty;
            Get_TemplateByKod(item, kod, out temp, out trescHTML);

            return trescHTML;
        }

        private static int Get_LookupId(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }




    }
}
