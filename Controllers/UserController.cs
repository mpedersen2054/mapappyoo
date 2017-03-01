using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mapapp.Controllers
{
    public class UserController : Controller
    {
        private MyContext _context;
        public UserController(MyContext context)
        {
            _context = context;
        }

        // GET: /users/login
        [HttpGet]
        [Route("users/login")]
        public IActionResult ShowLogin(){
            return View("ShowLogin");
        }

        // GET: /users/login
        [HttpGet]
        [Route("users/register")]
        public IActionResult ShowRegister(){
            return View("ShowRegister");
        }
        
        // POST: /login
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel loginModel){
            
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null)
            {
                if (ModelState.IsValid)
                {
                    User isInDB = _context.Users.Where(u => u.Username == loginModel.Username).SingleOrDefault();

                    if(isInDB == null){
                        ViewBag.error = "This Username isn't registered. Please register.";
                        return View("ShowRegister");
                    }
                    
                    HttpContext.Session.SetInt32("user", isInDB.UserId);
                    return RedirectToAction("Success");
                }
                return View("ShowLogin", loginModel);
            }
            return RedirectToAction("Success");
            
        }

        // POST: /login
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel regModel){
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null)
            {
                if (ModelState.IsValid)
                {
                    var isInDB = _context.Users.Where(u => u.Username == regModel.Username).SingleOrDefault();

                    if(isInDB != null){
                        ViewBag.error = "This email is already registered. Please log in.";
                        return View("ShowLogin");
                    }
                    else{
                        PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                        User user = new User{
                            Name = regModel.Name,
                            Username = regModel.Username,
                            Email = regModel.Email,
                            Password = Hasher.HashPassword(regModel, regModel.Password),
                            ProfilePic = regModel.ProfilePic,
                            Bio = regModel.Bio,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        User newUser = _context.Users.Last();
                    
                        HttpContext.Session.SetInt32("user", newUser.UserId);
                        return RedirectToAction("Success");
                    }
                    
                }
                return View("ShowRegister", regModel);
            }
            return RedirectToAction("Success");
        }

        [HttpGetAttribute]
        [RouteAttribute("success")]
        public IActionResult Success()
        {
            return RedirectToAction("ShowUserGroups");
        }
    }
}
