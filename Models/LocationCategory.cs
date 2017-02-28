namespace mapapp.Models
{
    public class LocationCategory : BaseEntity
    {
        public int LocationCategoryId { get; set; }

        // gives us LocationId
        public int CatLocationId { get; set; }

        // gives up CategoryId
        public int LocCategoryId { get; set; }
    }
}