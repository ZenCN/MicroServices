using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace DbAccess
{
    public class Db
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connection_str;

        private ConnectionType connection_type;

        protected SqlConnection sql_conn;

        protected OracleConnection orcl_conn;



        /// <summary>
        /// Db构造方法
        /// </summary>
        /// <param name="connection_str">数据库连接字符串</param>
        public Db(string connection_str, ConnectionType connection_type)
        {
            this.connection_str = connection_str;
            this.connection_type = connection_type;
        }

        public int CloseConnection() {
            int msg = 0;
            switch (connection_type) {
                case ConnectionType.Oracle:
                    if (orcl_conn != null && orcl_conn.State == System.Data.ConnectionState.Open)
                    {
                        orcl_conn.Close();
                        msg = 1;
                    }
                    break;
                default:
                    if (sql_conn != null && sql_conn.State == System.Data.ConnectionState.Open)
                    {
                        sql_conn.Close();
                        msg = 1;
                    }
                    break;
            }

            return msg;
        }

        public OracleDataReader GetDataReader(string sql)
        {
            OracleDataReader reader = null;
            try {
                orcl_conn = new OracleConnection(this.connection_str);
                OracleCommand cmd = new OracleCommand(sql, orcl_conn);
                orcl_conn.Open();
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                orcl_conn.Close();
                throw ex;
            }

            return reader;
        }

        public SqlDataReader GetDataReader(string sql, bool isSql = true)
        {
            SqlDataReader reader = null;
            try
            {
                sql_conn = new SqlConnection(this.connection_str);
                SqlCommand cmd = new SqlCommand(sql, sql_conn);
                sql_conn.Open();
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                sql_conn.Close();
                throw ex;
            }

            return reader;
        }

        public int Command(string sql) {
            SqlConnection conn = new SqlConnection(connection_str);
            try
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                return command.ExecuteNonQuery();
            }
            catch
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
    }

    public enum ConnectionType {
        Sql,
        Oracle,
        MySql
    }
}
