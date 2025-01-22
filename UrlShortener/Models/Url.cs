namespace UrlShortener.Models
{
    /// <summary>
    /// Represents an URL entity used for URL shortening services
    /// </summary>
    public class Url
    {
        /// <summary>
        /// Id of the URL
        /// </summary>
        public int Id { get; set; }
       
       /// <summary>
       /// the original URL string that is being schortened
       /// </summary>
        public string BasicUrl { get; set; }

        /// <summary>
        /// the code generated for the original URL 
        /// this is the value used to redirect to the original URL.
        /// </summary>
        public string GeneratedCode { get; set; }

        /// <summary>
        ///  the date and time when the short URL created 
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
