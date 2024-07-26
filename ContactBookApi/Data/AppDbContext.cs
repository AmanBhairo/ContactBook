using ContactBookApi.Dtos;
using ContactBookApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactBookApi.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ContactRecordReportDto> contactRecordBasedOnBirthdayMonthReportDtos { get; set; }

        public EntityState GetEntryState<TEntity>(TEntity entity) where TEntity : class
        {
            return Entry(entity).State;
        }

        public void SetEntryState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            Entry(entity).State = entityState;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
                .HasOne(s => s.Country)
                .WithMany(s => s.States)
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_State_Country");

            modelBuilder.Entity<ContactRecordReportDto>().HasNoKey();
            modelBuilder.Entity<ReportContDto>().HasNoKey();
        }

        public virtual IQueryable<ContactRecordReportDto> GetContactRecordBasedOnBirthdayMonth(int Month)
        {
            var month = new SqlParameter("@Month", Month);

            return Set<ContactRecordReportDto>().FromSqlRaw("dbo.GetContactRecordBasedOnBirthdayMonth @Month", month);
        }

        public virtual IQueryable<ContactRecordReportDto> GetContactsByState(int state)
        {
            var State = new SqlParameter("@StateId", state);

            return Set<ContactRecordReportDto>().FromSqlRaw("dbo.GetContactsByState @StateId", State);
        }

        public virtual int GetContactsCountByCountry(int country)
        {
            var Country = new SqlParameter("@CountryId", country);

            var result = Set<ReportContDto>().FromSqlRaw("dbo.GetContactsCountByCountry @CountryId", Country).AsEnumerable().FirstOrDefault();
            return result.TotalCount;
        }

        public virtual int GetContactCountByGender(Char gender)
        {
            var Gender = new SqlParameter("@Gender", gender);

            var result = Set<ReportContDto>().FromSqlRaw("dbo.GetContactCountByGender @Gender", Gender).AsEnumerable().FirstOrDefault();
            return result.TotalCount;
        }

    }
}
