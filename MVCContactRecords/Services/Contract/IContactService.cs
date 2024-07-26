using MVCContactRecords.Models;

namespace MVCContactRecords.Services.Contract
{
    public interface IContactService
    {
        IEnumerable<Contact> GetCategories(char? letter);
        IEnumerable<Contact> GetAllCategory();

        Contact? GetCategory(int id);

        string AddCategory(Contact category);

        string ModifyCategory(Contact category);

        string RemoveCategory(int id);

        int TotalContacts(char? letter);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize);
        IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter);
    }
}
