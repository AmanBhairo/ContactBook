﻿namespace ContactBookApi.Models
{
    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
