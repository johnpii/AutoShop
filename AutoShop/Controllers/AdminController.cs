using AutoShop.Interfaces;
using AutoShop.Models;
using AutoShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace AutoShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAutoRepository _autoRepo;
        private readonly IImageRepository _imageRepo;

        public AdminController(IAutoRepository autoRepo, IImageRepository imageRepo)
        {
            _autoRepo = autoRepo;
            _imageRepo = imageRepo;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _autoRepo.GetAllAutos());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AutoModel autoModel)
        {
            if (ModelState.IsValid)
            {
                var collection = _imageRepo.GetImagesCollection();
                if (collection != null)
                {
                    Auto auto = new Auto();
                    auto.Name = autoModel.Name;
                    auto.Info = autoModel.Info;
                    auto.Price = autoModel.Price;
                    _autoRepo.AddAuto(auto);
                    int lastId = _autoRepo.MaxId();
                    var imageName = lastId + ".jpg";
                    Image image = new Image();
                    image.FileName = imageName;
                    using (var memoryStream = new MemoryStream())
                    {
                        autoModel.Image.CopyTo(memoryStream);
                        image.Photo = memoryStream.ToArray();
                        collection.InsertOne(image);
                    }
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Error = "Сервис не доступен ";
                    return StatusCode(503);
                }
            }
            else
            {
                ViewBag.Error = "Некорректные данные ! ";
                return View();
            }
        }

        public IActionResult Delete(int id)
        {
            var collection = _imageRepo.GetImagesCollection();
            Auto auto = new Auto();
            auto = _autoRepo.FindById(id);
            string imageName = id + ".jpg";
            var image = collection.Find(p => p.FileName == imageName).FirstOrDefault();
            ViewBag.Image = image.Photo;
            return View(auto);
        }

        [HttpPost]
        public IActionResult DeleteAuto(int id)
        {
            if (ModelState.IsValid)
            {
                var collection = _imageRepo.GetImagesCollection();
                if (collection != null)
                {
                    Auto auto = new Auto();
                    auto = _autoRepo.FindById(id);
                    _autoRepo.DeleteAuto(auto);
                    string imageName = id + ".jpg";
                    collection.FindOneAndDeleteAsync(p => p.FileName == imageName);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Error = "Сервис не доступен ";
                    return StatusCode(503);
                }
            }
            else
            {
                ViewBag.Error = "Некорректные данные ! ";
                return View("Delete");
            }
        }

        public IActionResult Edit(int id)
        {
            var collection = _imageRepo.GetImagesCollection();
            Auto auto = new Auto();
            auto = _autoRepo.FindById(id);
            AutoModelWithId autoModelWithId = new AutoModelWithId();
            autoModelWithId.Id = id;
            autoModelWithId.Name = auto.Name;
            autoModelWithId.Info = auto.Info;
            autoModelWithId.Price = auto.Price;
            string imageName = id + ".jpg";
            var image = collection.Find(p => p.FileName == imageName).FirstOrDefault();
            ViewBag.Image = image.Photo;
            return View(autoModelWithId);

        }

        [HttpPost]
        public IActionResult Edit(AutoModelWithId autoModel)
        {
            if (ModelState.IsValid)
            {
                var collection = _imageRepo.GetImagesCollection();
                if (collection != null)
                {
                    Auto auto = new Auto();
                    auto = _autoRepo.FindById(autoModel.Id);
                    auto.Name = autoModel.Name;
                    auto.Info = autoModel.Info;
                    auto.Price = autoModel.Price;
                    _autoRepo.Save();
                    string imageName = autoModel.Id + ".jpg";
                    Image image = collection.Find(p => p.FileName == imageName).FirstOrDefault();
                    if (autoModel.Image != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            autoModel.Image.CopyTo(memoryStream);
                            image.Photo = memoryStream.ToArray();
                            collection.FindOneAndReplaceAsync(p => p.FileName == imageName, image);
                        }
                    }
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Error = "Сервис не доступен ";
                    return StatusCode(503);
                }
            }
            else
            {
                ViewBag.Error = "Некорректные данные ! ";
                return View();
            }
        }
    }
}
