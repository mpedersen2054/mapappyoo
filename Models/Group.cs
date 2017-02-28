using System;

namespace mapapp.Models
{
    public class Group : BaseEntity
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Password { get; set; }

        public int AdminId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}