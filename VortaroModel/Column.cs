using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VortaroModel
{
    /// <summary>
    /// 列字段信息
    /// </summary>
    public class Column
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
        /// 表信息编码（外键）
        /// </summary>
        public virtual Guid? TablesCode { get; set; }
        /// <summary>
        /// 前缀
        /// </summary>
        public virtual string Owner { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public virtual string Type { get; set; }
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
        /// <summary>
        /// 字段状态（1显示0隐藏）默认:1
        /// </summary>
        public virtual int FieldState { get; set; }
        /// <summary>
        /// 隐藏字段的作者(默认为空)
        /// </summary>
        public virtual string HideAuthor { get; set; }
        /// <summary>
        /// 隐藏字段的时间(默认为空)
        /// </summary>
        public virtual DateTime? HideTime { get; set; }
    }
}
