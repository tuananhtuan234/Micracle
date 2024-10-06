﻿using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using Repositories.Interface;
using Repositories.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CardRepository : ICardRepositories
    {
        private readonly ApplicationDbContext _context;

        public CardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts(string? searchterm)
        {
            if (searchterm != null)
            {
                return await _context.Products.Where(p => p.ProductName.Contains(searchterm)).ToListAsync();
            }
            else
            {
                return await _context.Products.ToListAsync();
            }
        }

        public async Task<bool> AddProducts(Product product)
        {
            _context.Products.Add(product);
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateProducts(Product product)
        {
            _context.Products.Update(product);
             await _context.SaveChangesAsync() ;
        }

        public async Task<Product> GetProductsById(string ProductId)
        {
            return await _context.Products.FirstOrDefaultAsync(sc => sc.Id.Equals(ProductId));
        }

        public async Task DeleteProducts(string ProductId)
        {
            Product product = await GetProductsById(ProductId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
     
    }
}
