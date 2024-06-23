using AutoShop.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace AutoShop.Tests.IntegrationTests
{
    public class AdminControllerIntegrationTests : IClassFixture<IntegretionTestWebAppFactory>
    {
        private IntegretionTestWebAppFactory _factory;
        private HttpClient _client;
        public AdminControllerIntegrationTests(IntegretionTestWebAppFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-Test-role", "admin");
        }
        [Fact]
        public async Task PostAddAutos_ShouldReturnOK()
        {
            // Arrange
            // Act
            var response = await AddAuto();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Index_ShouldReturnOneAuto()
        {
            //Arrange
            await AddAuto();

            //Act
            var response = await _client.GetAsync("/Admin");

            //Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
            Assert.Matches(@"<td>\s*TestAuto\s*</td>", content);
            Assert.Matches(@"<td>\s*Some info\s*</td>", content);
            Assert.Matches(@"<td>\s*1000\s*</td>", content);
        }

        public async Task<HttpResponseMessage> AddAuto()
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../", "images", "kia.jpg");
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            var multipartContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(imageBytes);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            multipartContent.Add(fileContent, "Image", imagePath);
            multipartContent.Add(new StringContent("TestAuto"), "Name");
            multipartContent.Add(new StringContent("Some info"), "Info");
            multipartContent.Add(new StringContent("1000"), "Price");

            var response = await _client.PostAsync("/Admin/Add", multipartContent);

            return response;
        }
    }
}
