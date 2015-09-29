using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class tabKartyKontrolne
    {
        const string targetList = "Karty kontrolne";

        public static void Update_PD_Data(Microsoft.SharePoint.SPListItem item)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            Copy_PDFields(item, form);
            form.SystemUpdate();
        }

        private static void Copy_PDFields(Microsoft.SharePoint.SPListItem item, SPListItem form)
        {
            Copy_Field(item, form, "colPotwierdzenieOdbioruDokumento");
            Copy_Field(item, form, "colFormaOpodatkowaniaPD");
            Copy_Field(item, form, "enumRozliczeniePD");
            Copy_Field(item, form, "colPD_OcenaWyniku");
            Copy_Field(item, form, "colPD_WartoscDochodu");
            Copy_Field(item, form, "colPD_WartoscDoZaplaty");
            Copy_Field(item, form, "colPD_WartoscStraty");

            Copy_Field(item, "colNieWysylajDoKlienta", form, "_NieWysylajDoKlienta_PD");

            Copy_Id(item, form, "_ZadanieID_PD");
        }

        public static void Update_PDS_Data(SPListItem item)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            Copy_PDFields(item, form);
            Copy_Field(item, form, "colIloscDokWBPKN");
            Copy_Field(item, form, "colIloscFaktur");
            Copy_Field(item, form, "colKosztyNKUP");
            Copy_Field(item, form, "colKosztyNKUP_WynWyl");
            Copy_Field(item, form, "colKosztyNKUP_ZUSPlatWyl");
            Copy_Field(item, form, "colKosztyNKUP_FakWyl");
            Copy_Field(item, form, "colKosztyNKUP_PozostaleKoszty");
            Copy_Field(item, form, "colKosztyWS");
            Copy_Field(item, form, "colKosztyWS_WynWlaczone");
            Copy_Field(item, form, "colKosztyWS_ZUSPlatWlaczone");
            Copy_Field(item, form, "colKosztyWS_FakWlaczone");
            Copy_Field(item, form, "colPrzychodyNP");
            Copy_Field(item, form, "colPrzychodyZwolnione");
            Copy_Field(item, form, "colWplaconaSZ");
            Copy_Field(item, form, "colZyskStrataNetto");

            form.SystemUpdate();
        }

        public static void Update_VAT_Data(SPListItem item)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            Copy_Field(item, form, "colFormaOpodatkowaniaVAT");
            Copy_Field(item, form, "enumRozliczenieVAT");
            Copy_Field(item, form, "colVAT_WartoscNadwyzkiZaPoprzedn");
            Copy_Field(item, form, "colVAT_Decyzja");
            Copy_Field(item, form, "colVAT_WartoscDoZaplaty");
            Copy_Field(item, form, "colVAT_WartoscDoPrzeniesienia");
            Copy_Field(item, form, "colVAT_WartoscDoZwrotu");
            Copy_Field(item, form, "colVAT_eDeklaracja");
            Copy_Field(item, form, "colVAT_VAT-UE_Zalaczony");
            Copy_Field(item, form, "colVAT_VAT-27_Zalaczony");

            Copy_Field(item, "colNieWysylajDoKlienta", form, "_NieWysylajDoKlienta_VAT");

            Copy_Id(item, form, "_ZadanieID_VAT");
            
            form.SystemUpdate();
        }

        public static void Update_ZUS_Data(SPListItem item)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            Copy_Field(item, form, "colFormaOpodakowania_ZUS");
            Copy_Field(item, form, "colZUS_SP_Skladka");
            Copy_Field(item, form, "colZUS_ZD_Skladka");
            Copy_Field(item, form, "colZUS_FP_Skladka");
            Copy_Field(item, form, "colZatrudniaPracownikow");
            Copy_Field(item, form, "colZUS_PIT-4R_Zalaczony");
            Copy_Field(item, form, "colZUS_PIT-4R");
            Copy_Field(item, form, "colVAT_eDeklaracja");
            Copy_Field(item, form, "colZUS_PIT-8AR_Zalaczony");
            Copy_Field(item, form, "colZUS_PIT-8AR");
            Copy_Field(item, form, "colZUS_ListaPlac_Zalaczona");
            Copy_Field(item, form, "colZUS_Rachunki_Zalaczone");

            Copy_Field(item, "colNieWysylajDoKlienta", form, "_NieWysylajDoKlienta_ZUS");

            Copy_Id(item, form, "_ZadanieID_ZUS");

            form.SystemUpdate();
        }

        public static void Update_PD_DataWysylki(SPListItem item, DateTime dateTime)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            form["colPD_DataWylaniaInformacji"] = dateTime;
            form.SystemUpdate();
        }

        public static void Update_VAT_DataWysylki(SPListItem item, DateTime dateTime)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            form["colVAT_DataWyslaniaInformacji"] = dateTime;
            form.SystemUpdate();
        }

        public static void Update_ZUS_DataWysylki(SPListItem item, DateTime dateTime)
        {
            string KEY = Create_KEY(item);
            int formId = Get_KartaKontrolnaId(item, KEY);

            SPListItem form = Get_KartaKontrolnaById(item.Web, formId);
            form["colZUS_DataWyslaniaInformacji"] = dateTime;
            form.SystemUpdate();
        }


        private static SPListItem Get_KartaKontrolnaById(SPWeb web, int formId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            return list.Items.GetItemById(formId);
        }

        private static int Get_KartaKontrolnaId(SPListItem task, string KEY)
        {
            SPList list = task.Web.Lists.TryGetList(targetList);
            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i["KEY"].ToString() == KEY)
                .FirstOrDefault();
            if (item != null)
            {
                return item.ID;
            }
            else
            {
                SPListItem newItem = list.AddItem();
                newItem["KEY"] = KEY;
                newItem["selKlient"] = Get_LookupId(task, "selKlient");
                newItem["selOkres"] = Get_LookupId(task, "selOkres");

                BLL.Models.Klient k = new Models.Klient(task.Web, Get_LookupId(task, "selKlient"));

                newItem["enumRozliczeniePD"] = k.RozliczeniePD;
                newItem["enumRozliczenieVAT"] = k.RozliczenieVAT;
                newItem["colFormaOpodatkowaniaPD"] = k.FormaOpodatkowaniaPD;
                newItem["colFormaOpodatkowaniaVAT"] = k.FormaOpodatkowaniaVAT;
                newItem["colFormaOpodakowania_ZUS"] = k.FormaOpodatkowaniaZUS;

                //ustaw CT
                if (k.TypKlienta == "KSH") newItem["ContentType"] = "Karta kontrolna KSH";
                else newItem["ContentType"] = "Karta kontrolna KPiR";

                newItem.SystemUpdate();

                return newItem.ID;
            }

        }

        #region Helpers
        private static string Create_KEY(Microsoft.SharePoint.SPListItem item)
        {
            int klientId = Get_LookupId(item, "selKlient");
            int okresId = Get_LookupId(item, "selOkres");
            return string.Format(@"{0}::{1}", klientId.ToString(), okresId.ToString());
        }

        private static string Get_LookupValue(Microsoft.SharePoint.SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupValue : string.Empty;
        }

        private static int Get_LookupId(Microsoft.SharePoint.SPListItem item, string col)
        {
            return item[col] != null ? new SPFieldLookupValue(item[col].ToString()).LookupId : 0;
        }

        private static void Copy_Field(SPListItem item, string col0, SPListItem form, string col1)
        {
            form[col1] = item[col0];
        }

        private static void Copy_Field(SPListItem item, SPListItem form, string col)
        {
            form[col] = item[col];
        }

        private static void Copy_Id(SPListItem item, SPListItem form, string col)
        {
            form[col] = item.ID;
        }

        #endregion



    }
}
