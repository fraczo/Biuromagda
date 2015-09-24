using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL.Models
{
    public class KontaKlienta
    {
        private SPWeb web;
        private int klientId;

        const string targetList = "Klienci";

        public KontaKlienta(SPWeb web, int klientId)
        {
            this.web = web;
            this.klientId = klientId;

            SPList list = web.Lists.TryGetList(targetList);
            if (list != null)
            {
                int urzadId = 0;

                SPListItem item = list.GetItemById(klientId);
                if (item != null)
                {

                    if (item["selUrzadSkarbowy"] != null)
                    {
                        urzadId = new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId;
                        NazwaUrzeduSkarbowego = new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupValue;
                        NazwaUrzeduSkarbowegoVAT = NazwaUrzeduSkarbowego;
                        IdUrzeduSkarbowego = urzadId;
                        IdUrzeduSkarbowegoVAT = urzadId;

                        KontoPIT = dicUrzedySkarbowe.Get_KontoPIT(web, urzadId);
                        KontoCIT = dicUrzedySkarbowe.Get_KontoCIT(web, urzadId);
                        KontoVAT = dicUrzedySkarbowe.Get_KontoVAT(web, urzadId);
                    }

                    //!!! DOTYCZY TYLKO KPIR

                    if (item.ContentType.Name == "KPiR")
                    {
                        if (item["selUrzadSkarbowyVAT"] != null) // czy jest dedykowany urząd skarbowy do rozliczeń VAT
                        {
                            urzadId = new SPFieldLookupValue(item["selUrzadSkarbowyVAT"].ToString()).LookupId;
                            NazwaUrzeduSkarbowegoVAT = new SPFieldLookupValue(item["selUrzadSkarbowyVAT"].ToString()).LookupValue;
                            KontoVAT = dicUrzedySkarbowe.Get_KontoVAT(web, urzadId);
                            IdUrzeduSkarbowego = urzadId;

                            KontoVAT = dicUrzedySkarbowe.Get_KontoVAT(web, urzadId);

                        }
                    }
                }
            }
        }

        public object KontoPIT { get; set; }
        public object KontoCIT { get; set; }
        public string KontoVAT { get; set; }

        public string NazwaUrzeduSkarbowego { get; set; }
        public string NazwaUrzeduSkarbowegoVAT { get; set; }

        public object IdUrzeduSkarbowego { get; set; }
        public object IdUrzeduSkarbowegoVAT { get; set; }


    }
}
