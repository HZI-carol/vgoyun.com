using LF.Toolkit.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vgoyun.idal.models;

namespace vgoyun.idal
{
    public interface IArticleStorage
    {
        Task<int> InsertAsync(Article model);

        Task<int> UpdateAsync(Article model);

        Task<PagedList<Article>> GetPagedListAsync(int page, int pageSize, string title, int typeid, int ishot, int isnew, int isshow, string orderBy);

        /// <summary>
        /// 获取资讯详细页数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ArticleViewData> GetViewDataAsync(int id);

        Task<Article> GetAsync(int id);

        Task<int> DeleteAsync(int id);
    }
}
