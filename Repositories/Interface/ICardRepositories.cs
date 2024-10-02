﻿using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface ICardRepositories
    {
        Task DeleteProducts(string ProductId);
        Task<List<Product>> GetAllProducts(string? searchterm);
        Task<bool> AddProducts(Product product);
        Task<bool> UpdateProducts(Product product);
        Task<Product> GetProductsById(string ProductId);
      
    }
}
