using global::MicroServicioLogin.Application.Interfaces;
using MicroServicioAuth.Application.DTOs;
using MicroServicioLogin.Application.Interfaces;
using System.Security.Claims;
using System.Web.Http;

namespace MicroServicioAuth.Presentation.Controllers
{

        [RoutePrefix("api/login")]
        public class LoginController : ApiController
        {
            private readonly IAuthService _authService;

            public LoginController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpPost]
            [Route("authenticate")]
            public IHttpActionResult Authenticate(string username, string password)
            {
                if (_authService.ValidateUser(username, password))
                {
                    return Ok("Login Successful");
                }
                return Unauthorized();
            }

            [HttpPost]
            [Route("register")]
            public IHttpActionResult Register(string username, string password, string email)
            {
                _authService.RegisterUser(username, password, email);
                return Ok("User Registered Successfully");
            }

            [HttpPost]
            [Route("login")]
            public IHttpActionResult Login([FromBody] LoginRequest loginRequest)
            {
                if (!_authService.ValidateUser(loginRequest.Username, loginRequest.Password))
                {
                    return Unauthorized();
                }

                var token = _authService.GenerateToken(loginRequest.Username);
                return Ok(new { Token = token });
            }

            [Authorize]
            [HttpGet]
            [Route("protected")]
            public IHttpActionResult GetProtectedData()
            {
                return Ok("Este es un endpoint protegido. ¡Autorización JWT exitosa!");
            }

    }

}
