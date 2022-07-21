using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using UrlShortenerApi.Interfaces;
using UrlShortenerApi.Models;
using UrlShortenerApi.Models.Context;
using UrlShortenerApi.Repos;
using Xunit;

namespace UrlShortenerTests
{
    //Example of unit tests on possible points of failure in repo class
    public class UrlShortenerTest
    {
        public Mock<IUrlRepo<Url>> mock = new Mock<IUrlRepo<Url>>();
        public Mock<IUrlService> mockService = new Mock<IUrlService>();
        private DbContextOptions<UrlDbContext> dbContextOptions;
        private readonly string _baseUrl = "https://localhost:44364/";

        public UrlShortenerTest()
        {
            string dbName = $"UrlShortenerDb_{DateTime.Now.ToFileTimeUtc()}";
            dbContextOptions = new DbContextOptionsBuilder<UrlDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var context = new UrlDbContext(dbContextOptions))
            {
                context.Urls.Add(new Url { Id = 1, OriginalUrl = "www.OriginalUrl.com", ShortUrl = "wrt" });
                context.SaveChanges();
            }
        }

        [Fact]
        public async void GetOriginalUrlTest()
        {
            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);
                
                mockService.Setup(p => p.AppendHttps("www.OriginalUrl.com")).Returns("https://www.OriginalUrl.com").Verifiable();

                string result = await um.GetOriginalUrl("wrt");
                Assert.Equal("https://www.OriginalUrl.com", result);

            }
        }
        
        [Fact]
        public async void GetOriginalUrlTest_Fail()
        {
            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);

                string result = await um.GetOriginalUrl("ShortUrl/rt");
                Assert.Null(result);
            }
        }

        [Fact]
        public async void AddTest()
        {
            var testUrl = new Url
            {
                OriginalUrl = "www.new.com"
            };

            mockService.Setup(p => p.GenerateShortUrl("2")).Returns("lk").Verifiable();
            mockService.Setup(p => p.IsValidUrl("www.new.com")).Returns(true).Verifiable();
            mockService.Setup(p => p.GetFullShortUrl("lk")).Returns($"{_baseUrl}lk").Verifiable();

            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);
                string result = await um.Add(testUrl);
                Assert.Equal($"{_baseUrl}lk", result);

            }
        }

        [Fact]
        public async void AddTest_Existing()
        {
            var testUrl = new Url
            {
                OriginalUrl = "www.OriginalUrl.com"
            };
            mockService.Setup(p => p.IsValidUrl("www.OriginalUrl.com")).Returns(true).Verifiable();
            mockService.Setup(p => p.GetFullShortUrl("wrt")).Returns($"{_baseUrl}wrt").Verifiable();

            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);
                string result = await um.Add(testUrl);
                Assert.Equal($"{_baseUrl}wrt", result);

            }
        }

        [Fact]
        public async void AddTestFail_Invalid()
        {
            var testUrl = new Url
            {
                OriginalUrl = "www.new.com"
            };
            mockService.Setup(p => p.IsValidUrl("www.new/com")).Returns(false).Verifiable();

            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);
                string result = await um.Add(testUrl);
                Assert.Equal("Invalid Url", result);
            }
        }

        [Fact]
        public async void AddTestFail_()
        {
            var testUrl = new Url
            {
                OriginalUrl = "www.new.com"
            };
            mockService.Setup(p => p.IsValidUrl("www.new/com")).Returns(false).Verifiable();

            using (var context = new UrlDbContext(dbContextOptions))
            {
                UrlRepo um = new UrlRepo(context, mockService.Object);
                string result = await um.Add(testUrl);
                Assert.Equal("Invalid Url", result);
            }
        }
    }
}
