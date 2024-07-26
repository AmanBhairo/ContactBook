namespace ContactBookApi.Dtos
{
    public class ContactRecordReportDto
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactDescription { get; set; }
        public string? ProfilePic { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public bool Favourite { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[]? ImageByte { get; set; }
        

    }
}
