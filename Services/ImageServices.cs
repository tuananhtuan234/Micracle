using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ImageServices: IImagesServices
    {
        private readonly IImagesRepository _imagesRepository;

        public ImageServices(IImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }

        public async Task<bool> AddImages(string ImageUrl)
        {
            try
            {
                Image newImages = new Image()
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = ImageUrl,
                };
                await _imagesRepository.AddImages(newImages);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Image>> GetAllImages()
        {
            return await _imagesRepository.GetAllImages();
        }

        public async Task<Image> GetImagesById(string id)
        {
            return await _imagesRepository.GetImageByid(id);
        }
    }
}
