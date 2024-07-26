using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data
{
    public interface IAppDbContext: IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ContactRecordReportDto> contactRecordBasedOnBirthdayMonthReportDtos { get; set; }

        //Report
        public IQueryable<ContactRecordReportDto> GetContactRecordBasedOnBirthdayMonth(int Month);
        public IQueryable<ContactRecordReportDto> GetContactsByState(int state);
        public int GetContactsCountByCountry(int country);
        public int GetContactCountByGender(Char gender);
    }
}
