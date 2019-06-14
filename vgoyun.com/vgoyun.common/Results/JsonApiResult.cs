using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace vgoyun.common.Results
{
    public class JsonApiResult<T>
    {
        /// <summary>
        /// 获取或设置返回状态码;OK=200
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 获取或设置返回的数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 判断返回结果是否成功
        /// </summary>
        [JsonIgnore]
        //[ScriptIgnore]
        public bool IsOK { get { return code == ResultCode.OK; } }

        public static JsonApiResult<T> Ok(T data)
        {
            return new JsonApiResult<T>
            {
                code = ResultCode.OK,
                msg = "",
                data = data
            };
        }
    }

    public class JsonApiResult : JsonApiResult<object>
    {

    }
}
