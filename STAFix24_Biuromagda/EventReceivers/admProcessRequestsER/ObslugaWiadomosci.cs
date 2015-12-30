using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.SharePoint;
using System.Diagnostics;

namespace EventReceivers.admProcessRequestsER
{
    public class ObslugaWiadomosci
    {
        static private Array results;
        static private IEnumerator myEnum;
        static private StringBuilder sb = new StringBuilder();

        internal static void Execute(Microsoft.SharePoint.SPItemEventProperties properties, Microsoft.SharePoint.SPWeb web)
        {
            results = BLL.tabWiadomosci.Select_Batch(web);

            foreach (SPListItem item in results)
            {
                BLL.Workflows.StartWorkflow(item, "Obsługa wiadomości");
                Debug.WriteLine("Workflow initiated for message #" + item.ID.ToString());
            }

        }
    }
}
