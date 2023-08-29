using AutoShop.Interfaces;
using AutoShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace AutoShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IAutoRepository _autoRepo;
        private readonly IImageRepository _imageRepo;

        public CartController(IAutoRepository autoRepo, IImageRepository imageRepo)
        {
            _autoRepo = autoRepo;
            _imageRepo = imageRepo;
        }

        public List<Auto> GetAutosInCart()
        {
            var cart = HttpContext.Session.GetString($"cart_{Request.Cookies["userId"]}");

            if (string.IsNullOrEmpty(cart))
            {
                return new List<Auto>();
            }
            else
            {
                var listOfIds = JsonConvert.DeserializeObject<List<int>>(cart);
                var autos = new List<Auto>();
                foreach (var id in listOfIds)
                {
                    var auto = _autoRepo.FindById(id);
                    if (auto != null)
                    {
                        autos.Add(auto);
                    }
                }
                return autos;
            }
        }

        public void UpdateAutosInCart(List<Auto> autos)
        {
            var listOfIds = new List<int>();
            foreach (var auto in autos)
            {
                listOfIds.Add(auto.Id);
            }
            if (listOfIds.IsNullOrEmpty())
            {
                HttpContext.Session.Remove($"cart_{Request.Cookies["userId"]}");
            }
            else
            {
                var cart = JsonConvert.SerializeObject(listOfIds);
                HttpContext.Session.SetString($"cart_{Request.Cookies["userId"]}", cart);
            }
        }

        public IActionResult Index()
        {
            var collection = _imageRepo.GetImagesCollection();
            var autos = GetAutosInCart();
            List<Image> images = new List<Image>();
            foreach (var auto in autos)
            {
                string imageName = auto.Id + ".jpg";
                var image = collection.Find(a => a.FileName == imageName).FirstOrDefault();
                images.Add(image);
            }
            List<CartItem> result = new List<CartItem>();
            for (int i = 0; i < autos.Count; i++)
            {
                var autoModel = new CartItem
                {
                    Id = autos[i].Id,
                    Name = autos[i].Name,
                    Photo = images[i].Photo,
                    Price = autos[i].Price
                };
                result.Add(autoModel);
            }
            return View(result);
        }

        public IActionResult deleteItem(int Id)
        {
            var autos = GetAutosInCart();
            var autoToRemove = autos.FirstOrDefault(a => a.Id == Id);
            if (autoToRemove != null)
            {
                autos.Remove(autoToRemove);
                UpdateAutosInCart(autos);
            }
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public void addToCart(int Id)
        {
            var autos = GetAutosInCart();
            var autoToAdd = _autoRepo.FindById(Id);
            if (autoToAdd != null)
            {
                autos.Add(autoToAdd);
                UpdateAutosInCart(autos);
            }
        }
    }
}

