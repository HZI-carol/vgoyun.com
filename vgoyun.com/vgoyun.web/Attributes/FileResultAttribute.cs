using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vgoyun.web.Attributes
{
    /// <summary>
    /// 表示文件结果标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FileResultAttribute : Attribute
    {

    }
}