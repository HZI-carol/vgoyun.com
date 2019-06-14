using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vgonyun.web.Common
{
    public class JwtCommon
    {
        /// <summary>
        /// 授权签名密钥
        /// </summary>
        public const string SignKey = "7tLOPwU6kvRSZSBuqWY6xkcpiEH87RtL";

        /// <summary>
        /// 授权过期时间（单位：分钟）
        /// </summary>
        public const int ExpireInMinutes = 120;
    }
}