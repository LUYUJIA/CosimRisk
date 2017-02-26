using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace BLL
{
    public class ProjectManager
    {
        #region 读取项目
        public static List<MODEL.risk_project> getProjects(int projectId)
        {
            List<MODEL.risk_project> projectList = CommonArugment.getProject.getProjectList();
            if (projectId != -1)
            {
                for (int i = 0; i < projectList.Count; ++i)
                {
                    if (projectList[i].Prj_id == projectId)
                    {
                        projectList.RemoveAt(i);
                        --i;
                    }
                }
            }
            return projectList;
        }
        #endregion

        #region 增添项目
        public static bool addProject(XmlDocument xmlDoc, string prjDesc)
        {
            List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
            List<MODEL.risk_link> linkList = new List<MODEL.risk_link>();
            XmlReader xmlContent = new XmlTextReader(new StringReader(xmlDoc.OuterXml));

            string projectName = xmlDoc.DocumentElement.GetAttribute("name");
            int projectId;
            MODEL.risk_project project = new MODEL.risk_project();

            if (!CommonUtils.checkProjectName(projectName))
            {
                CommonMsg.errMsg = "已经存在相同的项目名称";
                return false;
            }

            project.Prj_name = projectName;
            project.Prj_date = DateTime.Now;
            project.Prj_xml = xmlDoc.OuterXml;
            project.Prj_describe = prjDesc;
            try
            {
                projectId = CommonArugment.getProject.saveProject(project);
                CommonUtils.parseXML(xmlContent, taskList, linkList, projectId);
                CommonArugment.getTask.insertTask(taskList);
                CommonArugment.getTask.insertLink(linkList);
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }



        }
        #endregion

        #region 删除项目
        public static bool deleteProject(List<MODEL.risk_project> prjList)
        {
            try
            {
                if (CommonArugment.getProject.deleteProject(prjList))
                    return true;
                return false;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 更新项目
        public static bool updateProject(string name, string description)
        {
            MODEL.risk_project project = new MODEL.risk_project();
            project.Prj_name = name;
            project.Prj_describe = description;
            try
            {
                if (CommonArugment.getProject.updateProject(project))
                    return true;
                return false;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  给项目添加资源
        public static bool addResources(int projectId, MODEL.risk_resource res)
        {
                MODEL.risk_resource originalRes = CommonArugment.getResources.findResByName(res.Resource_name);
                if (originalRes.Resource_remains < res.Resource_amount)
                {
                    CommonMsg.errMsg = originalRes.Resource_name+"不足分配";
                    return false;
                }

                originalRes.Resource_remains -= res.Resource_amount;
                CommonArugment.getResources.updateByName(originalRes);
                CommonArugment.getPrjResAssign.insertRes(projectId, res);
            
            return true;
        }
        #endregion

        #region 显示资源
        public static List<MODEL.risk_resource> showResources(int projectId)
        {
            DAL.DALriskProjectResAssignment res = new DAL.DALriskProjectResAssignment();
            return res.showResources(projectId);
        }
        #endregion

        #region  给项目修改资源  两边
        public static bool modifyResources(string resource_name, MODEL.risk_project_res_assignment res,bool Isright)
        {
            MODEL.risk_resource originalRes = CommonArugment.getResources.findResByName(resource_name);
            if (Isright)
            {
                originalRes.Resource_remains -= res.Assignment_amount;
                res.Assignment_remains += res.Assignment_amount;
                CommonArugment.getResources.updateByName(originalRes);
                CommonArugment.getPrjResAssign.updateByPrjIdResId(res);
            }
            else
            {
                originalRes.Resource_remains += res.Assignment_amount;
                res.Assignment_remains -= res.Assignment_amount;
                CommonArugment.getResources.updateByName(originalRes);
                CommonArugment.getPrjResAssign.updateByPrjIdResId(res);
            }
            return true;
        }
        #endregion

        #region 删除资源 左加右删
        public static void deleteResources(string resource_name, MODEL.risk_project_res_assignment res_assignment)
        {
            MODEL.risk_resource originalRes = CommonArugment.getResources.findResByName(resource_name);

            originalRes.Resource_remains += res_assignment.Assignment_amount;
            CommonArugment.getResources.updateByName(originalRes);
            CommonArugment.getPrjResAssign.deleteByPrjIdResId(res_assignment);

        }
        #endregion
    }
}
