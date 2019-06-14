using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vgonyun.web.Extensions;

namespace vgonyun.web.Controllers
{
    /// <summary>
    /// 3D空间控制器
    /// </summary>
    [RoutePrefix("")]
    public class SpaceController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsMobile())
            {
                filterContext.Result = new RedirectResult("/m");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [Route("3d")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 产品介绍
        /// </summary>
        /// <returns></returns>
        [Route("3d/product")]
        public ActionResult Product()
        {
            return View();
        }

        /// <summary>
        /// 行业前景
        /// </summary>
        /// <returns></returns>
        [Route("3d/industry")]
        public ActionResult Industry()
        {
            return View();
        }

        /// <summary>
        /// 3d空间
        /// </summary>
        /// <returns></returns>
        [Route("3d/space")]
        public ActionResult SpaceCase()
        {
            return View();
        }

        /// <summary>
        /// 3D空间详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("3d/spaces/{id}")]
        public ActionResult ViewSpaceCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return HttpNotFound();

            ViewBag.id = id;

            return View();
        }
    }
}