using Repositories.Data.Entity;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class CategoryServices
    {
        private readonly ICategoryRepository _repository;

        public CategoryServices(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _repository.GetAllCategories();
        }

        public async Task<Category> GetCategoryById(string categoryId)
        {
            return await _repository.GetCagariesById(categoryId);
        }

        public async Task AddCategory(Category category)
        {
            await _repository.AddCategory(category);
        }
    }
}
