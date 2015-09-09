﻿using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;
using System.Text;
using BLL.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace tabZadania_EventReceiver.EventReceiver1
{
    public class EventReceiver1 : SPItemEventReceiver
    {

        const string WYSLIJ_INFORMACJE_DO_KLIENTA = "Wyślij informację do Klienta";
        const string ZATWIERDZ = "Zatwierdź";

        const string emptyMarker = "---";

        //szablony do informacji o załącznikach
        const string templateH = @"<table><tr valign='top'><td><div style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #808080'><strong>w załączeniu:</strong></div></td><td><ul>{0}</ul></td></tr></table>";
        const string templateR = @"<li style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small '>{0}</li>";

        #region EventHandlers

        public override void ItemAdding(SPItemEventProperties properties)
        {
            Validate(properties);
        }

        public override void ItemUpdating(SPItemEventProperties properties)
        {
            Validate(properties);
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        #endregion

        private void Execute(SPItemEventProperties properties)
        {
            try
            {
                this.EventFiringEnabled = false;

                SPListItem item = properties.ListItem;
                SPWeb web = item.Web;

                string ct = item.ContentType.Name;
                switch (ct)
                {
                    case "Prośba o dokumenty":
                    case "Prośba o przesłanie wyciągu bankowego":
                        Update_KEY(item);
                        break;
                    case "Rozliczenie ZUS":
                    case "Rozliczenie podatku dochodowego":
                    case "Rozliczenie podatku dochodowego spółki":
                    case "Rozliczenie podatku VAT":
                    case "Rozliczenie z biurem rachunkowym":
                        Update_KEY(item);
                        Update_GBW(web, item, ct);
                        break;
                    case "Zadanie":
                        Update_Zadanie(item, web);
                        break;
                    default:
                        break;
                }

                Manage_CT(item);

                //aktualizacja tytułu rekordu
                Update_Title(item);

                //aktualizacja pola user (_)
                Update_OperatorUser(item, web);

            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
#endif

            }
            finally
            {
                this.EventFiringEnabled = true;
            }

        }

        #region Updates

        /// <summary>
        /// Jeżeli operator jest przypisany to w zadaniu aktualizuje pole _KontoOperatora, które przechowuje jego login
        /// dla celów filtrowania zadań w/g bieżącego użytkownika.
        /// </summary>
        private static void Update_OperatorUser(SPListItem item, SPWeb web)
        {
            if (item["selOperator"] != null)
            {
                int operatorId = new SPFieldLookupValue(item["selOperator"].ToString()).LookupId;

                int userId = BLL.dicOperatorzy.Get_UserIdById(web, operatorId);

                if (item["_KontoOperatora"] == null)
                {
                    item["_KontoOperatora"] = userId;
                    item.Update();
                }
                else
                {
                    if (new SPFieldUserValue(web, item["_KontoOperatora"].ToString()).LookupId != userId)
                    {
                        item["_KontoOperatora"] = userId;
                        item.Update();
                    }
                }
            }
            else
            {
                if (item["_KontoOperatora"] != null)
                {
                    item["_KontoOperatora"] = 0;
                    item.Update();
                }

            }
        }

        private static void Update_Title(SPListItem item)
        {
            if (String.IsNullOrEmpty(item.Title))
            {
                item["Title"] = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue : "#" + item.ID.ToString();
                item.Update();
            }
        }

        private static void Update_Zadanie(SPListItem item, SPWeb web)
        {
            //przypisz procedurę na podstawie tematu
            int procId = Assign_ProceduraBasedOnTitle(item, web);

            //update termin realizacji
            Assign_TerminRealizacjiBasedOnProcedura(item, web, procId);

            //update operatora
            Assign_OperatorBasedOnProcedura(item, web, procId);
        }

        private bool Update_GBW(SPWeb web, SPListItem item, string ct)
        {
            bool result = false;
            string targetFileNameLeading = "DRUK WPŁATY__";

            UsunPodobneZalaczniki(item, targetFileNameLeading);

            if (item["colDrukWplaty"] != null && (bool)item["colDrukWplaty"])
            {
                string klient = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupValue : string.Empty;
                int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

                string nadawca = Get_Nadawca(web, klient, klientId);

                string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;

                switch (ct)
                {
                    case "Rozliczenie z biurem rachunkowym":
                        result = ObslugaGBW_RozliczenieZBiuremRachunkowym(web, item, result, targetFileNameLeading, klient, okres, nadawca);
                        break;
                    case "Rozliczenie podatku VAT":
                        result = ObslugaGBW_RozliczeniePodatkuVAT(web, item, result, targetFileNameLeading, klient, okres, klientId);
                        break;
                    case "Rozliczenie podatku dochodowego":
                        result = ObslugaGBW_RozliczeniePodatkuDochodowego(web, item, result, targetFileNameLeading, klient, okres, klientId);
                        break;
                    case "Rozliczenie ZUS":
                        result = ObslugaGBW_RozliczenieZUS(web, item, result, targetFileNameLeading, klient, okres, klientId);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private static void Update_KEY(SPListItem item)
        {
            string key = tabZadania.Define_KEY(item);
            tabZadania.Update_KEY(item, key);
        }

        /// <summary>
        ///jeżeli zadanie nie ma przypisanego operatora a ktoś go zaczął edytować to wtedy ta osoba zostanie przypisana
        ///do zadania o ile ma zdefiniowane konto operatora.
        /// </summary>
        /// <param name="item"></param>
        private void Update_PrzypiszOperatora(SPListItem item)
        {
            //sprawdź czy zadanie było edytowane
            DateTime datCreated = item["Created"] != null ? DateTime.Parse(item["Created"].ToString()) : new DateTime();
            DateTime datModified = item["Modified"] != null ? DateTime.Parse(item["Modified"].ToString()) : new DateTime();

            if (datCreated == datModified) return;

            //sprawdź czy przypisany operator
            int operatorId = item["selOperator"] != null ? new SPFieldLookupValue(item["selOperator"].ToString()).LookupId : 0;

            if (operatorId > 0) return;

            //sprawdź czy bieżący operator jest zdefiniowane konto operatora
            SPUser currentUser = item["Editor"] != null ? new SPFieldUserValue(item.Web, item["Editor"].ToString()).User : null;
            int targetOpId = dicOperatorzy.Get_OperatorIdByLoginName(item.Web, currentUser.LoginName);

            if (targetOpId > 0)
            {
                //przypisz operatora do zadania
                item["selOperator"] = targetOpId;
                item.Update();
            }
        }

        #endregion

        #region Obsługa GBW

        private bool ObslugaGBW_RozliczenieZUS(SPWeb web, SPListItem item, bool result, string targetFileNameLeading, string klient, string okres, int klientId)
        {
            bool wymaganyDrukWplaty = item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false;
            string opcja = item["colZUS_Opcja"] != null ? item["colZUS_Opcja"].ToString() : string.Empty;

            if (wymaganyDrukWplaty)
            {
                double kwota;
                string konto;
                string fileName;

                switch (opcja)
                {
                    case "Tylko zdrowotna":
                        //skladka zdrowotna
                        kwota = item["colZUS_ZD_Skladka"] != null ? Double.Parse(item["colZUS_ZD_Skladka"].ToString()) : 0;
                        konto = Clean_NumerRachunku(item, "colZUS_ZD_Konto");
                        fileName = String.Format(@"{0}Składka zdrowotna_{1}.pdf",
                                           targetFileNameLeading,
                                           okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        break;
                    default:

                        bool zatrudniaPracownikow = item["colZatrudniaPracownikow"] != null ? (bool)item["colZatrudniaPracownikow"] == true : false;

                        if (zatrudniaPracownikow)
                        {

                            //PIT 8AR  
                            bool pit8ARZalaczony = item["colZUS_PIT-8AR_Zalaczony"] != null ? (bool)item["colZUS_PIT-8AR_Zalaczony"] == true : false;
                            if (pit8ARZalaczony)
                            {
                                kwota = item["colZUS_PIT-8AR"] != null ? Double.Parse(item["colZUS_PIT-8AR"].ToString()) : 0;
                                if (kwota > 0)
                                {
                                    BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                                    konto = iok.NumerRachunkuPD;
                                    int urzadId = iok.UrzadSkarbowyId;

                                    BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                                    string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();
                                    fileName = String.Format(@"{0}PIT-8AR_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);

                                    string numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2); //TODO: obsługa kwartalnych deklaracji

                                    result = Generuj_DrukPD_FromZUS(web, item, klientId, okres, kwota, konto, fileName, odbiorca, numerDeklaracji, "PIT8AR", "ZRYCZ.POD.DOCH.UM.Z", iok);
                                }
                            }
                            //PIT 4R
                            bool pit4RZalaczony = item["colZUS_PIT-4R_Zalaczony"] != null ? (bool)item["colZUS_PIT-4R_Zalaczony"] == true : false;
                            if (pit4RZalaczony)
                            {
                                kwota = item["colZUS_PIT-4R"] != null ? Double.Parse(item["colZUS_PIT-4R"].ToString()) : 0;
                                if (kwota > 0)
                                {
                                    BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                                    konto = iok.NumerRachunkuPD;
                                    int urzadId = iok.UrzadSkarbowyId;

                                    BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                                    string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();
                                    fileName = String.Format(@"{0}PIT-4R_{1}.pdf",
                                       targetFileNameLeading,
                                       okres);

                                    string numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2); //TODO: obsługa kwartalnych deklaracji

                                    result = Generuj_DrukPD_FromZUS(web, item, klientId, okres, kwota, konto, fileName, odbiorca, numerDeklaracji, "PIT4R", "POD.DOCH.ZA PRAC.", iok);
                                }
                            }

                        }

                        //fundusz pracy
                        kwota = item["colZUS_FP_Skladka"] != null ? Double.Parse(item["colZUS_FP_Skladka"].ToString()) : 0;
                        konto = Clean_NumerRachunku(item, "colZUS_FP_Konto");
                        fileName = String.Format(@"{0}ZUS 53_{1}.pdf",
                                           targetFileNameLeading,
                                           okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);

                        //skladka zdrowotna
                        kwota = item["colZUS_ZD_Skladka"] != null ? Double.Parse(item["colZUS_ZD_Skladka"].ToString()) : 0;
                        konto = Clean_NumerRachunku(item, "colZUS_ZD_Konto");
                        fileName = String.Format(@"{0}ZUS 52_{1}.pdf",
                                            targetFileNameLeading,
                                            okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);

                        //skladka spoleczna
                        kwota = item["colZUS_SP_Skladka"] != null ? Double.Parse(item["colZUS_SP_Skladka"].ToString()) : 0;
                        konto = Clean_NumerRachunku(item, "colZUS_SP_Konto");
                        fileName = String.Format(@"{0}ZUS 51_{1}.pdf",
                                           targetFileNameLeading,
                                           okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);

                        break;
                }
            }

            return result;
        }

        private bool Generuj_DrukPD_FromZUS(SPWeb web, SPListItem item, int klientId, string okres, double kwota, string konto, string fileName, string odbiorca, string numerDeklaracji, string symbolFormularza, string identyfikatorZobowiazania, BLL.Models.Klient iok)
        {
            bool result = false;

            string nadawca = iok.NazwaFirmy + " " + iok.Adres + " " + iok.KodPocztowy + " " + iok.Miejscowosc;
            nadawca = nadawca.ToUpper();
            string nip = iok.NIP;
            string typIdentyfikatora = "N";

            if (konto.Length == 26 && kwota > 0 && !string.IsNullOrEmpty(fileName))
            {
                result = GeneratorDrukow.DrukWplaty.Attach_DrukWplatyPD(web, item,
                fileName,
                odbiorca,
                konto,
                kwota,
                nadawca,
                nip,
                typIdentyfikatora,
                numerDeklaracji, //15M07
                symbolFormularza, //PIT4R
                identyfikatorZobowiazania //POD.DOCH.ZA PRAC.
                );
            }
            return result;
        }

        private static bool Generuj_DrukZUS(SPWeb web, SPListItem item, bool result, string okres, int klientId, double kwota, string konto, string fileName)
        {
            if (konto.Length == 26 && kwota > 0 && !string.IsNullOrEmpty(fileName))
            {

                string typWplaty = "S";
                string numerDeklaracji = "01";
                string okresMiesiac = okres.Substring(5, 2);
                string okresRok = okres.Substring(0, 4);
                string numerDecyzji = ".";


                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                string nadawca = iok.Get_NazwaNadawcyPrzelewu().Trim().ToUpper();
                string regon = iok.Regon;
                string nip = iok.NIP;

                string identyfikatorDeklaracji = String.Format("{0} {1} {2}{3} {4}",
                    typWplaty.Substring(0, 1),
                    numerDeklaracji.Substring(0, 2),
                    okresMiesiac.Substring(0, 2),
                    okresRok.Substring(0, 4),
                    numerDecyzji = numerDecyzji.Length > 15 ? numerDecyzji.Substring(0, 15) : numerDecyzji);

                result = GeneratorDrukow.DrukWplaty.Attach_DrukWplatyZUS(web, item,
                fileName,
                konto,
                kwota,
                nadawca, nip, "R", regon, identyfikatorDeklaracji);
            }
            return result;
        }

        private bool ObslugaGBW_RozliczeniePodatkuDochodowego(SPWeb web, SPListItem item, bool result, string targetFileNameLeading, string klient, string okres, int klientId)
        {
            bool wymaganyDrukWplaty = item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false;
            string ocenaWyniku = item["colPD_OcenaWyniku"] != null ? item["colPD_OcenaWyniku"].ToString() : string.Empty;
            double kwota = item["colPD_WartoscDoZaplaty"] != null ? Double.Parse(item["colPD_WartoscDoZaplaty"].ToString()) : 0;
            string konto = Clean_NumerRachunku(item, "colPD_Konto");

            if (wymaganyDrukWplaty && konto.Length == 26 && kwota > 0 && ocenaWyniku == "Dochód")
            {

                string fileName = String.Format(@"{0}Podatek dochodowy_{1}.pdf",
                    targetFileNameLeading,
                    okres);

                int urzadId = item["selUrzadSkarbowy"] != null ? new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId : 0;
                BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();

                string numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2); // TODO:skorygować typ deklaracji

                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);


                string nadawca = iok.Get_NazwaNadawcyPrzelewu();

                //nip z kartoteki
                string nip = iok.NIP;
                string typIdentyfikatora = "N";

                string symbolFormularza = "PPL";
                string opis = "ZAL.POD.DOCH.";


                result = GeneratorDrukow.DrukWplaty.Attach_DrukWplatyPD(web, item,
                fileName,
                odbiorca,
                konto,
                kwota,
                nadawca,
                nip, typIdentyfikatora, numerDeklaracji, symbolFormularza, opis);
            }
            return result;
        }

        
        /// <summary>
        /// jeżeli numer rachunku ma więcej niż 26 znaków usówa wszystkie znaki nie będące cyfrą
        /// </summary>
        private string Clean_NumerRachunku(SPListItem item, string colName)
        {
            string numerRachunku = item[colName] != null ? item[colName].ToString() : string.Empty;

            if (numerRachunku.Length>26)
            {
                Regex rgx = new Regex("[^0-9]");
                numerRachunku = rgx.Replace(numerRachunku, "");
            }

            return numerRachunku;
        }

        private static string Get_Nadawca(SPWeb web, string klient, int klientId)
        {
            string pesel = string.Empty;
            string nip = string.Empty;
            string regon = string.Empty;
            string krs = string.Empty;

            tabKlienci.GetNIP(web, klientId, out pesel, out nip, out regon, out krs);

            string nadawca = String.Format("{0}{1}{2}{3}{4}",
                                klient,
                                !string.IsNullOrEmpty(pesel) ? " PESEL:" + pesel : string.Empty,
                                !string.IsNullOrEmpty(nip) ? " NIP:" + nip : string.Empty,
                                !string.IsNullOrEmpty(regon) ? " REGON:" + regon : string.Empty,
                                !string.IsNullOrEmpty(krs) ? " KRS" + krs : string.Empty);
            return nadawca;
        }

        private bool ObslugaGBW_RozliczeniePodatkuVAT(SPWeb web, SPListItem item, bool result, string targetFileNameLeading, string klient, string okres, int klientId)
        {
            bool wymaganyDrukWplaty = item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false;
            string decyzja = item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty;
            double kwota = item["colVAT_WartoscDoZaplaty"] != null ? Double.Parse(item["colVAT_WartoscDoZaplaty"].ToString()) : 0;
            string konto = Clean_NumerRachunku(item, "colVAT_Konto");

            if (wymaganyDrukWplaty && konto.Length == 26 && kwota > 0 && decyzja == "Do zapłaty")
            {
                string fileName = String.Format(@"{0}Podatek VAT_{1}.pdf",
                    targetFileNameLeading,
                    okres);

                int urzadId = item["selUrzadSkarbowy"] != null ? new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId : 0;
                BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();

                string numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2); // TODO:skorygować typ deklaracji

                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                string nadawca = iok.Get_NazwaNadawcyPrzelewu();

                //nip z kartoteki
                string nip = iok.NIP;
                string typIdentyfikatora = "N";

                string symbolFormularza = iok.FormaOpodatkowaniaVAT.Replace("-", "").Trim();
                string opis = "Podatek VAT";


                result = GeneratorDrukow.DrukWplaty.Attach_DrukWplatyPD(web, item,
                fileName,
                odbiorca,
                konto,
                kwota,
                nadawca, nip, typIdentyfikatora, numerDeklaracji, symbolFormularza, opis);
            }
            return result;
        }

        private bool ObslugaGBW_RozliczenieZBiuremRachunkowym(SPWeb web, SPListItem item, bool result, string targetFileNameLeading, string klient, string okres, string nadawca)
        {
            double kwota = item["colBR_WartoscDoZaplaty"] != null ? Double.Parse(item["colBR_WartoscDoZaplaty"].ToString()) : 0;
            string konto =  Clean_NumerRachunku(item, "colBR_Konto");

            if (konto.Length == 26 && kwota > 0)
            {
                string fileName = String.Format(@"{0}Faktura za obsługę księgową_{1}.pdf",
                    targetFileNameLeading,
                    okres);
                string odbiorca = admSetup.GetValue(web, "BR_NAZWA");
                string numerFaktury = item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty;
                string tytulem = String.Format("Zapłata za fakturę {0}", numerFaktury);

                result = GeneratorDrukow.DrukWplaty.Attach_DrukWplaty(web, item,
                fileName,
                odbiorca,
                konto,
                kwota,
                nadawca,
                tytulem);
            }
            return result;
        }

        #endregion

        private void Manage_CT(SPListItem item)
        {
            string status = item["enumStatusZadania"] != null ? item["enumStatusZadania"].ToString() : string.Empty;

            if (status != StatusZadania.Zakończone.ToString()
                && status != StatusZadania.Anulowane.ToString())
            {
                //Obsługa poleceń cmdFormatka - może zmieniać statusy zadania.
                Manage_CMD(item);

                if (item["enumStatusZadania"].ToString() == StatusZadania.Nowe.ToString())
                {
                    Update_PrzypiszOperatora(item);
                    Update_StatusZadania(item, StatusZadania.Obsługa);
                }

                string ct = item.ContentType.Name;
                switch (ct)
                {
                    case "Zadanie":
                        //Manage_Zadanie(item);
                        break;
                    case "Prośba o dokumenty":
                        //Manage_ProsbaODokumenty(item);
                        break;
                    case "Prośba o przesłanie wyciągu bankowego":
                        //Manage_ProsbaOWyciagBankowy(item);
                        break;
                    case "Rozliczenie z biurem rachunkowym":
                        //Manage_RBR(item);
                        break;
                    case "Rozliczenie podatku dochodowego":
                        //Manage_PD(item);
                        break;
                    case "Rozliczenie podatku VAT":
                        //Manage_VAT(item);
                        break;
                    case "Rozliczenie ZUS":
                        //Manage_ZUS(item);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Manage_CMD(SPListItem item)
        {
            //sprawdź czy wybrana jakaś komenda
            string cmd = GetCommand(item);

            if (string.IsNullOrEmpty(cmd)) return;

            //obsługa komend
            switch (cmd)
            {
                case ZATWIERDZ:
                    Manage_CMD_Zatwierdz(item);
                    //nie usówaj informacji z pola informacje dla klienta
                    ResetCommand(item, false);
                    break;
                case WYSLIJ_INFORMACJE_DO_KLIENTA:
                    Manage_CMD_WyslijInfo(item);
                    //wyczyść informacje dla klienta po wysyłce
                    ResetCommand(item, true);
                    break;
                default:
                    break;

            }
        }










        #region Manage CT

        private void Manage_CMD_WyslijInfo(SPListItem item)
        {
            string ct = item.ContentType.Name;

            switch (ct)
            {
                case "Zadanie":
                    Manage_CMD_WyslijInfo_Zadanie(item);
                    break;
                case "Prośba o dokumenty":
                case "Prośba o przesłanie wyciągu bankowego":
                case "Rozliczenie z biurem rachunkowym":
                case "Rozliczenie podatku dochodowego":
                case "Rozliczenie podatku VAT":
                case "Rozliczenie ZUS":
                    Manage_CMD_WyslijInfo_NoAtt(item);
                    break;
                default:
                    break;

            }
        }

        private void Manage_CMD_WyslijInfo_Zadanie(SPListItem item)
        {

            string cmd = GetCommand(item);
            string notatka = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == WYSLIJ_INFORMACJE_DO_KLIENTA
                && !string.IsNullOrEmpty(notatka))
            {
                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, klientId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "EMAIL_DEFAULT_BODY", out temat, out trescHTML);
                if (item["selProcedura"] != null)
                {
                    temat = string.Format("{0} :{1}",
                        new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue,
                        item.Title);
                }
                else
                {
                    temat = item.Title;
                }
                if (!temat.StartsWith(":"))
                {
                    temat = ":" + temat.Trim();
                }

                temat = AddSygnatura(temat, item);

                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }

            ResetCommand(item, true);
        }

        private void Manage_CMD_WyslijInfo_NoAtt(SPListItem item)
        {
            string cmd = GetCommand(item);

            string notatka = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == WYSLIJ_INFORMACJE_DO_KLIENTA)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = true; //wyślij kopię do nadawcy
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "EMAIL_DEFAULT_BODY", out temat, out trescHTML);
                //nadpisz temat wiadomości
                if (item["selProcedura"] != null)
                {
                    temat = new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue;
                }
                else
                {
                    temat = item.Title;
                }
                if (!temat.StartsWith(":"))
                {
                    temat = ":" + temat.Trim();
                }

                temat = string.Format("{0} - informacja uzupełniająca", temat, item.ID.ToString());

                temat = AddSygnatura(temat, item);

                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = new DateTime(); //wyślij natychmiast

                //nie kopiuj załączników
                BLL.tabWiadomosci.AddNew(item.Web, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }
        }

        private void Manage_CMD_Zatwierdz(SPListItem item)
        {
            string cmd = GetCommand(item);
            if (cmd == ZATWIERDZ)
            {
                string ct = item.ContentType.Name;

                switch (ct)
                {
                    case "Zadanie":
                        break;
                    case "Prośba o przesłanie wyciągu bankowego":
                        Manage_CMD_WyslijWynik_ProsbaOWyciagBankowy(item);
                        Update_StatusZadania(item, StatusZadania.Wysyłka);
                        break;
                    case "Prośba o dokumenty":
                        Manage_CMD_WyslijWynik_ProsbaODokumenty(item);
                        Update_StatusZadania(item, StatusZadania.Wysyłka);
                        break;
                    case "Rozliczenie ZUS":
                        if (isValidated_ZUS(item))
                        {
                            if (!isAuditRequest(item) || Get_Status(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                            {
                                Manage_CMD_WyslijWynik_ZUS(item);
                                Update_StatusZadania(item, StatusZadania.Wysyłka);
                            }
                            else
                            {
                                Update_StatusZadania(item, StatusZadania.Gotowe);
                            }
                        }
                        break;
                    case "Rozliczenie podatku dochodowego":
                        if (isValidated_PD(item))
                        {
                            if (!isAuditRequest(item) || Get_Status(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                            {
                                Manage_CMD_WyslijWynik_PD(item);
                                Update_StatusZadania(item, StatusZadania.Wysyłka);
                            }
                            else
                            {
                                Update_StatusZadania(item, StatusZadania.Gotowe);
                            }
                        }
                        break;
                    case "Rozliczenie podatku dochodowego spółki":
                        if (isValidated_PDS(item))
                        {
                            Manage_CMD_WyslijWynik_PDS(item);
                            Update_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        break;
                    case "Rozliczenie podatku VAT":
                        if (isValidated_VAT(item))
                        {
                            if (!isAuditRequest(item) || Get_Status(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                            {
                                Manage_CMD_WyslijWynik_VAT(item);
                                Update_StatusZadania(item, StatusZadania.Wysyłka);
                            }
                            else
                            {
                                Update_StatusZadania(item, StatusZadania.Gotowe);
                            }
                        }
                        break;

                    case "Rozliczenie z biurem rachunkowym":
                        if (isValidated_RBR(item))
                        {
                            Manage_CMD_WyslijWynik_RBR(item);
                            Update_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private string Get_Status(SPListItem item)
        {
            return item["enumStatusZadania"] != null ? item["enumStatusZadania"].ToString() : string.Empty;
        }

        private bool isAuditRequest(SPListItem item)
        {
            return item["colAudytDanych"] != null ? (bool)item["colAudytDanych"] : false;
        }


        private void Manage_CMD_WyslijWynik_ProsbaOWyciagBankowy(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {
                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "WBANK_TEMPLATE.Include", out temat, out trescHTML);

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }
        }

        private void Manage_CMD_WyslijWynik_ProsbaODokumenty(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "DOK_TEMPLATE.Include", out temat, out trescHTML);

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }
        }

        private void Manage_CMD_WyslijWynik_ZUS(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "ZUS_TEMPLATE.Include", out temat, out trescHTML);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat);

                //uzupełnia dane w formatce BR_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colZUS_SP_Skladka___", Format_Currency(item, "colZUS_SP_Skladka"));
                sb.Replace("___colZUS_SP_Konto___", item["colZUS_SP_Konto"] != null ? item["colZUS_SP_Konto"].ToString() : string.Empty);
                sb.Replace("___colZUS_TerminPlatnosciSkladek___", Format_Date(item, "colZUS_TerminPlatnosciSkladek"));
                sb.Replace("___colZUS_ZD_Skladka___", Format_Currency(item, "colZUS_ZD_Skladka"));
                sb.Replace("___colZUS_ZD_Konto___", item["colZUS_ZD_Konto"] != null ? item["colZUS_ZD_Konto"].ToString() : string.Empty);
                sb.Replace("___colZUS_FP_Skladka___", Format_Currency(item, "colZUS_FP_Skladka"));
                sb.Replace("___colZUS_FP_Konto___", item["colZUS_FP_Konto"] != null ? item["colZUS_FP_Konto"].ToString() : string.Empty);

                sb.Replace("___colZUS_PIT-4R___", Format_Currency(item, "colZUS_PIT-4R"));
                sb.Replace("___colZUS_PIT-8AR___", Format_Currency(item, "colZUS_PIT-8AR"));

                Klient k = new Klient(item.Web, klientId);

                sb.Replace("___colPIT_Konto___", k.NumerRachunkuPD);


                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag
                if (item["colZUS_PIT-4R_Zalaczony"] != null ? (bool)item["colZUS_PIT-4R_Zalaczony"] : false)
                {
                    info2 = info2 + string.Format(templateR, "PIT-4R");
                }
                if (item["colZUS_PIT-8AR_Zalaczony"] != null ? (bool)item["colZUS_PIT-8AR_Zalaczony"] : false)
                {
                    info2 = info2 + string.Format(templateR, "PIT-8AR");
                }
                if (item["colZUS_ListaPlac_Zalaczona"] != null ? (bool)item["colZUS_ListaPlac_Zalaczona"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Lista płac");
                }
                if (item["colZUS_Rachunki_Zalaczone"] != null ? (bool)item["colZUS_Rachunki_Zalaczone"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Rachunki");
                }
                if (item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Druk(i) wpłaty");
                }

                if (!string.IsNullOrEmpty(info2))
                {
                    info2 = string.Format(templateH, info2);
                    info = info + "<br>" + info2;
                }


                sb.Replace("___colInformacjaDlaKlienta___", info);

                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }

        }

        private string Format_Date(SPListItem item, string colName)
        {
            DateTime d = DateTime.Parse(Get_String(item, colName));
            return d.ToShortDateString();
        }

        private string Format_Currency(SPListItem item, string colName)
        {
            double n = GetValue(item, colName);

            if (n > 0) return n.ToString("c", new CultureInfo("pl-PL"));
            else return emptyMarker;

        }

        private void Manage_CMD_WyslijWynik_PD(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "PD_TEMPLATE.Include", out temat, out trescHTML);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat);

                //uzupełnia dane w formatce PD_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);

                sb.Replace("___colPD_OcenaWyniku___", Get_String(item, "colPD_OcenaWyniku"));
                //sb.Replace("___colPD_WartoscDochodu___", Format_Currency(item, "colPD_WartoscDochodu"));
                //sb.Replace("___colPD_WysokoscStraty___", Format_Currency(item, "colPD_WartoscStraty")); //nazwa kolumny rozbieżna
                sb.Replace("___colFormaOpodatkowaniaPD___", Get_String(item, "colFormaOpodatkowaniaPD"));
                sb.Replace("___colPD_WartoscDoZaplaty___", Format_Currency(item, "colPD_WartoscDoZaplaty"));
                sb.Replace("___colPD_Konto___", Get_String(item, "colPD_Konto"));
                sb.Replace("___colPD_TerminPlatnosciPodatku___",Format_Date(item, "colPD_TerminPlatnosciPodatku"));

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag

                if (Get_String(item, "colPD_OcenaWyniku") == "Dochód"
                && item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Druk wpłaty");
                }

                if (!string.IsNullOrEmpty(info2))
                {
                    info2 = string.Format(templateH, info2);
                    info = info + "<br>" + info2;
                }

                sb.Replace("___colInformacjaDlaKlienta___", info);

                //ukrywanie zbędnych elementów
                string ocena = Get_String(item, "colPD_OcenaWyniku");
                switch (ocena)
                {
                    case "Dochód":
                        sb.Replace("___Display_T_Platnosc___", string.Empty);
                        sb.Replace("___OpisDochodu_Straty___", "Wysokość dochodu");
                        sb.Replace("___colPD_WartoscDochodu_Straty___", Format_Currency(item, "colPD_WartoscDochodu"));
                        break;
                    case "Strata":
                        sb.Replace("___OpisDochodu_Straty___", "Wielkość straty");
                        sb.Replace("___colPD_WartoscDochodu_Straty___", Format_Currency(item, "colPD_WartoscStraty"));
                        
                        break;
                    default:
                        
                        break;
                }
                //czyszczenie parametrów
                sb.Replace("___Display_T_Platnosc___", "none");
                sb.Replace("___OpisDochodu_Straty___", string.Empty);
                sb.Replace("___colPD_WartoscDochodu_Straty___", string.Empty);

                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }

        }

        private void Manage_CMD_WyslijWynik_PDS(SPListItem item)
        {
            throw new NotImplementedException();
        }

        private void Manage_CMD_WyslijWynik_VAT(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "VAT_TEMPLATE.Include", out temat, out trescHTML);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat);

                //uzupełnia dane w formatce BR_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colVAT_Decyzja___", item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty);
                sb.Replace("___colVAT_TerminZwrotuPodatku___", item["colVAT_TerminZwrotuPodatku"] != null ? item["colVAT_TerminZwrotuPodatku"].ToString() : "?");
                sb.Replace("___colVAT_WartoscNadwyzkiZaPoprzedniMiesiac___", item["colVAT_WartoscNadwyzkiZaPoprzedniMiesiac"] != null ? item["colVAT_WartoscNadwyzkiZaPoprzedniMiesiac"].ToString() : emptyMarker);
                sb.Replace("___colVAT_WartoscDoZwrotu___", item["colVAT_WartoscDoZwrotu"] != null ? item["colVAT_WartoscDoZwrotu"].ToString() : emptyMarker);
                sb.Replace("___colVAT_WartoscDoPrzeniesienia___", item["colVAT_WartoscDoPrzeniesienia"] != null ? item["colVAT_WartoscDoPrzeniesienia"].ToString() : emptyMarker);
                sb.Replace("___colFormaOpodatkowaniaVAT___", item["colFormaOpodatkowaniaVAT"] != null ? item["colFormaOpodatkowaniaVAT"].ToString() : string.Empty);
                sb.Replace("___colVAT_WartoscDoZaplaty___", item["colVAT_WartoscDoZaplaty"] != null ? item["colVAT_WartoscDoZaplaty"].ToString() : emptyMarker);
                sb.Replace("___colVAT_Konto___", item["colVAT_Konto"] != null ? item["colVAT_Konto"].ToString() : string.Empty);
                sb.Replace("___colVAT_TerminPlatnosciPodatku___", item["colVAT_TerminPlatnosciPodatku"] != null ? DateTime.Parse(item["colVAT_TerminPlatnosciPodatku"].ToString()).ToShortDateString() : string.Empty);

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag
                if (item["colVAT_VAT-UE_Zalaczony"] != null ? (bool)item["colVAT_VAT-UE_Zalaczony"] : false)
                {
                    info2 = info2 + string.Format(templateR, "VAT-UE");
                }
                if (item["colVAT_VAT_x002d_27_Zalaczony0"] != null ? (bool)item["colVAT_VAT_x002d_27_Zalaczony0"] : false)
                {
                    info2 = info2 + string.Format(templateR, "VAT-27");
                }

                if (item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Druk wpłaty");
                }

                if (!string.IsNullOrEmpty(info2))
                {
                    info2 = string.Format(templateH, info2);
                    info = info + "<br>" + info2;
                }


                sb.Replace("___colInformacjaDlaKlienta___", info);

                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }
        }

        private void Manage_CMD_WyslijWynik_RBR(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "BR_TEMPLATE.Include", out temat, out trescHTML);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat);

                //uzupełnia dane w formatce BR_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colBR_NumerFaktury___", item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty);
                sb.Replace("___colBR_WartoscDoZaplaty___", item["colBR_WartoscDoZaplaty"] != null ? item["colBR_WartoscDoZaplaty"].ToString() : string.Empty);
                sb.Replace("___colBR_Konto___", item["colBR_Konto"] != null ? item["colBR_Konto"].ToString() : string.Empty);
                sb.Replace("___colBR_TerminPlatnosci___", item["colBR_TerminPlatnosci"] != null ? DateTime.Parse(item["colBR_TerminPlatnosci"].ToString()).ToShortDateString() : string.Empty);

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag
                if (item["colBR_FakturaZalaczona"] != null ? (bool)item["colBR_FakturaZalaczona"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Faktura za obsługę księgową");
                }
                if (item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false)
                {
                    info2 = info2 + string.Format(templateR, "Druk wpłaty");
                }

                if (!string.IsNullOrEmpty(info2))
                {
                    info2 = string.Format(templateH, info2);
                    info = info + info2;
                }

                sb.Replace("___colInformacjaDlaKlienta___", info);

                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);
            }
        }


        private bool isValidated_ZUS(SPListItem item)
        {
            //oczyść dane w zależności od wybranej Decyzji
            string opcja = item["colZUS_Opcja"] != null ? item["colZUS_Opcja"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(opcja))
            {
                return false;
            }

            switch (opcja)
            {
                case "Tylko zdrowotna":
                    ClearValue(item, "colZUS_SP_Skladka");
                    ClearValue(item, "colZUS_FP_Skladka");
                    if (GetValue(item, "colZUS_ZD_Skladka") >= 0)
                    {
                        bool foundError = false;

                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_SP_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS SP");
                            foundError = true;
                        }
                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_ZD_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS ZD");
                            foundError = true;
                        }
                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_FP_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS FP");
                            foundError = true;
                        }

                        if (foundError) return false;

                        return true;
                    }
                    break;
                default:
                    if (GetValue(item, "colZUS_SP_Skladka") >= 0
                        && GetValue(item, "colZUS_ZD_Skladka") >= 0
                        && GetValue(item, "colZUS_FP_Skladka") >= 0)
                    {
                        bool foundError = false;

                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_SP_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS SP");
                            foundError = true;
                        }
                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_ZD_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS ZD");
                            foundError = true;
                        }
                        if (string.IsNullOrEmpty(Get_String(item, "colZUS_FP_Konto")))
                        {
                            Add_Comment(item, "brak numeru konta ZUS FP");
                            foundError = true;
                        }

                        if (foundError) return false;

                        return true;
                    }
                    break;
            }

            return false;
        }

        private bool isValidated_PD(SPListItem item)
        {
            //oczyść dane w zależności od wybranej Decyzji
            string ocena = Get_String(item,"colPD_OcenaWyniku");
            if (string.IsNullOrEmpty(ocena))
            {
                return false;
            }

            switch (ocena)
            {
                case "Dochód":
                    ClearValue(item, "colPD_WartoscStraty");

                    if (GetValue(item, "colPD_WartoscDoZaplaty") >= 0
                        && GetValue(item, "colPD_WartoscDochodu") >= 0)
                        if (!string.IsNullOrEmpty(Get_String(item, "colPD_Konto"))) return true;
                        else Add_Comment(item, "brak numeru konta");
                    break;
                case "Strata":
                    ClearValue(item, "colPD_WartoscDochodu");

                    if (GetValue(item, "colPD_WartoscStraty") >= 0) return true;
                    break;
                default:
                    break;
            }

            return false;
        }

        private bool isValidated_PDS(SPListItem item)
        {
            throw new NotImplementedException();
        }

        private bool isValidated_VAT(SPListItem item)
        {
            //oczyść dane w zależności od wybranej Decyzji
            string decyzja = item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(decyzja))
            {
                return false;
            }

            switch (decyzja)
            {
                case "Do zapłaty":
                    //ClearValue(item, "colVAT_WartoscDoZaplaty");
                    ClearValue(item, "colVAT_WartoscDoPrzeniesienia");
                    ClearValue(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoZaplaty") >= 0)
                        if (!string.IsNullOrEmpty(Get_String(item, "colVAT_Konto"))) return true;
                        else Add_Comment(item, "brak numeru konta");
                    break;
                case "Do przeniesienia":
                    ClearValue(item, "colVAT_WartoscDoZaplaty");
                    //ClearValue(item, "colVAT_WartoscDoPrzeniesienia");
                    ClearValue(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoPrzeniesienia") >= 0) return true;
                    break;
                case "Do zwrotu":
                    ClearValue(item, "colVAT_WartoscDoZaplaty");
                    ClearValue(item, "colVAT_WartoscDoPrzeniesienia");
                    //ClearValue(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoZwrotu") >= 0) return true;
                    break;
                case "Do przeniesienia i do zwrotu":
                    ClearValue(item, "colVAT_WartoscDoZaplaty");
                    //ClearValue(item, "colVAT_WartoscDoPrzeniesienia");
                    //ClearValue(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoPrzeniesienia") >= 0
                        && GetValue(item, "colVAT_WartoscDoZwrotu") >= 0) return true;
                    break;
                default:
                    break;
            }

            return false;
        }

        private void Add_Comment(SPListItem item, string comment)
        {
            string uwagi = Get_String(item, "colUwagi");
            uwagi = uwagi + "\n" + DateTime.Now.ToString() + "\n" + comment;
            item["colUwagi"] = uwagi.Trim();
            item.Update();
        }

        private string Get_String(SPListItem item, string colName)
        {
            return item[colName] != null ? item[colName].ToString() : string.Empty;
        }

        private double GetValue(SPListItem item, string colName)
        {
            if (item[colName] != null)
            {
                return double.Parse(item[colName].ToString());
            }
            else
            {
                //jeżeli pusta wartość to wpisz 0
                item[colName] = 0;
                item.Update();
                return 0;
            }
        }

        private void ClearValue(SPListItem item, string colName)
        {
            if (item[colName] != null)
            {
                item[colName] = string.Empty;
                item.Update();
            }
        }

        private bool isValidated_RBR(SPListItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers
        private string AddSpecyfikacja(SPListItem item, string temat)
        {
            string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;
            string klient = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupValue : string.Empty;

            if (!string.IsNullOrEmpty(okres)) temat = temat + " " + okres;
            if (!string.IsNullOrEmpty(klient)) temat = temat + " " + klient;

            return temat;
        }
        private string AddSygnatura(string temat, SPListItem item)
        {
            if (item != null)
            {
                return string.Format("{0} [#{1}]", temat, item.ID.ToString());
            }
            return temat;
        }
        private void Update_StatusZadania(SPListItem item, StatusZadania statusZadania)
        {
            item["enumStatusZadania"] = statusZadania.ToString();
            item.Update();
        }
        private static void UsunPodobneZalaczniki(SPListItem item, string targetFileNameLeading)
        {
            if (item.Attachments.Count > 0)
            {

                System.Collections.Generic.List<string> foundNames = new System.Collections.Generic.List<string>();

                foreach (string attName in item.Attachments)
                {
                    if (attName.StartsWith(targetFileNameLeading))
                    {
                        foundNames.Add(attName);
                    }
                }

                if (foundNames.Count > 0)
                {
                    foreach (string attName in foundNames)
                    {
                        item.Attachments.Delete(attName);
                    }
                    item.Update();
                }

            }
        }
        private void Validate(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            string ct = properties.AfterProperties["ContentType"] != null ? properties.AfterProperties["ContentType"].ToString() : string.Empty;
            int klientId = properties.AfterProperties["selKlient"] != null ? new SPFieldLookupValue(properties.AfterProperties["selKlient"].ToString()).LookupId : 0;
            int okresId = properties.AfterProperties["selOkres"] != null ? new SPFieldLookupValue(properties.AfterProperties["selOkres"].ToString()).LookupId : 0;

            if (!string.IsNullOrEmpty(ct)
                && klientId > 0
                && okresId > 0)
            {
                string key = tabZadania.Define_KEY(ct, klientId, okresId);
                using (SPWeb web = properties.Web)
                {
                    properties.Cancel = !tabZadania.Check_KEY_IsAllowed(key, web, properties.ListItemId);
                    properties.ErrorMessage = "Zdublowany klucz zadania";
                }
            }

            this.EventFiringEnabled = true;
        }

        private void ResetCommand(SPListItem item, bool clearInformacjaDlaKlienta)
        {
            item["cmdFormatka"] = string.Empty;
            if (clearInformacjaDlaKlienta
                && item["colInformacjaDlaKlienta"] != null)
            {
                item["colInformacjaDlaKlienta"] = string.Empty;
            }
            item.Update();

        }

        private string GetCommand(SPListItem item)
        {
            return item["cmdFormatka"] != null ? item["cmdFormatka"].ToString() : string.Empty;
        }

        private static void Assign_OperatorBasedOnProcedura(SPListItem item, SPWeb web, int procId)
        {
            if (procId > 0 && item["selOperator"] == null)
            {
                int operatorId = BLL.tabProcedury.Get_OperatorById(web, procId);
                if (operatorId > 0)
                {

                    item["selOperator"] = operatorId;
                    item.Update();

                }
            }
        }

        private static void Assign_TerminRealizacjiBasedOnProcedura(SPListItem item, SPWeb web, int procId)
        {
            if (procId > 0 && (item["colTerminRealizacji"] == null || (DateTime)item["colTerminRealizacji"] != new DateTime()))
            {

                int termin = BLL.tabProcedury.Get_TerminRealizacjiOfsetById(web, procId);
                if (termin > 0)
                {
                    item["colTerminRealizacji"] = DateTime.Today.AddDays(termin);
                    item.Update();
                }
            }
        }

        private static int Assign_ProceduraBasedOnTitle(SPListItem item, SPWeb web)
        {
            int procId = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupId : 0;
            if (procId == 0)
            {
                procId = BLL.tabProcedury.Update(web, item.Title);
                item["selProcedura"] = procId;
                item.Update();
            }
            return procId;
        }

        #endregion
    }
}
