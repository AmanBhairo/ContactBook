using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCContactRecords.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
