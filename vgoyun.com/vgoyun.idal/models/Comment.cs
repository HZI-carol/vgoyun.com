using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.idal.models
{
    public class Comment
    {

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 获取或设置留言内容
        /// </summary>
        public string contents { get; set; }

        /// <summary>
        /// 获取或设置留言ip地址
        /// </summary>
        public string ipaddress { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime created { get; set; }

    }
}
