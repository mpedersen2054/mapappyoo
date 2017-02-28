using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace mapapp.Controllers
{
    public class LocationController : Controller
    {
        private MyContext _context;
        public LocationController(MyContext context)
        {
            _context = context;
        }


        private GeoResult getCoordsFromAdr(string StreetAdr, string City, string State)
        {
            string trimmedAdr = StreetAdr.Trim('.');
            List<string> splitAdr = trimmedAdr.Split(' ');
        }

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
                

                var coords = getCoordsFromAdr(locModel.StreetAdr, locModel.Zip);
                double newLat = coords.lat;
                string gpId = 

                Location newLoc = new Location{
                    Name = locModel.Name,
                    StreetAdr = locModel.StreetAdr,
                    City = locModel.City,
                    State = locModel.State,
                    Zip = locModel.Zip,
                    Lat = newLat,
                    Lng = newLng,
                    GooglePlacesId = gpId,
                    CreatorId = (int)HttpContext.Session.GetInt32("user"),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Creator = _context.Users.Where(u => u.UserId == (int)HttpContext.Session.GetInt32("user")).SingleOrDefault()
                };

                _context.Locations.Add(newLoc);
                _context.SaveChanges();
                Location currentLoc = _context.Locations.Last();

                return RedirectToAction("ShowLocation", currentLoc.LocationId);
            }
            return View("ShowLocationNew", locModel);
        }
        
    }
}
