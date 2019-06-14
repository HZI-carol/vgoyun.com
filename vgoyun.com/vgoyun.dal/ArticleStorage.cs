using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.Toolkit.Data.Dapper;
using vgoyun.idal.models;
using vgoyun.idal;
using Dapper;

namespace vgoyun.dal
{
    public class ArticleStorage : SqlStorageBase, IArticleStorage
    {
        public ArticleStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> IArticleStorage.InsertAsync(Article model)
        {
            string sql = @"INSERT INTO article (typeid, author, imgurl, title, samllcontents, contents, ishot, isnew, isshow, sort, seecount, created)
                    values (@typeid, @author, @imgurl, @title, @samllcontents, @contents, @ishot, @isnew, @isshow , @sort, @seecount, @created);";

            return base.ExecuteAsync(sql, model);
        }

        Task<int> IArticleStorage.UpdateAsync(Article model)
        {
            string sql = @"UPDATE article SET typeid = @typeid, author = @author, imgurl = @imgurl , title = @title, samllcontents = @samllcontents
                , contents = @contents, ishot = @ishot, isnew = @isnew, isshow = @isshow, sort = @sort, seecount = @seecount, created = @created WHERE id = @id;";

            return base.ExecuteAsync(sql, model);
        }

        async Task<Article> IArticleStorage.GetAsync(int id)
        {
            string sql = @"SELECT TOP 1 *, ISNULL((select top 1 text from type where typeid = a.typeid), '')  as typetext FROM article a WHERE id = @id";
            var list = await base.QueryAsync<Article>(sql, new { id });

            return list.FirstOrDefault();
        }

        Task<ArticleViewData> IArticleStorage.GetViewDataAsync(int id)
        {
            return base.ConnectAsync<ArticleViewData>(async conn =>
            {
                var data = new ArticleViewData();

                string sql = $@"SELECT TOP 1 *, ISNULL((select top 1 text from type where typeid = a.typeid), '')  as typetext FROM article a  ";
                var model = await conn.QueryFirstOrDefaultAsync<Article>(sql + "  WHERE id = @id", new { id });
                if(model != null)
                {
                    var param = new { model.created, model.typeid };
                    data.Previous = await conn.QueryFirstOrDefaultAsync<Article>(sql + "  WHERE created < @created and typeid = @typeid", param);
                    data.Next = await conn.QueryFirstOrDefaultAsync<Article>(sql + "  WHERE created > @created and typeid = @typeid", param);
                }
                sql = $@"SELECT TOP 6 *, ISNULL((select top 1 text from type where typeid = a.typeid), '')  as typetext FROM article a  ";
                //热点
                data.HotList = await conn.QueryAsync<Article>(sql + " WHERE ishot = 1 order by created desc");
                //最新
                data.NewList = await conn.QueryAsync<Article>(sql + " WHERE isnew = 1 order by created desc");
                //推荐
                data.ShowList = await conn.QueryAsync<Article>(sql + " WHERE isshow = 1 order by created desc");

                return data;
            });
        }

        /// <summary>
        /// 允许分页排序的字段信息列表
        /// </summary>
        readonly IEnumerable<AliasColumn> m_PagedOrderColumns = new List<AliasColumn>
        {
            new AliasColumn("title"),
            new AliasColumn("typeid"),
            new AliasColumn("sort"),
            new AliasColumn("author"),
            new AliasColumn("seecount"),
            new AliasColumn("created")
        };

        Task<PagedList<Article>> IArticleStorage.GetPagedListAsync(int page, int pageSize, string title, int typeid, int ishot, int isnew, int isshow, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("id"), m_PagedOrderColumns);
            string sql = string.Format(@"SELECT * FROM (SELECT ROW_NUMBER() OVER({0}) AS row , *, ISNULL((select top 1 text from type where typeid = a.typeid), '') as typetext
                                  FROM article a where (@title = '' or title like @title) and (@typeid <= 0 or typeid = @typeid) and (@ishot <= -1 or ishot = @ishot) 
                                  and (@isnew <= -1 or isnew = @isnew) and (@isshow <= -1 or isshow = @isshow)) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                           SELECT COUNT(1) FROM article where (@title = '' or title like @title) and (@typeid <=0 or typeid = @typeid) and (@ishot <= -1 or ishot = @ishot) 
                                and (@isnew <= -1 or isnew = @isnew) and (@isshow <= -1 or isshow = @isshow);", order);

            return base.GetPagedListAsync<Article>(sql, new
            {
                page,
                pageSize,
                title = base.ToLikeParam(title),
                typeid,
                ishot,
                isnew,
                isshow
            });
        }

        Task<int> IArticleStorage.DeleteAsync(int id)
        {
            string sql = @"DELETE FROM article WHERE id = @id";

            return base.ExecuteAsync(sql, new { id });
        }
    }
}
