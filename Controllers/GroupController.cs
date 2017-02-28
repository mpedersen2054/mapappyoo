using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mapapp.Controllers
{
    public class GroupController : Controller
    {
        // GET: /groups
        [HttpGet]
        [Route("groups")]
        public IActionResult ShowUsergroups()
        {
            return View("UserGroups");
        }

        // GET: /groups/all
        [HttpGet]
        [Route("groups/all")]
        public IActionResult ShowAllGroups()
        {
            return View("AllGroups");
        }

        // GET: /groups/new
        [HttpGet]
        [Route("groups/new")]
        public IActionResult ShowGroupNew()
        {
            return View("GroupNew");
        }

        // GET: /groups/{friendgroupId}/login
        [HttpGet]
        [Route("groups/{gString}/login")]
        public IActionResult ShowGroupLogin(string gString)
        {
            return View("GroupLogin");
        }

        // GET: /groups/{friengroupId}
        [HttpGet]
        [Route("groups/{gString}")]
        public IActionResult ShowGroup(string gString)
        {
            return View("Group");
        }
        
    }
}
