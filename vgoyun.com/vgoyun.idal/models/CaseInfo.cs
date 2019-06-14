using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vgoyun.idal.models
{
    public class CaseInfo
    {
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int typeid { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string imgurl { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int seecount { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int prizecount { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public DateTime created { get; set; }

        #region 扩展字段
        public string typetext { get; set; }

        #endregion
    }
}
