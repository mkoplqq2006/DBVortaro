using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VortaroModel
{
    /// <summary>
    /// 功能分组信息
    /// </summary>
    public class Group
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
        /// 项目编码（外键）
        /// </summary>
        public virtual Guid? ProjectCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
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
