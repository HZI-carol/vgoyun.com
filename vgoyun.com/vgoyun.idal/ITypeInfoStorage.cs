using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface ITypeInfoStorage
    {
        Task<int> InsertAsync(TypeInfo model);

        Task<int> UpdateAsync(TypeInfo model);

        Task<IEnumerable<TypeInfo>> GetListAsync(int parentid);
    }
}
