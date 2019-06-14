using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using vgoyun.web.Filters;
using System.Web.Http.Cors;
using LF.Toolkit.IOC;
using vgoyun.dal;
using System.Reflection;
using Autofac.Integration.WebApi;
using vgoyun.web;
using WebActivatorEx;
using vgoyun.idal;
using LF.Toolkit.Data.Dapper;
using FluentValidation;

[assembly: PreApplicationStartMethod(typeof(WebApiConfig), "PreRegister")]
namespace vgoyun.web
{
    public static class WebApiConfig
    {
        /// <summary>
        /// 预加载网站启动相关步骤
        /// </summary>
        public static void PreRegister()
        {
            var config = GlobalConfiguration.Configuration;
            var dict = new Dictionary<Type, object>();
            dict.Add(typeof(string), "vgoyun.com");
            InjectionContainer.Register<SqlStorageBase>(typeof(UserStorage).Assembly, dict);
            //注册数据库模型验证类
            InjectionContainer.Register<IValidator>(typeof(IUserStorage).Assembly);
            InjectionContainer.Build((builder) =>
            {
                // Register your Web API controllers.
                builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
                // OPTIONAL: Register the Autofac filter provider.
                builder.RegisterWebApiFilterProvider(config);
            }, c => config.DependencyResolver = new AutofacWebApiDependencyResolver(c));
        }

        public static void Register(HttpConfiguration config)
        {
            if (WebConfigs.EnableCors)
            {
                var cors = new EnableCorsAttribute("*", "Content-Type", "GET, POST, PUT, DELETE, OPTIONS");
                config.EnableCors(cors);
            }

            // map router attribute
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //global exception filter
            config.Filters.Add(new ExceptionFilter());
        }
    }
}
