using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mapapp.Controllers
{
    public class UserController : Controller
    {
        // POST: /login
        [HttpPost]
        [Route("login")]
        public IActionResult PostLogin()
        {
            return View();
        }

        // POST: /login
        [HttpPost]
        [Route("register")]
        public IActionResult PostRegister()
        {
            return View();
        }
    }
}
