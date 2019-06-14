using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.idal.models
{
    public class Intention
    {
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 获取或设置姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 获取或设置手机号
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 获取或设置user-agent
        /// </summary>
        public string useragent { get; set; }

        /// <summary>
        /// 获取或设置备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 获取或设置意向度集合(多个以逗号分隔)
        /// </summary>
        public string intention { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime created { get; set; }

        #region 扩展字段

        /// <summary>
        /// 获取或设置意向度
        /// </summary>
        public int[] intentions { get; set; }

        #endregion
    }
}
