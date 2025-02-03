using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Data {

    /// <summary>
    /// Represents the database context for authentication.
    /// </summary>
    public class AuthDbContext : IdentityDbContext {

        /// <summary>
        /// Initializes a new instance of the AuthDbContext class.
        /// </summary>
        /// <param name="options">The DbContextOptions to be used.</param>
        public AuthDbContext(DbContextOptions options) : base(options) {
        }
    }
}
