using LF.Toolkit.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface ICommentStorage
    {
        Task<int> InsertAsync(Comment model);

        Task<PagedList<Comment>> GetPagedListAsync(int page, int pageSize, string ipAddress, string created, string orderBy);
    }
}
