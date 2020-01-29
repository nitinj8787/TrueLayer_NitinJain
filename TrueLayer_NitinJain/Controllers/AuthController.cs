using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Redirects user to authorization endpoint. this will be called by AccountController method if token is not present
        /// </summary>
        [HttpGet("login")]
        public ActionResult LogIn()
        {
            Uri address = _authService.GetAuthorizationUri();
            return Redirect(address.ToString());
        }
       
    }
}