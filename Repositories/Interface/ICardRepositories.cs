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
        Task<List<Product>> GetAllProducts();
        Task AddProducts(Product product);
        Task UpdateProducts(Product product);
        Task<Product> GetProductsById(string ProductId);
    }
}
