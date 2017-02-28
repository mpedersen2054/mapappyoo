using System;

namespace mapapp.Models
{
    public class UserGroup : BaseEntity
    {
        public int UserGroupId { get; set; }

        // corresponds to UserId
        public int MemberId { get; set; }

        // corresponds to GroupId
        public int OrganizationId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}