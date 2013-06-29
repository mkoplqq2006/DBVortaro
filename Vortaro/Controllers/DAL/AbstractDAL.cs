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
            ISession session = null;
            try
            {
                session = NHibernateHelper.GetCurrentSession();
                ITransaction transaction = session.BeginTransaction();
                session.Save(entity);
                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("执行添加操作", ex);
                throw null;
            }
            finally
            {
                session.Close();
            }
        }
        /// <summary>
        /// 通过编号获得单个对象
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static T GetProjectById(Guid code)
        {
            ISession session = null;
            try
            {
                session = NHibernateHelper.GetCurrentSession();
                ITransaction transaction = session.BeginTransaction();
                T entity = session.Load<T>(code);
                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("通过编号获得单个对象", ex);
                throw null;
            }
            finally
            {
                session.Close();
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public static T Update(T entity)
        {
            ISession session = null;
            try
            {
                session = NHibernateHelper.GetCurrentSession();
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(entity);
                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("执行更新操作", ex);
                throw null;
            }
            finally
            {
                session.Close();
            }
        }
        /// <summary>
        /// 根据编码删除表并返回这个实体对象.
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public static T Delete(Guid code)
        {
            ISession session = null;
            try
            {
                T entity = GetProjectById(code);
                session = NHibernateHelper.GetCurrentSession();
                ITransaction transaction = session.BeginTransaction();
                if (entity != null)
                {
                    session.Delete(entity);
                }
                transaction.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("执行删除操作", ex);
                throw null;
            }
            finally
            {
                session.Close();
            }
        }
    }
}