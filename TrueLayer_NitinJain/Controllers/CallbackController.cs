using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrueLayer_NitinJain.Interfaces;

namespace TrueLayer_NitinJain.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {

        private readonly IAuthService _authService;

        public CallbackController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// This will be called by truelayer when asked for AuthorizationUrl, respose is code & scope
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Callback()
        {
            string code = Request.Form["code"];
            _authService.ExchangeCodeForAccessToken(code);
            return Redirect("/");
        }
    }
}