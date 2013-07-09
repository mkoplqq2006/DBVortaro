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
        /// 分页得到数据库信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public static string GetPageDatabase(int start, int pageSize, string query, string projectCode)
        {
            //获得当前运行的NHibernate实例
            ISession session = NHibernateHelper.GetCurrentSession();
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
                        count = criteria.List<Database>().Count;
                        list = criteria.SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Desc("Id")).List<Database>();
                    }
                    //提交事务
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    NHibernateHelper.WriteErrorLog("分页得到数据库信息", ex);
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
        /// <summary>
        /// 根据项目编码，获取数据库
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public static IList<Database> GetDatabase(Guid projectCode) 
        {
            ISession session = null;
            try
            {
                //获得当前运行的NHibernate实例
                session = NHibernateHelper.GetCurrentSession();
                //事务开始
                ITransaction transaction = session.BeginTransaction();
                ICriteria criteria = session.CreateCriteria<Database>();
                criteria.Add(Expression.Eq("ProjectCode", projectCode));
                IList<Database> list = criteria.AddOrder(Order.Desc("Id")).List<Database>();
                int count = criteria.List<Database>().Count;
                //提交事务
                transaction.Commit();
                return list;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("根据项目编码，获取数据库", ex);
                throw;
            }
            finally
            {
                session.Close();
            }
        }
    }
}