using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL.Models;

namespace BLL
{
    public class Tools
    {
        internal static string Get_ItemInfo(Microsoft.SharePoint.SPListItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CT=" + item.ContentType.Name);
            sb.AppendLine("ID=" + item.ID.ToString());

            return sb.ToString();
        }

        public static void Ensure_LinkColumn(SPListItem item, string sourceColumn)
        {
            string col1 = sourceColumn + "_LINK";
            string col2 = sourceColumn + "_LINKID";

            Ensure_Column(item, col1);
            item[col1] = item[sourceColumn] != null ? item[sourceColumn].ToString() : string.Empty;

            Ensure_Column(item, col2);
            item[col2] = item[sourceColumn] != null ? new SPFieldLookupValue(item[sourceColumn].ToString()).LookupId.ToString() : string.Empty;

            item.SystemUpdate();

        }

        /// <summary>
        /// definiuje kolumnę w razie potrzeby
        /// </summary>
        private static void Ensure_Column(SPListItem item, string targetColumn)
        {
            bool found = false;
            SPList list = item.ParentList;
            foreach (SPField col in list.Fields)
            {
                if (col.InternalName == targetColumn)
                {
                    found = true;
                    break;
                }
            }

            if (!found) Create_Column(list, targetColumn);
        }

        private static void Create_Column(SPList list, string targetColumn)
        {
            SPFieldText f = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), targetColumn);

            list.Fields.Add(f);
            list.Update();
        }

        public static string AddCompanyName(string temat, SPListItem item)
        {
            if (item != null)
            {
                if (item.ContentType.Name == "KPiR" || item.ContentType.Name == "KSH")
                {
                    int klientId = Get_LookupId(item, "selKlient");
                    if (klientId > 0)
                    {
                        BLL.Models.Klient k = new Klient(item.Web, klientId);
                        return string.Format("{0} {1}", temat, k.PelnaNazwaFirmy);
                    }
                }

                if (item.ContentType.Name == "Prośba o dokumenty"
                    || item.ContentType.Name == "Prośba o przesłanie wyciągu bankowego"
                    || item.ContentType.Name == "Rozliczenie podatku dochodowego"
                    || item.ContentType.Name == "Rozliczenie podatku dochodowego spółki"
                    || item.ContentType.Name == "Rozliczenie podatku VAT"
                    || item.ContentType.Name == "Rozliczenie z biurem rachunkowym"
                    || item.ContentType.Name == "Rozliczenie ZUS")
                {
                    int klientId = Get_LookupId(item, "selKlient");
                    if (klientId > 0)
                    {
                        BLL.Models.Klient k = new Klient(item.Web, klientId);
                        return string.Format("{0} {1}", temat, k.PelnaNazwaFirmy);
                    }
                }
            }
            return temat;
        }

        private static int Get_LookupId(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }
    }
}
