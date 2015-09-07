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
using System.Text;

namespace Workflows.ObslugaZadania2
{
    public sealed partial class ObslugaZadania2 : SequentialWorkflowActivity
    {
        public ObslugaZadania2()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public string ct;
        public SPListItem item;

        const string WYSLIJ_INFORMACJE_DO_KLIENTA = "Wyślij informację do Klienta";
        const string ZATWIERDZ = "Zatwierdź";

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
            ct = item.ContentType.Name;
        }

        private void isZadanie(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Zadanie" ? true : false;
        }

        private void isProsbaODokumenty(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Prośba o dokumenty" ? true : false;
        }

        private void isProsbaOWyciagBankowy(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Prośba o przesłanie wyciągu bankowego" ? true : false;
        }

        private void isRozliczeniePD(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku dochodowego" ? true : false;
        }

        private void isRozliczeniePDS(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku dochodowego spółki" ? true : false;
        }

        private void isRozliczenieVAT(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie podatku VAT" ? true : false;
        }

        private void isRozliczenieZUS(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie ZUS" ? true : false;
        }

        private void isRozliczenieRBR(object sender, ConditionalEventArgs e)
        {
            e.Result = ct == "Rozliczenie z biurem rachunkowym" ? true : false;
        }

        private void Manage_ProsbaODokumenty_ExecuteCode(object sender, EventArgs e)
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

            BLL.tabWiadomosci.AddNew(item.Web, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);

        }

        private void Manage_ProsbaOWyciagBankowy_ExecuteCode(object sender, EventArgs e)
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

            BLL.tabWiadomosci.AddNew(item.Web, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);

        }

        private void Manage_Zadanie_ExecuteCode(object sender, EventArgs e)
        {
            string cmd = GetCommand(item);
            string notatka = item["colInformacjaDlaKlienta"]!=null?item["colInformacjaDlaKlienta"].ToString():string.Empty;
            int klientId = item["selKlient"] != null ? new SPFieldLookupValue(item["selKlient"].ToString()).LookupId : 0;

            if (klientId > 0
                && cmd == WYSLIJ_INFORMACJE_DO_KLIENTA 
                && !string.IsNullOrEmpty(notatka))
            {
                string nadawca = new SPFieldUserValue(item.Web, item["Editor"].ToString()).User.Email;
                string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, klientId);
                string kopiaDla = string.Empty;
                bool KopiaDoNadawcy = false;
                bool KopiaDoBiura = false;
                string temat = string.Empty;
                string tresc = string.Empty;
                string trescHTML = string.Empty;
                BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item.Web, "EMAIL_DEFAULT_BODY", out temat, out trescHTML);
                temat = string.Format("{0} [sprawa#{1}]", item.Title, item.ID.ToString());
                StringBuilder sb = new StringBuilder(trescHTML);
                sb.Replace("___BODY___", notatka);
                trescHTML = sb.ToString();

                DateTime planowanaDataNadania = item["colTerminWyslaniaInformacji"] != null ? DateTime.Parse(item["colTerminWyslaniaInformacji"].ToString()) : new DateTime();

                BLL.tabWiadomosci.AddNew(item.Web, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, planowanaDataNadania, item.ID);

                UpdateNotatka(item, notatka);
            }

            ResetCommand(item, true);
        }

        #region Helpers

        private void UpdateNotatka(SPListItem item, string notatka)
        {
            item["colInformacjaDlaKlienta"] = string.Format("{0}\n wysłana: {1}", notatka, DateTime.Now.ToString());
        }

        private void ResetCommand(SPListItem item, bool clearInformacjaDlaKlienta)
        {
            item["cmdFormatka"] = string.Empty;
            if (clearInformacjaDlaKlienta)
            {
                item["colInformacjaDlaKlienta"] = string.Empty;
            }
            item.Update();
        }

        private string GetCommand(SPListItem item)
        {
            return item["cmdFormatka"] != null ? item["cmdFormatka"].ToString() : string.Empty;
        }

        #endregion




    }
}
