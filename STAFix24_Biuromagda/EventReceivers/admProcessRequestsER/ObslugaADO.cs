using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace EventReceivers.admProcessRequestsER
{
    class ObslugaADO
    {
        internal static void Execute(Microsoft.SharePoint.SPItemEventProperties properties, Microsoft.SharePoint.SPWeb web)
        {
            SPListItem item = properties.ListItem;

            // sprawdź czy wybrana procedura jest obsługiwana
            string procName = string.Empty;
            if (item["selProcedura"]!=null)
            {
                procName = new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue;
            }

            switch (procName)
            {
                case ": Rozliczenie podatku dochodowego":
                case ": Rozliczenie podatku dochodowego spółki":
                case ": Rozliczenie podatku VAT":
                case ": Rozliczenie ZUS":
                    Array tasks = BLL.tabZadania.Get_GotoweTasksByProceduraId(web, new SPFieldLookupValue(item["selProcedura"].ToString()).LookupId);
                    foreach (SPListItem task in tasks)
                    {
                        Set_Command(task, "Zatwierdź");
                        //EventReceivers.tabZadaniaER.tabZadaniaER o = new tabZadaniaER.tabZadaniaER();
                        //o.Execute(task);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        private static void Set_Command(SPListItem item, string command)
        {
            item["cmdFormatka"] = command;
            item.Update();
        }
    }
}
