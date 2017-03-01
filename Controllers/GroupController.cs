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
        [Route("groups/{gid}/login")]
        public IActionResult ShowGroupLogin(int gid)
        {
            
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
                //hash the password and add it if the group admin decided to create a password
                if(groupModel.Password != null){
                    PasswordHasher<GroupViewModel> Hasher = new PasswordHasher<GroupViewModel>();
                    newGroup.Password = Hasher.HashPassword(groupModel, groupModel.Password);
                }

                _context.Groups.Add(newGroup);
                _context.SaveChanges();
                Group currentGroup = _context.Groups.Last();
                //add created group to user session so they remain logged in
                HttpContext.Session.SetInt32(currentGroup.GroupId.ToString(), 1);


                return RedirectToAction("ShowGroup", currentGroup.GroupId);
            }
            return RedirectToAction("AddGroup");
        }
    }
}
