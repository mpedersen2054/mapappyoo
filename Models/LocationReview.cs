namespace mapapp.Models
{
    public class LocationReview : BaseEntity
    {
        public int LocationCategoryId { get; set; }

        // gives us LocationId
        public int LocReviewId { get; set; }
        // public Location LocReview { get; set; }

        // gives up CategoryId
        public int RevLocationId { get; set; }
    }
}