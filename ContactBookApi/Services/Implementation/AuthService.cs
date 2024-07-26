using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using ContactBookApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ContactBookApi.Services.Contract;
using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;

namespace ContactBookApi.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IVerifyPasswordHash _verifyPasswordHash;
        public AuthService(IAuthRepository authRepository, IVerifyPasswordHash verifyPasswordHash)
        {
            _verifyPasswordHash = verifyPasswordHash;
            _authRepository = authRepository;
        }

        public ServiceResponse<string> RegisterUserService(RegisterDto register)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (register != null)
            {
                message = CheckPasswordStrength(register.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else if (_authRepository.UserExist(register.LoginId, register.Email))
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }
                else
                {
                    //Save User
                    User user = new User()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        LoginId = register.LoginId,
                        ContactNumber = register.ContactNumber,
                        FavouriteNumber = register.FavouriteNumber,
                        FavouriteColor = register.FavouriteColor,
                        BestFriend = register.BestFriend,
                        ProfilePic = register.ProfilePic,
                        ImageByte = register.ImageByte
                    };

                    CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _authRepository.RegisterUser(user);
                    response.Success = result;
                    response.Message = result ? string.Empty : "Something went wrong, please try after sometimes.";
                }
            }
            return response;
        }

        public ServiceResponse<UpdateUserDto> GetUserByUserName(string userName)
        {
            var response = new ServiceResponse<UpdateUserDto>();
            var userDetail = _authRepository.ValidateUser(userName);
            
            if (userDetail != null)
            {
                var updateUserDtos = new UpdateUserDto()
                {
                    userId = userDetail.userId,
                    LoginId = userDetail.LoginId,
                    FirstName = userDetail.FirstName,
                    LastName = userDetail.LastName,
                    Email = userDetail.Email,
                    ContactNumber = userDetail.ContactNumber,
                    ProfilePic = userDetail.ProfilePic,
                    ImageByte = userDetail.ImageByte,

                };
                response.Data = updateUserDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }

        public ServiceResponse<string> ModifyUser(UpdateUserDto user)
        {
            var response = new ServiceResponse<string>();
            if (_authRepository.UserExist(user.LoginId, user.ContactNumber, user.userId))
            {
                response.Success = false;
                response.Message = "User Exists!";
                return response;
            }
            var existingCategory = _authRepository.ValidateUser(user.LoginId);
            var result = false;
            if (existingCategory != null)
            {
                existingCategory.FirstName = user.FirstName;
                existingCategory.LastName = user.LastName;
                existingCategory.ContactNumber = user.ContactNumber;
                existingCategory.ProfilePic = user.ProfilePic;
                existingCategory.ImageByte = user.ImageByte;

                result = _authRepository.UpdateUser(existingCategory);
            }
            if (result)
            {
                response.Message = "User Updated Successfully!";
            }
            else
            {
                response.Message = "Something went wrong please try after sometime";
            }
            return response;
        }

        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            if (login != null)
            {
                var user = _authRepository.ValidateUser(login.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                else if (!_verifyPasswordHash.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }

                string token = _verifyPasswordHash.CreateToken(user);
                response.Data = token;
                return response;
            }
            response.Message = "Something went wrong,please try after sometime.";
            return response;
        }

        public ServiceResponse<string> ValidateUserForForgetPassword(ForgotPasswordDto forgotPassword)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (forgotPassword != null)
            {
                var user = _authRepository.ValidateUser(forgotPassword.UserName);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username";
                    return response;
                }
                else if (ValidateUserForForgetPassword(forgotPassword.UserName, forgotPassword.FavouriteNumber, forgotPassword.FavouriteColor,forgotPassword.BestFriend) == false)
                {
                    response.Success = false;
                    response.Message = "Invalid username or other details";
                    return response;
                }
                else
                {
                    response.Success = true;
                    response.Message = message;
                    return response;
                }
                
            }
            response.Message = "Something went wrong,please try after sometime.";
            return response;
        }

        public ServiceResponse<string> AddNewPassword(NewPasswordDto newPassword)
        {
            var response = new ServiceResponse<string>();
            var message = string.Empty;
            if (newPassword != null)
            {
                message = CheckPasswordStrength(newPassword.Password);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                else
                {
                    User userData = _authRepository.ValidateUser(newPassword.UserName);
                    if(userData != null)
                    {

                        CreatePasswordHash(newPassword.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        userData.PasswordHash = passwordHash;
                        userData.PasswordSalt = passwordSalt;
                        var result = _authRepository.UpdatePassword(userData);
                        response.Success = result;
                        response.Message = result ? string.Empty : "Something went wrong, please try after sometimes.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Please enter valid user details.";
                    }
                }
            }
            return response;
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (password.Length < 8)
            {
                stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,*,(,),_,]"))
            {
                stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool ValidateUserForForgetPassword(string username, string favouriteNumber, string favouriteColor, string bestFriend)
        {
            var user = _authRepository.ValidateUser(username);
            if(user.FavouriteNumber == favouriteNumber && user.FavouriteColor == favouriteColor && user.BestFriend == bestFriend)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
