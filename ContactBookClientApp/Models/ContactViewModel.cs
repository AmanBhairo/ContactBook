namespace ContactBookClientApp.Models
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactDescription { get; set; }
        public string? ProfilePic { get; set; }
        public IFormFile? File { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool Favourite { get; set; }

        public StateViewModel state { get; set; }
        public CountryViewModel country { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
