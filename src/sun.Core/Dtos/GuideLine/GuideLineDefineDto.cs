﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace sun.Core.Dtos.GuideLine
{
    /// <summary>
    /// 指标元数据定义 
    /// 
    /// </summary>
    public class GuideLineDefineDto
    {
        /// <summary>
        /// 指标Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 指标名称
        /// </summary>
        [DataMember]
        public string GuideLineName { get; set; }

        /// <summary>
        /// 指标所在的组名称
        /// </summary>
        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string GuideLineMethod { get; set; }
        /// <summary>
        /// 父指标ID
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }

        /// <summary>
        /// 指标显示顺序
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public string GuideLineMeta { get; set; }
        /// <summary>
        /// 指标说明
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// 子指标集合
        /// </summary>
        [DataMember]
        public List<GuideLineDefineDto> Children { get; set; }

        [DataMember]
        public AutoGuideLineGroupDto MD_GuideLineGroup { get; set; }

        [DataMember]
        public string DetailMeta { get; set; }
        /// <summary>
        /// 指标查询参数集合
        /// </summary>
        [DataMember]
        public List<AutoGuideLineParameterDto> Parameters { get; set; }

        /// <summary>
        /// 指标查询结果分组集合
        /// </summary>
        [DataMember]
        public List<AutoGuideLineFieldGroupDto> ResultGroups { get; set; }
        /// <summary>
        /// 详细信息链接
        /// </summary>
        [DataMember]
        public List<AutoGuideLineDetailDefineDto> DetailDefines { get; set; }

        [DataMember]
        public string GuideLineQueryMethod { get; set; }

        public GuideLineDefineDto() { }
        public GuideLineDefineDto(string id, string name, string groupname, string fatherid, int displayorder, string descript)
        {
            Id = id;
            GuideLineName = name;
            GroupName = groupname;
            ParentId = fatherid;
            DisplayOrder = displayorder;
            Description = descript;
        }
        public GuideLineDefineDto(string id, string glName, string groupName, string glMethod, string glMeta, string fid, string glQueryMethod,
               string detailMeta, int order, string _des)
        {
            Id = id;
            GuideLineName = glName;
            GroupName = groupName;
            GuideLineMethod = glMethod;
            GuideLineMeta = glMeta + detailMeta;
            ParentId = fid;
            GuideLineQueryMethod = glQueryMethod;
            DetailMeta = detailMeta;
            DisplayOrder = order;
            this.Description = _des;
        }
        public override string ToString()
        {
            return GuideLineName;
        }
    }

    /// <summary>
    /// 指标定义扩展 查询数据时转换使用
    /// </summary>
    public class GuideLineDefineDto_Ex
    {
        public List<AutoGuideLineFieldGroupDto> ResultGroups { get; set; }

        public string QueryString { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
