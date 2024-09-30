using Repositories.Data.Entity;
using Repositories.Data;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly ApplicationDbContext _context;

        public CartProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartProduct> AddCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
            await _context.SaveChangesAsync();
            return cartProduct;
        }
        public async Task<CartProduct> UpdateCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Update(cartProduct); 
            await _context.SaveChangesAsync(); 
            return cartProduct; 
        }
        public async Task<bool> RemoveCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Remove(cartProduct); 
            await _context.SaveChangesAsync(); 
            return true; 
        }

    }

}
