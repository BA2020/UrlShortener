namespace UrlShortener.Data
{
    using Microsoft.EntityFrameworkCore;
    using UrlShortener.Models;

    /// <summary>
    /// Represents the Entity Framework Core database context for the UrLShortener application.
    /// This context is used to interact with the database.
    /// </summary>
    public class UrlShortenerDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShortenerContext"/> class.
        /// </summary>
        /// <param name="options">The options to configure the database context.</param>
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the database set for the <see cref="Url"/> entities.
        /// This represents the "Urls" table in the database.
        /// </summary>
        public DbSet<Url> Urls { get; set; }

    }
}
