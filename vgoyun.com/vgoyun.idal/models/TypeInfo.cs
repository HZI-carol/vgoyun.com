using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vgoyun.idal.models
{
    public class TypeInfo
    {       
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int  typeid { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int  parentid { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string  text { get; set; }

        public DateTime created { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int  sort { get; set; }


        #region 扩展字段
        
        /// <summary>
        /// 获取或设置类型文本
        /// </summary>
        public string typetext { get; set; }

        #endregion
    }

    public static class TypeInfoParentIds
    {
        /// <summary>
        /// 资讯
        /// </summary>
        public const int Information = 1;

        /// <summary>
        /// 案例
        /// </summary>
        public const int Case = 2;
    }
}
