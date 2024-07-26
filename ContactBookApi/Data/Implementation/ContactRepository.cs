using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private readonly IAppDbContext _appDbContext;
        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Contact> GetAll(char? letter)
        {
            List<Contact> categories = _appDbContext.Contacts.Include(c => c.Country).Include(s=>s.State).Where(c => c.FirstName.StartsWith(letter.ToString().ToLower())).OrderBy(s => s.FirstName).ToList();
            return categories;
        }

        public IEnumerable<Contact> GetAllContacts()
        {
            List<Contact> categories = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).OrderBy(s=>s.FirstName).ToList();
            return categories;
        }

        public IEnumerable<Contact> GetAllFavouriteContacts()
        {
            List<Contact> categories = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).OrderBy(s => s.FirstName).Where(c => c.Favourite == true).ToList();
            return categories;
        }

        public Contact? GetContact(int id)
        {
            var category = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).FirstOrDefault(c => c.ContactId == id);
            return category;
        }

        public bool InsertContact(Contact category)
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

        public bool UpdateContact(Contact category)
        {
            var result = false;
            if (category != null)
            {
                _appDbContext.Contacts.Update(category);
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
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

        public bool ContactExists(string number)
        {
            var category = _appDbContext.Contacts.FirstOrDefault(c => c.ContactNumber == number);
            if (category != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int supplierid, string number)
        {
            var category = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != supplierid && c.ContactNumber == number);
            if (category != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int TotalContacts(string? letter, string search)
        {
            IQueryable<Contact> query = _appDbContext.Contacts;

            if (search == "yes")
            {
                query = query.Where(c => c.FirstName.Contains(letter.ToString()) || c.LastName.Contains(letter.ToString()));
            }
            else if (letter != null && search == "no")
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));

            }
            return query.Count();
        }
        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize,string sort)
        {
            int skip = (page - 1) * pageSize;
            if(sort == "dsc")
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .OrderByDescending(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .OrderBy(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            
        }

        public IEnumerable<Contact> GetPaginatedContacts(int page, int pageSize, string sort, string? letter, string search)
        {
            if (letter == null)
            {
                if(sort != "asc")
                {
                    int skip = (page - 1) * pageSize;

                    return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                        .OrderByDescending(s => s.FirstName)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    int skip = (page - 1) * pageSize;

                    return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .OrderBy(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
                }
            }
            else
            {
                if (search == "yes")
                {
                    int skip = (page - 1) * pageSize;
                    if (sort != "asc")
                    {
                        return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                        .Where(c => c.FirstName.Contains(letter.ToString()) || c.LastName.Contains(letter.ToString()))
                        .OrderByDescending(s => s.FirstName)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    }
                    else
                    {
                        return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                        .Where(c => c.FirstName.Contains(letter.ToString()) || c.LastName.Contains(letter.ToString()))
                        .OrderBy(s => s.FirstName)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    }
                }
                else
                {

                    int skip = (page - 1) * pageSize;
                    if (sort != "asc")
                    {
                        return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                        .Where(c => c.FirstName.StartsWith(letter.ToString()))
                        .OrderByDescending(s => s.FirstName)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    }
                    else
                    {
                        return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                        .Where(c => c.FirstName.StartsWith(letter.ToString()))
                        .OrderBy(s => s.FirstName)
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    }

                }
            }
            
            
            
        }
        public int TotalContactsForFavourite(char? letter)
        {
            IQueryable<Contact> query = _appDbContext.Contacts.Where(c=> c.Favourite == true);

            if (letter.HasValue)
            {
                query = query.Where(c => c.FirstName.StartsWith(letter.ToString()));
            }
            return query.Count();
        }
        public IEnumerable<Contact> GetPaginatedContactsForFavourite(int page, int pageSize, string sort)
        {
            int skip = (page - 1) * pageSize;
            if (sort == "dsc")
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .Where(c => c.Favourite == true)
                .OrderByDescending(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .Where(c => c.Favourite == true)
                .OrderBy(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            
        }

        public IEnumerable<Contact> GetPaginatedContactsForFavourite(int page, int pageSize, string sort, char? letter)
        {
            int skip = (page - 1) * pageSize;
            if (sort == "dsc")
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .Where(c => c.FirstName.StartsWith(letter.ToString()) && c.Favourite == true)
                .OrderByDescending(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                return _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State)
                .Where(c => c.FirstName.StartsWith(letter.ToString()) && c.Favourite == true)
                .OrderBy(s => s.FirstName)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
            }
            
        }


        //Report
        public IEnumerable<ContactRecordReportDto?> GetContactRecordBasedOnBirthdayMonthReport(int month)
        {
            var contactReport = _appDbContext.GetContactRecordBasedOnBirthdayMonth(month);
            return contactReport;

        }
        public IEnumerable<ContactRecordReportDto?> GetContactsByState(int state)
        {
            var contactReport = _appDbContext.GetContactsByState(state);
            return contactReport;

        }

        public int GetContactsCountByCountry(int country)
        {
            var query = _appDbContext.GetContactsCountByCountry(country);
            return query;
        }

        public int GetContactCountByGender(Char gender)
        {
            var query = _appDbContext.GetContactCountByGender(gender);
            return query;
        }
    }
}
