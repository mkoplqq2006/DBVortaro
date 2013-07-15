using System;
using NHibernate;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 提供类的抽象支持
    /// </summary>
    public class AbstractDAL<T> 
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static T Add(T entity)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ITransaction transaction = session.BeginTransaction();
                try
                {
                    session.Save(entity);
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    NHibernateHelper.WriteErrorLog("NHibernate执行添加操作", ex);
                    throw null;
                }
            }
        }
        /// <summary>
        /// 通过编号获得单个对象
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static T GetProjectById(Guid code)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ITransaction transaction = session.BeginTransaction();
                try
                {
                    T entity = session.Load<T>(code);
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    NHibernateHelper.WriteErrorLog("NHibernate通过编号获得单个对象", ex);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public static T Update(T entity)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ITransaction transaction = session.BeginTransaction();
                try
                {
                    session.Update(entity);
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    NHibernateHelper.WriteErrorLog("NHibernate执行更新操作", ex);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 根据编码删除表并返回这个实体对象.
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static T Delete(Guid code)
        {
            using (ISession session = NHibernateHelper.GetCurrentSession())
            {
                ITransaction transaction = session.BeginTransaction();
                try
                {
                    T entity = GetProjectById(code);
                    if (entity != null)
                    {
                        session.Delete(entity);
                    }
                    session.Flush();
                    transaction.Commit();
                    return entity;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    NHibernateHelper.WriteErrorLog("NHibernate执行删除操作", ex);
                    throw ex;
                }
            }
        }
    }
}