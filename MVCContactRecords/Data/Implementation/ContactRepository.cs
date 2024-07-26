using Microsoft.EntityFrameworkCore;
using MVCContactRecords.Data.Contract;
using MVCContactRecords.Models;
using System.Diagnostics.Metrics;

namespace MVCContactRecords.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _appDbContext;
        public ContactRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Contact> GetAll(char? letter)
        {
            List<Contact> categories = _appDbContext.Contacts.Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).ToList();
            return categories;
        }

        public IEnumerable<Contact> GetAllCategory()
        {
            List<Contact> categories = _appDbContext.Contacts.ToList();
            return categories;
        }

        public Contact? GetCategory(int id)
        {
            var category = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId == id);
            return category;
        }

        public bool InsertCategory(Contact category)
        {
            var result = false;
            if (category != null)
            {
                _appDbContext.Contacts.Add(category);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool UpdateCategory(Contact category)
        {
            var result = false;
            if (category != null)
            {
                _appDbContext.Contacts.Update(category);
                _appDbContext.Entry(category).State = EntityState.Modified;
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteCategory(int id)
        {
            var result = false;
            var category = _appDbContext.Contacts.Find(id);
            if (category != null)
            {
                _appDbContext.Contacts.Remove(category);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool CategoryExists(string firstName, string lastName)
        {
            var category = _appDbContext.Contacts.FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);
            if (category != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CategoryExists(int supplierid, string firstName, string lastName)
        {
            var category = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != supplierid && c.FirstName == firstName && c.LastName == lastName);
            if (category != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int TotalContacts(char? letter)
        {
            IQueryable<Contact> query = _appDbContext.Contacts;

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, char? letter)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .Where(c => c.FirstName.StartsWith(letter.ToString()))
                .OrderBy(c => c.ContactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
    }
}
