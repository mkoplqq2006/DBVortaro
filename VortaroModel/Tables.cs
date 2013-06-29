using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VortaroModel
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class Tables
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
        /// 数据库编码（外键）
        /// </summary>
        public virtual Guid? DatabaseCode { get; set; }
        /// <summary>
        /// 功能分组编码（外键）
        /// </summary>
        public virtual Guid? GroupCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public virtual string Alias { get; set; }
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
