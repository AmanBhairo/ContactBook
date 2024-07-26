using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Services.Contract;

namespace ContactBookApi.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public ServiceResponse<IEnumerable<CountryContactDto>> GetAllCountry()
        {
            var response = new ServiceResponse<IEnumerable<CountryContactDto>>();
            var countries = _countryRepository.GetAllCountry();

            if (countries != null && countries.Any())
            {
                List<CountryContactDto> countryDtos = new List<CountryContactDto>();
                foreach (var country in countries.ToList())
                {
                    countryDtos.Add(new CountryContactDto()
                    {
                        CountryId = country.CountryId,
                        CountryName = country.CountryName,

                    });
                }
                response.Data = countryDtos;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }
            return response;
        }
    }
}
