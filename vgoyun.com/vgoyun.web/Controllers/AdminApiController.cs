using LF.Toolkit.Common;
using System;
using System.Collections.Generic;
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
using vgoyun.common;
using vgoyun.common.Common;
using vgoyun.common.Exceptions;
using vgoyun.common.Jwt;
using vgoyun.common.Results;
using vgoyun.idal;
using vgoyun.idal.models;
using vgoyun.idal.validators;
using vgoyun.web.Extensions;
using vgoyun.web.Filters;

namespace vgoyun.web.Controllers
{
    /// <summary>
    /// 表示管理后台接口控制器
    /// </summary>
    [RoutePrefix("api/admin")]
    [JweAuthenticationFilter]
    public class AdminApiController : ApiController
    {
        readonly IUserStorage m_UserStorage;
        readonly ITypeInfoStorage m_TypeInfoStorage;
        readonly IArticleStorage m_ArticleStorage;
        readonly IBannerStorage m_BannerStorage;
        readonly ICaseInfoStorage m_CaseInfoStorage;
        readonly ICommentStorage m_CommentStorage;
        readonly IIntentionStorage m_IntentionStorage;

        public AdminApiController(IUserStorage userStorage, ITypeInfoStorage typeInfoStorage, IArticleStorage articleStorage, ICaseInfoStorage caseInfoStorage
            , IBannerStorage bannerStorage, ICommentStorage commentStorage, IIntentionStorage intentionStorage)
        {
            m_UserStorage = userStorage;
            m_TypeInfoStorage = typeInfoStorage;
            m_ArticleStorage = articleStorage;
            m_CaseInfoStorage = caseInfoStorage;
            m_CommentStorage = commentStorage;
            m_BannerStorage = bannerStorage;
            m_IntentionStorage = intentionStorage;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        [Route("vcode")]
        [AllowAnonymous]
        public HttpResponseMessage GetValidateCode(string sid)
        {
            string code = RandomStringGenerator.CreateRandomNumeric(5);
            var resp = new HttpResponseMessage();

            try
            {
                if (string.IsNullOrEmpty(sid) || sid.Length < 32)
                {
                    resp.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    CacheProvider.Set("vcode." + sid, code, TimeSpan.FromMinutes(10));
                    byte[] buffer = ValidateCodeGenerator.CreateImage(code);
                    resp.Content = new ByteArrayContent(buffer);
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
            }
            catch
            {
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }

            return resp;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> LoginAsync(User json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            string vcode = "";
            if (!CacheProvider.TryGet("vcode." + json.sid, out vcode) || vcode != json.vcode) throw new BadRequestException(ResultCode.ArgumentException, "验证码错误");
            if (string.IsNullOrEmpty(json.username)) throw new BadRequestException(ResultCode.ArgumentException, "登录用户名不能为空");
            if (string.IsNullOrEmpty(json.password)) throw new BadRequestException(ResultCode.ArgumentException, "登录密码不能为空");

            string password = HashAlgorithmProvider.ComputeHash("MD5", json.password, true);
            var user = await this.m_UserStorage.GetAsync(json.username);
            if (user == null || user.password != password) throw new BadRequestException(ResultCode.ArgumentException, "用户名或密码错误");
            if (user.status != 1) throw new BadRequestException(ResultCode.ArgumentException, "用户已禁用，请联系管理员");

            //remove validate code
            CacheProvider.Remove("vcode." + json.sid);

            var data = new
            {
                user.username,
                user.nickname,
                access_token = JweProvider.Encode(JwtCommon.SignKey, user.id, JwtCommon.ExpireInMinutes),
                expires = DateTimeUtil.GetTimestamp(DateTime.Now.AddHours(2))
            };

            return Json(JsonApiResult.Ok(data));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("user/password")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePasswordAsync(User json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            if (string.IsNullOrEmpty(json.password)) throw new BadRequestException(ResultCode.ArgumentException, "旧密码不能为空");
            if (string.IsNullOrEmpty(json.newpassword)) throw new BadRequestException(ResultCode.ArgumentException, "新密码不能为空");
            if (json.newpassword.Length > 18 || json.newpassword.Length < 5) throw new BadRequestException(ResultCode.ArgumentException, "新密码长度为5～18位");

            string oldpassword = HashAlgorithmProvider.ComputeHash("MD5", json.password, true);
            var user = Request.GetProperty<User>(HttpPropertyKeys.AuthorizedUser);
            if (user.password != oldpassword) throw new BadRequestException(ResultCode.ArgumentException, "旧密码不正确");

            string newpassword = HashAlgorithmProvider.ComputeHash("MD5", json.newpassword, true);
            user.password = newpassword;
            int count = await this.m_UserStorage.UpdateAsync(user);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "更新密码失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 获取指定类型列表
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [Route("types/{parentid}")]
        public async Task<IHttpActionResult> GetTypeListAsync(int parentid)
        {
            var list = await this.m_TypeInfoStorage.GetListAsync(parentid);

            return Json(JsonApiResult.Ok(list));
        }

        #region 图片上传

        /// <summary>
        /// 富文本编辑器图片上传
        /// </summary>
        /// <returns></returns>
        [Route("editor/image/upload")]
        public async Task<IHttpActionResult> UploadEditorImageAsync()
        {
            object data = null;
            string tempDir = HttpContext.Current.Server.MapPath("~/temp");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            string dstDir = HttpContext.Current.Server.MapPath("~/upload/file/editor/image/") + dateStr + "/";
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            var fileProvider = new MultipartFormDataStreamProvider(tempDir);
            var multipart = await Request.Content.ReadAsMultipartAsync(fileProvider);

            try
            {
                ValidatorProvider.ThrowIfEqual(multipart.FileData.Count, 0, "请选择上传的文件");

                var fileData = multipart.FileData[0];
                var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "").Replace("\"", "");
                var ext = Path.GetExtension(fileName).ToLower();
                string[] picExts = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                ValidatorProvider.ThrowIfNotIn(ext, picExts, "上传的图片格式不正确");
                var fi = new FileInfo(fileData.LocalFileName);
                ValidatorProvider.ThrowIfMoreThan((int)fi.Length, 5 * 1024 * 1024, "上传的图片不能大于5M");

                string file = Guid.NewGuid().ToString("N") + ext;
                File.Move(fileData.LocalFileName, Path.Combine(dstDir, file));
                var uri = Request.RequestUri;
                data = new
                {
                    url = $"/upload/file/editor/image/{dateStr}/{file}",
                    fullurl = $"{WebConfigs.UrlPrefix}upload/file/editor/image/{dateStr}/{file}"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Request.RemoveTempFile(multipart.FileData);
            }

            return Json(JsonApiResult.Ok(data));
        }

        /// <summary>
        /// 通用图片上传
        /// </summary>
        /// <returns></returns>
        [Route("image/upload")]
        public async Task<IHttpActionResult> UploadImageAsync()
        {
            object data = null;
            string tempDir = HttpContext.Current.Server.MapPath("~/temp");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string dstDir = HttpContext.Current.Server.MapPath("~/upload/file/image");
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            var fileProvider = new MultipartFormDataStreamProvider(tempDir);
            var multipart = await Request.Content.ReadAsMultipartAsync(fileProvider);

            try
            {
                ValidatorProvider.ThrowIfEqual(multipart.FileData.Count, 0, "请选择上传的文件");

                var fileData = multipart.FileData[0];
                var fileName = fileData.Headers.ContentDisposition.FileName.Replace("\"", "").Replace("\"", "");
                var ext = Path.GetExtension(fileName).ToLower();
                string[] picExts = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                ValidatorProvider.ThrowIfNotIn(ext, picExts, "上传的图片格式不正确");
                var fi = new FileInfo(fileData.LocalFileName);
                ValidatorProvider.ThrowIfMoreThan((int)fi.Length, 1 * 1024 * 1024, "上传的图片不能大于1M");

                string file = Guid.NewGuid().ToString("N") + ext;
                File.Move(fileData.LocalFileName, Path.Combine(dstDir, file));
                var uri = Request.RequestUri;
                data = new
                {
                    url = "/upload/file/image/" + file,
                    fullurl = string.Format("/{0}upload/file/image/{1}", WebConfigs.UrlPrefix, file)
                };
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Request.RemoveTempFile(multipart.FileData);
            }

            return Json(JsonApiResult.Ok(data));
        }

        #endregion

        #region banner

        /// <summary>
        /// 添加Banner
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("banners")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBannerAsync(Banner json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            ValidatorProvider.ThrowIfInValidate<BannerValidator, Banner>(json);
            int count = await this.m_BannerStorage.InsertAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "添加失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 更新Banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("banners/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateBannerAsync(int id, Banner json)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要更新的Banner");
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            var banner = await this.m_BannerStorage.GetAsync(id);
            if (banner == null) throw new BadRequestException(ResultCode.ArgumentException, "更新的案例不存在");

            banner.title = json.title;
            banner.image_url = json.image_url;
            banner.type = json.type;
            banner.link_url = json.link_url;
            banner.sort = json.sort;

            ValidatorProvider.ThrowIfInValidate<BannerValidator, Banner>(banner);
            int count = await this.m_BannerStorage.UpdateAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "更新失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 删除指定Banner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("banners/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBannerAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要删除的Banner");
            int count = await this.m_BannerStorage.DeleteAsync(id);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "删除失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 获取Banner列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("banners/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetBannerListAsync(int page, int pageSize, string title = "", int type = 0, string orderBy = "")
        {
            var pageList = await this.m_BannerStorage.GetPagedListAsync(page, pageSize, type, title ?? "", orderBy);
            var data = new
            {
                list = pageList.RowSet.Select(i => new
                {
                    i.id,
                    i.image_url,
                    full_image_url = WebConfigs.UrlPrefix + i.image_url,
                    i.title,
                    i.sort,
                    i.link_url,
                    i.created,
                    i.type,
                    i.type_text
                }),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        #endregion

        #region 案例

        /// <summary>
        /// 添加案例
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("cases")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCaseAsync(CaseInfo json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            ValidatorProvider.ThrowIfInValidate<CaseInfoValidator, CaseInfo>(json);
            int count = await this.m_CaseInfoStorage.InsertAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "添加失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 更新案例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("cases/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCaseAsync(int id, CaseInfo json)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要更新的案例");
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            var cases = await this.m_CaseInfoStorage.GetAsync(id);
            if (cases == null) throw new BadRequestException(ResultCode.ArgumentException, "更新的案例不存在");

            cases.title = json.title;
            cases.imgurl = json.imgurl;
            cases.typeid = json.typeid;
            cases.link = json.link;
            cases.seecount = json.seecount;
            cases.prizecount = json.prizecount;
            cases.sort = json.sort;

            ValidatorProvider.ThrowIfInValidate<CaseInfoValidator, CaseInfo>(cases);
            int count = await this.m_CaseInfoStorage.UpdateAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "更新失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 删除指定案例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("cases/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCaseAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要删除的案例");
            int count = await this.m_CaseInfoStorage.DeleteAsync(id);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "删除失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 获取案例列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("cases/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetCaseListAsync(int page, int pageSize, string title = "", int type = 0, string orderBy = "")
        {
            var pageList = await this.m_CaseInfoStorage.GetPagedListAsync(page, pageSize, title ?? "", type, orderBy);
            var data = new
            {
                list = pageList.RowSet.Select(i => new
                {
                    i.id,
                    i.imgurl,
                    fullimgurl = WebConfigs.UrlPrefix + i.imgurl,
                    i.title,
                    i.sort,
                    i.link,
                    i.seecount,
                    i.prizecount,
                    i.created,
                    i.typeid,
                    i.typetext
                }),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }


        #endregion

        #region 资讯

        /// <summary>
        /// 添加资讯
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Route("articles")]
        [HttpPost]
        public async Task<IHttpActionResult> AddArticleAsync(Article json)
        {
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            ValidatorProvider.ThrowIfInValidate<ArticleValidator, Article>(json);

            json.created = json.created.ToLocalTime();
            int count = await this.m_ArticleStorage.InsertAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "添加失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 获取指定资讯
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("articles/{id}")]
        public async Task<IHttpActionResult> GetArticleAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要获取的资讯");
            var article = await this.m_ArticleStorage.GetAsync(id);
            if (article == null) throw new BadRequestException(ResultCode.ActionFail, "指定获取的资讯不存在");

            var data = new
            {
                article.id,
                article.imgurl,
                fullimgurl = WebConfigs.UrlPrefix + article.imgurl,
                article.title,
                article.sort,
                article.author,
                article.seecount,
                article.samllcontents,
                article.contents,
                article.ishot,
                article.isnew,
                article.isshow,
                article.typeid,
                article.typetext,
                article.created
            };

            return Json(JsonApiResult.Ok(data));
        }

        /// <summary>
        /// 更新资讯
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("articles/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateArticleAsync(int id, Article json)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要更新的资讯");
            if (json == null) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            var cases = await this.m_ArticleStorage.GetAsync(id);
            if (cases == null) throw new BadRequestException(ResultCode.ArgumentException, "更新的资讯不存在");

            cases.title = json.title;
            cases.author = json.author;
            cases.imgurl = json.imgurl;
            cases.typeid = json.typeid;
            cases.samllcontents = json.samllcontents;
            cases.seecount = json.seecount;
            cases.contents = json.contents;
            cases.sort = json.sort;
            cases.ishot = json.ishot;
            cases.isnew = json.isnew;
            cases.isshow = json.isshow;
            cases.created = json.created.ToLocalTime();

            ValidatorProvider.ThrowIfInValidate<ArticleValidator, Article>(json);
            int count = await this.m_ArticleStorage.UpdateAsync(json);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "更新失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 删除指定资讯
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("articles/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteArticleAsync(int id)
        {
            if (id <= 0) throw new BadRequestException(ResultCode.ArgumentException, "请选择要删除的资讯");
            int count = await this.m_ArticleStorage.DeleteAsync(id);
            if (count <= 0) throw new BadRequestException(ResultCode.ActionFail, "删除失败");

            return Json(JsonApiResult.Ok(""));
        }

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="ishot"></param>
        /// <param name="isnew"></param>
        /// <param name="isshow"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("articles/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetArticleListAsync(int page, int pageSize, string title = "", int type = 0, int ishot = -1, int isnew = -1, int isshow = -1, string orderBy = "")
        {
            var pageList = await this.m_ArticleStorage.GetPagedListAsync(page, pageSize, title ?? "", type, ishot, isnew, isshow, orderBy);
            var data = new
            {
                list = pageList.RowSet.Select(i => new
                {
                    i.id,
                    i.imgurl,
                    fullimgurl = WebConfigs.UrlPrefix + i.imgurl,
                    i.title,
                    i.sort,
                    i.author,
                    i.seecount,
                    i.samllcontents,
                    i.contents,
                    i.ishot,
                    i.isnew,
                    i.isshow,
                    i.typeid,
                    i.typetext,
                    i.created
                }),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        #endregion

        #region 留言（吐槽）

        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="created"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("comments/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetCommentListAsync(int page, int pageSize, string created = "", string orderBy = "")
        {
            var pageList = await this.m_CommentStorage.GetPagedListAsync(page, pageSize, "", created ?? "", orderBy);
            var data = new
            {
                list = pageList.RowSet.Select(i => new
                {
                    i.id,
                    i.contents,
                    i.ipaddress,
                    i.created
                }),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        #endregion

        #region 手机意向度

        string GetIntentionText(string intention)
        {
            var ids = intention.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            if (ids.Any())
            {
                var text = new List<string>();
                foreach (var id in ids)
                {
                    if (id == "1")
                    {
                        text.Add("兼职做");
                    }
                    else if (id == "2")
                    {
                        text.Add("想创业");
                    }
                    else if (id == "3")
                    {
                        text.Add("看看而已");
                    }
                    else if (id == "4")
                    {
                        text.Add("官网联系我们");
                    }
                }

                return string.Join("、", text);
            }

            return "";
        }

        /// <summary>
        /// 获取手机意向度列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="intention"></param>
        /// <param name="created"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("intentions/{page}/{pageSize}")]
        public async Task<IHttpActionResult> GetIntentionListAsync(int page, int pageSize, string keyword = "", string intention = "", string orderBy = "")
        {
            var intentions = StringProvider.SpiltToNumbers<int>(",", intention);
            var pageList = await this.m_IntentionStorage.GetPagedListAsync(page, pageSize, keyword ?? "", intentions, orderBy);
            var data = new
            {
                list = pageList.RowSet.Select(i => new
                {
                    i.id,
                    i.name,
                    i.phone,
                    i.intention,
                    i.useragent,
                    i.remark,
                    intention_text = GetIntentionText(i.intention),
                    i.created
                }),
                count = pageList.Count
            };

            return Json(JsonApiResult.Ok(data));
        }

        #endregion
    }
}
