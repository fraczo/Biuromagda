using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;

namespace TimerJobs
{

    public class ObslugaWiadomosciTJ : Microsoft.SharePoint.Administration.SPJobDefinition
    {

        public static void CreateTimerJob(SPSite site)
        {
            var timerJob = new ObslugaWiadomosciTJ(site);
            timerJob.Schedule = new SPMinuteSchedule
            {
                BeginSecond = 0,
                EndSecond = 0
            };
            timerJob.Update();
        }

        public static void DelteTimerJob(SPSite site)
        {
            site.WebApplication.JobDefinitions
                .OfType<ObslugaWiadomosciTJ>()
                .Where(i => string.Equals(i.SiteUrl, site.Url, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .ForEach(i => i.Delete());
        }

        public ObslugaWiadomosciTJ()
            : base()
        {

        }

        public ObslugaWiadomosciTJ(SPSite site)
            : base(string.Format("Biuromagda_Obsługa wiadomości Timer Job ({0})", site.Url), site.WebApplication, null, SPJobLockType.Job)
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
                var targetList = site.RootWeb.Lists.TryGetList("Wiadomości");

                if (targetList != null)
                {
                    targetList.Items.Cast<SPListItem>()
                        .Where(i => (bool)i["colCzyWyslana"] != true)
                        .Where(i => i["colPlanowanaDataNadania"] == null || (i["colPlanowanaDataNadania"] != null && (DateTime)i["colPlanowanaDataNadania"] <= DateTime.Now))
                        .ToList()
                        .ForEach(item =>
                        {
                            try
                            {
                                StartWorkflow(item, "Obsługa wiadomości");

                                item["_Output"] = "TimerJob: " + DateTime.Now.ToString();
                                item.Update();

                            }
                            catch (Exception ex)
                            {
                                ElasticEmail.EmailGenerator.ReportError(ex, "BRMagda TimerJob");
                            }
                        });
                }

            }
        }


        #region Helpers

        private static void StartWorkflow(SPListItem listItem, string workflowName)
        {
            try
            {
                SPWorkflowManager manager = listItem.Web.Site.WorkflowManager;
                SPWorkflowAssociationCollection objWorkflowAssociationCollection = listItem.ParentList.WorkflowAssociations;
                foreach (SPWorkflowAssociation objWorkflowAssociation in objWorkflowAssociationCollection)
                {
                    if (String.Compare(objWorkflowAssociation.Name, workflowName, true) == 0)
                    {

                        //We found our workflow association that we want to trigger.

                        //Replace the workflow_GUID with the GUID of the workflow feature that you
                        //have deployed.

                        try
                        {
                            manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, true);
                            //The above line will start the workflow...
                        }
                        catch (Exception)
                        { }


                        break;
                    }
                }
            }
            catch (Exception)
            { }
        }

        #endregion
    }
}


