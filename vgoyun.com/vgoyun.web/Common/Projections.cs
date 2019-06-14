using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vgoyun.idal.models;
using vgoyun.web;

namespace vgonyun.web.Common
{
    public static class Projections
    {
        public static dynamic GetArticleProjection(Article article)
        {
            if(article != null)
            {
                return new
                {
                    article.id,
                    article.author,
                    article.samllcontents,
                    article.ishot,
                    article.seecount,
                    article.isnew,
                    article.isshow,
                    article.imgurl,
                    article.sort,
                    article.title,
                    article.typeid,
                    article.typetext,
                    article.created
                };
            }

            return null;
        }

        public static dynamic GetCaseProjection(CaseInfo info)
        {
            if (info != null)
            {
                return new
                {
                    info.id,
                    info.imgurl,
                    infoullimgurl = WebConfigs.UrlPrefix + info.imgurl,
                    info.title,
                    info.sort,
                    info.link,
                    info.seecount,
                    info.prizecount,
                    info.created,
                    info.typeid,
                    info.typetext
                };
            }

            return null;
        }
    }
}