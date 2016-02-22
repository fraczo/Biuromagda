using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using BLL.Models;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.SharePoint.Utilities;
using System.Collections.Specialized;

namespace Workflows.tabZadaniaWF
{
    public sealed partial class tabZadaniaWF : SequentialWorkflowActivity
    {

        const string WYSLIJ_INFORMACJE_DO_KLIENTA = "Wyślij informację do Klienta";
        const string WYSLIJ_INFORMACJE_I_ZAKONCZ_ZADANIE = "Wyślij informację i zakończ zadanie";
        public String logErrorMessage_HistoryDescription = default(System.String);
        const string ZATWIERDZ = "Zatwierdź";
        const string ANULUJ = "Anuluj";
        private TaskCommands taskCMD = TaskCommands.NotDefined;

        const string emptyMarker = "---";

        //szablony do informacji o załącznikach
        const string templateH = @"<table><tr valign='top'><td><div style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #808080'><strong>w załączeniu:</strong></div></td><td><ul>{0}</ul></td></tr></table>";
        const string templateR = @"<li style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small '>{0}</li>";

        public tabZadaniaWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        public BLL.Models.ZadanieCT zadanieCT;

        DateTime startTime;
        private StatusZadania status;
        private StringBuilder vm; // validation message - from validator
        private StringBuilder vm1; // validation message - from zadania wspólników updat procedure (problemy z aktualizacją)

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("tabZadaniaWF:{" + workflowProperties.WorkflowId + "} initiated");

            item = workflowProperties.Item;
        }

        private void Manage_CMD_Zatwierdz_WyslijInfo_Zadanie(SPListItem item)
        {
            string cmd = GetCommand(item);
            string notatka = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == WYSLIJ_INFORMACJE_I_ZAKONCZ_ZADANIE
                && !string.IsNullOrEmpty(notatka))
            {
                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, klientId);

                string kopiaDla = Get_KopiaDlaOperatora(item);

                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY.Include", out temat, out trescHTML, nadawca);
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
                    temat = ": " + temat.Trim();
                }

                temat = AddSygnatura(temat, item);
                temat = BLL.Tools.AddCompanyName(temat, item);

                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);

                Set_StatusZadania(item, StatusZadania.Wysyłka);
            }
        }

        private void SetTitle_ExecuteCode(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(item.Title))
            {
                item["Title"] = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue : "#" + item.ID.ToString();
            }
        }

        /// <summary>
        /// Jeżeli operator jest przypisany to w zadaniu aktualizuje pole _KontoOperatora, które przechowuje jego login
        /// dla celów filtrowania zadań w/g bieżącego użytkownika.
        /// </summary>
        private void Set_KontoOperatora_ExecuteCode(object sender, EventArgs e)
        {
            BLL.Logger.LogEvent(item.Web.ToString(), "Zadanie.Set_OperatorUser_" + item.ID.ToString());

            if (item["selOperator"] != null)
            {
                int operatorId = new SPFieldLookupValue(item["selOperator"].ToString()).LookupId;

                int userId = BLL.dicOperatorzy.Get_UserIdById(item.Web, operatorId);

                if (item["_KontoOperatora"] == null)
                {
                    item["_KontoOperatora"] = userId;
                }
                else
                {
                    if (new SPFieldUserValue(item.Web, item["_KontoOperatora"].ToString()).LookupId != userId)
                    {
                        item["_KontoOperatora"] = userId;
                        //item.SystemUpdate();
                    }
                }
            }
            else
            {
                if (item["_KontoOperatora"] != null)
                {
                    item["_KontoOperatora"] = 0;
                    //item.SystemUpdate();
                }

            }
        }

        private void UpdateItem_ExecuteCode(object sender, EventArgs e)
        {
            item.SystemUpdate();
        }

        #region Updates

        public static void Set_Zadanie(SPListItem item, SPWeb web)
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

                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                string nadawca = iok.Get_NazwaNadawcyPrzelewu();

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
                    case "Rozliczenie podatku dochodowego spółki":
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

        public static void Set_KEY(SPListItem item)
        {
            string key = BLL.tabZadania.Define_KEY(item);
            BLL.Tools.Set_Text(item, "KEY", key);
        }

        /// <summary>
        ///jeżeli zadanie nie ma przypisanego operatora a ktoś go zaczął edytować to wtedy ta osoba zostanie przypisana
        ///do zadania o ile ma zdefiniowane konto operatora.
        /// </summary>
        /// <param name="item"></param>
        private void Set_PrzypiszOperatora(SPListItem item)
        {

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

                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);

                if (Get_FlagValue(item, "colZatrudniaPracownikow"))
                {

                    //PIT 8AR  
                    if (Get_FlagValue(item, "colZUS_PIT-8AR_Zalaczony"))
                    {
                        kwota = item["colZUS_PIT-8AR"] != null ? Double.Parse(item["colZUS_PIT-8AR"].ToString()) : 0;
                        if (kwota > 0)
                        {

                            konto = iok.NumerRachunkuPIT_PD;
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
                    if (Get_FlagValue(item, "colZUS_PIT-4R_Zalaczony"))
                    {
                        kwota = item["colZUS_PIT-4R"] != null ? Double.Parse(item["colZUS_PIT-4R"].ToString()) : 0;
                        if (kwota > 0)
                        {

                            konto = iok.NumerRachunkuPIT_PD;
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


                switch (opcja)
                {
                    case "Tylko zdrowotna":
                        //skladka zdrowotna
                        kwota = item["colZUS_ZD_Skladka"] != null ? Double.Parse(item["colZUS_ZD_Skladka"].ToString()) : 0;
                        if (kwota > 0)
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_ZD_Konto");
                            fileName = String.Format(@"{0}Składka zdrowotna_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);


                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }



                        //skladka spoleczna
                        kwota = item["colZUS_SP_Skladka"] != null ? Double.Parse(item["colZUS_SP_Skladka"].ToString()) : 0;
                        if (kwota > 0) //przypadek gdzie zatrudnia pracowników
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_SP_Konto");
                            fileName = String.Format(@"{0}ZUS 51_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);

                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }

                        //fundusz pracy
                        kwota = item["colZUS_FP_Skladka"] != null ? Double.Parse(item["colZUS_FP_Skladka"].ToString()) : 0;
                        if (kwota > 0) //przypadek gdy zatrudnia pracowników
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_FP_Konto");
                            fileName = String.Format(@"{0}ZUS 53_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);

                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }

                        break;
                    default:


                        //fundusz pracy
                        kwota = item["colZUS_FP_Skladka"] != null ? Double.Parse(item["colZUS_FP_Skladka"].ToString()) : 0;
                        if (kwota > 0)
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_FP_Konto");
                            fileName = String.Format(@"{0}ZUS 53_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);

                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }


                        //skladka zdrowotna
                        kwota = item["colZUS_ZD_Skladka"] != null ? Double.Parse(item["colZUS_ZD_Skladka"].ToString()) : 0;
                        if (kwota > 0)
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_ZD_Konto");
                            fileName = String.Format(@"{0}ZUS 52_{1}.pdf",
                                                targetFileNameLeading,
                                                okres);

                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }

                        //skladka spoleczna
                        kwota = item["colZUS_SP_Skladka"] != null ? Double.Parse(item["colZUS_SP_Skladka"].ToString()) : 0;
                        if (kwota > 0)
                        {
                            konto = Clean_NumerRachunku(item, "colZUS_SP_Konto");
                            fileName = String.Format(@"{0}ZUS 51_{1}.pdf",
                                               targetFileNameLeading,
                                               okres);

                            result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);
                        }

                        break;
                }


            }

            return result;
        }

        private bool Generuj_DrukPD_FromZUS(SPWeb web, SPListItem item, int klientId, string okres, double kwota, string konto, string fileName, string odbiorca, string numerDeklaracji, string symbolFormularza, string identyfikatorZobowiazania, BLL.Models.Klient iok)
        {
            bool result = false;

            konto = Clean_NumerRachunku(konto);

            string nadawca = iok.Get_NazwaNadawcyPrzelewu();
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
            konto = Clean_NumerRachunku(konto);

            if (konto.Length == 26 && kwota > 0 && !string.IsNullOrEmpty(fileName))
            {

                string typWplaty = "S";
                string numerDeklaracji = "01";
                string okresMiesiac = okres.Substring(5, 2);
                string okresRok = okres.Substring(0, 4);
                string numerDecyzji = ".";


                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                string nadawca = iok.Get_NazwaNadawcyPrzelewu();
                string regon = iok.Regon;
                string typIdentyfikatora = "R";
                if (string.IsNullOrEmpty(regon)) //jeżeli nie ma regonu podaj pesel
                {
                    regon = iok.Pesel;
                    typIdentyfikatora = "P";
                }
                if (string.IsNullOrEmpty(regon)) //jeżeli nie ma pesela nie podawaj drugiego identyfikatora
                {
                    typIdentyfikatora = string.Empty;
                }
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
                nadawca, nip, typIdentyfikatora.Substring(0, 1), regon, identyfikatorDeklaracji);
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

                //string fileName = String.Format(@"{0}Podatek dochodowy_{1}.pdf",
                //    targetFileNameLeading,
                //    okres);

                string fileName = String.Format(@"{0}Podatek dochodowy.pdf",
                targetFileNameLeading);

                int urzadId = item["selUrzadSkarbowy"] != null ? new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId : 0;
                BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();

                //jeżeli rozliczenie kwartalne

                string numerDeklaracji = string.Empty;

                string rozliczenie = item["enumRozliczeniePD"] != null ? item["enumRozliczeniePD"].ToString() : string.Empty;
                if (rozliczenie == "Kwartalnie")
                {
                    numerDeklaracji = okres.Substring(2, 2) + "K";

                    string m = okres.Substring(5, 2); //oznaczenie miesiąca
                    switch (m)
                    {
                        case "01":
                        case "02":
                        case "03":
                            numerDeklaracji = numerDeklaracji + "01";
                            break;
                        case "04":
                        case "05":
                        case "06":
                            numerDeklaracji = numerDeklaracji + "02";
                            break;
                        case "07":
                        case "08":
                        case "09":
                            numerDeklaracji = numerDeklaracji + "03";
                            break;
                        case "10":
                        case "11":
                        case "12":
                            numerDeklaracji = numerDeklaracji + "04";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2);
                }

                BLL.Models.Klient iok = new BLL.Models.Klient(web, klientId);
                string nadawca = iok.Get_NazwaNadawcyPrzelewu();

                //nip z kartoteki
                string nip = iok.NIP;
                string typIdentyfikatora = "N";

                string symbolFormularza = Get_SymbolFormularzaPD(item);
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

        private string Get_SymbolFormularzaPD(SPListItem item)
        {
            string result = string.Empty;
            string formaOpodatkowaniaPD = item["colFormaOpodatkowaniaPD"] != null ? item["colFormaOpodatkowaniaPD"].ToString() : string.Empty;
            switch (formaOpodatkowaniaPD)
            {
                case "CIT":
                    result = "CIT-8";
                    break;
                case "Zasady ogólne":
                    result = "PIT-5";
                    break;
                case "Podatek liniowy":
                    result = "PPL";
                    break;
                case "Karta podatkowa":
                    result = "KP";
                    break;
                case "Ryczałt":
                    result = "PPE";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// jeżeli numer rachunku ma więcej niż 26 znaków usówa wszystkie znaki nie będące cyfrą
        /// </summary>
        private string Clean_NumerRachunku(SPListItem item, string colName)
        {
            string numerRachunku = item[colName] != null ? item[colName].ToString() : string.Empty;

            numerRachunku = Clean_NumerRachunku(numerRachunku);

            return numerRachunku;
        }

        private static string Clean_NumerRachunku(string numerRachunku)
        {
            if (numerRachunku.Length > 26)
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

            BLL.tabKlienci.GetNIP(web, klientId, out pesel, out nip, out regon, out krs);

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
                //string fileName = String.Format(@"{0}Podatek VAT_{1}.pdf",
                //    targetFileNameLeading,
                //    okres);

                string fileName = String.Format(@"{0}Podatek VAT.pdf",
                targetFileNameLeading);

                int urzadId = item["selUrzadSkarbowy"] != null ? new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId : 0;
                BLL.Models.UrzadSkarbowy us = new BLL.Models.UrzadSkarbowy(web, urzadId);
                string odbiorca = us.Get_NazwaOdbiorcyPrzelewu();

                string numerDeklaracji = string.Empty;

                string rozliczenie = item["enumRozliczenieVAT"] != null ? item["enumRozliczenieVAT"].ToString() : string.Empty;
                if (rozliczenie == "Kwartalnie")
                {
                    numerDeklaracji = okres.Substring(2, 2) + "K";

                    string m = okres.Substring(5, 2); //oznaczenie miesiąca
                    switch (m)
                    {
                        case "01":
                        case "02":
                        case "03":
                            numerDeklaracji = numerDeklaracji + "01";
                            break;
                        case "04":
                        case "05":
                        case "06":
                            numerDeklaracji = numerDeklaracji + "02";
                            break;
                        case "07":
                        case "08":
                        case "09":
                            numerDeklaracji = numerDeklaracji + "03";
                            break;
                        case "10":
                        case "11":
                        case "12":
                            numerDeklaracji = numerDeklaracji + "04";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    numerDeklaracji = okres.Substring(2, 2) + "M" + okres.Substring(5, 2);
                }

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
            string konto = Clean_NumerRachunku(item, "colBR_Konto");

            if (konto.Length == 26 && kwota > 0)
            {
                //string fileName = String.Format(@"{0}Faktura za obsługę księgową_{1}.pdf",
                //    targetFileNameLeading,
                //    okres);

                string fileName = String.Format(@"{0}Faktura za obsługę księgową.pdf",
                targetFileNameLeading);


                //string odbiorca = admSetup.GetValue(web, "BR_NAZWA");
                string odbiorca = BLL.admSetup.Get_NazwaBiura(web);
                string numerFaktury = item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty;
                string tytulem = String.Format("Zapłata za FV {0}", numerFaktury);

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

        #region Manage CT

        private void Manage_CMD_Anuluj(SPListItem item)
        {
            string cmd = GetCommand(item);
            if (cmd == ANULUJ)
            {
                Set_StatusZadania(item, StatusZadania.Anulowane);
            }
        }

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

                string kopiaDla = Get_KopiaDlaOperatora(item);

                bool KopiaDoNadawcy = true;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY.Include", out temat, out trescHTML, nadawca);
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
                    temat = ": " + temat.Trim();
                }

                temat = AddSygnatura(temat, item);
                temat = BLL.Tools.AddCompanyName(temat, item);

                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
            }

            ResetCommand(item, true);
        }

        /// <summary>
        /// jeżeli Editor nie jest aktualnym właścicielem zadania dodaj kopię do właściciela
        /// </summary>
        private string Get_KopiaDlaOperatora(SPListItem item)
        {
            int operatorId = Get_LookupId(item, "selOperator");
            string editorLoginName = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.LoginName;
            int editorId = BLL.dicOperatorzy.Get_OperatorIdByLoginName(item.Web, editorLoginName);
            if (operatorId > 0 && editorId != operatorId)
            {
                return BLL.dicOperatorzy.Get_EmailById(item.Web, operatorId);
            }

            return string.Empty;
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

                string kopiaDla = Get_KopiaDlaOperatora(item);

                bool KopiaDoNadawcy = true; //wyślij kopię do nadawcy
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "EMAIL_DEFAULT_BODY.Include", out temat, out trescHTML, nadawca);
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
                temat = BLL.Tools.AddCompanyName(temat, item);

                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                sb.Replace("___FOOTER___", string.Empty);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = new DateTime(); //wyślij natychmiast

                //nie wysyłaj załączników
                int zadanieId = item.ID;
                BLL.tabWiadomosci.AddNew_NoAtt(item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, zadanieId, klientId);
            }
        }

        private void Manage_CMD_Zatwierdz(SPListItem item)
        {
            string ct = item.ContentType.Name;

            switch (ct)
            {
                case "Zadanie":
                    Set_StatusZadania(item, StatusZadania.Zakończone);
                    break;
                case "Prośba o przesłanie wyciągu bankowego":
                    Manage_CMD_WyslijWynik_ProsbaOWyciagBankowy(item);
                    Set_StatusZadania(item, StatusZadania.Wysyłka);
                    break;
                case "Prośba o dokumenty":
                    Manage_CMD_WyslijWynik_ProsbaODokumenty(item);
                    Set_StatusZadania(item, StatusZadania.Wysyłka);
                    break;
                case "Rozliczenie ZUS":
                    if (isValidated_ZUS(item))
                    {
                        if (!isAuditRequest(item) || Get_StatusZadania(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                        {
                            Update_GBW(item.Web, item, ct);

                            Manage_CMD_WyslijWynik_ZUS(item);
                            Update_KartaKlienta_ZUS(item);
                            Set_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        else
                        {
                            //jeżeli status gotowe to aktualizuj kartę kontrolną
                            Update_KartaKlienta_ZUS(item);
                            Set_StatusZadania(item, StatusZadania.Gotowe);
                        }
                    }
                    break;
                case "Rozliczenie podatku dochodowego":
                    if (isValidated_PD(item))
                    {
                        if (!isAuditRequest(item) || Get_StatusZadania(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                        {
                            Update_GBW(item.Web, item, ct);

                            Manage_CMD_WyslijWynik_PD(item, OpcjaWysylkiPD.PD);
                            Update_KartaKlienta_PD(item);
                            Set_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        else
                        {
                            //jeżeli status gotowe to aktualizuj kartę kontrolną
                            Update_KartaKlienta_PD(item);
                            Set_StatusZadania(item, StatusZadania.Gotowe);
                        }
                    }
                    break;
                case "Rozliczenie podatku dochodowego spółki":
                    if (isValidated_PDS(item))
                    {
                        if (!isAuditRequest(item) || Get_StatusZadania(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                        {
                            Update_GBW(item.Web, item, ct);

                            if (!BLL.Tools.Get_Flag(item, "_IsSpolkaZoo"))
                            {
                                //spółka osobowa 
                                Update_DochodyWspolnikow(item);
                            }
                            else
                            {
                                // wyślij wyniki tylko do spólek kapitałowych
                                Manage_CMD_WyslijWynik_PDS(item);
                            }

                            Update_KartaKlienta_PDS(item);
                            Set_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        else
                        {
                            Update_DochodyWspolnikow(item);

                            //jeżeli status gotowe to aktualizuj kartę kontrolną
                            Update_KartaKlienta_PDS(item);
                            Set_StatusZadania(item, StatusZadania.Gotowe);
                        }
                    }
                    break;

                case "Rozliczenie podatku dochodowego wspólnika":
                    if (isValidated_PDW(item))
                    {
                        if (!isAuditRequest(item) || Get_StatusZadania(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                        {
                            //ToDo: dla wspólników jako dochód bierzemy wartość colPD_WartoscDoZaplaty
                            //Update_GBW(item.Web, item, ct); 

                            Manage_CMD_WyslijWynik_PDW(item);
                            //ToDo: BLL.Tools.DoWithRetry(() => Update_KartaKlienta_PDW(item));
                            Set_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        else
                        {
                            //jeżeli status gotowe to aktualizuj kartę kontrolną
                            //BLL.Tools.DoWithRetry(() => Update_KartaKlienta_PDW(item));
                            Set_StatusZadania(item, StatusZadania.Gotowe);
                        }
                    }
                    break;
                case "Rozliczenie podatku VAT":
                    if (isValidated_VAT(item))
                    {
                        if (!isAuditRequest(item) || Get_StatusZadania(item) == StatusZadania.Gotowe.ToString()) //zatwiedzenie gotowego zadania powoduje jego zwolnienie
                        {
                            //!!!rekord nie jest zaktualizowany
                            //item.SystemUpdate();
                            Update_GBW(item.Web, item, ct);

                            Manage_CMD_WyslijWynik_VAT(item);
                            Update_KartaKlienta_VAT(item);
                            Set_StatusZadania(item, StatusZadania.Wysyłka);
                        }
                        else
                        {
                            //jeżeli status gotowe to aktualizuj kartę kontrolną
                            Update_KartaKlienta_VAT(item);
                            Set_StatusZadania(item, StatusZadania.Gotowe);
                        }
                    }
                    break;

                case "Rozliczenie z biurem rachunkowym":
                    if (isValidated_RBR(item))
                    {
                        //!!!rekord nie jest zaktualizowany
                        //item.SystemUpdate();
                        Update_GBW(item.Web, item, ct);

                        Manage_CMD_WyslijWynik_RBR(item);
                        Update_KartaKlienta_RBR(item);
                        Set_StatusZadania(item, StatusZadania.Wysyłka);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Update_DochodyWspolnikow(SPListItem item)
        {
            int klientId = BLL.Tools.Get_LookupId(item, "selKlient");
            int okresId = BLL.Tools.Get_LookupId(item, "selOkres");

            //double colZyskStrataNetto = BLL.Tools.Get_Value(item, "colZyskStrataNetto"); << ta wartość jest zaokrąglana

            double zsn = 0;
            string colPD_OcenaWyniku = BLL.Tools.Get_Text(item, "colPD_OcenaWyniku");
            if (colPD_OcenaWyniku.Equals("Dochód")) zsn = BLL.Tools.Get_Value(item, "colPD_WartoscDochodu");
            else if (colPD_OcenaWyniku.Equals("Strata")) zsn = BLL.Tools.Get_Value(item, "colPD_WartoscStraty");

            // kalkulacja do podziału nie uwzględnia straty ponieważ strata powinna być 0 i jest ona rozliczana na wspólniku
            double colZyskStrataDoPodzialu = zsn - BLL.Tools.Get_Value(item, "colPrzychodyZwolnione");

            //rozpisz na wspólników i zainicjuj aktulizację wyników wspólników
            string validationMessage = string.Empty;
            double variance = BLL.tabDochodyWspolnikow.Update_DochodyWspolnikow(item.Web, klientId, okresId, colZyskStrataDoPodzialu, out validationMessage);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                vm1.Append(validationMessage);
            }
        }



        #region Obsługa przypomnień o terminie płatności


        private int Get_LookupId(SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }

        private bool hasPrzypomnienieOTerminiePlatnosci(SPListItem item)
        {
            string col = "colPrzypomnienieOTerminiePlatnos";
            return item[col] != null ? bool.Parse(item[col].ToString()) : false;
        }

        #endregion

        #region Aktualizacja KartyKlienta
        private void Update_KartaKlienta_ZUS(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_ZUS_Data(item));
        }

        private void Update_KartaKlienta_PD(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_PD_Data(item));
        }

        private void Update_KartaKlienta_PDS(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_PDS_Data(item));
        }

        private void Update_KartaKlienta_PDW(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_PDW_Data(item));
        }

        private void Update_KartaKlienta_VAT(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_VAT_Data(item));
        }
        private void Update_KartaKlienta_RBR(SPListItem item)
        {
            BLL.Tools.DoWithRetry(() => BLL.tabKartyKontrolne.Update_RBR_Data(item));
        }
        #endregion

        private string Get_StatusZadania(SPListItem item)
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
                //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;

                string nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);

                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                //BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "WBANK_TEMPLATE.Include", out temat, out trescHTML);
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "WBANK_TEMPLATE.Include", out temat, out trescHTML, false);

                //dodaj nazwę firmy w tytule wiadomości
                temat = BLL.Tools.AddCompanyName(temat, item);

                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                int okresId = Get_LookupId(item, "selOkres");
                string aktualnyMiesiac = BLL.tabOkresy.Get_PoprzedniMiesiacSlownieById(item.Web, okresId, 0);
                if (aktualnyMiesiac != null) aktualnyMiesiac = string.Format(@"({0})", aktualnyMiesiac);
                trescHTML = trescHTML.Replace("___PoprzedniMiesiac___", aktualnyMiesiac);

                string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);
                trescHTML = trescHTML.Replace("___Firma___", firma);

                trescHTML = trescHTML.Replace("___colInformacjaDlaKlienta___", info);

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
            }
        }

        private void Manage_CMD_WyslijWynik_ProsbaODokumenty(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {
                CreateMessage_ProsbaODokumenty(item, klientId);
            }
        }

        private static void CreateMessage_ProsbaODokumenty(SPListItem item, int klientId)
        {
            //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
            string nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
            string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

            string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);

            bool KopiaDoNadawcy = false;
            bool KopiaDoBiura = false;
            string temat = string.Empty;
            string tresc = string.Empty;
            string trescHTML = string.Empty;

            //weź szablon bez stopki
            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "DOK_TEMPLATE.Include", out temat, out trescHTML, false);

            //dodaj nazwę firmy w tytule wiadomości
            temat = BLL.Tools.AddCompanyName(temat, item);

            string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);
            trescHTML = trescHTML.Replace("___Firma___", firma);

            string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
            trescHTML = trescHTML.Replace("___colInformacjaDlaKlienta___", info);

            DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

            BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
        }


        private void Manage_CMD_WyslijWynik_ZUS(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;

                //string nadawca = Get_WlacicielZadania(item);
                //string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

                string nadawca = Get_CurrentUser(item);
                //string kopiaDla = Get_WlacicielZadania(item);
                string kopiaDla = string.Empty;

                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);

                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                //wybór szablonu wiadomości

                string zusOpcja = Get_String(item, "colZUS_Opcja");

                if (Get_FlagValue(item, "colZatrudniaPracownikow"))
                {
                    if (GetValue(item, "colZUS_PIT-4R") > 0 || GetValue(item, "colZUS_PIT-8AR") > 0)
                    {

                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_S_Z_F_PIT_TEMPLATE.Include", out temat, out trescHTML, nadawca);

                    }
                    else
                    {
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_S_Z_F_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                    }
                }
                else
                {
                    switch (zusOpcja)
                    {
                        case "Tylko zdrowotna":
                            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_Z_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                            break;
                        default:
                            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_S_Z_F_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                            break;
                    }
                }

                string lt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_LEADING_TEXT", false);
                string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);

                BLL.Models.Klient iok = new Klient(item.Web, klientId);
                if (iok.TypKlienta == "Osoba fizyczna")
                {
                    firma = "wspólnika " + firma;
                }
                else
                {
                    firma = "firmy " + firma;
                }
                lt = lt.Replace("___FIRMA___", firma);
                string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;
                lt = lt.Replace("___OKRES___", okres);
                trescHTML = trescHTML.Replace("___ZUS_LEADING_TEXT___", lt);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat, string.Empty);

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

                sb.Replace("___colPIT_Konto___", k.NumerRachunkuPIT_PD);

                int okresId = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupId : 0;

                DateTime terminPlatnosciPodatku = BLL.tabOkresy.Get_TerminPlatnosciByOkresId(item.Web, "colPD_TerminPlatnosciPodatku", okresId);
                sb.Replace("___colZUS_TerminPlatnosciPodatku___", terminPlatnosciPodatku.ToShortDateString());

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag
                if (item["colZUS_PIT-4R_Zalaczony"] != null ? (bool)item["colZUS_PIT-4R_Zalaczony"] : false)
                {
                    if (Get_Flag(item, "colDrukWplaty"))
                    {
                        info2 = info2 + string.Format(templateR, "Druk wpłaty PIT-4R");
                    }
                }
                if (item["colZUS_PIT-8AR_Zalaczony"] != null ? (bool)item["colZUS_PIT-8AR_Zalaczony"] : false)
                {
                    if (Get_Flag(item, "colDrukWplaty"))
                    {
                        info2 = info2 + string.Format(templateR, "Druk wpłaty PIT-8AR");
                    }
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
                    if (BLL.Tools.Get_Value(item, "colZUS_SP_Skladka") > 0
                        || BLL.Tools.Get_Value(item, "colZUS_ZD_Skladka") > 0
                        || BLL.Tools.Get_Value(item, "colZUS_FP_Skladka") > 0)
                    {
                        info2 = info2 + string.Format(templateR, "Druk(i) wpłaty ZUS");
                    }

                }

                if (!string.IsNullOrEmpty(info2))
                {
                    info2 = string.Format(templateH, info2);
                    info = info + "<br>" + info2;
                }


                sb.Replace("___colInformacjaDlaKlienta___", info);

                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                odbiorca = Check_NieWysylacDoKlientaFlag(item, nadawca, odbiorca);

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);


                //reminders
                if (hasPrzypomnienieOTerminiePlatnosci(item))
                {
                    KopiaDoNadawcy = false;
                    KopiaDoBiura = false;

                    //składki zus
                    DateTime terminPlatnosci = Get_Date(item, "colZUS_TerminPlatnosciSkladek");

                    if (GetValue(item, "colZUS_SP_Skladka") > 0
                        || GetValue(item, "colZUS_ZD_Skladka") > 0
                        || GetValue(item, "colZUS_FP_Skladka") > 0)
                    {
                        //ustaw reminder
                        nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");

                        if (Get_FlagValue(item, "colZatrudniaPracownikow"))
                        {
                            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_S_Z_F_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        }
                        else
                        {
                            switch (zusOpcja)
                            {
                                case "Tylko zdrowotna":
                                    BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_Z_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                                    break;
                                default:
                                    BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_S_Z_F_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                                    break;
                            }
                        }

                        temat = Update_Data(temat, terminPlatnosci);
                        temat = BLL.Tools.AddCompanyName(temat, item);

                        //leading reminder text
                        string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_LEADING_REMINDER_TEXT", false);
                        lrt = lrt.Replace("___FIRMA___", firma);
                        lrt = lrt.Replace("___OKRES___", okres);
                        trescHTML = trescHTML.Replace("___ZUS_LEADING_REMINDER_TEXT___", lrt);

                        //trailing reminder text
                        string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_TRAILING_REMINDER_TEXT", false);
                        trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                        trescHTML = trescHTML.Replace("___ZUS_TRAILING_REMINDER_TEXT___", trt);

                        //aktualizacja danych z tabelki
                        sb = new StringBuilder(trescHTML);
                        sb.Replace("___colZUS_SP_Skladka___", Format_Currency(item, "colZUS_SP_Skladka"));
                        sb.Replace("___colZUS_SP_Konto___", item["colZUS_SP_Konto"] != null ? item["colZUS_SP_Konto"].ToString() : string.Empty);
                        sb.Replace("___colZUS_TerminPlatnosciSkladek___", Format_Date(item, "colZUS_TerminPlatnosciSkladek"));
                        sb.Replace("___colZUS_ZD_Skladka___", Format_Currency(item, "colZUS_ZD_Skladka"));
                        sb.Replace("___colZUS_ZD_Konto___", item["colZUS_ZD_Konto"] != null ? item["colZUS_ZD_Konto"].ToString() : string.Empty);
                        sb.Replace("___colZUS_FP_Skladka___", Format_Currency(item, "colZUS_FP_Skladka"));
                        sb.Replace("___colZUS_FP_Konto___", item["colZUS_FP_Konto"] != null ? item["colZUS_FP_Konto"].ToString() : string.Empty);

                        trescHTML = sb.ToString();


                        planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);

                        //nie wysyłaj przypomnienia jeżeli krócej niż 3 dni do terminu
                        if (planowanaDataNadania.CompareTo(DateTime.Now.AddDays(3)) > 0)
                        {
                            BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.ReminderZUS);
                        }
                    }

                    //podatek za pracowników

                    terminPlatnosci = BLL.tabOkresy.Get_TerminPlatnosciByOkresId(item.Web, "colPD_TerminPlatnosciPodatku", okresId);

                    if (GetValue(item, "colZUS_PIT-4R") > 0
                        || GetValue(item, "colZUS_PIT-8AR") > 0)
                    {
                        //ustaw reminder
                        nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_PIT_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);

                        temat = Update_Data(temat, terminPlatnosci);
                        temat = BLL.Tools.AddCompanyName(temat, item);

                        //leading reminder text
                        string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_PIT_LEADING_REMINDER_TEXT", false);
                        lrt = lrt.Replace("___FIRMA___", firma);
                        lrt = lrt.Replace("___OKRES___", okres);
                        trescHTML = trescHTML.Replace("___ZUS_PIT_LEADING_REMINDER_TEXT___", lrt);

                        //trailing reminder text
                        string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "ZUS_PIT_TRAILING_REMINDER_TEXT", false);
                        trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                        trescHTML = trescHTML.Replace("___ZUS_PIT_TRAILING_REMINDER_TEXT___", trt);

                        //aktualizacja danych z tabelki
                        sb = new StringBuilder(trescHTML);
                        sb.Replace("___colPIT_Konto___", k.NumerRachunkuPD);
                        sb.Replace("___colZUS_PIT-4R___", Format_Currency(item, "colZUS_PIT-4R"));
                        sb.Replace("___colZUS_PIT-8AR___", Format_Currency(item, "colZUS_PIT-8AR"));
                        sb.Replace("___colZUS_TerminPlatnosciPodatku___", terminPlatnosciPodatku.ToShortDateString());

                        trescHTML = sb.ToString();

                        planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);

                        //nie wysyłaj przypomnienia jeżeli krócej niż 3 dni do terminu
                        if (planowanaDataNadania.CompareTo(DateTime.Now.AddDays(3)) > 0)
                        {
                            BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.ReminderZUS_PIT);
                        }
                    }
                }
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

        /// <summary>
        /// obsługuje 3 typy formatek w zależności od wybranej opcji wysyłki
        /// </summary>
        private void Manage_CMD_WyslijWynik_PD(SPListItem item, OpcjaWysylkiPD opcjaWyslki)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {
                //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;

                //string nadawca = Get_WlacicielZadania(item);
                //string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

                string nadawca = Get_CurrentUser(item);
                //string kopiaDla = Get_WlacicielZadania(item);
                string kopiaDla = string.Empty;

                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);

                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                switch (Get_String(item, "colPD_OcenaWyniku"))
                {
                    case "Dochód":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_DOCHOD_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        //jeżeli wartość do zapłaty = 0 wtdy zastąp tekst formułką i ukryj tabelkę z płatnościami
                        if (GetValue(item, "colPD_WartoscDoZaplaty") == 0)
                            trescHTML = trescHTML.Replace("___NOTIFICATION___", BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_DOCHOD_0_NOTIFICATION", false));
                        else
                            trescHTML = trescHTML.Replace("___NOTIFICATION___", "WARTOŚĆ DO ZAPŁATY");
                        break;
                    case "Strata":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_STRATA_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        break;
                    default:
                        break;
                }

                string lt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_LEADING_TEXT", false);
                string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);

                BLL.Models.Klient iok = new Klient(item.Web, klientId);
                if (iok.TypKlienta == "Osoba fizyczna")
                {
                    firma = "wspólnika " + firma;
                }
                else
                {
                    firma = "firmy " + firma;
                }

                lt = lt.Replace("___FIRMA___", firma);

                //opis okresu rozliczeniowego
                string okresTemat;
                string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;

                if (Get_String(item, "enumRozliczeniePD") == "Kwartalnie")
                {
                    okresTemat = BLL.Tools.Get_KwartalDisplayName(okres);
                    okres = "kwartał " + okresTemat;
                }
                else
                {
                    okresTemat = okres;
                    okres = "miesiąc " + okresTemat;
                }
                lt = lt.Replace("___OKRES___", okres);
                trescHTML = trescHTML.Replace("___PD_LEADING_TEXT___", lt);

                //VAT alert
                string va = string.Empty;
                int okresId = Get_LookupId(item, "selOkres");
                int vatZadanieId = BLL.tabZadania.Get_NumerZadaniaVAT(item.Web, klientId, okresId);
                if (vatZadanieId > 0)
                {
                    va = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_VAT_ALERT_TEXT", false);
                }
                trescHTML = trescHTML.Replace("___PD_VAT_ALERT_TEXT___", va);


                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat, okresTemat);


                //uzupełnia dane w formatce PD_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);

                sb.Replace("___colPD_OcenaWyniku___", Get_String(item, "colPD_OcenaWyniku"));
                sb.Replace("___colPD_WartoscDochodu___", Format_Currency(item, "colPD_WartoscDochodu"));
                sb.Replace("___colPD_WysokoscStraty___", Format_Currency(item, "colPD_WartoscStraty")); //nazwa kolumny rozbieżna
                sb.Replace("___colFormaOpodatkowaniaPD___", Get_String(item, "colFormaOpodatkowaniaPD"));
                sb.Replace("___colPD_WartoscDoZaplaty___", Format_Currency(item, "colPD_WartoscDoZaplaty"));
                sb.Replace("___colPD_Konto___", Get_String(item, "colPD_Konto"));
                sb.Replace("___colPD_TerminPlatnosciPodatku___", Format_Date(item, "colPD_TerminPlatnosciPodatku"));

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag

                //dodaj specyfikację dochodów z innych spółek
                string sinfo = BLL.Tools.Get_Text(item, "_Specyfikacja");
                if (!string.IsNullOrEmpty(sinfo)) info = "<b>Powyższa informacja uwzględnia rozliczenie spółek:</b><br>" + sinfo + "<br>";

                if (Get_String(item, "colPD_OcenaWyniku") == "Dochód"
                && (item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false))
                {
                    if (GetValue(item, "colPD_WartoscDoZaplaty") > 0)
                    {
                        info2 = info2 + string.Format(templateR, "Druk wpłaty");
                    }
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
                        if (GetValue(item, "colPD_WartoscDoZaplaty") > 0)
                            sb.Replace("___Display_T_Platnosc___", string.Empty);
                        break;
                    case "Strata":

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

                odbiorca = Check_NieWysylacDoKlientaFlag(item, nadawca, odbiorca);

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);

                //obsługa remindera
                if (hasPrzypomnienieOTerminiePlatnosci(item))
                {
                    KopiaDoNadawcy = false;
                    KopiaDoBiura = false;

                    DateTime terminPlatnosci = Get_Date(item, "colPD_TerminPlatnosciPodatku");

                    if (Get_String(item, "colPD_OcenaWyniku") == "Dochód")
                    {
                        if (GetValue(item, "colPD_WartoscDoZaplaty") > 0)
                        {
                            //ustaw reminder
                            nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_DOCHOD_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                            temat = Update_Data(temat, terminPlatnosci);
                            temat = BLL.Tools.AddCompanyName(temat, item);

                            //leading reminder text
                            string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_LEADING_REMINDER_TEXT", false);
                            if (item.ContentType.Name == "Osoba fizyczna")
                            {
                                firma = "wspólnika " + firma;
                            }
                            else
                            {
                                firma = "firmy " + firma;
                            }
                            lrt = lrt.Replace("___FIRMA___", firma);
                            lrt = lrt.Replace("___OKRES___", okres);
                            trescHTML = trescHTML.Replace("___PD_LEADING_REMINDER_TEXT___", lrt);

                            //trailing reminder text
                            string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "PD_TRAILING_REMINDER_TEXT", false);
                            trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                            trescHTML = trescHTML.Replace("___PD_TRAILING_REMINDER_TEXT___", trt);

                            //aktualizacja danych z tabelki
                            sb = new StringBuilder(trescHTML);
                            sb.Replace("___colFormaOpodatkowaniaPD___", Get_String(item, "colFormaOpodatkowaniaPD"));
                            sb.Replace("___colPD_WartoscDoZaplaty___", Format_Currency(item, "colPD_WartoscDoZaplaty"));
                            sb.Replace("___colPD_Konto___", Get_String(item, "colPD_Konto"));
                            sb.Replace("___colPD_TerminPlatnosciPodatku___", Format_Date(item, "colPD_TerminPlatnosciPodatku"));

                            trescHTML = sb.ToString();

                            planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);

                            //nie wysyłaj przypomnienia jeżeli krócej niż 3 dni do terminu
                            if (planowanaDataNadania.CompareTo(DateTime.Now.AddDays(3)) > 0)
                            {
                                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
                            }
                        }
                    }
                }

            }

        }

        private void Manage_CMD_WyslijWynik_PDS(SPListItem item)
        {
            //obsługa wysyłki informacji identyczna jak w przypadku PD
            Manage_CMD_WyslijWynik_PD(item, OpcjaWysylkiPD.PDS);
        }

        private void Manage_CMD_WyslijWynik_PDW(SPListItem item)
        {
            //obsługa wysyłki informacji identyczna jak w przypadku PD
            Manage_CMD_WyslijWynik_PD(item, OpcjaWysylkiPD.PDW);
        }

        private string Get_CurrentUser(SPListItem item)
        {
            string result = item["Editor"] != null ? new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email : string.Empty;

            if (string.IsNullOrEmpty(result))
            {
                //ustaw domyślnie adres biura
                result = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
            }

            return result;
        }

        private string Update_Data(string temat, DateTime terminPlatnosci)
        {
            return temat.Replace("___DATA___", terminPlatnosci.ToShortDateString());
        }

        /// <summary>
        /// domyślnym nadawcą wiadomości jest bieżący operator a jeżeli go nie ma to biuro
        /// </summary>
        private string Get_WlacicielZadania(SPListItem item)
        {
            string result = string.Empty;
            int operatorId = Get_LookupId(item, "selOperator");
            if (operatorId > 0)
            {
                result = BLL.dicOperatorzy.Get_EmailById(item.Web, operatorId);
            }

            if (string.IsNullOrEmpty(result))
            {
                result = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
            }

            return result;

        }

        private static string Calc_ReminderSubject(SPListItem item, string kodFormatki, DateTime terminPlatnosci)
        {
            string result = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, kodFormatki, false);
            result = result.Replace("___DATA___", terminPlatnosci.ToShortDateString());
            return result;
        }

        private static DateTime Calc_ReminderTime(SPListItem item, DateTime terminPlatnosci)
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
            return reminderDate;
        }



        private void Manage_CMD_WyslijWynik_VAT(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {

                //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;

                //string nadawca = Get_WlacicielZadania(item);
                //string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

                string nadawca = Get_CurrentUser(item);
                //string kopiaDla = Get_WlacicielZadania(item);
                string kopiaDla = string.Empty;


                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                bool IsBWAllowed = false; //Czy informacja o blankicie wpłaty może być załączona

                switch (item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty)
                {
                    case "Do zapłaty":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_DO_ZAPLATY_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        IsBWAllowed = true;
                        break;
                    case "Do przeniesienia":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_DO_PRZENIESIENIA_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        break;
                    case "Do zwrotu":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_DO_ZWROTU_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        break;
                    case "Do przeniesienia i do zwrotu":
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_DO_PRZENIESIENIA_ZWROTU_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        break;
                    default:
                        break;
                }

                string lt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_LEADING_TEXT", false);
                string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);
                lt = lt.Replace("___FIRMA___", firma);

                //zdefiniuj opis bieżącego okresu
                string okresTemat;
                string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;

                if (Get_String(item, "enumRozliczenieVAT") == "Kwartalnie")
                {
                    okresTemat = BLL.Tools.Get_KwartalDisplayName(okres);
                    okres = "kwartał " + okresTemat;
                }
                else
                {
                    okresTemat = okres;
                    okres = "miesiąc " + okresTemat;
                }

                lt = lt.Replace("___OKRES___", okres);
                trescHTML = trescHTML.Replace("___VAT_LEADING_TEXT___", lt);


                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat, okresTemat);

                //uzupełnia dane w formatce VAT_TEMPLATE 
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colVAT_Decyzja___", item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty);
                sb.Replace("___colVAT_TerminZwrotuPodatku___", item["colVAT_TerminZwrotuPodatku"] != null ? item["colVAT_TerminZwrotuPodatku"].ToString() : "?");
                sb.Replace("___colVAT_WartoscNadwyzkiZaPoprzedniMiesiac___", Format_Currency(item, "colVAT_WartoscNadwyzkiZaPoprzedniMiesiac"));
                sb.Replace("___colVAT_WartoscDoZwrotu___", Format_Currency(item, "colVAT_WartoscDoZwrotu"));
                sb.Replace("___colVAT_WartoscDoPrzeniesienia___", Format_Currency(item, "colVAT_WartoscDoPrzeniesienia"));
                sb.Replace("___colFormaOpodatkowaniaVAT___", item["colFormaOpodatkowaniaVAT"] != null ? item["colFormaOpodatkowaniaVAT"].ToString() : string.Empty);
                sb.Replace("___colVAT_WartoscDoZaplaty___", Format_Currency(item, "colVAT_WartoscDoZaplaty"));
                sb.Replace("___colVAT_Konto___", item["colVAT_Konto"] != null ? item["colVAT_Konto"].ToString() : string.Empty);
                sb.Replace("___colVAT_TerminPlatnosciPodatku___", item["colVAT_TerminPlatnosciPodatku"] != null ? DateTime.Parse(item["colVAT_TerminPlatnosciPodatku"].ToString()).ToShortDateString() : string.Empty);

                string info2 = string.Empty;
                string info = item["colInformacjaDlaKlienta"] != null ? item["colInformacjaDlaKlienta"].ToString() : string.Empty;
                //dodaj informację o z załącznikach w/g ustawionych flag
                //if (item["colVAT_VAT-UE_Zalaczony"] != null ? (bool)item["colVAT_VAT-UE_Zalaczony"] : false)
                //{
                //    info2 = info2 + string.Format(templateR, "VAT-UE");
                //}
                //if (item["colVAT_VAT_x002d_27_Zalaczony0"] != null ? (bool)item["colVAT_VAT_x002d_27_Zalaczony0"] : false)
                //{
                //    info2 = info2 + string.Format(templateR, "VAT-27");
                //}

                if ((item["colDrukWplaty"] != null ? (bool)item["colDrukWplaty"] : false)
                    && IsBWAllowed) //dodawaj informację o załącznikach tylko w przypadku płatności VAT
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

                odbiorca = Check_NieWysylacDoKlientaFlag(item, nadawca, odbiorca);

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);

                //obsługa remindera
                if (hasPrzypomnienieOTerminiePlatnosci(item))
                {
                    KopiaDoNadawcy = false;
                    KopiaDoBiura = false;

                    DateTime terminPlatnosci = Get_Date(item, "colVAT_TerminPlatnosciPodatku");

                    if (Get_String(item, "colVAT_Decyzja") == "Do zapłaty")
                    {
                        if (GetValue(item, "colVAT_WartoscDoZaplaty") > 0)
                        {
                            //ustaw reminder
                            nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_DO_ZAPLATY_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                            temat = Update_Data(temat, terminPlatnosci);
                            temat = BLL.Tools.AddCompanyName(temat, item);

                            //leading reminder text
                            string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_LEADING_REMINDER_TEXT", false);
                            lrt = lrt.Replace("___FIRMA___", firma);
                            lrt = lrt.Replace("___OKRES___", okres);
                            trescHTML = trescHTML.Replace("___VAT_LEADING_REMINDER_TEXT___", lrt);

                            //trailing reminder text
                            string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "VAT_TRAILING_REMINDER_TEXT", false);
                            trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                            trescHTML = trescHTML.Replace("___VAT_TRAILING_REMINDER_TEXT___", trt);

                            //aktualizacja danych z tabelki
                            sb = new StringBuilder(trescHTML);
                            sb.Replace("___colFormaOpodatkowaniaVAT___", item["colFormaOpodatkowaniaVAT"] != null ? item["colFormaOpodatkowaniaVAT"].ToString() : string.Empty);
                            sb.Replace("___colVAT_WartoscDoZaplaty___", Format_Currency(item, "colVAT_WartoscDoZaplaty"));
                            sb.Replace("___colVAT_Konto___", item["colVAT_Konto"] != null ? item["colVAT_Konto"].ToString() : string.Empty);
                            sb.Replace("___colVAT_TerminPlatnosciPodatku___", item["colVAT_TerminPlatnosciPodatku"] != null ? DateTime.Parse(item["colVAT_TerminPlatnosciPodatku"].ToString()).ToShortDateString() : string.Empty);

                            trescHTML = sb.ToString();

                            planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);

                            //nie wysyłaj przypomnienia jeżeli krócej niż 3 dni do terminu
                            if (planowanaDataNadania.CompareTo(DateTime.Now.AddDays(3)) > 0)
                            {
                                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
                            }
                        }
                    }
                }
            }
        }

        private void Manage_CMD_WyslijWynik_RBR(SPListItem item)
        {
            string cmd = GetCommand(item);
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == ZATWIERDZ)
            {
                //string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;

                //string nadawca = Get_WlacicielZadania(item);
                //string kopiaDla = Get_KopiaDlaEdytora(item, nadawca);

                string nadawca = Get_CurrentUser(item);
                //string kopiaDla = Get_WlacicielZadania(item);
                string kopiaDla = string.Empty;

                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, new SPFieldLookupValue(item["selKlient"].ToString()).LookupId);


                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_TEMPLATE.Include", out temat, out trescHTML, nadawca);

                string lt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_LEADING_TEXT", false);
                string firma = BLL.tabKlienci.Get_NazwaFirmyById(item.Web, klientId);
                lt = lt.Replace("___FIRMA___", firma);
                string okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;
                lt = lt.Replace("___OKRES___", okres);
                trescHTML = trescHTML.Replace("___RBR_LEADING_TEXT___", lt);

                //uzupełnia temat kodem klienta i okresu
                temat = AddSpecyfikacja(item, temat, string.Empty);

                //uzupełnia dane w formatce BR_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colBR_NumerFaktury___", item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty);
                sb.Replace("___colBR_DataWystawienia___", Format_Date(item, "colBR_DataWystawieniaFaktury"));
                sb.Replace("___colBR_WartoscDoZaplaty___", BLL.Tools.Format_Currency(item["colBR_WartoscDoZaplaty"] != null ? double.Parse(item["colBR_WartoscDoZaplaty"].ToString()) : 0));
                sb.Replace("___colBR_Konto___", item["colBR_Konto"] != null ? item["colBR_Konto"].ToString() : string.Empty);
                sb.Replace("___colBR_TerminPlatnosci___", Format_Date(item, "colBR_TerminPlatnosci"));

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

                BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);

                //obsługa remindera
                if (hasPrzypomnienieOTerminiePlatnosci(item))
                {
                    KopiaDoNadawcy = false;
                    KopiaDoBiura = false;

                    DateTime terminPlatnosci = Get_Date(item, "colBR_TerminPlatnosci");


                    if (GetValue(item, "colBR_WartoscDoZaplaty") > 0)
                    {
                        //ustaw reminder
                        nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                        temat = Update_Data(temat, terminPlatnosci);
                        temat = BLL.Tools.AddCompanyName(temat, item);

                        //leading reminder text
                        string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_LEADING_REMINDER_TEXT", false);
                        lrt = lrt.Replace("___FIRMA___", firma);
                        lrt = lrt.Replace("___OKRES___", okres);
                        trescHTML = trescHTML.Replace("___RBR_LEADING_REMINDER_TEXT___", lrt);

                        //trailing reminder text
                        string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_TRAILING_REMINDER_TEXT", false);
                        trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                        trescHTML = trescHTML.Replace("___RBR_TRAILING_REMINDER_TEXT___", trt);

                        //aktualizacja danych z tabelki
                        sb = new StringBuilder(trescHTML);
                        sb.Replace("___colBR_NumerFaktury___", item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty);
                        sb.Replace("___colBR_DataWystawienia___", Format_Date(item, "colBR_DataWystawieniaFaktury"));
                        sb.Replace("___colBR_WartoscDoZaplaty___", Format_Currency(item, "colBR_WartoscDoZaplaty"));
                        sb.Replace("___colBR_Konto___", item["colBR_Konto"] != null ? item["colBR_Konto"].ToString() : string.Empty);
                        sb.Replace("___colBR_TerminPlatnosci___", Format_Date(item, "colBR_TerminPlatnosci"));

                        trescHTML = sb.ToString();

                        planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);


                        BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
                    }

                }
            }
        }

        private static string Get_KopiaDlaEdytora(SPListItem item, string nadawca)
        {
            string result = string.Empty;

            SPUser user = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User;
            //sprawdź przypisanie adresu na liście operatorów
            int operatorId = BLL.dicOperatorzy.Get_OperatorIdByLoginName(item.Web, user.LoginName);
            if (operatorId > 0)
            {
                result = BLL.dicOperatorzy.Get_EmailById(item.Web, operatorId);
            }

            if (result == nadawca)
            {
                //jeżeli operator nie ma przypisanego adresu mailowego lub pokrywa się z adresem nadawcy
                return string.Empty;
            }

            return result;
        }


        private bool isValidated_ZUS(SPListItem item)
        {
            Set_ValidationFlag(item, false);

            //oczyść dane w zależności od wybranej Decyzji
            bool zpFlag = BLL.Tools.Get_Flag(item, "colZatrudniaPracownikow");
            string opcja = BLL.Tools.Get_Text(item, "colZUS_Opcja");
            if (string.IsNullOrEmpty(opcja))
            {
                return false;
            }

            if (zpFlag) //zatrudnia pracowników
            {
                switch (opcja)
                {
                    case "Tylko zdrowotna":

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

                            if (foundError)
                            {
                                Set_ValidationFlag(item, true);
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            Add_Comment(item, "Niedozwolona ujemna wartość składki zdrowotnej");
                            Set_ValidationFlag(item, true);
                            return false;
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

                            if (BLL.Tools.Get_Flag(item, "colZUS_PIT-8AR_Zalaczony")
                                && GetValue(item, "colZUS_PIT-4R") < 0)
                            {
                                Add_Comment(item, "Niedozolona ujemna wartość PIT-4R");
                                foundError = true;
                            }
                            if (BLL.Tools.Get_Flag(item, "colZUS_PIT-8AR_Zalaczony")
                                && GetValue(item, "colZUS_PIT-8AR") < 0)
                            {
                                Add_Comment(item, "Niedozolona ujemna wartość PIT-8AR");
                                foundError = true;
                            }

                            if (foundError)
                            {
                                Set_ValidationFlag(item, true);
                                return false;
                            }

                            return true;
                        }
                        else
                        {
                            Add_Comment(item, "Niedozwolona ujemna wartość składki");
                            Set_ValidationFlag(item, true);
                            return false;
                        }
                        break;
                }

            }
            else //nie zatrudnia pracowników
            {
                BLL.Tools.Clear_Flag(item, "colZUS_PIT-4R_Zalaczony");
                BLL.Tools.Clear_Value(item, "colZUS_PIT-4R");
                BLL.Tools.Clear_Flag(item, "colZUS_PIT-8AR_Zalaczony");
                BLL.Tools.Clear_Value(item, "colZUS_PIT-8AR");

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

                    if (foundError)
                    {
                        Set_ValidationFlag(item, true);
                        return false;
                    }

                    return true;
                }
                else
                {
                    Add_Comment(item, "Niedozwolona ujemna wartość składki");
                    Set_ValidationFlag(item, true);
                    return false;
                }
            }
        }

        private bool isValidated_PD(SPListItem item)
        {
            Set_ValidationFlag(item, false);

            //oczyść dane w zależności od wybranej Decyzji
            string ocena = Get_String(item, "colPD_OcenaWyniku");
            if (string.IsNullOrEmpty(ocena))
            {
                Add_Comment(item, "brak oceny wyniku");
                Set_ValidationFlag(item, true);
                return false;
            }

            switch (ocena)
            {
                case "Dochód":
                    BLL.Tools.Clear_Value(item, "colPD_WartoscStraty");

                    if (GetValue(item, "colPD_WartoscDoZaplaty") >= 0
                        && GetValue(item, "colPD_WartoscDochodu") >= 0)
                        if (!string.IsNullOrEmpty(Get_String(item, "colPD_Konto"))) return true;
                        else
                        {
                            Add_Comment(item, "brak numeru konta");
                            Set_ValidationFlag(item, true);
                        }
                    break;
                case "Strata":
                    BLL.Tools.Clear_Value(item, "colPD_WartoscDochodu");
                    BLL.Tools.Clear_Value(item, "colPD_WartoscDoZaplaty");

                    if (GetValue(item, "colPD_WartoscStraty") > 0) return true;
                    else
                    {
                        Add_Comment(item, "wartość straty musi być większa niż 0");
                        Set_ValidationFlag(item, true);
                    }
                    break;
                default:
                    Add_Comment(item, "niedozwolona wartość pola ocena wyniku");
                    break;
            }

            Set_ValidationFlag(item, true);
            return false;
        }

        private void Set_ValidationFlag(SPListItem item, bool flag)
        {
            BLL.tabZadania.Set_ValidationFlag(item, flag);
        }

        private bool isValidated_PDS(SPListItem item)
        {
            int klientId = BLL.Tools.Get_LookupId(item, "selKlient");

            //wszystkie warunki dla PD powinny być spełnione dla PDS
            bool result = isValidated_PD(item);
            if (result)
            {
                //dodatkowe warunki do sprawdzenia dla PDS
                bool foundError = false;

                // koszty NKUP

                if (BLL.Tools.Get_Flag(item, "colKosztyNKUP"))
                {
                    if (GetValue(item, "colKosztyNKUP_WynWyl") < 0
                        | GetValue(item, "colKosztyNKUP_ZUSPlatWyl") < 0
                        | GetValue(item, "colKosztyNKUP_FakWyl") < 0
                        | GetValue(item, "colKosztyNKUP_PozostaleKoszty") < 0)
                    {
                        Add_Comment(item, "Niedozolone ujemne wartości w sekcji Koszty NKUP");
                        foundError = true;
                    }
                }
                else
                {
                    BLL.Tools.Clear_Value(item, "colKosztyNKUP_WynWyl");
                    BLL.Tools.Clear_Value(item, "colKosztyNKUP_ZUSPlatWyl");
                    BLL.Tools.Clear_Value(item, "colKosztyNKUP_FakWyl");
                    BLL.Tools.Clear_Value(item, "colKosztyNKUP_PozostaleKoszty");
                }

                // Przychody WS

                if (BLL.Tools.Get_Flag(item, "colKosztyWS"))
                {
                    if (GetValue(item, "colKosztyWS_WynWlaczone") < 0
                        | GetValue(item, "colKosztyWS_ZUSPlatWlaczone") < 0
                        | GetValue(item, "colKosztyWS_FakWlaczone") < 0)
                    {
                        Add_Comment(item, "Niedozolone ujemne wartości w sekcji Koszty WS");
                        foundError = true;
                    }
                }
                else
                {
                    BLL.Tools.Clear_Value(item, "colKosztyWS_WynWlaczone");
                    BLL.Tools.Clear_Value(item, "colKosztyWS_ZUSPlatWlaczone");
                    BLL.Tools.Clear_Value(item, "colKosztyWS_FakWlaczone");
                }

                // Przychody NP

                if (BLL.Tools.Get_Flag(item, "colPrzychodyNP"))
                {
                    if (GetValue(item, "colPrzychodyNP_DywidendySpO") < 0
                        | GetValue(item, "colPrzychodyNP_Inne") < 0)
                    {
                        Add_Comment(item, "Niedozolone ujemne wartości w sekcji Przychody NP");
                        foundError = true;
                    }
                }
                else
                {
                    BLL.Tools.Clear_Value(item, "colPrzychodyNP_DywidendySpO");
                    BLL.Tools.Clear_Value(item, "colPrzychodyNP_Inne");
                }

                // rozwinięcie walidatora : weryfikacja kosztów z przeniesienia czy nie są mniejsze niż były

                //jeżeli bieżący miesiąc > styczeń to kopuj dane z poprzedniej karty odpowiednio w/g trybu (miesięcznie/kwartalnie)

                int okresId = BLL.Tools.Get_LookupId(item, "selOkres");

                SPListItem okres = BLL.tabOkresy.Get_OkresById(item.Web, okresId);
                DateTime dataRozpoczecia = BLL.Tools.Get_Date(okres, "colDataRozpoczecia");
                if (dataRozpoczecia.Month > 1)
                {
                    //wyszukaj źródłową kartę kontrolną

                    bool trybKwartalny = false;
                    if (BLL.Tools.Get_Text(item, "enumRozliczeniePD").Equals("Kwartalnie")) trybKwartalny = true;

                    DateTime targetStartDate = BLL.Tools.Get_TargetStartDate(trybKwartalny, dataRozpoczecia);

                    if (!targetStartDate.Equals(new DateTime()))
                    {
                        SPListItem targetOkres = BLL.tabOkresy.Get_OkresByStartDate(item.Web, targetStartDate);

                        int targetOkresId = 0;

                        if (targetOkres != null) targetOkresId = targetOkres.ID;

                        if (!targetOkresId.Equals(0))
                        {
                            //znajdź kartę kontrolną

                            SPListItem kk = BLL.tabKartyKontrolne.Get_KartaKontrolna(item.Web, klientId, targetOkresId);

                            if (kk == null) { }//dane niedostępne
                            else
                            {
                                //znalazł dane do porównania na karcie kk

                                //Koszty NKUP

                                if (BLL.Tools.Get_Flag(item, "colKosztyNKUP"))
                                {
                                    if (Check_IsLowerValue(item, kk, "colKosztyNKUP_WynWyl"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Wynagrodzenia wyłączone' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colKosztyNKUP_ZUSPlatWyl"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'ZUS płatnika wyłączony' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colKosztyNKUP_FakWyl"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Faktury wyłączone' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colKosztyNKUP_PozostaleKoszty"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Pozostale koszty NKUP' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }
                                }

                                if (BLL.Tools.Get_Flag(item, "colKosztyWS"))
                                {
                                    if (Check_IsLowerValue(item, kk, "colKosztyWS_WynWlaczone"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Wynagrodzenia włączone' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colKosztyWS_ZUSPlatWlaczone"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'ZUS płatnika włączony' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colKosztyWS_FakWlaczone"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Faktury włączone' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }
                                }

                                if (BLL.Tools.Get_Flag(item, "colPrzychodyNP"))
                                {
                                    if (Check_IsLowerValue(item, kk, "colPrzychodyNP_DywidendySpO"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Dywidendy ze spółek osobowych' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }

                                    if (Check_IsLowerValue(item, kk, "colPrzychodyNP_Inne"))
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Przychody inne' mniejsza niż w poprzednim okresie");
                                        foundError = true;
                                    }
                                }

                                // Wpłacona składka zdrowotna
                                if (Check_IsLowerValue(item, kk, "colWplaconaSZ"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Wpłacona składka zdrowotna' mniejsza niż w poprzednim okresie");
                                    foundError = true;
                                }
                                // Wpłacone zaliczki od początku roku
                                if (Check_IsLowerValue(item, kk, "colWplaconeZaliczkiOdPoczatkuRok"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Wpłacone zaliczki od początku roku' mniejsza niż w poprzednim okresie");
                                    foundError = true;
                                }
                                // Strata do odliczenia
                                if (Check_IsNotEqual(item, kk, "colStrataDoOdliczenia"))
                                {
                                    if (!BLL.Tools.Get_Flag(item, "_IsSpolkaZoo"))
                                    {
                                        // dla spółek osobowych strata powinna być 0
                                        Add_Comment(item, "Wartość w pozycji 'Strata do odliczenia' dla spółek osobowych powinna być 0 (jest "
                                            + BLL.Tools.Get_Value(item, "colStrataDoOdliczenia") + ")");
                                        foundError = true;
                                    }
                                    else
                                    {
                                        Add_Comment(item, "Wartość w pozycji 'Strata do odliczenia' niezgodna z wartością z poprzedniego okresu");
                                        foundError = true;
                                    }
                                }
                                // Strona Winien
                                //if (Check_IsLowerValue(item, kk, "colStronaWn"))
                                //{
                                //    Add_Comment(item, "Wartość w pozycji 'Strona Wn' mniejsza niż w poprzednim okresie");
                                //    foundError = true;
                                //}
                                // Strona Ma
                                //if (Check_IsLowerValue(item, kk, "colStronaMa"))
                                //{
                                //    Add_Comment(item, "Wartość w pozycji 'Strona Ma' mniejsza niż w poprzednim okresie");
                                //    foundError = true;
                                //}
                            }
                        }
                    }
                }

                //sprawdzenie czy zgadza się suma odliczeń z lat poprzenidnich

                double sumaDoOdliczeniaZRejestru = BLL.tabStratyZLatUbieglych.Get_SumaDoOdliczenia(item.Web, klientId, okresId);
                double sumaDoOdliczenia = BLL.Tools.Get_Value(item, "colStrataDoOdliczenia");

                if (sumaDoOdliczenia != sumaDoOdliczeniaZRejestru)
                {
                    Add_Comment(item, "Strata do odliczenia (" + sumaDoOdliczenia.ToString() + ") nie zgadza się z rejestrem (" + sumaDoOdliczeniaZRejestru.ToString() + ")");
                    foundError = true;
                }

                //oblicz "podstawa do opodatkowania (kalkulacja na formularzu).

                //double sumNKUP = BLL.Tools.Get_Value(item, "colKosztyNKUP_WynWyl")
                //    + BLL.Tools.Get_Value(item, "colKosztyNKUP_ZUSPlatWyl")
                //    + BLL.Tools.Get_Value(item, "colKosztyNKUP_FakWyl")
                //    + BLL.Tools.Get_Value(item, "colKosztyNKUP_PozostaleKoszty");

                //double sumWS = BLL.Tools.Get_Value(item, "colKosztyWS_WynWlaczone")
                //    + BLL.Tools.Get_Value(item, "colKosztyWS_ZUSPlatWlaczone")
                //    + BLL.Tools.Get_Value(item, "colKosztyWS_FakWlaczone");

                //double sumPN = BLL.Tools.Get_Value(item, "colPrzychodyNP_DywidendySpO")
                //    + BLL.Tools.Get_Value(item, "colPrzychodyNP_Inne");

                //double colDochodStrataZInnychSp = BLL.Tools.Get_Value(item, "colDochodStrataZInnychSp");

                //double colPrzychodyZwolnione = BLL.Tools.Get_Value(item, "colPrzychodyZwolnione"); //czy to do czegoś jest potrzebne?

                //double zsn = 0;
                //if (BLL.Tools.Get_Text(item, "colPD_OcenaWyniku").Equals("Dochód")) zsn = zsn + BLL.Tools.Get_Value(item, "colPD_WartoscDochodu");
                //if (BLL.Tools.Get_Text(item, "colPD_OcenaWyniku").Equals("Strata")) zsn = zsn - 1 * BLL.Tools.Get_Value(item, "colPD_WartoscStraty");

                //double zyskStrataNetto = zsn - sumNKUP + sumWS + sumPN - colDochodStrataZInnychSp;

                //if (!BLL.Tools.Get_Value(item, "colZyskStrataNetto").Equals(zyskStrataNetto))
                //{
                //    Add_Comment(item, string.Format(@"Nieprawidłowo obliczona wartość ZyskStrata Netto. Powinno być {0}, jest {1}",
                //                        zyskStrataNetto.ToString(),
                //                        BLL.Tools.Get_Value(item, "colZyskStrataNetto").ToString()));
                //    foundError = true;

                //    //ustaw prawidłową wartość - roboczo
                //    BLL.Tools.Set_Value(item, "colZyskStrataNetto", zyskStrataNetto);
                //}

                //sprawdź udziały wspólników
                double sumaUdzialow = BLL.tabDochodyWspolnikow.Sum_UdzalyWspolnikow(item.Web, klientId, okresId) * 100;

                if (!BLL.Tools.Get_Flag(item, "_IsSpolkaZoo")) //jeżeli spółka osobowa wtedy sprawdź sumę udziałów wspólników
                {
                    if (sumaUdzialow == 0)
                    {
                        // ta spółka nie ma zdefiniowanych wspólników 
                        Add_Comment(item, "Spółka osobowa nie ma zdefiniowanych wspólników (suma udziałów wspólników = 0%)");
                        foundError = true;
                    }
                    else
                    {
                        // ta spółka ma wspólników - weryfikuj
                        if (sumaUdzialow != 100)
                        {
                            Add_Comment(item, "Suma udziałów wspólników (" + sumaUdzialow.ToString() + "%) nie jest równa 100%");
                            foundError = true;
                        }
                    }
                }
                else
                {
                    if (sumaUdzialow > 0)
                    {
                        Add_Comment(item, "Spółka kapitałowa nie powinna mieć zdefiniowanych wspólników (suma udziałów = " + sumaUdzialow.ToString() + "%)");
                        foundError = true;
                    }
                }


                // sprawdź czy zysk-strata netto = strona winien - strona ma >> podać różnice

                double stronaWn = BLL.Tools.Get_Value(item, "colStronaWn");
                double stronaMa = BLL.Tools.Get_Value(item, "colStronaMa");

                double stronaWn_Ma = BLL.Tools.Get_Value(item, "colStronaWn-StronaMa");

                if (stronaWn_Ma != stronaWn - stronaMa)
                {
                    Add_Comment(item, string.Format(@"Kalkulacja StonaWn-StronaMa nieprawidłowa. Jest ({0}), powinno być ({1})", stronaWn_Ma.ToString(), (stronaWn - stronaMa).ToString()));
                    //popraw roboczo
                    stronaWn_Ma = stronaWn - stronaMa;
                    BLL.Tools.Set_Value(item, "colStronaWn-StronaMa", stronaWn_Ma);

                    foundError = true;
                }



                if (!BLL.Tools.Get_Value(item, "colZyskStrataNetto").Equals(stronaWn_Ma))
                {
                    Add_Comment(item, string.Format(@"Zysk-Strata Netto ({0}) nie równa się Strona Winien-Strona Ma ({1})", BLL.Tools.Get_Value(item, "colZyskStrataNetto").ToString(), stronaWn_Ma.ToString()));
                    foundError = true;
                }

                if (foundError)
                {
                    Set_ValidationFlag(item, true);
                    result = false;
                }
            }

            return result;
        }

        private bool isValidated_PDW(SPListItem item)
        {
            int klientId = BLL.Tools.Get_LookupId(item, "selKlient");

            //wszystkie warunki dla PD powinny być spełnione dla PDW
            bool result = isValidated_PD(item);
            if (result)
            {
                bool foundError = false;
                if (GetValue(item, "colWplaconaSZ") < 0)
                {
                    Add_Comment(item, "Pozycja 'Wpłacona składka zdrowotna' nie może być ujemna");
                    foundError = true;
                }
                if (GetValue(item, "colWplaconeZaliczkiOdPoczatkuRoku") < 0)
                {
                    Add_Comment(item, "Pozycja 'Wpłacone zaliczki od początku roku' nie może być ujemna");
                    foundError = true;
                }

                //wypełnij informacje o dochodzie/stracie

                double przychod = 0;
                string przychodWyszczegolnienie = string.Empty;

                int okresId = BLL.Tools.Get_LookupId(item, "selOkres");
                int wspolnikId = BLL.Tools.Get_LookupId(item, "selKlient");

                BLL.tabDochodyWspolnikow.Get_PrzychodyWspolnika(item.Web, wspolnikId, okresId, out przychod, out przychodWyszczegolnienie);

                if (przychod >= 0)
                {
                    BLL.Tools.Set_Text(item, "colPD_OcenaWyniku", "Dochód");
                    BLL.Tools.Set_Value(item, "colPD_WartoscDochodu", przychod);
                    BLL.Tools.Clear_Value(item, "colPD_WartoscStraty");
                }
                else
                {
                    BLL.Tools.Set_Text(item, "colPD_OcenaWyniku", "Strata");
                    BLL.Tools.Set_Value(item, "colPD_WartoscStraty", -1 * przychod);
                    BLL.Tools.Clear_Value(item, "colPD_WartoscDochodu");
                }

                BLL.Tools.Set_Text(item, "_Specyfikacja", przychodWyszczegolnienie);


                //jeżeli bieżący miesiąc > styczeń to kopuj dane z poprzedniej karty odpowiednio w/g trybu (miesięcznie/kwartalnie)

                SPListItem okres = BLL.tabOkresy.Get_OkresById(item.Web, okresId);
                DateTime dataRozpoczecia = BLL.Tools.Get_Date(okres, "colDataRozpoczecia");
                if (dataRozpoczecia.Month > 1)
                {
                    //wyszukaj źródłową kartę kontrolną

                    bool trybKwartalny = false;
                    if (BLL.Tools.Get_Text(item, "enumRozliczeniePD").Equals("Kwartalnie")) trybKwartalny = true;

                    DateTime targetStartDate = BLL.Tools.Get_TargetStartDate(trybKwartalny, dataRozpoczecia);

                    if (!targetStartDate.Equals(new DateTime()))
                    {
                        SPListItem targetOkres = BLL.tabOkresy.Get_OkresByStartDate(item.Web, targetStartDate);

                        int targetOkresId = 0;

                        if (targetOkres != null) targetOkresId = targetOkres.ID;

                        if (!targetOkresId.Equals(0))
                        {
                            //znajdź kartę kontrolną

                            SPListItem kk = BLL.tabKartyKontrolne.Get_KartaKontrolna(item.Web, klientId, targetOkresId);

                            if (kk == null) { }//dane niedostępne
                            else
                            {
                                // znalazł dane do porównania na karcie kk

                                // Nieuwzględniona w skosztach składka społeczna
                                if (Check_IsLowerValue(item, kk, "colNieuwzglednionaSkladkaSpolecz"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Nieuwzględniona w kosztach składka społeczna' mniejsza niż w poprzednim okresie");
                                    foundError = true;
                                }
                                // Wpłacona składka zdrowotna
                                if (Check_IsLowerValue(item, kk, "colWplaconaSZ"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Wpłacona składka zdrowotna' mniejsza niż w poprzednim okresie");
                                    foundError = true;
                                }
                                // Wpłacone zaliczki od początku roku
                                if (Check_IsLowerValue(item, kk, "colWplaconeZaliczkiOdPoczatkuRok"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Wpłacone zaliczki od początku roku' mniejsza niż w poprzednim okresie");
                                    foundError = true;
                                }
                                // Strata do odliczenia
                                if (Check_IsNotEqual(item, kk, "colStrataDoOdliczenia"))
                                {
                                    Add_Comment(item, "Wartość w pozycji 'Strata do odliczenia' niezgodna z wartością z poprzedniego okresu");
                                    foundError = true;
                                }
                            }
                        }
                    }
                }

                // report results
                if (foundError)
                {
                    Set_ValidationFlag(item, true);
                    result = false;
                }
            }

            return result;
        }

        private bool Check_IsLowerValue(SPListItem item, SPListItem kk, string col)
        {
            if (item[col] != null && kk[col] != null)
            {
                double v1 = BLL.Tools.Get_Value(item, col);
                double v0 = BLL.Tools.Get_Value(kk, col);

                if (v1 < v0) return true;
            }

            return false;
        }

        private bool Check_IsNotEqual(SPListItem item, SPListItem kk, string col)
        {
            if (item[col] != null && kk[col] != null)
            {
                double v1 = BLL.Tools.Get_Value(item, col);
                double v0 = BLL.Tools.Get_Value(kk, col);

                if (v1 != v0) return true;
            }

            return false;
        }

        private bool isValidated_VAT(SPListItem item)
        {
            Set_ValidationFlag(item, false);

            //oczyść dane w zależności od wybranej Decyzji
            string decyzja = item["colVAT_Decyzja"] != null ? item["colVAT_Decyzja"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(decyzja))
            {
                return false;
            }

            switch (decyzja)
            {
                case "Do zapłaty":
                    //BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZaplaty");
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoPrzeniesienia");
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoZaplaty") >= 0)
                        if (!string.IsNullOrEmpty(Get_String(item, "colVAT_Konto"))) return true;
                        else Add_Comment(item, "brak numeru konta");
                    else
                    {
                        Add_Comment(item, "Wartość do zapłaty nie może być ujemna");
                    }
                    break;
                case "Do przeniesienia":
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZaplaty");
                    //BLL.Tools.Clear_Value(item, "colVAT_WartoscDoPrzeniesienia");
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoPrzeniesienia") >= 0) return true;
                    else
                    {
                        Add_Comment(item, "Wartość do przeniesienia nie może być ujemna");
                    }
                    break;
                case "Do zwrotu":
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZaplaty");
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoPrzeniesienia");
                    //BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoZwrotu") >= 0) return true;
                    else
                    {
                        Add_Comment(item, "Wartość do zwrotu nie może być ujemna");
                    }
                    break;
                case "Do przeniesienia i do zwrotu":
                    BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZaplaty");
                    //BLL.Tools.Clear_Value(item, "colVAT_WartoscDoPrzeniesienia");
                    //BLL.Tools.Clear_Value(item, "colVAT_WartoscDoZwrotu");

                    if (GetValue(item, "colVAT_WartoscDoPrzeniesienia") >= 0
                        && GetValue(item, "colVAT_WartoscDoZwrotu") >= 0) return true;
                    else
                    {
                        Add_Comment(item, "Niedozwolone wartości ujemne");
                    }
                    break;
                default:
                    break;
            }

            Set_ValidationFlag(item, true);
            return false;
        }

        private void Add_Comment(SPListItem item, string comment)
        {
            if (vm != null) vm.AppendFormat(@"<li>{0}</li>", comment);

            string uwagi = Get_String(item, "colUwagi");
            uwagi = uwagi + "\n" + DateTime.Now.ToString() + "\n" + comment;
            item["colUwagi"] = uwagi.Trim();
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
                //item.SystemUpdate();
                return 0;
            }
        }

        private bool isValidated_RBR(SPListItem item)
        {
            Set_ValidationFlag(item, false);

            StringBuilder sb = new StringBuilder();

            bool foundErrors = false;

            if (string.IsNullOrEmpty(Get_String(item, "colBR_DataWystawieniaFaktury")))
            {
                foundErrors = true;
                sb.AppendLine(@"brak daty wystawienia faktury");
            }

            if (string.IsNullOrEmpty(Get_String(item, "colBR_NumerFaktury")))
            {
                foundErrors = true;
                sb.AppendLine(@"brak numeru faktury");
            }

            if (GetValue(item, "colBR_WartoscDoZaplaty") <= 0)
            {
                foundErrors = true;
                sb.AppendLine(@"nieprawidłowa wartość do zapłaty");
            }
            if (item["colBR_FakturaZalaczona"] != null ? (bool)item["colBR_FakturaZalaczona"] : false)
            {
                if (item.Attachments.Count == 0)
                {
                    foundErrors = true;
                    sb.AppendLine(@"brak załącznika");
                }
            }

            if (string.IsNullOrEmpty(Get_String(item, "colBR_TerminPlatnosci")))
            {
                foundErrors = true;
                sb.AppendLine(@"brak terminu płatności faktury");
            }

            if (!foundErrors) return true;
            else
            {
                Add_Comment(item, sb.ToString());
                Set_ValidationFlag(item, true);
                return false;
            }

        }

        #endregion

        #region Potwierdzenie odbioru dokumentów
        private void Manage_PotwierdzenieOdbioruDokumentow(SPListItem item)
        {

        }
        #endregion

        #region Helpers
        private bool Get_FlagValue(SPListItem item, string col)
        {
            return item[col] != null ? bool.Parse(item[col].ToString()) : false;
        }

        private DateTime Get_Date(SPListItem item, string col)
        {
            return item[col] != null ? DateTime.Parse(item[col].ToString()) : new DateTime();
        }
        private static string Check_NieWysylacDoKlientaFlag(SPListItem item, string nadawca, string odbiorca)
        {
            bool czyNieWysylacDoKlienta = item["colNieWysylajDoKlienta"] != null ? (bool)item["colNieWysylajDoKlienta"] : false;
            if (czyNieWysylacDoKlienta)
            {
                odbiorca = nadawca;
            }
            return odbiorca;
        }
        private string AddSpecyfikacja(SPListItem item, string temat, string okres)
        {
            if (string.IsNullOrEmpty(okres))
            {
                okres = item["selOkres"] != null ? new SPFieldLookupValue(item["selOkres"].ToString()).LookupValue : string.Empty;
            }
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

        private void Set_StatusZadania(SPListItem item, StatusZadania statusZadania)
        {
            item["enumStatusZadania"] = statusZadania.ToString();
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
                    //item.SystemUpdate();
                }

            }
        }

        private void ResetCommand(SPListItem item, bool clearInformacjaDlaKlienta)
        {
            item["cmdFormatka"] = string.Empty; //czyszczenie komendy

            if (clearInformacjaDlaKlienta
                && item["colInformacjaDlaKlienta"] != null)
            {
                item["colInformacjaDlaKlienta"] = string.Empty;
            }
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
                    //item.SystemUpdate();

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
                    //item.SystemUpdate();
                }
            }
        }

        private static int Assign_ProceduraBasedOnTitle(SPListItem item, SPWeb web)
        {
            int procId = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupId : 0;
            if (procId == 0)
            {
                procId = BLL.tabProcedury.Ensure(web, item.Title);
                item["selProcedura"] = procId;
                //item.SystemUpdate();
            }
            return procId;
        }
        private bool Get_Flag(SPListItem item, string col)
        {
            return item[col] != null ? bool.Parse(item[col].ToString()) : false;
        }

        #endregion

        #region Error Handlers
        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fa = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fa != null)
            {
                Debug.WriteLine("*********************************************");
                Debug.WriteLine(fa.Fault.Source);
                Debug.WriteLine(fa.Fault.Message);
                Debug.WriteLine(fa.Fault.StackTrace);

                logErrorMessage_HistoryDescription = string.Format("{0}::{1}",
                    fa.Fault.Message,
                    fa.Fault.StackTrace);

                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fa.Fault.Message, fa.Fault.StackTrace);
            }
        }
        #endregion

        private void Get_CT_ExecuteCode(object sender, EventArgs e)
        {
            switch (item.ContentType.Name)
            {
                case "Prośba o dokumenty":
                    zadanieCT = ZadanieCT.POD;
                    break;
                case "Prośba o przesłanie wyciągu bankowego":
                    zadanieCT = ZadanieCT.POPWB;
                    break;
                case "Rozliczenie ZUS":
                    zadanieCT = ZadanieCT.RZ;
                    break;
                case "Rozliczenie podatku dochodowego":
                    zadanieCT = ZadanieCT.RPD;
                    break;
                case "Rozliczenie podatku dochodowego spółki":
                    zadanieCT = ZadanieCT.RPDS;
                    break;
                case "Rozliczenie podatku dochodowego wspólnika":
                    zadanieCT = ZadanieCT.RPDW;
                    break;
                case "Rozliczenie podatku VAT":
                    zadanieCT = ZadanieCT.RPV;
                    break;
                case "Rozliczenie z biurem rachunkowym":
                    zadanieCT = ZadanieCT.RZBR;
                    break;
                case "Zadanie":
                    zadanieCT = ZadanieCT.Z;
                    break;
                case "Wiadomość z ręki":
                    zadanieCT = ZadanieCT.WZR;
                    break;
                case "Wiadomość z szablonu":
                    zadanieCT = ZadanieCT.WZS;
                    break;
                case "Wiadomość grupowa":
                    zadanieCT = ZadanieCT.WG;
                    break;
                case "Wiadomość grupowa z szablonu":
                    zadanieCT = ZadanieCT.WGZS;
                    break;
            }
        }

        private void ifZ(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.Z)) e.Result = true;
        }

        private void ifKomunikat(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.POD)
                | zadanieCT.Equals(ZadanieCT.POPWB)) e.Result = true;
        }

        private void ifPOD(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.POD)) e.Result = true;
        }

        private void ifPOPWB(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.POPWB)) e.Result = true;
        }

        private void ifFormatki(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RZ)
                | zadanieCT.Equals(ZadanieCT.RPD)
                | zadanieCT.Equals(ZadanieCT.RPDS)
                | zadanieCT.Equals(ZadanieCT.RPDW)
                | zadanieCT.Equals(ZadanieCT.RPV)
                | zadanieCT.Equals(ZadanieCT.RZBR)) e.Result = true;
        }

        private void ifRZ(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RZ)) e.Result = true;
        }

        private void ifRPD(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RPD)) e.Result = true;
        }

        private void ifRPDS(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RPDS)) e.Result = true;
        }

        private void ifRPDW(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RPDW)) e.Result = true;
        }

        private void ifRPV(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RPV)) e.Result = true;
        }

        private void ifRZBR(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.RZBR)) e.Result = true;
        }

        private void ifWiadomosci(object sender, ConditionalEventArgs e)
        {
            if (zadanieCT.Equals(ZadanieCT.WZR)
                | zadanieCT.Equals(ZadanieCT.WZS)
                | zadanieCT.Equals(ZadanieCT.WG)
                | zadanieCT.Equals(ZadanieCT.WGZS)) e.Result = true;
        }

        private void Set_KEY_ExecuteCode(object sender, EventArgs e)
        {
            string key = BLL.tabZadania.Define_KEY(item);
            BLL.Tools.Set_Text(item, "KEY", key);
        }

        private void Set_Zadanie_ExecuteCode(object sender, EventArgs e)
        {
            //przypisz procedurę na podstawie tematu
            int procId = Assign_ProceduraBasedOnTitle(item, item.Web);

            //update termin realizacji
            Assign_TerminRealizacjiBasedOnProcedura(item, item.Web, procId);

            //update operatora
            Assign_OperatorBasedOnProcedura(item, item.Web, procId);
        }

        private void Get_Status_ExecuteCode(object sender, EventArgs e)
        {
            string s = BLL.Tools.Get_Text(item, "enumStatusZadania");
            switch (s)
            {
                case "Nowe":
                    status = StatusZadania.Nowe;
                    break;
                case "Obsługa":
                    status = StatusZadania.Obsługa;
                    break;
                case "Gotowe":
                    status = StatusZadania.Gotowe;
                    break;
                case "Wysyłka":
                    status = StatusZadania.Wysyłka;
                    break;
                case "Zakończone":
                    status = StatusZadania.Zakończone;
                    break;
                case "Anulowane":
                    status = StatusZadania.Anulowane;
                    break;
            }
        }

        private void isActive(object sender, ConditionalEventArgs e)
        {
            if (!status.Equals(StatusZadania.Zakończone)
                && !status.Equals(StatusZadania.Anulowane)) e.Result = true;
        }


        private void isStatus_Nowe(object sender, ConditionalEventArgs e)
        {
            if (status.Equals(StatusZadania.Nowe)) e.Result = true;
        }

        private void Set_Status_Obsluga_ExecuteCode(object sender, EventArgs e)
        {
            Set_StatusZadania(item, StatusZadania.Obsługa);
        }

        private void Set_Operator_ExecuteCode(object sender, EventArgs e)
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
            int targetOpId = BLL.dicOperatorzy.Get_OperatorIdByLoginName(item.Web, currentUser.LoginName);

            if (targetOpId > 0)
            {
                //przypisz operatora do zadania
                item["selOperator"] = targetOpId;
                //item.SystemUpdate();
            }
        }

        private void Manage_POD_ExecuteCode(object sender, EventArgs e)
        {
            if (Get_FlagValue(item, "colPotwierdzenieOdbioruDokumento"))
            {
                int klientId = Get_LookupId(item, "selKlient");
                int okresId = Get_LookupId(item, "selOkres");

                if (klientId > 0 && okresId > 0) BLL.tabZadania.Complete_PrzypomnienieOWysylceDokumentow(item, klientId, okresId);

                if (Get_Flag(item, "colPotwierdzenieOdbioruDokumento")) BLL.tabKartyKontrolne.Update_PotwierdzenieOdbioruDokumentow(item.Web, klientId, okresId);
            }
        }

        private void Create_Message_ExecuteCode(object sender, EventArgs e)
        {
            BLL.tabWiadomosci.CreateMailMessage(item);
        }

        private void isCommandExist(object sender, ConditionalEventArgs e)
        {
            if (taskCMD != null & !taskCMD.Equals(TaskCommands.NotDefined)) e.Result = true;
        }

        private void Get_Command_ExecuteCode(object sender, EventArgs e)
        {
            switch (GetCommand(item))
            {
                case ZATWIERDZ:
                    taskCMD = TaskCommands.Zatwiedz;
                    break;
                case WYSLIJ_INFORMACJE_DO_KLIENTA:
                    taskCMD = TaskCommands.WyslijInfo;
                    break;
                case WYSLIJ_INFORMACJE_I_ZAKONCZ_ZADANIE:
                    taskCMD = TaskCommands.WyslijInfoIZakoncz;
                    break;
                case ANULUJ:
                    taskCMD = TaskCommands.Anuluj;
                    break;
                default:
                    taskCMD = TaskCommands.NotDefined;
                    break;
            }
        }

        private void isCmd_Zatwierdz(object sender, ConditionalEventArgs e)
        {
            if (taskCMD.Equals(TaskCommands.Zatwiedz)) e.Result = true;
        }

        private void isCmd_WyslijInfo(object sender, ConditionalEventArgs e)
        {
            if (taskCMD.Equals(TaskCommands.WyslijInfo)) e.Result = true;
        }

        private void isCmd_WyslijInfoIZakoncz(object sender, ConditionalEventArgs e)
        {
            if (taskCMD.Equals(TaskCommands.WyslijInfoIZakoncz)) e.Result = true;
        }

        private void isCmd_Anuluj(object sender, ConditionalEventArgs e)
        {
            if (taskCMD.Equals(TaskCommands.Anuluj)) e.Result = true;
        }

        private void Manage_Cmd_Zatwierdz_ExecuteCode(object sender, EventArgs e)
        {
            Manage_CMD_Zatwierdz(item);
            //nie usówaj informacji z pola informacje dla klienta
            ResetCommand(item, false);
        }

        private void Mange_Cmd_WyslijInfo_ExecuteCode(object sender, EventArgs e)
        {
            Manage_CMD_WyslijInfo(item);
            //wyczyść informacje dla klienta po wysyłce
            ResetCommand(item, true);
        }

        private void Manage_Cmd_WyslijInfoIZakoncz_ExecuteCode(object sender, EventArgs e)
        {
            Manage_CMD_Zatwierdz_WyslijInfo_Zadanie(item);
            //wyczyść informacje dla klienta po wysyłce
            ResetCommand(item, false);
        }

        private void Manage_Cmd_Anuluj_ExecuteCode(object sender, EventArgs e)
        {
            Manage_CMD_Anuluj(item);
            //wyczyść informacje dla klienta po wysyłce
            ResetCommand(item, false);
        }

        private void Reset_ValidationMessage_ExecuteCode(object sender, EventArgs e)
        {
            vm = new StringBuilder();
            vm1 = new StringBuilder();
        }

        private void isValidationMessageExist(object sender, ConditionalEventArgs e)
        {
            if (vm.Length > 0) e.Result = true;
        }

        public String msgSubject = default(System.String);
        public String msgTo = default(System.String);
        public String msgBody = default(System.String);
        private string _VALIDATION_MESSAGE_TEMPLATE = @"<p>Klient: [[NazwaKlienta]]</p><p>Wynik weryfikacji zadania <a href=""[[Url]]"">[[NumerZadania]]</a> negatywny</p><p>Lista zdiagnozowanych niezgodności:</p><ol>[[ListItems]] </ol>";

        private void Setup_ValidationMessage_ExecuteCode(object sender, EventArgs e)
        {
            StringBuilder vmt = new StringBuilder(_VALIDATION_MESSAGE_TEMPLATE);
            vmt.Replace("[[NazwaKlienta]]", iok.NazwaFirmy);
            vmt.Replace("[[NumerZadania]]", item.ID.ToString());
            vmt.Replace("[[Url]]", SPUtility.ConcatUrls(workflowProperties.Site.Protocol + "//" + workflowProperties.Site.HostName, item.ParentList.DefaultEditFormUrl + "?ID=" + item.ID.ToString()));
            vmt.Replace("[[ListItems]]", vm.ToString());

            msgBody = vmt.ToString();
            msgSubject = string.Format(@"Wynik weryfikacji zadania {0} negatywny", item.ID.ToString());
        }


        private void isUpdateIssueMessageExist(object sender, ConditionalEventArgs e)
        {
            if (vm1.Length > 0) e.Result = true;
        }

        public StringDictionary msgHeaders = new System.Collections.Specialized.StringDictionary();

        private Klient iok;

        private void Setup_UpdateIssueMessage_ExecuteCode(object sender, EventArgs e)
        {
            msgBody1 = vm1.ToString();
            msgSubject1 = string.Format(@"Aktulizacja zadań wspólników spółki {0} wymaga ręcznej obsługi", iok.NazwaFirmy);
        }

        StringDictionary headers;

        private void Preset_Message_ExecuteCode(object sender, EventArgs e)
        {
            iok = new Klient(item.Web, BLL.Tools.Get_LookupId(item, "selKlient"));
            msgTo = workflowProperties.OriginatorEmail;

            headers = new StringDictionary();
            headers.Add("o:tag", "Validation Results");
            headers.Add("Importance", "high");
            headers.Add("X-Priority", "1");
            headers.Add("X-MSMail-Priority", "High");
            //headers.Add("Expiry-Date", "Wed, 10 Feb 2016 17:15:00 +0100");
            //headers.Add("Expiry-Date2", SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddMinutes(5)));
            //headers.Add("X-Message-Flag", "=?iso-8859-2?Q?Flaga_monituj=B1ca?=");

            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            headers.Add("Reply-By", DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss +0100", ci));
            //headers.Add("Reply-By", "Wed, 10 Feb 2016 15:00:00 +0000");

            msgHeaders = headers;
        }


        public String msgBody1 = default(System.String);
        public String msgSubject1 = default(System.String);

    }

    public enum TaskCommands
    {
        Zatwiedz,
        WyslijInfo,
        WyslijInfoIZakoncz,
        Anuluj,
        NotDefined
    }
}
