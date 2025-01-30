using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Data {
    public class AuthDbContext : IdentityDbContext {
        public AuthDbContext(DbContextOptions options) : base(options) {
        }
    }
}
