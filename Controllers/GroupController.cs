using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mapapp.Controllers
{
    public class GroupController : Controller
    {
        // private MyContext _context;
        private MyContext _context;
        public GroupController(MyContext context)
        {
            _context = context;
        }
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
        [Route("groups/{gid:int}")]
        public IActionResult ShowGroup(int gid)
        {
            List<Group> currentGroup = _context.Groups.Where(g => g.GroupId == gid)
                .Include(a => a.Admin)
                .Include(u => u.Users)
                .Include(l => l.Locations).ToList();

            return View("Group", currentGroup);
        }
        

        //post: new location
        [HttpPostAttribute]
        [RouteAttribute("addGroup")]
        public IActionResult AddGroup(GroupViewModel groupModel)
        {
            if (ModelState.IsValid)
            {
                
                User currentUser = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault();

                Group newGroup = new Group{
                    GroupName = groupModel.GroupName,
                    Description = groupModel.Description,
                    AdminId = (int)HttpContext.Session.GetInt32("user"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Admin = currentUser
                };

                if(groupModel.Password != null){
                    newGroup.Password = groupModel.Password;
                }

                _context.Groups.Add(newGroup);
                _context.SaveChanges();
                Group currentGroup = _context.Groups.Last();


                return RedirectToAction("ShowGroup", currentGroup.GroupId);
            }
            return RedirectToAction("AddGroup");
        }
    }
}
