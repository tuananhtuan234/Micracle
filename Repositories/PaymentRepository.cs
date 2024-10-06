using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> GetPaymentById(string paymnetId)
        {
            return await _context.Payments.FirstOrDefaultAsync(sc => sc.Id.Equals(paymnetId));  
        }

        public async Task<List<Payment>> GetAllPayment(string searchterm)
        {
            if (searchterm != null)
            {
                return await _context.Payments.Where(sc => sc.Method.Contains(searchterm)).ToListAsync();
            }
            else
            {
                return await _context.Payments.ToListAsync();
            }
        }

      
    }
}
