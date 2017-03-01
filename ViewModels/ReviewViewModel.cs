using System.ComponentModel.DataAnnotations;

namespace mapapp.ViewModels
{
    public class ReviewViewModel
    {
        [RequiredAttribute(ErrorMessage="You must include a rating.")]
        public int Rating { get; set; }
        [RequiredAttribute(ErrorMessage="Please include a review.")]
        [MinLengthAttribute(20, ErrorMessage="Review must be at least 20 characters.")]
        public string Message { get; set; }
    }
}