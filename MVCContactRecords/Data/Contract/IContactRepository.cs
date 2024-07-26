using MVCContactRecords.Models;

namespace MVCContactRecords.Data.Contract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll(char? letter);
        IEnumerable<Contact> GetAllCategory();
        Contact? GetCategory(int id);
        bool CategoryExists(string firstName, string lastName);
        bool CategoryExists(int supplierid, string firstName, string lastName);
        bool InsertCategory(Contact category);
        bool DeleteCategory(int id);
        bool UpdateCategory(Contact category);
        int TotalContacts(char? letter);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter);
    }
}
