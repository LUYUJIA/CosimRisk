using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace DAL
{
        public class DALtask
        {
                #region 保存task
                public bool insertTask(MODEL.risk_task task)
                {
                        string sql = "insert into risk_task(auto_id,task_is_summary,task_name,task_wbs,task_id,task_level,task_project_id,task_nested_parent_id)values( risk_task_AUTOID.NEXTVAL ,:isSummary,:taskName,:taskWBS,:taskId,:taskLevel,:projectId,:taskNestedParentId)";
                        OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":isSummary",OracleDbType.Int32,1),
                                         new OracleParameter(":taskName",OracleDbType.Varchar2,255),
                                         new OracleParameter(":taskWBS",OracleDbType.Varchar2,10),                                    
                                         new OracleParameter(":taskId",OracleDbType.Varchar2,64),
                                         new OracleParameter(":taskLevel",OracleDbType.Int32,11),
                                         new OracleParameter(":projectId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskNestedParentId",OracleDbType.Int32,11)
                                };
                        parameters[0].Value = task.Task_is_summary;
                        parameters[1].Value = task.Task_name;
                        parameters[2].Value = task.Task_wbs;
                        parameters[3].Value = task.Task_id;
                        parameters[4].Value = task.Task_level;
                        parameters[5].Value = task.Task_project_id;
                        parameters[6].Value = task.Task_nested_parent_id;
                        try
                        {
                                SqlHelper.ExecuteNonQuery(sql, parameters);
                                return true;
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }
                }
                public bool insertTask(List<MODEL.risk_task> taskList)
                {
                        string sql = "insert into risk_task(auto_id,task_is_summary,task_name,task_wbs,task_id,task_level,task_project_id,task_nested_parent_id)values( risk_task_AUTOID.NEXTVAL ,:isSummary,:taskName,:taskWBS,:taskId,:taskLevel,:projectId,:taskNestedParentId)";
                        foreach (MODEL.risk_task task in taskList)
                        {
                                OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":isSummary",OracleDbType.Int32,1),
                                         new OracleParameter(":taskName",OracleDbType.Varchar2,255),
                                         new OracleParameter(":taskWBS",OracleDbType.Varchar2,10),                                    
                                         new OracleParameter(":taskId",OracleDbType.Varchar2,64),
                                         new OracleParameter(":taskLevel",OracleDbType.Int32,11),
                                         new OracleParameter(":projectId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskNestedParentId",OracleDbType.Int32,11)
                                };
                                parameters[0].Value = task.Task_is_summary;
                                parameters[1].Value = task.Task_name;
                                parameters[2].Value = task.Task_wbs;
                                parameters[3].Value = task.Task_id;
                                parameters[4].Value = task.Task_level;
                                parameters[5].Value = task.Task_project_id;
                                parameters[6].Value = task.Task_nested_parent_id;
                                try
                                {
                                        SqlHelper.ExecuteNonQuery(sql, parameters);
                                } catch (System.Exception ex)
                                {
                                        throw ex;
                                }
                        }
                        return true;
                }
                #endregion

                #region 保存link
                public bool insertLink(MODEL.risk_link link)
                {
                        string sql = "insert into risk_link(auto_id,task_pre_id,task_suc_id,link_type,delay_days,prj_id)values( risk_link_AUTOID.NEXTVAL ,:task_pre_id,:task_suc_id,:link_type,:delay_days,:prj_id)";
                        OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":task_pre_id",OracleDbType.Int32,11),
                                         new OracleParameter(":task_suc_id",OracleDbType.Int32,11),                                    
                                         new OracleParameter(":link_type",OracleDbType.Int32,11),
                                         new OracleParameter(":delay_days",OracleDbType.Int32,11),
                                         new OracleParameter(":prj_id",OracleDbType.Int32,11)
                                };
                        parameters[0].Value = link.Task_pre_id;
                        parameters[1].Value = link.Task_suc_id;
                        parameters[2].Value = link.Link_type;
                        parameters[3].Value = link.Delay_days;
                        parameters[4].Value = link.Prj_id;
                        try
                        {
                                SqlHelper.ExecuteNonQuery(sql, parameters);
                                return true;
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }
                }
                public bool insertLink(List<MODEL.risk_link> linkList)
                {
                        string sql = "insert into risk_link(auto_id,task_pre_id,task_suc_id,link_type,delay_days,prj_id)values( risk_link_AUTOID.NEXTVAL ,:task_pre_id,:task_suc_id,:link_type,:delay_days,:prj_id)";
                        foreach (MODEL.risk_link link in linkList)
                        {
                                OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":task_pre_id",OracleDbType.Int32,11),
                                         new OracleParameter(":task_suc_id",OracleDbType.Int32,11),                                    
                                         new OracleParameter(":link_type",OracleDbType.Int32,11),
                                         new OracleParameter(":delay_days",OracleDbType.Int32,11),
                                         new OracleParameter(":prj_id",OracleDbType.Int32,11)
                                };
                                parameters[0].Value = link.Task_pre_id;
                                parameters[1].Value = link.Task_suc_id;
                                parameters[2].Value = link.Link_type;
                                parameters[3].Value = link.Delay_days;
                                parameters[4].Value = link.Prj_id;
                                try
                                {
                                        SqlHelper.ExecuteNonQuery(sql, parameters);
                                } catch (System.Exception ex)
                                {
                                        throw ex;
                                }
                        }
                        return true;
                }
                #endregion

                #region 读取task
                public MODEL.risk_task getTask(int auto_id)
                {
                        MODEL.risk_task myTask = new MODEL.risk_task();
                        string sql = "select * from risk_task where auto_id=:autoId";
                        OracleParameter parameter = new OracleParameter(":autoId", OracleDbType.Int32, 11);
                        parameter.Value = auto_id;
                        try
                        {
                                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameter))
                                {
                                        if (reader.Read())
                                        {
                                                myTask.Auto_id = auto_id;
                                                if (reader["task_is_summary"].ToString() == "1")
                                                        myTask.Task_is_summary = true;
                                                else myTask.Task_is_summary = false;
                                                myTask.Task_name = reader["task_name"].ToString();
                                                myTask.Task_wbs = reader["task_wbs"].ToString();
                                                myTask.Task_id = Int32.Parse(reader["task_id"].ToString());
                                                myTask.Task_level = Int32.Parse(reader["task_level"].ToString());
                                                myTask.Task_project_id = Int32.Parse(reader["task_project_id"].ToString());
                                                myTask.Task_nested_parent_id = Int32.Parse(reader["task_nested_parent_id"].ToString());
                                                if (reader["task_priority"] != null && reader["task_priority"].ToString() != "")
                                                        myTask.Task_priority = Int32.Parse(reader["task_priority"].ToString());
                                                return myTask;
                                        }
                                        return null;
                                }

                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }
                }

                public List<MODEL.risk_task> getTaskList(int projectId)
                {
                        List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                        string sql = "select * from risk_task where TASK_PROJECT_ID=" + projectId;
                        OracleParameter p = null;
                        using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, p))
                        {
                                while (myReader.Read())
                                {
                                        MODEL.risk_task task = new MODEL.risk_task();
                                        if (myReader["task_is_summary"].ToString() == "1")
                                                task.Task_is_summary = true;
                                        else task.Task_is_summary = false;
                                        task.Auto_id = Int32.Parse(myReader["auto_id"].ToString()); ;
                                        task.Task_name = myReader["task_name"].ToString();
                                        task.Task_wbs = myReader["task_wbs"].ToString();
                                        task.Task_id = Int32.Parse(myReader["task_id"].ToString());
                                        task.Task_level = Int32.Parse(myReader["task_level"].ToString());
                                        task.Task_project_id = Int32.Parse(myReader["task_project_id"].ToString());
                                        task.Task_nested_parent_id = Int32.Parse(myReader["task_nested_parent_id"].ToString());
                                        if (myReader["task_priority"] != null && myReader["task_priority"].ToString() != "")
                                                task.Task_priority = Int32.Parse(myReader["task_priority"].ToString());
                                        taskList.Add(task);
                                }
                                return taskList;
                        }     
                }
                public List<MODEL.risk_task> getNestedTaskList(int projectId)
                {
                    List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                    string sql = "select * from risk_task where task_is_summary='0' and TASK_PROJECT_ID=" + projectId;
                    OracleParameter p = null;
                    using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, p))
                    {
                        while (myReader.Read())
                        {
                            MODEL.risk_task task = new MODEL.risk_task();
                            if (myReader["task_is_summary"].ToString() == "1")
                                task.Task_is_summary = true;
                            else task.Task_is_summary = false;
                            task.Auto_id = Int32.Parse(myReader["auto_id"].ToString()); ;
                            task.Task_name = myReader["task_name"].ToString();
                            task.Task_wbs = myReader["task_wbs"].ToString();
                            task.Task_id = Int32.Parse(myReader["task_id"].ToString());
                            task.Task_level = Int32.Parse(myReader["task_level"].ToString());
                            task.Task_project_id = Int32.Parse(myReader["task_project_id"].ToString());
                            task.Task_nested_parent_id = Int32.Parse(myReader["task_nested_parent_id"].ToString());
                            if (myReader["task_priority"] != null && myReader["task_priority"].ToString() != "")
                                task.Task_priority = Int32.Parse(myReader["task_priority"].ToString());
                            taskList.Add(task);
                        }
                        return taskList;
                    }
                }
               
                public List<MODEL.risk_task> getWaitTaskList(int VersionId)
                {
                    List<MODEL.risk_task> taskList = new List<MODEL.risk_task>();
                    string sql = "SELECT * FROM RISK_TASK where AUTO_ID in (SELECT distinct Task_AUTO_ID from RISK_TASK_INSTANCE where SIM_VERSION = :VersionId and AUTO_ID in (SELECT TASK_INSTANCE_ID FROM RISK_TASK_INSTANCE_RES))";
                    OracleParameter[] parameters = new OracleParameter[] {
                    new OracleParameter(":VersionId",OracleDbType.Int32,11)
                };
                    parameters[0].Value = VersionId;
                    using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, parameters))
                    {
                        while (myReader.Read())
                        {
                            MODEL.risk_task task = new MODEL.risk_task();
                            task.Auto_id = Int32.Parse(myReader["auto_id"].ToString()); ;
                            task.Task_name = myReader["task_name"].ToString();
                            taskList.Add(task);
                        }
                        return taskList;
                    }
                }
                #endregion

                #region 读取link
                public List<MODEL.risk_link> getLinkList(int projectId)
                {
                        List<MODEL.risk_link> linkList = new List<MODEL.risk_link>();
                        string sql = "select * from risk_link where PRJ_ID='" + projectId + "'";
                        OracleParameter p = null;
                        using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, p))
                        {
                                while (myReader.Read())
                                {
                                        MODEL.risk_link link = new MODEL.risk_link();
                                        link.Delay_days = Int32.Parse(myReader["delay_days"].ToString());
                                        link.Link_type = Int32.Parse(myReader["link_type"].ToString());
                                        link.Task_suc_id = Int32.Parse(myReader["task_suc_id"].ToString());
                                        link.Task_pre_id = Int32.Parse(myReader["task_pre_id"].ToString());
                                        link.Prj_id = Int32.Parse(myReader["prj_id"].ToString());
                                        linkList.Add(link);
                                }
                                return linkList;
                        }
                }
                #endregion

                #region 更新task
                public bool updateTask(MODEL.risk_task task)
                {
                        string sql = "update risk_task set task_is_summary=:isSummary,task_name=:taskName,task_wbs=:taskWBS,task_id=:taskId,task_level=:taskLevel,task_project_id=:projectId,task_nested_parent_id=:taskNestedParentId,task_expression_id=:taskExpressionId,task_priority=:taskPriority where auto_id=:autoId";

                        OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":isSummary",OracleDbType.Int32,1),
                                         new OracleParameter(":taskName",OracleDbType.Varchar2,255),
                                         new OracleParameter(":taskWBS",OracleDbType.Varchar2,10),                                    
                                         new OracleParameter(":taskId",OracleDbType.Varchar2,64),
                                         new OracleParameter(":taskLevel",OracleDbType.Int32,11),
                                         new OracleParameter(":projectId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskNestedParentId",OracleDbType.Int32,11),   
                                         new OracleParameter(":taskExpressionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskPriority",OracleDbType.Int32,11),
                                         new OracleParameter(":autoId",OracleDbType.Int32,11)
                                };
                        parameters[0].Value = task.Task_is_summary;
                        parameters[1].Value = task.Task_name;
                        parameters[2].Value = task.Task_wbs;
                        parameters[3].Value = task.Task_id;
                        parameters[4].Value = task.Task_level;
                        parameters[5].Value = task.Task_project_id;
                        parameters[6].Value = task.Task_nested_parent_id;
                        parameters[7].Value = task.Task_expression_id;
                        parameters[8].Value = task.Task_priority;
                        parameters[9].Value = task.Auto_id;
                        try
                        {
                                SqlHelper.ExecuteNonQuery(sql, parameters);
                                return true;
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }
                }
                #endregion
        }
}
