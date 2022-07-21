using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Models;
using UrlShortenerApi.Models.Context;

namespace UrlShortenerApi.Repos
{
    public class UrlRepo : IUrlRepo<Url>
    {
        readonly UrlDbContext _urlDb;
        readonly IUrlService _urlService;

        public UrlRepo(UrlDbContext urlDbContext, IUrlService urlService)
        {
            _urlDb = urlDbContext;
            _urlService = urlService;
        }
        public async Task<string> Add(Url entity)
        {
            string invalidUrl = "Invalid Url";
            if (!_urlService.IsValidUrl(entity.OriginalUrl))
            {
                return invalidUrl;
            }

            if (!_urlDb.Urls.Any(url => url.OriginalUrl == entity.OriginalUrl))
            {
                await _urlDb.Urls.AddAsync(entity);
                await _urlDb.SaveChangesAsync();

                string id = entity.Id.ToString();
                entity.ShortUrl = _urlService.GenerateShortUrl(id);

                //TODO  Handle if error occurs updating
                //      check if short url being inserted matches pattern
                await Update(entity);

                return _urlService.GetFullShortUrl(entity.ShortUrl);
            }
            else
            {
                Url result = await _urlDb.Urls.FirstOrDefaultAsync(x => x.OriginalUrl == entity.OriginalUrl);
                return _urlService.GetFullShortUrl(result.ShortUrl);
            }
        }

        public void Delete(Url entity)
        {
            _urlDb.Urls.Remove(entity);
        }

        public async Task<Url> Get(int id)
        {
            Url result = await _urlDb.Urls.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public IEnumerable<Url> GetAll()
        {
            return _urlDb.Urls;
        }

        public async Task<string> GetOriginalUrl(string ShortUrl)
        {

            Url result = await _urlDb.Urls.FirstOrDefaultAsync(x => x.ShortUrl == ShortUrl);
            return !string.IsNullOrEmpty(result?.OriginalUrl) ? _urlService.AppendHttps(result.OriginalUrl) : null;
        }

        public async Task Update(Url entity)
        {
            Url currentUrl = await Get(entity.Id);
            if (currentUrl != null)
            {
                _urlDb.Update(entity);
                await _urlDb.SaveChangesAsync();
            }
            
        }
    }
}
