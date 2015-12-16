using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Biuromagda.TimerJobs
{
    class WysylkaWiadomosciTJ : Microsoft.SharePoint.Administration.SPJobDefinition
    {
        public static void CreateTimerJob(SPSite site)
        {
            var timerJob = new WysylkaWiadomosciTJ(site);
            timerJob.Schedule = new SPMinuteSchedule
            {
                BeginSecond = 0,
                EndSecond = 0,
                Interval = 60
            };

            timerJob.Update();
        }

        public static void DelteTimerJob(SPSite site)
        {
            site.WebApplication.JobDefinitions
                .OfType<WysylkaWiadomosciTJ>()
                .Where(i => string.Equals(i.SiteUrl, site.Url, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .ForEach(i => i.Delete());
        }

        public WysylkaWiadomosciTJ()
            : base()
        {

        }

        public WysylkaWiadomosciTJ(SPSite site)
            : base(string.Format("Biuromagda_Wysyłka wiadomości Timer Job ({0})", site.Url), site.WebApplication, null, SPJobLockType.Job)
        {
            Title = Name;
            SiteUrl = site.Url;
        }

        public string SiteUrl
        {
            get { return (string)this.Properties["SiteUrl"]; }
            set { this.Properties["SiteUrl"] = value; }
        }

        public override void Execute(Guid targetInstanceId)
        {
            using (var site = new SPSite(SiteUrl))
            {
                //uruchom siteWF

            }
        }
    }
}
