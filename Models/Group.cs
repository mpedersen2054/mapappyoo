using System.Collections.Generic;

namespace mapapp.Models
{
    public class Group : BaseEntity
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Password { get; set; }

        public int AdminId { get; set; }

        public string Description { get; set; }
        public List<UserGroup> Users {get; set;}
        public List<GroupLocation> Locations {get; set;}
        public Group(){
            Users = new List<UserGroup>();
            Locations = new List<GroupLocation>();
        }
    }
}