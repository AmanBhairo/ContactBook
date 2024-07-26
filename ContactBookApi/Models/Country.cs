using System.IO;

namespace ContactBookApi.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public ICollection<Contact> Contacts { get; set; }
        public ICollection<State> States { get; set; }
    }
}
