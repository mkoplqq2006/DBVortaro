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
    /// 数据库信息
    /// </summary>
    public class DDatabase : AbstractDAL<Database>
    {
        /// <summary>
        /// 分页获取数据库信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public static string GetPageDatabase(int start, int pageSize, string query, string projectCode)
        {
            //获得当前运行的NHibernate实例
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                //事务开始
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<Database> list = null;//分页的记录
                    int count = 0;//总的记录条数
                    try
                    {
                        ICriteria criteria = session.CreateCriteria<Database>();
                        if (projectCode == "")
                        {
                            count = 0;
                            list = null;
                        }
                        else
                        {
                            criteria.Add(Expression.Eq("ProjectCode", new Guid(projectCode)));
                            if (!String.IsNullOrEmpty(query))
                            {
                                criteria.Add(Expression.Or(Expression.Like("Name", "%" + query + "%"), Expression.Like("Alias", "%" + query + "%")));
                            }
                            count = criteria.SetCacheable(true).List<Database>().Count;
                            list = criteria.SetCacheable(true).SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Asc("Id")).List<Database>();
                        }
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        NHibernateHelper.WriteErrorLog("分页获取数据库信息", ex);
                        throw;
                    }
                    Hashtable hasTable = new Hashtable();
                    hasTable.Add("total", count);
                    hasTable.Add("rows", list);
                    return JsonHelper.ToJson(hasTable);
                }
            }
        }
        /// <summary>
        /// 根据项目编码，获取数据库
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public static IList<Database> GetDatabase(Guid projectCode) 
        {
            try
            {
                //获得当前运行的NHibernate实例
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    //事务开始
                    ITransaction transaction = session.BeginTransaction();
                    ICriteria criteria = session.CreateCriteria<Database>();
                    criteria.Add(Expression.Eq("ProjectCode", projectCode));
                    IList<Database> list = criteria.SetCacheable(true).AddOrder(Order.Asc("Id")).List<Database>();
                    //提交事务
                    transaction.Commit();
                    return list;
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("根据项目编码，获取数据库", ex);
                throw;
            }
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public static bool ConnectionDatabase(Database database)
        {
            string SqlConnection = string.Format("server={0};database={1};uid={2};pwd={3};", database.ServerName, database.Name, database.ServerUser, database.ServerPwd);
            return SQLHelper.TestConnection(SqlConnection);
        }
    }
}