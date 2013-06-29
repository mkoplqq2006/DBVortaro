using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VortaroModel
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class Database
    {
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual Guid Code { get; set; }
        /// <summary>
        /// 项目信息编码（外键）
        /// </summary>
        public virtual Guid? ProjectCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string Alias { get; set; }
        /// <summary>
        /// 分类（默认MSSQLSERVER）
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public virtual string ServerName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public virtual string ServerUser { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string ServerPwd { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Bewrite { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public virtual string Author { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    }
}
