using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class tabOkresy
    {
        const string targetList = "Okresy"; //"tabOkresy";

        public static void Get_ZUS_D(SPWeb web, int okresId, bool isChorobowa, bool isPracownicy, out double skladkaSP, out double skladkaZD, out double skladkaFP, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            skladkaSP = -1;
            skladkaZD = -1;
            skladkaFP = -1;
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {

                Get_ZUS_Terminy(isPracownicy, ref terminPlatnosci, ref terminPrzekazania, item);

                //duży ZUS - odczyt składek

                if (isChorobowa)
                {
                    skladkaSP = Double.Parse(item["colZUS_D_SPC_Skladka"].ToString());
                }
                else
                {
                    skladkaSP = Double.Parse(item["colZUS_D_SP_Skladka"].ToString());
                }

                skladkaZD = Double.Parse(item["colZUS_D_ZD_Skladka"].ToString());
                skladkaFP = Double.Parse(item["colZUS_D_FP_Skladka"].ToString());
            }
            //}
        }
        public static void Get_ZUS_M(SPWeb web, int okresId, bool isChorobowa, bool isPracownicy, out double skladkaSP, out double skladkaZD, out double skladkaFP, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            skladkaSP = -1;
            skladkaZD = -1;
            skladkaFP = -1;
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {

                Get_ZUS_Terminy(isPracownicy, ref terminPlatnosci, ref terminPrzekazania, item);

                //maly ZUS - odczyt składek

                if (isChorobowa)
                {
                    skladkaSP = Double.Parse(item["colZUS_M_SPC_Skladka"].ToString());
                }
                else
                {
                    skladkaSP = Double.Parse(item["colZUS_M_SP_Skladka"].ToString());
                }

                skladkaZD = Double.Parse(item["colZUS_M_ZD_Skladka"].ToString());
                skladkaFP = Double.Parse(item["colZUS_M_FP_Skladka"].ToString());

            }
            //}
        }
        public static void Get_ZUS_ZD(SPWeb web, int okresId, bool isChorobowa, bool isPracownicy, out double skladkaZD, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {

            skladkaZD = -1;

            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {

                Get_ZUS_Terminy(isPracownicy, ref terminPlatnosci, ref terminPrzekazania, item);

                //składka zdrowotna
                skladkaZD = Double.Parse(item["colZUS_D_ZD_Skladka"].ToString());

            }
            //}
        }

        public static void Get_ZUS_PRAC(SPWeb web, int okresId, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {

                Get_ZUS_Terminy(true, ref terminPlatnosci, ref terminPrzekazania, item);

            }
        }


        private static void Get_ZUS_Terminy(bool isPracownicy, ref DateTime terminPlatnosci, ref DateTime terminPrzekazania, SPListItem item)
        {
            if (isPracownicy)
            {
                terminPlatnosci = DateTime.Parse(item["colZUS_TerminPlatnosciSkladek_ZP"].ToString());

            }
            else
            {
                terminPlatnosci = DateTime.Parse(item["colZUS_TerminPlatnosciSkladek_Be"].ToString());
            }

            int offset = int.Parse(item["ZUS_TerminPrzekazaniaWynikow_Ofs"].ToString());
            terminPrzekazania = terminPlatnosci.AddDays(offset);
        }


        public static void Get_VAT_M(SPWeb web, int okresId, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {
                terminPlatnosci = DateTime.Parse(item["colVAT_TerminPlatnosciPodatku"].ToString());
                int ofset = int.Parse(item["colVAT_TerminPrzekazaniaWynikow_"].ToString());

                terminPrzekazania = terminPlatnosci.AddDays(ofset);
            }
            //}
        }

        public static void Get_VAT_KW(SPWeb web, int okresId, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {
                terminPlatnosci = DateTime.Parse(item["colVAT_TerminPlatnosciPodatkuKW"].ToString());
                int ofset = int.Parse(item["colVAT_TerminPrzekazaniaWynikow_"].ToString());

                terminPrzekazania = terminPlatnosci.AddDays(ofset);
            }
            //}
        }

        public static void Get_PD_M(SPWeb web, int okresId, int klientId, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            //if (list != null)
            //{
            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {
                int ofset = 0;


                tabKlienci.TypKlienta typKlienta = tabKlienci.Get_TypKlienta(web, klientId);

                switch (typKlienta.ToString())
                {
                    case "KPiR":
                    case "Osoba fizyczna":

                        terminPlatnosci = DateTime.Parse(item["colPD_TerminPlatnosciPodatku"].ToString());
                    ofset = int.Parse(item["colPD_TerminPrzekazaniaWynikow_O"].ToString());
                        break;

                    case "KSH":
                    case "Firma":

                        terminPlatnosci = DateTime.Parse(item["colCIT_TerminPlatnosciPodatku"].ToString());
                    ofset = int.Parse(item["colCIT_TerminPrzekazaniaWynikow_"].ToString());
                        break;

                    default:
                        break;
                }


                terminPrzekazania = terminPlatnosci.AddDays(ofset);


            }
            //}
        }

        public static void Get_PD_KW(SPWeb web, int okresId, int klientId, out DateTime terminPlatnosci, out DateTime terminPrzekazania)
        {
            terminPlatnosci = new DateTime();
            terminPrzekazania = new DateTime();

            SPList list = web.Lists.TryGetList(targetList);

            SPListItem item = list.GetItemById(okresId);

            if (item != null)
            {
                int ofset = 0;

                tabKlienci.TypKlienta typKlienta = tabKlienci.Get_TypKlienta(web, klientId);

                switch (typKlienta.ToString())
                {
                    case "KPiR":
                    case "Osoba fizyczna":

                        terminPlatnosci = DateTime.Parse(item["colPD_TerminPlatnosciPodatkuKW"].ToString());
                    ofset = int.Parse(item["colPD_TerminPrzekazaniaWynikow_O"].ToString());
                        break;

                    case "KSH":
                    case "Firma":

                        terminPlatnosci = DateTime.Parse(item["colCIT_TerminPlatnosciPodatkuKW"].ToString());
                    ofset = int.Parse(item["colCIT_TerminPrzekazaniaWynikow_"].ToString());
                        break;

                    default:
                        break;
                }

                terminPrzekazania = terminPlatnosci.AddDays(ofset);
            }

        }

    }
}
