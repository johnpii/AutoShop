using AutoShop.Interfaces;
using AutoShop.Models;
using MongoDB.Driver;

namespace AutoShop.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IMongoDatabase _mongoDB;
        public ImageRepository(IMongoDatabase mongoDB)
        {
            _mongoDB = mongoDB;
        }

        public List<Image> GetAllImages()
        {
            return _mongoDB.GetCollection<Image>("images").Find("{}").ToList();
        }

        public IMongoCollection<Image> GetImagesCollection()
        {
            return _mongoDB.GetCollection<Image>("images");
        }
    }
}
