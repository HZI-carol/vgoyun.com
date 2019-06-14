using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vgoyun.idal.models
{

    public enum BannerType
    {
        首页 = 1
    }

    public class Banner
    {
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 获取或设置图片地址
        /// </summary>
        public string image_url { get; set; }

        /// <summary>
        /// 获取或设置调转地址
        /// </summary>
        public string link_url { get; set; }

        /// <summary>
        /// 获取或设置排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 获取或设置图片类型 1=首页
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 获取或设置创建事件
        /// </summary>
        public DateTime created { get; set; }

        #region 扩展字段

        public string type_text => Enum.GetName(typeof(BannerType), type);

        #endregion
    }
}
