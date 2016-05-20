using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;

namespace EventReceivers.admProcessRequestsER
{
    public class ImportPrzeterminowanychNaleznosci
    {
        const string targetList = @"Przeterminowane należności - import"; //"intPrzeterminowaneNaleznosci";

        public static void Execute(SPListItem item, Microsoft.SharePoint.SPWeb web)
        {
            string mode = item["cmdPrzeterminowaneNaleznosci"] != null ? item["cmdPrzeterminowaneNaleznosci"].ToString() : string.Empty;

            SPList list = web.Lists.TryGetList(targetList);


            Array aRekordy = list.Items.Cast<SPListItem>()
                .OrderBy(i => i["Title"].ToString())
                .ThenBy(i => i["colDataSprzedazy"].ToString())
                .ThenBy(i => i["colTerminPlatnosci"].ToString())
                .ToArray();

            List<string> lstDluznicy = new List<string>();

            if (aRekordy != null)
            {
                string klient = string.Empty;
                string klient0 = string.Empty;
                foreach (SPListItem oItem in aRekordy)
                {
                    string dluznik = oItem["Title"].ToString();

                    if (!lstDluznicy.Exists(i => i == dluznik))
                    {
                        lstDluznicy.Add(dluznik);
                    }
                }
            }

            foreach (string dluznik in lstDluznicy)
            {
                int klientId = tabKlienci.Get_KlientId_BestFit(web, dluznik);
                if (klientId > 0)
                {
                    Array items = list.Items.Cast<SPListItem>()
                        .Where(i => i["Title"].ToString() == dluznik)
                        .ToArray();

                    StringBuilder sb = new StringBuilder();
                    string rowTemplate = dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TR_TEMPLATE", false);
                    double value1total = 0;
                    double value2total = 0;


                    foreach (SPListItem oItem in items)
                    {
                        oItem["selKlient"] = klientId;
                        oItem.SystemUpdate();


                        StringBuilder sbRow = new StringBuilder(rowTemplate);
                        sbRow.Replace("___colNumerFaktury___", oItem["colNumerFaktury"].ToString());
                        sbRow.Replace("___colDataSprzedazy___", oItem["colDataSprzedazy"].ToString());
                        sbRow.Replace("___colDataWystawienia___", oItem["colDataWystawienia"].ToString());
                        sbRow.Replace("___colTerminPlatnosci___", oItem["colTerminPlatnosci"].ToString());

                        //int dniZwloki = (DateTime.Today - DateTime.Parse(oItem["colTerminPlatnosci"].ToString())).Days;

                        double value1 = Double.Parse(oItem["colKwotaFaktury"].ToString());
                        value1total = value1total + value1;
                        double value2 = Double.Parse(oItem["colKwotaDlugu"].ToString());
                        value2total = value2total + value2;

                        sbRow.Replace("___colKwotaFaktury___", BLL.Tools.Format_Currency(value1));
                        sbRow.Replace("___colZaplacono___", BLL.Tools.Format_Currency(value1 - value2));
                        sbRow.Replace("___colKwotaDlugu___", BLL.Tools.Format_Currency(value2));

                        sb.Append(sbRow);


                    }

                    string temat;
                    string trescHTML;

                    //StringBuilder sb0 = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TEMPLATE.Include", false));
                    dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TEMPLATE.Include", out temat, out trescHTML, false);
                    StringBuilder sb0 = new StringBuilder(trescHTML);
                    sb0.Replace("___TABLE_ROW___", sb.ToString());
                    sb0.Replace("___colKwotaFakturyRazem___", BLL.Tools.Format_Currency(value1total));
                    sb0.Replace("___colKwotaDluguRazem___", BLL.Tools.Format_Currency(value2total));


                    StringBuilder lt = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_LEADING_TEXT", false));
                    lt.Replace("___FIRMA___", BLL.tabKlienci.Get_NazwaFirmyById(web, klientId));
                    lt.Replace("___ADRES___", BLL.tabKlienci.Get_PelnyAdresFirmyById(web, klientId));
                    lt.Replace("___DATA___", DateTime.Now.ToShortDateString());
                    sb0.Replace("___OVERDUE_PAYMENTS_LEADING_TEXT___", lt.ToString());

                    StringBuilder tt = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TRAILING_TEXT", false));
                    tt.Replace("___DATA___", DateTime.Now.ToShortDateString());
                    tt.Replace("___KwotaDoZaplaty___", BLL.Tools.Format_Currency(value2total));
                    sb0.Replace("___OVERDUE_PAYMENTS_TRAILING_TEXT___", tt.ToString());


                    if (mode == "Import")
                    {
                        //zapisz w buforze wiadomości o ile 

                        string nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");
                        string odbiorca = BLL.tabKlienci.Get_EmailById(web, klientId);
                        string kopiaDla = BLL.dicOperatorzy.EmailByUserId(web, item.Web.CurrentUser.ID);

                        //dodanie nazwy firmy do tematu
                        temat = BLL.Tools.AddCompanyName(web, temat, klientId);

                        trescHTML = sb0.ToString();

                        BLL.tabWiadomosci.AddNew(web, null, nadawca, odbiorca, kopiaDla, false, true, temat, string.Empty, trescHTML, new DateTime(), 0, klientId, BLL.Models.Marker.NoAttachements);

                        foreach (SPListItem oItem in items)
                        {
#if DEBUG
                            // w trybie debugowania przetworzony rekord nie jest usówany z tablicy źródłowej
#else
                            oItem.Delete();
#endif

                        }
                    }

                    // jeżeli mode "Weryfikacja" to jedynie mamy przypisanie kodu klient do rekordu i dzięki temu wiemy czy rekord został rozpoznany czy nie

                }
            }

        }
    }
}
