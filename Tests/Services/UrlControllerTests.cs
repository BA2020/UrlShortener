using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using UrlShortener.Models;
using UrlShortener.Services;

namespace Tests.Services
{
    public class UrlControllerTests
    {
        private readonly Mock<IUrlService> _mockUrlService;
        private readonly UrlController _controller;
        public UrlControllerTests()
        {
            _mockUrlService = new Mock<IUrlService>();
            _controller = new UrlController(_mockUrlService.Object);
        }

        /// <summary>
        /// Test the controller redirection action : Ensures the the controller behaves correctly by redirecting valid short links
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RedirectToUrl_ShouldRedirectToOriginalUrl()
        {
            // Arrange
            var shortLink = "abcde";
            var originalUrl = "http://google.com";

            _mockUrlService.Setup(service => service.GetOriginalUrlByCodeAsync(shortLink))
                       .ReturnsAsync(new Url { BasicUrl = originalUrl });
  
            // Act
            var result = await _controller.RedirectToOriginal(shortLink) as RedirectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(originalUrl, result.Url);
        }
    }
}
