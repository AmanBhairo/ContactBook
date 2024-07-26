using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactBookClientApp.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Favourite number is requires.")]
        [DisplayName("Whats your favourite number?")]
        [StringLength(15)]
        public string FavouriteNumber { get; set; }

        [Required(ErrorMessage = "Favourite color is required.")]
        [DisplayName("Whats your favourite color?")]
        [StringLength(15)]
        public string FavouriteColor { get; set; }

        [Required(ErrorMessage = "Best friend name is required.")]
        [DisplayName("Who is your best friend?")]
        [StringLength(15)]
        public string BestFriend { get; set; }

    }
}
