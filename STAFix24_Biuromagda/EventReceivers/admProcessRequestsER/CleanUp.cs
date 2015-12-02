using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace EventReceivers.admProcessRequestsER
{
    class CleanUp
    {
        internal static void Execute(Microsoft.SharePoint.SPItemEventProperties properties, Microsoft.SharePoint.SPWeb web)
        {

            BLL.Workflows.StartWorkflow(properties.ListItem, "CleanUp");

        }
    }
}
