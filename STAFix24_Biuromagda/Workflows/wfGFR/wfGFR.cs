﻿using System;
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
using EventReceivers.admProcessRequestsER;
using System.Text;

namespace Workflows.wfGFR
{
    public sealed partial class wfGFR : SequentialWorkflowActivity
    {
        public wfGFR()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        private SPListItem item;
        private int okresId;
        private SPFieldMultiChoiceValue selTypyKlientow;
        private SPFieldLookupValueCollection selSerwisy;
        private opType ot;
        private Array klienci;
        private SPListItem klient;

        public String logKlient_HistoryOutcome = default(System.String);
        private StringBuilder sb;

        public String msgTo = default(System.String);
        public String msgBody = default(System.String);
        public String msgSubject = default(System.String);
        private string _SUBJECT_TEMPLATE = @"#{0} :: Generowanie hurtowe kart kontrolnych :: {1}";
        private string _LINE_TEMPLATE = @"<li>{0}</li>";

        private string _CT_KPIR = "KPiR";
        private string _CT_KSH = "KSH";
        private string _CT_Firma = "Firma";
        private string _CT_FirmaZewnętrzna = "Firma zewnętrzna";
        private string _CT_OsobaFizyczna = "Osoba fizyczna";

        public String logErrorMessage_HistoryDescription = default(System.String);
        public String logErrorMessage_HistoryOutcome = default(System.String);
        private StringBuilder sbForms;


        private void isValidRequest(object sender, ConditionalEventArgs e)
        {
            if (okresId > 0) e.Result = true;
        }

        private void isTypK_Serwis(object sender, ConditionalEventArgs e)
        {

            if (selTypyKlientow.Count > 0 && selSerwisy.Count > 0)
            {
                ot = opType.TKandS;
                e.Result = true;
            }
        }

        private void isTypK(object sender, ConditionalEventArgs e)
        {
            if (selTypyKlientow.Count > 0)
            {
                ot = opType.TK;
                e.Result = true;
            }
        }

        private void isSerwis(object sender, ConditionalEventArgs e)
        {
            if (selSerwisy.Count > 0)
            {
                ot = opType.S;
                e.Result = true;
            }
        }

        private void Select_Klienci_ExecuteCode(object sender, EventArgs e)
        {
            ArrayList selKlienci = new ArrayList();

            switch (ot)
            {
                case opType.TKandS:
                case opType.TK:
                    for (int i = 0; i < selTypyKlientow.Count; i++)
                    {
                        string tk = selTypyKlientow[i].ToString();
                        selKlienci.AddRange(BLL.tabKlienci.Get_AktywniKlienci_Serwis(item.Web, tk));
                    }
                    break;
                case opType.S:
                case opType.None:
                    selKlienci.AddRange(BLL.tabKlienci.Get_AktywniKlienci_Serwis(item.Web));
                    break;
                default:
                    break;
            }

            klienci = selKlienci.ToArray();

            logKlientCounter_HistoryOutcome = klienci.Length.ToString();
        }

        private static Array Refine_Klienci_Serwis(Array klienci, SPFieldLookupValueCollection serwisy)
        {
            ArrayList results = new ArrayList();

            foreach (SPListItem klientItem in klienci)
            {
                foreach (SPFieldLookupValue s in serwisy)
                {
                    if (BLL.Tools.Has_Service(klientItem, s.LookupValue, "selSewisy")
                        | BLL.Tools.Has_Service(klientItem, s.LookupValue, "selSerwisyWspolnicy"))
                    {
                        results.Add(klientItem);
                        Debug.WriteLine(BLL.Tools.Get_Text(klientItem, "_NazwaPrezentowana") + " - added");
                        break;
                    }
                }

            }

            return results.ToArray();
        }

        private void Refine_Klienci_ExecuteCode(object sender, EventArgs e)
        {
            klienci = Refine_Klienci_Serwis(klienci, selSerwisy);

            logKlientCounter_HistoryOutcome = klienci.Length.ToString();
        }

        public String logKlientCounter_HistoryOutcome = default(System.String);
        private IEnumerator myEnum;
        private int klientId;


        private void Prepare_List_ExecuteCode(object sender, EventArgs e)
        {
            myEnum = klienci.GetEnumerator();
        }

        private void isKPIR(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_KPIR)) e.Result = true;
        }

        private void isKSH(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_KSH)) e.Result = true;
        }

        private void isFirma(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_Firma)) e.Result = true;
        }

        private void isOsobaFizyczna(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_OsobaFizyczna)) e.Result = true;
        }

        private void isFirmaZewnetrzna(object sender, ConditionalEventArgs e)
        {
            if (klient.ContentType.Name.Equals(_CT_FirmaZewnętrzna)) e.Result = true;
        }

        private void whileKlientExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Manage_ZUS_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "ZUS-*", "selSewisy"))
                {
                    ZUS_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "ZUS");
                }
            }
            else
            {
                ZUS_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "ZUS");
            }

        }

        private void Manage_PD_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "PD-*", "selSewisy"))
                {
                    PD_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "PD");
                }
            }
            else
            {
                PD_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "PD");
            }



        }

        private void Manage_PDS_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "PDS-*", "selSewisy"))
                {
                    PDS_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "PDS");
                }
            }
            else
            {
                PDS_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "PDS");
            }


        }

        private void Manage_PDW_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "PDW-*", "selSewisy"))
                {
                    PDW_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "PDW");
                }
            }
            else
            {
                PDW_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "PDW");
            }


        }

        private void Manage_VAT_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "VAT-*", "selSewisy"))
                {
                    VAT_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "VAT");
                }
            }
            else
            {
                VAT_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "VAT");
            }


        }

        private void Manage_RBR_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "RBR*", "selSewisy"))
                {
                    BR_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "RBR");
                }
            }
            else
            {
                BR_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "RBR");
            }



        }

        private void Manage_Reminders_ExecuteCode(object sender, EventArgs e)
        {
            if (ot.Equals(opType.TKandS) || ot.Equals(opType.S))
            {
                if (BLL.Tools.Has_Service(klient, "POW-*", "selSewisy"))
                {
                    Reminder_Forms.CreateNew(item.Web, klient, okresId);
                    sbForms.AppendFormat("<li>{0}</li>", "POW");
                }
            }
            else
            {
                Reminder_Forms.CreateNew(item.Web, klient, okresId);
                sbForms.AppendFormat("<li>{0}</li>", "POW");
            }

        }

        private void Set_Klient_ExecuteCode(object sender, EventArgs e)
        {
            klient = (SPListItem)myEnum.Current;
            klientId = klient.ID;

            sbForms = new StringBuilder();

            logKlient_HistoryOutcome = BLL.Tools.Get_Text(klient, "_NazwaPrezentowana");
        }



        private void send_CtrlMsg_MethodInvoking(object sender, EventArgs e)
        {
            msgTo = workflowProperties.OriginatorEmail;
            msgSubject = String.Format(_SUBJECT_TEMPLATE, item.ID.ToString(), "rozpoczęte");
            sb = new StringBuilder();
        }

        private void send_CtrlMsg2_MethodInvoking(object sender, EventArgs e)
        {
            msgSubject = String.Format(_SUBJECT_TEMPLATE, item.ID.ToString(), "zakończone");

            if (sb.Length > 0) msgBody = String.Format(@"<ol>{0}</ol>", sb.ToString());

        }

        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fha = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fha != null)
            {
                logErrorMessage_HistoryDescription = fha.Fault.Message;
                logErrorMessage_HistoryOutcome = fha.Fault.StackTrace;

                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fha.Fault.Message, fha.Fault.StackTrace);

                Debug.WriteLine(fha.Fault.TargetSite);
                Debug.WriteLine(fha.Fault.Message);
                Debug.WriteLine(fha.Fault.StackTrace);

            }

        }

        private void Preset_ExecuteCode(object sender, EventArgs e)
        {
            item = workflowProperties.Item;
            selTypyKlientow = new SPFieldMultiChoiceValue(BLL.Tools.Get_Text(item, "enumTypKlienta"));
            selSerwisy = BLL.Tools.Get_LookupValueColection(item, "selSewisy");
            okresId = BLL.Tools.Get_LookupId(item, "selOkres");
        }

        private void Preset_ot_ExecuteCode(object sender, EventArgs e)
        {
            ot = opType.None;
        }

        private void hasManagedForms(object sender, ConditionalEventArgs e)
        {
            if (sbForms.Length > 0) e.Result = true;
        }

        private void UpdateMessage_ExecuteCode(object sender, EventArgs e)
        {
            string strKlient = BLL.Tools.Get_Text(klient, "_NazwaPrezentowana");
            string strManagedForms = string.Format(@"<ul>{0}</ul>", sbForms.ToString());

            logManagedForms_HistoryDescription = strKlient;
            logManagedForms_HistoryOutcome = strManagedForms;

            sb.AppendFormat(_LINE_TEMPLATE, strKlient + strManagedForms);
        }

        public String logManagedForms_HistoryOutcome = default(System.String);
        public String logManagedForms_HistoryDescription = default(System.String);


    }

    enum opType
    {
        TKandS,
        TK,
        S,
        None
    }
}