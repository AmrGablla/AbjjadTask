using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WepAPI.Interfaces;
using WepAPI.Models;
using WepAPI.Services;

namespace WepAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthenticationRequest request)
        {
            var user = _authService.AuthenticateAsync(request);
            return Ok(user);
        }


        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var user = _authService.RegisterAsync(request);
            return Ok(user);
        }
    }
}
