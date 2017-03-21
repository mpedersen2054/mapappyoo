using System;
using System.Collections.Generic;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace mapapp.Controllers
{
    public class LocationController : Controller
    {
        private MyContext _context;
        public LocationController(MyContext context)
        {
            _context = context;
        }

        // GET: /locations individual location
        [HttpGet]
        [Route("locations/{lid}")]
        public IActionResult ShowLocation(int lid)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            Location ratingVals = _context.Locations.Where(l => l.LocationId == lid).Include(r => r.Reviews).SingleOrDefault();

            
            int sumOfRatings = 0;
            int totalRatings = 0;
            foreach(var rating in ratingVals.Reviews){
                sumOfRatings += rating.Rating;
                totalRatings ++;
            }
            double locRating = (double)sumOfRatings / (double)totalRatings;
            ViewBag.LocRating = Math.Round(locRating, 2);

            Location currentLoc = _context.Locations.Where(l => l.LocationId == lid).Include(r => r.Reviews)
            .ThenInclude(u => u.Reviewer).SingleOrDefault();
            List<Review> reviewList = currentLoc.Reviews;
            ViewBag.Location = currentLoc;
            ViewBag.reviews = reviewList;
            return View("Location");
        }

        // GET: /locations/all
        [HttpGet]
        [Route("locations/all")]
        public IActionResult ShowAllLocations()
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            //this returns a list of all locations along with each locations creator user obj and a list of all groups connected to that location
            List<Location> allLocs = _context.Locations.OrderByDescending(l => l.CreatedAt).Include(u => u.Creator).Include(g => g.Groups).ToList();
            return View("AllLocations");
        }

        // GET: /locations/new
        [HttpGet]
        [Route("locations/groups/{groupId:int}/new")]
        public IActionResult ShowLocationNew(int groupId)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            ViewBag.gid = groupId;
            return View("LocationNew");
        }

        //post: new location
        [HttpPostAttribute]
        [RouteAttribute("locations/new")]
        public IActionResult AddLocation(LocationViewModel locModel)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            // System.Console.WriteLine("Name =>");
            // System.Console.WriteLine(locModel.Name);
            // System.Console.WriteLine("Street Adr =>");
            // System.Console.WriteLine(locModel.StreetAdr);
            // System.Console.WriteLine("City =>");
            // System.Console.WriteLine(locModel.City);
            // System.Console.WriteLine("State =>");
            // System.Console.WriteLine(locModel.State);
            // System.Console.WriteLine("Zip =>");
            // System.Console.WriteLine(locModel.Zip);
            // System.Console.WriteLine("Lat =>");
            // System.Console.WriteLine(locModel.Lat);
            // System.Console.WriteLine("Lng =>");
            // System.Console.WriteLine(locModel.Lng);
            // System.Console.WriteLine("GooglePlacesId =>");
            // System.Console.WriteLine(locModel.GooglePlacesId);
            // System.Console.WriteLine("GroupId =>");
            // System.Console.WriteLine(locModel.GroupId);

            if (ModelState.IsValid)
            {
                
                User currentUser = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault();
                Location newLoc = new Location{
                    Name = locModel.Name,
                    StreetAdr = locModel.StreetAdr,
                    City = locModel.City,
                    State = locModel.State,
                    Zip = locModel.Zip,
                    Lat = locModel.Lat,
                    Lng = locModel.Lng,
                    GooglePlacesId = locModel.GooglePlacesId,
                    CreatorId = (int)HttpContext.Session.GetInt32("user"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Creator = currentUser
                };

                _context.Locations.Add(newLoc);
                _context.SaveChanges();
                Location currentLoc = _context.Locations.Last();


                Review newReview = new Review{
                    RevieweeId = currentLoc.LocationId,
                    ReviewerId = currentUser.UserId,
                    Rating = locModel.Rating,
                    Message = locModel.Message,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Reviews.Add(newReview);
                _context.SaveChanges();

                GroupLocation newGLoc = new GroupLocation{
                    LocGroupId = locModel.GroupId,
                    GroupLocId = currentLoc.LocationId
                };

                _context.GroupLocations.Add(newGLoc);
                _context.SaveChanges();

                Review lastReview = _context.Reviews.Last();

                return RedirectToAction("ShowLocation", new { lid = currentLoc.LocationId });
            }
            ViewBag.Error = "Please use a valid address.";
            return RedirectToAction("ShowLocationNew", new {groupId = locModel.GroupId});
        }

        //post: new review
        [HttpPostAttribute]
        [RouteAttribute("addReview")]
        public IActionResult AddReview(ReviewViewModel reviewModel)
        {
            int? isInSession = HttpContext.Session.GetInt32("user");
            if(isInSession == null){
                return RedirectToAction("ShowLogin", "User");
            }
            Location currentLoc = _context.Locations.Where(l => l.LocationId == reviewModel.LocationId).SingleOrDefault();
            if (ModelState.IsValid)
            {
                
                User currentUser = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault();
                
                Review newReview = new Review{
                    RevieweeId = currentLoc.LocationId,
                    ReviewerId = currentUser.UserId,
                    Rating = reviewModel.Rating,
                    Message = reviewModel.Message,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Reviews.Add(newReview);
                _context.SaveChanges();

                return RedirectToAction("ShowLocation", new {lid = currentLoc.LocationId});
            }
            return RedirectToAction("ShowLocation", new {lid = currentLoc.LocationId});
        }

        [HttpGet]
        [RouteAttribute("getGroupLocations/{groupId}")]
        public JsonResult GetGroupLocations(int groupId)
        {
            Group Groupz = _context.Groups.Where(g => g.GroupId == groupId)
                .Include(g => g.GroupLocs)
                .ThenInclude(gl => gl.GroupLoc)
                .SingleOrDefault();

            foreach(GroupLocation GroupLoc in Groupz.GroupLocs) {
                    GroupLoc.GroupLoc.Groups = null;
                    GroupLoc.LocGroup = null;
            }
            var json = JsonConvert.SerializeObject(Groupz);

            return Json(json);
        }
    }
}
