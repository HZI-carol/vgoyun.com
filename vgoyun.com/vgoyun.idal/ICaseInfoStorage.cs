using LF.Toolkit.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface ICaseInfoStorage
    {
        Task<int> InsertAsync(CaseInfo model);

        Task<int> UpdateAsync(CaseInfo model);

        Task<PagedList<CaseInfo>> GetPagedListAsync(int page, int pageSize, string title, int typeid, string orderBy);

        Task<CaseInfo> GetAsync(int id);

        Task<int> DeleteAsync(int id);
    }
}
