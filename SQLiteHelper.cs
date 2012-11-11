using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Data.SqlClient;

namespace Weiddler
{

    /// <summary>
    /// 将常用的数据操作以高性能、可扩展方式进行封装，适用于 Microsoft SQLServer 2000 及以上版本。
    /// </summary>
    public sealed class SQLiteHelper
    {

        //public static readonly string CONN_STRING_NON_DTC = ConfigHelper.ConnectionString;
        // 仅提供静态方法，因此设置私有构造函数以避免在外部被实例化。
        private SQLiteHelper()
        {
        }
        public SQLiteHelper(string connectionString)
        {
            defaultConnectionString = connectionString;
        }

        #region 简化方法
        /// <summary>
        /// 使用默认连接串  用ConnectionString节里面的 LocalMySqlServer
        /// </summary>
        public static string defaultConnectionString;

        public static void SetConnectionString(string connectionString)
        {
            defaultConnectionString = connectionString;
        }

        /// 使用默认连接串 执行sql返回一个MySQLiteDataReader
        /// </summary>
        /// <param name="commandText">sql文本</param>
        /// <returns>MySQLiteDataReader对象</returns>
        public static SQLiteDataReader ExecuteReader(string commandText)
        {
            using (SQLiteConnection conn = new SQLiteConnection(defaultConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;
                return cmd.ExecuteReader();
            }
        }
        /// <summary>
        /// 使用默认连接串 执行sql返回影响到的行
        /// </summary>
        /// <param name="commandText">sql文本</param>
        /// <returns>影响到的行</returns>
        public static int ExecuteNonQuery(string commandText)
        {
            using (SQLiteConnection conn = new SQLiteConnection(defaultConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;
                return cmd.ExecuteNonQuery();
            }
        }

        public static DataSet ExecuteDataSet(string commandText)
        {
            using (SQLiteConnection conn = new SQLiteConnection(defaultConnectionString))
            {
                conn.Open();
                DataSet ds = new DataSet();
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;
                SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd);
                da.Fill(ds);
                return ds;

            }
        }

        #endregion
















    }


}
