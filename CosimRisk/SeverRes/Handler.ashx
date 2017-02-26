<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System.Globalization;
public class Handler : IHttpHandler
{
    protected XmlDocument xmlDoc = null;
    protected XmlElement root = null;
    protected XmlNamespaceManager nsmgr = null;
    protected XmlReader xmlContent = null;
    public void ProcessRequest(HttpContext context)
    {
        int method = Int32.Parse(context.Request.QueryString["method"]);
        switch (method)
        {
            case 0:
                try
                {
                    JObject sucMsg = new JObject();
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 1: //新建项目，读取XML
                try
                {
                    if (context.Request.Files.Count == 0)
                    {
                        context.Response.Write("{success:false}");
                        return;
                    }
                    HttpPostedFile file = context.Request.Files[0];
                    string projectDescription = context.Request.Form["PRJ_DESCRIBE"];
                    if (projectDescription == null)
                        projectDescription = "";
                    if (file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        int FileLen = file.ContentLength;
                        byte[] input = new byte[FileLen];
                        System.IO.Stream UpLoadStream = file.InputStream;
                        UpLoadStream.Read(input, 0, FileLen);
                        UpLoadStream.Position = 0;
                        StreamReader sr = new System.IO.StreamReader(UpLoadStream, System.Text.Encoding.Default);
                        string ans = sr.ReadToEnd();
                        sr.Close();
                        xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(ans);
                    }

                    if (BLL.ProjectManager.addProject(xmlDoc, projectDescription))
                    {
                        JObject sucMsg = new JObject();
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        JObject errMsg = new JObject();
                        errMsg.Add("success", false);
                        errMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(errMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 2:  //获得所有的项目信息
                try
                {
                    JArray prjLists = new JArray();
                    List<MODEL.risk_project> PrjList = BLL.ProjectManager.getProjects(-1);
                    foreach (MODEL.risk_project project in PrjList)
                    {
                        JObject prj = new JObject();
                        prj.Add("PRJ_ID", project.Prj_id);
                        prj.Add("PRJ_NAME", project.Prj_name);
                        prj.Add("PRJ_DESCRIBE", project.Prj_describe);
                        prj.Add("PRJ_DATE", project.Prj_date.ToLocalTime().ToString());
                        prj.Add("PRJ_XML", project.Prj_xml);
                        prjLists.Add(prj);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("data", prjLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 3: //删除所选的项目
                try
                {
                    string dataString = context.Request.Params["data"];
                    string[] datas = dataString.Split(',');
                    List<MODEL.risk_project> prjList = new List<MODEL.risk_project>();
                    JObject sucMsg = new JObject();
                    for (int i = 0; i < datas.Length; ++i)
                    {
                        MODEL.risk_project project = new MODEL.risk_project();
                        project.Prj_name = datas[i];
                        prjList.Add(project);
                    }
                    if (BLL.ProjectManager.deleteProject(prjList))
                    {
                        sucMsg.Add("success", true);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 4: //修改项目描述
                try
                {
                    string name = context.Request.Form["PRJ_NAME"];
                    string description = context.Request.Form["PRJ_DESCRIBE"];
                    JObject sucMsg = new JObject();
                    if (BLL.ProjectManager.updateProject(name, description))
                    {
                        sucMsg.Add("success", true);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 5: //给项目添加资源
                try
                {
                    MODEL.risk_resource res = new MODEL.risk_resource();
                    Stream stream = context.Request.InputStream;
                    int projectId;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Resource_amount = Int32.Parse(obj["assign_amount"].ToString());
                        projectId = Int32.Parse(obj["project_id"].ToString());
                        res.Auto_id = Int32.Parse(obj["resrouce_id"].ToString());
                        res.Resource_name = obj["name"].ToString();
                        JObject sucMsg = new JObject();
                        if (BLL.ProjectManager.addResources(projectId, res))
                        {
                            sucMsg.Add("success", true);
                            context.Response.Write(sucMsg);
                        }
                        else
                        {
                            sucMsg.Add("success", false);
                            string a = BLL.CommonMsg.errMsg;
                            sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                            context.Response.Write(sucMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 6://显示项目资源
                try
                {
                    int projectId;

                    projectId = Int32.Parse(context.Request.QueryString["projectId"]);
                    JArray resLists = new JArray();
                    List<MODEL.risk_resource> ResList = BLL.ProjectManager.showResources(projectId);
                    foreach (MODEL.risk_resource Res in ResList)
                    {
                        JObject res = new JObject();
                        res.Add("Resource_Name", Res.Resource_name);
                        res.Add("Resource_Type", Res.Resource_type);
                        res.Add("Resource_Mount", Res.Resource_amount);
                        res.Add("Resource_Price", Res.Resource_unit_price);
                        res.Add("Auto_id", Res.Auto_id);
                        resLists.Add(res);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("data", resLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 7://修改项目已有资源
                try
                {
                    MODEL.risk_project_res_assignment res_assignment = new MODEL.risk_project_res_assignment();
                    Stream stream = context.Request.InputStream;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res_assignment.Assignment_amount = Int32.Parse(obj["assign_amount"].ToString());
                        res_assignment.Assignment_remains = Int32.Parse(obj["assign_original"].ToString());
                        res_assignment.Pri_id = Int32.Parse(obj["project_id"].ToString());
                        res_assignment.Resource_id = Int32.Parse(obj["resrouce_id"].ToString());
                        bool Isright = bool.Parse(obj["Isright"].ToString());
                        string resource_name = obj["name"].ToString();
                        BLL.ProjectManager.modifyResources(resource_name, res_assignment, Isright);
                        JObject sucMsg = new JObject();
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 8://删除项目资源
                try
                {
                    MODEL.risk_project_res_assignment res_assignment = new MODEL.risk_project_res_assignment();
                    Stream stream = context.Request.InputStream;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res_assignment.Assignment_amount = Int32.Parse(obj["assign_amount"].ToString());
                        res_assignment.Assignment_remains = Int32.Parse(obj["assign_original"].ToString());
                        res_assignment.Pri_id = Int32.Parse(obj["project_id"].ToString());
                        res_assignment.Resource_id = Int32.Parse(obj["resrouce_id"].ToString());
                        bool Isright = bool.Parse(obj["Isright"].ToString());
                        string resource_name = obj["name"].ToString();
                        BLL.ProjectManager.deleteResources(resource_name, res_assignment);
                        JObject sucMsg = new JObject();
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 21://得到某一层任务的图（需要参数projectId，taskId, taskLevel。其中taskLevel从第0层开始计数）
                try
                {
                    int projectId = -1;
                    if (context.Request.QueryString["projectId"] != null)
                        projectId = Int32.Parse(context.Request.QueryString["projectId"]);
                    int taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    //         int taskLevel = Int32.Parse(context.Request.QueryString["taskLevel"]); //没用到！！！
                    JObject sucMsg = new JObject();
                    if (BLL.DrawImages.calTaskAndLink(projectId, taskId))
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        sucMsg.Add("link", ser.Serialize(BLL.CommonMsg.linkList).ToString());
                        sucMsg.Add("task", ser.Serialize(BLL.CommonMsg.taskList).ToString());
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }

                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 22: //设置某一个任务节点的属性值（需要参数task的Auto_id，优先级taskPriority,分布名称expressionName,分布参数A、B、C）
                try
                {
                    JObject sucMsg = new JObject();
                    int auto_id = Int32.Parse(context.Request.Form["Auto_id"]);
                    int taskPriority = Int32.Parse(context.Request.Form["taskPriority"]);
                    double actualValue = Convert.ToDouble(context.Request.Form["actualValue"]);
                    string expressionName;
                    if (context.Request.Form["expressionName"] != null && context.Request.Form["expressionName"] != "")
                    {
                        expressionName = context.Request.Form["expressionName"];
                    }
                    else
                        expressionName = "正态分布";
                    double[] value = null;  //仿真得到工期
                    switch (expressionName)
                    {
                        case "三角分布":
                            double a = Double.Parse(context.Request.Form["A"]);
                            double b = Double.Parse(context.Request.Form["B"]);
                            double m = Double.Parse(context.Request.Form["C"]);
                            value = new double[3];
                            value[0] = a;
                            value[1] = b;
                            value[2] = m;
                            break;
                        case "Beta分布":
                            double alpha = Double.Parse(context.Request.Form["A"]);
                            double beta = Double.Parse(context.Request.Form["B"]);
                            value = new double[2];
                            value[0] = alpha;
                            value[1] = beta;
                            break;
                        case "正态分布":
                            double e = 0, d = 0;
                            if (context.Request.Form["A"] != null && context.Request.Form["A"] != "")
                                e = Double.Parse(context.Request.Form["A"]);
                            if (context.Request.Form["B"] != null && context.Request.Form["A"] != "")
                                d = Double.Parse(context.Request.Form["B"]);
                            value = new double[2];
                            value[0] = e;
                            value[1] = d;
                            break;
                        case "固定":
                            value = new double[1];
                            value[0] = Double.Parse(context.Request.Form["A"]);
                            break;
                        default:
                            sucMsg.Add("success", false);
                            sucMsg.Add("errMsg", "该分布不存在");
                            context.Response.Write(sucMsg);
                            return;
                    }

                    if (BLL.DrawImages.setTaskAttribute(auto_id, taskPriority, value, actualValue, expressionName))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }

                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 23://得到所有任务的列表（需要参数projectId）
                try
                {
                    int projectId = -1;
                    if (context.Request.QueryString["projectId"] != null)
                        projectId = Int32.Parse(context.Request.QueryString["projectId"]);
                    JObject sucMsg = new JObject();
                    BLL.DrawImages.getAllTaskAndLink(projectId);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    sucMsg.Add("link", ser.Serialize(BLL.CommonMsg.linkList).ToString());
                    sucMsg.Add("task", ser.Serialize(BLL.CommonMsg.taskList).ToString());
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);

                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 24://添加资源到总资源
                try
                {
                    MODEL.risk_resource res = new MODEL.risk_resource();
                    Stream stream = context.Request.InputStream;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Resource_description = obj["Resource_description"].ToString();
                        res.Resource_amount = Int32.Parse(obj["Resource_mount"].ToString());
                        res.Resource_name = obj["Resource_name"].ToString();
                        res.Resource_remains = Int32.Parse(obj["Resource_remains"].ToString());
                        res.Resource_type = obj["Resource_type"].ToString();
                        res.Resource_unit_price = Int32.Parse(obj["Resource_unit_price"].ToString());
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.CommonArugment.getResources.insertRes(res))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 25://修改资源到总资源
                try
                {
                    MODEL.risk_resource res = new MODEL.risk_resource();
                    Stream stream = context.Request.InputStream;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Resource_description = obj["Resource_description"].ToString();
                        res.Resource_amount = Int32.Parse(obj["Resource_mount"].ToString());
                        res.Resource_name = obj["Resource_name"].ToString();
                        res.Resource_remains = Int32.Parse(obj["Resource_remains"].ToString());
                        res.Resource_type = obj["Resource_type"].ToString();
                        res.Resource_unit_price = Int32.Parse(obj["Resource_unit_price"].ToString());
                        res.Auto_id = Int32.Parse(obj["Auto_id"].ToString());
                        BLL.CommonArugment.getResources.updateById(res);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 26://删除资源到总资源
                try
                {
                    Stream stream = context.Request.InputStream;
                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        String name = obj["Resource_name"].ToString();
                        BLL.CommonArugment.getResources.deleteByName(name);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 27://显示总资源
                try
                {
                    JArray resLists = new JArray();
                    List<MODEL.risk_resource> ResList = BLL.CommonArugment.getResources.getAllRes();
                    foreach (MODEL.risk_resource Res in ResList)
                    {
                        JObject res = new JObject();
                        res.Add("Resource_Name", Res.Resource_name);
                        res.Add("Resource_Type", Res.Resource_type);
                        res.Add("Resource_Mount", Res.Resource_amount);
                        res.Add("Resource_Remains", Res.Resource_remains);
                        res.Add("Resource_Price", Res.Resource_unit_price);
                        res.Add("Resource_Description", Res.Resource_description);
                        res.Add("Auto_id", Res.Auto_id);
                        resLists.Add(res);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("data", resLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 28://显示任务的资源
                /*
                 * 返回为JSON格式
                 * 返回的数据在data中，有autoId,name,description...
                 */
                try
                {
                    int taskAutoId = -1;
                    if (context.Request.QueryString["taskAutoId"] != null)
                        taskAutoId = Int32.Parse(context.Request.QueryString["taskAutoId"]);
                    List<MODEL.risk_resource> resourcesList = BLL.DrawImages.getTaskResources(taskAutoId);
                    JArray array = new JArray();
                    foreach (MODEL.risk_resource res in resourcesList)
                    {
                        JObject obj = new JObject();
                        obj.Add("Auto_id", res.Auto_id);
                        obj.Add("Resource_Mount", res.Resource_amount);
                        obj.Add("Resource_Name", res.Resource_name);
                        obj.Add("Resource_Price", res.Resource_unit_price);
                        obj.Add("Resource_Type", res.Resource_type);
                        array.Add(obj);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("data", array);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 29:
                /*
                 * 单个任务的资源插入，接收一个JSON格式
                 */
                try
                {
                    int taskAutoId = -1;
                    MODEL.risk_resource res = new MODEL.risk_resource();

                    Stream stream = context.Request.InputStream;

                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Resource_amount = Int32.Parse(obj["assign_amount"].ToString());
                        res.Resource_name = obj["name"].ToString();
                        taskAutoId = Int32.Parse(obj["Task_autoid"].ToString());
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.DrawImages.setTaskResources(taskAutoId, res))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }


                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 30: //显示项目资源名字给任务
                try
                {
                    int projectId;

                    projectId = Int32.Parse(context.Request.QueryString["projectId"]);
                    JArray resLists = new JArray();
                    List<MODEL.risk_resource> ResList = BLL.ProjectManager.showResources(projectId);
                    foreach (MODEL.risk_resource Res in ResList)
                    {
                        JObject res = new JObject();
                        res.Add("Resource_Name", Res.Resource_name);
                        res.Add("Resource_Id", Res.Auto_id);
                        resLists.Add(res);
                    }
                    JObject sucMsg = new JObject();
                    sucMsg.Add("data", resLists);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 31://修改更新任务的资源
                try
                {
                    MODEL.risk_task_resource_assignment res = new MODEL.risk_task_resource_assignment();

                    Stream stream = context.Request.InputStream;

                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Assignment_amount = Int32.Parse(obj["assign_amount"].ToString());
                        res.Resource_id = Int32.Parse(obj["resource_id"].ToString());
                        res.Task_auto_id = Int32.Parse(obj["Task_autoid"].ToString());
                        res.Assignment_own = 0;
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.DrawImages.updateTaskResources(res))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }


                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 32://删除任务的资源
                try
                {
                    MODEL.risk_task_resource_assignment res = new MODEL.risk_task_resource_assignment();

                    Stream stream = context.Request.InputStream;

                    if (stream.Length != 0)
                    {
                        StreamReader streamReader = new StreamReader(stream);
                        string json = streamReader.ReadToEnd();
                        JObject obj = JObject.Parse(json);
                        res.Resource_id = Int32.Parse(obj["resource_id"].ToString());
                        res.Task_auto_id = Int32.Parse(obj["Task_autoid"].ToString());
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.DrawImages.deleteTaskResources(res))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 41: //开始仿真
                try
                {

                    int prjId = Int32.Parse(context.Request.Form["projectId"]);
                    int times = Int32.Parse(context.Request.Form["times"]);
                    string desciption = context.Request.Form["description"];
                    int have_resource = Int32.Parse(context.Request.Form["resource_radio"]);
                    JObject sucMsg = new JObject();
                    if (have_resource == 0)
                    {
                        if (BLL.Simulation.calCriticalRoute(prjId, times, desciption, 0, have_resource))
                        {
                            sucMsg.Add("success", true);
                            context.Response.Write(sucMsg);
                        }
                        else
                        {
                            sucMsg.Add("success", false);
                            sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                            context.Response.Write(sucMsg);
                        }
                    }
                    else if (have_resource == 1)
                    {
                        BLL.SimulationWithResources Sim = new BLL.SimulationWithResources();
                        if (Sim.SimCriticalResource(prjId, times, desciption, 0, have_resource))
                        {
                            sucMsg.Add("success", true);
                            context.Response.Write(sucMsg);
                        }
                        else
                        {
                            sucMsg.Add("success", false);
                            sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                            context.Response.Write(sucMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 42://显示项目版本
                try
                {
                    JObject sucMsg = new JObject();
                    List<MODEL.risk_prj_version_info> prjVersionInfoList = BLL.Simulation.getSimulationVersion();
                    //JavaScriptSerializer ser = new JavaScriptSerializer();
                    //sucMsg.Add("data", ser.Serialize(prjVersionInfoList).ToString());
                    //sucMsg.Add("success", true);
                    //context.Response.Write(sucMsg);
                    JArray prjLists = new JArray();
                    foreach (MODEL.risk_prj_version_info projectVerInfo in prjVersionInfoList)
                    {
                        DateTime t1 = projectVerInfo.Sim_starttime;
                        string s1 = t1.ToString("G", DateTimeFormatInfo.InvariantInfo);
                        DateTime t2 = projectVerInfo.Sim_endtime;
                        string s2 = t2.ToString("G", DateTimeFormatInfo.InvariantInfo);

                        JObject prj = new JObject();
                        prj.Add("priId", projectVerInfo.Pri_id);
                        prj.Add("simVersionId", projectVerInfo.Sim_version_id);
                        prj.Add("projectName", projectVerInfo.ProjectName);
                        prj.Add("desciption", projectVerInfo.Desciption);
                        prj.Add("count", projectVerInfo.Count);
                        //          prj.Add("simStarttime", projectVerInfo.Sim_starttime.ToString("G", DateTimeFormatInfo.InvariantInfo));
                        //          prj.Add("simEndtime", projectVerInfo.Sim_endtime.ToString("G", DateTimeFormatInfo.InvariantInfo));
                        prj.Add("simStarttime", s1);
                        prj.Add("simEndtime", s2);
                        if (projectVerInfo.Have_resource == 1)
                            prj.Add("have_resource", "√");
                        else
                            prj.Add("have_resource", "×");
                        prjLists.Add(prj);
                    }
                    sucMsg.Add("data", prjLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);

                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 43: //删除版本
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    JObject sucMsg = new JObject();

                    if (BLL.CommonArugment.getProjectVersionInfo.deletePrjVersionInfo(simVersionId))
                    {
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 60://just a test(dot test) 
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    JObject sucMsg = new JObject();
                    if (BLL.GetResults.dotTestFunc(simVersionId))
                    {
                        List<MODEL.bar_data> barDataList = BLL.CommonMsg.barDataList;
                        JArray resLists = new JArray();
                        foreach (MODEL.bar_data barData in barDataList)
                        {
                            JObject barDObject = new JObject();
                            barDObject.Add("Range", barData.ScFrom + "-" + barData.ScTo);
                            barDObject.Add("ScNum", barData.ScNum);
                            resLists.Add(barDObject);
                        }
                        sucMsg.Add("barData", resLists);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 61://显示某一版本某一次的关键路径
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int simSequence = Int32.Parse(context.Request.QueryString["simSequence"]);
                    int taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    int taskLevel = Int32.Parse(context.Request.QueryString["taskLevel"]);
                    JObject sucMsg = new JObject();
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    if (BLL.GetResults.getCriticalRoute(simVersionId, simSequence - 1, taskId, taskLevel))
                    {
                        sucMsg.Add("link", ser.Serialize(BLL.CommonMsg.linkList).ToString());
                        sucMsg.Add("task", ser.Serialize(BLL.CommonMsg.taskList).ToString());
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 62: //显示平均工期
                try
                {
                    JObject sucMsg = new JObject();
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    List<MODEL.risk_task> taskList = BLL.GetResults.getAverageProjectTime(simVersionId);
                    JArray taskLists = new JArray();
                    foreach (MODEL.risk_task task in taskList)
                    {
                        JObject tObj = new JObject();
                        tObj.Add("Task_name", task.Task_name);
                        tObj.Add("AverageProjectTime", task.AverageProjectTime);
                        taskLists.Add(tObj);
                    }
                    sucMsg.Add("task", taskLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 63: //显示关键路径概率
                try
                {
                    JObject sucMsg = new JObject();
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int taskId;
                    if (context.Request.QueryString["simVersionId"] == null)
                    {
                        taskId = -1;
                    }
                    else taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    List<MODEL.risk_task> taskList = BLL.GetResults.getCriticalRatio(simVersionId, taskId);
                    JArray taskLists = new JArray();
                    foreach (MODEL.risk_task task in taskList)
                    {
                        JObject tObj = new JObject();
                        tObj.Add("Task_name", task.Task_name);
                        tObj.Add("CriticalRatio", task.CriticalRatio);
                        taskLists.Add(tObj);
                    }
                    sucMsg.Add("task", taskLists);
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 64: //显示工期分布
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int scale = Int32.Parse(context.Request.QueryString["scale"]);
                    JObject sucMsg = new JObject();

                    if (BLL.GetResults.getBarData(simVersionId, scale))
                    {
                        List<MODEL.bar_data> barDataList = BLL.CommonMsg.barDataList;
                        JArray resLists = new JArray();
                        foreach (MODEL.bar_data barData in barDataList)
                        {
                            JObject barDObject = new JObject();
                            barDObject.Add("Range", barData.ScFrom + "-" + barData.ScTo);
                            barDObject.Add("ScNum", barData.ScNum);
                            resLists.Add(barDObject);
                        }
                        sucMsg.Add("barData", resLists);
                        sucMsg.Add("barSum", barDataList.Count);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 65: //显示任务树
                try
                {
                    int projectId = Int32.Parse(context.Request.QueryString["projectId"]);
                    context.Response.Write(BLL.GetResults.getChildren(projectId, -1));
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 66:
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int scale = 30;
                    if (context.Request.QueryString["scale"] != null && context.Request.QueryString["scale"] != "")
                    {
                        scale = Int32.Parse(context.Request.QueryString["scale"]);
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.GetResults.getOverdueRiskRatio(simVersionId, scale))
                    {
                        List<MODEL.bar_data> barDataList = BLL.CommonMsg.barDataList;
                        JArray resLists = new JArray();
                        foreach (MODEL.bar_data barData in barDataList)
                        {
                            JObject barDObject = new JObject();
                            barDObject.Add("Range", ">" + barData.ScFrom);
                            barDObject.Add("ScNum", barData.ScNum);
                            barDObject.Add("Ratio", barData.Ratio);
                            resLists.Add(barDObject);
                        }
                        sucMsg.Add("barData", resLists);
                        sucMsg.Add("barSum", barDataList.Count);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 67: //显示最大概率关键路径
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    int taskLevel = Int32.Parse(context.Request.QueryString["taskLevel"]);
                    JObject sucMsg = new JObject();
                    JavaScriptSerializer ser = new JavaScriptSerializer();

                    sucMsg.Add("maxProjectTime", BLL.GetResults.getCriticalRouteByMaxRatio(simVersionId, taskId, taskLevel));
                    sucMsg.Add("link", ser.Serialize(BLL.CommonMsg.linkList).ToString());
                    sucMsg.Add("task", ser.Serialize(BLL.CommonMsg.taskList).ToString());
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);

                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 68: //任务资源延阻
                try
                {
                    JObject sucMsg = new JObject();
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int taskId;
                    if (context.Request.QueryString["simVersionId"] == null)
                    {
                        taskId = -1;
                    }
                    else taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    if (taskId == -1)
                    {
                        List<MODEL.risk_task> taskList = BLL.GetResults.getWaitTask(simVersionId, taskId);
                        JArray taskLists = new JArray();
                        foreach (MODEL.risk_task task in taskList)
                        {
                            JObject tObj = new JObject();
                            tObj.Add("Task_name", task.Task_name);
                            tObj.Add("wait_time", task.Value);
                            taskLists.Add(tObj);
                        }
                        sucMsg.Add("task", taskLists);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        List<MODEL.risk_task_instance_res> resList = BLL.GetResults.getWaitResource(simVersionId, taskId);
                        JArray taskLists = new JArray();
                        foreach (MODEL.risk_task_instance_res res in resList)
                        {
                            JObject tObj = new JObject();
                            tObj.Add("Resource_name", BLL.CommonArugment.getResources.findResById(res.Resource_id).Resource_name);
                            tObj.Add("wait_time", res.Wait_time);
                            taskLists.Add(tObj);
                        }
                        sucMsg.Add("task", taskLists);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 69://资源用量分析
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int resourceId = Int32.Parse(context.Request.QueryString["resourceId"]);
                    int scale = 20;
                    if (context.Request.QueryString["scale"] != null && context.Request.QueryString["scale"] != "")
                    {
                        scale = Int32.Parse(context.Request.QueryString["scale"]);
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.GetResults.getResoueceUse(simVersionId, scale, resourceId))
                    {
                        List<MODEL.bar_data> barDataList = BLL.CommonMsg.barDataList;
                        JArray resLists = new JArray();
                        foreach (MODEL.bar_data barData in barDataList)
                        {
                            JObject barDObject = new JObject();
                            barDObject.Add("Range", barData.ScFrom + "-" + barData.ScTo);
                            barDObject.Add("Ratio", barData.Ratio);
                            resLists.Add(barDObject);
                        }
                        sucMsg.Add("barData", resLists);
                        sucMsg.Add("barSum", barDataList.Count);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 70://'成本分析
                try
                {
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    double indirect_cost = Double.Parse(context.Request.QueryString["indirect_cost"]);
                    int scale = 30;
                    if (context.Request.QueryString["scale"] != null && context.Request.QueryString["scale"] != "")
                    {
                        scale = Int32.Parse(context.Request.QueryString["scale"]);
                    }
                    JObject sucMsg = new JObject();
                    if (BLL.GetResults.getOverdueCostRatio(simVersionId, scale))
                    {
                        List<MODEL.bar_data> barDataList = BLL.CommonMsg.barDataList;
                        JArray resLists = new JArray();
                        foreach (MODEL.bar_data barData in barDataList)
                        {
                            JObject barDObject = new JObject();
                            barDObject.Add("Range", ">" + (barData.ScFrom + (int)indirect_cost));
                            barDObject.Add("ScNum", barData.ScNum);
                            barDObject.Add("Ratio", barData.Ratio);
                            resLists.Add(barDObject);
                        }
                        sucMsg.Add("barData", resLists);
                        sucMsg.Add("barSum", barDataList.Count);
                        sucMsg.Add("success", true);
                        context.Response.Write(sucMsg);
                    }
                    else
                    {
                        sucMsg.Add("success", false);
                        sucMsg.Add("errMsg", BLL.CommonMsg.errMsg);
                        context.Response.Write(sucMsg);
                    }
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 71://资源成本联合分险
                break;
            case 72: //显示等待资源的任务树
                try
                {
                    int VersionId = Int32.Parse(context.Request.QueryString["VersionId"]);
                    context.Response.Write(BLL.GetResults.getwaitResTaskTree(VersionId));
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 73: //显示直接成本
                try
                {
                    JObject sucMsg = new JObject();
                    int VersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    sucMsg.Add("Direct_cost", BLL.GetResults.getDirectCost(VersionId));
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 74: //返回平均工期
                try
                {
                    JObject sucMsg = new JObject();
                    int VersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    sucMsg.Add("project_value", BLL.GetResults.getAvgDuration(VersionId));
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            case 75: //进度压缩
                try
                {
                    JObject sucMsg = new JObject();
                    int simVersionId = Int32.Parse(context.Request.QueryString["simVersionId"]);
                    int taskId = Int32.Parse(context.Request.QueryString["taskId"]);
                    int taskLevel = Int32.Parse(context.Request.QueryString["taskLevel"]);
                    double targetDuration = Double.Parse(context.Request.QueryString["targetDuration"]);
                    BLL.ScheduleManagement.ShrinkHours(simVersionId, taskId, taskLevel, targetDuration);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    sucMsg.Add("link", ser.Serialize(BLL.CommonMsg.linkList).ToString());
                    sucMsg.Add("task", ser.Serialize(BLL.CommonMsg.taskList).ToString());
                    sucMsg.Add("success", true);
                    context.Response.Write(sucMsg);
                }
                catch (Exception ex)
                {
                    JObject errMsg = new JObject();
                    errMsg.Add("success", false);
                    errMsg.Add("errMsg", ex.Message);
                    context.Response.Write(errMsg);
                }
                break;
            default:
                JObject errorObject = new JObject();
                errorObject.Add("success", false);
                errorObject.Add("errMsg", "未知的方法错误");
                context.Response.Write(errorObject);
                break;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}