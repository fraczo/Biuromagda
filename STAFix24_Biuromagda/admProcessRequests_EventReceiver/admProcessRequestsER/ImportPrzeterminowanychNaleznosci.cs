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

            //if (list != null)
            //{
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
                        string rowTemplate = admSetup.GetText(web, "IOZ_TABLE_TEMPLATE_ROW");
                        double value1total = 0;
                        double value2total = 0;


                        foreach (SPListItem item in items)
                        {
                            item["selKlient"] = klientId;
                            item["selWiadomoscWBuforze"] = 0;
                            item.Update();


                            StringBuilder sbRow = new StringBuilder(rowTemplate);
                            sbRow.Replace("___NumerFaktury___", item["colNumerFaktury"].ToString());
                            sbRow.Replace("___DataSprzedazy___", item["colDataSprzedazy"].ToString());
                            sbRow.Replace("___DataWystawienia___", item["colDataWystawienia"].ToString());
                            sbRow.Replace("___TerminPlatnosci___", item["colTerminPlatnosci"].ToString());

                            //int dniZwloki = (DateTime.Today - DateTime.Parse(item["colTerminPlatnosci"].ToString())).Days;

                            double value1 = Double.Parse(item["colKwotaFaktury"].ToString());
                            value1total = value1total + value1;
                            double value2 = Double.Parse(item["colKwotaDlugu"].ToString());
                            value2total = value2total + value2;

                            sbRow.Replace("___KwotaFaktury___", value1.ToString());
                            sbRow.Replace("___KwotaDlugu___", value2.ToString());
                            //sbRow.Replace("___DniZwloki___", dniZwloki.ToString());
                            sbRow.Replace("___DniZwloki___", string.Empty);

                            sb.Append(sbRow);


                        }


                        StringBuilder sbTable = new StringBuilder(admSetup.GetText(web, "IOZ_TABLE_TEMPLATE_NOROWS"));

                        sbTable.Replace("___ROWS___", sb.ToString());
                        sbTable.Replace("___RazemKwotaFaktury___", value1total.ToString());
                        sbTable.Replace("___RazemKwotaDlugu___", value2total.ToString());

                        if (mode == "Import")
                        {
                            //zapisz w buforze wiadomości o ile 

                            int messageId = BLL.intBuforWiadomosci.AddNewItem(web, klientId, sbTable.ToString(), BLL.tabSzablonyWiadomosci.GetSzablonId(web, "Informacja o zadłużeniu"));

                            foreach (SPListItem item in items)
                            {
#if true
                                item.Delete();
#else
                                item["selWiadomoscWBuforze"] = messageId;
                                item.Update();
#endif
                                
                            }
                        }
                    }
                }
            //}
        }
    }
}
