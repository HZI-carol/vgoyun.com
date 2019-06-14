using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.Toolkit.Data.Dapper;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface IIntentionStorage
    {
        Task<int> InsertAsync(Intention model);

        Task<PagedList<Intention>> GetPagedListAsync(int page, int pageSize, string keyword, int[] intentions, string orderBy);
    }
}
