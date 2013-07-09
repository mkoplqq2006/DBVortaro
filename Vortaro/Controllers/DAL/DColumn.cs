using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Criterion;
using VortaroModel;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 列字段信息
    /// </summary>
    public class DColumn : AbstractDAL<Column>
    {
        /// <summary>
        /// 分页得到列字段信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="tablesCode">数据库编码</param>
        /// <returns></returns>
        public static string GetPageColumn(int start, int pageSize, string query, string tablesCode)
        {
            //获得当前运行的NHibernate实例
            ISession session = NHibernateHelper.GetCurrentSession();
            //事务开始
            using (ITransaction transaction = session.BeginTransaction())
            {
                IList<Column> list = null;//分页的记录
                int count = 0;//总的记录条数
                try
                {
                    ICriteria criteria = session.CreateCriteria<Column>();
                    if (tablesCode == "")
                    {
                        count = 0;
                        list = null;
                    }
                    else
                    {
                        criteria.Add(Expression.Eq("TablesCode", new Guid(tablesCode)));
                        if (!String.IsNullOrEmpty(query))
                        {
                            criteria.Add(Expression.Or(Expression.Like("Name", "%" + query + "%"), Expression.Like("Alias", "%" + query + "%")));
                        }
                        count = criteria.List<Column>().Count;
                        list = criteria.SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Desc("Id")).List<Column>();
                    }
                    transaction.Commit();//提交事务
                }
                catch (Exception ex) 
                {
                    NHibernateHelper.WriteErrorLog("分页得到列字段信息", ex);
                    transaction.Rollback();//回滚事务
                }
                finally
                {
                    session.Close();
                }
                Hashtable hasTable = new Hashtable();
                hasTable.Add("total", count);
                hasTable.Add("rows", list);
                return JsonHelper.ToJson(hasTable);
            }
        }
        /// <summary>
        /// 判断列名是否重复
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="TablesCode">表编码</param>
        /// <param name="Author">作者</param>
        /// <returns>列名是否重复</returns>
        public static bool RepeatColumnName(string ColumnName, Guid TablesCode, string Author)
        {
            ISession session = null;
            try
            {
                //获得当前运行的NHibernate实例
                session = NHibernateHelper.GetCurrentSession();
                //事务开始
                ITransaction transaction = session.BeginTransaction();
                ICriteria criteria = session.CreateCriteria<Column>();
                if (!String.IsNullOrEmpty(ColumnName))
                {
                    criteria.Add(Expression.Eq("Name", ColumnName));
                }
                if (!TablesCode.Equals(Guid.Empty))
                {
                    criteria.Add(Expression.Eq("TablesCode", TablesCode));
                }
                if (!String.IsNullOrEmpty(Author))
                {
                    criteria.Add(Expression.Eq("Author", Author));
                }
                int count = criteria.List<Column>().Count;
                //提交事务
                transaction.Commit();
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("判断列名是否重复", ex);
                throw;
            }
            finally
            {
                session.Close();
            }
        }
        /// <summary>
        /// 根据表名称，获取列字段信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connection">连接字符</param>
        /// <returns></returns>
        public static DataTable GetTableColumn(string tableName, string connection)
        {
            string Sql = string.Format(@"select A.ORDINAL_POSITION as ordinalPosition,A.TABLE_SCHEMA as [owner],B.Name as columnsName,A.DATA_TYPE as columnType,A.CHARACTER_MAXIMUM_LENGTH as typeLength,
            A.column_default as defaultsetting,A.is_nullable as isnullable,C.value as remark 
            from information_schema.columns A
            left join sys.extended_properties C on A.ORDINAL_POSITION=C.minor_id and C.major_id=object_id('{0}')
            left join sys.columns B on B.column_id=A.ORDINAL_POSITION and B.object_id=object_id('{0}')
            where B.Name is not null and A.table_name='{0}' order by ordinalPosition desc", tableName);
            return SQLHelper.GetDataTable(connection, Sql);
        }
        /// <summary>
        /// 根据表编码，获取列字段
        /// </summary>
        /// <param name="tablesCode">表编码</param>
        /// <returns></returns>
        public static IList<Column> GetColumn(Guid tablesCode)
        {
            ISession session = null;
            try
            {
                //获得当前运行的NHibernate实例
                session = NHibernateHelper.GetCurrentSession();
                //事务开始
                ITransaction transaction = session.BeginTransaction();
                ICriteria criteria = session.CreateCriteria<Column>();
                criteria.Add(Expression.Eq("TablesCode", tablesCode));
                IList<Column> list = criteria.AddOrder(Order.Desc("Id")).List<Column>();
                int count = criteria.List<Column>().Count;
                //提交事务
                transaction.Commit();
                return list;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("根据表编码，获取列字段", ex);
                throw;
            }
            finally
            {
                session.Close();
            }
        }
    }
}