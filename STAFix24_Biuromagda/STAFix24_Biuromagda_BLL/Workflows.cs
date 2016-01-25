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
                                manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, true);
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
                                Debug.WriteLine("Workflow: "+workflowName + " Internal State: " + spw.InternalState);
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

        public static void StartSiteWorkflow(SPSite site, string workflowName)
        {
            using (SPWeb web = site.OpenWeb()) // get the web
            {
                //find workflow to start
                var assoc = web.WorkflowAssociations.GetAssociationByName(workflowName, CultureInfo.InvariantCulture);

                //this is the call to start the workflow
                var result = site.WorkflowManager.StartWorkflow(null, assoc, string.Empty, SPWorkflowRunOptions.Synchronous);

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
            string workFlowHistoryListName = "Workflow History";
            string workFlowTaskListName = "Workflow Tasks";

            SPWorkflowTemplateCollection workflowTemplates = web.WorkflowTemplates;
            SPWorkflowTemplate workflowTemplate = workflowTemplates.GetTemplateByBaseID(new Guid(workflowTemplateBaseGuid));

            if (workflowTemplate != null)
            {
                // Create the workflow association
                SPList taskList = web.Lists.TryGetList(workFlowTaskListName);
                if (taskList == null)
                {
                    taskList = BLL.Workflows.CreateTaskList(web, workFlowTaskListName);
                }
                SPList historyList = web.Lists.TryGetList(workFlowHistoryListName);
                if (historyList == null)
                {
                    historyList = BLL.Workflows.CreateHistoryListy(web, workFlowHistoryListName);
                }
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
    }
}
