using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace DAL
{
    public class DALtaskInstance
    {
        #region add
        public int save(MODEL.risk_task_instance taskInstance)
        {
            string sql = "insert into risk_task_instance(AUTO_ID,TASK_AUTO_ID,PRJ_ID,TASK_IS_CRITICAL,TASK_ACTUAL_DUR_PERIOD,SIM_SEQUENCE,SIM_VERSION,TASK_VE,TASK_VL,START_TIME) values(RISK_TASK_INSTANCE_AUTOID.nextval,:taskAutoId,:prjId,:isCritical,:durationTime,:simSeq,:simVersion,:taskVe,:taskVl,:StartTime)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                                         new OracleParameter(":prjId",OracleDbType.Int32,11),
                                         new OracleParameter(":isCritical",OracleDbType.Int32,1),
                                         new OracleParameter(":durationTime",OracleDbType.Double),
                                         new OracleParameter(":simSeq",OracleDbType.Int32,11),
                                         new OracleParameter(":simVersion",OracleDbType.Int32,11),
                                         new OracleParameter(":taskVe",OracleDbType.Double,9),
                                         new OracleParameter(":taskVl",OracleDbType.Double,9),
                                         new OracleParameter(":StartTime",OracleDbType.Double,9)
                         };
            parameters[0].Value = taskInstance.Task_auto_id;
            parameters[1].Value = taskInstance.Prj_id;
            parameters[2].Value = taskInstance.Task_is_critical;
            parameters[3].Value = taskInstance.Task_actual_dur_period;
            parameters[4].Value = taskInstance.Sim_sequence;
            parameters[5].Value = taskInstance.Sim_version;
            parameters[6].Value = taskInstance.Task_ve;
            parameters[7].Value = taskInstance.Task_vl;
            parameters[8].Value = taskInstance.Starttime;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
                sql = "select RISK_TASK_INSTANCE_AUTOID.currval from dual";
                parameters = null;
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        int id = Int32.Parse(reader.GetValue(0).ToString());
                        return id;
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void saveResource(MODEL.risk_task_instance_res taskInstance_res)
        {
            string sql = "insert into risk_task_instance_res(TASK_INSTANCE_ID,TASK_AUTO_ID,RESOURCE_ID,WAIT_TIME) values(:Instance_id,:Task_auto_id,:Resource_id,:Wait_time)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":Instance_id",OracleDbType.Int32,11),
                                         new OracleParameter(":Task_auto_id",OracleDbType.Int32,11),
                                         new OracleParameter(":Resource_id",OracleDbType.Int32,1),
                                         new OracleParameter(":Wait_time",OracleDbType.Double)
                         };
            parameters[0].Value = taskInstance_res.Instance_id;
            parameters[1].Value = taskInstance_res.Task_auto_id;
            parameters[2].Value = taskInstance_res.Resource_id;
            parameters[3].Value = taskInstance_res.Wait_time;
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

        #region  update
        #endregion

        #region  get
        public List<MODEL.risk_task_instance> get(int projectId)
        {
            List<MODEL.risk_task_instance> taskInstanceList = new List<MODEL.risk_task_instance>();
            string sql = "select * from risk_task_instance where PRJ_ID=:prjId";

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
                        MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance();
                        taskInstance.Auto_id = Int32.Parse(reader["AUTO_ID"].ToString());
                        taskInstance.Task_auto_id = Int32.Parse(reader["TASK_AUTO_ID"].ToString());
                        taskInstance.Prj_id = projectId;
                        taskInstance.Task_is_critical = Int32.Parse(reader["TASK_IS_CRITICAL"].ToString());
                        taskInstance.Task_actual_dur_period = Double.Parse(reader["TASK_ACTUAL_DUR_PERIOD"].ToString());
                        taskInstance.Sim_sequence = Int32.Parse(reader["SIM_SEQUENCE"].ToString());
                        taskInstance.Sim_version = Int32.Parse(reader["SIM_VERSION"].ToString());
                        taskInstanceList.Add(taskInstance);
                    }
                    return taskInstanceList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public List<MODEL.risk_task_instance> getVersionTaskInstance(int projectId, int versionId)
        {
            List<MODEL.risk_task_instance> taskInstanceList = new List<MODEL.risk_task_instance>();
            string sql = "select * from risk_task_instance where PRJ_ID=:prjId and SIM_VERSION=:simVersionId";

            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":prjId",OracleDbType.Int32,11),
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = projectId;
            parameters[1].Value = versionId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance();
                        taskInstance.Auto_id = Int32.Parse(reader["AUTO_ID"].ToString());
                        taskInstance.Task_auto_id = Int32.Parse(reader["TASK_AUTO_ID"].ToString());
                        taskInstance.Prj_id = projectId;
                        taskInstance.Task_is_critical = Int32.Parse(reader["TASK_IS_CRITICAL"].ToString());
                        taskInstance.Task_actual_dur_period = Double.Parse(reader["TASK_ACTUAL_DUR_PERIOD"].ToString());
                        taskInstance.Sim_sequence = Int32.Parse(reader["SIM_SEQUENCE"].ToString());
                        taskInstance.Sim_version = Int32.Parse(reader["SIM_VERSION"].ToString());
                        taskInstance.Task_ve = Double.Parse(reader["TASK_VE"].ToString());
                        taskInstance.Starttime = Double.Parse(reader["START_TIME"].ToString());
                        taskInstanceList.Add(taskInstance);
                    }
                    return taskInstanceList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public double getSum(int simVersionId, int taskAutoId)
        {
            string sql = @"SELECT SUM(task_actual_dur_period) FROM RISK_TASK_INSTANCE where SIM_VERSION=:simVersionId and TASK_AUTO_ID=:taskAutoId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        object dd = reader.GetValue(0);
                        double sum = Convert.ToDouble(reader.GetValue(0));
                        return sum;
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public double getMaxPeriod(int simVersionId, int taskAutoId) //选出最大的工期
        {
            string sql = "SELECT MAX(TASK_ACTUAL_DUR_PERIOD) FROM RISK_TASK_INSTANCE WHERE SIM_VERSION=:simVersionId AND TASK_AUTO_ID=:taskAutoId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        return Convert.ToDouble(reader.GetValue(0));
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public double getMinPeriod(int simVersionId, int taskAutoId)
        {
            string sql = "SELECT MIN(TASK_ACTUAL_DUR_PERIOD) FROM RISK_TASK_INSTANCE WHERE SIM_VERSION=:simVersionId AND TASK_AUTO_ID=:taskAutoId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        return Convert.ToDouble(reader.GetValue(0));
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public int dotTest(int simVersionId, int maxN, int minN, int taskAutoId)
        {
            string sql = "SELECT COUNT(*) FROM RISK_TASK_INSTANCE WHERE SIM_VERSION=:simVersionId AND TASK_ACTUAL_DUR_PERIOD>=:minN AND TASK_ACTUAL_DUR_PERIOD<:maxN AND TASK_AUTO_ID=:taskAutoId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":minN",OracleDbType.Int32,11),
                                         new OracleParameter(":maxN",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = minN;
            parameters[2].Value = maxN;
            parameters[3].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        int sum = Convert.ToInt32(reader.GetValue(0));
                        return sum;
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public int getCriticalSum(int simVersionId, int taskAutoId)//获取所有任务仿真为关键任务的个数
        {
            string sql = @"SELECT SUM(TASK_IS_CRITICAL) FROM RISK_TASK_INSTANCE where SIM_VERSION=:simVersionId and TASK_AUTO_ID=:taskAutoId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = taskAutoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader.GetValue(0));
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public List<MODEL.risk_task_instance> getBySeqVer(int simVersionId, int simSequence)
        {
            List<MODEL.risk_task_instance> taskInstanceList = new List<MODEL.risk_task_instance>();
            string sql = @"select * from risk_task_instance where sim_sequence=:simSequence and sim_version=:simVersionId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simSequence",OracleDbType.Int32,11),
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simSequence;
            parameters[1].Value = simVersionId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_task_instance taskInstance = new MODEL.risk_task_instance();
                        taskInstance.Auto_id = Int32.Parse(reader["AUTO_ID"].ToString());
                        taskInstance.Task_auto_id = Int32.Parse(reader["TASK_AUTO_ID"].ToString());
                        taskInstance.Prj_id = Int32.Parse(reader["PRJ_ID"].ToString()); ;
                        taskInstance.Task_is_critical = Int32.Parse(reader["TASK_IS_CRITICAL"].ToString());
                        taskInstance.Task_actual_dur_period = Double.Parse(reader["TASK_ACTUAL_DUR_PERIOD"].ToString());
                        taskInstance.Sim_sequence = Int32.Parse(reader["SIM_SEQUENCE"].ToString());
                        taskInstance.Sim_version = Int32.Parse(reader["SIM_VERSION"].ToString());
                        taskInstanceList.Add(taskInstance);
                    }
                    return taskInstanceList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion



    }
}
