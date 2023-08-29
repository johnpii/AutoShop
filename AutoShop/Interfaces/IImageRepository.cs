using AutoShop.Models;
using MongoDB.Driver;

namespace AutoShop.Interfaces
{
    public interface IImageRepository
    {
        List<Image> GetAllImages();
        IMongoCollection<Image> GetImagesCollection();
    }
}
