using AutoShop.Controllers;
using AutoShop.Entities;
using AutoShop.Interfaces;
using AutoShop.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoShop.Tests.UnitTests
{
    public class AdminControllerUnitTests
    {
        private readonly Mock<IAutoRepository> _autoRepoMock;
        private readonly Mock<IImageRepository> _dbMock;

        private readonly AutoShopController _autoShopController;

        public AdminControllerUnitTests()
        {
            _autoRepoMock = new Mock<IAutoRepository>();
            _dbMock = new Mock<IImageRepository>();

            _autoShopController = new AutoShopController(_autoRepoMock.Object, _dbMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithAutoModels()
        {
            // Arrange
            var autos = new List<Auto>
        {
            new Auto { Id = 1, Name = "Auto 1", Info = "Info 1", Price = 1000 },
            new Auto { Id = 2, Name = "Auto 2", Info = "Info 2", Price = 2000 }
        };

            var images = new List<Image>
        {
            new Image { Id = "1", FileName = "image1.jpg", Photo = new byte[0] },
            new Image { Id = "2", FileName = "image2.jpg", Photo = new byte[0] }
        };

            _autoRepoMock.Setup(repo => repo.GetAllAutos()).ReturnsAsync(autos);
            _dbMock.Setup(db => db.GetAllImages()).Returns(images);

            // Act
            var result = await _autoShopController.Index();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<AutoModelWithIdAndImage>>(viewResult.Model);

            Assert.Equal(2, model.Count);
            Assert.Equal(autos[0].Id, model[0].Id);
            Assert.Equal(autos[0].Name, model[0].Name);
            Assert.Equal(autos[0].Info, model[0].Info);
            Assert.Equal(images[0].Photo, model[0].Photo);
            Assert.Equal(autos[0].Price, model[0].Price);

            Assert.Equal(autos[1].Id, model[1].Id);
            Assert.Equal(autos[1].Name, model[1].Name);
            Assert.Equal(autos[1].Info, model[1].Info);
            Assert.Equal(images[1].Photo, model[1].Photo);
            Assert.Equal(autos[1].Price, model[1].Price);
            _autoRepoMock.VerifyAll();
            _dbMock.VerifyAll();
        }
    }
}