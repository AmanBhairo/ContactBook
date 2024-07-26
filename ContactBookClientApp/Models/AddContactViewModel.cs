using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactBookClientApp.Models
{
    public class AddContactViewModel
    {
        [Required(ErrorMessage = "Country name is required")]
        [DisplayName("Country name")]

        public int CountryId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        [DisplayName("State name")]
        public int StateId { get; set; }
        [DisplayName("First name")]
        [Required(ErrorMessage ="First name is required.")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Contact number is required.")]
        [DisplayName("Contact number")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [DisplayName("Contact Description")]
        public string ContactDescription { get; set; }
        public string? ProfilePic { get; set; }
        [DisplayName("Profile Image")]
        public IFormFile? File { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public bool Favourite { get; set; }
        public byte[]? ImageByte { get; set; }

        public List<StateViewModel>? states { get; set; }
        public List<CountryViewModel>? countries { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
