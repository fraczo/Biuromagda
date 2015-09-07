using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Biuromagda.Features.ObslugaWiadomosciTJ
{
    [Guid("37b1cfd9-f8f0-4a21-8374-c0c5e3ccfc1d")]
    public class ObslugaWiadomosciTJEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            TimerJobs.ObslugaWiadomosciTJ.CreateTimerJob(site);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            TimerJobs.ObslugaWiadomosciTJ.DelteTimerJob(site);
        }
    }
}
