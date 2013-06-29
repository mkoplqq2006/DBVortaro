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
        /// 分页得到项目信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static string GetPageProject(int start, int pageSize, string query, string userName)
        {
            ISession session = null;
            try
            {
                //获得当前运行的NHibernate实例
                session = NHibernateHelper.GetCurrentSession();
                //事务开始
                ITransaction transaction = session.BeginTransaction();
                ICriteria criteria = session.CreateCriteria<Project>();
                criteria.Add(Expression.Eq("Author", userName));
                if (!String.IsNullOrEmpty(query))
                {
                    criteria.Add(Expression.Or(Expression.Like("Name", "%" + query + "%"), Expression.Like("Bewrite", "%" + query + "%")));
                }
                IList<Project> list = criteria.SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Desc("Id")).List<Project>();
                int count = criteria.List<Project>().Count;
                //提交事务
                transaction.Commit();
                Hashtable hasTable = new Hashtable();
                hasTable.Add("total", list.Count);
                hasTable.Add("rows", list);
                return JsonHelper.ToJson(hasTable);
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("分页得到项目信息", ex);
                throw;
            }
            finally
            {
                session.Close();
            }
        }
    }
}