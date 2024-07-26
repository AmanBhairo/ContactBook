using ContactBookApi.Data.Contract;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactBookApi.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly IAppDbContext _appDbContext;
        public StateRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<State> GetAllState()
        {
            List<State> states = _appDbContext.States.ToList();
            return states;
        }

        public IEnumerable<State> GetStateByCountryId(int id)
        {
            List<State> state = _appDbContext.States.Where(c => c.CountryId == id).ToList();
            return state;
        }
    }
}
