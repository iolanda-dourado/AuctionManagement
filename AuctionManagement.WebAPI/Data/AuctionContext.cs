using AuctionManagement.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Data {

    /// <summary>
    /// Represents the database context for the auction management system.
    /// This class inherits from AuthDbContext and provides access to the Items, Categories, and Sales entities.
    /// </summary>
    public class AuctionContext : AuthDbContext {

        /// <summary>
        /// Initializes a new instance of the AuctionContext class.
        /// </summary>
        /// <param name="options"></param>
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }


        /// <summary>
        /// Initializes a new instance of the AuctionContext class.
        /// </summary>
        public DbSet<Item> Items {  get; set; }


        /// <summary>
        /// Initializes a new instance of the AuctionContext class.
        /// </summary>
        public DbSet<Category> Categories { get; set; }


        /// <summary>
        /// Initializes a new instance of the AuctionContext class.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }
    }
}
