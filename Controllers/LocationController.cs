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



        private GeoResult getCoordsFromAdr(string StreetAdr, string City, string State)
        {
            string trimmedAdr = StreetAdr.Trim('.');
            string[] splitAdr = trimmedAdr.Split(' ');
            string togetherAdr = "";
            for(int i = 0; i < splitAdr.Length; i++){
                togetherAdr = togetherAdr + splitAdr[i];
                if(i < splitAdr.Length-1){
                    togetherAdr = togetherAdr + "+";
                }
            }
            string togetherCity = "";
            string[] splitCity = City.Split(' ');
            for(int i = 0; i < splitCity.Length; i++){
                togetherCity = togetherCity + splitCity[i];
                if(i < splitCity.Length-1){
                    togetherCity = togetherCity + "+";
                }
            }
            
            // string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={togetherAdr},+{togetherCity},+{State}&key={googleAPI}";
            // var request = WebRequest.Create(requestUrl);
            // var response = request.GetResponse();
            // dynamic jObj = JsonConvert.DeserializeObject(response);

            return new GeoResult();
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
                

                GeoResult geoResult = getCoordsFromAdr(locModel.StreetAdr, locModel.City, locModel.State);

                Location newLoc = new Location{
                    Name = locModel.Name,
                    StreetAdr = locModel.StreetAdr,
                    City = locModel.City,
                    State = locModel.State,
                    Zip = locModel.Zip,
                    Lat = geoResult.Lat,
                    Lng = geoResult.Lng,
                    GooglePlacesId = geoResult.GooglePlacesId,
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
