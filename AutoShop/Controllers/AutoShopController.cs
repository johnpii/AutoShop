using AutoShop.Interfaces;
using AutoShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AutoShop.Controllers
{
    public class AutoShopController : Controller
    {
        private readonly IAutoRepository _autoRepo;
        private readonly IImageRepository _imageRepo;
        //private readonly IDistributedCache _cache;

        //public AutoShopController(IAutoRepository autoRepo, IImageRepository imageRepo, IDistributedCache cache)
        //{
        //    _autoRepo = autoRepo;
        //    _imageRepo = imageRepo;
        //    _cache = cache;
        //}
        public AutoShopController(IAutoRepository autoRepo, IImageRepository imageRepo)
        {
            _autoRepo = autoRepo;
            _imageRepo = imageRepo;
        }
        public async Task<IActionResult> Index()
        {
            var images = _imageRepo.GetAllImages();
            var autos = await _autoRepo.GetAllAutos();

            //List<AutoModelWithIdAndImage> result = null;
            //var allAutos = await _cache.GetStringAsync("allAutos");
            //if (allAutos != null) result = JsonSerializer.Deserialize<List<AutoModelWithIdAndImage>>(allAutos);
            //if (result == null)
            //{
            //    var images = _imageRepo.GetAllImages();
            //    var autos = await _autoRepo.GetAllAutos();
            //    if (autos != null && images != null)
            //    {
            //        Console.WriteLine("Извлечение из базы данных");
            //        result = new List<AutoModelWithIdAndImage>();
            //        for (int i = 0; i < autos.Count; i++)
            //        {
            //            var autoModel = new AutoModelWithIdAndImage
            //            {
            //                Id = autos[i].Id,
            //                Name = autos[i].Name,
            //                Info = autos[i].Info,
            //                Photo = images[i].Photo,
            //                Price = autos[i].Price
            //            };
            //            result.Add(autoModel);
            //        }
            //        allAutos = JsonSerializer.Serialize(result);
            //        await _cache.SetStringAsync("allAutos", allAutos, new DistributedCacheEntryOptions
            //        {
            //            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            //        });
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Извлечение из кэша");
            //}
            //return View(result);

            List<AutoModelWithIdAndImage> result = new List<AutoModelWithIdAndImage>();
            for (int i = 0; i < autos.Count; i++)
            {
                var autoModel = new AutoModelWithIdAndImage
                {
                    Id = autos[i].Id,
                    Name = autos[i].Name,
                    Info = autos[i].Info,
                    Photo = images[i].Photo,
                    Price = autos[i].Price
                };
                result.Add(autoModel);
            }
            return View(result);
        }
    }
}
