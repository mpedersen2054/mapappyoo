using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mapapp.Controllers
{
    public class FriendgroupController : Controller
    {
        // GET: /friendgroups
        [HttpGet]
        [Route("friendgroups")]
        public IActionResult ShowUserFriendgroups()
        {
            return View("UserFriendgroups");
        }

        // GET: /friendgroups/all
        [HttpGet]
        [Route("friendgroups/all")]
        public IActionResult ShowAllFriendgroups()
        {
            return View("AllFriendgroups");
        }

        // GET: /friendgroups/new
        [HttpGet]
        [Route("friendgroups/new")]
        public IActionResult ShowFriendgroupNew()
        {
            return View("FriendgroupNew");
        }

        // GET: /friendgroups/{friendgroupId}/login
        [HttpGet]
        [Route("friendgroups/{fgString}/login")]
        public IActionResult ShowFriendgroupLogin(string fgString)
        {
            return View("FriendgroupLogin");
        }

        // GET: /friendgroups/{friengroupId}
        [HttpGet]
        [Route("friendgroups/{fgString}")]
        public IActionResult ShowFriendgroup(string fgString)
        {
            return View("Friendgroup");
        }
        
    }
}
