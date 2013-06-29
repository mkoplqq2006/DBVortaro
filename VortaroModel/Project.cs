using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VortaroModel
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class Project
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
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 简介
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
