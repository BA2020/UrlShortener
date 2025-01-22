using UrlShortener.Models;

namespace UrlShortener.Services
{
    /// <summary>
    /// This interface defines the contract for the operations in the UrlService
    /// </summary>
    public interface IUrlService
    {
        Task<List<Url>> GetAllShotedlUrlsAsync();
        Task CreateShortedUrlAsync(Url url);
        Task<Url> GetOriginalUrlByCodeAsync(string code);

    }
}
