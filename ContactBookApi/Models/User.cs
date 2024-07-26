using System.ComponentModel.DataAnnotations;

namespace ContactBookApi.Models
{
    public class User
    {
        public int userId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        public string LoginId { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(15)]
        public string FavouriteNumber { get; set; }

        [Required]
        [StringLength(15)]
        public string FavouriteColor { get; set; }

        [Required]
        [StringLength(15)]
        public string BestFriend {  get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }
        public string? ProfilePic { get; set; }
        public byte[]? ImageByte { get; set; }

    }
}
