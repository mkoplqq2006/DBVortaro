using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// SQL处理
    /// </summary>
    public abstract class SQLHelper
    {
        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sqlConnection">数据库连接字符</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sqlConnection, string sql)
        {
            if (string.IsNullOrEmpty(sqlConnection))
            {
                sqlConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                DataTable dt = new DataTable();
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);
                    dataAdapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    NHibernateHelper.WriteErrorLog("SQLHelper获取DataTable异常", ex);
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
                return dt;
            }
        }

        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sqlConnection">数据库连接字符</param>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static int ExecuteSql(string sqlConnection, string sql)
        {
            if (string.IsNullOrEmpty(sqlConnection))
            {
                sqlConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                int rows = 0;
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    connection.Open();
                    rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    NHibernateHelper.WriteErrorLog("SQLHelper增删改异常", ex);
                }
                finally
                {
                    connection.Close();
                }
                return rows;
            }
        }
    }
}