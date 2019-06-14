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
    public class UserStorage : SqlStorageBase, IUserStorage
    {
        public UserStorage(string connectionKey)
            : base(connectionKey)
        {
        }

        Task<int> IUserStorage.InsertAsync(User model)
        {
            string sql = @" if not exists (select 1 from [user] where username = @username)
                    INSERT INTO [user] (username, nickname, password, status, created) values (@username, @nickname, @password, @status, GETDATE());";

            return base.ExecuteAsync(sql, model);
        }

        Task<int> IUserStorage.UpdateAsync(User model)
        {
            string sql = @"UPDATE [user] SET nickname = @nickname, password = @password, status = @status WHERE id = @id;";

            return base.ExecuteAsync(sql, model);
        }

        async Task<User> IUserStorage.GetAsync(int id)
        {
            string sql = @"SELECT TOP 1 * FROM [user] WHERE id = @id";
            var list = await base.QueryAsync<User>(sql, new { id });

            return list.FirstOrDefault();
        }

        Task<PagedList<User>> IUserStorage.GetPagedListAsync(int page, int pageSize)
        {
            string sql = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id DESC) AS row , id, username, nickname, password, status, created
                                        FROM [user]) t where t.row between (@page - 1) * @pageSize + 1 AND @page * @pageSize ;
                              SELECT COUNT(1) FROM [user] ;";

            return base.GetPagedListAsync<User>(sql, new { page, pageSize });
        }

        Task<int> IUserStorage.DeleteAsync(int id)
        {
            string sql = @"DELETE FROM [user] WHERE id = @id";

            return base.ExecuteAsync(sql, new { id });
        }

        async Task<User> IUserStorage.GetAsync(string username)
        {
            string sql = @"SELECT TOP 1 * FROM [user] WHERE username = @username";
            var list = await base.QueryAsync<User>(sql, new { username });

            return list.FirstOrDefault();
        }
    }
}
