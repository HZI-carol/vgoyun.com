using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace vgoyun.web.Extensions
{
    public static class HttpRequestMessageExtension
    {
        /// <summary>
        /// 获取Http请求上下文
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static HttpContextBase GetHttpContext(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException("request");

            object obj;
            if (request.Properties.TryGetValue("MS_HttpContext", out obj))
            {
                return obj as HttpContextBase;
            }

            return null;
        }

        /// <summary>
        /// 获取客户端Ip地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetUserHostAddress(this HttpRequestMessage request)
        {
            var context = request.GetHttpContext();
            if (context != null)
            {
                return context.Request.UserHostAddress == "::1" ? "127.0.0.1" : context.Request.UserHostAddress;
            }

            return "";
        }

        /// <summary>
        /// 获取本次请求指定名称的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetProperty<T>(this HttpRequestMessage request, string key)
        {
            object obj = null;
            if (request.Properties.TryGetValue(key, out obj))
            {
                if (typeof(T).IsAssignableFrom(obj.GetType()))
                {
                    return (T)obj;
                }
            }

            return default(T);
        }

        /// <summary>
        /// 获取当前请求的授权用户信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static T GetAuthorizedUser<T>(this HttpRequestMessage request)
        {
            return GetProperty<T>(request, HttpPropertyKeys.AuthorizedUser);
        }

        /// <summary>
        /// 移除当此请求上传的临时文件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fileData"></param>
        public static void RemoveTempFile(this HttpRequestMessage request, IEnumerable<MultipartFileData> fileData)
        {
            foreach (var item in fileData)
            {
                try
                {
                    if (File.Exists(item.LocalFileName))
                    {
                        File.Delete(item.LocalFileName);
                    }
                }
                catch { }
            }
        }
    }
}