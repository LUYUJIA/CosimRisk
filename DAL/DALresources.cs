using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace DAL
{
    public class DALresources
    {
        #region 保存

        public bool insertRes(MODEL.risk_resource res)
        {
            string sql = "insert into risk_resource(auto_id,resource_amount,resource_description,resource_name,resource_remains,resource_type,resource_unit_price)values(RISK_RESOURCE_AUTOID.NEXTVAL,:resourceAmount,:resourceDescription,:resourceName,:resourceRemains,:resourceType,:resourceUnitPrice)";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":resourceAmount",OracleDbType.Int32,11),
                new OracleParameter(":resourceDescription",OracleDbType.Varchar2,255),
                new OracleParameter(":resourceName",OracleDbType.Varchar2,64),
                new OracleParameter(":resourceRemains",OracleDbType.Int32,11),
                new OracleParameter(":resourceType",OracleDbType.Int32,11),
                new OracleParameter(":resourceUnitPrice",OracleDbType.Int32,11)
            };
            parameters[0].Value = res.Resource_amount;
            parameters[1].Value = res.Resource_description;
            parameters[2].Value = res.Resource_name;
            parameters[3].Value = res.Resource_remains;
            parameters[4].Value = findresourceTypeIdByName(res.Resource_type);
            parameters[5].Value = res.Resource_unit_price;
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

        #region 获取总资源
        public List<MODEL.risk_resource> getAllRes()
        {
            string sql = "select * from RISK_RESOURCE";
            OracleParameter parameter = null;
            try
            {
                using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, parameter))
                {
                    List<MODEL.risk_resource> ResList = new List<MODEL.risk_resource>();
                    while (myReader.Read())
                    {
                        MODEL.risk_resource Res = new MODEL.risk_resource();
                        Res.Auto_id = Int32.Parse(myReader["Auto_id"].ToString());
                        Res.Resource_name = myReader["Resource_name"].ToString();
                        Res.Resource_description = myReader["Resource_description"].ToString();
                        Res.Resource_amount = Int32.Parse(myReader["Resource_amount"].ToString());
                        Res.Resource_remains = Int32.Parse(myReader["Resource_remains"].ToString());
                        Res.Resource_type = findresourceTypeNameById(Int32.Parse(myReader["Resource_type"].ToString()));
                        Res.Resource_unit_price = Int32.Parse(myReader["Resource_unit_price"].ToString());
        
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
        public void deleteByName(String Name)
        {
            string sql = "delete risk_resource where RESOURCE_NAME=:Name";

            OracleParameter[] parameters = new OracleParameter[] {
   
                new OracleParameter(":Name",OracleDbType.Varchar2,64)
            };
            parameters[0].Value = Name;
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

        #region 更新

        public void updateById(MODEL.risk_resource res)
        {
            string sql = "update risk_resource set resource_amount=:resourceAmount,resource_description=:resourceDescription,resource_name=:resourceName,resource_remains=:resourceRemains,resource_type=:resourceType,resource_unit_price=:resourceUnitPrice where auto_id=:autoId";

            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":resourceAmount",OracleDbType.Int32,11),
                new OracleParameter(":resourceDescription",OracleDbType.Varchar2,255),
                new OracleParameter(":resourceName",OracleDbType.Varchar2,64),
                new OracleParameter(":resourceRemains",OracleDbType.Int32,11),
                new OracleParameter(":resourceType",OracleDbType.Int32,11),
                new OracleParameter(":resourceUnitPrice",OracleDbType.Int32,11),
                new OracleParameter(":autoId",OracleDbType.Int32,11)
            };
            parameters[0].Value = res.Resource_amount;
            parameters[1].Value = res.Resource_description;
            parameters[2].Value = res.Resource_name;
            parameters[3].Value = res.Resource_remains;
            parameters[4].Value = findresourceTypeIdByName(res.Resource_type);
            parameters[5].Value = res.Resource_unit_price;
            parameters[6].Value = res.Auto_id;
            try
            {
                SqlHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void updateByName(MODEL.risk_resource res)
        {
            string sql = "update risk_resource set resource_amount=:resourceAmount,resource_description=:resourceDescription,resource_remains=:resourceRemains,resource_type=:resourceType,resource_unit_price=:resourceUnitPrice where resource_name=:resourceName";

            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":resourceAmount",OracleDbType.Int32,11),
                new OracleParameter(":resourceDescription",OracleDbType.Varchar2,255),
                new OracleParameter(":resourceRemains",OracleDbType.Int32,11),
                new OracleParameter(":resourceType",OracleDbType.Int32,11),
                new OracleParameter(":resourceUnitPrice",OracleDbType.Int32,11),
                new OracleParameter(":resourceName",OracleDbType.Varchar2,64)
            };
            parameters[0].Value = res.Resource_amount;
            parameters[1].Value = res.Resource_description;
            parameters[2].Value = res.Resource_remains;
            parameters[3].Value = findresourceTypeIdByName(res.Resource_type);
            parameters[4].Value = res.Resource_unit_price;
            parameters[5].Value = res.Resource_name;
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
        public static int findresourceTypeIdByName(string typeName)
        {
            int ans = -1;
            string sql = "select AUTO_ID from risk_resource_type where RESOURCE_TYPE_NAME=:typeName";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":typeName",OracleDbType.Varchar2,64)
            };
            parameters[0].Value = typeName;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        ans = Convert.ToInt32(reader["AUTO_ID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ans;
        }
        public static  string findresourceTypeNameById(int autoId)
        {
            string ans = string.Empty;
            string sql = "select RESOURCE_TYPE_NAME from risk_resource_type where AUTO_ID=:autoId";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":autoId",OracleDbType.Int32,11)
            };
            parameters[0].Value = autoId;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        ans = reader["RESOURCE_TYPE_NAME"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ans;
        }
        public MODEL.risk_resource findResByName(string name)
        {
            MODEL.risk_resource res = new MODEL.risk_resource();
            string sql = "select * from risk_resource where resource_name=:resourceName";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":resourceName",OracleDbType.Varchar2,64)
            };
            parameters[0].Value = name;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        res.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        res.Resource_amount = Convert.ToInt32(reader["resource_amount"]);
                        res.Resource_description = reader["resource_description"].ToString();
                        res.Resource_name = reader["resource_name"].ToString();
                        res.Resource_remains = Convert.ToInt32(reader["resource_remains"]);
                        int resourceTypeId = Convert.ToInt32(reader["resource_type"]);
                        res.Resource_type = findresourceTypeNameById(resourceTypeId);
                        res.Resource_unit_price = Convert.ToDouble(reader["resource_unit_price"]);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  MODEL.risk_resource findResById(int resource_id)
        {
            MODEL.risk_resource res = new MODEL.risk_resource();
            string sql = "select * from risk_resource where auto_id =:resource_id";
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter(":resource_id",OracleDbType.Int32,11)
            };
            parameters[0].Value = resource_id;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                {
                    if (reader.Read())
                    {
                        res.Auto_id = Convert.ToInt32(reader["auto_id"]);
                        res.Resource_amount = Convert.ToInt32(reader["resource_amount"]);
                        res.Resource_description = reader["resource_description"].ToString();
                        res.Resource_name = reader["resource_name"].ToString();
                        res.Resource_remains = Convert.ToInt32(reader["resource_remains"]);
                        int resourceTypeId = Convert.ToInt32(reader["resource_type"]);
                        res.Resource_type = findresourceTypeNameById(resourceTypeId);
                        res.Resource_unit_price = Convert.ToDouble(reader["resource_unit_price"]);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
