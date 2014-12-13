using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MYBBS
{
    public static class  SqlHelper
    {
        public static readonly string connstr =
            ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }

        public static int ExecuteNonQuery(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);
            }
        }

        public static object ExecuteScalar(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteScalar(conn, cmdText, parameters);
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteDataTable(conn, cmdText, parameters);
            }
        }

        public static int ExecuteNonQuery(SqlConnection conn,string cmdText,
           params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable ExecuteDataTable(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static object ToDBValue(this object value)
        {
            return value == null ? DBNull.Value : value;
        }

        public static object FromDBValue(this object dbValue)
        {
            return dbValue == DBNull.Value ? null : dbValue;
        }
        
        public static int[] GetCount(string action, string cls)
        {
            int count;         //总条数
            int Mpage;         //总共多少页
            string sqlconn = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            SqlConnection sqlconnection = new SqlConnection(sqlconn);
            string strcommand = "select * into #TEMP from  Table_3 where " + cls + " like '" + action + "' and Donate <> '1';select count(*) from #TEMP";
            SqlCommand comm = new SqlCommand(strcommand, sqlconnection);
            sqlconnection.Open();
            count = int.Parse(comm.ExecuteScalar().ToString());
            sqlconnection.Close();
            //总共有多少页 
            if (count % 40 == 0)
            {
                Mpage = count / 40;
            }
            else
            {
                Mpage = count / 40 + 1;
            }
            int[] temp = new int[2] { Mpage, count };
            return temp;
        }
    }
}
