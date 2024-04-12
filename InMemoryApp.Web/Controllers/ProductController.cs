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

            _memoryCache.Remove("date");
            
            return View();
        }

        public IActionResult Show()
        {
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("date")))
            {
                // get from db
                var value = DateTime.Now.ToString();
                _memoryCache.Set<string>("date",value , DateTimeOffset.Now.AddSeconds(20));
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
