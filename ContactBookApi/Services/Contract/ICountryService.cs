using ContactBookApi.Dtos;

namespace ContactBookApi.Services.Contract
{
    public interface ICountryService
    {
        ServiceResponse<IEnumerable<CountryContactDto>> GetAllCountry();
    }
}
