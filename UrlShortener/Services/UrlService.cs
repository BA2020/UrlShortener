using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    /// <summary>
    /// A service class used to manipulate CRUD operations: Get, Add, Update, Delete - shorted url
    /// </summary>
    public class UrlService : IUrlService
    {
        private readonly UrlShortenerDbContext _context;
        private readonly ILogger<Url> _logger;

        /** Inject the DB context **/
        public UrlService(UrlShortenerDbContext context, ILogger<Url> logger) {
            _context = context;
            _logger = logger;
        } 


        /** Get all existing shorted URL **/
        public async Task<List<Url>> GetAllShotedlUrlsAsync()
        {
            return await _context.Urls.ToListAsync();
        }

        /** Add a new shorted URL to the database **/
        public async Task CreateShortedUrlAsync(Url shortedUrl)
        {
            // Validate the input
            if (shortedUrl == null)
                throw new ArgumentNullException(nameof(shortedUrl), "The shorted URL cannot be null.");
            try
            {
                // Add the shorted URL to the database context
                await _context.Urls.AddAsync(shortedUrl);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Log the success
                _logger.LogInformation("Shorted URL created successfully with code: {GeneratedCode}", shortedUrl.GeneratedCode);
            }
            catch (DbUpdateException dbEx)
            {
                // Log database-specific errors
                _logger.LogError(dbEx, "A database error occurred while creating the shorted URL with code: {GeneratedCode}", shortedUrl.GeneratedCode);

                // Optionally rethrow or wrap in a custom exception
                throw new InvalidOperationException("Failed to save the shorted URL to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Log any other unexpected errors
                _logger.LogError(ex, "An unexpected error occurred while creating the shorted URL with code: {GeneratedCode}", shortedUrl.GeneratedCode);
                throw; // Preserve the stack trace for debugging
            }
        }

        /** Get original URL by code **/
        public async Task<Url> GetOriginalUrlByCodeAsync(string code)
        {
            try
            {
                var url = await _context.Urls
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.GeneratedCode == code);
                
                // Log a warning if the URL was not found
                if (url == null)
                    _logger.LogWarning("No URL found for the provided code: {Code}", code);

                return url;
            }
            catch (Exception ex)
            {
                // Log the error for troubleshooting
                _logger.LogError(ex, "An error occurred while fetching the URL for code: {Code}", code);
                throw; // Re-throw the exception to preserve stack trace
            }
            
        }
    }
}
