using ContactBookApi.Dtos;
using ContactBookApi.Models;

namespace ContactBookApi.Data.Contract
{
    public interface IAuthRepository
    {
        bool RegisterUser(User user);
        bool UpdateUser(User user);
        bool UpdatePassword(User user);
        User? ValidateUser(string username);
        bool UserExist(string loginId, string email);
        bool UserExist(string loginId, string email, int userId);
    }
}
