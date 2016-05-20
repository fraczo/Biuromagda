using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers.tabDochodyWspolnikow
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class tabDochodyWspolnikow : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            this.Execute(properties);
        }

        public override void ItemUpdated(SPItemEventProperties properties)
        {
            this.Execute(properties);
        }

        private static void Update_KEY(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;

            string key = BLL.tabDochodyWspolnikow.Define_KEY(
                BLL.Tools.Get_LookupId(item, "selKlient_NazwaSkrocona"),
                BLL.Tools.Get_LookupId(item, "selKlient"),
                BLL.Tools.Get_LookupId(item, "selOkres"));

            if (!BLL.Tools.Get_Text(item, "KEY").Equals(key))
            {
                BLL.Tools.Set_Text(item, "KEY", key);
                try
                {
                    item.SystemUpdate();
                }
                catch (Exception)
                {}
                
            }
        }

        private void Execute(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;
            try
            {
                Update_KEY(properties);

                SPListItem item = properties.ListItem;
                BLL.Tools.Ensure_LinkColumn(item, "selKlient");
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
               BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
               var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
#endif

            }
            finally
            {
                this.EventFiringEnabled = true;
            }

        }


    }
}
