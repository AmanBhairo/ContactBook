using ContactBookApi.Data.Contract;
using ContactBookApi.Models;

namespace ContactBookApi.Data.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IAppDbContext _appDbContext;
        public CountryRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Country> GetAllCountry()
        {
            List<Country> countries = _appDbContext.Countries.ToList();
            return countries;
        }
    }
}
