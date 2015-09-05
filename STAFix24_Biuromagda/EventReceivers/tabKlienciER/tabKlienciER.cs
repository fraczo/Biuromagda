using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers.tabKlienciER
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class tabKlienciER : SPItemEventReceiver
    {
       /// <summary>
       /// An item was updated.
       /// </summary>
       public override void ItemUpdated(SPItemEventProperties properties)
       {
           try
           {
               SPListItem item = properties.ListItem;
               SPWeb web = properties.Web;

               string typKlienta = item["ContentType"].ToString();
               switch (typKlienta)
               {
                   case "KPiR":
                   case "KSH":
                       string folderName = item["colNazwaSkrocona"] != null ? item["colNazwaSkrocona"].ToString() : string.Empty;
                       string status = item["enumStatus"] != null ? item["enumStatus"].ToString() : string.Empty;

                       if (status == "Aktywny" && !String.IsNullOrEmpty(folderName))
                       {
                           int docId = BLL.libDokumenty.Ensure_FolderExist(web, folderName);
                           int currDocId = item["_DocumentId"]!=null?int.Parse(item["_DocumentId"].ToString()):0;

                           if (docId>0 && currDocId!=docId)
                           {
                               item["_DocumentId"] = docId;
                               item.Update();
                           }
                       }
                       break;

                   default:
                       break;
               }
           }
           catch (Exception ex)
           {
               var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
           }
       }


    }
}
