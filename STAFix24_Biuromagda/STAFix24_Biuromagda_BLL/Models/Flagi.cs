using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL.Models
{
    public class Flagi
    {
        public Flagi(SPWeb web, int klientId)
        {

            PrzypomnienieOTerminiePlatnosci = false;
            GenerowanieDrukuWplaty = false;
            AudytDanych = false;


            if (tabKlienci.IsServiceAssgned(web, klientId, "POT"))
            {
                PrzypomnienieOTerminiePlatnosci = true;
            }

            if (tabKlienci.IsServiceAssgned(web, klientId, "GBW"))
            {
                GenerowanieDrukuWplaty = true;
            }

            if (tabKlienci.IsServiceAssgned(web, klientId, "AD") || tabKlienci.IsServiceAssgned(web, klientId, "ADO"))
            {
                AudytDanych = true;
            }
            
        }

        public bool PrzypomnienieOTerminiePlatnosci { get; set; }
        public bool GenerowanieDrukuWplaty { get; set; }
        public bool AudytDanych { get; set; }



    }
}
