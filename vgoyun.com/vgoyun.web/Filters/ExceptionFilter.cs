using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using vgoyun.common;
using vgoyun.common.Exceptions;
using vgoyun.common.Results;
using vgoyun.web.Attributes;

namespace vgoyun.web.Filters
{
    /// <summary>
    /// 表示全局的异常过滤器
    /// </summary>
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var exception = actionExecutedContext.Exception;
            bool process = false;
            int code = 0;
            string message = "";

            if (exception is HttpResponseException)
            {
                process = true;
                actionExecutedContext.Response = (exception as HttpResponseException).Response;
            }
            else if (exception is ArgumentException)
            {
                code = ResultCode.ArgumentException;
                message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                var ex = exception as BadRequestException;
                code = ex.ErrorCode;
                message = ex.Message ?? "";
            }
            else
            {
                code = ResultCode.SystemException;
#if DEBUG
                message = exception.ToString();
#else

                message = "系统出错，请稍后再试～";
                                //记录未处理异常
                LogProvider.Error.Error(exception);
#endif
            }

            if (!process)
            {
                var data = new JsonApiResult
                {
                    code = code,
                    msg = message
                };
                //判断是否文件Action
                if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<FileResultAttribute>().Any())
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, data);
                }
                else
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}