using System;
using System.Collections.Generic;
using System.Linq;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public IActionResult ShowUserGroups()
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            ViewBag.userGroups = 
                _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user"))
                    .Include(u => u.Groups)
                        .ThenInclude(g => g.Organization)
                        .ThenInclude(o => o.Members);

            ViewBag.otherGroups = 
                _context.Groups.Include(g => g.Members)
                    .Where(g => (g.Members.FindIndex(users => users.MemberId == (int)HttpContext.Session.GetInt32("user"))) == -1);

            return View("UserGroups");
        }

        // GET: /groups/new
        [HttpGet]
        [Route("groups/new")]
        public IActionResult ShowGroupNew()
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            return View("GroupNew");
        }

        // GET: /groups/{friendgroupId}/login
        [HttpGet]
        [Route("groups/{gid}/login")]
        public IActionResult ShowGroupLogin(int gid)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            
            Group currentGroup = _context.Groups.Where(g => g.GroupId == gid).SingleOrDefault();

            //check to see if the logged in user is already in the group
            int? loggedIn = (int)HttpContext.Session.GetInt32(gid.ToString());
            if((int)loggedIn == 1){
                return RedirectToAction("ShowGroup", gid);
            }
            // if there is no password, set that group to the user's session
            else if(currentGroup.Password == null){
                HttpContext.Session.SetInt32(gid.ToString(), 1);
                return RedirectToAction("ShowGroup", gid);
            }
            // otherwise display the group login page
            else{
                return View("GroupLogin", currentGroup);
            }
        }

        // POST: log into a group
        [HttpPostAttribute]
        [RouteAttribute("groups/{gid:int}/userlogin")]
        public IActionResult GroupLogin(GroupLoginViewModel groupLoginModel, int gid)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            Group currentGroup = _context.Groups.Where(g => g.GroupId == gid).SingleOrDefault();
            if(currentGroup.Password == null){
                if(groupLoginModel.Password == null){
                    HttpContext.Session.SetInt32(gid.ToString(), 1);
                    return RedirectToAction("ShowGroup", gid);
                }else{
                    ViewBag.error = "Your password was incorrect.";
                    return View("GroupLogin", currentGroup);
                }
            }else{
                var Hasher = new PasswordHasher<Group>();
                    
                if(0 != Hasher.VerifyHashedPassword(currentGroup, currentGroup.Password, groupLoginModel.Password))
                {
                    HttpContext.Session.SetInt32(gid.ToString(), 1);
                    return RedirectToAction("ShowGroup", gid);
                }
                else{
                    ViewBag.error = "Your password was incorrect.";
                    return View("GroupLogin", currentGroup);
                }
            }
        }

        // GET: /groups/{friengroupId}
        [HttpGet]
        [Route("groups/{gid}")]
        public IActionResult ShowGroup(int gid)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            Group currentGroup = _context.Groups.Where(g => g.GroupId == gid)
                .Include(g => g.Admin)
                .Include(g => g.GroupLocs)
                .Include(g => g.Members)
                    .ThenInclude(m => m.Member)
                .SingleOrDefault();

            User uzer = _context.Users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("user"));

            if (uzer == null)
            {
                return RedirectToAction("ShowLogin", "User");
            }

            if (currentGroup == null)
            {
                return RedirectToAction("AddGroup");
            }

            ViewBag.Group = currentGroup;
            ViewBag.CurrentUser = uzer;
            return View("Group");
        }
        

        //post: new group
        [HttpPostAttribute]
        [RouteAttribute("addGroup")]
        public IActionResult AddGroup(GroupViewModel groupModel)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            if (ModelState.IsValid)
            {   
                Group newGroup = new Group{
                    GroupName = groupModel.GroupName,
                    Description = groupModel.Description,
                    AdminId = (int)HttpContext.Session.GetInt32("user"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                //hash the password and add it if the group admin decided to create a password
                if(groupModel.Password != null){
                    PasswordHasher<GroupViewModel> Hasher = new PasswordHasher<GroupViewModel>();
                    newGroup.Password = Hasher.HashPassword(groupModel, groupModel.Password);
                }

                _context.Groups.Add(newGroup);
                _context.SaveChanges();
                Group currentGroup = _context.Groups.Last();
                //add current user to group Members
                UserGroup addMem = new UserGroup{
                    MemberId = (int)HttpContext.Session.GetInt32("user"),
                    OrganizationId = (int)currentGroup.GroupId
                };
                _context.UserGroups.Add(addMem);
                _context.SaveChanges();
                //add created group to user session so they remain logged in
                HttpContext.Session.SetInt32(currentGroup.GroupId.ToString(), 1);

                return RedirectToAction("ShowGroup", new { gid = currentGroup.GroupId });
            }
            return View("GroupNew", groupModel);
        }

        //post: new group
        [HttpGetAttribute]
        [RouteAttribute("joinGroup/{groupId}")]
        public IActionResult JoinGroup(int groupId)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            User currentUser = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault();

            UserGroup isThere = _context.UserGroups.Where(ug => ug.MemberId == currentUser.UserId && ug.OrganizationId == groupId).SingleOrDefault();

            if (isThere == null)
            {
                UserGroup newUgroup = new UserGroup{
                    MemberId = currentUser.UserId,
                    OrganizationId = groupId
                };
                _context.UserGroups.Add(newUgroup);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowGroup", new {gid = groupId});
        }
    }
}
