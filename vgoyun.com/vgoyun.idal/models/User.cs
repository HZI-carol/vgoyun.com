using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vgoyun.idal.models
{
    public class User
    {
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int  id { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string  username { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string  nickname { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public string  password { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public int  status { get; set; }
        
        /// <summary>
        /// 获取或设置
        /// </summary>
        public DateTime  created { get; set; }


        #region 扩展字段

        public string sid { get; set; }

        public string vcode { get; set; }

        public string newpassword { get; set; }

        #endregion
    }
}
