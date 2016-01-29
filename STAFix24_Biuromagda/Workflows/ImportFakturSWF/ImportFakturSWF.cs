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
using System.Diagnostics;
using BLL;
using BLL.Models;
using System.Text;

namespace Workflows.ImportFakturSWF
{
    public sealed partial class ImportFakturSWF : SequentialWorkflowActivity
    {
        public ImportFakturSWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public String logParameters_HistoryOutcome = default(System.String);
        int sourceItemId;
        int okresId;
        string _ZAKONCZONY = "Zakończony";
        string _ANULOWANY = "Anulowany";

        string targetListLFZO = @"Faktury za obsługę - import";
        string targetListLFE = @"Faktury elektroniczne - import";
        private string targetListKK = @"Karty kontrolne";

        public String logPowiazanyDokument_HistoryOutcome = default(System.String);
        private SPList kkList;

        const string templateH = @"<table><tr valign='top'><td><div style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small; color: #808080'><strong>w załączeniu:</strong></div></td><td><ul>{0}</ul></td></tr></table>";
        const string templateR = @"<li style= 'font-family: Arial, Helvetica, sans-serif; font-size: x-small '>{0}</li>";


        #region Error Handler
        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fa = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fa != null)
            {
                Debug.WriteLine(fa.Fault.Source);
                Debug.WriteLine(fa.Fault.Message);
                Debug.WriteLine(fa.Fault.StackTrace);

                logErrorMessage_HistoryDescription = string.Format("{0}::{1}",
                    fa.Fault.Message,
                    fa.Fault.StackTrace);


                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fa.Fault.Message, fa.Fault.StackTrace);
            }
        }

        public String logErrorMessage_HistoryDescription = default(System.String);
        private IEnumerator lfzoEnum;
        private IEnumerator lfeEnum;
        private BiuroRachunkowe biuroRachunkowe;
        private FakturaDoZaplaty faktura;
        private string okresTitle;
        private DateTime planowanaDataNadania;
        private SPList lfzoList;
        private SPList lfeList;
        private SPListItem item;
        #endregion

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("ImportFakturSWF:{" + workflowProperties.WorkflowId + "} initiated");
        }

        private void Get_Parameteres_ExecuteCode(object sender, EventArgs e)
        {
            if (workflowProperties.InitiationData.Length > 0)
            {
                string[] param = workflowProperties.InitiationData.Split(new string[] { ";" }, StringSplitOptions.None);

                okresId = int.Parse(param.GetValue(0).ToString());
                SPListItem o = BLL.tabOkresy.Get_OkresById(workflowProperties.Web, okresId);
                okresTitle = o.Title;
                sourceItemId = int.Parse(param.GetValue(1).ToString());

                logParameters_HistoryOutcome = string.Format("OkresId={0}, SourceItemId={1}",
                                                              okresId.ToString(),
                                                              sourceItemId.ToString());


            }
            else
            {
                logParameters_HistoryOutcome = "brak";
            }
        }

        private void UpdateItem_Anulowany_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem(_ANULOWANY);
        }

        private void UpdateItem_Zakonczony_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem(_ZAKONCZONY);
        }

        private void Update_SourceItem(string statusZlecenia)
        {
            SPListItem sourceItem = BLL.admProcessRequests.GetItemById(workflowProperties.Web, sourceItemId);
            if (sourceItem != null)
            {
                BLL.Tools.Set_Text(sourceItem, "enumStatusZlecenia", statusZlecenia);
                sourceItem.Update();
            }
        }

        private void hasValidParams(object sender, ConditionalEventArgs e)
        {
            if (okresId > 0 && sourceItemId > 0) e.Result = true;
        }

        #region Obsuga listy faktur za obsługę
        private void Select_LFZO_ExecuteCode(object sender, EventArgs e)
        {
            lfzoEnum = lfzoList.GetItems().GetEnumerator();
        }

        private void whileLFZOExist(object sender, ConditionalEventArgs e)
        {
            if (lfzoEnum.MoveNext() && lfzoEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Manage_LFZO_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = lfzoEnum.Current as SPListItem;

            Debug.WriteLine("LFZO Item#" + item.ID.ToString());

            Map_DaneOFakturze(workflowProperties.Web, item, okresId);
        }

        private static void Map_DaneOFakturze(SPWeb web, SPListItem item, int okresId)
        {
            int klientId = 0;

            string nazwaSkrocona = item["_Klient"] != null ? item["_Klient"].ToString().Trim() : string.Empty;

            if (!String.IsNullOrEmpty(nazwaSkrocona))
            {
                klientId = tabKlienci.Get_KlientId(item.Web, nazwaSkrocona);
            }

            if (klientId > 0)
            {
                //item["Title"] = tabKlienci.Get_KlientById(item.Web, klientId).Title;
                item["Title"] = String.Empty;
                item["selKlient"] = klientId;
                item["selOkres"] = okresId;
                if (item["selZadanie"] != null) item["selZadanie"] = 0;

                DateTime dataWystawienia = item["cDataWystawienia"] != null ? DateTime.Parse(item["cDataWystawienia"].ToString()) : new DateTime();
                Klient iok = new Klient(web, klientId);
                DateTime terminPlatnosci = new DateTime();
                terminPlatnosci = dataWystawienia.AddDays(iok.TerminPlatnosci);
                item["colBR_TerminPlatnosci"] = terminPlatnosci;
            }
            else
            {
                item["Title"] = "niezgodna nazwa pliku";
                item["selKlient"] = 0;
                item["selOkres"] = 0;
                if (item["selZadanie"] != null) item["selZadanie"] = 0;
            }

            item.Update();
        }
        #endregion

        private void Select_LFE_ExecuteCode(object sender, EventArgs e)
        {
            lfeEnum = lfeList.GetItems().GetEnumerator();

        }

        private void whileLFEExist(object sender, ConditionalEventArgs e)
        {
            if (lfeEnum.MoveNext() && lfeEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Manage_LFE_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem item = lfeEnum.Current as SPListItem;

            Debug.WriteLine("LFE Item#" + item.ID.ToString());

            Map_Faktura(item, okresId);
        }

        private static void Map_Faktura(SPListItem item, int okresId)
        {
            int klientId = 0;
            string fileName = item.File.Name;

            string nazwaSkrocona = Extract_NazwaSkrocona(fileName);

            if (!String.IsNullOrEmpty(nazwaSkrocona))
            {
                klientId = tabKlienci.Get_KlientId(item.Web, nazwaSkrocona);
            }

            if (klientId > 0)
            {
                //item["Title"] = tabKlienci.Get_KlientById(item.Web, klientId).Title;
                item["Title"] = String.Empty;
                item["selKlient"] = klientId;
                item["selOkres"] = okresId;
                if (item["selZadanie"] != null) item["selZadanie"] = 0;

            }
            else
            {
                item["Title"] = "niezgodna nazwa pliku";
                item["selKlient"] = 0;
                item["selOkres"] = 0;
                if (item["selZadanie"] != null) item["selZadanie"] = 0;
            }

            item.Update();
        }

        private static string Extract_NazwaSkrocona(string fileName)
        {
            string result = string.Empty;

            var startIndex = 4;
            var endIndex = fileName.IndexOf(@" -");
            var len = endIndex - startIndex + 1;

            if (len > 0)
            {
                result = fileName.Substring(startIndex, len)
                    .Trim()
                    .ToUpper();
            }

            return result;

        }

        private void Manage_RBR(SPWeb web, BLL.Models.FakturaDoZaplaty faktura, BLL.Models.BiuroRachunkowe biuroRachunkowe)
        {
            int klientId = faktura.KlientId;

            if (klientId > 0)
            {
                string nadawca = faktura.EmailNadawcy;
                string odbiorca = faktura.EmailOdbiorcy;

                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = true;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;

                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(web, "RBR_TEMPLATE.Include", out temat, out trescHTML, nadawca);

                temat = temat + " - " + faktura.NumerFaktury;

                string lt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(web, "RBR_LEADING_TEXT", false);
                string firma = BLL.tabKlienci.Get_NazwaFirmyById(web, klientId);
                lt = lt.Replace("___FIRMA___", firma);
                string okres = faktura.Okres;
                lt = lt.Replace("___OKRES___", okres);
                trescHTML = trescHTML.Replace("___RBR_LEADING_TEXT___", lt);

                //uzupełnia temat kodem klienta, numerem okresu i numerem faktury
                //temat = AddSpecyfikacja(item, temat, string.Empty);

                //uzupełnia dane w formatce BR_TEMPLATE
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___colBR_NumerFaktury___", faktura.NumerFaktury);
                sb.Replace("___colBR_DataWystawienia___", BLL.Tools.Format_Date(faktura.DataWystawieniaFaktury));
                sb.Replace("___colBR_WartoscDoZaplaty___", BLL.Tools.Format_Currency(faktura.WartoscDoZaplaty));
                sb.Replace("___colBR_Konto___", BLL.Tools.Format_Konto(biuroRachunkowe.Konto));
                sb.Replace("___colBR_TerminPlatnosci___", BLL.Tools.Format_Date(faktura.TerminPlatnosci));

                string info2 = string.Empty;
                string info = faktura.InformacjaDlaKlienta;

                //dodaj informację o z załącznikach w/g ustawionych flag
                if (faktura.FakturaPDF_Exist())
                {
                    info2 = info2 + string.Format(templateR, "Faktura za usługi biura rachunkowego");
                }
                if (faktura.DrukWplatyWymagany)
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

                planowanaDataNadania = DateTime.Now.AddMinutes(30);

                BLL.tabWiadomosci.AddNew_FakturaDoZaplaty(web, faktura, biuroRachunkowe, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, faktura.FakturaPDF_Url, faktura.DrukWplatyWymagany, planowanaDataNadania, klientId);

                #region Obsługa remindera
                ////obsługa remindera
                //if (hasPrzypomnienieOTerminiePlatnosci(item))
                //{
                //    KopiaDoNadawcy = false;
                //    KopiaDoBiura = false;

                //    DateTime terminPlatnosci = Get_Date(item, "colBR_TerminPlatnosci");


                //    if (GetValue(item, "colBR_WartoscDoZaplaty") > 0)
                //    {
                //        //ustaw reminder
                //        nadawca = BLL.admSetup.GetValue(item.Web, "EMAIL_BIURA");
                //        BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_REMINDER_TEMPLATE.Include", out temat, out trescHTML, nadawca);
                //        temat = Update_Data(temat, terminPlatnosci);
                //        temat = BLL.Tools.AddCompanyName(temat, item);

                //        //leading reminder text
                //        string lrt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_LEADING_REMINDER_TEXT", false);
                //        lrt = lrt.Replace("___FIRMA___", firma);
                //        lrt = lrt.Replace("___OKRES___", okres);
                //        trescHTML = trescHTML.Replace("___RBR_LEADING_REMINDER_TEXT___", lrt);

                //        //trailing reminder text
                //        string trt = BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "RBR_TRAILING_REMINDER_TEXT", false);
                //        trt = trt.Replace("___DATA___", DateTime.Now.ToShortDateString()); //zakłada że wysyłka oryginalnej wiadomości wyjdzie w dniu zlecenia
                //        trescHTML = trescHTML.Replace("___RBR_TRAILING_REMINDER_TEXT___", trt);

                //        //aktualizacja danych z tabelki
                //        sb = new StringBuilder(trescHTML);
                //        sb.Replace("___colBR_NumerFaktury___", item["colBR_NumerFaktury"] != null ? item["colBR_NumerFaktury"].ToString() : string.Empty);
                //        sb.Replace("___colBR_DataWystawienia___", Format_Date(item, "colBR_DataWystawieniaFaktury"));
                //        sb.Replace("___colBR_WartoscDoZaplaty___", Format_Currency(item, "colBR_WartoscDoZaplaty"));
                //        sb.Replace("___colBR_Konto___", item["colBR_Konto"] != null ? item["colBR_Konto"].ToString() : string.Empty);
                //        sb.Replace("___colBR_TerminPlatnosci___", Format_Date(item, "colBR_TerminPlatnosci"));

                //        trescHTML = sb.ToString();

                //        planowanaDataNadania = Calc_ReminderTime(item, terminPlatnosci);


                //        BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID, klientId, Marker.Ignore);
                //    }

                //} 
                #endregion
            }
        }

        private void codeActivity1_ExecuteCode(object sender, EventArgs e)
        {
            biuroRachunkowe = new BiuroRachunkowe(workflowProperties.Web);
        }

        private void Select_LFDO_ExecuteCode(object sender, EventArgs e)
        {
            //załaduj informacje o fakturach
            Array lfzoResults = workflowProperties.Web.Lists.TryGetList(targetListLFZO).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId)
                            && BLL.Tools.Get_LookupId(i, "selKlient") > 0)
                .ToArray();

            lfzoEnum = lfzoResults.GetEnumerator();
        }

        private void whileRecordExist(object sender, ConditionalEventArgs e)
        {
            if (lfzoEnum.MoveNext() && lfzoEnum != null) e.Result = true;
            else e.Result = false;

        }

        private void CopySrcData_ExecuteCode(object sender, EventArgs e)
        {
            item = lfzoEnum.Current as SPListItem;

            if (!string.IsNullOrEmpty(item.Title))
            {
                item["Title"] = string.Empty;
            }

            int klientId = BLL.Tools.Get_LookupId(item, "selKlient");

            faktura = new FakturaDoZaplaty(workflowProperties.Web, klientId);

            faktura.DataWystawieniaFaktury = BLL.Tools.Get_Date(item, "cDataWystawienia");
            faktura.NumerFaktury = BLL.Tools.Get_Text(item, "colBR_NumerFaktury");
            faktura.WartoscDoZaplaty = BLL.Tools.Get_Value(item, "colBR_WartoscDoZaplaty");
            faktura.TerminPlatnosci = BLL.Tools.Get_Date(item, "colBR_TerminPlatnosci");
            faktura.EmailNadawcy = biuroRachunkowe.Email;
            faktura.Okres = okresTitle;
            faktura.IOF_Id = item.ID;
        }

        private void Find_RelatedPDF_ExecuteCode(object sender, EventArgs e)
        {
            if (faktura.KlientId > 0)
            {
                string customNumerFaktury = Create_SubstringPattern(faktura.NumerFaktury);

                SPListItem o = workflowProperties.Web.Lists.TryGetList(targetListLFE).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId)
                            && BLL.Tools.Get_LookupId(i, "selKlient").Equals(faktura.KlientId))
                .Where(i => i.DisplayName.Contains(customNumerFaktury))
                .FirstOrDefault();

                if (o != null)
                {
                    faktura.FakturaPDF_Url = o.Url;
                    faktura.PDF_Id = o.ID;
                }
            }
        }

        /// <summary>
        /// numer faktury
        /// </summary>
        private string Create_SubstringPattern(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("/", ".");
            return sb.ToString();
        }

        /// <summary>
        /// numer okresu
        /// </summary>
        private string Create_SubstringPattern2(string s)
        {
            if (s.Length == 7)
            {
                return s.Substring(5, 2) + "." + s.Substring(0, 4);
            }
            else
            {
                return string.Empty;
            }
        }




        private void hasRelatedPDF(object sender, ConditionalEventArgs e)
        {
            if (faktura.FakturaPDF_Exist())
            {
                logPowiazanyDokument_HistoryOutcome = faktura.FakturaPDF_Url;
                e.Result = true;
            }
        }

        private void Setup_Faktura_ExecuteCode(object sender, EventArgs e)
        {
            Manage_RBR(workflowProperties.Web, faktura, biuroRachunkowe);
        }

        private void Update_KK_ExecuteCode(object sender, EventArgs e)
        {

            if (faktura.KK_Id > 0)
            {
                SPListItem kkItem = kkList.GetItemById(faktura.KK_Id);

                Debug.WriteLine("Aktualizacja KK#" + kkItem.ID.ToString());

                BLL.Tools.Set_Date(kkItem, "colBR_DataWystawieniaFaktury", faktura.DataWystawieniaFaktury);
                BLL.Tools.Set_Text(kkItem, "colBR_NumerFaktury", faktura.NumerFaktury);
                BLL.Tools.Set_Value(kkItem, "colBR_WartoscDoZaplaty", faktura.WartoscDoZaplaty);
                BLL.Tools.Set_Date(kkItem, "colBR_TerminPlatnosci", faktura.TerminPlatnosci);
                BLL.Tools.Set_Flag(kkItem, "colBR_FakturaZalaczona", faktura.FakturaPDF_Exist());
                //BLL.Tools.Set_Date(kkItem, "colBR_DataWyslaniaInfomacji", planowanaDataNadania);



                try
                {
                    BLL.Tools.DoWithRetry(() => kkItem.Update());
                    faktura.KK_Zaktualizowana = true;
                }
                catch (Exception)
                { }
            }
            else
            {
                Debug.WriteLine("!!! Nie znalezniono KK");
            }

        }

        private void Delete_SourceData_ExecuteCode(object sender, EventArgs e)
        {
            if (faktura.KK_Id > 0 && faktura.KK_Zaktualizowana)
            {
                Debug.WriteLine("Usuwanie PDF#" + faktura.PDF_Id.ToString());
                lfeList.GetItemById(faktura.PDF_Id).Delete();

                Debug.WriteLine("Usuwanie IOF#" + faktura.IOF_Id.ToString());
                lfzoList.GetItemById(faktura.IOF_Id).Delete();
            }
        }

        private void isSent(object sender, ConditionalEventArgs e)
        {
            if (faktura.Wyslana) e.Result = true;
        }

        private void Preset_kkList_ExecuteCode(object sender, EventArgs e)
        {
            kkList = workflowProperties.Web.Lists.TryGetList(targetListKK);
        }

        private void Init_Lists_ExecuteCode(object sender, EventArgs e)
        {
            lfzoList = workflowProperties.Web.Lists.TryGetList(targetListLFZO);
            lfeList = workflowProperties.Web.Lists.TryGetList(targetListLFE);
        }

        private void Find_KK_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem kkItem = kkList.Items.Cast<SPListItem>()
                  .Where(i => BLL.Tools.Get_LookupId(i, "selKlient").Equals(faktura.KlientId))
                  .Where(i => BLL.Tools.Get_LookupId(i, "selOkres").Equals(okresId))
                  .FirstOrDefault();

            if (kkItem != null)
            {
                Debug.WriteLine("Skojarzona KK#" + kkItem.ID.ToString());
                faktura.KK_Id = kkItem.ID;
            }
            else
            {
                Debug.WriteLine("!!! Nie znaleziono karty kontrolnej");
            }


        }

        private void isIstniejeKK(object sender, ConditionalEventArgs e)
        {
            if (faktura.KK_Id > 0) e.Result = true;
        }

        private void update_ExecuteCode(object sender, EventArgs e)
        {

        }

        private void isKKIstnieje(object sender, ConditionalEventArgs e)
        {
            if (faktura.KK_Id > 0) e.Result = true;
        }

        private void Report_KKNieIstnieje_ExecuteCode(object sender, EventArgs e)
        {
            item["Title"] = item.Title + "Nie istnieje karta kontrolna";
            item.Update();
        }

        private void Report_NoRelatedPDF_ExecuteCode(object sender, EventArgs e)
        {
            item["Title"] = item.Title + "Nie znaleziono powiązanego PDF'a";
            item.Update();
        }

        public String msgTo = default(System.String);
        public String msgSubject = default(System.String);

        private void sendEmail1_MethodInvoking(object sender, EventArgs e)
        {

            msgSubject = "Import faktur: veryfikacja powiązań zakończona";
        }

        private void sendEmail2_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = "Import faktur: generowanie wiadomości i aktualizacja kart kontrolnych zakończona";
        }

        private void sendEmail4_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = "Import faktur: brak obrazu faktury nr " + faktura.NumerFaktury + " dla klienta " + faktura.NazwaKlienta + " za okres " + okresTitle;
        }

        private void sendEmail3_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = "Import faktur: brak karty kontrolnej dla klienta " + faktura.NazwaKlienta + " za okres " + okresTitle;
        }

        private void sendEmail5_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = "Import faktur: nieprawidłowe paramety początkowe";
        }

        private void sendEmail6_MethodInvoking(object sender, EventArgs e)
        {
            msgFrom = "noreply@stafix24.pl";
            msgTo = workflowProperties.OriginatorEmail;
            msgSubject = "Import faktur: zainicjowany";
        }

        public String msgFrom = default(System.String);

        private void UpdateItem_ObslugaFaza1_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem("Obsługa: Faza 1/2");
        }

        private void UpdateItem_ObslugaFaza2_ExecuteCode(object sender, EventArgs e)
        {
            Update_SourceItem("Obsługa: Faza 2/2");
        }

        private void isUpdateKKIssue(object sender, ConditionalEventArgs e)
        {
            if (!faktura.KK_Zaktualizowana) e.Result = true;
        }

        private void sendEmail7_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = "Import faktur: aktualizcja karty kontrolnej #" + faktura.KK_Id.ToString() + " nie powiodła sie";
        }

        private void Report_InvSent_UpdateKKIssue_ExecuteCode(object sender, EventArgs e)
        {
            item["Title"] = item.Title + "Wiadomość przygotowana. Aktualizacja karty kontrolnej nie powiodła się";
            item.Update();
        }

    }
}
