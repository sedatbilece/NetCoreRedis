﻿using RedisContainer.API.Models;

namespace RedisContainer.API.Repositories
{
    public interface IProductRepository
    {

        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}
