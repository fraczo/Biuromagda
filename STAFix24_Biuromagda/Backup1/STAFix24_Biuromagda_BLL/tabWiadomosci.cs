﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace BLL
{
    public class tabWiadomosci
    {
        const string targetList = "Wiadomości";
        private static string targetFileNameLeading = "DRUK WPŁATY__";

        public static void AddNew_NoAtt(SPListItem item, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        {
            AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, zadanieId, klientId, BLL.Models.Marker.NoAttachements);
        }

        //public static void AddNew(SPWeb web, SPListItem item, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        //{
        //    AddNew(web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, BLL.Models.Marker.Ignore);
        //}

        /// <summary>
        /// tworzy zlecenie wysyłki wiadomości bez załączników (nie przekazuje item)
        /// </summary>
        //public static void AddNew(SPWeb web, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        //{
        //    AddNew(web, null, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, zadanieId, klientId);
        //}

        //public static void AddNew(SPListItem item, bool hasAttachements, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId)
        //{
        //    AddNew(item.Web, null, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, zadanieId, klientId);
        //}

        //private static void AddNew(SPListItem item, DateTime reminderDate, string subject, string bodyHtml)
        //{
        //    int klientId = Get_KlientId(item);
        //    string nadawca = string.Empty;
        //    string odbiorca = Get_String(item, "colEmail");
        //    AddNew(item.Web, nadawca, odbiorca, string.Empty, false, false, subject, string.Empty, bodyHtml, reminderDate, item.ID, klientId);
        //}


        public static void AddNew(SPWeb web, SPListItem item, string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId, BLL.Models.Marker marker)
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
                        //sprawdź markety i dodawaj tylko odpowiednie pliki
                        switch (marker)
                        {
                            case BLL.Models.Marker.ReminderZUS:
                                if (file.Name.StartsWith("DRUK WPŁATY__ZUS")
                                    || file.Name.StartsWith("DRUK WPŁATY__Składka zdrowotna"))
                                    Copy_Attachement(newItem, file);
                                break;
                            case BLL.Models.Marker.ReminderZUS_PIT:
                                if (file.Name.StartsWith("DRUK WPŁATY__PIT"))
                                    Copy_Attachement(newItem, file);
                                break;
                            case BLL.Models.Marker.NoAttachements:
                                break;
                            default:
                                Copy_Attachement(newItem, file);
                                break;
                        }
                    }
                }
            }

            newItem.SystemUpdate();
        }



        private static void Copy_Attachement(SPListItem newItem, SPFile file)
        {
            int bufferSize = 20480;
            byte[] byteBuffer = new byte[bufferSize];
            byteBuffer = file.OpenBinary();
            newItem.Attachments.Add(file.Name, byteBuffer);
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



        public static void CreateMailMessage(SPListItem item)
        {
            string cmd = BLL.Tools.Get_Text(item, "cmdFormatka_Wiadomosc");
            if (!string.IsNullOrEmpty(cmd))
            {
                int klientId;

                switch (item.ContentType.Name)
                {
                    case "Wiadomość z ręki":
                        klientId = BLL.Tools.Get_LookupId(item, "selKlient");
                        CreateMailMessage_WiadomoscZReki(item, klientId);
                        break;
                    case "Wiadomość z szablonu":
                        klientId = BLL.Tools.Get_LookupId(item, "selKlient");
                        CreateMailMessage_WiadomoscZSzablonu(item, klientId);
                        break;
                    case "Wiadomość grupowa":
                        CreateMailMessage_WiadomoscDoGrupy(item);
                        break;
                    case "Wiadomość grupowa z szablonu":
                        CreateMailMessage_WiadomoscDoGrupyZSzablonu(item);
                        break;
                    default:
                        break;
                }

                BLL.Tools.Set_Text(item, "cmdFormatka_Wiadomosc", string.Empty);
                item.SystemUpdate();
            }
        }

        private static void CreateMailMessage_Wiadomosc(SPListItem item, int klientId, string subject, string bodyHTML)
        {
#if DEBUG
            Logger.LogEvent("CreateMailMessage_Wiadomosc " + item.ContentType.Name + " z:" + item.ID.ToString() + " k:" + klientId.ToString(), string.Empty);
#endif
            string cmd = BLL.Tools.Get_Text(item, "cmdFormatka_Wiadomosc");

            if (!string.IsNullOrEmpty(cmd))
            {


                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = false;

                string nadawca = BLL.Tools.Get_CurrentUser(item);

                if (cmd == "Wyślij z kopią do mnie") KopiaDoNadawcy = true;

                // przygotuj wiadomość
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY.Include", out temat, out trescHTML, nadawca);
                temat = subject;
                trescHTML = trescHTML.Replace("___BODY___", bodyHTML);

                switch (cmd)
                {
                    case "Wyślij":
                    case "Wyślij z kopią do mnie":

                        string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, klientId);
                        if (BLL.Tools.Is_ValidEmail(odbiorca))
                        {
                            BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, BLL.Tools.Get_Date(item, "colPlanowanaDataNadania"), item.ID, klientId, BLL.Models.Marker.WithAttachements);
                            BLL.Tools.Set_Text(item, "enumStatusZadania", "Wysyłka");
                            item.SystemUpdate();
                        }
                        break;
                    case "Wyślij wiadomość testową":

                        temat = string.Format(@"::TEST::{0}", temat.ToString());
                        kopiaDla = string.Empty;
                        KopiaDoNadawcy = false;
                        KopiaDoBiura = false;

                        odbiorca = BLL.Tools.Get_CurrentUser(item);
                        if (BLL.Tools.Is_ValidEmail(odbiorca))
                        {
                            BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, new DateTime(), 0, 0, Models.Marker.WithAttachements);
                        }
                        break;
                    default:
                        break;
                }
            }
        }



        private static void CreateMailMessage_WiadomoscZReki(SPListItem item, int klientId)
        {
#if DEBUG
            Logger.LogEvent("CreateMailMessage_WiadomoscZReki", item.ID.ToString());
#endif
            string bodyHTML = BLL.Tools.Get_Text(item, "colTresc");
            //string subject = BLL.Tools.Get_Text(item, "colTematWiadomosci");
            string subject = item.Title;
            CreateMailMessage_Wiadomosc(item, klientId, subject, bodyHTML);
        }

        private static void CreateMailMessage_WiadomoscZSzablonu(SPListItem item, int klientId)
        {
#if DEBUG
            Logger.LogEvent("CreateMailMessage_WiadomoscZSzablonu", item.ID.ToString());
#endif
            int szablonId = BLL.Tools.Get_LookupId(item, "selSzablonWiadomosci");
            string bodyHTML = BLL.Tools.Get_Text(item, "colInformacjaDlaKlienta");
            string subject = string.Empty;
            BLL.tabSzablonyWiadomosci.GetSzablonId(item.Web, szablonId, out subject, ref bodyHTML);

            CreateMailMessage_Wiadomosc(item, klientId, subject, bodyHTML);
        }

        private static void CreateMailMessage_WiadomoscDoGrupy(SPListItem item)
        {
#if DEBUG
            Logger.LogEvent("CreateMailMessage_WiadomoscDoGrupy", item.ID.ToString());
#endif
            string cmd = BLL.Tools.Get_Text(item, "cmdFormatka_Wiadomosc");

            if (!string.IsNullOrEmpty(cmd) && cmd == "Wyślij wiadomość testową")
            {
                CreateMailMessage_WiadomoscZReki(item, 0);
            }
            else
            {
                Array klientListItems = BLL.tabKlienci.Get_WybraniKlienci(item);

                //obsługa duplikatów
                if (BLL.Tools.Get_Flag(item,"colUsunDuplikaty"))
                {
                    klientListItems = Remove_DuplicatedEmails(klientListItems);
                }

                foreach (SPListItem klientItem in klientListItems)
                {
                    CreateMailMessage_WiadomoscZReki(item, klientItem.ID);
                }
            }
        }

        private static void CreateMailMessage_WiadomoscDoGrupyZSzablonu(SPListItem item)
        {
#if DEBUG
            Logger.LogEvent("CreateMailMessage_WiadomoscDoGrupyZSzablonu", item.ID.ToString());
#endif

            string cmd = BLL.Tools.Get_Text(item, "cmdFormatka_Wiadomosc");

            if (!string.IsNullOrEmpty(cmd) && cmd == "Wyślij wiadomość testową")
            {
                CreateMailMessage_WiadomoscZSzablonu(item, 0);
            }
            else
            {
                Array klientListItems = BLL.tabKlienci.Get_WybraniKlienci(item);

                //obsługa duplikatów
                if (BLL.Tools.Get_Flag(item, "colUsunDuplikaty"))
                {
                    klientListItems = Remove_DuplicatedEmails(klientListItems);
                }

                foreach (SPListItem klientItem in klientListItems)
                {
                    int klientId = BLL.Tools.Get_LookupId(item, "selKlient");

                    CreateMailMessage_WiadomoscZSzablonu(item, klientItem.ID);

                }
            }
        }

        public static void Ensure_ColumnExist(SPWeb web, string col)
        {
            SPListItem item = web.Lists.TryGetList(targetList).Items.Add();
            BLL.Tools.Ensure_Column(item, col);
        }




        public static Array Select_Batch(SPWeb web)
        {
            Debug.WriteLine("BLL.tabWiadomosci.Select_Batch");

            SPList list = web.Lists.TryGetList(targetList);

            return list.Items.Cast<SPListItem>()
                .Where(i => (bool)i["colCzyWyslana"] != true)
                .Where(i => i["colPlanowanaDataNadania"] == null
                    || (i["colPlanowanaDataNadania"] != null
                       && (DateTime)i["colPlanowanaDataNadania"] <= DateTime.Now))
                .ToArray();
        }

        /// <summary>
        /// zwraca listę wiadomości nie modyfikowanych w ciągu ostatniego miesiąca, zawierających załączniki
        /// </summary>
        public static Array Get_GotoweDoArchiwizacji(SPWeb web)
        {
            return web.Lists.TryGetList(targetList).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_Date(i, "Modified") <= DateTime.Now.AddMonths(-1)
                            && i.Attachments.Count>0)
                .ToArray();

        }

        private static Array Remove_DuplicatedEmails(Array klienci)
        {
            ArrayList results = new ArrayList();
            foreach (SPListItem k in klienci)
            {
                bool isFound = false;

                string email = BLL.Tools.Get_Email(k, "colEmail");
                if (!string.IsNullOrEmpty(email))
                {
                    foreach (SPListItem item in results)
                    {
                        string email1 = BLL.Tools.Get_Email(item, "colEmail");
                        if (!string.IsNullOrEmpty(email1) && email1.Equals(email))
                        {
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound) results.Add(k);
                }
            }

            return results.ToArray();
        }

        public static void AddNew_FakturaDoZaplaty(SPWeb web, BLL.Models.FakturaDoZaplaty faktura, BLL.Models.BiuroRachunkowe biuroRachunkowe, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, string attachementUrl, bool drukWplatyWymagany, DateTime planowanaDataNadania, int klientId)
        {
            Debug.WriteLine("AddNew_FakturaDoZaplaty");

            SPList list = web.Lists.TryGetList(targetList);
            SPListItem newItem = list.AddItem();
            newItem["Title"] = temat;

            string nadawca = faktura.EmailNadawcy;
            if (string.IsNullOrEmpty(nadawca)) nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");

            newItem["colNadawca"] = nadawca;
            newItem["colOdbiorca"] = faktura.EmailOdbiorcy;
            //newItem["colKopiaDla"] = string.Empty;
            newItem["colTresc"] = tresc;
            newItem["colTrescHTML"] = trescHTML;
            if (!string.IsNullOrEmpty(planowanaDataNadania.ToString()) && planowanaDataNadania != new DateTime())
            {
                newItem["colPlanowanaDataNadania"] = planowanaDataNadania.ToString();
            }
            newItem["colKopiaDoNadawcy"] = KopiaDoNadawcy;
            newItem["colKopiaDoBiura"] = KopiaDoBiura;

            if (klientId > 0) newItem["selKlient_NazwaSkrocona"] = klientId;


            //dodanie obrazu faktury PDF do wiadomości
            if (!string.IsNullOrEmpty(attachementUrl))
            {
                SPFile file = web.GetFile(attachementUrl);
                if (file.Exists) Copy_Attachement(newItem, file);
            }

            newItem.Update();

            //dodanie druku wpłaty do wiadomości
            if (drukWplatyWymagany)
            {
                Debug.WriteLine("DW wymagany");
                string fileName = String.Format(@"{0}do faktury_{1}.pdf", targetFileNameLeading, faktura.NumerFaktury);


                //string odbiorca = admSetup.GetValue(web, "BR_NAZWA");
                string odbiorca = BLL.admSetup.Get_NazwaBiura(web);
                string numerFaktury = faktura.NumerFaktury;
                string tytulem = String.Format("Zapłata za {0}", numerFaktury);

                if (GeneratorDrukow.DrukWplaty.Attach_DrukWplaty(web,
                                                                newItem,
                                                                fileName,
                                                                odbiorca,
                                                                biuroRachunkowe.Konto,
                                                                faktura.WartoscDoZaplaty,
                                                                faktura.NazwaKlienta,
                                                                tytulem))
                {
                    Debug.WriteLine("DW załączony");
                }
                else
                {
                    Debug.WriteLine("ERR: DW nie załączony");
                    ElasticEmail.EmailGenerator.SendMail("ERR: DW nie dołączony do wiadomości: " + newItem.ID, "");
                }
            }

            faktura.Wyslana = true;
        }


        public static SPList Get_List(SPWeb web)
        {
            return web.Lists.TryGetList(targetList);
        }
    }
}
