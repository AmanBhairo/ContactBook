using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetAllState();
        IEnumerable<State> GetStateByCountryId(int id);
    }
}
