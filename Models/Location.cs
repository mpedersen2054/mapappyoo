using System;

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

        public int Lat { get; set; }
        public int Lng { get; set; }

        public string GooglePlacesId { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}