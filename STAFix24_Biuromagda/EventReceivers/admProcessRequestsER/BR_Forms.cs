using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using Microsoft.SharePoint;

namespace admProcessRequests_EventReceiver.admProcessRequestsER
{
    public class BR_Forms
    {
        const string ctBR = @"Rozliczenie z biurem rachunkowym";

        internal static void Create(Microsoft.SharePoint.SPWeb web, int klientId, int okresId)
        {
            SPListItem item = tabKlienci.Get_KlientById(web, klientId);

            if (item != null)
            {
                SPFieldLookupValueCollection kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
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

        internal static void Create(Microsoft.SharePoint.SPWeb web, Array aKlienci, int okresId)
        {
            foreach (SPListItem item in aKlienci)
            {
                SPFieldLookupValueCollection kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());
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

        private static void Create_BR_Form(SPWeb web, int klientId, int okresId)
        {
            string key = tabZadania.Define_KEY(ctBR, klientId, okresId);
            if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
            {
                tabZadania.Create_ctBR_Form(web, ctBR, klientId, okresId, key);
            }
        }
    }
}
