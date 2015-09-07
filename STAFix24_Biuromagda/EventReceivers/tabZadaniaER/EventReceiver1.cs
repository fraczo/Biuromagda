using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;

namespace tabZadania_EventReceiver.EventReceiver1
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Execute(properties);
        }

        private void Execute(SPItemEventProperties properties)
        {
            try
            {
                this.EventFiringEnabled = false;

                SPListItem item = properties.ListItem;
                SPWeb web = item.Web;

                string ct = item["ContentType"].ToString();
                switch (ct)
                {
                    case "Rozliczenie z biurem rachunkowym":
                    case "Rozliczenie podatku dochodowego":
                    case "Rozliczenie podatku VAT":
                    case "Rozliczenie ZUS":
                        UpdateKEY(item);
                        UpdateGBW(web, item, ct);
                        break;
                    case "Zadanie":
                        int procId = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupId : 0;
                        if (procId == 0)
                        {
                            procId = BLL.tabProcedury.Update(web, item.Title);
                            item["selProcedura"] = procId;
                            item.Update();
                        }
                        //update termin realizacji
                        if (procId > 0 && (item["colTerminRealizacji"] == null || (DateTime)item["colTerminRealizacji"] != new DateTime()))
                        {

                            int termin = BLL.tabProcedury.Get_TerminRealizacjiOfsetById(web, procId);
                            if (termin > 0)
                            {
                                item["colTerminRealizacji"] = DateTime.Today.AddDays(termin);
                                item.Update();
                            }
                        }

                        //update operatora
                        if (procId > 0 && item["selOperator"] == null)
                        {
                            int operatorId = BLL.tabProcedury.Get_OperatorById(web, procId);
                            if (operatorId > 0)
                            {

                                item["selOperator"] = operatorId;
                                item.Update();

                            }
                        }
                        break;
                    default:
                        break;
                }

                // update Title
                if (String.IsNullOrEmpty(item.Title))
                {
                    item["Title"] = item["selProcedura"] != null ? new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue : "#" + item.ID.ToString();
                    item.Update();
                }

                // update _KontoOperatora
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
            catch (Exception ex)
            {

                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());

            }
            finally
            {
                this.EventFiringEnabled = true;
            }

        }

        private bool UpdateGBW(SPWeb web, SPListItem item, string ct)
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
                        konto = item["colZUS_ZD_Konto"] != null ? item["colZUS_ZD_Konto"].ToString() : string.Empty;
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
                                    konto = iok.NumerRachunkuPIT;
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
                                    konto = iok.NumerRachunkuPIT;
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
                        konto = item["colZUS_FP_Konto"] != null ? item["colZUS_FP_Konto"].ToString() : string.Empty;
                        fileName = String.Format(@"{0}ZUS 53_{1}.pdf",
                                           targetFileNameLeading,
                                           okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);

                        //skladka zdrowotna
                        kwota = item["colZUS_ZD_Skladka"] != null ? Double.Parse(item["colZUS_ZD_Skladka"].ToString()) : 0;
                        konto = item["colZUS_ZD_Konto"] != null ? item["colZUS_ZD_Konto"].ToString() : string.Empty;
                        fileName = String.Format(@"{0}ZUS 52_{1}.pdf",
                                            targetFileNameLeading,
                                            okres);

                        result = Generuj_DrukZUS(web, item, result, okres, klientId, kwota, konto, fileName);

                        //skladka spoleczna
                        kwota = item["colZUS_SP_Skladka"] != null ? Double.Parse(item["colZUS_SP_Skladka"].ToString()) : 0;
                        konto = item["colZUS_SP_Konto"] != null ? item["colZUS_SP_Konto"].ToString() : string.Empty;
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
            string konto = item["colPD_Konto"] != null ? item["colPD_Konto"].ToString() : string.Empty;

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
            string konto = item["colVAT_Konto"] != null ? item["colVAT_Konto"].ToString() : string.Empty;

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

        private static bool ObslugaGBW_RozliczenieZBiuremRachunkowym(SPWeb web, SPListItem item, bool result, string targetFileNameLeading, string klient, string okres, string nadawca)
        {
            double kwota = item["colBR_WartoscDoZaplaty"] != null ? Double.Parse(item["colBR_WartoscDoZaplaty"].ToString()) : 0;
            string konto = item["colBR_Konto"] != null ? item["colBR_Konto"].ToString() : string.Empty;

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

        private static void UpdateKEY(SPListItem item)
        {
            string key = tabZadania.Define_KEY(item);
            tabZadania.Update_KEY(item, key);
        }

        /// <summary>
        /// An item is being added
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            Validate(properties);
        }

        /// <summary>
        /// An item is being updated
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            Validate(properties);
        }

        private void Validate(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            string ct = properties.AfterProperties["ContentType"].ToString();
            int klientId = new SPFieldLookupValue(properties.AfterProperties["selKlient"].ToString()).LookupId;
            int okresId = new SPFieldLookupValue(properties.AfterProperties["selOkres"].ToString()).LookupId;

            if (string.IsNullOrEmpty(ct) || klientId<=0 || okresId <=0)
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
    }
}
