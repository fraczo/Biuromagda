using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace EventReceivers.admProcessRequestsER
{
    public class ObslugaZadan
    {
        public static void Execute(SPListItem item, Microsoft.SharePoint.SPWeb web)
        {
            ManageTasks_WorkingHours(item);
            ManageTasks(item);
        }

        private static void ManageTasks_WorkingHours(SPListItem item)
        {

            TimeSpan startTS = TimeSpan.Parse(BLL.admSetup.GetValue(item.Web, "PROC_TASK_START"));
            TimeSpan endTS = TimeSpan.Parse(BLL.admSetup.GetValue(item.Web, "PROC_TASK_END"));
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime.CompareTo(startTS) < 0 || currentTime.CompareTo(endTS) > 0)
            {
                //zatrzymaj procedurę jeżeli aktualny czas nie mieści się w dozwolonym przedziale
                return;
                //Manage_ProsbaOPrzeslanieWyciaguBankowego(properties);
                //Manage_ProsbaODokumenty(properties);
                //Manage_AudytowaneZadania(properties);
            }
        }

        private static void ManageTasks(SPListItem item)
        {
            // TODO:obsługa zadań nie podlegających restrykcjom czasowym

            Manage_ProsbaOPrzeslanieWyciaguBankowego(item);
            Manage_ProsbaODokumenty(item);
        }

        private static void Manage_ProsbaOPrzeslanieWyciaguBankowego(SPListItem item)
        {
            List<SPListItem> list = BLL.tabZadania.Get_ActiveTasksByContentType(item.Web, "Prośba o przesłanie wyciągu bankowego");
            foreach (SPListItem oItem in list)
            {
                Set_StatusZadania(oItem, "Gotowe");
                //uruchoma proces
            }
        }

        private static void Set_StatusZadania(SPListItem item, string status)
        {
            if (item["enumStatusZadania"]!=null)
            {
                item["enumStatusZadania"] = status;
                item.SystemUpdate();
            }
        }

        private static void Manage_ProsbaODokumenty(SPListItem item)
        {
            List<SPListItem> list = BLL.tabZadania.Get_ActiveTasksByContentType(item.Web, "Prośba o dokumenty");
            foreach (SPListItem oItem in list)
            {
                Set_StatusZadania(oItem, "Gotowe");
                //uruchom proces
            }
        } 
    }
}
