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
    public class BannerStorage : SqlStorageBase, IBannerStorage
    {
        public BannerStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> IBannerStorage.InsertAsync(Banner model)
        {
            //SQLServer => SELECT SCOPE_IDENTITY();
            string sql = @"INSERT INTO banner (title, image_url, link_url, sort, type, created) values (@title, @image_url, @link_url, @sort, @type, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        Task<int> IBannerStorage.UpdateAsync(Banner model)
        {
            string sql = @"UPDATE banner SET title = @title, image_url = @image_url, link_url = @link_url, sort = @sort, type = @type WHERE id = @id;";

            return base.ExecuteAsync(sql, model);
        }

        Task<Banner> IBannerStorage.GetAsync(int id)
            => base.QueryFirstOrDefaultAsync<Banner>(@"SELECT TOP 1 * FROM banner  WHERE id = @id", new { id });

        Task<IEnumerable<Banner>> IBannerStorage.GetListAsync(int count, int type, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("id"), m_PagedOrderColumns);
            return base.QueryAsync<Banner>($"select top {count} * from banner WHERE type = @type {order}", new { type });
        }

        /// <summary>
        /// 允许分页排序的字段信息列表
        /// </summary>
        readonly IEnumerable<AliasColumn> m_PagedOrderColumns = new List<AliasColumn>
        {
            new AliasColumn("id"),
            new AliasColumn("sort"),
            new AliasColumn("type"),
            new AliasColumn("created"),
        };

        Task<PagedList<Banner>> IBannerStorage.GetPagedListAsync(int page, int pageSize, int type, string title, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("id"), m_PagedOrderColumns);
            var condition = SqlClauses.Create().AndCaluse("type = @type", type > 0).AndCaluse("title like @title", !string.IsNullOrEmpty(title)).ToSql();

            string sql = $@"SELECT * FROM (SELECT ROW_NUMBER() OVER({order}) AS row, *  FROM banner {condition}
                            ) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                     SELECT COUNT(1) FROM banner {condition} ;";
            return base.GetPagedListAsync<Banner>(sql, new { page, pageSize, type, title = base.ToLikeParam(title) });
        }

        Task<int> IBannerStorage.DeleteAsync(int id)
            => base.ExecuteAsync(@"DELETE FROM banner  WHERE id = @id ", new { id });
    }
}
