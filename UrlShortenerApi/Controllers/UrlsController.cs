using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Models;
using UrlShortenerApi.Models.Context;

namespace UrlShortenerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private IUrlRepo<Url> _repo;
        public UrlsController(IUrlRepo<Url> repo)
        {
            _repo = repo;
        }

        // GET: api/Urls/ShortUrl
        [HttpGet("/{ShortUrl}", Name = "Get")]
        public async Task<ActionResult> GetUrl(string ShortUrl)
        {
            string url = await _repo.GetOriginalUrl(ShortUrl);
            if (!string.IsNullOrEmpty(url))
                return Redirect(url);
            
            return NotFound();
        }

        
        [HttpPost]
        public async Task<ActionResult<Url>> PostUrl(Url url)
        {
            string ShortUrl = await _repo.Add(url);

            if (!string.IsNullOrEmpty(ShortUrl))
            {
                Url urlHelper = new Url()
                {
                    ShortUrl = ShortUrl
                };
                return urlHelper;
            }

            return NotFound();
        }
    }
}
