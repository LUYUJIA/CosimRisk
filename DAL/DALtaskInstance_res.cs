using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace DAL
{
    public class DALtaskInstance_res
    {
        public List<MODEL.risk_task_instance_res> getWaitResList(int simVersionId, int taskAutoId)
        {
            string sql = "SELECT RESOURCE_ID , Round(AVG(WAIT_TIME),2) as WaitTime FROM RISK_TASK_INSTANCE_RES  where TASK_INSTANCE_ID IN (SELECT  AUTO_ID from RISK_TASK_INSTANCE where SIM_VERSION =:simVersionId ) AND TASK_AUTO_ID =:taskAutoId GROUP BY RESOURCE_ID";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":simVersionId",OracleDbType.Int32,11),
                                         new OracleParameter(":taskAutoId",OracleDbType.Int32,11),
                         };
            parameters[0].Value = simVersionId;
            parameters[1].Value = taskAutoId;
            try
            {
                List<MODEL.risk_task_instance_res> reslist = new List<MODEL.risk_task_instance_res>();
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    while (reader.Read())
                    {
                        MODEL.risk_task_instance_res res = new MODEL.risk_task_instance_res();
                        res.Resource_id = Int32.Parse(reader["RESOURCE_ID"].ToString());
                        res.Wait_time = Double.Parse(reader["WaitTime"].ToString());
                        reslist.Add(res);
                    }
                    return reslist;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
