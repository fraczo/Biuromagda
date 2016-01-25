using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;
using System.Text;
using BLL.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace EventReceivers.tabZadaniaER
{
    public class tabZadaniaER : SPItemEventReceiver
    {

        #region Event Handlers
        public override void ItemAdding(SPItemEventProperties properties)
        {
            Validate(properties);
        }

        public override void ItemAdded(SPItemEventProperties properties)
        {
            Run_tabZadaniaWF(properties);
        } 

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Run_tabZadaniaWF(properties);
        }
        #endregion

        #region Helpers
        private void Run_tabZadaniaWF(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            Debug.WriteLine("tabZadania.ItemUpdated #" + properties.ListItemId.ToString());
            BLL.Workflows.StartWorkflow(properties.ListItem, "tabZadaniaWF");
            Debug.WriteLine("tabZadania.tabZadanieWF initiated #" + properties.ListItemId.ToString());
            this.EventFiringEnabled = true;
        }

        private void Validate(SPItemEventProperties properties)
        {
            BLL.Logger.LogEvent(properties.Web.ToString(), "Zadanie.Valpidate_" + properties.ListItemId.ToString());

            string ct = properties.AfterProperties["ContentType"] != null ? properties.AfterProperties["ContentType"].ToString() : string.Empty;
            int klientId = properties.AfterProperties["selKlient"] != null ? new SPFieldLookupValue(properties.AfterProperties["selKlient"].ToString()).LookupId : 0;
            int okresId = properties.AfterProperties["selOkres"] != null ? new SPFieldLookupValue(properties.AfterProperties["selOkres"].ToString()).LookupId : 0;

            if (!string.IsNullOrEmpty(ct)
                && klientId > 0
                && okresId > 0)
            {
                string key = BLL.tabZadania.Define_KEY(ct, klientId, okresId);
                using (SPWeb web = properties.Web)
                {
                    properties.Cancel = !BLL.tabZadania.Check_KEY_IsAllowed(key, web, properties.ListItemId);
                    properties.ErrorMessage = "Zdublowany klucz zadania";
                }
            }
        } 
        #endregion

    }
}
