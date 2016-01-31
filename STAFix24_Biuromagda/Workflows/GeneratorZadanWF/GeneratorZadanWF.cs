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

namespace Workflows.GeneratorZadanWF
{
    public sealed partial class GeneratorZadanWF : SequentialWorkflowActivity
    {
        public GeneratorZadanWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        private SPListItem item;
        public String logErrorMessage_HistoryDescription = default(System.String);

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            Debug.WriteLine("GeneratorZadanWF:{" + workflowProperties.WorkflowId + "} initiated");
            item = workflowProperties.Item;
        }

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
        #endregion

        public String logKlienci_HistoryOutcome = default(System.String);
        public static DependencyProperty logTask_HistoryDescriptionProperty = DependencyProperty.Register("logTask_HistoryDescription", typeof(System.String), typeof(Workflows.GeneratorZadanWF.GeneratorZadanWF));

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [BrowsableAttribute(true)]
        [CategoryAttribute("Misc")]
        public String logTask_HistoryDescription
        {
            get
            {
                return ((string)(base.GetValue(Workflows.GeneratorZadanWF.GeneratorZadanWF.logTask_HistoryDescriptionProperty)));
            }
            set
            {
                base.SetValue(Workflows.GeneratorZadanWF.GeneratorZadanWF.logTask_HistoryDescriptionProperty, value);
            }
        }

        public String logTask_HistoryOutcome = default(System.String);
        public String msgTo = default(System.String);
        public String msgSubject = default(System.String);
        public String msgFrom = default(System.String);
        private ArrayList alKlienci;
        private IEnumerator myEnumerator;

        private void sendEmail1_MethodInvoking(object sender, EventArgs e)
        {
            msgFrom = "noreply@stafix24.pl";
            msgTo = workflowProperties.OriginatorEmail;
            msgSubject = string.Format("Generator zleceń - uruchomiony dla {0} rekordów", alKlienci.Count.ToString());
        }

        private void sendEmail2_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = string.Format("Generator zleceń - zakończony", 0);
        }

        private void Select_Klienci_ExecuteCode(object sender, EventArgs e)
        {
            Array klienci = BLL.tabKlienci.Get_ListItems(item.Web);
            alKlienci = new ArrayList();

            bool includeInactive = BLL.Tools.Get_Flag(item, "Uwzgl_x0119_dnij_x0020_nieaktywn");
            SPFieldLookupValueCollection serwisyWybrane = BLL.Tools.Get_LookupValueColection(item, "selSewisy");
            string wybranyTypKlienta = BLL.Tools.Get_Text(item, "enumTypKlienta");
            string wybraneBiuro = BLL.Tools.Get_LookupValue(item, "selBiuro");

            bool zadanieDlaWszystkich = BLL.Tools.Get_Flag(item, "Zadanie_x0020_dla_x0020_wszystki");

            foreach (SPListItem k in klienci)
            {
                string statusKlienta = BLL.Tools.Get_Text(k, "enumStatus");

                switch (k.ContentType.Name)
                {
                    case "KPiR":
                    case "KSH":
                    case "Osoba fizyczna":
                    case "Firma":
                    case "Firma zewnętrzna":

                        Debug.WriteLine(BLL.Tools.Get_Text(k, "_NazwaPrezentowana"));

                        bool classified = true;

                        if (zadanieDlaWszystkich)
                        {
                            if (includeInactive)
                            {
                                //dopuszcza dowolny status
                            }
                            else
                            {
                                if (statusKlienta != BLL.Models.StatusKlienta.Aktywny.ToString())
                                    classified = false;
                            }
                        }
                        else
                        {
                            //wybór klientów w/g kryteriów

                            if (includeInactive)
                            {
                                //dopuszcza dowolny status
                            }
                            else
                            {
                                if (statusKlienta != BLL.Models.StatusKlienta.Aktywny.ToString())
                                    classified = false;
                            }

                            //Typ klienta

                            string typKlienta = k.ContentType.Name;

                            if (classified && wybranyTypKlienta.Length > 0)
                            {
                                Debug.WriteLine("Wybrany typ klienta:" + wybranyTypKlienta);
                                switch (wybranyTypKlienta)
                                {
                                    case "KPiR":
                                        if (!typKlienta.Equals("KPiR")) classified = false;
                                        break;
                                    case "KSH":
                                        if (!typKlienta.Equals("KSH")) classified = false;
                                        break;
                                    case "Osoba fizyczna":
                                        if (!typKlienta.Equals("Osoba fizyczna")) classified = false;
                                        break;
                                    case "Firma":
                                        if (!typKlienta.Equals("Firma")) classified = false;
                                        break;
                                    case "Firma zewnętrzna":
                                        if (!typKlienta.Equals("Firma zewnętrzna")) classified = false;
                                        break;
                                }
                            }

                            //analiza serwisów

                            SPFieldLookupValueCollection serwisyKlienta = BLL.Tools.Get_LookupValueColection(k, "selSewisy");

                            if (classified && serwisyWybrane.Count > 0)
                            {
                                Debug.WriteLine("Wybrany serwis:" + serwisyWybrane.ToString());
                                bool found = false;

                                foreach (var wybranySerwis in serwisyWybrane)
                                {
                                    foreach (var serwisKlienta in serwisyKlienta)
                                    {
                                        if (serwisKlienta.LookupValue.Equals(wybranySerwis.LookupValue))
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (found) break;
                                }

                                if (!found) classified = false;
                            }

                            //analiza biura

                            string biuro = BLL.Tools.Get_LookupValue(k, "selBiuro");

                            if (classified && !string.IsNullOrEmpty(wybraneBiuro))
                            {
                                Debug.WriteLine("Wybrane biuro:" + wybraneBiuro);
                                if (!wybraneBiuro.Equals(biuro))
                                {
                                    classified = false;
                                }
                            }
                        }


                        //dodaj jeżeli spełnia warunki

                        if (classified)
                        {
                            alKlienci.Add(k);
                            Debug.WriteLine("DODANY");
                        }

                        break;
                }

            }

            logKlienci_HistoryOutcome = alKlienci.Count.ToString();

            Debug.WriteLine("Liczba klientów=" + alKlienci.Count.ToString());
        }

        private void Set_TaskEnumerator_ExecuteCode(object sender, EventArgs e)
        {
            myEnumerator = alKlienci.GetEnumerator();
        }

        private void whileTaskExist(object sender, ConditionalEventArgs e)
        {
            if (myEnumerator.MoveNext() && myEnumerator != null) e.Result = true;
            else e.Result = false;
        }

        private void Create_Task_ExecuteCode(object sender, EventArgs e)
        {
            SPListItem klient = myEnumerator.Current as SPListItem;
                
            Debug.WriteLine("Create_Task: " + BLL.Tools.Get_Text(klient, "_NazwaPrezentowana"));

            string tytul = item.Title;
            int proceduraId = BLL.Tools.Get_LookupId(item, "selProcedura");
            string opis = BLL.Tools.Get_Text(item, "colOpis");
            string uwagi = BLL.Tools.Get_Text(item, "colUwagi");
            DateTime terminRealizacji = BLL.Tools.Get_Date(item, "colTerminRealizacji");

            //przydziel do
            int operatorId = 0;
            string przydzielDo = BLL.Tools.Get_Text(item, "Przydziel_x0020_do");
            switch (przydzielDo)
            {
                case "Opiekun klienta":
                    operatorId = BLL.Tools.Get_LookupId(klient, "selOpiekunKlienta");
                    break;
                case "Dedykowany operator Podatki":
                    operatorId = BLL.Tools.Get_LookupId(klient, "selDedykowanyOperator_Podatki");
                    break;
                case "Dedykowany operator Kadry":
                    operatorId = BLL.Tools.Get_LookupId(klient, "selDedykowanyOperator_Kadry");
                    break;
                case "Dedykowany operator Audyt":
                    operatorId = BLL.Tools.Get_LookupId(klient, "selDedykowanyOperator_Audyt");
                    break;
                case "Operator przypisany do procedury":
                    if (proceduraId > 0)
                    {
                        SPListItem procedura = BLL.tabProcedury.Get_ById(item.Web, proceduraId);
                        operatorId = BLL.Tools.Get_LookupId(procedura, "selDedykowanyOperator");
                    }
                    break;
                case "Wskazany operator":
                    operatorId = BLL.Tools.Get_LookupId(item, "selOperator");
                    break;
            }

            string informacjaDlaKlienta = BLL.Tools.Get_Text(item, "colInformacjaDlaKlienta");

            int itemId = BLL.tabZadania.Create_Task(item.Web, klient.ID, tytul, proceduraId, opis, uwagi, terminRealizacji, operatorId, informacjaDlaKlienta);
        }
    }
}
