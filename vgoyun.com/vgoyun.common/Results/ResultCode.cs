using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.common.Results
{
    public static class ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const int OK = 200;

        /// <summary>
        /// 标识系统内部错误
        /// </summary>
        public const int SystemException = -1;

        /// <summary>
        /// 表示参数错误
        /// </summary>
        public const int ArgumentException = 400001;

        /// <summary>
        /// 表示参数字符串超过指定长度
        /// </summary>
        public const int MaximumLength = 400002;

        /// <summary>
        /// 表示上传的文件类型不合法
        /// </summary>
        public const int InvalidUploadFile = 400003;

        /// <summary>
        /// 表示请求参数不合法
        /// </summary>
        public const int IllegalRequest = 400403;

        /// <summary>
        /// 表示指定的操作失败
        /// </summary>
        public const int ActionFail = 400400;
    }
}
