using System;

namespace mapapp.Models
{
    public abstract class BaseEntity {}
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePic { get; set; }

        public string Bio { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}