using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Services.Contract
{
    public interface IContactService
    {
        ServiceResponse<IEnumerable<ContactDto>> GetContacts(char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetAllContacts();
        ServiceResponse<ContactDto> GetContact(int id);
        ServiceResponse<string> AddContact(Contact contact);
        ServiceResponse<string> ModifyContact(Contact contact);
        ServiceResponse<string> RemoveContact(int id);
        ServiceResponse<int> TotalContacts(string? letter, string search);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, string sort);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, string sort, string? letter, string search);
        ServiceResponse<int> TotalContactsForFavourite(char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsForFavourite(int page, int pageSize, string sort);
        ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsForFavourite(int page, int pageSize, string sort, char? letter);
        ServiceResponse<IEnumerable<ContactDto>> GetAllFavouriteContacts();

        //Report
        ServiceResponse<IEnumerable<ContactRecordReportDto>> GetContactRecordBasedOnBirthdayMonthReport(int month);
        ServiceResponse<IEnumerable<ContactRecordReportDto>> GetContactsByState(int state);
        ServiceResponse<int> GetContactsCountByCountry(int country);
        ServiceResponse<int> GetContactCountByGender(string gender);
    }
}
