using System;
using System.Collections.Generic;

namespace mapapp.Models
{
    public class Location : BaseEntity
    {
        public int LocationId { get; set; }

        public string Name { get; set; }

        public string StreetAdr { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public string GooglePlacesId { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Review> Users {get; set;}
        public List<GroupLocation> Groups {get; set;}
        public List<LocationCategory> Categories {get; set;}
        public Location(){
            Users = new List<Review>();
            Groups = new List<GroupLocation>();
            Categories = new List<LocationCategory>();
        }
    }
}