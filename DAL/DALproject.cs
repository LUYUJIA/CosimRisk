using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace DAL
{
    public class DALproject
    {
        protected OracleConnection connection = null;

        #region 检索project的名字是否重复
        public bool checkProjectName(string pName)
        {
            string sql = "select prj_id from risk_project where prj_name=:name";
            //  string sql = "SELECT PRJ_ID FROM RISK_PROJECT WHERE PRJ_NAME = :name";
            OracleParameter parameter = new OracleParameter(":name", OracleDbType.Varchar2, 64);
            parameter.Value = pName;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameter))
                {
                    if (reader.Read())
                    {
                        int id = Int32.Parse(reader["prj_id"].ToString());
                        return false;
                    }
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 保存project
        public int saveProject(MODEL.risk_project project)
        {
            //connection = connDB.getConn();
            //connection.Open();
            //OracleCommand command = connection.CreateCommand();
            //command.CommandText = @"insert into RISK_PROJECT(PRJ_ID,PRJ_NAME,PRJ_DESCRIBE,PRJ_DATE,PRJ_XML) values(RISK_PROJECT_PRJ_ID.NEXTVAL,:PRJ_NAME,:PRJ_DESCRIBE,:PRJ_DATE,to_blob(:PRJ_XML))";
            //command.CommandTimeout = 60;
            //OracleParameter[] parameters = new OracleParameter[] {
            //                 new OracleParameter(":PRJ_NAME",OracleDbType.Varchar2,64),
            //                 new OracleParameter(":PRJ_DESCRIBE",OracleDbType.Varchar2,255),
            //                 new OracleParameter(":PRJ_DATE",OracleDbType.Date,7),                                        
            //                 new OracleParameter(":PRJ_XML",OracleDbType.Blob,4000)
            //        };
            //byte[] byteXML = Encoding.Default.GetBytes(project.Prj_xml);
            //parameters[0].Value = project.Prj_name;
            //parameters[1].Value = project.Prj_describe;
            //parameters[2].Value = project.Prj_date;
            //parameters[3].Value = byteXML;

            //if (parameters != null)
            //{
            //        foreach (OracleParameter p in parameters)
            //        {
            //                command.Parameters.Add(p);
            //        }
            //        try
            //        {
            //                command.ExecuteNonQuery();
            //                command.CommandText = "select RISK_PROJECT_PRJ_ID.currval from dual ";
            //                OracleDataReader reader = command.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                        int id = Int32.Parse(reader.GetValue(0).ToString());
            //                        return id;
            //                }
            //        } catch (System.Exception ex)
            //        {
            //                throw ex;
            //        } finally
            //        {
            //                connDB.closeConn();
            //        }
            //}
            //return -1;
            string sql = @"insert into RISK_PROJECT(PRJ_ID,PRJ_NAME,PRJ_DESCRIBE,PRJ_DATE,PRJ_XML) values(RISK_PROJECT_PRJ_ID.NEXTVAL,:PRJ_NAME,:PRJ_DESCRIBE,:PRJ_DATE,to_blob(:PRJ_XML))";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":PRJ_NAME",OracleDbType.Varchar2,64),
                                         new OracleParameter(":PRJ_DESCRIBE",OracleDbType.Varchar2,255),
                                         new OracleParameter(":PRJ_DATE",OracleDbType.Date,7),                                        
                                         new OracleParameter(":PRJ_XML",OracleDbType.Blob,4000)
                                };
            byte[] byteXML = Encoding.Default.GetBytes(project.Prj_xml);
            parameters[0].Value = project.Prj_name;
            parameters[1].Value = project.Prj_describe;
            parameters[2].Value = project.Prj_date;
            parameters[3].Value = byteXML;

            if (parameters != null)
            {

                try
                {
                    SqlHelper.ExecuteNonQuery(sql, parameters);
                    sql = "select RISK_PROJECT_PRJ_ID.currval from dual ";
                    OracleParameter parameternull = null;
                    using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameternull))
                    {
                        while (reader.Read())
                        {
                            int id = Int32.Parse(reader.GetValue(0).ToString());
                            return id;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            return -1;
        }
        #endregion

        #region 获取projectId
        public int getProjectId(string pName)
        {
            string sql = "select prj_id from risk_project where prj_name=:name";
            OracleParameter parameter = new OracleParameter(":name", OracleDbType.Varchar2, 64);
            parameter.Value = pName;
            try
            {
                using (OracleDataReader reader = SqlHelper.ExecuteQuery(sql, parameter))
                {
                    if (reader.Read())
                    {
                        int id = Int32.Parse(reader["prj_id"].ToString());
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

        #region 获取project
        public List<MODEL.risk_project> getProjectList()
        {
            string sql = @"select * from RISK_PROJECT";
            OracleParameter parameter = null;
            try
            {
                using (OracleDataReader myReader = SqlHelper.ExecuteQuery(sql, parameter))
                {
                    List<MODEL.risk_project> PrjList = new List<MODEL.risk_project>();
                    while (myReader.Read())
                    {
                        MODEL.risk_project project = new MODEL.risk_project();
                        project.Prj_id = Int32.Parse(myReader["PRJ_ID"].ToString());
                        project.Prj_name = myReader["PRJ_NAME"].ToString();
                        project.Prj_describe = myReader["PRJ_DESCRIBE"].ToString();
                        project.Prj_date = myReader.GetDateTime(3);
                        OracleBlob PrjXML = myReader.GetOracleBlob(4);
                        project.Prj_xml = Encoding.Default.GetString(PrjXML.Value);
                        PrjList.Add(project);
                    }
                    return PrjList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


        }
        #endregion

        #region 删除project
        public bool deleteProject(List<MODEL.risk_project> projectList)
        {
            string sql = "";
            foreach (MODEL.risk_project project in projectList)
            {

                int prjId = getProjectId(project.Prj_name);
                //delete tasks                          
                sql = @"delete from RISK_TASK where TASK_PROJECT_ID='" + prjId + "'";
                SqlHelper.ExecuteNonQuery(sql, null);
                //delete links
                sql = @"delete from RISK_LINK where PRJ_ID='" + prjId + "'";
                SqlHelper.ExecuteNonQuery(sql, null);
                //delete projects
                sql = @"delete from RISK_PROJECT where PRJ_ID='" + prjId + "'";
                SqlHelper.ExecuteNonQuery(sql, null);
            }
            return true;
        }
        public bool deleteProject(MODEL.risk_project project)
        {
            return true;
        }
        #endregion

        #region 更新project
        public bool updateProject(MODEL.risk_project project)
        {

            string sql = @"update RISK_PROJECT set PRJ_DESCRIBE=:PRJ_DESCRIBE where PRJ_NAME=:prjName";
            OracleParameter[] parameters = new OracleParameter[] {
                                         new OracleParameter(":PRJ_DESCRIBE",OracleDbType.Varchar2,255),
                                         new OracleParameter(":prjName",OracleDbType.Varchar2,64),
                        };
            parameters[0].Value = project.Prj_describe;
            parameters[1].Value = project.Prj_name;
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
    }
}