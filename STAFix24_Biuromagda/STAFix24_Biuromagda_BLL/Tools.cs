using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL.Models;
using System.Globalization;

namespace BLL
{
    public class Tools
    {
        const string emptyMarker = "---";

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
        public static void Ensure_Column(SPListItem item, string targetColumn)
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


        public static string AddCompanyName(SPWeb web, string temat, int klientId)
        {
            if (klientId > 0)
            {
                BLL.Models.Klient k = new Klient(web, klientId);
                return string.Format("{0} {1}", temat, k.PelnaNazwaFirmy);
            }

            return temat;
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

        public static int Get_LookupId(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }

        public static string Format_Currency(SPListItem item, string colName)
        {
            double n = Get_Value(item, colName);

            if (n > 0) return n.ToString("c", new CultureInfo("pl-PL"));
            else return emptyMarker;

        }

        public static double Get_Value(SPListItem item, string colName)
        {
            if (item[colName] != null)
            {
                return double.Parse(item[colName].ToString());
            }
            else
            {
                return 0;
            }
        }

        public static string Format_Currency(double value)
        {
            if (value > 0) return value.ToString("c", new CultureInfo("pl-PL"));
            else return emptyMarker;
        }

        internal static DateTime Get_Date(SPListItem item, string col)
        {
            return item[col] != null ? DateTime.Parse(item[col].ToString()) : new DateTime();
        }

        public static string Get_Text(SPListItem item, string col)
        {
            return item[col] != null ? item[col].ToString() : string.Empty;
        }


        // zakłada format wejściowy YYYY-MM
        public static string Get_KwartalDisplayName(string okres)
        {
            if (okres.Length == 7)
            {
                string rok = okres.Substring(0, 4);
                string miesiac = okres.Substring(5, 2);
                int mNumber = int.Parse(miesiac);
                switch (mNumber)
                {
                    case 1:
                    case 2:
                    case 3:
                        return "I " + rok;
                    case 4:
                    case 5:
                    case 6:
                        return "II " + rok;
                    case 7:
                    case 8:
                    case 9:
                        return "III " + rok;
                    case 10:
                    case 11:
                    case 12:
                        return "IV " + rok;
                    default:
                        return string.Empty;
                }
            }
            return string.Empty;
        }

        public static string Get_LookupValue(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupValue : string.Empty;
        }

        public static bool Get_Flag(SPListItem item, string col)
        {
            return item[col] != null ? (bool)item[col] : false;
        }

        public static void Clear_Value(SPListItem item, string col)
        {

            if (item[col] != null)
            {
                item[col] = string.Empty;
                item.SystemUpdate();
            }
        }

        public static void Clear_Flag(SPListItem item, string col)
        {
            if (item[col] != null)
            {
                item[col] = false;
                item.SystemUpdate();
            }
        }

        internal static string Get_CurrentUser(SPListItem item)
        {
            string result = item["Editor"] != null ? new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email : string.Empty;

            if (string.IsNullOrEmpty(result))
            {
                //ustaw domyślnie adres biura
                result = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
            }

            if (BLL.Tools.Is_ValidEmail(result))
            {
                return result;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool Is_ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void Set_Text(SPListItem item, string col, string val)
        {
            item[col] = val.ToString();
        }

        internal static Array Get_LookupValueCollection(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValueCollection(item[col].ToString()).ToArray() : null;
        }


        public static void Set_Flag(SPListItem item, string col, bool v)
        {
            item[col] = v;
        }

        public static SPFieldLookupValueCollection Get_LookupValueColection(SPListItem item, string col)
        {
            return item[col]!=null?new SPFieldLookupValueCollection(item[col].ToString()):null;
        }


        public static bool Has_Service(SPListItem item, SPFieldLookupValue s, string col)
        {
            foreach (SPFieldLookupValue v in BLL.Tools.Get_LookupValueColection(item, col))
            {
                if (v.LookupId.Equals(s.LookupValue)) return true;
            }

            return false;
        }
    }
}
