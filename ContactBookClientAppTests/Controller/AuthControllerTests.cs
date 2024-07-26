using ContactBookClientApp.Controllers;
using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookClientAppTests.Controller
{
    public class AuthControllerTests
    {
        //HttpGet Register
        [Fact]
        public void Register_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object,mockIAddImage.Object,mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.Register() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //Register Post
        [Fact]
        public void Register_ModelIsInvalid()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel { FirstName = "firstname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object,mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("LastName", "last name is required");
            //Act
            var actual = target.Register(registerViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(registerViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_RedirectToRegisterSuccess_WhenUserSavedSuccessfully()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel{ 
                FirstName = "firstname", 
                LastName = "lastname", 
                Password = "Password@123", 
                Email = "email@gmail.com", 
                ConfirmPassword = "Password@123", 
                ContactNumber = "1234567890", 
                LoginId = "loginid",
                FavouriteColor = "color",
                FavouriteNumber = "1",
                BestFriend = "Friend",
                File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg")
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "User saved successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("RegisterSuccess", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            { FirstName = "firstname", LastName = "lastname", Password = "Password@123", Email = "email@gmail.com", ConfirmPassword = "Password@123", ContactNumber = "1234567890", LoginId = "loginid" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Register", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Register_RedirectToAction_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var registerViewModel = new RegisterViewModel
            { FirstName = "firstname", LastName = "lastname", Password = "Password@123", Email = "email@gmail.com", ConfirmPassword = "Password@123", ContactNumber = "1234567890", LoginId = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Register(registerViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Register", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), registerViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        //HttpGet RegisterSuccess
        [Fact]
        public void RegisterSuccess_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.RegisterSuccess() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //HttpGet Login
        [Fact]
        public void Login_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //Login HttpPost
        [Fact]
        public void Login_ModelIsInvalid()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "Username is required");
            //Act
            var actual = target.Login(loginViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(loginViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void Login_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Error Occurs";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Login", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Login_Success_RedirectToAction()
        {
            //Arrange
            var loginViewModel = new LoginViewModel { UserName = "LoginId", Password = "Password" };
            var mockToken = "mockToken";
            var mockLoginId = "LoginId";
            var mockUserDetails = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "Firstname",
                LastName = "last name",
                Email = "abc@gmail.com",
                ContactNumber = "9089876567",
                LoginId = "LoginId",
                ProfilePic = "abc.png",
                ImageByte = new byte[10]
            };
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()));
            mockResponseCookie.Setup(c => c.Append("LoginId", It.IsAny<string>(), It.IsAny<CookieOptions>()));

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedUserDetail = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "Firstname",
                LastName = "last name",
                Email = "abc@gmail.com",
                ContactNumber = "9089876567",
                LoginId = "LoginId",
                ProfilePic = "abc.png",
                ImageByte = new byte[10]
            };
            var successMessage = "User login Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage,
                Data = mockToken,

            };
            var expectedUserServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = true,
                Data = expectedUserDetail,

            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var expectedUserResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedUserServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
             .Returns(expectedResponse);
            var loginid = (mockLoginId);

            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedUserResponse);
            var jwtToken = new JwtSecurityToken(claims: new[] { new Claim("LoginId", mockLoginId) });
            mockTokenHandler.Setup(t => t.ReadJwtToken(It.IsAny<string>())).Returns(jwtToken);

            var mockAuthController = new Mock<AuthController> { CallBase = true };
            //mockAuthController.Setup(c => c.UserDetailById("loginid")).Returns(mockUserDetails);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            if (mockUserDetails != null && mockUserDetails.ImageByte != null)
            {
                var image = Convert.ToBase64String(mockUserDetails.ImageByte);
                int chunkSize = 3800;
                int totalChunks = (image.Length + chunkSize - 1) / chunkSize;

                for (int i = 0; i < totalChunks; i++)
                {
                    string chunk = image.Substring(i * chunkSize, Math.Min(chunkSize, image.Length - i * chunkSize));
                    mockResponseCookie.Verify(c => c.Append($"image_chunk_{i}", chunk, It.IsAny<CookieOptions>()), Times.Once);
                }
            }
            mockTokenHandler.Verify(t => t.ReadJwtToken(It.IsAny<string>()), Times.Once);
            mockResponseCookie.Verify(c => c.Append("jwtToken", mockToken, It.IsAny<CookieOptions>()), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Exactly(3));
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Exactly(3));
            Assert.True(target.ModelState.IsValid);

            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);

            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);

        }
        [Fact]
        public void Login_RedirectToAction_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var loginViewModel = new LoginViewModel
            { Password = "Password@123", UserName = "loginid" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.Login(loginViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("Login", actual.ActionName);
            Assert.Equal("Something went wrong.Please try after sometime.", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), loginViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void Logout_DeleteJwtToken()
        {
            //Arrange
            var mockResponseCookie = new Mock<IResponseCookies>();
            mockResponseCookie.Setup(c => c.Delete("jwtToken"));
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpResponse = new Mock<HttpResponse>();
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockHttpContext.SetupGet(c => c.Response).Returns(mockHttpResponse.Object);
            mockHttpResponse.SetupGet(c => c.Cookies).Returns(mockResponseCookie.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.LogOut() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Index", actual.ActionName);
            Assert.Equal("Home", actual.ControllerName);
            mockResponseCookie.Verify(c => c.Delete("jwtToken"), Times.Once);
            mockHttpContext.VerifyGet(c => c.Response, Times.Once);
            mockHttpResponse.VerifyGet(c => c.Cookies, Times.Once);
        }

        //HttpGet Forgot password
        [Fact]
        public void ForgotPassword_ReturnsView()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.ForgotPassword() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        //Forgot password post
        [Fact]
        public void ForgotPassword_ModelIsInvalid()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel { BestFriend ="friend",FavouriteColor ="color", FavouriteNumber ="7"};
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "User name is required.");
            //Act
            var actual = target.ForgotPassword(forgotPasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(forgotPasswordViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void ForgotPassword_RedirectToRegisterSuccess_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel { UserName = "username", BestFriend = "friend", FavouriteColor = "color", FavouriteNumber = "7" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = string.Empty;
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ForgotPassword(forgotPasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("UpdatePassword", actual.ActionName);
            Assert.Equal(string.Empty, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void ForgotPassword_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel { UserName = "username", BestFriend = "friend", FavouriteColor = "color", FavouriteNumber = "7" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Invalid username or other details";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ForgotPassword(forgotPasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPassword", actual.ActionName);
            Assert.Equal("Invalid username or other details", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void ForgotPassword_RedirectToAction_WhenBadRequest_WhenSomethingWentWrong()
        {
            // Arrange
            var forgotPasswordViewModel = new ForgotPasswordViewModel { UserName = "username", BestFriend = "friend", FavouriteColor = "color", FavouriteNumber = "7" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.ForgotPassword(forgotPasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPassword", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), forgotPasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        //HttpGet update password
        [Fact]
        public void UpdatePassword_ReturnsView()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            string userName = "user";
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.UpdatePassword(userName) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdatePasswordModel>(result.Model);
        }
        //Update password post
        [Fact]
        public void UpdatePassword_ModelIsInvalid()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            target.ModelState.AddModelError("UserName", "User name is required.");
            //Act
            var actual = target.UpdatePassword(updatePasswordViewModel) as ViewResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(updatePasswordViewModel, actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            Assert.False(target.ModelState.IsValid);
        }
        [Fact]
        public void UpdatePassword_RedirectToRegisterSuccessWithPasswordUpdatedSuccessfullyMessage_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = string.Empty;
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdatePassword(updatePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal("Password updated successfully", target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void UpdatePassword_RedirectToRegisterSuccess_WhenUserValidatedSuccessfully()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var successMessage = "Success";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = true,
                Message = successMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdatePassword(updatePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal("Success", target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void UpdatePassword_RedirectToAction_WhenBadRequest()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var errorMessage = "Invalid username or other details";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Success = false,
                Message = errorMessage
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdatePassword(updatePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPassword", actual.ActionName);
            Assert.Equal("Invalid username or other details", target.TempData["ErrorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }
        [Fact]
        public void UpdatePassword_WhenSomethingWentWrong()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");


            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()))
               .Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.UpdatePassword(updatePasswordViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("ForgotPassword", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), updatePasswordViewModel, It.IsAny<HttpRequest>()), Times.Once);
            Assert.True(target.ModelState.IsValid);
        }

        //HttpGet change password
        [Fact]
        public void ChnagePassword_ReturnsView()
        {
            // Arrange
            var updatePasswordViewModel = new UpdatePasswordModel { UserName = "user", Password = "password@123", ConfirmPassword = "password@123" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockIAddImage = new Mock<IAddImageFileToPathService>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockIAddImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };
            //Act
            var result = target.ChnagePassword() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UpdatePasswordModel>(result.Model);
            var model = result.Model as UpdatePasswordModel;
            Assert.Equal("user", model.UserName);
        }

        [Fact]
        public void Create_ReturnsViewResult_WithCountryList_WhenCountryExists()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel>
            {
                new CountryViewModel {CountryId = 1,CountryName = "Country 1"},
                new CountryViewModel {CountryId = 2,CountryName = "Country 2"},

            };
            var expectedState = new List<StateViewModel>
            {
                new StateViewModel {StateId=1,CountryId = 1,StateName = "State 1"},
                new StateViewModel {StateId=2,CountryId = 2,StateName = "State 2"},

            };
            var expectedResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            var mockHttpContext = new Mock<HttpContext>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            //Act
            var actual = target.Create() as ViewResult;

            //Assert
            Assert.NotNull(actual);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //HttpGet UserDetailByUserName
        [Fact]
        public void UserDetailByUserName_ReturnsData_WhenUserDetailsObtainedSuccessfully()
        {
            //Arrange

            string userName = "testuser";
            var expectedProducts = new UpdateUserViewModel {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");


            var expectedServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = true,
                Data = expectedProducts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object, mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UserDetailById(userName) ;

            //Assert
            Assert.NotNull(actual);
            var okResult = Assert.IsType<UpdateUserViewModel>(actual);
            //Assert.Equal(userName, userDetail.LoginId);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            
        }

        //Update User
        [Fact]
        public void UpdateUser_ReturnsViewResult_WhenUserDetailsObtainedSuccessfully()
        {
            //Arrange
            string userName = "testuser";
            var expectedProducts = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

            var expectedServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = true,
                Data = expectedProducts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser() as ViewResult;
            var model = actual.Model as UpdateUserViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.NotNull(model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(1));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            
        }
        
        [Fact]
        public void UpdateUser_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            string userName = "testuser";
            var expectedProducts = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);

            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = false,
                Data = expectedProducts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = true,
                Data = null,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateUserViewModel>
            {
                Success = false,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser() as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateUserViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUser_RedirectToActionResult_WhenUserUpdatedSuccessfullyWhenFileIsNull()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890"
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var successMessage = "User Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };

            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_RedirectToActionResult_WhenUserUpdatedSuccessfullyWhenFileIsNotNull()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg")
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var successMessage = "User Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void UpdateUser_RedirectToActionResult_WhenUserUpdatedSuccessfullyWhenFileIsNullAndRemoveImageHiddenIsTrue()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                removeImageHidden = "true",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var successMessage = "User Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsViewResultWitherrorMessage_WhenResponseIsNotSuccess()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                removeImageHidden = "true",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                FirstName = "John",
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                removeImageHidden = "true",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var errorMessage = "Something went wrong try after some time";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.UpdateUser(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact", actual.ControllerName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            //Arrange
            string userName = "testuser";
            var viewModel = new UpdateUserViewModel
            {
                userId = 1,
                LastName = "Doe",
                LoginId = "testuser",
                Email = "john.doe@example.com",
                ContactNumber = "1234567890",
                removeImageHidden = "true",
            };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var mockTokenHandler = new Mock<IJwtTokenHandler>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Name).Returns("user"); // Mocking User.Identity.Name
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            var mockHttpContext = new Mock<HttpContext>();
            mockPrincipal.Setup(x => x.Identity).Returns(mockIdentity.Object);
            mockHttpContext.Setup(x => x.User).Returns(mockPrincipal.Object);
            var target = new AuthController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object,mockTokenHandler.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("FirstName", "First name is required.");

            //Act
            var actual = target.UpdateUser(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        


    }
}
