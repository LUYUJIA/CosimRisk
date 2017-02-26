using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Data;
namespace DAL
{
        public class connDB
        {
                private static string connectString = ConfigurationManager.ConnectionStrings["connectOracle"].ToString();

                private static OracleConnection conn = null;              
                
                public static OracleConnection getConn()
                {
                        return conn = new OracleConnection(connectString); 
                }
                
                public static string GetConnectionString()
                {
                        return connectString;
                }

                public static OracleDataReader getReader(string sqlString, params OracleParameter[] SqlParams)   //返回DataReader对象时要注意关闭数据库。。。。。。。。。。。。
                {
                        OracleConnection conn = getConn();
                        conn.Open();
                        OracleCommand cmd = new OracleCommand(sqlString, conn);
                        OracleDataReader reader = null;
                        if (SqlParams != null)
                        {
                                foreach (OracleParameter p in SqlParams)
                                {
                                        cmd.Parameters.Add(p);
                                }
                                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        }
                        return reader;

                }


        }
}
