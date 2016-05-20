using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Diagnostics;
using BLL;

namespace EventReceivers.admProcessRequestsER
{
    public class BR_Forms
    {
        const string ctBR = @"Rozliczenie z biurem rachunkowym";

        public static void CreateAll(SPWeb web, Array aKlienci, int okresId, bool createKK)
        {
            foreach (SPListItem item in aKlienci)
            {
                Debug.WriteLine("klientId=" + item.ID.ToString());

                SPFieldLookupValueCollection kody;

                switch (item.ContentType.Name)
                {
                    case "Osoba fizyczna":
                    case "Firma":
                        kody = new SPFieldLookupValueCollection(item["selSerwisyWspolnicy"].ToString());
                        break;
                    default:
                        kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                        break;
                }

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"RBR":
                            if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                            Create_BR_Form(web, item.ID, okresId);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static void Create(SPWeb web, int klientId, int okresId, bool createKK)
        {
            Debug.WriteLine("Create RBR Form");

            SPListItem item = tabKlienci.Get_KlientById(web, klientId);

            SPFieldLookupValueCollection kody;

            switch (item.ContentType.Name)
            {
                case "Osoba fizyczna":
                case "Firma":
                    kody = new SPFieldLookupValueCollection(item["selSerwisyWspolnicy"].ToString());
                    break;
                default:
                    kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                    break;
            }

            foreach (SPFieldLookupValue kod in kody)
            {
                switch (kod.LookupValue)
                {
                    case @"RBR":
                        if (createKK) BLL.tabKartyKontrolne.Create_KartaKontrolna(web, item.ID, okresId);

                        Create_BR_Form(web, item.ID, okresId);
                        break;
                    default:
                        break;
                }
            }

        }

        private static void Create_BR_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctBR, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    tabZadania.Create_ctBR_Form(web, ctBR, klientId, okresId, key);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
#endif

            }
        }





        public static void CreateNew(SPWeb web, SPListItem item, int okresId)
        {
            Debug.WriteLine("Create RBR Form");

            SPFieldLookupValueCollection kody;

            kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());

            foreach (SPFieldLookupValue kod in kody)
            {
                switch (kod.LookupValue)
                {
                    case @"RBR":
                        Create_BR_Form(web, item.ID, okresId);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
