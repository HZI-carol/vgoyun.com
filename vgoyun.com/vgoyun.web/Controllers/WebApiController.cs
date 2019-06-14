using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using vgonyun.web;
using vgonyun.web.Common;
using vgoyun.common.Exceptions;
using vgoyun.common.Results;
using vgoyun.idal;
using vgoyun.idal.models;
using vgoyun.idal.validators;
using vgoyun.web.Extensions;

namespace vgoyun.web.Controllers
{
    [RoutePrefix("api/web")]
    public partial class WebApiController : ApiController
    {
        readonly ITypeInfoStorage m_TypeInfoStorage;
        readonly IArticleStorage m_ArticleStorage;
        readonly IBannerStorage m_BannerStorage;
        readonly ICaseInfoStorage m_CaseInfoStorage;
        readonly ICommentStorage m_CommentStorage;
        readonly IIntentionStorage m_IntentionStorage;

        public WebApiController(ITypeInfoStorage typeInfoStorage, IArticleStorage articleStorage, ICaseInfoStorage caseInfoStorage, ICommentStorage commentStorage
            , IBannerStorage bannerStorage, IIntentionStorage intentionStorage)
        {
            m_TypeInfoStorage = typeInfoStorage;
            m_ArticleStorage = articleStorage;
            m_CaseInfoStorage = caseInfoStorage;
            m_CommentStorage = commentStorage;
            m_BannerStorage = bannerStorage;
            m_IntentionStorage = intentionStorage;
        }

        /// <summary>
        /// 获取指定内容的二维码图片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("qrcode")]
        public HttpResponseMessage GetQrcode(string data)
        {
            var response = Request.CreateResponse();
            if (!String.IsNullOrEmpty(data))
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                Bitmap icon = null;
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/favicon.ico")))
                {
                    icon = new Bitmap(HttpContext.Current.Server.MapPath("~/favicon.ico"));
                }
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap qrCodeImage = qrCode.GetGraphic(10, Color.Black, Color.White, icon))
                {
                    var stream = new MemoryStream();
                    qrCodeImage.Save(stream, ImageFormat.Png);
                    stream.Position = 0;
                    var buffer = stream.ToArray();
                    stream.Dispose();
                    response.Content = new ByteArrayContent(buffer);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
            }

            return response;
        }

        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [Route("types")]
        public async Task<IHttpActionResult> GetTypeListAsync(int parentid)
        {
            var list = await this.m_TypeInfoStorage.GetListAsync(parentid);
            var data = list.Where(i => i.parentid == parentid);
            return Json(JsonApiResult.Ok(data));
        }

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="typeid"></param>
        /// <param name="ishot"></param>
        /// <returns></returns>
        [Route("articles/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetArticleListAsync(int page, int pageSize, int typeid = 0, int ishot = -1)
        {
            var pageList = await this.m_ArticleStorage.GetPagedListAsync(page, pageSize, "", typeid, ishot, -1, -1, "created_DESC");
            var data = new
            {
                list = pageList.RowSet.Select(i => Projections.GetArticleProjection(i)),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        #region 案例

        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="typeid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Route("cases/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetCaseListAsync(int page, int pageSize, int typeid = 0, string title = "")
        {
            var pageList = await this.m_CaseInfoStorage.GetPagedListAsync(page, pageSize, title ?? "", typeid, "");
            var data = new
            {
                list = pageList.RowSet.Select(i => Projections.GetCaseProjection(i)),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        /// <summary>
        /// 设置点赞数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("case/{id}/setprizecount")]
        [HttpPost]
        public async Task<IHttpActionResult> SetCasePrizeCountAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要更新的案例");
            var cases = await this.m_CaseInfoStorage.GetAsync(id);
            if (cases == null) throw new BadRequestException(ResultCode.ArgumentException, "更新的案例不存在");

            cases.prizecount++;
            int count = await this.m_CaseInfoStorage.UpdateAsync(cases);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "操作失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 设置浏览数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("case/{id}/setseecount")]
        [HttpPost]
        public async Task<IHttpActionResult> SetCaseSeeCountAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要更新的案例");
            var cases = await this.m_CaseInfoStorage.GetAsync(id);
            if (cases == null) throw new BadRequestException(ResultCode.ArgumentException, "更新的案例不存在");

            cases.seecount++;
            int count = await this.m_CaseInfoStorage.UpdateAsync(cases);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "操作失败");

            return Json(JsonApiResult.Ok(""));
        }

        #endregion

        #region 留言（吐槽）

        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("comments")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCommenttAsync(Comment json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            //获取ip地址
            json.ipaddress = Request.GetUserHostAddress();
            ValidatorProvider.ThrowIfInValidate<CommentValidator, Comment>(json);
            int count = await this.m_CommentStorage.InsertAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "添加失败");

            return Json(JsonApiResult.Ok(""));
        }

        #endregion
    }

    public partial class WebApiController : ApiController
    {
        /// <summary>
        /// 添加手机网站提交的意向度
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("intentions")]
        [HttpPost]
        public async Task<IHttpActionResult> AddAsync(Intention json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            if (json.intentions == null || !json.intentions.Distinct().Any()) throw new ArgumentException("请选择意向度");
            if (!ValidatorProvider.IsPhone(json.phone)) throw new ArgumentException("手机号格式不正确");
            json.intention = string.Join(",", json.intentions.Distinct());
            json.useragent = Request.Headers.UserAgent.ToString();

            ValidatorProvider.ThrowIfInValidate<IntentionValidator, Intention>(json);
            int count = await this.m_IntentionStorage.InsertAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "添加失败");

            return Json(JsonApiResult.Ok(""));
        }
    }
}