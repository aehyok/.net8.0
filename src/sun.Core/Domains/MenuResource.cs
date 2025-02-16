﻿using sun.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sun.Core.Domains
{
    /// <summary>
    /// 菜单与接口关联数据
    /// </summary>
    public class MenuResource : AuditedEntity
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 接口编号
        /// </summary>
        public long ApiResourceId { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public virtual Menu Menu { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public virtual ApiResource ApiResource { get; set; }
    }
}
