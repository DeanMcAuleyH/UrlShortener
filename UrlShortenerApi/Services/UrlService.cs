using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UrlShortenerApi.Interfaces;

namespace UrlShortenerApi.Services
{
    public class UrlService : IUrlService
    {
        private readonly string _baseUrl = "https://localhost:44364/";
        public string GenerateShortUrl(string id)
        {
            byte[] encodedId = System.Text.ASCIIEncoding.ASCII.GetBytes(id);
            return Convert.ToBase64String(encodedId).Replace("=", "");
        }

        public bool IsValidUrl(string url)
        {
            var validUrlRegex = new Regex("^(http[s]?:\\/\\/(www\\.)?|ftp:\\/\\/(www\\.)?|www\\.){1}([0-9A-Za-z-\\.@:%_\\+~#=]+)+((\\.[a-zA-Z]{2,3})+)(/(.)*)?(\\?(.)*)?");
            return validUrlRegex.IsMatch(url);
        }

        public string GetFullShortUrl(string shortened)
        {
            return $"{_baseUrl}{shortened}";
        }

        public string AppendHttps(string url)
        {
            string https = "https://";
            string http = "http://";
            if (!url.StartsWith(https) && !url.StartsWith(http))
                return $"{https}{url}";
            else
                return url;
        }
    }
}
