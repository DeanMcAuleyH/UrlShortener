using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortenerApi.Interfaces
{
    public interface IUrlService
    {
        string GenerateShortUrl(string id);
        public bool IsValidUrl(string url);
        public string GetFullShortUrl(string shortened);
        public string AppendHttps(string url);
    }
}
