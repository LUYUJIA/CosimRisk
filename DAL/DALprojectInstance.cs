using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace DAL
{
    public class DALprojectInstance
    {
        #region add
        public int save(MODEL.risk_prj_instance projectInstance)
        {
            string sql = "insert into risk_prj_instance(AUTO_ID,SIM_VERSION,SIM_PROJECT_TIME,SIM_SEQUENCE,SIM_PROJECT_COST) values(RISK_PRJ_INSTANCE_AUTOID.nextval,:simVersion,:simPrjTime,:myCount,:simPrjCost)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersion",OracleDbType.Int32,11),
                                         new OracleParameter(":simPrjTime",OracleDbType.Double),
                                         new OracleParameter(":myCount",OracleDbType.Int32,11),
                                          new OracleParameter(":simPrjCost",OracleDbType.Double)
                         };
            parameters[0].Value = projectInstance.Sim_version;
            parameters[1].Value = projectInstance.Sim_project_time;
            parameters[2].Value = projectInstance.Sim_sequence;
            parameters[3].Value = projectInstance.Sim_project_cost;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
                sql = "select RISK_PRJ_INSTANCE_AUTOID.currval from dual";
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
        #endregion

        #region save resource
        public void saveResource(MODEL.risk_prj_instance_res projectInstance_res)
        {
            string sql = "insert into risk_prj_instance_res(PRJ_INSTANCE_ID,RESOURCE_ID,COST_AMOUNT) values(:Instance_id,:Resource_id,:Cost_amount)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":Instance_id",OracleDbType.Int32,11),
                                         new OracleParameter(":Resource_id",OracleDbType.Double),
                                         new OracleParameter(":Cost_amount",OracleDbType.Int32,11),
                         };
            parameters[0].Value = projectInstance_res.Instance_id;
            parameters[1].Value = projectInstance_res.Resource_id;
            parameters[2].Value = projectInstance_res.Cost_amount;
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
        public bool update(MODEL.risk_prj_instance projectInstance)
        {
            string sql = "update risk_prj_instance set SIM_VERSION=:simVersion,SIM_PROJECT_TIME=:simPrjTime,SIM_SEQUENCE=:simSeq where AUTO_ID=:autoId";
            OracleParameter[] parameters = new OracleParameter[]{
                                new OracleParameter(":simVersion",OracleDbType.Int32,11),
                                new OracleParameter(":simPrjTime",OracleDbType.Varchar2,255),
                                new OracleParameter(":simSeq",OracleDbType.Int32,11),
                                new OracleParameter(":autoId",OracleDbType.Double)
                        };
            parameters[0].Value = projectInstance.Sim_version;
            parameters[1].Value = projectInstance.Sim_project_time;
            parameters[2].Value = projectInstance.Sim_sequence;
            parameters[3].Value = projectInstance.Auto_id;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  get
        public List<MODEL.risk_prj_instance> getBySimVersion(int simVersionId)
        {
            string sql = "select auto_id,sim_sequence,sim_project_time from risk_prj_instance where sim_version=:simVersion";
            OracleParameter parameter = new OracleParameter(":simVersion", OracleDbType.Int32, 11);
            parameter.Value = simVersionId;
            try
            {
                List<MODEL.risk_prj_instance> prjInstanceList = new List<MODEL.risk_prj_instance>();
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameter))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_prj_instance prjInstance = new MODEL.risk_prj_instance();
                        prjInstance.Auto_id = Int32.Parse(reader["auto_id"].ToString());
                        prjInstance.Sim_sequence = Int32.Parse(reader["sim_sequence"].ToString());
                        prjInstance.Sim_project_time = Double.Parse(reader["sim_project_time"].ToString());
                        prjInstance.Sim_version = simVersionId;
                        prjInstanceList.Add(prjInstance);
                    }
                }
                return prjInstanceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public double findMaxPrjTimeBelow(int simVersionId, double maxN)
        {
            string sql = "SELECT MAX(SIM_PROJECT_TIME)FROM (SELECT SIM_PROJECT_TIME FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_TIME<:maxN)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":maxN",OracleDbType.Double,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = maxN;
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
        public double findMinPrjTimeAbove(int simVersionId, double minN)
        {
            string sql = "SELECT MIN(SIM_PROJECT_TIME)FROM (SELECT SIM_PROJECT_TIME FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_TIME>:minN)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":minN",OracleDbType.Double,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = minN;
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
        public double findMaxPrjCostBelow(int simVersionId, double maxN)
        {
            string sql = "SELECT MAX(SIM_PROJECT_COST)FROM (SELECT SIM_PROJECT_COST FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_COST<:maxN)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":maxN",OracleDbType.Double,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = maxN;
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
        public double findMinPrjCostAbove(int simVersionId, double minN)
        {
            string sql = "SELECT MIN(SIM_PROJECT_COST)FROM (SELECT SIM_PROJECT_COST FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_COST>:minN)";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":minN",OracleDbType.Double,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = minN;
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
        public double findAVGDirectCost(int simVersionId)
        {
            string sql = "SELECT  AVG(SIM_PROJECT_COST) AS avg_cost FROM RISK_PRJ_INSTANCE where SIM_VERSION = :simVersionId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        return Convert.ToDouble(reader["avg_cost"]);
                    }
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #region  得到工期和
        public double getTotalDuration(int simVersionId)
        {
            string sql = "SELECT SUM(SIM_PROJECT_TIME) FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simVersionId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
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
        public double getAvgDuration(int simVersionId)
        {
            string sql = "SELECT AVG(SIM_PROJECT_TIME) FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simVersionId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
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
        #endregion

        public int getCount(int simVersionId, int maxN, int minN)
        {
            string sql = "SELECT COUNT(*) FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_TIME>=:minN AND SIM_PROJECT_TIME<:maxN ";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":minN",OracleDbType.Int32,11),
                                         new OracleParameter(":maxN",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = minN;
            parameters[2].Value = maxN;
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
        public int getCostCount(int simVersionId, int maxN, int minN)
        {
            string sql = "SELECT COUNT(*) FROM RISK_PRJ_INSTANCE WHERE SIM_VERSION=:simVersionId AND SIM_PROJECT_COST>=:minN AND SIM_PROJECT_COST<=:maxN ";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":minN",OracleDbType.Int32,11),
                                         new OracleParameter(":maxN",OracleDbType.Int32,11)
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = minN;
            parameters[2].Value = maxN;
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
    }
}
