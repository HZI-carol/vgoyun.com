using LF.Toolkit.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using vgonyun.web.Common;
using vgoyun.idal;
using vgoyun.idal.models;
using vgonyun.web.Extensions;
using vgoyun.common.Common;

namespace vgoyun.web.Controllers
{
    [RoutePrefix("")]
    public class HomeController : Controller
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

        [Route("~/")]
        public async Task<ActionResult> Index()
        {
            string key = "homecontroller->banner.index.list";
            if (!CacheProvider.TryGet(key, out IEnumerable<Banner> list))
            {
                list = await InjectionContainer.Resolve<IBannerStorage>().GetListAsync(10, (int)BannerType.首页, "sort_DESC");
                if (list.Any())
                {
                    CacheProvider.Set(key, list, TimeSpan.FromMinutes(10));
                }
            }
            ViewBag.BannerList = list;

            return View(list);
        }

        /// <summary>
        /// 产品介绍
        /// </summary>
        /// <returns></returns>
        [Route("product")]
        public ActionResult Product()
        {
            return View();
        }

        /// <summary>
        /// 产品介绍-全景VR营销
        /// </summary>
        /// <returns></returns>
        [Route("product/sales")]
        public ActionResult ProductSales()
        {
            return View();
        }

        /// <summary>
        /// 产品介绍-全景广告联盟
        /// </summary>
        /// <returns></returns>
        [Route("product/adsense")]
        public ActionResult ProductAdSense()
        {
            return View();
        }

        /// <summary>
        /// 系统购买
        /// </summary>
        /// <returns></returns>
        [Route("buy")]
        public ActionResult Buy()
        {
            return View();
        }

        /// <summary>
        /// 系统购买 - 专属定制
        /// </summary>
        /// <returns></returns>
        [Route("buy/customize")]
        public ActionResult SystemCustomize()
        {
            return View();
        }

        /// <summary>
        /// 解决方案
        /// </summary>
        /// <returns></returns>
        [Route("solution")]
        public ActionResult Solution()
        {
            return View();
        }

        /// <summary>
        /// 全景案例
        /// </summary>
        /// <returns></returns>
        [Route("case")]
        public async Task<ActionResult> Case()
        {
            //类型列表
            int parentid = 2;
            var list = await InjectionContainer.Resolve<ITypeInfoStorage>().GetListAsync(parentid);
            ViewBag.TypeList = list.Where(i => i.parentid == parentid).Select(i => new
            {
                i.typeid,
                i.text
            });
            // 分页列表
            var pageList = await InjectionContainer.Resolve<ICaseInfoStorage>().GetPagedListAsync(1, 6, "", list.Count() > 0 ? list.First().typeid : 0, "");
            ViewBag.CaseList = new
            {
                list = pageList.RowSet.Select(i => Projections.GetCaseProjection(i)),
                count = pageList.Count
            };

            return View();
        }

        /// <summary>
        /// 资讯
        /// </summary>
        /// <returns></returns>
        [Route("information")]
        public async Task<ActionResult> Information()
        {
            //类型列表
            int parentid = TypeInfoParentIds.Information;
            var list = await InjectionContainer.Resolve<ITypeInfoStorage>().GetListAsync(parentid);
            ViewBag.TypeList = list.Where(i => i.parentid == parentid).OrderBy(i => i.sort).Select(i => new
            {
                i.typeid,
                i.text
            });
            // 分页列表
            var pageList = await InjectionContainer.Resolve<IArticleStorage>().GetPagedListAsync(1, 10, "", list.Count() > 0 ? list.First().typeid : 0, -1, -1, -1, "created_DESC");
            ViewBag.InformationList = new
            {
                list = pageList.RowSet.Select(i => Projections.GetArticleProjection(i)),
                count = pageList.Count
            };
            //热点阅读
            var hotList = await InjectionContainer.Resolve<IArticleStorage>().GetPagedListAsync(1, 6, "", 0, 1, -1, -1, "created_DESC");
            ViewBag.HotList = pageList.RowSet.Select(i => Projections.GetArticleProjection(i));

            return View();
        }

        /// <summary>
        /// 资讯
        /// </summary>
        /// <returns></returns>
        [Route("information/{id}")]
        public async Task<ActionResult> InformationDetail(int id)
        {
            var storage = InjectionContainer.Resolve<IArticleStorage>();
            var model = await storage.GetAsync(id);
            if (model == null) throw new Exception("指定资讯详情不存在");
            //更新查看数
            model.seecount++;
            await storage.UpdateAsync(model);

            //获取页面数据
            var data = await storage.GetViewDataAsync(id);
            data.Current = model;

            return View(data);
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        [Route("about")]
        public ActionResult About()
        {
            return View();
        }
    }
}