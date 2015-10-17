using System;
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
            int procId = 0;
            if (item["selProcedura"]!=null)
            {
                procName = new SPFieldLookupValue(item["selProcedura"].ToString()).LookupValue;
                procId = new SPFieldLookupValue(item["selProcedura"].ToString()).LookupId;
            }

            switch (procName)
            {
                case ": Rozliczenie podatku dochodowego":
                case ": Rozliczenie podatku dochodowego spółki":
                case ": Rozliczenie podatku VAT":
                case ": Rozliczenie ZUS":
                    Array tasks = BLL.tabZadania.Get_GotoweZadaniaByProceduraId(web, procId);
                    foreach (SPListItem task in tasks)
                    {
                        //Sprawdź czy klient ma ustawiony serwis AD czy ADO
                        //w przypadku AD zablokuj automatyczną akceptację

                        if (BLL.tabKlienci.Has_ServiceById(task.Web, BLL.Tools.Get_LookupId(task, "selKlient"), "ADO"))
                        {
                            //uruchom proces zatwierdzenia
                            BLL.WorkflowHelpers.StartWorkflow(task, "Zatwierdzenie zadania");
                        }
                    }
                    
                    break;

                case ": Prośba o dokumenty":
                case ": Prośba o przesłanie wyciągu bankowego":
                case ": Rozliczenie z biurem rachunkowym":
                    Array tasks2 = BLL.tabZadania.Get_AktywneZadaniaByProceduraId(web, procId);
                    foreach (SPListItem task in tasks2)
                    {
                            BLL.WorkflowHelpers.StartWorkflow(task, "Zatwierdzenie zadania");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
