using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
namespace BLL
{
        public class HandleXML
        {         
                
               
                #region 将xml解析读取到数据库

                private static void readXML(XmlReader xml)
                {
                        int parentId = 0;
                        int taskLevel = 0;
                        int curParentTId = -1;
                        int possibleParentTId = -1;
                        Stack<int> parentTaskId = new Stack<int>(); ;
                        parentTaskId.Push(-1);
                        while (xml.Read())
                        {
                                if (xml.NodeType == XmlNodeType.Element)
                                {
                                        if (xml.Name == "sub-tasks")
                                                ++taskLevel;
                                        if (xml.Name == "link")
                                        {
                                                MODEL.risk_link link = new MODEL.risk_link();
                                                link.Delay_days = Int32.Parse(xml.GetAttribute("delay"));
                                                switch (xml.GetAttribute("type"))
                                                {
                                                        case "FS":
                                                                link.Link_type = 0;
                                                                break;
                                                        default:
                                                                link.Link_type = 0;
                                                                break;
                                                }
                                                link.Task_suc_id = Int32.Parse(xml.GetAttribute("taskId"));
                                                link.Task_pre_id = parentId;
                                                link.Prj_id = projectId;
                                                linkList.Add(link);
                                        }
                                        if (xml.Name == "task" || xml.Name == "start-task" || xml.Name == "end-task")
                                        {
                                                if (xml.Name == "start-task")
                                                {
                                                        if (possibleParentTId != -1)
                                                                parentTaskId.Push(possibleParentTId);
                                                        possibleParentTId = Int32.Parse(xml.GetAttribute("id"));
                                                }
                                                if (xml.Name == "task")
                                                {
                                                        possibleParentTId = Int32.Parse(xml.GetAttribute("id"));
                                                }
                                                curParentTId = parentTaskId.Peek();
                                                MODEL.risk_task task = new MODEL.risk_task();
                                                if (xml.GetAttribute("isSummary") == null)
                                                {
                                                        task.Task_is_summary = false;
                                                }
                                                else task.Task_is_summary = true;
                                                task.Task_name = xml.GetAttribute("name");
                                                task.Task_wbs = xml.GetAttribute("wbs");
                                                task.Task_id = Int32.Parse(xml.GetAttribute("id"));
                                                task.Task_level = taskLevel;
                                                task.Task_project_id = projectId;
                                                task.Task_nested_parent_id = curParentTId;
                                                taskList.Add(task);
                                                parentId = task.Task_id;
                                        }
                                }
                                else if (xml.NodeType == XmlNodeType.EndElement)
                                {
                                        if (xml.Name == "sub-tasks")
                                                --taskLevel;
                                        if (xml.Name == "end-task")
                                        {
                                                possibleParentTId = parentTaskId.Pop();
                                        }
                                }
                        }
                }

                private static void save()
                {
                        CommonArugment.getTask.insertTask(taskList);
                        CommonArugment.getTask.insertLink(linkList);
                }

                private static bool checkProject(XmlReader xml, string prjDesc)
                {
                        while (xml.Read())
                        {
                                if (xml.NodeType == XmlNodeType.Element)
                                {
                                        if (xml.Name == "project")
                                        {
                                                try
                                                {
                                                        string projectName = xml.GetAttribute("name");
                                                        if (!CommonArugment.getProject.checkProjectName(projectName))
                                                        {
                                                                CommonMsg.errMsg = "该项目名称已经存在";
                                                                return false;
                                                        }
                                                        MODEL.risk_project project = new MODEL.risk_project();
                                                        project.Prj_name = projectName;
                                                        project.Prj_date = DateTime.Now;
                                                        project.Prj_xml = xml.ReadOuterXml();
                                                        project.Prj_describe = prjDesc;
                                                        projectId = CommonArugment.getProject.saveProject(project);                                                      
                                                        return true;
                                                } catch (System.Exception ex)
                                                {
                                                        throw ex;
                                                }
                                        }
                                }
                        }
                        return false;
                }

                public static string SaveToDB(XmlDocument xmlDoc,string prjDesc)
                {
                        taskList.Clear();
                        linkList.Clear();
                        XmlReader xmlProject = new XmlTextReader(new StringReader(xmlDoc.OuterXml));
                        XmlReader xmlContent = new XmlTextReader(new StringReader(xmlDoc.OuterXml));
                        try
                        {
                                if (checkProject(xmlProject,prjDesc))
                                {
                                        readXML(xmlContent);
                                        save();
                                }
                                else return CommonMsg.errMsg;
                        } catch (System.Exception ex)
                        {
                                return ex.Message;
                        }
                        return "";
                }

                #endregion

                #region 读取数据库中的xml信息

                public static List<MODEL.risk_task> readTasks(int projectId)
                {
                        return CommonMsg.getTask.getTaskList(projectId);
                }

                public static List<MODEL.risk_link> readLinks(int projectId)
                {
                        return CommonMsg.getTask.getLinkList(projectId);
                }

                public static List<MODEL.risk_project>readProjects(){
                        return CommonMsg.getProject.getProjectList();
                }
                #endregion

        }
}


//private static void analyzeXML(XmlDocument xmlDoc)
//                {
//                        XmlNode xn = xmlDoc.SelectSingleNode("project");
//                        XmlElement pro = (XmlElement)xn;
//                        string projectName = pro.GetAttribute("name");

//                        XmlNodeList nodelist = xn.SelectNodes("child::*");
//                        foreach (XmlNode xnf in nodelist)
//                        {
//                                XmlElement xe = (XmlElement)xnf;
//                                if (xe.Name == "link")
//                                {
//                                        MODEL.risk_link link = new MODEL.risk_link();

//                                }
//                                if (xe.Name == "task" || xe.Name == "start-task" || xe.Name == "end-task")
//                                {
//                                        MODEL.risk_task task = new MODEL.risk_task();
//                                        if (xe.GetAttribute("isSummary") == null)
//                                        {
//                                                task.Task_is_summary = false;
//                                        }
//                                        else task.Task_is_summary = true;
//                                        task.Task_name = xe.GetAttribute("name");
//                                        task.Task_wbs = xe.GetAttribute("wbs");
//                                        task.Task_id = Int32.Parse(xe.GetAttribute("id"));
//                                        taskList.Add(task);
//                                        XmlNodeList linkNodeList = xnf.SelectNodes("child::link");
//                                        foreach (XmlNode linkNode in linkNodeList)
//                                        {
//                                                XmlElement lNode = (XmlElement)linkNode;
//                                                MODEL.risk_link link = new MODEL.risk_link();
//                                                link.Delay_days = Int32.Parse(lNode.GetAttribute("delay"));
//                                                switch (lNode.GetAttribute("type"))
//                                                {
//                                                        case "FS":
//                                                                link.Link_type = 0;
//                                                                break;
//                                                        default:
//                                                                link.Link_type = 0;
//                                                                break;
//                                                }
//                                                link.Task_suc_id = Int32.Parse(lNode.GetAttribute("taskId"));
//                                                link.Task_pre_id = task.Task_id;
//                                                linkList.Add(link);
//                                        }
//                                }
//                        }
//                }