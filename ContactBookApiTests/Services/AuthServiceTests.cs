using AutoFixture;
using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public void RegisterUserService_ReturnsSuccess_WhenValidRegistration()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mockAuthRepository.Setup(repo => repo.RegisterUser(It.IsAny<User>())).Returns(true);


            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "loginid",
                ContactNumber = "1234567890",
                Password = "Password@123"
            };

            // Act
            var result = target.RegisterUserService(registerDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockAuthRepository.Verify(c => c.RegisterUser(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenWeakPassword()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var registerDto = new RegisterDto
            {
                Password = "weak"
            };

            // Act
            var result = authService.RegisterUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(stringBuilder.ToString(), result.Message);
        }

        [Fact]
        public void RegisterUserService_ReturnsFailure_WhenUserAlreadyExists()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            mockAuthRepository.Setup(repo => repo.UserExist(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            var registerDto = new RegisterDto
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = "existingUser",
                ContactNumber = "1234567890",
                Password = "Password@123",
                FavouriteNumber = "7",
                FavouriteColor = "color",
                BestFriend = "friemd",
            };

            // Act
            var result = authService.RegisterUserService(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User already exists.", result.Message);
            mockAuthRepository.Verify(c => c.UserExist(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void LoginUserService_ReturnsSomethingWentWrong_WhenLoginDtoIsNull()
        {
            //Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(null);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Something went wrong,please try after sometime.", result.Message);

        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenUserIsNull()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username"
            };
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns<User>(null);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);


        }
        [Fact]
        public void LoginUserService_ReturnsInvalidUsernameOrPassword_WhenPasswordIsWrong()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);


        }

        [Fact]
        public void LoginUserService_ReturnsResponse_WhenLoginIsSuccessful()
        {
            //Arrange
            var loginDto = new LoginDto
            {
                UserName = "username",
                Password = "password"
            };
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(repo => repo.ValidateUser(loginDto.UserName)).Returns(user);
            mockConfiguration.Setup(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)).Returns(true);
            mockConfiguration.Setup(repo => repo.CreateToken(user)).Returns("");

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);


            // Act
            var result = target.LoginUserService(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            mockAuthRepository.Verify(repo => repo.ValidateUser(loginDto.UserName), Times.Once);
            mockConfiguration.Verify(repo => repo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt), Times.Once);
            mockConfiguration.Verify(repo => repo.CreateToken(user), Times.Once);


        }

        [Fact]
        public void ValidateUserForForgetPassword_ReturnsFailure_WhenInvalidUserIsPassed()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            ForgotPasswordDto forgotPasswordDto = new ForgotPasswordDto()
            {
                UserName = "user",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend"
            };

            User user = null;
            //var user = new User
            //{
            //    userId = 1,
            //    FirstName = "FirstName",
            //    LastName = "LastName",
            //    LoginId = "1",
            //    Email = "email@gmail.com",
            //    ContactNumber = "1234567890",
            //    FavouriteNumber = "1",
            //    FavouriteColor = "color",
            //    BestFriend = "friend",
            //};
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgotPasswordDto.UserName)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.ValidateUserForForgetPassword(forgotPasswordDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username", result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(forgotPasswordDto.UserName), Times.Once);
            
        }
        [Fact]
        public void ValidateUserForForgetPassword_ReturnsFailure_WhenUserRecoveryDetailsAreInvalid()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            ForgotPasswordDto forgotPasswordDto = new ForgotPasswordDto()
            {
                UserName = "user",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend"
            };

            var user = new User
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "1",
                Email = "email@gmail.com",
                ContactNumber = "1234567890",
                FavouriteNumber = "2",
                FavouriteColor = "color",
                BestFriend = "friend",
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgotPasswordDto.UserName)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.ValidateUserForForgetPassword(forgotPasswordDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or other details", result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(forgotPasswordDto.UserName), Times.Exactly(2));

        }

        [Fact]
        public void ValidateUserForForgetPassword_ReturnsTrue_WhenValidRecoveryDetailsArePassed()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            ForgotPasswordDto forgotPasswordDto = new ForgotPasswordDto()
            {
                UserName = "user",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend"
            };

            var user = new User
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "1",
                Email = "email@gmail.com",
                ContactNumber = "1234567890",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend",
            };
            mockAuthRepository.Setup(repo => repo.ValidateUser(forgotPasswordDto.UserName)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.ValidateUserForForgetPassword(forgotPasswordDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(String.Empty, result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(forgotPasswordDto.UserName), Times.Exactly(2));

        }
        [Fact]
        public void ValidateUserForForgetPassword_ReturnsFailure_WhenRecoveryPasswordDetailsAreNull()
        {
            // Arrange  
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();

            ForgotPasswordDto forgotPasswordDto = null;

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.ValidateUserForForgetPassword(forgotPasswordDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Something went wrong,please try after sometime.", result.Message);

        }

        [Fact]
        public void AddNewPassword_ReturnFailure_WhenWeakPassword()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var newPasswordDto = new NewPasswordDto
            {
                Password = "weak"
            };

            // Act
            var result = authService.AddNewPassword(newPasswordDto);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(stringBuilder.ToString(), result.Message);
        }

        [Fact]
        public void AddNewPassword_ReturnFailure_WhenUpdatingPasswordFails()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var newPasswordDto = new NewPasswordDto
            {
                UserName = "user",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };
            var user = new User
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "1",
                Email = "email@gmail.com",
                ContactNumber = "1234567890",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend",
            };

            mockAuthRepository.Setup(repo => repo.ValidateUser(newPasswordDto.UserName)).Returns(user);
            mockAuthRepository.Setup(repo => repo.UpdatePassword(user)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.AddNewPassword(newPasswordDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Something went wrong, please try after sometimes.", result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(newPasswordDto.UserName), Times.Once);
            mockAuthRepository.Verify(c => c.UpdatePassword(user), Times.Once);

        }

        [Fact]
        public void AddNewPassword_ReturnSuccess_WhenNewPasswordSuccessfully()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            var newPasswordDto = new NewPasswordDto
            {
                UserName = "user",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };
            var user = new User
            {
                userId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                LoginId = "1",
                Email = "email@gmail.com",
                ContactNumber = "1234567890",
                FavouriteNumber = "1",
                FavouriteColor = "color",
                BestFriend = "friend",
            };

            mockAuthRepository.Setup(repo => repo.ValidateUser(newPasswordDto.UserName)).Returns(user);
            mockAuthRepository.Setup(repo => repo.UpdatePassword(user)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.AddNewPassword(newPasswordDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(string.Empty, result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(newPasswordDto.UserName), Times.Once);
            mockAuthRepository.Verify(c => c.UpdatePassword(user), Times.Once);

        }

        [Fact]
        public void AddNewPassword_ReturnFailure_WhenDetailsAreInvalid()
        {
            // Arrange
            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockConfiguration = new Mock<IVerifyPasswordHash>();
            var authService = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Mininum password length should be 8" + Environment.NewLine);
            stringBuilder.Append("Password should be alphanumeric" + Environment.NewLine);
            stringBuilder.Append("Password should contain special characters" + Environment.NewLine);
            NewPasswordDto newPasswordDto = new NewPasswordDto
            {
                UserName = "user",
                Password = "Password@123",
                ConfirmPassword = "Password@123"
            };
            User user = null;

            mockAuthRepository.Setup(repo => repo.ValidateUser(newPasswordDto.UserName)).Returns(user);

            var target = new AuthService(mockAuthRepository.Object, mockConfiguration.Object);

            // Act
            var result = target.AddNewPassword(newPasswordDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Please enter valid user details.", result.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(newPasswordDto.UserName), Times.Once);
        }

        [Fact]
        public void GetUserByUserName_ReturnsContacts_WhenUserExist()
        {

            // Arrange
            string loginId = "user";
            var user = new User()
            {
                userId = 1,
                LoginId = "user",
                FirstName = "firstName",
                LastName = "lastName",
                Email = "user@gmail.com",
                ContactNumber = "1234567890",
                ProfilePic = "image",
                

            };

            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyPassword = new Mock<IVerifyPasswordHash>();
            mockRepository.Setup(r => r.ValidateUser(loginId)).Returns(user);

            var contactService = new AuthService(mockRepository.Object,mockVerifyPassword.Object);

            // Act
            var actual = contactService.GetUserByUserName(loginId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.ValidateUser(loginId), Times.Once);
        }

        [Fact]
        public void GetUserByUserName_ReturnsNoRecordFound_WhenNoUserExist()
        {
            // Arrange
            string loginId = "user";
            User user = null;


            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyPassword = new Mock<IVerifyPasswordHash>();
            mockRepository.Setup(r => r.ValidateUser(loginId)).Returns(user);
            var contactService = new AuthService(mockRepository.Object, mockVerifyPassword.Object);

            // Act
            var actual = contactService.GetUserByUserName(loginId);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.ValidateUser(loginId), Times.Once);
        }

        [Fact]
        public void ModifyUser_ReturnsAlreadyExists_WhenUserAlreadyExists()
        {
            int userId = 1;
            string loginId = "user";
            var user = new UpdateUserDto()
            {
                userId = userId,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = loginId,
                ContactNumber = "1234567891",
            };


            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            mockRepository.Setup(r => r.UserExist(loginId, user.ContactNumber, user.userId)).Returns(true);

            var authService = new AuthService(mockRepository.Object,mockVerifyPasswordHash.Object);

            // Act
            var actual = authService.ModifyUser(user);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("User Exists!", actual.Message);
            mockRepository.Verify(r => r.UserExist(loginId, user.ContactNumber, user.userId), Times.Once);
        }
        [Fact]
        public void ModifyUser_ReturnsSomethingWentWrong_WhenUserNotFound()
        {
            int userId = 1;
            string loginId = "user";
            var existingUser = new UpdateUserDto()
            {
                userId = userId,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = loginId,
                ContactNumber = "1234567891",
            };

            var updatedUser = new UpdateUserDto()
            {
                LoginId = "user1",
                FirstName = "C1"
            };


            var mockRepository = new Mock<IAuthRepository>();
            var mockVerifyPasswordHash = new Mock<IVerifyPasswordHash>();
            //mockRepository.Setup(r => r.UserExist(loginId, updatedUser.ContactNumber, updatedUser.userId)).Returns(false);
            mockRepository.Setup(r => r.ValidateUser(updatedUser.LoginId)).Returns<User>(null);

            var contactService = new AuthService(mockRepository.Object,mockVerifyPasswordHash.Object);

            // Act
            var actual = contactService.ModifyUser(updatedUser);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            //mockRepository.Verify(r => r.UserExist(loginId, updatedUser.ContactNumber, updatedUser.userId), Times.Once);
            mockRepository.Verify(r => r.ValidateUser(updatedUser.LoginId), Times.Once);
        }

        [Fact]
        public void ModifyUser_ReturnsUpdatedSuccessfully_WhenUserModifiedSuccessfully()
        {

            //Arrange
            int userId = 1;
            string loginId = "user";
            var existingUser = new User()
            {
                userId = userId,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = loginId,
                ContactNumber = "1234567891",
            };

            var updatedUser = new UpdateUserDto()
            {
                userId = userId,
                LoginId = loginId,
                FirstName = "newFirstName",
                ContactNumber = "1234567891"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyHashPassword = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(c => c.UserExist(updatedUser.LoginId, updatedUser.ContactNumber,updatedUser.userId)).Returns(false);
            mockAuthRepository.Setup(c => c.ValidateUser(updatedUser.LoginId)).Returns(existingUser);
            mockAuthRepository.Setup(c => c.UpdateUser(existingUser)).Returns(true);

            var target = new AuthService(mockAuthRepository.Object,mockVerifyHashPassword.Object);

            //Act

            var actual = target.ModifyUser(updatedUser);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("User Updated Successfully!", actual.Message);

            mockAuthRepository.Verify(c => c.ValidateUser(updatedUser.LoginId), Times.Once);


            mockAuthRepository.Verify(c => c.UpdateUser(existingUser), Times.Once);

        }
        [Fact]
        public void ModifyUser_ReturnsError_WhenUserModifiedFails()
        {

            //Arrange
            int userId = 1;
            string loginId = "user";
            var existingUser = new User()
            {
                userId = userId,
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@example.com",
                LoginId = loginId,
                ContactNumber = "1234567891",
            };

            var updatedUser = new UpdateUserDto()
            {
                userId = userId,
                LoginId = loginId,
                FirstName = "newFirstName",
                ContactNumber = "1234567891"
            };

            var mockAuthRepository = new Mock<IAuthRepository>();
            var mockVerifyHashPassword = new Mock<IVerifyPasswordHash>();
            mockAuthRepository.Setup(c => c.UserExist(updatedUser.LoginId, updatedUser.ContactNumber, updatedUser.userId)).Returns(false);
            mockAuthRepository.Setup(c => c.ValidateUser(updatedUser.LoginId)).Returns(existingUser);
            mockAuthRepository.Setup(c => c.UpdateUser(existingUser)).Returns(false);

            var target = new AuthService(mockAuthRepository.Object, mockVerifyHashPassword.Object);

            //Act

            var actual = target.ModifyUser(updatedUser);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            mockAuthRepository.Verify(c => c.ValidateUser(updatedUser.LoginId), Times.Once);
            mockAuthRepository.Verify(c => c.UpdateUser(existingUser), Times.Once);

        }
    }
}
