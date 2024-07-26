using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAllCountry();
    }
}
