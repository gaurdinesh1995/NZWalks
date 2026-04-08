using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            // Validate model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create user object
            var user = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Username
            };

            // Create user in DB
            var createResult = await userManager.CreateAsync(user, dto.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }

            // Assign roles if provided
            if (dto.Roles != null && dto.Roles.Any())
            {
                var roleResult = await userManager.AddToRolesAsync(user, dto.Roles);

                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
            }

            // Success response
            return Ok(new
            {
                Message = "User registered successfully",
                Username = user.UserName,
                Roles = dto.Roles
            });
        }

        //Login controller
        //POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, dto.Password);
                if (checkPasswordResult)
                {
                    //Get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        // create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken=jwtToken
                        };
                        return Ok(response);
                    }


                }
            }

            return BadRequest("Username or Password incorrect");
        }
    }
}