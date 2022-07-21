using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortenerApi.Interfaces
{
    public interface IUrlRepo<TEntity>
    {
        Task<TEntity> Get(int id);
        Task<string> GetOriginalUrl(string ShortUrl);
        Task<string> Add(TEntity entity);
        void Delete(TEntity entity);
        Task Update(TEntity entity);
    }
}
