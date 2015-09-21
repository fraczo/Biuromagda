using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL.Models;
using System.IO;

namespace BLL
{
    public class tabZadania
    {
        const string lstZadania = "Zadania"; // "tabZadania";

        //public static string Define_KEY(SPItemEventDataCollection item)
        //{
        //    string result;
        //    string ct = item["ContentType"].ToString();

        //    if (ct == "Zadanie" || ct == "Element" || ct == "Folder")
        //    {
        //        return String.Empty;
        //    }

        //    int klientId = 0;
        //    int okresId = 0;

        //    if (item["selKlient"] != null)
        //    {
        //        klientId = new SPFieldLookupValue(item["selKlient"].ToString()).LookupId;
        //    }

        //    if (item["selOkres"] != null)
        //    {
        //        okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;
        //    }

        //    result = String.Format(@"{0}:{1}:{2}",
        //        ct.ToString(),
        //        klientId.ToString(),
        //        okresId.ToString());

        //    return result;
        //}
        public static string Define_KEY(string ct, int klientId, int okresId)
        {
            string result;

            if (ct == "Zadanie" || ct == "Element" || ct == "Folder")
            {
                return String.Empty;
            }

            result = String.Format(@"{0}:{1}:{2}",
                ct.ToString(),
                klientId.ToString(),
                okresId.ToString());

            return result;
        }

        public static string Define_KEY(SPListItem item)
        {
            string ct = item["ContentType"].ToString();

            if (ct == "Zadanie" || ct == "Element" || ct == "Folder")
            {
                return String.Empty;
            }

            int klientId = 0;
            int okresId = 0;

            if (item["selKlient"] != null)
            {
                klientId = new SPFieldLookupValue(item["selKlient"].ToString()).LookupId;
            }

            if (item["selOkres"] != null)
            {
                okresId = new SPFieldLookupValue(item["selOkres"].ToString()).LookupId;
            }

            return Define_KEY(ct, klientId, okresId);
        }

        public static void Update_KEY(SPListItem item, string key)
        {

            string ct = item["ContentType"].ToString();

            if (item["KEY"] != null)
            {
                if (item["KEY"].ToString() != key)
                {
                    item["KEY"] = key;
                    item.Update();
                }
            }
            else
            {
                item["KEY"] = key;
                item.Update();
            }

            return;
        }

        /// <summary>
        /// zwraca identyfikator rekordu w tabZadania, który zawiera szukan klucz.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="web"></param>
        /// <returns></returns>
        public static bool Check_KEY_IsAllowed(string key, SPWeb web, int currentId)
        {
            bool result = true;

            var targetList = web.Lists.TryGetList(lstZadania);

            //if (targetList != null)
            //{
            Array li = targetList.Items.Cast<SPListItem>()
                    .Where(i => i.ID != currentId)
                    .Where(i => i["ContentType"].ToString() != "Zadanie" && i["ContentType"].ToString() != "Element" && i["ContentType"].ToString() != "Folder")
                    .Where(i => i["KEY"].ToString() == key)
                    .ToArray();


            if (li.Length > 0)
            {
                result = false;
            }
            //}

            return result;
        }

        public static void Create_ctVAT_Form(SPWeb web, string ct, int klientId, int okresId, string key, DateTime terminPlatnosci, DateTime terminPrzekazania, bool isKwartalnie)
        {
            Klient iok = new Klient(web, klientId);

            if (iok.FormaOpodatkowaniaVAT == "Nie podlega")
            {
                return; //nie generuj formatki
            }

            SPList list = web.Lists.TryGetList(lstZadania);

            if (list != null)
            {

                SPListItem item = list.AddItem();
                item["ContentType"] = ct;
                item["selKlient"] = klientId;
                item["selOkres"] = okresId;
                item["KEY"] = key;

                //procedura

                string procName = ": " + ct.ToString();
                var procId = tabProcedury.GetID(web, procName, true);

                item["selProcedura"] = procId;

                //numery kont i nazwa urzędu

                KontaKlienta k = new KontaKlienta(web, klientId);
                item["colVAT_Konto"] = k.KontoVAT;
                item["selUrzadSkarbowy"] = k.IdUrzeduSkarbowego;

                //terminy
                item["colVAT_TerminPlatnosciPodatku"] = terminPlatnosci;
                item["colVAT_TerminPrzekazaniaWynikow"] = terminPrzekazania;

                //flagi

                Flagi fl = new Flagi(web, klientId);

                item["colPrzypomnienieOTerminiePlatnos"] = fl.PrzypomnienieOTerminiePlatnosci;
                item["colDrukWplaty"] = fl.GenerowanieDrukuWplaty;
                item["colAudytDanych"] = fl.AudytDanych;

                //rozliczenie
                if (isKwartalnie)
                {
                    item["enumRozliczenieVAT"] = "Kwartalnie";
                }
                else
                {
                    item["enumRozliczenieVAT"] = "Miesięcznie";
                }

                //termin realizacji

                item["colTerminRealizacji"] = terminPrzekazania;

                item["colFormaOpodatkowaniaVAT"] = iok.FormaOpodatkowaniaVAT;
                item["colOsobaDoKontaktu"] = iok.OsobaDoKontaktu;
                item["colTelefon"] = iok.Telefon;
                item["colEmail"] = iok.Email;
                item["colAdres"] = iok.Adres;
                item["colKodPocztowy"] = iok.KodPocztowy;
                item["colMiejscowosc"] = iok.Miejscowosc;

                item["Title"] = procName;

                item.Update();
            }

        }

        public static void Create_ctPD_Form(SPWeb web, string ct, int klientId, int okresId, string key, DateTime terminPlatnosci, DateTime terminPrzekazania, bool isKwartalnie)
        {
            Klient iok = new Klient(web, klientId);

            SPList list = web.Lists.TryGetList(lstZadania);

            //if (list != null)
            //{
            SPListItem item = list.AddItem();
            item["ContentType"] = ct;
            item["selKlient"] = klientId;
            item["selOkres"] = okresId;
            item["KEY"] = key;
            //procedura

            string procName = ": " + ct.ToString();
            var procId = tabProcedury.GetID(web, procName, true);

            item["selProcedura"] = procId;

            //numery kont i nazwa urzędu

            item["colPD_Konto"] = iok.NumerRachunkuPD;
            item["selUrzadSkarbowy"] = iok.UrzadSkarbowyId;

            //terminy
            item["colPD_TerminPlatnosciPodatku"] = terminPlatnosci;
            item["colPD_TerminPrzekazaniaWynikow"] = terminPrzekazania;

            //flagi

            Flagi fl = new Flagi(web, klientId);

            item["colPrzypomnienieOTerminiePlatnos"] = fl.PrzypomnienieOTerminiePlatnosci;
            item["colDrukWplaty"] = fl.GenerowanieDrukuWplaty;
            item["colAudytDanych"] = fl.AudytDanych;

            //rozliczenie
            if (isKwartalnie)
            {
                item["enumRozliczeniePD"] = "Kwartalnie";
            }
            else
            {
                item["enumRozliczeniePD"] = "Miesięcznie";
            }

            //termin realizacji

            item["colTerminRealizacji"] = terminPrzekazania;

            item["colFormaOpodatkowaniaPD"] = iok.FormaOpodatkowaniaPD;
            item["colOsobaDoKontaktu"] = iok.OsobaDoKontaktu;
            item["colTelefon"] = iok.Telefon;
            item["colEmail"] = iok.Email;
            item["colAdres"] = iok.Adres;
            item["colKodPocztowy"] = iok.KodPocztowy;
            item["colMiejscowosc"] = iok.Miejscowosc;

            item["Title"] = procName;

            item.Update();
            //}
        }

        public static void Create_Form(SPWeb web, string ct, int klientId, int okresId, string key, int operatorId)
        {
            Klient iok = new Klient(web, klientId);

            if (operatorId == 0)
            {
                operatorId = dicOperatorzy.GetID(web, string.Empty, true);
            }

            SPList list = web.Lists.TryGetList(lstZadania);

            //if (list != null)
            //{
            string procName = ": " + ct.ToString();
            var procId = tabProcedury.GetID(web, procName, true);

            SPListItem item = list.AddItem();
            item["ContentType"] = ct;
            item["selKlient"] = klientId;
            item["selOkres"] = okresId;
            item["KEY"] = key;
            item["selProcedura"] = procId;
            item["selOperator"] = operatorId;

            item["colOsobaDoKontaktu"] = iok.OsobaDoKontaktu;
            item["colTelefon"] = iok.Telefon;
            item["colEmail"] = iok.Email;

            item["Title"] = procName;

            item.Update();
            //}
        }

        public static void Create_ctZUS_Form(SPWeb web, string ct, int klientId, int okresId, string key, bool isTylkoZdrowotna, bool isChorobowa, bool isPracownicy, double skladkaSP, double skladkaZD, double skladkaFP, DateTime terminPlatnosci, DateTime terminPrzekazania, string zus_sp_konto, string zus_zd_konto, string zus_fp_konto, Klient iok)
        {
            SPList list = web.Lists.TryGetList(lstZadania);

            //if (list != null)
            //{
            string procName = ": " + ct.ToString();
            var procId = tabProcedury.GetID(web, procName, true);



            Flagi fl = new Flagi(web, klientId);

            SPListItem item = list.AddItem();
            item["ContentType"] = ct;
            item["selKlient"] = klientId;
            item["selOkres"] = okresId;
            item["KEY"] = key;
            item["selProcedura"] = procId;

            if (isTylkoZdrowotna)
            {
                item["colZUS_Opcja"] = "Tylko zdrowotna";
            }
            else
            {
                if (isChorobowa)
                {
                    item["colZUS_Opcja"] = "Z chorobowym";
                }
                else
                {
                    item["colZUS_Opcja"] = "Bez chorobowego";
                }
            }

            //jeżeli ZUS-PRAC to nie wypełniaj wysokości składek
            if (hasKlientMaAktywnySerwis(item, "ZUS-PRAC"))
            {
                item["colZatrudniaPracownikow"] = true;
            }
            else
            {
                item["colZUS_SP_Skladka"] = skladkaSP;
                item["colZUS_ZD_Skladka"] = skladkaZD;
                item["colZUS_FP_Skladka"] = skladkaFP;
            }

            item["colZUS_TerminPlatnosciSkladek"] = terminPlatnosci;

            KontaZUS konta = admSetup.GetKontaZUS(web);

            item["colZUS_SP_Konto"] = konta.KontoSP;
            item["colZUS_ZD_Konto"] = konta.KontoZD;
            item["colZUS_FP_Konto"] = konta.KontoFP;
            item["colZUS_TerminPrzekazaniaWynikow"] = terminPrzekazania;

            //flagi
            item["colPrzypomnienieOTerminiePlatnos"] = fl.PrzypomnienieOTerminiePlatnosci;
            item["colDrukWplaty"] = fl.GenerowanieDrukuWplaty;
            item["colAudytDanych"] = fl.AudytDanych;

            //uwagi kadrowe
            item["colUwagiKadrowe"] = iok.UwagiKadrowe;

            //termin realizacji
            item["colTerminRealizacji"] = item["colZUS_TerminPrzekazaniaWynikow"];

            item["colFormaOpodakowania_ZUS"] = iok.FormaOpodatkowaniaZUS;
            item["selOddzialZUS"] = iok.OddzialZUSId;
            item["colOsobaDoKontaktu"] = iok.OsobaDoKontaktu;
            item["colTelefon"] = iok.Telefon;
            item["colEmail"] = iok.Email;
            item["colAdres"] = iok.Adres;
            item["colKodPocztowy"] = iok.KodPocztowy;
            item["colMiejscowosc"] = iok.Miejscowosc;

            item["Title"] = procName;

            item.Update();
            //}
        }

        public static void Create_ctBR_Form(SPWeb web, string ct, int klientId, int okresId, string key)
        {
            SPList list = web.Lists.TryGetList(lstZadania);

            //if (list != null)
            //{

            SPListItem item = list.AddItem();
            item["ContentType"] = ct;
            item["selKlient"] = klientId;
            item["selOkres"] = okresId;
            item["KEY"] = key;

            //procedura

            string procName = ": " + ct.ToString();
            var procId = tabProcedury.GetID(web, procName, true);

            item["selProcedura"] = procId;

            //numer konta biura

            BiuroRachunkowe br = new BiuroRachunkowe(web, okresId);
            item["colBR_Konto"] = br.Konto;
            if (br.TerminPrzekazania > new DateTime())
            {
                item["colBR_TerminPrzekazania"] = br.TerminPrzekazania;
                item["colTerminRealizacji"] = br.TerminPrzekazania;
            }

            //flagi

            Flagi fl = new Flagi(web, klientId);

            item["colPrzypomnienieOTerminiePlatnos"] = fl.PrzypomnienieOTerminiePlatnosci;
            item["colDrukWplaty"] = fl.GenerowanieDrukuWplaty;

            item["Title"] = procName;

            item.Update();
            //}
        }

        public static int Get_NumerZadaniaBR(SPWeb web, int klientId, int okresId)
        {
            int result = 0;

            SPList list = web.Lists.TryGetList(lstZadania);
            //if (list!=null)
            //{
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i["ContentType"].ToString() == @"Rozliczenie z biurem rachunkowym")
                .Where(i => new SPFieldLookupValue(i["selKlient"].ToString()).LookupId == klientId)
                .Where(i => new SPFieldLookupValue(i["selOkres"].ToString()).LookupId == okresId)
                .SingleOrDefault();

            if (item != null)
            {
                result = item.ID;
            }
            //}

            return result;
        }


        public static bool Add_FileFromURL(SPWeb web, int zadanieId, SPFile file)
        {
            bool result = false;
            string srcUrl = file.ServerRelativeUrl;

            SPList list = web.Lists.TryGetList(lstZadania);


            SPListItem item = list.GetItemById(zadanieId);

            if (item != null)
            {
                try
                {
                    srcUrl = web.Url + "/" + file.Url;

                    SPFile attachmentFile = web.GetFile(srcUrl);

                    //item.Attachments.Add(attachmentFile.Name, attachmentFile.OpenBinaryStream();


                    //FileStream fs = new FileStream(srcUrl, FileMode.Open, FileAccess.Read);

                    Stream fs = attachmentFile.OpenBinaryStream();

                    // Create a byte array of file stream length
                    byte[] buffer = new byte[fs.Length];

                    //Read block of bytes from stream into the byte array
                    fs.Read(buffer, 0, System.Convert.ToInt32(fs.Length));

                    //Close the File Stream
                    fs.Close();

                    item.Attachments.AddNow(attachmentFile.Name, buffer);

                    //aktualizuj informacje o załączonej fakturze
                    item["colBR_FakturaZalaczona"] = true;

                    item.Update();

                }
                catch (Exception)
                {
                    //zabezpieczenie przed zdublowaniem plików
                }

            }

            return result;
        }

        public static void Update_InformacjeOWystawionejFakturze(SPWeb web, int zadanieId, string numerFaktury, double wartoscDoZaplaty, DateTime terminPlatnosci)
        {
            SPList list = web.Lists.TryGetList(lstZadania);
            //if (list != null)
            //{
            SPListItem item = list.GetItemById(zadanieId);
            if (item != null)
            {
                item["colBR_NumerFaktury"] = numerFaktury;
                item["colBR_WartoscDoZaplaty"] = wartoscDoZaplaty;
                item["colBR_TerminPlatnosci"] = terminPlatnosci;
                item.Update();
            }
            //}
        }


        /// <summary>
        /// Aktualizuje informacje o wysyłce wyników do klienta
        /// Procedura wywoływana w procesu obsługi wiadomości po poprawnie zakończonej wysyłce
        /// </summary>
        public static void Update_StatusWysylki(SPWeb web, SPListItem messageItem, int zadanieId, StatusZadania statusZadania)
        {
            SPList list = web.Lists.TryGetList(lstZadania);
            SPListItem item = list.GetItemById(zadanieId);
            if (item != null)
            {
                string status = item["enumStatusZadania"] != null ? item["enumStatusZadania"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(status)
                    && status == BLL.Models.StatusZadania.Wysyłka.ToString())
                {
                    //aktualizuj status i dodaj komentarz
                    item["enumStatusZadania"] = statusZadania.ToString();
                    string uwagi = item["colUwagi"] != null ? item["colUwagi"].ToString() : string.Empty;
                    uwagi = string.Format("{0} \n{1}",
                        uwagi,
                        messageItem.Title + " wysłane " + messageItem["Modified"].ToString() + " #" + messageItem.ID.ToString()).Trim();
                    item["colUwagi"] = uwagi;
                    item.Update();
                }
            }

        }

        private static bool hasKlientMaAktywnySerwis(SPListItem item, string serviceName)
        {
            int klientId;

            if (item["selKlient"]!=null) klientId = new SPFieldLookupValue(item["selKlient"].ToString()).LookupId;
            else klientId = 0;

            if (klientId > 0)
	        {
		         if (BLL.tabKlienci.HasServiceAssigned(item.Web, klientId, serviceName)) return true;
	        }

            return false;
        }


        public static void Complte_PrzypomnienieOWysylceDokumentow(SPListItem item, int klientId, int okresId)
        {
            string KEY = Define_KEY("Prośba o dokumenty",klientId, okresId);
            if (!string.IsNullOrEmpty(KEY))
            {
                int taskId = Get_ZadanieByKEY(item.Web, KEY);
                if (taskId>0)
                {
                    Set_Status(BLL.tabZadania.Get_ZadanieById(item.Web,taskId), "Zakończone");
                }
            }
        }

        public static SPListItem Get_ZadanieById(SPWeb web, int taskId)
        {
            SPList list = web.Lists.TryGetList(lstZadania);
            return list.GetItemById(taskId);
        }

        private static string Define_KEY(SPListItem item, string p)
        {
            throw new NotImplementedException();
        }

        private static void Set_Status(SPListItem item, string s)
        {
            string status = item["enumStatusZadania"]!=null?item["enumStatusZadania"].ToString():string.Empty;
            if (status!=s)
            {
                item["enumStatusZadania"] = s;
                item.SystemUpdate();
            }
        }

        private static int Get_ZadanieByKEY(SPWeb web, string KEY)
        {
            SPList list = web.Lists.TryGetList(lstZadania);
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i["KEY"].ToString() == KEY)
                .FirstOrDefault();
            return item!=null?item.ID:0;
        }


        public static void Update_PD_DataWysylki(SPListItem task, DateTime date)
        {
            SPListItem item = Get_ZadanieById(task.Web, task.ID);
            item["colPD_DataWylaniaInformacji"] = date;
            item.SystemUpdate();
        }

        public static void Update_VAT_DataWysylki(SPListItem task, DateTime date)
        {
            SPListItem item = Get_ZadanieById(task.Web, task.ID);
            item["colVAT_DataWyslaniaInformacji"] = date;
            item.SystemUpdate();
        }

        public static void Update_ZUS_DataWysylki(SPListItem task, DateTime date)
        {
            SPListItem item = Get_ZadanieById(task.Web, task.ID);
            item["colZUS_DataWyslaniaInformacji"] = date;
            item.SystemUpdate();
        }
    }
}
