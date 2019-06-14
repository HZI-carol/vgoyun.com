using LF.Toolkit.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface IUserStorage
    {
        Task<int> InsertAsync(User model);

        Task<int> UpdateAsync(User model);

        Task<PagedList<User>> GetPagedListAsync(int page, int pageSize);

        Task<User> GetAsync(int id);

        Task<User> GetAsync(string username);

        Task<int> DeleteAsync(int id);
    }
}
