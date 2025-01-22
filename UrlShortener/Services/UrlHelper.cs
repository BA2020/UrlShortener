namespace UrlShortener.Services
{
    /// <summary>
    ///  a helper class used to define utilities (generate random string and short URL)
    /// </summary>
    public static class UrlHelper
    {
        private const string BaseDomain = "https://www.focusmr.eu";
        /// <summary>
        /// Generates a random string with a length of up to the specified maxLength.
        /// </summary>
        /// <param name="maxLength">The maximum length of the generated string. Default is 5.</param>
        /// <returns>A randomly generated string </returns>
        public static string GenerateRandomString(int maxLength = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, maxLength)
                .Select(s => s[random.Next(s.Length)]).ToArray()); // Random length is 5
        }


        /// <summary>
        /// Generate a full short URL
        /// </summary>
        /// <param name="shortCode"></param>
        /// <returns> shorted URL as string </returns>
        public static string GenerateShortUrl(string shortCode)
        {
            return $"{BaseDomain}/{shortCode}";
        }
    }
}
