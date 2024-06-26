﻿using Microsoft.EntityFrameworkCore;
using RedisContainer.API.Models;
using RedisContainer.Cache;

namespace RedisContainer.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly RedisService _redisService;


        public ProductRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            

           return await _context.Products.FindAsync(id);
        }
    }
}
