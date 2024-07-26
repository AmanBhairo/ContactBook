using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Services.Contract;

namespace ContactBookApi.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;
        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public ServiceResponse<IEnumerable<StateContactDto>> GetAllState()
        {
            var response = new ServiceResponse<IEnumerable<StateContactDto>>();
            var states = _stateRepository.GetAllState();

            if (states != null && states.Any())
            {
                List<StateContactDto> stateDtos = new List<StateContactDto>();
                foreach (var state in states.ToList())
                {
                    stateDtos.Add(new StateContactDto()
                    {
                        StateId = state.StateId,
                        StateName = state.StateName,
                        CountryId = state.CountryId,

                    });
                }
                response.Data = stateDtos;
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

        public ServiceResponse<IEnumerable<StateContactDto>> GetStateByCountryId(int id)
        {
            var response = new ServiceResponse<IEnumerable<StateContactDto>>();
            var states = _stateRepository.GetStateByCountryId(id);
            List<StateContactDto> stateContactDto = new List<StateContactDto>();
            if (states != null && states.Any())
            {
                foreach (var state in states)
                {
                    stateContactDto.Add(new StateContactDto()
                    {
                        StateId = state.StateId,
                        StateName  = state.StateName,
                        CountryId = state.CountryId,
                        
                    });
                }
                response.Data = stateContactDto;
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
