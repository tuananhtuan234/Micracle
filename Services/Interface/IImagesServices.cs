using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IImagesServices
    {
        Task<List<Image>> GetAllImages();
        Task<Image> GetImagesById(string id);
        Task<bool> AddImages(string ImageUrl);
    }
}
