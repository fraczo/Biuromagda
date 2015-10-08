using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using BLL;

namespace admProcessRequests_EventReceiver
{
    internal class ImportPrzeterminowanychNaleznosci
    {
        const string targetList = @"Przeterminowane należności - import"; //"intPrzeterminowaneNaleznosci";

        internal static void Execute(Microsoft.SharePoint.SPItemEventProperties properties, Microsoft.SharePoint.SPWeb web)
        {
            string mode = properties.ListItem["cmdPrzeterminowaneNaleznosci"] != null ? properties.ListItem["cmdPrzeterminowaneNaleznosci"].ToString() : string.Empty;

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
                foreach (SPListItem item in aRekordy)
                {
                    string dluznik = item["Title"].ToString();

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


                    foreach (SPListItem item in items)
                    {
                        item["selKlient"] = klientId;
                        item["selWiadomoscWBuforze"] = 0;
                        item.SystemUpdate();


                        StringBuilder sbRow = new StringBuilder(rowTemplate);
                        sbRow.Replace("___colNumerFaktury___", item["colNumerFaktury"].ToString());
                        sbRow.Replace("___colDataSprzedazy___", item["colDataSprzedazy"].ToString());
                        sbRow.Replace("___colDataWystawienia___", item["colDataWystawienia"].ToString());
                        sbRow.Replace("___colTerminPlatnosci___", item["colTerminPlatnosci"].ToString());

                        //int dniZwloki = (DateTime.Today - DateTime.Parse(item["colTerminPlatnosci"].ToString())).Days;

                        double value1 = Double.Parse(item["colKwotaFaktury"].ToString());
                        value1total = value1total + value1;
                        double value2 = Double.Parse(item["colKwotaDlugu"].ToString());
                        value2total = value2total + value2;

                        sbRow.Replace("___colKwotaFaktury___", value1.ToString());
                        sbRow.Replace("___colKwotaDlugu___", value2.ToString());
                        //sbRow.Replace("___DniZwloki___", dniZwloki.ToString());
                        //sbRow.Replace("___colDniZwloki___", string.Empty);

                        sb.Append(sbRow);


                    }

                    string temat;
                    string trescHTML;

                    //StringBuilder sb0 = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TEMPLATE.Include", false));
                    dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TEMPLATE.Include", out temat, out trescHTML, false);
                    StringBuilder sb0 = new StringBuilder(trescHTML);
                    sb0.Replace("___TABLE_ROW___", sb.ToString());
                    sb0.Replace("___colKwotaFakturyRazem___", value1total.ToString());
                    sb0.Replace("___colKwotaDluguRazem___", value2total.ToString());

                    
                    StringBuilder lt = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_LEADING_TEXT", false));
                    lt.Replace("___FIRMA___", BLL.tabKlienci.Get_NazwaFirmyById(web, klientId));
                    lt.Replace("___ADRES___", BLL.tabKlienci.Get_PelnyAdresFirmyById(web, klientId));
                    lt.Replace("___DATA___", DateTime.Now.ToShortDateString());
                    sb0.Replace("___OVERDUE_PAYMENTS_LEADING_TEXT___", lt.ToString());

                    StringBuilder tt = new StringBuilder(dicSzablonyKomunikacji.Get_TemplateByKod(web, "OVERDUE_PAYMENTS_TRAILING_TEXT", false));
                    tt.Replace("___DATA___", DateTime.Now.ToShortDateString());
                    tt.Replace("___KwotaDoZaplaty___", value2total.ToString());
                    sb0.Replace("___OVERDUE_PAYMENTS_TRAILING_TEXT___", tt.ToString());


                    if (mode == "Import")
                    {
                        //zapisz w buforze wiadomości o ile 

                        string nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");
                        string odbiorca = BLL.tabKlienci.Get_EmailById(web, klientId);
                        string kopiaDla = BLL.dicOperatorzy.EmailByUserId(web, properties.CurrentUserId);
                        temat = temat;
                        trescHTML = sb0.ToString();
                        BLL.tabWiadomosci.AddNew(web, nadawca, odbiorca, kopiaDla, false, true, temat, string.Empty, trescHTML, new DateTime(), 0, klientId);

                        foreach (SPListItem item in items)
                        {
                            //item.Delete();
                        }
                    }
                    else //Weryfikcja
                    {
                        int messageId = BLL.intBuforWiadomosci.AddNewItem(web, klientId, sb0.ToString(), BLL.tabSzablonyWiadomosci.GetSzablonId(web, "Informacja o zadłużeniu"));

                        foreach (SPListItem item in items)
                        {
                            item["selWiadomoscWBuforze"] = messageId;
                            item.SystemUpdate();
                        }
                    }
                }
            }

        }
    }
}
