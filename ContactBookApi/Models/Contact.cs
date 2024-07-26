using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ContactBookApi.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactDescription { get; set; }

        public string? ProfilePic { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool Favourite { get; set; }

        public Country Country { get; set; }
        public  State State { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? BirthDate { get; set; }

    }
}
