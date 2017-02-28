using System;

namespace mapapp.Models
{
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}