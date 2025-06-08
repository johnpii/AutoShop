using AutoShop.Hubs;
using AutoShop.Interfaces;
using AutoShop.Models;
using AutoShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace AutoShop.Controllers
{
    /// <summary>
    /// Контроллер для действий, доступных только админу
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAutoRepository _autoRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IHubContext<AutoShopHub> _autoShopHub;

        public AdminController(IAutoRepository autoRepo, IImageRepository imageRepo, IHubContext<AutoShopHub> autoShopHub)
        {
            _autoRepo = autoRepo;
            _imageRepo = imageRepo;
            _autoShopHub = autoShopHub;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _autoRepo.GetAllAutos(cancellationToken));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AutoModel autoModel, CancellationToken cancellationToken)
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

                    var newAuto = new AutoModelWithIdAndImage
                    {
                        Id = auto.Id,
                        Name = auto.Name,
                        Info = auto.Info,
                        Photo = image.Photo,
                        Price = auto.Price
                    };
                    await _autoShopHub.Clients.All.SendAsync("newAuto", newAuto, cancellationToken);

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
        public async Task<IActionResult> DeleteAuto(int id, CancellationToken cancellationToken)
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

                    await _autoShopHub.Clients.All.SendAsync("deleteAuto", auto.Id, cancellationToken);
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
        public async Task<IActionResult> Edit(AutoModelWithId autoModel, CancellationToken cancellationToken)
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

                    var updatedAuto = new AutoModelWithIdAndImage
                    {
                        Id = auto.Id,
                        Name = auto.Name,
                        Info = auto.Info,
                        Photo = image.Photo,
                        Price = auto.Price
                    };

                    await _autoShopHub.Clients.All.SendAsync("updateAuto", updatedAuto, cancellationToken);
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
