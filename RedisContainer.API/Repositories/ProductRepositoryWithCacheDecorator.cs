using RedisContainer.API.Models;
using RedisContainer.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisContainer.API.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {

        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _database = _redisService.GetDb(2);
        }


        public async Task<Product> CreateAsync(Product product)
        {
           var newPrd =await _productRepository.CreateAsync(product);
            if (await _database.KeyExistsAsync(productKey)) { 

                var json = JsonSerializer.Serialize(newPrd);
                _database.HashSet(productKey, newPrd.Id, json);
            }


                return newPrd;
        }

        public async  Task<List<Product>> GetAsync()
        {
            if(! await _database.KeyExistsAsync(productKey))
            {
               return await LoadCacheFromDb(productKey);
            }

            var products =await GetDataFromCache(productKey);
            
            return products;

        }

        public async  Task<Product> GetByIdAsync(int id)
        {
            if (await _database.KeyExistsAsync(productKey))
            {
                var prd = await _database.HashGetAsync(productKey, id);
                return prd.HasValue ? JsonSerializer.Deserialize<Product>(prd) : null;
            }

            var products = await LoadCacheFromDb(productKey);
            return products.FirstOrDefault(x => x.Id == id);
        }

        private async Task<List<Product>> LoadCacheFromDb(string ListKey)
        {
            var products = await _productRepository.GetAsync();

            foreach (var product in products)
            {
                var json = JsonSerializer.Serialize(product);
                _database.HashSet(ListKey, product.Id, json);
            }
            return products;
        }


        private async Task<List<Product>> GetDataFromCache(string ListKey)
        {
            var products = new List<Product>();
            var cachedProducts = _database.HashGetAll(ListKey);
            foreach (var product in cachedProducts)
            {
                var productObj = JsonSerializer.Deserialize<Product>(product.Value);
                products.Add(productObj);
            }
            return products;
        }


    }
}
