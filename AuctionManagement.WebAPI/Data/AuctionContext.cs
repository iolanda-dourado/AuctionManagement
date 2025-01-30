using AuctionManagement.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Data {
    public class AuctionContext : AuthDbContext {

        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        public DbSet<Item> Items {  get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Sale> Sales { get; set; }
    }
}
