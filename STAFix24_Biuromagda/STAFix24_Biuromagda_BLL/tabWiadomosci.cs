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

        public static void AddNew(SPWeb web, SPListItem item, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId)
        {

            SPList list = web.Lists.TryGetList(targetList);
            SPListItem newItem = list.AddItem();
            newItem["Title"] = temat;
            if (string.IsNullOrEmpty(nadawca))
            {
                nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");
            }
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
            if (zadanieId > 0)
            {
                newItem["_ZadanieId"] = zadanieId;
            }

            newItem.Update();

            // TODO: obsługa wysyłki załączników

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


            newItem.Update();
        }
    }
}
