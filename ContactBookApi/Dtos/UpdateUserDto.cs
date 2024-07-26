namespace ContactBookApi.Dtos
{
    public class UpdateUserDto
    {
        public int userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string? ProfilePic { get; set; }
        public byte[]? ImageByte { get; set; }

    }
}
