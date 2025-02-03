using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {

    /// <summary>
    /// User Controller for handling user tasks
    /// </summary>
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {

        /// <summary>
        /// Method to get to the user controller area
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get() {
            return Ok("You have accessed the User controller.");
        }
    }
}
