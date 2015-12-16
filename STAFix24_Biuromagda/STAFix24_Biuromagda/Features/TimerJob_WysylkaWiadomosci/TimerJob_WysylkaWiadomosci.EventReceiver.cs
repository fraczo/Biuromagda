using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Biuromagda.Features.TimerJob_WysylkaWiadomosci
{
    [Guid("37a6ae1a-aa0e-4d75-b2b2-8542cb12b532")]
    public class TimerJob_WysylkaWiadomosciEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            TimerJobs.WysylkaWiadomosciTJ.CreateTimerJob(site);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            TimerJobs.WysylkaWiadomosciTJ.DelteTimerJob(site);
        }
    }
}
