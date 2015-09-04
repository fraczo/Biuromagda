using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL.Models
{
    public class Klient
    {
        private Microsoft.SharePoint.SPWeb web;
        private int klientId;

        public Klient(Microsoft.SharePoint.SPWeb web, int klientId)
        {
            this.web = web;
            this.klientId = klientId;

            SPListItem item = tabKlienci.Get_KlientById(web, klientId);

            if (item != null)
            {
                OddzialZUSId = item["selOddzialZUS"] != null ? new SPFieldLookupValue(item["selOddzialZUS"].ToString()).LookupId : 0;
                FormaOpodatkowaniaZUS = item["colFormaOpodakowania_ZUS"] != null ? item["colFormaOpodakowania_ZUS"].ToString() : string.Empty;
                FormaOpodatkowaniaKPiR = item["colFormaOpodatkowaniaPD_KPiR"] != null ? item["colFormaOpodatkowaniaPD_KPiR"].ToString() : string.Empty;
                FormaOpodatkowaniaKSH = item["colFormaOpodatkowaniaPD_KSH"] != null ? item["colFormaOpodatkowaniaPD_KSH"].ToString() : string.Empty;
                FormaOpodatkowaniaVAT = item["colFormaOpodatkowaniaVAT"] != null ? item["colFormaOpodatkowaniaVAT"].ToString() : string.Empty;


                string ct = item["ContentType"].ToString();
                switch (ct)
                {
                    case "KPiR":
                    case "Osoba fizyczna":
                        FormaOpodatkowaniaPD = FormaOpodatkowaniaKPiR;
                        break;
                    case "KSH":
                    case "Firma":
                        FormaOpodatkowaniaPD = FormaOpodatkowaniaKSH;
                        break;
                    default:
                        FormaOpodatkowaniaPD = string.Empty;
                        break;
                }

                OsobaDoKontaktu = item["colOsobaDoKontaktu"] != null ? item["colOsobaDoKontaktu"].ToString() : string.Empty;
                Email = item["colEmail"] != null ? item["colEmail"].ToString() : string.Empty;
                Telefon = item["colTelefon"] != null ? item["colTelefon"].ToString() : string.Empty;
                Adres = item["colAdres"] != null ? item["colAdres"].ToString() : string.Empty;
                KodPocztowy = item["colKodPocztowy"] != null ? item["colKodPocztowy"].ToString() : string.Empty;
                Miejscowosc = item["colMiejscowosc"] != null ? item["colMiejscowosc"].ToString() : string.Empty;
                NIP = item["colNIP"] != null ? item["colNIP"].ToString() : string.Empty;
                NazwaFirmy = item.Title;
                Regon = item["colRegon"] != null ? item["colRegon"].ToString() : string.Empty;

                UwagiKadrowe = item["colUwagiKadrowe"] != null ? item["colUwagiKadrowe"].ToString() : string.Empty;


                int TerminPlatnosci = 0;
                if (item["selTerminPlatnosci"] != null)
                {
                    int terminPlatnosciId = new SPFieldLookupValue(item["selTerminPlatnosci"].ToString()).LookupId;
                    TerminPlatnosci = dicTerminyPlatnosci.Get_TerminPlatnosci(web, terminPlatnosciId);
                }

                int urzadId = item["selUrzadSkarbowy"] != null ? new SPFieldLookupValue(item["selUrzadSkarbowy"].ToString()).LookupId : 0;
                if (urzadId>0)
                {
                    NumerRachunkuPIT = tabUrzedySkarbowe.Get_NumerRachunkuPITById(web, urzadId);
                    NazwaUrzeduSkarbowego = tabUrzedySkarbowe.Get_NazwaUrzeduById(web, urzadId);
                    UrzadSkarbowyId = urzadId;
                }
                else
                {
                    NumerRachunkuPIT = string.Empty;
                    UrzadSkarbowyId = 0;
                }
                
            }
        }

        public string FormaOpodatkowaniaZUS { get; set; }
        public string FormaOpodatkowaniaKPiR { get; set; }
        public string FormaOpodatkowaniaKSH { get; set; }

        public object FormaOpodatkowaniaPD { get; set; }

        public string FormaOpodatkowaniaVAT { get; set; }
        public string OsobaDoKontaktu { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public string KodPocztowy { get; set; }
        public string Miejscowosc { get; set; }
        public int OddzialZUSId { get; set; }
        public int TerminPlatnosci { get; set; }
        public string NIP { get; set; }

        public string NazwaFirmy { get; set; }

        public string Regon { get; set; }

        public string NumerRachunkuPIT { get; set; }

        public string NazwaUrzeduSkarbowego { get; set; }

        public string Get_NazwaNadawcyPrzelewu()
        {
            string s=  NazwaFirmy + " " + Adres + " " + KodPocztowy+ " " + Miejscowosc;
            s = s.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Trim();
            return s;

        }

        public int UrzadSkarbowyId { get; set; }
        public string UwagiKadrowe { get; set; }
    }
}
