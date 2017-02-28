using System;

namespace mapapp.Models
{
    public class Review : BaseEntity
    {
        public int ReviewId { get; set; }

        // give us UserId
        public int ReviewerId { get; set; }

        // gives us LocationId
        public int RevieweeId { get; set; }

        public string Rating { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}