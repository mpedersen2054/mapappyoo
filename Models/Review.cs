namespace mapapp.Models
{
    public class Review : BaseEntity
    {
        public int ReviewId { get; set; }

        // give us UserId
        public int ReviewerId { get; set; }
        public User Reviewer {get; set;}

        // gives us LocationId
        public int RevieweeId { get; set; }
        public Location Reviewee {get; set;}

        public int Rating { get; set; }

        public string Message { get; set; }
    }
}