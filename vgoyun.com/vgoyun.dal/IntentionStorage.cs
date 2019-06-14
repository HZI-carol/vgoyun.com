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
    public class IntentionStorage : SqlStorageBase, IIntentionStorage
    {
        public IntentionStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> IIntentionStorage.InsertAsync(Intention model)
        {
            string sql = @"INSERT INTO intention (name, phone, intention, useragent, remark, created) values (@name, @phone, @intention, @useragent, @remark, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        /// <summary>
        /// 允许分页排序的字段信息列表
        /// </summary>
        readonly IEnumerable<AliasColumn> m_PagedOrderColumns = new List<AliasColumn>
        {
            new AliasColumn("id"),
            new AliasColumn("phone"),
            new AliasColumn("created"),
        };

        Task<PagedList<Intention>> IIntentionStorage.GetPagedListAsync(int page, int pageSize, string keyword, int[] intentions, string orderBy)
        {
            var order = base.GetSortInfo(orderBy, new SortInfo("id"), m_PagedOrderColumns);
            var intentionSqls = new List<string>();
            if (intentions != null && intentions.Any())
            {
                foreach (var item in intentions.Distinct())
                {
                    intentionSqls.Add($" intention like '%{item}%' ");
                }
            }
            var condition = SqlClauses.Create()
                .AndCaluse($"({string.Join(" or ", intentionSqls)})", intentionSqls.Any())
                .AndCaluse("( name like @keyword or phone like @keyword)", !string.IsNullOrEmpty(keyword)).ToSql();
            string sql = $@"SELECT * FROM (SELECT ROW_NUMBER() OVER({order}) AS row, *  FROM intention {condition} 
                            ) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                     SELECT COUNT(1) FROM intention {condition} ;";

            return base.GetPagedListAsync<Intention>(sql, new { page, pageSize, keyword = base.ToLikeParam(keyword) });
        }
    }
}
