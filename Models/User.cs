using System.Collections.Generic;

namespace mapapp.Models
{
    
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Username {get; set;}

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePic { get; set; }

        public string Bio { get; set; }
        public List<UserGroup> Groups {get; set;}
        public List<Review> Reviews {get; set;}
        public User(){
            Groups = new List<UserGroup>();
            Reviews = new List<Review>();
        }
        
    }
}