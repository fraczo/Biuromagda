﻿using System;
using System.Linq;
using Microsoft.SharePoint;

namespace SharePointConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            const string webUrl = @"http://spf2010/sites/biuromagda";
            //const string webUrl = @"http://biuromagda.stafix24.pl";


            using (SPSite site = new SPSite(webUrl))
            {
                using (SPWeb web = site.RootWeb)
                {
                    SPList list = web.Lists.TryGetList("Zadania");

                    foreach (SPListItem item in list.Items)
                    {
                        Console.WriteLine(item.ID.ToString());
                    }

                }
            }

            Console.WriteLine("koniec");
            Console.ReadKey();
        }

        #region Set 1
        //if (list != null)
        //{
        //    SPListItem item  = list.GetItemById(1091);
        //    if (item != null)
        //    {
        //        string fname = DateTime.Now.ToString()+ ".pdf";
        //        double wartosc = 1000 + int.Parse(DateTime.Now.Minute.ToString());

        //        //bool r2 = DrukWplaty.Attach_DrukWplaty(web, item, fname, "1mandhfjgk2kdjcudhs3jdkfjshcn4kdjfncmsn5mskfjcurd6mzdjfnchv7jdhvnxbdf", "01A234B678C012D456E890F234", wartosc, "1QWDFREAXZ2QWDFREAXZ3QWDFREAXZ4QWDFREAXZ5QWDFREAXZ6QWDFREAXZ7QWDFREAXZ8QWDFREAXZ", "Dotyczy faktury za płatność cząstkową do zlecenia numer &#984746647566475849. Płatne w terminie 1234 dni od daty rozpoczęcia");
        //        //string result = GenerujDrukWplaty("nazwa pliku.pdf");

        //        //string result = BLL.DrukWplaty.Generuj("Druk wpłaty.pdf");

        //        //string result = BLL.GenTest.Generuj("druk.pdf");

        //        //GeneratorPrzelewow o = new BLL.GeneratorPrzelewow();
        //        //bool result = o.Attach_DrukWplaty(web, item, @"12_Naźeczońół-@#$test", "nadawca", "odbiorca", 1234.45);
        //    }
        //}

        //Console.WriteLine(tabProcedury.GetID(web, ":Moja procedura", true)); 
        #endregion
    }
}
