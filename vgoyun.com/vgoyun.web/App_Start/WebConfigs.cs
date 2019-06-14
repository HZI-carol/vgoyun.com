using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace vgoyun.web
{
    public class WebConfigs
    {
        /// <summary>
        /// 是否开启跨域（若以应用程序部署在IIS的站点下则需要关闭）
        /// </summary>
        public static bool EnableCors
        {
            get
            {
                return ConfigurationManager.AppSettings["enableCors"] == "1";
            }
        }

        /// <summary>
        /// 用于显示图片、上传文件的url前缀
        /// </summary>
        public static string UrlPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings["urlPrefix"];
            }
        }
    }
}