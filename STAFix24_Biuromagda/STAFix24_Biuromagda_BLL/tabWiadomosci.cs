using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using System.IO;

namespace BLL
{
    public class tabWiadomosci
    {
        const string targetList = "Wiadomości";

        public static void AddNew(SPWeb web, SPListItem item, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem newItem = list.AddItem();
            newItem["Title"] = temat;
            if (string.IsNullOrEmpty(nadawca)) nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");

            newItem["colNadawca"] = nadawca;
            newItem["colOdbiorca"] = odbiorca;
            newItem["colKopiaDla"] = kopiaDla;
            newItem["colTresc"] = tresc;
            newItem["colTrescHTML"] = trescHTML;
            if (!string.IsNullOrEmpty(planowanaDataNadania.ToString()) && planowanaDataNadania != new DateTime())
            {
                newItem["colPlanowanaDataNadania"] = planowanaDataNadania.ToString();
            }
            newItem["colKopiaDoNadawcy"] = KopiaDoNadawcy;
            newItem["colKopiaDoBiura"] = KopiaDoBiura;
            if (zadanieId > 0) newItem["_ZadanieId"] = zadanieId;

            if (klientId > 0) newItem["selKlient_NazwaSkrocona"] = klientId;


            //newItem.SystemUpdate();

            //obsługa wysyłki załączników jeżeli Item został przekazany w wywołaniu procedury
            if (item != null)
            {
                for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
                {
                    string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
                    SPFile file = item.ParentList.ParentWeb.GetFile(url);

                    if (file.Exists)
                    {
                        int bufferSize = 20480;
                        byte[] byteBuffer = new byte[bufferSize];
                        //byteBuffer = File.ReadAllBytes(pdfFilePath);
                        byteBuffer = file.OpenBinary();
                        //string targetUrl = newItem.Attachments.UrlPrefix + file.Name;
                        newItem.Attachments.Add(file.Name, byteBuffer);
                    }
                }
            }

            newItem.SystemUpdate();
        }

        /// <summary>
        /// tworzy zlecenie wysyłki wiadomości bez załączników (nie przekazuje item)
        /// </summary>
        public static void AddNew(SPWeb web, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        {
            AddNew(web, null, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, zadanieId, klientId);
        }

        public static void Create_PrzypomnienieOTerminiePlatnosci(SPListItem item, DateTime terminPlatnosci, string subject, string body)
        {
            //ustaw datę powiadomienia
            int reminderDateOffset = -1 * int.Parse(BLL.admSetup.GetValue(item.Web, "REMINDER_DATE_OFFSET"));
            if (reminderDateOffset >= 0) reminderDateOffset = -1;
            DateTime reminderDate = terminPlatnosci.AddDays(reminderDateOffset);

            //ustaw godzinę wysyłki powiadomienia
            TimeSpan ts = new TimeSpan(0, 8, 15);
            string reminderTime = BLL.admSetup.GetValue(item.Web, "REMINDER_TIME");
            if (reminderTime.Length == 5) TimeSpan.TryParse(reminderTime, out ts);
            reminderDate = new DateTime(reminderDate.Year, reminderDate.Month, reminderDate.Day, ts.Hours, ts.Minutes, ts.Seconds);

            string bodyHtml = string.Empty;
            string tmp = string.Empty;
            if(!string.IsNullOrEmpty(body))
            {
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY", out tmp, out bodyHtml);
                bodyHtml = bodyHtml.Replace("___BODY___", body.ToString());
            }

            AddNew(item, reminderDate, subject, bodyHtml);
        }

        private static void AddNew(SPListItem item, DateTime reminderDate, string subject, string bodyHtml)
        {
            int klientId = Get_KlientId(item);
            string nadawca = string.Empty;
            string odbiorca = Get_String(item, "colEmail");
            AddNew(item.Web, nadawca, odbiorca, string.Empty, false, false, subject, string.Empty, bodyHtml, reminderDate, item.ID, klientId);
        }

        private static string Get_String(SPListItem item, string col)
        {
            return item[col] != null ? item[col].ToString() : string.Empty;
        }

        #region Helpers
        private static int Get_KlientId(SPListItem item)
        {
            string col = "selKlient";
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }
        #endregion
    }
}
