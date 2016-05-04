using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using System.Diagnostics;

namespace EventReceivers.admProcessRequestsER
{
    public class Reminder_Forms
    {
        const string ctPOW_DOK = @"Prośba o dokumenty";
        const string ctPOW_WBANK = @"Prośba o przesłanie wyciągu bankowego";

        public static void CreateAll(SPWeb web, Array klienci, int okresId)
        {
            foreach (SPListItem item in klienci)
            {
                Debug.WriteLine("klientId=" + item.ID.ToString());

                SPFieldLookupValueCollection kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                foreach (SPFieldLookupValue kod in kody)
                {
                    if (kod.LookupValue == "POW-Dok")
                    {
                        Create_POW_DOK_Form(web, item.ID, okresId);
                    }
                    if (kod.LookupValue == "POW-WBank")
                    {
                        Create_POW_WBANK_Form(web, item.ID, okresId);
                    }
                }
            }
        }

        public static void Create(SPWeb web, int klientId, int okresId)
        {
            Debug.WriteLine("Create Reminder Forms");

            SPListItem item = tabKlienci.Get_KlientById(web, klientId);

            if (item != null)
            {
                SPFieldLookupValueCollection kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                foreach (SPFieldLookupValue kod in kody)
                {
                    if (kod.LookupValue == "POW-Dok")
                    {
                        Create_POW_DOK_Form(web, item.ID, okresId);
                    }
                    if (kod.LookupValue == "POW-WBank")
                    {
                        Create_POW_WBANK_Form(web, item.ID, okresId);
                    }
                }
            }
        }

        private static void Create_POW_DOK_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctPOW_DOK, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    //To do..
                    //uzupełnij dodatkowymi parametrami zasilającymi formatkę
                    tabZadania.Create_Form(web, ctPOW_DOK, klientId, okresId, key, 0);
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

        private static void Create_POW_WBANK_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(ctPOW_WBANK, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    //To do..
                    //uzupełnij dodatkowymi parametrami zasilającymi formatkę
                    tabZadania.Create_Form(web, ctPOW_WBANK, klientId, okresId, key, 0);
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
            Debug.WriteLine("Create Reminder Forms");

            if (item != null)
            {
                SPFieldLookupValueCollection kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
                foreach (SPFieldLookupValue kod in kody)
                {
                    if (kod.LookupValue == "POW-Dok")
                    {
                        Create_POW_DOK_Form(web, item.ID, okresId);
                    }
                    if (kod.LookupValue == "POW-WBank")
                    {
                        Create_POW_WBANK_Form(web, item.ID, okresId);
                    }
                }
            }
        }
    }
}
