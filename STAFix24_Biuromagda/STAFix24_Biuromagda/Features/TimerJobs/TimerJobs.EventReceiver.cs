using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Diagnostics;

namespace Biuromagda.Features.TimerJobs
{

    [Guid("e63b2f20-7d62-4b54-af25-871a15b6a503")]
    public class TimerJobsEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                var site = properties.Feature.Parent as SPSite;
                Biuromagda.TimerJobs.WysylkaWiadomosciTJ.CreateTimerJob(site);
            }
            catch (Exception ex)
            {
                ElasticEmail.EmailGenerator.ReportError(ex, (properties.Feature.Parent as SPSite).Url);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;
            Biuromagda.TimerJobs.WysylkaWiadomosciTJ.DelteTimerJob(site);
        }
    }
}
