using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Tools
    {
        internal static string Get_ItemInfo(Microsoft.SharePoint.SPListItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CT=" + item.ContentType.Name);
            sb.AppendLine("ID=" + item.ID.ToString());

            return sb.ToString();
        }
    }
}
