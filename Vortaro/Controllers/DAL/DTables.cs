using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using VortaroModel;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 表字段信息
    /// </summary>
    public class DTables : AbstractDAL<Tables>
    {
        /// <summary>
        /// 分页得到表字段信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">查询条件</param>
        /// <param name="databaseCode">数据库编码</param>
        /// <param name="groupCode">分组编码</param>
        /// <returns></returns>
        public static string GetPageTables(int start, int pageSize, string query, string databaseCode, string groupCode)
        {
            //获得当前运行的NHibernate实例
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                //事务开始
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<Tables> list = null;//分页的记录
                    int count = 0;//总的记录条数
                    try
                    {
                        ICriteria criteria = session.CreateCriteria<Tables>();
                        if (databaseCode == "" || groupCode == "")
                        {
                            count = 0;
                            list = null;
                        }
                        else
                        {
                            criteria.Add(Expression.Eq("DatabaseCode", new Guid(databaseCode)));
                            criteria.Add(Expression.Eq("GroupCode", new Guid(groupCode)));
                            if (!String.IsNullOrEmpty(query))
                            {
                                criteria.Add(Expression.Or(Expression.Like("Name", "%" + query + "%"), Expression.Like("Alias", "%" + query + "%")));
                            }
                            count = criteria.SetCacheable(true).List<Tables>().Count;
                            list = criteria.SetCacheable(true).SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Desc("Id")).List<Tables>();
                        }
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        NHibernateHelper.WriteErrorLog("分页得到表字段信息", ex);
                        throw;
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
        }
        /// <summary>
        /// 判断表名是否重复
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="DatabaseCode">数据库编码</param>
        /// <param name="GroupCode">功能分组编码</param>
        /// <param name="Author">作者</param>
        /// <returns>表名是否重复</returns>
        public static bool RepeatTablesName(string TableName, string DatabaseCode, string GroupCode, string Author)
        {
            try
            {
                //获得当前运行的NHibernate实例
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    //事务开始
                    ITransaction transaction = session.BeginTransaction();
                    ICriteria criteria = session.CreateCriteria<Tables>();
                    if (!String.IsNullOrEmpty(TableName))
                    {
                        criteria.Add(Expression.Eq("Name", TableName));
                    }
                    if (!String.IsNullOrEmpty(DatabaseCode))
                    {
                        criteria.Add(Expression.Eq("DatabaseCode", new Guid(DatabaseCode)));
                    }
                    if (!String.IsNullOrEmpty(GroupCode))
                    {
                        criteria.Add(Expression.Eq("GroupCode", new Guid(GroupCode)));
                    }
                    if (!String.IsNullOrEmpty(Author))
                    {
                        criteria.Add(Expression.Eq("Author", Author));
                    }
                    int count = criteria.List<Tables>().Count;
                    //提交事务
                    transaction.Commit();
                    return count > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("判断表名是否重复", ex);
                throw;
            }
        }
        /// <summary>
        /// 根据数据库编码，获取表
        /// </summary>
        /// <param name="databaseCode">数据库编码</param>
        /// <returns></returns>
        public static IList<Tables> GetTables(Guid databaseCode)
        {
            try
            {
                //获得当前运行的NHibernate实例
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    //事务开始
                    ITransaction transaction = session.BeginTransaction();
                    ICriteria criteria = session.CreateCriteria<Tables>();
                    criteria.Add(Expression.Eq("DatabaseCode", databaseCode));
                    IList<Tables> list = criteria.AddOrder(Order.Desc("GroupCode")).List<Tables>();
                    //提交事务
                    transaction.Commit();
                    return list;
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("根据数据库编码，获取表", ex);
                throw;
            }
        }
        /// <summary>
        /// 根据数据库编码，获取表名称
        /// </summary>
        /// <param name="databaseCode">数据库编码</param>
        /// <returns></returns>
        public static string GetTablesName(Guid databaseCode)
        {
            try
            {
                //获得当前运行的NHibernate实例
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    //事务开始
                    ITransaction transaction = session.BeginTransaction();
                    ICriteria criteria = session.CreateCriteria<Tables>();
                    criteria.Add(Expression.Eq("DatabaseCode", databaseCode));
                    IList<Tables> list = criteria.AddOrder(Order.Desc("Id")).List<Tables>();
                    //提交事务
                    transaction.Commit();
                    if (list.Count > 0)
                    {
                        string TablesName = string.Empty;
                        foreach (Tables tbs in list)
                        {
                            TablesName += "'" + tbs.Name + "',";
                        }
                        return TablesName.Substring(0, TablesName.Length - 1);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("根据数据库编码，获取表名称", ex);
                throw;
            }
        }
    }
}