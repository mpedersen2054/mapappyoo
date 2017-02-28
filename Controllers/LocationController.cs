using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mapapp.Controllers
{
    public class LocationController : Controller
    {
        // GET: /locations
        [HttpGet]
        [Route("locations")]
        public IActionResult ShowLocation()
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
        
    }
}
