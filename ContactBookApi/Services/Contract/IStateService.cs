using ContactBookApi.Dtos;

namespace ContactBookApi.Services.Contract
{
    public interface IStateService
    {
        ServiceResponse<IEnumerable<StateContactDto>> GetAllState();
        ServiceResponse<IEnumerable<StateContactDto>> GetStateByCountryId(int id);
    }
}
