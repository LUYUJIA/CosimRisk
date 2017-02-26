using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;
using System.Runtime.InteropServices;
namespace DAL
{
    internal class SqlHelper
    {


        public static int ExecuteNonQuery(string commandText, OracleParameter[] commandParameters)
        {
            try
            {
                using (OracleConnection aConnection = connDB.getConn())
                {
                    if (aConnection.State == System.Data.ConnectionState.Closed)
                    {
                        aConnection.Open();
                    }
                    OracleCommand aCommand = aConnection.CreateCommand();
                    aCommand.Connection = aConnection;
                    aCommand.CommandText = commandText;
                    aCommand.CommandTimeout = 60;
                    if (commandParameters != null)
                    {
                        foreach (OracleParameter p in commandParameters)
                        {
                            aCommand.Parameters.Add(p);
                        }
                    }
                    int i = aCommand.ExecuteNonQuery();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OracleDataReader ExecuteQuery(string commandText, OracleParameter[] commandParameters)
        {
            try
            {
                OracleConnection aConnection = connDB.getConn();
                if (aConnection.State == System.Data.ConnectionState.Closed)
                {
                    aConnection.Open();
                }
                OracleCommand aCommand = aConnection.CreateCommand();
                aCommand.Connection = aConnection;
                aCommand.CommandText = commandText;
                aCommand.CommandTimeout = 60;
                if (commandParameters != null)
                {
                    foreach (OracleParameter p in commandParameters)
                    {
                        aCommand.Parameters.Add(p);
                    }
                }
                return aCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OracleDataReader ExecuteQuery(string commandText, OracleParameter commandParameters)
        {
            try
            {
                OracleConnection aConnection = connDB.getConn();
                if (aConnection.State == System.Data.ConnectionState.Closed)
                {
                    aConnection.Open();
                }
                OracleCommand aCommand = aConnection.CreateCommand();
                aCommand.Connection = aConnection;
                aCommand.CommandText = commandText;
                aCommand.CommandTimeout = 60;
                if (commandParameters != null)
                {
                    aCommand.Parameters.Add(commandParameters);
                }
                return aCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static void closeConn()
        //{
        //        if (connDB.getConn().State == System.Data.ConnectionState.Open)
        //        {
        //                connDB.getConn().Close();
        //        }
        //}
    }
}
