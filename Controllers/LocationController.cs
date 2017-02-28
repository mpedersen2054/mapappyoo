using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mapapp.Models;
using mapapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace mapapp.Controllers
{
    public class LocationController : Controller
    {
        private MyContext _context;
        public LocationController(MyContext context)
        {
            _context = context;
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
                

                double newLat = getLatFromAdr(locModel.StreetAdr, locModel.Zip);
                double newLng = getLngFromAdr(locModel.StreetAdr, locModel.Zip);
                string gpId = getGPId(newLat, newLng);

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
                    UpdatedAt = DateTime.Now
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
