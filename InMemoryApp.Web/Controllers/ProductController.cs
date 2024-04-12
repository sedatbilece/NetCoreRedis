using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {

            if (_memoryCache.Get<Product>("prd")==null)
            {
                // get from db
                var p = new Product() { Id = 1,Name = "Product 1", Price = 100 };


                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                    SlidingExpiration = TimeSpan.FromSeconds(10),
                    Priority = CacheItemPriority.High,
                };

                _memoryCache.Set<Product>("prd", p, options);

                ViewBag.prd = p;
            }
            else
            {
                ViewBag.prd = _memoryCache.Get<Product>("prd");
            }


            return View();
        }

        public IActionResult Show()
        {
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("date")))
            {
                // get from db
                var value = DateTime.Now.ToString();

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(60),
                    SlidingExpiration = TimeSpan.FromSeconds(10),
                    Priority = CacheItemPriority.High,
                };

                _memoryCache.Set<string>("date",value , options);

                ViewBag.Date = value;
            }
            else
            {
                ViewBag.Date = _memoryCache.Get<string>("date");
            }
            
          
            return View();
        }
    }
}
