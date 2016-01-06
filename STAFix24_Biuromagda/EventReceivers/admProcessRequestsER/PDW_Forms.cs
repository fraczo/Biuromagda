using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;
using System.Diagnostics;

namespace EventReceivers.admProcessRequestsER
{
    public class PDW_Forms
    {

        const string _CT_NAME_PDW = @"Rozliczenie podatku dochodowego wspólnika";

        public static void CreateNew(SPWeb web, SPListItem item, int okresId)
        {
            if (item != null)
            {
                SPFieldLookupValueCollection kody;

                kody = new SPFieldLookupValueCollection(item["selSewisy"].ToString());

                foreach (SPFieldLookupValue kod in kody)
                {
                    switch (kod.LookupValue)
                    {
                        case @"PDW-M":
                            Create_PDW_M_Form(web, item.ID, okresId);
                            break;
                        case @"PDW-KW":
                            Create_PDW_KW_Form(web, item.ID, okresId);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void Create_PDW_KW_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(_CT_NAME_PDW, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    Debug.WriteLine("PDW_KW klient:" + klientId.ToString());

                    //zainicjowanie formatki PDS

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_KW(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDW_Form(web, _CT_NAME_PDW, klientId, okresId, key, terminPlatnosci, terminPrzekazania, true);

                }
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());

            }
        }

        private static void Create_PDW_M_Form(SPWeb web, int klientId, int okresId)
        {
            try
            {
                string key = tabZadania.Define_KEY(_CT_NAME_PDW, klientId, okresId);
                if (tabZadania.Check_KEY_IsAllowed(key, web, 0))
                {
                    Debug.WriteLine("PDW_M klient:" + klientId.ToString());

                    DateTime terminPlatnosci;
                    DateTime terminPrzekazania;

                    //terminy płatności VAT KSH jak dla KPiR
                    tabOkresy.Get_PD_M(web, okresId, klientId, out terminPlatnosci, out terminPrzekazania);

                    tabZadania.Create_ctPDW_Form(web, _CT_NAME_PDW, klientId, okresId, key, terminPlatnosci, terminPrzekazania, false);
                }
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(web.Url, ex.ToString() + " KlientId= " + klientId.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, web.Url, "KlientId=" + klientId.ToString());
            }
        }
    }
}
