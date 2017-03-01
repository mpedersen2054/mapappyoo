using System.ComponentModel.DataAnnotations;

namespace mapapp.ViewModels{
    public class GroupLoginViewModel{
        [Required(ErrorMessage = "You must include a group name.")]
        public string GroupName {get; set;}
        [DataTypeAttribute(DataType.Password)]
        public string Password {get; set;}
    }
}