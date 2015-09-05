using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace BLL
{
    public class libDokumenty
    {
        const string targetList = "Dokumenty";

        public static int Ensure_FolderExist(SPWeb web, string folderName)
        {
            folderName = CleanupFileName(folderName);

            SPList list = web.Lists.TryGetList(targetList);

            SPListItem item = list.Items.Cast<SPListItem>()
                .Where(i => i["LinkFilename"].ToString() == folderName)
                .FirstOrDefault();

            if (item != null)
            {
                return item.ID;
            }

            web.AllowUnsafeUpdates = true;
            SPListItem newItem = list.Items.Add("", SPFileSystemObjectType.Folder, folderName);
            newItem.Update();
            web.AllowUnsafeUpdates = false;

            return newItem.ID;

        }

        private static string CleanupFileName(string nazwaPliku)
        {
            //string illegal = "\"M\"\\a/ry/ h**ad:>> a\\/:*?\"| li*tt|le|| la\"mb.?";
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));

            nazwaPliku = r.Replace(nazwaPliku, "");
            
            Regex illegalPathChars = new Regex(@"^\.|[\x00-\x1F,\x7B-\x9F,"",#,%,&,*,/,:,<,>,?,\\]+|(\.\.)+|\.$", RegexOptions.Compiled);
            
            nazwaPliku =illegalPathChars.Replace(nazwaPliku,"");

            return nazwaPliku;

        }

        //static Regex illegalPathChars = new Regex(@"^\.|[\x00-\x1F,\x7B-\x9F,"",#,%,&,*,/,:,<,>,?,\\]+|(\.\.)+|\.$", RegexOptions.Compiled);
        
    }
}
