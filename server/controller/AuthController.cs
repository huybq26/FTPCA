using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using AccountServiceGroup;
using UserGroup;
using JwtServiceGroup;
using PasswordHashingServiceGroup;

namespace AuthControllerGroup
{
    // [EnableCors("MyPolicy")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        [Route("checkapi")]
        [HttpGet]
        [Authorize]
        public IActionResult Checkapi()
        {
            return Ok("This is the response from the Get method.");
        }
        // [EnableCors("MyPolicy")]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                bool result = await AccountService.registerUser(user.Username, user.PhoneNumber, user.Email, user.Name, PasswordHashingService.Encrypt(user.HashedPassword));
                if (result)
                {
                    return Ok(new { Message = "User " + user.Username + " created successfully", Data = user });
                }
                else
                {
                    return BadRequest(new { Message = "This user already exists", Data = user });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return BadRequest(new { Message = "Unexpected error", Data = user });
        }
        // [EnableCors("MyPolicy")]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest requestData)
        {
            string username = requestData.Username;
            string password = requestData.HashedPassword;
            try
            {
                bool result = await AccountService.loginUser(username, password);
                if (result)
                {
                    User user = await AccountService.GetUserFromUsername(username);
                    string token = JwtService.CreateToken(user);
                    return Ok(new
                    {
                        Message = "Log in user " + username + " successfully",
                        Data = new
                        {
                            User = user,
                            Token = token,
                        }
                    });
                }
                else
                {
                    return BadRequest(new { Message = "Wrong username or password", Data = requestData });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest(new { Message = "Unexpected error", Data = requestData });
        }
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
    }
}
