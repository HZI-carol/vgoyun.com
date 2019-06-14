using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using LF.Toolkit.IOC;
using vgonyun.web.Common;
using vgoyun.common;
using vgoyun.common.Jwt;
using vgoyun.idal;
using vgoyun.web.Extensions;

namespace vgoyun.web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JweAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //判断是否匿名访问的接口或控制器
            if (!context.ActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(false).Any()
                && !context.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(false).Any())
            {
                string token = "";
                //从url中获取token
                var qs = context.Request.GetQueryNameValuePairs().ToDictionary(i => i.Key, i => i.Value);
                if (qs.ContainsKey("token"))
                {
                    token = qs["token"];
                }
                else
                {
                    var httpContext = context.Request.GetHttpContext();
                    token = httpContext.Request["token"];
                }
                if (string.IsNullOrWhiteSpace(token) || Regex.Matches(token, @"\.").Count != 4) throw new HttpResponseException(HttpStatusCode.Unauthorized);

                //解析token荷载部分
                JwePayload payload = null;
                try
                {
                    payload = JweProvider.Decode(token, JwtCommon.SignKey);
                }
                catch (Exception e)
                {
                    LogProvider.Error.Error(e, "解析jwepayload失败");
                }
                if (payload == null) throw new HttpResponseException(HttpStatusCode.Unauthorized);
                //判断是否过期
                if (payload.IsExpires()) throw new HttpResponseException(HttpStatusCode.Unauthorized);
                var user = await InjectionContainer.Resolve<IUserStorage>().GetAsync(payload.uid);
                if (user == null) throw new HttpResponseException(HttpStatusCode.Unauthorized);
                //保存user
                context.Request.Properties.Add(HttpPropertyKeys.AuthorizedUser, user);
                //保存jwe payload
                context.Request.Properties.Add(HttpPropertyKeys.JwePayload, payload);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}