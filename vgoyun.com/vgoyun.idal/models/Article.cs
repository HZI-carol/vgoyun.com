using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vgoyun.idal.models
{
    public class Article
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
        public string author { get; set; }

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
        public string samllcontents { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public string contents { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool ishot { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool isnew { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public bool isshow { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public int seecount { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public DateTime created { get; set; }

        #region 扩展字段
        public string typetext { get; set; }

        #endregion
    }

    public class ArticleViewData
    {
        /// <summary>
        /// 当前
        /// </summary>
        public Article Current { get; set; }

        /// <summary>
        /// 上一篇
        /// </summary>
        public Article Previous { get; set;}

        /// <summary>
        /// 下一篇
        /// </summary>
        public Article Next { get; set; }

        /// <summary>
        /// 热点
        /// </summary>
        public IEnumerable<Article> HotList { get; set; }

        /// <summary>
        /// 最新
        /// </summary>
        public IEnumerable<Article> NewList { get; set; }

        /// <summary>
        /// 推荐列表
        /// </summary>
        public IEnumerable<Article> ShowList { get; set; }
    }
}
