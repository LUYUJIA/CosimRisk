using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace DAL
{
    public class DALriskProjectResAssignment
    {
        #region 保存

        public void insertRes(int projectId, MODEL.risk_resource res)
        {
            string sql = "insert into risk_project_res_assignment(auto_id,pri_id,resource_id,assignment_amount,assignment_remains)values(RISK_RESOURCE_AUTOID.NEXTVAL,:prjId,:resourceId,:assignmentAmount,:assignmentRemains)";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":prjId",OracleDbType.Int32,11),
                new OracleParameter(":resourceId",OracleDbType.Int32,11),
                new OracleParameter(":assignmentAmount",OracleDbType.Int32,11),
                new OracleParameter(":assignmentRemains",OracleDbType.Int32,11)
            };
            parameters[0].Value = projectId;
            parameters[1].Value = res.Auto_id;
            parameters[2].Value = res.Resource_amount;
            parameters[3].Value = res.Resource_amount;
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

         #region 显示项目资源
        public  List<MODEL.risk_resource> showResources(int projectId)
        {
            string sql = "select * from risk_project_res_assignment where pri_id =:projectId";
            OracleParameter[] parameters = new OracleParameter[] {

                new OracleParameter(":projectId",OracleDbType.Int32,11)
            };
            parameters[0].Value = projectId;
            try
            {
                using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    List<MODEL.risk_resource> ResList = new List<MODEL.risk_resource>();
                    while (myReader.Read())
                    {
                        DALresources get_Resource = new DALresources();
                        MODEL.risk_resource Res = new MODEL.risk_resource();
                        Res = get_Resource.findResById(Int32.Parse(myReader["RESOURCE_ID"].ToString()));
                        Res.Resource_amount = Int32.Parse(myReader["assignment_amount"].ToString());
                        ResList.Add(Res);
                    }
                    return ResList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除
        public void deleteByPrjIdResId(MODEL.risk_project_res_assignment res_assignment)
        {
            string sql = "delete risk_project_res_assignment where PRI_ID=:prjId and RESOURCE_ID=:resourceId";

            OracleParameter[] parameters = new OracleParameter[] {
   
                 new OracleParameter(":prjId",OracleDbType.Int32,11),
                new OracleParameter(":resourceId",OracleDbType.Int32,11)
            };
            parameters[0].Value = res_assignment.Pri_id;
            parameters[1].Value = res_assignment.Resource_id;
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

        #region 修改
        public void updateByPrjIdResId(MODEL.risk_project_res_assignment prjResAssign)
        {
            string sql = "update risk_project_res_assignment set assignment_amount=:assignmentAmount,assignment_remains=:assignmentRemains where PRI_ID=:prjId and RESOURCE_ID=:resId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":assignmentAmount",OracleDbType.Int32,11),
                new OracleParameter(":assignmentRemains",OracleDbType.Int32,11),
                new OracleParameter(":prjId",OracleDbType.Int32,11),
                new OracleParameter(":resId",OracleDbType.Int32,11)
            };
            parameters[0].Value = prjResAssign.Assignment_remains;
            parameters[1].Value = prjResAssign.Assignment_remains;
            parameters[2].Value = prjResAssign.Pri_id;
            parameters[3].Value = prjResAssign.Resource_id;
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

        #region 查找

        public MODEL.risk_project_res_assignment findResByPrjIdResId(int prjId, int resId)
        {
            string sql = "select * from risk_project_res_assignment where PRI_ID=:prjId and RESOURCE_ID=:resId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":prjId",OracleDbType.Int32,11),
                new OracleParameter(":resId",OracleDbType.Int32,11)
            };
            parameters[0].Value = prjId;
            parameters[1].Value = resId;
            MODEL.risk_project_res_assignment prjResAssign = new MODEL.risk_project_res_assignment();
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        prjResAssign.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        prjResAssign.Assignment_amount = Convert.ToInt32(reader["ASSIGNMENT_AMOUNT"]);
                        prjResAssign.Assignment_remains = Convert.ToInt32(reader["ASSIGNMENT_REMAINS"]);
                        prjResAssign.Pri_id = prjId;
                        prjResAssign.Resource_id = resId;
                    }
                }
                return prjResAssign;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MODEL.risk_project_res_assignment> getPrjResAssignListByPrjId(int projectId)
        {
            List<MODEL.risk_project_res_assignment> prjResAssignList = new List<MODEL.risk_project_res_assignment>();
            string sql = "select * from risk_project_res_assignment where PRI_ID=:prjId";
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
                        MODEL.risk_project_res_assignment prjResAssign = new MODEL.risk_project_res_assignment();
                        prjResAssign.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        prjResAssign.Assignment_amount = Convert.ToInt32(reader["ASSIGNMENT_AMOUNT"]);
                        prjResAssign.Assignment_remains = Convert.ToInt32(reader["ASSIGNMENT_REMAINS"]);
                        prjResAssign.Pri_id = projectId;
                        prjResAssign.Resource_id = Convert.ToInt32(reader["resource_id"]); ;
                        prjResAssignList.Add(prjResAssign);
                    }
                }
                return prjResAssignList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
