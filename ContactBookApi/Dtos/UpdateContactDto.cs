namespace ContactBookApi.Dtos
{
    public class UpdateContactDto
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
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool Favourite { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
