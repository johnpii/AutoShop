using System.Net;
using Xunit;

namespace AutoShop.Tests.IntegrationTests
{
    public class AutoShopControllerIntegrationTests : IClassFixture<IntegretionTestWebAppFactory>
    {
        private IntegretionTestWebAppFactory _factory;
        private HttpClient _client;
        public AutoShopControllerIntegrationTests(IntegretionTestWebAppFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Index_ShouldReturnNothing()
        {
            //Arrange

            //Act
            var response = await _client.GetAsync("/AutoShop");

            //Assert
            Assert.True(response.StatusCode.Equals(HttpStatusCode.OK));
            Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        }
    }
}
