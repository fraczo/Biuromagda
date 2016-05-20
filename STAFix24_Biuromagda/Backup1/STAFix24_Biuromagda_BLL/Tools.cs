using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL.Models;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

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

        public static DateTime Get_Date(SPListItem item, string col)
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
            if (val != null) item[col] = val.ToString();
            else item[col] = string.Empty;
        }

        internal static Array Get_LookupValueCollection(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValueCollection(item[col].ToString()).ToArray() : null;
        }


        public static void Set_Flag(SPListItem item, string col, bool v)
        {
            item[col] = (bool)v;
        }

        public static SPFieldLookupValueCollection Get_LookupValueColection(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValueCollection(item[col].ToString()) : null;
        }


        public static bool Has_Service(SPListItem item, string code, string col)
        {
            if (code.EndsWith("*"))
            {
                code = code.Substring(0, code.Length - 1);
                foreach (SPFieldLookupValue v in BLL.Tools.Get_LookupValueColection(item, col))
                {
                    if (v.LookupValue.StartsWith(code)) return true;
                }
            } 
            else
            {
            foreach (SPFieldLookupValue v in BLL.Tools.Get_LookupValueColection(item, col))
            {
                if (v.LookupValue.Equals(code)) return true;
            }
            }

            return false;
        }

        public static string Get_Email(SPListItem item, string col)
        {
            string email = BLL.Tools.Get_Text(item, col);
            if (Is_ValidEmail(email))
            {
                return email;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void Set_Value(SPListItem item, string col, double v)
        {
            item[col] = (double)v;
        }

        /// <summary>
        /// zwraca datę w poszukiwanym okresie rozliczeniowym (try miesięczny cofnięcie o 1 miesiąc, try miesięczny cofa do ostatniego kwartału)
        /// </summary>
        /// <param name="trybKwartalny">kwartalny lub miesięczny</param>
        /// <param name="dataRozpoczecia">dowolna data z zakresu bieżącego okresu rozliczeniowego</param>
        /// <returns></returns>
        public static DateTime Get_TargetStartDate(bool trybKwartalny, DateTime dataRozpoczecia)
        {
            DateTime targetStartDate = new DateTime();

            if (trybKwartalny)
            {
                if (dataRozpoczecia.Month > 3)
                {
                    int reverse = dataRozpoczecia.Month % 3;
                    if (reverse == 0) reverse = 3;

                    targetStartDate = new DateTime(dataRozpoczecia.Year, dataRozpoczecia.Month, 1).AddMonths(-1 * reverse);
                }
                else
                {
                    //dane niedostępne
                }
            }
            else //tryb miesięczny
            {
                if (dataRozpoczecia.Month > 1)
                {
                    targetStartDate = new DateTime(dataRozpoczecia.Year, dataRozpoczecia.Month, 1).AddMonths(-1);
                }
                else
                {
                    //dane niedostępne
                }
            }
            return targetStartDate;
        }

        /// <summary>
        /// Wywołanie funkcji:
        /// DoWithRetry(DoSomething)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="sleepPeriod"></param>
        /// <param name="retryCount"></param>
        //public void DoWithRetry(Action action)
        public static void DoWithRetry(Action action)
        {
            TimeSpan sleepPeriod = TimeSpan.FromSeconds(2);
            int retryCount = 3;

            Debug.WriteLine("DoWithRetry activated");

            while (true)
            {
                try
                {
                    action();
                    break; // success!      
                }
                catch (Exception ex)
                {
                    if (--retryCount == 0)
                        throw;
                    else Thread.Sleep(sleepPeriod);

                    var r = ElasticEmail.EmailGenerator.ReportError(ex, "No of retries left: " + retryCount.ToString());
                }
            }
        }

        public static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(';');
            }
            return builder.ToString();
        }

        public static string ConvertStringArrayToStringJoin(string[] array)
        {
            //
            // Use string Join to concatenate the string elements.
            //
            string result = string.Join(";", array);
            return result;
        }

        public static string Format_Date(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string Format_Konto(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Regex rgx = new Regex("[^0-9]");
                s = rgx.Replace(s, "");
                if (s.Length == 26)
                {
                    s = "1" + s;
                    string r = Convert.ToDecimal(s).ToString("### #### #### #### #### #### ####");
                    return r.Substring(1, r.Length - 1);
                }
            }

            return "nieprawidłowy numer rachunku";
        }

        public static string Format_KontoBezSpacji(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Regex rgx = new Regex("[^0-9]");
                s = rgx.Replace(s, "");
                if (s.Length == 26)
                {
                    s = "1" + s;
                    string r = Convert.ToDecimal(s).ToString("###########################");
                    return r.Substring(1, r.Length - 1);
                }
            }

            return "nieprawidłowy numer rachunku";
        }

        public static void Set_Date(SPListItem item, string col, DateTime date)
        {
           if (date!=null && date!=new DateTime()) item[col] = date;
        }

        public static bool Has_ServiceMask(SPListItem item, string mask)
        {
            string col = "colMaskaSerwisu";
            if (mask.EndsWith("*")) mask = mask.Substring(0, mask.Length - 1);

            SPFieldMultiChoiceValue choices = item[col] != null ? new SPFieldMultiChoiceValue(item[col].ToString()) : null;
            for (int i = 0; i < choices.Count; i++)
            {
                string s = choices[i];
                if (s.StartsWith(mask)) return true;
            }

            return false;
        }
    }
}
