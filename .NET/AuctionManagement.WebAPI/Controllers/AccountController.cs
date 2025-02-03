using AuctionManagement.WebAPI.AuthClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionManagement.WebAPI.Controllers {

    /// <summary>
    /// Controller for handling user authentication and authorization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {

        /// <summary>
        /// User manager and role manager for managing users and roles
        /// </summary>
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Constructor for the AccountController
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="configuration"></param>
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }


        /// <summary>
        /// Handles HTTP POST requests to register a new user.
        /// </summary>
        /// <param name="model">The registration model containing the username and password.</param>
        /// <returns>An IActionResult indicating the result of the registration attempt.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model) {
            var user = new IdentityUser { UserName = model.Username };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {
                return Ok(new {message = "User registered successfully."});
            }
            return BadRequest(result.Errors);
        }


        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="model">The UserRole model containing the username and role to assign.</param>
        /// <returns>An IActionResult indicating the result of the role assignment attempt.</returns>
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model) {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null) {
                return BadRequest("User not found");
            }

            var result = await userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded) {
                return Ok(new { message = "Role assigned successfully" });
            }

            return BadRequest(result.Errors);
        }


        /// <summary>
        /// Handles HTTP POST requests to authenticate a user.
        /// </summary>
        /// <param name="model">The Login model containing the username and password.</param>
        /// <returns>An IActionResult indicating the result of the authentication attempt.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model) {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                    )
                    );

                return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)});
            }

            return Unauthorized();
        }


        /// <summary>
        /// Handles HTTP POST requests to add a new role.
        /// </summary>
        /// <param name="role">The name of the role to add.</param>
        /// <returns>An IActionResult indicating the result of the role addition attempt.</returns>
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role) {
            if (!await roleManager.RoleExistsAsync(role)) {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded) {
                    return Ok(new { message = "Role added successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }

    }
}
