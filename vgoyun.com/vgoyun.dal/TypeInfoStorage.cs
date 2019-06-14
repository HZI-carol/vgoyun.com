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
    public class TypeInfoStorage : SqlStorageBase, ITypeInfoStorage
    {
        public TypeInfoStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> ITypeInfoStorage.InsertAsync(TypeInfo model)
        {
            string sql = @"INSERT INTO type (parentid, text, sort, created) values (@parentid, @text, @sort, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        Task<int> ITypeInfoStorage.UpdateAsync(TypeInfo model)
        {
            string sql = @"UPDATE type SET parentid = @parentid, text = @text, sort = @sort WHERE typeid = @typeid;";

            return base.ExecuteAsync(sql, model);
        }

        Task<IEnumerable<TypeInfo>> ITypeInfoStorage.GetListAsync(int parentid)
        {
            string sql = @"SELECT * FROM type where parentid = @parentid";

            return base.QueryAsync<TypeInfo>(sql, new { parentid });
        }
    }
}
