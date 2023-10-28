using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        //Inject repository into constructor
        private readonly IUserRepository userRepository;

        public AuthController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate the Incoming request: check if the fields are not empty
            if (!ValidateLoginAsync(loginRequest))
            {
                return BadRequest(ModelState);
            }
            //Check if user is authenticated:check the username && pasword
            var isAuthenticated = await userRepository.AuthenticateAsync(loginRequest.Username,loginRequest.Password);

            if (isAuthenticated)
            {
                //Generate a JWT Token
            }

            return BadRequest("Username or Password is Incorrect");
        }

        #region Private Methods
            private bool ValidateLoginAsync(Models.DTO.LoginRequest loginRequest)
            {
                if (loginRequest == null)
                {
                    ModelState.AddModelError(nameof(loginRequest),
                            $"The data fields are empty");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(loginRequest.Username))
                {
                    ModelState.AddModelError(nameof(loginRequest.Username),
                            $"{nameof(loginRequest.Username)} cannot be null, empty, or a whitespace");
                }
                if (string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    ModelState.AddModelError(nameof(loginRequest.Password),
                            $"{nameof(loginRequest.Password)} cannot be null, empty, or a whitespace");
                }
                if (ModelState.ErrorCount > 0)
                {
                    return false;
                }

                return true;
            }
        #endregion

    }
}
