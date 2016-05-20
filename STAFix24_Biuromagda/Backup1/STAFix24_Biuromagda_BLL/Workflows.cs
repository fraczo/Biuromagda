using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Globalization;
using System.Diagnostics;

namespace BLL
{
    public class Workflows
    {
        public const string workflowHistoryListName = "Workflow History";
        public const string workflowTaskListName = "Workflow Tasks";

        public static void StartWorkflow(SPListItem listItem, string workflowName)
        {
            try
            {
                SPWorkflowManager manager = listItem.Web.Site.WorkflowManager;
                SPWorkflowAssociationCollection objWorkflowAssociationCollection = listItem.ParentList.WorkflowAssociations;
                foreach (SPWorkflowAssociation objWorkflowAssociation in objWorkflowAssociationCollection)
                {
                    if (String.Compare(objWorkflowAssociation.Name, workflowName, true) == 0)
                    {
                        try
                        {
                            SPWorkflowCollection wfc = manager.GetItemActiveWorkflows(listItem);
                            bool isActive = false;
                            foreach (SPWorkflow wf in wfc)
                            {
                                // wf.IsCompleted nie używać - blokuje kolejne uruchomienie procesu jeżęli status jest "Ukończono"
                                if (wf.IsLocked && objWorkflowAssociation.Id.Equals(wf.AssociationId))
                                {
                                    isActive = true;
                                    break;
                                }
                            }

                            if (!isActive)
                            {
                                //manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, true);
                                SPWorkflow wf = manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, SPWorkflowRunOptions.SynchronousAllowPostpone);
                                Debug.WriteLine("Workflow InternalState:" + wf.InternalState.ToString());
                                //The above line will start the workflow...
                            }
                            else
                            {
                                Debug.WriteLine("WF aktualnie uruchomiony - kolejna aktywacja procesu przerwana");
                                //ElasticEmail.EmailGenerator.SendMail("wf aktualnie uruchomiony" + listItem.ID.ToString(), string.Empty);
                            }
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

        public static void StartWorkflow(SPListItem listItem, string workflowName, SPWorkflowRunOptions runOption)
        {
            try
            {
                SPWorkflowManager manager = listItem.Web.Site.WorkflowManager;
                SPWorkflowAssociationCollection objWorkflowAssociationCollection = listItem.ParentList.WorkflowAssociations;
                Debug.WriteLine("WF.Count=" + objWorkflowAssociationCollection.Count.ToString());

                foreach (SPWorkflowAssociation objWorkflowAssociation in objWorkflowAssociationCollection)
                {
                    Debug.WriteLine("WF.InternalName=" + objWorkflowAssociation.InternalName);
                    Debug.WriteLine("WF.Id=" + objWorkflowAssociation.Id.ToString());

                    if (String.Compare(objWorkflowAssociation.Name, workflowName, true) == 0)
                    {
                        Debug.WriteLine("WF.Selected");

                        try
                        {
                            SPWorkflowCollection wfc = manager.GetItemActiveWorkflows(listItem);
                            bool isActive = false;
                            foreach (SPWorkflow wf in wfc)
                            {
                                Debug.WriteLine("WF.InternalName=" + wf.ItemName);

                                // wf.IsCompleted nie używać - blokuje kolejne uruchomienie procesu jeżęli status jest "Ukończono"
                                if (wf.IsLocked && objWorkflowAssociation.Id.Equals(wf.AssociationId))
                                {
                                    isActive = true;
                                    Debug.WriteLine("WF.IsLocked=" + wf.IsLocked.ToString());
                                    Debug.WriteLine("WF.AssociationId=" + wf.AssociationId.ToString());
                                    break;
                                }
                            }

                            if (!isActive)
                            {
                                SPWorkflow spw = manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, runOption);
                                Debug.WriteLine("Workflow: " + workflowName + " Internal State: " + spw.InternalState);
                                //The above line will start the workflow...
                            }
                            else
                            {
                                Debug.WriteLine("WF aktualnie uruchomiony - kolejna aktywacja procesu przerwana");
                                //ElasticEmail.EmailGenerator.SendMail("wf aktualnie uruchomiony" + listItem.ID.ToString(), string.Empty);
                            }
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

        public static void StartSiteWorkflow(SPSite site, string workflowName, SPWorkflowRunOptions runOption)
        {
            StartSiteWorkflow(site, workflowName, runOption, string.Empty);
        }

        public static void StartSiteWorkflow(SPSite site, string workflowName, SPWorkflowRunOptions runOption, string initiationData)
        {
            using (SPWeb web = site.OpenWeb()) // get the web
            {
                //find workflow to start
                var assoc = web.WorkflowAssociations.GetAssociationByName(workflowName, CultureInfo.InvariantCulture);

                //this is the call to start the workflow
                var result = site.WorkflowManager.StartWorkflow(null, assoc, initiationData, runOption);

            }
        }

        public static SPList CreateTaskList(SPWeb web, string listName)
        {
            Guid listGuid = web.Lists.Add(listName, string.Empty, SPListTemplateType.Tasks);
            SPList list = web.Lists.GetList(listGuid, false);
            list.Hidden = false;
            list.Update();
            return list;
        }

        public static SPList CreateHistoryListy(SPWeb web, string listName)
        {
            Guid listGuid = web.Lists.Add(listName, string.Empty, SPListTemplateType.WorkflowHistory);
            SPList list = web.Lists.GetList(listGuid, false);
            list.Hidden = false;
            list.Update();
            return list;
        }

        public static void AssociateWorflow(SPWeb web, string workflowTemplateBaseGuid, string workflowAssociationName)
        {
            //string workflowTemplateBaseGuid = "0b5d7c6b-2764-45dc-8fc1-33fa98145d1c";
            //string workflowAssociationName = "Odchudzanie bazy danych";

            SPWorkflowTemplateCollection workflowTemplates = web.WorkflowTemplates;
            SPWorkflowTemplate workflowTemplate = workflowTemplates.GetTemplateByBaseID(new Guid(workflowTemplateBaseGuid));

            if (workflowTemplate != null)
            {
                // Create the workflow association
                SPList taskList = EnsureListExist(web, workflowTaskListName);
                SPList historyList = EnsureListExist(web, workflowHistoryListName);

                SPWorkflowAssociation workflowAssociation = web.WorkflowAssociations.GetAssociationByName(workflowAssociationName, CultureInfo.InvariantCulture);

                if (workflowAssociation == null)
                {
                    workflowAssociation = SPWorkflowAssociation.CreateWebAssociation(workflowTemplate, workflowAssociationName, taskList, historyList);
                    workflowAssociation.AllowManual = true;
                    //workflowAssociation.Enabled = true;  - nie wiem dlaczego ale ta pozycja wywala błąd.
                    web.WorkflowAssociations.Add(workflowAssociation);
                }
            }
        }

        private static SPList EnsureListExist(SPWeb web, string listName)
        {
            SPList list = web.Lists.TryGetList(listName);
            if (list == null)
            {
                list = BLL.Workflows.CreateTaskList(web, listName);
            }
            return list;
        }


        // nie używana procedura

        //public static void AssociateWorkflowWithList(SPWeb web, string listName, string workflowTemplateBaseGuid, string workflowAssociationName)
        //{
        //    SPList list = web.Lists.TryGetList(listName);
        //    if (list != null)
        //    {
        //        var existingAssociation = list.WorkflowAssociations.GetAssociationByName(workflowAssociationName, CultureInfo.CurrentCulture);
        //        if (existingAssociation == null)
        //        {
        //            // Create the workflow association
        //            SPList taskList = EnsureListExist(web, workflowTaskListName);
        //            SPList historyList = EnsureListExist(web, workflowHistoryListName);

        //            //Create a worklow manager and associate the Workflow template to the list

        //            SPWorkflowManager workflowManager = web.Site.WorkflowManager;
        //            SPWorkflowTemplateCollection templates = workflowManager.GetWorkflowTemplatesByCategory(web, null);
        //            SPWorkflowTemplate template = templates.GetTemplateByBaseID(new Guid(workflowTemplateBaseGuid));
        //            SPWorkflowAssociation association = SPWorkflowAssociation.CreateListAssociation(template, template.Name, taskList, historyList);
        //            association.AllowManual = true;
        //            association.AutoStartCreate = true;
        //            list.WorkflowAssociations.Add(association);
        //            list.Update();
        //            association.Enabled = true;

        //            Debug.WriteLine("List.Workflow: " + workflowAssociationName + " associated");
                    
        //        }
        //    }
        //}


        public static void EnsureWorkflowAssociation(SPList list, string workflowTemplateName, string associationName, bool allowManual, bool startCreate, bool startUpdate)
        {
            var web = list.ParentWeb;
            var lcid = (int)web.Language;
            var defaultCulture = new CultureInfo(lcid);

            // Create the workflow association
            SPList taskList = EnsureListExist(web, workflowTaskListName);
            SPList historyList = EnsureListExist(web, workflowHistoryListName);

            var workflowAssociation =
                list.WorkflowAssociations.Cast<SPWorkflowAssociation>().FirstOrDefault(i => i.Name == associationName);
            if (workflowAssociation != null)
            {
                list.WorkflowAssociations.Remove(workflowAssociation);
                list.Update();
            }

            var template = web.WorkflowTemplates.GetTemplateByName(workflowTemplateName, defaultCulture);
            var association = SPWorkflowAssociation.CreateListAssociation(template, associationName, taskList, historyList);

            association.AllowManual = true;
            association.AutoStartChange = true;
            association.AutoStartCreate = true;

            list.WorkflowAssociations.Add(association);
            list.Update();

            association = list.WorkflowAssociations[association.Id];
            association.AllowManual = allowManual;
            association.AutoStartChange = startUpdate;
            association.AutoStartCreate = startCreate;
            association.AssociationData = "<Dummy></Dummy>";
            association.Enabled = true;
            list.WorkflowAssociations.Update(association);
            list.Update();

            Debug.WriteLine("Ensure.List.Workflow: " + associationName + " associated");

        }


    }
}
