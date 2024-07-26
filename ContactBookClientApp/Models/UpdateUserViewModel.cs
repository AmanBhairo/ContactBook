using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookClientApp.Models
{
    public class UpdateUserViewModel
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(15)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(15)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login id is required.")]
        [StringLength(15)]
        [DisplayName("Login ID")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Emal address is required.")]
        [StringLength(50)]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(15)]
        [DisplayName("Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        public string ContactNumber { get; set; }

        public string? ProfilePic { get; set; }
        [DisplayName("Profile Image")]
        public IFormFile? File { get; set; }

        public byte[]? ImageByte { get; set; }
        public string? removeImageHidden { get; set; }

    }
}
