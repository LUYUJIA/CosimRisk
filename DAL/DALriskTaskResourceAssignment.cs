using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace DAL
{
    public class DALriskTaskResourceAssignment
    {
        #region 插入
        public void insertRes(int taskAutoId, MODEL.risk_resource res)
        {
            string sql = "insert into risk_task_resource_assignment(auto_id,task_auto_id,resource_id,assignment_amount,assignment_own)values(RISK_TASK_RES_ASSIGN_AUTOID.NEXTVAL,:taskAutoId,:resourceId,:assignmentAmount,:assignmentOwn)";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                new OracleParameter(":resourceId",OracleDbType.Int32,11),
                new OracleParameter(":assignmentAmount",OracleDbType.Int32,11),
                new OracleParameter(":assignmentOwn",OracleDbType.Int32,11)
            };
            DAL.DALresources resource_operation = new DALresources();
            parameters[0].Value = taskAutoId;
            parameters[1].Value = resource_operation.findResByName(res.Resource_name).Auto_id;
            parameters[2].Value = res.Resource_amount;
            parameters[3].Value = 0;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查找
        public MODEL.risk_task_resource_assignment findByTaskAutoIdResId(int taskAutoId, int resId)
        {
            MODEL.risk_task_resource_assignment taskResAssign = new MODEL.risk_task_resource_assignment();
            string sql = "select * from risk_task_resource_assignment where task_auto_id=:taskAutoId and resource_id=:resId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                new OracleParameter(":resId",OracleDbType.Int32,11)
            };
            parameters[0].Value = taskAutoId;
            parameters[1].Value = resId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        taskResAssign.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        taskResAssign.Assignment_amount = Convert.ToInt32(reader["assignment_amount"]);
                        taskResAssign.Assignment_own = Convert.ToInt32(reader["assignment_own"]);
                        taskResAssign.Resource_id = resId;
                        taskResAssign.Task_auto_id = taskAutoId;
                    }
                    return taskResAssign;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<MODEL.risk_resource> findResByTaskAutoId(int taskAutoId)
        {
            List<MODEL.risk_resource> resourcesList = new List<MODEL.risk_resource>();
            string sql = "select * from risk_resource where AUTO_ID in (select RESOURCE_ID from risk_task_resource_assignment where TASK_AUTO_ID=:taskAutoId)";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":taskAutoId",OracleDbType.Int32,11)
            };
            parameters[0].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_resource res = new MODEL.risk_resource();
                        res.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        res.Resource_amount = Convert.ToInt32(reader["resource_amount"]);
                        res.Resource_description = reader["resource_description"].ToString();
                        res.Resource_name = reader["resource_name"].ToString();
                        res.Resource_remains = Convert.ToInt32(reader["resource_remains"]);
                        int resourceTypeId = Convert.ToInt32(reader["resource_type"]);
                        res.Resource_type = DALresources.findresourceTypeNameById(resourceTypeId);
                        res.Resource_unit_price = Convert.ToDouble(reader["resource_unit_price"]);
                        resourcesList.Add(res);
                    }
                }
                return resourcesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MODEL.risk_task_resource_assignment> getTaskResAssignListByPrjId(int projectId)
        {
            List<MODEL.risk_task_resource_assignment> TaskResAssignList = new List<MODEL.risk_task_resource_assignment>();
            string sql = "select * from RISK_TASK_RESOURCE_ASSIGNMENT where TASK_AUTO_ID IN (SELECT AUTO_ID from RISK_TASK WHERE TASK_PROJECT_ID =:prjId)";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":prjId",OracleDbType.Int32,11)
            };
            parameters[0].Value = projectId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_task_resource_assignment TaskResAssign = new MODEL.risk_task_resource_assignment();
                        TaskResAssign.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        TaskResAssign.Task_auto_id = Convert.ToInt32(reader["task_auto_id"]);
                        TaskResAssign.Resource_id = Convert.ToInt32(reader["resource_id"]);
                        TaskResAssign.Assignment_amount = Convert.ToInt32(reader["assignment_amount"]);
                        TaskResAssign.Assignment_own = Convert.ToInt32(reader["assignment_own"]); 
                        TaskResAssignList.Add(TaskResAssign);
                    }
                }
                return TaskResAssignList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 更新
        public void updateByTaskAutoIdResId(MODEL.risk_task_resource_assignment taskResAssign)
        {
            string sql = "update risk_task_resource_assignment set assignment_amount=:assignmentAmount,assignment_own=:assignmentOwn where task_auto_id=:taskAutoId and RESOURCE_ID=:resId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":assignmentAmount",OracleDbType.Int32,11),
                new OracleParameter(":assignmentOwn",OracleDbType.Int32,11),
                new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                new OracleParameter(":resId",OracleDbType.Int32,11)
            };
            parameters[0].Value = taskResAssign.Assignment_amount;
            parameters[1].Value = taskResAssign.Assignment_own;
            parameters[2].Value = taskResAssign.Task_auto_id;
            parameters[3].Value = taskResAssign.Resource_id;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 
        
        #region 删除
        public void deleteByTaskAutoIdResId(MODEL.risk_task_resource_assignment taskResAssign)
        {
            string sql = "delete risk_task_resource_assignment where task_auto_id=:taskAutoId and RESOURCE_ID=:resId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                new OracleParameter(":resId",OracleDbType.Int32,11)
            };
            parameters[0].Value = taskResAssign.Task_auto_id;
            parameters[1].Value = taskResAssign.Resource_id;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 
    }
}
