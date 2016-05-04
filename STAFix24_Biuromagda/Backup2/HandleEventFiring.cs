using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace EventReceivers
{
    /// <summary>
    /// użycie tej klasy ma zapobiec wielokrotnemu wywoływaniu obsługa zdarzenia jeżeli aktualizujemy nasz rekord
    /// procedurami Update() SystemUpdate() SystemUpdate(false) UpdateOverwriteVersion()
    /// </summary>
    public class HandleEventFiring: SPItemEventReceiver
    {
        public void DisableHandleEventFiring()
        {
            this.EventFiringEnabled = false;
        }
        public void EnableHandleEventFiring()
        {
            this.EventFiringEnabled = true;
        }

    }

    #region Example

//    private void button1_Click(object sender, EventArgs e)  
//02.{  
//03.    const string siteUrl = "http://sp2010";  
//04.    SPSite site = new SPSite(siteUrl);  
//05.    SPWeb web = site.RootWeb;  
//06.  
//07.    SPList list = web.Lists["Announcements"];  
//08.    SPListItem item = list.Items[3];  
//09.  
//10.    HandleEventFiring handleEventFiring = new HandleEventFiring();  
//11.    handleEventFiring.DisableHandleEventFiring();  
//12.  
//13.    try  
//14.    {  
//15.        item.Update();  
//16.    }  
//17.    finally  
//18.    {  
//19.        handleEventFiring.EnableHandleEventFiring();  
//20.    }  
//21.}  

    #endregion


}