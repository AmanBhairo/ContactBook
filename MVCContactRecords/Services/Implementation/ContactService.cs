using MVCContactRecords.Data.Contract;
using MVCContactRecords.Models;
using MVCContactRecords.Services.Contract;
using System.Diagnostics.Metrics;

namespace MVCContactRecords.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<Contact> GetCategories(char? letter)
        {
            var categories = _contactRepository.GetAll(letter);
            if (categories != null && categories.Any())
            {
                return categories;
            }

            return new List<Contact>();
        }

        public IEnumerable<Contact> GetAllCategory()
        {
            var categories = _contactRepository.GetAllCategory();
            if (categories != null && categories.Any())
            {
                return categories;
            }

            return new List<Contact>();
        }

        public Contact? GetCategory(int id)
        {
            var category = _contactRepository.GetCategory(id);
            return category;

        }

        public string AddCategory(Contact category)
        {
            if (_contactRepository.CategoryExists(category.FirstName, category.LastName))
            {
                return "Category Already Exist";
            }

            var result = _contactRepository.InsertCategory(category);
            return result ? "Category Saved Successfully" : "Something went wrong try after some time";
        }

        public string ModifyCategory(Contact category)
        {
            var message = string.Empty;
            if (_contactRepository.CategoryExists(category.ContactId, category.FirstName, category.LastName))
            {
                message = "Category Exists!";
                return message;

            }
            var existingCategory = _contactRepository.GetCategory(category.ContactId);
            var result = false;
            if (existingCategory != null)
            {
                existingCategory.FirstName = category.FirstName;
                existingCategory.LastName = category.LastName;
                existingCategory.ContactNumber = category.ContactNumber;
                existingCategory.Email = category.Email;
                existingCategory.ContactDescription = category.ContactDescription;

                result = _contactRepository.UpdateCategory(existingCategory);
            }
            message = result ? "Category Updated Successfully!" : "Something went wrong please try after sometime";
            return message;
        }

        public string RemoveCategory(int id)
        {
            var result = _contactRepository.DeleteCategory(id);
            if (result)
            {
                return "Category deleted Successfully";
            }
            else
            {
                return "Something went wrong try after some time";
            }

        }


        public int TotalContacts(char? letter)
        {
            return _contactRepository.TotalContacts(letter);
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            return _contactRepository.GetPaginatedContacts(page, pageSize);
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter)
        {
            return _contactRepository.GetPaginatedContacts(page, pageSize, letter);
        }
    }
}
