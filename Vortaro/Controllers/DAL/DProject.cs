using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using VortaroModel;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class DProject : AbstractDAL<Project>
    {
        /// <summary>
        /// 分页获取项目信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static string GetPageProject(int start, int pageSize, string query, string userName)
        {
            //获得当前运行的NHibernate实例
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                //事务开始
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<Project> list = null;//分页的记录
                    int count = 0;//总的记录条数
                    try
                    {
                        ICriteria criteria = session.CreateCriteria<Project>();
                        criteria.Add(Expression.Eq("Author", userName));
                        if (!String.IsNullOrEmpty(query))
                        {
                            criteria.Add(Expression.Or(Expression.Like("Name", "%" + query + "%"), Expression.Like("Bewrite", "%" + query + "%")));
                        }
                        count = criteria.SetCacheable(true).List<Project>().Count;
                        list = criteria.SetCacheable(true).SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Asc("Id")).List<Project>();
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        NHibernateHelper.WriteErrorLog("分页获取项目信息", ex);
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
    }
}