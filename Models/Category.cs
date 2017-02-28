using System.Collections.Generic;

namespace mapapp.Models
{
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public List<LocationCategory> Locations {get; set;}
        public Category(){
            Locations = new List<LocationCategory>();
        }
    }
}