using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll(char? letter);
        IEnumerable<Contact> GetAllContacts();
        IEnumerable<Contact> GetAllFavouriteContacts();
        Contact? GetContact(int id);
        bool ContactExists(string number);
        bool ContactExists(int supplierid, string number);
        bool InsertContact(Contact category);
        bool DeleteContact(int id);
        bool UpdateContact(Contact category);
        int TotalContacts(string? letter, string search);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, string sort);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, string sort, string? letter, string search);
        int TotalContactsForFavourite(char? letter);
        IEnumerable<Contact> GetPaginatedContactsForFavourite(int page, int pageSize, string sort);

        IEnumerable<Contact> GetPaginatedContactsForFavourite(int page, int pageSize, string sort, char? letter);

        //Report
        IEnumerable<ContactRecordReportDto?> GetContactRecordBasedOnBirthdayMonthReport(int month);
        IEnumerable<ContactRecordReportDto?> GetContactsByState(int state);
        int GetContactsCountByCountry(int country);
        int GetContactCountByGender(Char gender);
    }
}
