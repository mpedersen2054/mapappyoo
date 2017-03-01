using System.ComponentModel.DataAnnotations;

namespace mapapp.ViewModels{
    public class LocationViewModel{
        [Required(ErrorMessage = "You must include a name.")]
        [MinLength(2, ErrorMessage = "Name must be filled in and at least 2 characters.")]
        public string Name {get; set;}
        [RequiredAttribute(ErrorMessage = "You must include a street address.")]
        [MinLengthAttribute(5, ErrorMessage = "Street address must be at least 5 characters.")]
        public string StreetAdr {get; set;}
        [Required(ErrorMessage = "You must include a city name.")]
        public string City {get; set;}
        [RequiredAttribute(ErrorMessage = "Please include a state.")]
        [RegularExpressionAttribute(@"^((AL)|(AK)|(AS)|(AZ)|(AR)|(CA)|(CO)|(CT)|(DE)|(DC)|(FM)|(FL)|(GA)|(GU)|(HI)|(ID)|(IL)|(IN)|(IA)|(KS)|(KY)|(LA)|(ME)|(MH)|(MD)|(MA)|(MI)|(MN)|(MS)|(MO)|(MT)|(NE)|(NV)|(NH)|(NJ)|(NM)|(NY)|(NC)|(ND)|(MP)|(OH)|(OK)|(OR)|(PW)|(PA)|(PR)|(RI)|(SC)|(SD)|(TN)|(TX)|(UT)|(VT)|(VI)|(VA)|(WA)|(WV)|(WI)|(WY))$", ErrorMessage = "Please enter the state in capital letters.")]
        public string State {get; set;}
        [RequiredAttribute(ErrorMessage="Please enter a zip code.")]
        [MinLengthAttribute(5, ErrorMessage="Zip code must be 5 characters.")]
        [MaxLengthAttribute(5, ErrorMessage="Zip code must be 5 characters.")]
        public string Zip {get; set;}
        public double Lat {get; set;}
        public double Lng {get; set;}
        public string GooglePlacesId {get; set;}
        [RequiredAttribute(ErrorMessage="You must include a rating.")]
        public int Rating { get; set; }
        [RequiredAttribute(ErrorMessage="Please include a review.")]
        [MinLengthAttribute(20, ErrorMessage="Review must be at least 20 characters.")]
        public string Message { get; set; }
        
    }
}