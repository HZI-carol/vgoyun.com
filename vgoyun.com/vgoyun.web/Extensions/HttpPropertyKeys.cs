using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vgoyun.web.Extensions
{
    public class HttpPropertyKeys
    {
        /// <summary>
        /// 当前Jwe的头部信息
        /// </summary>
        public static readonly string JweHeader = "http.property.jwe.header";

        /// <summary>
        /// 当前Jwe的载荷信息
        /// </summary>
        public static readonly string JwePayload = "http.property.jwe.payload";

        /// <summary>
        /// 获取当前认证的用户信息
        /// </summary>
        public static readonly string AuthorizedUser = "http.property.authorize.user";
    }
}