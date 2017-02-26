using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CommonMsg
    {
        //private static List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
        //private static List<MODEL.risk_link> linkList = new List<MODEL.risk_link>();
        //private static MODEL.risk_project riskProject = new MODEL.risk_project();
        private static int projectId;
        public static double maxNumber = 99999999999;
        public static string errMsg = "未知错误";
        public static string simulateTime = "";
        public static List<MODEL.risk_task> taskList = null;
        public static List<MODEL.risk_link> linkList = null;
        public static List<MODEL.bar_data> barDataList = null;
    }

    public class CommonArugment
    {
        public static readonly DAL.DALtask getTask = new DAL.DALtask();
        public static readonly DAL.DALproject getProject = new DAL.DALproject();
        public static readonly DAL.DALexpression getExpression = new DAL.DALexpression();
        public static readonly DAL.DALprojectVersionInfo getProjectVersionInfo = new DAL.DALprojectVersionInfo();
        public static readonly DAL.DALprojectInstance getProjectInstance = new DAL.DALprojectInstance();
        public static readonly DAL.DALtaskInstance getTaskIntance = new DAL.DALtaskInstance();
        public static readonly DAL.DALresources getResources = new DAL.DALresources();
        public static readonly DAL.DALriskProjectResAssignment getPrjResAssign = new DAL.DALriskProjectResAssignment();
        public static readonly DAL.DALriskTaskResourceAssignment getTaskResAssign = new DAL.DALriskTaskResourceAssignment();
        public static readonly DAL.DALtaskInstance_res getTaskIntance_res = new DAL.DALtaskInstance_res();
    }
}
