using LF.Toolkit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.common.Jwt
{
    /// <summary>
    /// 表示jwe荷载信息类
    /// </summary>
    public class JwePayload
    {
        /// <summary>
        /// 获取或设置颁发时间
        /// </summary>
        public long iat { get; set; }

        /// <summary>
        /// 获取或设置过期时间
        /// </summary>
        public long exp { get; set; }

        #region 扩展字段

        /// <summary>
        /// 获取或设置token关联的作用域内的用户ID
        /// </summary>
        public int uid { get; set; }

        #endregion

        /// <summary>
        /// 获取当前Token是否过期
        /// </summary>
        /// <returns></returns>
        public bool IsExpires()
        {
            return DateTimeUtil.Timestamp - exp > 0;
        }
    }
}
