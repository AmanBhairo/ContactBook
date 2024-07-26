using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookApi.Dtos
{
    public class RegisterDto
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
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "The password must be at least 8 characters long and contain at least 1 uppercase letter, 1 number, and 1 special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirm Password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Favourite number is requires.")]
        [DisplayName("Whats your favourite number?")]
        [StringLength(15)]
        public string FavouriteNumber { get; set; }

        [Required(ErrorMessage = "Favourite color is required.")]
        [DisplayName("Whats your favourite color?")]
        [StringLength(15)]
        public string FavouriteColor {  get; set; }

        [Required(ErrorMessage = "Best friend name is required.")]
        [DisplayName("Who is your best friend?")]
        [StringLength(15)]
        public string BestFriend { get; set; }
        public string? ProfilePic { get; set; }
        public byte[]? ImageByte { get; set; }

    }
}
