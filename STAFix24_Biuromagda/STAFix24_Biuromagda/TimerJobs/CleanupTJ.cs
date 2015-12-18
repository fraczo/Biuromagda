using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace Biuromagda.TimerJobs
{
    class CleanupTJ : Microsoft.SharePoint.Administration.SPJobDefinition
    {
        public static void CreateTimerJob(SPSite site)
        {
            var timerJob = new CleanupTJ(site);

            timerJob.Schedule = new SPWeeklySchedule
            {
                BeginDayOfWeek = System.DayOfWeek.Saturday,
                EndDayOfWeek = System.DayOfWeek.Saturday,
                BeginHour = 0,
                EndHour = 1
            };


            timerJob.Update();
        }

        public static void DelteTimerJob(SPSite site)
        {
            site.WebApplication.JobDefinitions
                .OfType<CleanupTJ>()
                .Where(i => string.Equals(i.SiteUrl, site.Url, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .ForEach(i => i.Delete());
        }

        public CleanupTJ()
            : base()
        { }

        public CleanupTJ(SPSite site)
            : base(string.Format("Biuromagda_Cleanup Timer Job ({0})", site.Url), site.WebApplication, null, SPJobLockType.Job)
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
                BLL.Workflows.StartSiteWorkflow(site, "Odchudzanie bazy danych");
            }
        }
    }
}
