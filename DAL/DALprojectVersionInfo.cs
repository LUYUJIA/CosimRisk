using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace DAL
{
        public class DALprojectVersionInfo
        {
                #region  add
                public int savePrjVersionInfo(MODEL.risk_prj_version_info projectVersionInfo)
                {
                    string sql = "insert into risk_prj_version_info(sim_version_id,pri_id,description,count,DURATION_MAX,SIM_STARTTIME,SIM_ENDTIME,HAVE_RESOURCE) values(RISK_PRJ_VERSION_INFO_AUTOID.nextval,:priId,:myDescription,:myCount,0,:simStartTime,:simEndTime,:haveResource)";
                        OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":priId",OracleDbType.Int32,11),
                                         new OracleParameter(":myDescription",OracleDbType.Varchar2,255),
                                         new OracleParameter(":myCount",OracleDbType.Int32,11),
                                         new OracleParameter(":simStartTime",OracleDbType.Date,7),
                                         new OracleParameter(":simEndTime",OracleDbType.Date,7),
                                         new OracleParameter(":haveResource",OracleDbType.Int32,11),
                         };
                        parameters[0].Value = projectVersionInfo.Pri_id;
                        parameters[1].Value = projectVersionInfo.Desciption;
                        parameters[2].Value = projectVersionInfo.Count;
                        parameters[3].Value = projectVersionInfo.Sim_starttime;
                        parameters[4].Value = projectVersionInfo.Sim_endtime;
                        parameters[5].Value = projectVersionInfo.Have_resource;
                        try
                        {
                                SqlHelper.ExecuteNonQuery(sql, parameters);
                                sql = "select RISK_PRJ_VERSION_INFO_AUTOID.currval from dual";
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
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }


                }
                #endregion

                #region delete
                public bool deletePrjVersionInfo(int sim_version_id)
                {
                        string sql = "delete from risk_prj_version_info where sim_version_id=:simVersionId";
                        OracleParameter[] parameter = new OracleParameter[]{
                                new OracleParameter(":simVersionId",OracleDbType.Int32,11)
                        };
                        parameter[0].Value = sim_version_id;
                        try
                        {
                                SqlHelper.ExecuteNonQuery(sql, parameter);
                        } catch (System.Exception ex)
                        {
                                throw ex;
                        } 
                        return false;
                }
                #endregion

                #region get
                public MODEL.risk_prj_version_info getPrjVersionInfo(int sim_version_id)
                {
                        MODEL.risk_prj_version_info projectVersionInfo = new MODEL.risk_prj_version_info();
                        projectVersionInfo.Sim_version_id = sim_version_id;
                        string sql = "select PRI_ID,DESCRIPTION,COUNT,DURATION_MAX,SIM_STARTTIME,SIM_ENDTIME,HAVE_RESOURCE from risk_prj_version_info where sim_version_id=:simVersionId";
                        OracleParameter[] parameters = new OracleParameter[]{
                                new OracleParameter(":simVersionId",OracleDbType.Int32,11)
                        };
                        parameters[0].Value = sim_version_id;
                        try
                        {
                                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                                {
                                        if (reader.Read())
                                        {
                                                projectVersionInfo.Pri_id = Int32.Parse(reader["PRI_ID"].ToString());
                                                projectVersionInfo.Desciption = reader["DESCRIPTION"].ToString();
                                                projectVersionInfo.Count = Int32.Parse(reader["COUNT"].ToString());
                                                projectVersionInfo.Duration_max = Int32.Parse(reader["DURATION_MAX"].ToString());
                                                projectVersionInfo.Sim_starttime = reader.GetDateTime(4);
                                                projectVersionInfo.Sim_endtime = reader.GetDateTime(5);
                                                projectVersionInfo.Duration_max = Int32.Parse(reader["HAVE_RESOURCE"].ToString());
                                                return projectVersionInfo;
                                        }
                                        else return null;
                                }

                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }

                }

                public List<MODEL.risk_prj_version_info> getAll()
                {
                    string sql = "select PRI_ID,DESCRIPTION,COUNT,DURATION_MAX,SIM_STARTTIME,SIM_ENDTIME,HAVE_RESOURCE,SIM_VERSION_ID from risk_prj_version_info";
                        OracleParameter parameters = null;
                        try
                        {
                                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameters))
                                {
                                        List<MODEL.risk_prj_version_info> projectVersionInfoList = new List<MODEL.risk_prj_version_info>();
                                        while (reader.Read())
                                        {
                                                MODEL.risk_prj_version_info projectVersionInfo = new MODEL.risk_prj_version_info();
                                                projectVersionInfo.Pri_id = Int32.Parse(reader["PRI_ID"].ToString());
                                                projectVersionInfo.Desciption = reader["DESCRIPTION"].ToString();
                                                projectVersionInfo.Count = Int32.Parse(reader["COUNT"].ToString());
                                                projectVersionInfo.Sim_version_id = Int32.Parse(reader["SIM_VERSION_ID"].ToString());
                                                projectVersionInfo.Duration_max = Int32.Parse(reader["DURATION_MAX"].ToString());
                                                projectVersionInfo.Sim_starttime = reader.GetDateTime(4);
                                                projectVersionInfo.Sim_endtime = reader.GetDateTime(5);
                                                projectVersionInfo.Have_resource = Int32.Parse(reader["HAVE_RESOURCE"].ToString());
                                                sql = "select prj_name from risk_project where prj_id=:prjId";
                                                OracleParameter parameter = new OracleParameter(":prjId", OracleDbType.Int32, 11);
                                                parameter.Value = projectVersionInfo.Pri_id;
                                                OracleDataReader prjIdReader = SqlHelper.ExecuteQuery(sql, parameter);
                                                if (prjIdReader.Read())
                                                {
                                                        projectVersionInfo.ProjectName = prjIdReader.GetString(0);
                                                }
                                                else
                                                {
                                                        projectVersionInfo.ProjectName = "unable to get name";
                                                }
                                                prjIdReader.Close();
                                                projectVersionInfoList.Add(projectVersionInfo);
                                        }
                                        return projectVersionInfoList;
                                }

                        } catch (System.Exception ex)
                        {
                                throw ex;
                        }

                }
                #endregion

                #region update
                public bool updatePrjVersionInfo(MODEL.risk_prj_version_info projectVersionInfo)
                {
                    string sql = "update risk_prj_version_info set SIM_ENDTIME=:simEndTime where SIM_VERSION_ID=:simVersionId";
                        OracleParameter[] parameters = new OracleParameter[]{
                               
                                new OracleParameter(":simEndTime",OracleDbType.Date,7),
                                new OracleParameter(":simVersionId",OracleDbType.Int32,11)
                  
                        };
          
                        parameters[0].Value = projectVersionInfo.Sim_endtime;
                        parameters[1].Value = projectVersionInfo.Sim_version_id;
          
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
