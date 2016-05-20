using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Models
{
    public enum ZadanieCT
    {
        Z, //"Zadanie"
        POD, //"Prośba o dokumenty"
        POPWB, //"Prośba o przesłanie wyciągu bankowego"
        RZBR, //"Rozliczenie z biurem rachunkowym"
        RPD, //"Rozliczenie podatku dochodowego"
        RPDS, //"Rozliczenie podatku dochodowego spółki"
        RPDW, //"Rozliczenie podatku dochodowego wspólnika
        RPV, // "Rozliczenie podatku VAT"
        RZ, //"Rozliczenie ZUS"
        WZR, // "Wiadomość z ręki"
        WZS, //"Wiadomość z szablonu"
        WG, // "Wiadomość grupowa"
        WGZS //"Wiadomość grupowa z szablonu"
    }
}
