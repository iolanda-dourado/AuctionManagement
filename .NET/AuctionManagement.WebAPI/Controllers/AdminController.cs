using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {
    
    /// <summary>
    /// Admin Controller for handling admin tasks
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase {

        /// <summary>
        /// Method to get to the admin controller area
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() {
            return Ok("You have accessed the Admin controller.");
        }
    }
}
