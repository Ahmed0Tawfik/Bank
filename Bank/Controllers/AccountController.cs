using Bank.DTO;
using Bank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Account> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly SignInManager<Account> _SigninManager;

        public AccountController(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Account> signinManager)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _SigninManager = signinManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateAccount(CreateUserDTO request)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                var response = new
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList(),
                    Message = "Invalid data",
                    IsSuccess = false
                };
                return BadRequest(response); // Return BadRequest for validation errors
            }

            // Create a new account object
            var newAccount = new Account
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
            };

            // Attempt to create the account
            var result = await _UserManager.CreateAsync(newAccount, request.Password);

            if (!result.Succeeded)
            {
                var response = new
                {
                    Errors = result.Errors.Select(e => e.Description).ToList(),
                    Message = "Account creation failed",
                    IsSuccess = false
                };
                return BadRequest(response);
            }

            // Return success response
            var successResponse = new
            {
                Message = "Account created successfully",
                IsSuccess = true,
                Payload = new
                {
                    newAccount.UniqueId,
                    newAccount.FirstName,
                    newAccount.LastName,
                    newAccount.UserName,
                    newAccount.Balance
                }
            };

            return Ok(successResponse);
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> Login(LoginDTO request)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                var response = new
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList(),
                    Message = "Invalid data",
                    IsSuccess = false
                };
                return BadRequest(response); // Return BadRequest for validation errors
            }
            // Attempt to sign in the user
            var result = await _SigninManager.PasswordSignInAsync(request.Username, request.Password, true, false);
            if (!result.Succeeded)
            {
                var response = new
                {
                    Errors = new List<string> { "Invalid Username or Password" },
                    Message = "Login failed",
                    IsSuccess = false
                };
                return BadRequest(response);
            }

            var user = await _UserManager.FindByNameAsync(request.Username);
            // Return success response
            var successResponse = new
            {
                Message = "Login successful",
                IsSuccess = true,
                Payload = new
                {
                    user.UniqueId,
                    user.FirstName,
                    user.LastName,
                    user.Balance
                }
            };
            return Ok(successResponse);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _SigninManager.SignOutAsync();
            var response = new
            {
                Message = "Logout successful",
                IsSuccess = true
            };
            return Ok(response);
        }

        

    }
}
