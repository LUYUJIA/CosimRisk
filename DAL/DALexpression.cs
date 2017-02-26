using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace DAL
{
    public class DALexpression
    {
        #region 保存expression
        public int saveExpression(MODEL.risk_math_expression_arg expression)
        {
            try
            {
                string expressionName = expression.Name;
                string sql = @"select auto_id  from risk_math_expression_arg where task_auto_id=:taskId";
                OracleParameter taskIdParameter = new OracleParameter(":expName", OracleDbType.Int32, 11);
                taskIdParameter.Value = expression.Task_id;
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, taskIdParameter))
                {
                    if (reader.Read())  //if exists
                    {
                        int expressAutoId = Int32.Parse(reader.GetValue(0).ToString());
                        reader.Close();
                        sql = "select auto_id from RISK_MATH_EXPRESSION where name=:expName";
                        OracleParameter expName = new OracleParameter(":expName", OracleDbType.Varchar2, 64);
                        expName.Value = expression.Name;
                        using (OracleDataReader idReader = SqlHelper.ExecuteQuery(sql, expName))
                        {
                            if (idReader.Read())
                                expression.Expression_id = Int32.Parse(idReader.GetOracleValue(0).ToString());
                            else return -1;
                            sql = "update risk_math_expression_arg set name=:Name, value=to_blob(:Value),expression_id=:ExpressionId,actual_value=:actualValue where task_auto_id=:taskAutoId";
                            OracleParameter[] parameters = new OracleParameter[] {
                                                                new OracleParameter(":Name",OracleDbType.Varchar2,64),
                                                                new OracleParameter(":Value",OracleDbType.Blob),
                                                                new OracleParameter(":ExpressionId",OracleDbType.Int32,11),
                                                                new OracleParameter(":actualValue",OracleDbType.Double),
                                                                new OracleParameter(":taskAutoId",OracleDbType.Int32,11)
                                                        };
                            parameters[0].Value = expression.Name;
                            parameters[1].Value = expression.Value;
                            parameters[2].Value = expression.Expression_id;
                            parameters[3].Value = expression.Actual_value;
                            parameters[4].Value = expression.Task_id;
                            SqlHelper.ExecuteNonQuery(sql, parameters);
                            return expressAutoId;
                        }
                    }
                    else
                    {
                        sql = @"select auto_id from RISK_MATH_EXPRESSION where name=:expName";
                        OracleParameter expName = new OracleParameter(":expName", OracleDbType.Varchar2, 64);
                        expName.Value = expression.Name;
                        reader.Close();
                        using (OracleDataReader idReader = SqlHelper.ExecuteQuery(sql, expName))
                        {
                            if (idReader.Read())
                                expression.Expression_id = Int32.Parse(idReader.GetOracleValue(0).ToString());
                            else
                            {
                                return -1;
                            }
                            sql = "insert into risk_math_expression_arg(auto_id,name,value,task_auto_id,expression_id,actual_value) values(RISK_EXPRESSION_ARG_AUTOID.NEXTVAL,:NAME,to_blob(:VALUE),:TASKID,:EXPRESSIONID,:actualValue)";
                            OracleParameter[] parameters = new OracleParameter[] {
                                                                new OracleParameter(":NAME",OracleDbType.Varchar2,64),
                                                                new OracleParameter(":VALUE",OracleDbType.Blob),
                                                                new OracleParameter(":TASKID",OracleDbType.Int32,11),                                        
                                                                new OracleParameter(":EXPRESSIONID",OracleDbType.Int32,11),
                                                                new OracleParameter(":actualValue",OracleDbType.Double)
                                                        };
                            parameters[0].Value = expression.Name;
                            parameters[1].Value = expression.Value;
                            parameters[2].Value = expression.Task_id;
                            parameters[3].Value = expression.Expression_id;
                            parameters[4].Value = expression.Actual_value;
                            SqlHelper.ExecuteNonQuery(sql, parameters);
                            sql = "select RISK_EXPRESSION_ARG_AUTOID.currval from dual";
                            parameters = null;
                            using (OracleDataReader curReader = SqlHelper.ExecuteQuery(sql, parameters))
                            {
                                if (curReader.Read())
                                {
                                    int id = Int32.Parse(curReader.GetValue(0).ToString());
                                    return id;
                                }
                                else return -1;
                            }
                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  更新expression
        #endregion

        #region 删除expression
        #endregion

        #region 获得expression

        public MODEL.risk_math_expression_arg getExpression(int taskAutoId)
        {
            try
            {
                MODEL.risk_math_expression_arg expression = new MODEL.risk_math_expression_arg();
                expression.Task_id = taskAutoId;
                string sql = "select auto_id,expression_id,TASK_AUTO_ID,value,name,actual_value from risk_math_expression_arg where task_auto_id=:taskAutoId";
                OracleParameter taskIdParameter = new OracleParameter(":expName", OracleDbType.Int32, 11);
                taskIdParameter.Value = expression.Task_id;
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, taskIdParameter))
                {
                    if (reader.Read())
                    {
                        expression.Auto_id = Int32.Parse(reader["auto_id"].ToString());
                        expression.Expression_id = Int32.Parse(reader["expression_id"].ToString());
                        expression.Task_id = Int32.Parse(reader["TASK_AUTO_ID"].ToString());
                        expression.Value = reader.GetOracleBlob(3).Value;
                        expression.Name = reader["name"].ToString();
                        expression.Actual_value = Convert.ToDouble(reader["actual_value"]);
                        return expression;
                    }
                    else return null;
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
