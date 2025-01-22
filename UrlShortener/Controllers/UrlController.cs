using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class UrlController : Controller
    {
        /* UrlService */
       private readonly IUrlService _urlService;

        /* Constructor to inject the UrlService to handle the CRUD operations ("add") */
        public UrlController(IUrlService urlService) => _urlService = urlService;

        /** Get: Url/Index - Show form to enter an URL **/
        public IActionResult Index()
        {
            return View();
        }

        /** POST: /Url/ShortUrl - Handles the URL shortening process: Add the shortened Url to DB **/
        [HttpPost]
        public async Task<IActionResult> AddShortUrl([FromForm]string url)
        {
            if (ModelState.IsValid)
            {
                /* Check if the given URL is valide or not, */
                if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    return BadRequest("Invalid URL");
                }

                /* Generate a random string with maximum length of 5 => short code */
                string shortCode = UrlHelper.GenerateRandomString();

                /* create a new Url object */
                Url newUrl = new()
                {
                    BasicUrl = url,
                    GeneratedCode = shortCode,
                    CreationDate = DateTime.Now,
                };

                /* Save the new shorted URL in DB using UrlService */
                await _urlService.CreateShortedUrlAsync(newUrl);

                /* Generate the shorted URL */
                string shortenedUrl =  UrlHelper.GenerateShortUrl(shortCode);

                var model = new ShortenedUrlViewModel
                {
                    ShortUrl = shortenedUrl,
                    OriginalUrl = url
                };
                ViewBag.model = model;


                // Return the Index view with the shortened URL
                return View("Index");
            }

            // If validation fails, return the default view with the entred URL
            return View("Index");
        }

        /** Redirect to original Address by getting the shortCode as parameter from the given URL in Browser **/
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            /** Check if the code has value or not **/
            if (string.IsNullOrWhiteSpace(shortCode))
                return NotFound();

            /** Get the correspndant basic Url by code**/
            var shortUrl = await _urlService.GetOriginalUrlByCodeAsync(shortCode);
            if (shortUrl == null)
            {
                return NotFound();
            }

            // redirect to the correct website
            return Redirect(shortUrl.BasicUrl);
        }


        // GET: Urls - List all shorted URLs
        public async Task<IActionResult> History()
        {
            var urls = await _urlService.GetAllShotedlUrlsAsync();
            return View(urls); // Return the list of URLs to the View
        }
    }
}
