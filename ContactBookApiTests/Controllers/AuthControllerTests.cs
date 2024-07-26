using AutoFixture;
using ContactBookApi.Controllers;
using ContactBookApi.Dtos;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("User already exists.")]
        [InlineData("Something went wrong, please try after sometime.")]
        [InlineData("Mininum password length should be 8")]
        [InlineData("Password should be apphanumeric")]
        [InlineData("Password should contain special characters")]
        public void Register_ReturnsBadRequest_WhenRegistrationFails(string message)
        {
            // Arrange
            var registerDto = new RegisterDto();
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Fact]
        public void Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var registerDto = new RegisterDto()
            {
                userId = 0,
                LoginId = "loginid",
                Email = "email@email.com",
                Password = "password",
                ConfirmPassword = "password",
                ContactNumber = "1234567890",
                FirstName = "firstname",
                LastName = "lastname",
                BestFriend ="BestFriend",
                FavouriteColor = "Color",
                FavouriteNumber = "12345",
            };
            var mockAuthService = new Mock<IAuthService>();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            mockAuthService.Setup(service => service.RegisterUserService(registerDto))
                           .Returns(expectedServiceResponse);

            var controller = new AuthController(mockAuthService.Object);

            // Act
            var actual = controller.Register(registerDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.RegisterUserService(registerDto), Times.Once);
        }

        [Theory]
        [InlineData("Invalid username or password!")]
        [InlineData("Something went wrong, please try after sometime.")]
        public void Login_ReturnsBadRequest_WhenLoginFails(string message)
        {
            // Arrange
            var loginDto = new LoginDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Fact]
        public void Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var loginDto = new LoginDto { UserName = "username", Password = "password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginUserService(loginDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.LoginUserService(loginDto), Times.Once);
        }

        [Theory]
        [InlineData("Please enter valid user details.")]
        [InlineData("Something went wrong, please try after sometimes.")]
        public void AddNewPassword_ReturnBadRequest_WhenAddingNewPasswordFails(string message)
        {
            // Arrange
            var newPasswordDto = new NewPasswordDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.AddNewPassword(newPasswordDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.AddNewPassword(newPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.AddNewPassword(newPasswordDto), Times.Once);
        }

        [Fact]
        public void AddNewPassword_ReturnsOk_WhenNewPasswordAddedSuccessfully()
        {
            // Arrange
            var newPasswordDtoDto = new NewPasswordDto { UserName = "username", Password = "password", ConfirmPassword ="password" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.AddNewPassword(newPasswordDtoDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.AddNewPassword(newPasswordDtoDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.AddNewPassword(newPasswordDtoDto), Times.Once);
        }

        [Theory]
        [InlineData("Invalid username")]
        [InlineData("Invalid username or other details")]
        public void ValidateForgotPassword_ReturnBadRequest_WhenValidatingUserFailsFails(string message)
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto();
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = message

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ValidateUserForForgetPassword(forgotPasswordDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ValidateForgotPassword(forgotPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(message, ((ServiceResponse<string>)actual.Value).Message);
            Assert.False(((ServiceResponse<string>)actual.Value).Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, actual.StatusCode);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(badRequestResult.Value);
            Assert.False(((ServiceResponse<string>)badRequestResult.Value).Success);
            mockAuthService.Verify(service => service.ValidateUserForForgetPassword(forgotPasswordDto), Times.Once);
        }

        [Fact]
        public void ValidateForgotPassword_ReturnsOk_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var forgotPasswordDto = new ForgotPasswordDto { UserName = "username", FavouriteNumber ="7", FavouriteColor="color", BestFriend = "Friend" };
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = string.Empty

            };
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.ValidateUserForForgetPassword(forgotPasswordDto))
                           .Returns(expectedServiceResponse);

            var target = new AuthController(mockAuthService.Object);

            // Act
            var actual = target.ValidateForgotPassword(forgotPasswordDto) as ObjectResult;

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull((ServiceResponse<string>)actual.Value);
            Assert.Equal(string.Empty, ((ServiceResponse<string>)actual.Value).Message);
            Assert.True(((ServiceResponse<string>)actual.Value).Success);
            var okResult = Assert.IsType<OkObjectResult>(actual);
            Assert.IsType<ServiceResponse<string>>(okResult.Value);
            Assert.True(((ServiceResponse<string>)okResult.Value).Success);
            mockAuthService.Verify(service => service.ValidateUserForForgetPassword(forgotPasswordDto), Times.Once);
        }

        [Fact]
        public void GetUserByUserName_ReturnsOkWithUser_WhenUserExists()
        {
            //Arrange
            string userName = "user";
            var fixture = new Fixture();
            var contact = fixture.Create<UpdateUserDto>();

            var response = new ServiceResponse<UpdateUserDto>
            {
                Success = true,
                Data = contact,
                //Data = contact.Select(c => new UpdateUserDto { userId = c.userId, FirstName = c.FirstName, LastName = c.LastName, LoginId = c.LoginId, ContactNumber = c.ContactNumber, Email = c.Email, ProfilePic = c.ProfilePic, ImageByte = c.ImageByte}) // Convert to StateDto
            };
            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUserByUserName(userName)).Returns(response);

            //Act
            var actual = target.GetUserByUserName(userName) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.GetUserByUserName(userName), Times.Once);
        }

        [Fact]
        public void GetUserByUserName_ReturnsNotFound_WhenUserExists()
        {
            //Arrange
            string userName = "user";
            UpdateUserDto user = null;
            var response = new ServiceResponse<UpdateUserDto>
            {
                Success = false,
                Data = user,
            };

            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.GetUserByUserName(userName)).Returns(response);

            //Act
            var actual = target.GetUserByUserName(userName) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);

            mockAuthService.Verify(c => c.GetUserByUserName(userName), Times.Once);
        }

        [Fact]
        public void UpdateUser_ReturnsOk_WhenUserIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateUserDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ModifyUser(It.IsAny<UpdateUserDto>())).Returns(response);

            //Act

            var actual = target.UpdateUser(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ModifyUser(It.IsAny<UpdateUserDto>()), Times.Once);

        }

        [Fact]
        public void UpdateUser_ReturnsBadRequest_WhenUserIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateUserDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockAuthService = new Mock<IAuthService>();
            var target = new AuthController(mockAuthService.Object);
            mockAuthService.Setup(c => c.ModifyUser(It.IsAny<UpdateUserDto>())).Returns(response);

            //Act

            var actual = target.UpdateUser(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockAuthService.Verify(c => c.ModifyUser(It.IsAny<UpdateUserDto>()), Times.Once);

        }
    }
}
