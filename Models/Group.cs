using System.Collections.Generic;

namespace mapapp.Models
{
    public class Group : BaseEntity
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Password { get; set; }

        public int AdminId { get; set; }
        public User Admin {get; set;}

        public string Description { get; set; }

        public List<UserGroup> Members {get; set;}
        
        public List<GroupLocation> GroupLocs {get; set;}
        
        public Group(){
            Members = new List<UserGroup>();
            GroupLocs = new List<GroupLocation>();
        }
    }
}