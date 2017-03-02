namespace mapapp.Models
{
    public class UserGroup : BaseEntity
    {
        public int UserGroupId { get; set; }

        // corresponds to UserId
        public int MemberId { get; set; }
        public User Member { get; set; }

        // corresponds to GroupId
        public int OrganizationId { get; set; }
        public Group Organization { get; set; }
    }
}