using Xunit;
using Moq;
using FluentAssertions;

using UrlShortener.Services;
using UrlShortener.Data;
using UrlShortener.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Tests.Services
{
    public class UrlServiceTests
    {
 
        private DbContextOptions<UrlShortenerDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<UrlShortenerDbContext>()
                .UseSqlite("Data Source=:memory:") // SQLite in-memory database : for testing purposes
                .Options;
        }

        private UrlService CreateMockedUrlService(UrlShortenerDbContext context)
        {
            context.Database.OpenConnection();      // Open connection to the in-memory database
            context.Database.EnsureCreated();      // Ensure database schema is created for testing

            // create a mocked ILogger : Verify that logging happens as expected: The mock allows you to simulate the behavior of the logger, 
            // and inspect whether the correct logging calls are made during execution.
            var loggerMock = new Mock<ILogger<Url>>();
            
            return new UrlService(context, loggerMock.Object);
        }

        /// <summary>
        /// Ensures that the generated code is unique
        /// </summary>
        [Fact]
        public void GenerateShortCode_ShouldReturnUniqueCode()
        {

            // Act
            var code1 = UrlHelper.GenerateRandomString();
            var code2 = UrlHelper.GenerateRandomString();

            // Assert
            Assert.NotEqual(code1, code2);
            Assert.True(code1.Length == 5);
            Assert.True(code2.Length == 5);
        }

        /// <summary>
        /// Validates that URLs are saved correctly.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateShortedUrlAsync_ShouldSaveToDatabase()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();

            using (var context = new UrlShortenerDbContext(options))
            {
                var service = CreateMockedUrlService(context);

                var testUrl = new Url
                {
                    BasicUrl = "http://google.com",
                    GeneratedCode = "abcde"
                };

                // Act
                await service.CreateShortedUrlAsync(testUrl);

                // Assert
                var savedUrl = await context.Urls.FirstOrDefaultAsync();
                Assert.NotNull(savedUrl);
                Assert.Equal("http://google.com", savedUrl.BasicUrl);
                Assert.Equal("abcde", savedUrl.GeneratedCode);
            }
        }

        /// <summary>
        /// Ensures that the correct URL is returned for a given code.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOriginalUrlByCodeAsync_ShouldReturnCorrectUrl()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();

            using (var context = new UrlShortenerDbContext(options))
            {
                var service = CreateMockedUrlService(context);

                var testUrl = new Url
                {
                    BasicUrl = "http://google.com",
                    GeneratedCode = "abcde"
                };

                await context.Urls.AddAsync(testUrl);
                await context.SaveChangesAsync();

                // Act
                var result = await service.GetOriginalUrlByCodeAsync("abcde");

                // Assert
                Assert.NotNull(result);
                Assert.Equal("http://google.com", result.BasicUrl);
            }
        }
    }
}
