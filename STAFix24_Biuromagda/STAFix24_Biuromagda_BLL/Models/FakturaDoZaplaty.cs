using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL.Models
{
    public class FakturaDoZaplaty
    {
        public FakturaDoZaplaty(SPWeb web, int klientId)
        {
            BLL.Models.Klient iok = new Klient(web, klientId);
            this.KlientId = klientId;
            this.Web = web;
            this.DrukWplatyWymagany = iok.DrukWplatyWymagany;
            this.PrzypomnienieOTerminiePlatnosciWymagane = iok.PrzypomnienieOTerminiePlatnosciWymagane;
            this.EmailOdbiorcy = iok.Email;
            this.NazwaKlienta = iok.PelnaNazwaFirmy;
            this.AdresNadawcy = string.Format("{0} {1} {2}",iok.Adres, iok.KodPocztowy, iok.Miejscowosc).Trim();
            this.Wyslana = false;
            this.KK_Zaktualizowana = false;
            this.KK_Id = 0;
        }

        public SPWeb Web { get; set; }
        public DateTime DataWystawieniaFaktury { get; set; }
        public string NumerFaktury { get; set; }
        public double WartoscDoZaplaty { get; set; }
        public DateTime TerminPlatnosci { get; set; }
        public bool PrzypomnienieOTerminiePlatnosciWymagane { get; set; }
        public bool DrukWplatyWymagany { get; set; }
        public string FakturaPDF_Url { get; set; }
        public bool FakturaPDF_Exist()
        {
            if (this.FakturaPDF_Url != null)
            {
                SPFile file = Web.GetFile(this.FakturaPDF_Url);
                if (file.Exists) return true;
            }
            return false;
        }

        public int KlientId { get; set; }

        public string EmailNadawcy { get; set; }

        public string EmailOdbiorcy { get; set; }

        public string Okres { get; set; }

        public string InformacjaDlaKlienta { get; set; }

        public string NazwaKlienta { get; set; }

        public bool Wyslana { get; set; }

        public bool KK_Zaktualizowana { get; set; }

        //identyfikator pliku pdf
        public int  PDF_Id { get; set; }

        //identyfikator rekordu źródłowego
        public int IOF_Id { get; set; }

        public int KK_Id { get; set; }

        public string AdresNadawcy { get; set; }
    }
}
