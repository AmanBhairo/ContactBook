using ContactBookApi.Dtos;

namespace ContactBookApi.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> RegisterUserService(RegisterDto register);
        ServiceResponse<string> AddNewPassword(NewPasswordDto newPassword);
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ValidateUserForForgetPassword(ForgotPasswordDto forgotPassword);
        ServiceResponse<UpdateUserDto> GetUserByUserName(string userName);
        ServiceResponse<string> ModifyUser(UpdateUserDto user);
    }
}
