
namespace mapapp.Models
{
    public class GroupLocation : BaseEntity
    {
        public int GroupLocationId { get; set; }

        // FK = GroupId
        public int LocGroupId { get; set; }
        // gives us Group

        // FK = LocationId
        public int GroupLocId { get; set; }
        // gives us Location

    }
}