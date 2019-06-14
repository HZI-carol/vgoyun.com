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
    public class CommentStorage : SqlStorageBase, ICommentStorage
    {
        public CommentStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> ICommentStorage.InsertAsync(Comment model)
        {
            string sql = @"INSERT INTO comment (contents, ipaddress, created) values (@contents, @ipaddress, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        /// <summary>
        /// 允许分页排序的字段信息列表
        /// </summary>
        readonly IEnumerable<AliasColumn> m_PagedOrderColumns = new List<AliasColumn>
        {
            new AliasColumn("ipaddress"),
            new AliasColumn("created")
        };


        Task<PagedList<Comment>> ICommentStorage.GetPagedListAsync(int page, int pageSize, string ipAddress, string created, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("created"), m_PagedOrderColumns);
            string sql = string.Format(@"SELECT * FROM (SELECT ROW_NUMBER() OVER({0}) AS row , * FROM comment
                                where (@ipaddress = '' or ipaddress = @ipaddress) and (@created = '' or created >= @created)) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                              SELECT COUNT(1) FROM comment where (@ipaddress = '' or ipaddress = @ipaddress) and (@created = '' or created >= @created) ;", order);

            return base.GetPagedListAsync<Comment>(sql, new { page, pageSize, ipAddress, created = base.ParseSqlDateTime(created) });
        }
    }
}