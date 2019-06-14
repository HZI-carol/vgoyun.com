using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.Toolkit.Data.Dapper;
using vgoyun.idal.models;
using vgoyun.idal;

namespace vgoyun.dal
{
    public class CaseInfoStorage : SqlStorageBase, ICaseInfoStorage
    {
        public CaseInfoStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> ICaseInfoStorage.InsertAsync(CaseInfo model)
        {
            string sql = @"INSERT INTO [case] (typeid, imgurl, title, link, seecount, prizecount, sort, created) values (@typeid, @imgurl, @title, @link, @seecount, @prizecount, @sort, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        Task<int> ICaseInfoStorage.UpdateAsync(CaseInfo model)
        {
            string sql = @"UPDATE [case] SET typeid = @typeid, imgurl = @imgurl, title = @title, link = @link, seecount = @seecount, prizecount = @prizecount, sort = @sort WHERE id = @id;";

            return base.ExecuteAsync(sql, model);
        }

        async Task<CaseInfo> ICaseInfoStorage.GetAsync(int id)
        {
            string sql = @"SELECT TOP 1 *, ISNULL((select top 1 text from type where typeid = c.typeid), '') as typetext  FROM [case] c WHERE id = @id";
            var list = await base.QueryAsync<CaseInfo>(sql, new { id });

            return list.FirstOrDefault();
        }

        /// <summary>
        /// 允许分页排序的字段信息列表
        /// </summary>
        readonly IEnumerable<AliasColumn> m_PagedOrderColumns = new List<AliasColumn>
        {
            new AliasColumn("title"),
            new AliasColumn("typeid"),
            new AliasColumn("sort"),
            new AliasColumn("seecount"),
            new AliasColumn("prizecount"),
            new AliasColumn("created")
        };

        Task<PagedList<CaseInfo>> ICaseInfoStorage.GetPagedListAsync(int page, int pageSize, string title, int typeid, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("id"), m_PagedOrderColumns);
            string sql = string.Format(@"SELECT * FROM (SELECT ROW_NUMBER() OVER({0}) AS row ,* , ISNULL((select top 1 text from type where typeid = c.typeid), '') as typetext 
                                        FROM [case] c where (@title = '' or title like @title) and (@typeid <= 0 or typeid = @typeid)) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                              SELECT COUNT(1) FROM [case] where (@title = '' or title like @title) and (@typeid <= 0 or typeid = @typeid);", order);

            return base.GetPagedListAsync<CaseInfo>(sql, new
            {
                page,
                pageSize,
                title = base.ToLikeParam(title),
                typeid
            });
        }

        Task<int> ICaseInfoStorage.DeleteAsync(int id)
        {
            string sql = @"DELETE FROM [case] WHERE id = @id";

            return base.ExecuteAsync(sql, new { id });
        }
    }
}
