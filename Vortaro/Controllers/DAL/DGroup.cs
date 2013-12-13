using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using VortaroModel;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 功能分组信息
    /// </summary>
    public class DGroup : AbstractDAL<Group>
    {
        /// <summary>
        /// 分页获取功能分组信息
        /// </summary>
        /// <param name="start">起始</param>
        /// <param name="pageSize">结束</param>
        /// <param name="query">条件</param>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public static string GetPageGroup(int start, int pageSize, string query, string projectCode)
        {
            //获得当前运行的NHibernate实例
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                //事务开始
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IList<Group> list = null;//分页的记录
                    int count = 0;//总的记录条数
                    try
                    {
                        ICriteria criteria = session.CreateCriteria<Group>();
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
                                criteria.Add(Expression.Like("Name", "%" + query + "%"));
                            }
                            count = criteria.SetCacheable(true).List<Group>().Count;
                            list = criteria.SetCacheable(true).SetFirstResult(start).SetMaxResults(pageSize).AddOrder(Order.Desc("Id")).List<Group>();
                        }
                        //提交事务
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        NHibernateHelper.WriteErrorLog("分页获取功能分组信息", ex);
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
        /// 根据分组编码，获取分组名称
        /// </summary>
        /// <param name="Code">分组编码</param>
        /// <returns></returns>
        public static string GetGroupName(Guid? Code) 
        {
            try
            {
                //获得当前运行的NHibernate实例
                using (ISession session = NHibernateHelper.GetCurrentSession())
                {
                    //事务开始
                    ITransaction transaction = session.BeginTransaction();
                    ICriteria criteria = session.CreateCriteria<Group>();
                    criteria.Add(Expression.Eq("Code", Code));
                    IList<Group> list = criteria.List<Group>();
                    //提交事务
                    transaction.Commit();
                    return list[0].Name;
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("分页获取功能分组信息", ex);
                throw;
            }
        }
    }
}