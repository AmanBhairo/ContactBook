using MVCContactRecords.ViewModels;

namespace MVCContactRecords.Services.Contract
{
    public interface IAuthService
    {
        string RegisterUserService(RegisterViewModel register);
        string LoginUserService(LoginViewModel login);
    }
}
