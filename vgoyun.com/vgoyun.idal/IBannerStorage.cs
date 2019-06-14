using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LF.Toolkit.Data.Dapper;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface IBannerStorage
    {
        Task<int> InsertAsync(Banner model);

        Task<int> UpdateAsync(Banner model);

        Task<PagedList<Banner>> GetPagedListAsync(int page, int pageSize, int type, string title, string orderBy);

        Task<IEnumerable<Banner>> GetListAsync(int count, int type, string orderBy);

        Task<Banner> GetAsync(int id);

        Task<int> DeleteAsync(int id);
    }
}
