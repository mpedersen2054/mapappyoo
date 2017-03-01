using System;
using System.Collections.Generic;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net;
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

        // public void AddReview(Location currentLocation, User currentUser, )
        // {

        // }

        // GET: /locations
        [HttpGet]
        [Route("locations/{lid:int}")]
        public IActionResult ShowLocation(int lid)
        {
            return View("Locations");
        }

        // GET: /locations/all
        [HttpGet]
        [Route("locations/all")]
        public IActionResult ShowAllLocations()
        {
            //this returns a list of all locations along with each locations creator user obj and a list of all groups connected to that location
            List<Location> allLocs = _context.Locations.OrderByDescending(l => l.CreatedAt).Include(u => u.Creator).Include(g => g.Groups).ToList();
            return View("AllLocations");
        }

        // GET: /locations/new
        [HttpGet]
        [Route("locations/new")]
        public IActionResult ShowLocationNew()
        {
            return View("LocationNew");
        }


        // GET: /locations/{locationId}
        [HttpGet]
        [Route("locations/{locString}")]
        public IActionResult ShowGroup(string locString)
        {
            return View("Location");
        }

        //post: new location
        [HttpPostAttribute]
        [RouteAttribute("addLocation")]
        public IActionResult AddLocation(LocationViewModel locModel)
        {
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
                    Reviewee = currentLoc,
                    Rating = locModel.Rating,
                    Message = locModel.Message,
                    ReviewerId = currentUser.UserId,
                    Reviewer = currentUser,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Reviews.Add(newReview);
                _context.SaveChanges();

                return RedirectToAction("ShowLocation", currentLoc.LocationId);
            }
            ViewBag.Error = "Please use a valid address.";
            return View("ShowLocationNew", locModel);
        }

        //post: new review
        [HttpPostAttribute]
        [RouteAttribute("addReview/{locId:int}")]
        public IActionResult AddReview(ReviewViewModel reviewModel, int locId)
        {
            if (ModelState.IsValid)
            {
                
                User currentUser = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault();
                Location currentLoc = _context.Locations.Where(l => l.LocationId == locId).SingleOrDefault();


                Review newReview = new Review{
                    RevieweeId = currentLoc.LocationId,
                    Reviewee = currentLoc,
                    Rating = reviewModel.Rating,
                    Message = reviewModel.Message,
                    ReviewerId = currentUser.UserId,
                    Reviewer = currentUser,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Reviews.Add(newReview);
                _context.SaveChanges();

                return RedirectToAction("ShowLocation", currentLoc.LocationId);
            }
            return View("ShowLocationNew", reviewModel);
        }
        
    }
}
