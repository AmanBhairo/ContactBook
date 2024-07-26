using ContactBookClientApp.Controllers;
using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace ContactBookClientAppTests.Controller
{
    public class ContactControllerTests
    {
        
        [Fact]
        public void Index_ReturnsView_WithContacts()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();

            var mockConfiguration = new Mock<IConfiguration>();

            var mockImageUpload = new Mock<IAddImageFileToPathService>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var expectedCategories = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };

            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = expectedCategories
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act

            var actual = target.Index() as ViewResult;

            // Assert

            Assert.NotNull(actual);

            Assert.IsType<List<ContactViewModel>>(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        [Fact]
        public void Index_ReturnsView_WithEmptyListOfContacts()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();

            var mockConfiguration = new Mock<IConfiguration>();

            var mockImageUpload = new Mock<IAddImageFileToPathService>();

            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");

            var mockHttpContext = new Mock<HttpContext>();
            var expectedResponse = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
            };

            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(
                It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60)).Returns(expectedResponse);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImageUpload.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            // Act

            var actual = target.Index() as ViewResult;

            // Assert

            Assert.NotNull(actual);
            Assert.IsType<List<ContactViewModel>>(actual.Model);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60), Times.Once);

        }

        //Test cases for Create() httpget
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

        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully_WhenFileISNotNull()
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

            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry, File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
           }
        [Fact]
        public void Create_RedirectToActionResult_WhenContactSavedSuccessfully_WhenFileISNull()
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

            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Saved Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Contact Saved Successfully", target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
           }
        [Fact]
        public void Create_ReturnsViewResultWitherrorMessage_WhenResponseIsNotSuccess()
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
            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);

            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            }
        [Fact]
        public void Create_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
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
            var viewModel = new AddContactViewModel { FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedCountryResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedStateResponse = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedCountryResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedStateResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Create(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PostHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            }
        [Fact]
        public void Create_ReturnsViewResult_WithContactList_WhenModelStateIsInvalid()
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
            var expectedResponseState = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var viewModel = new AddContactViewModel { FirstName = "firstname", LastName="lastname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
               .Returns(expectedResponseState);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);

            }
        [Fact]
        public void Create_ReturnsViewResult_WithEmptyCountryandStateList_WhenModelStateIsInvalid()
        {
            //Arrange

            var expectedResponse = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false,
            };
            var expectedResponseState = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,
            };
            var viewModel = new AddContactViewModel { FirstName = "firstname",LastName="lastname" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseState);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("LastName", "Last name is required.");

            //Act
            var actual = target.Create(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
           
        }

        //Edit HttpGet
        [Fact]
        public void Edit_ReturnsViewResult_WhenContactDetailsObtainedSuccessfully_WithCountryAndStateList()
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

            var contactId = 1;
            var expectedProducts = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedResponseForcountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = true,
                Data = expectedCountry,
            };
            var expectedResponseForstates = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = true,
                Data = expectedState,
            };
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Success = true,
                Data = expectedProducts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseForcountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseForstates);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as ViewResult;
            var model = actual.Model as UpdateContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.NotNull(model);
            Assert.NotNull(model.countries);
            Assert.NotNull(model.states);
            Assert.Equal(expectedCountry.Count, model.countries.Count());
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenContactDetailsObtainedSuccessfully_WithcountriesEmptyList()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedResponseForcountries = new ServiceResponse<IEnumerable<CountryViewModel>>
            {
                Success = false,
            };
            var expectedResponseForState = new ServiceResponse<IEnumerable<StateViewModel>>
            {
                Success = false,
            };
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
                .Returns(expectedResponseForcountries);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60))
              .Returns(expectedResponseForState);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as ViewResult;
            var model = actual.Model as UpdateContactViewModel;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.NotNull(model);
            Assert.Empty(model.countries);
            Assert.Empty(model.states);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Exactly(3));
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedProducts = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange

            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<UpdateContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<UpdateContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNull()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNotNull()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry, File = new FormFile(new MemoryStream(new byte[1]), 5, 4, "xyz", "xyz.jpg") };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
            }
        
        [Fact]
        public void Edit_RedirectToActionResult_WhenContactsUpdatedSuccessfullyWhenFileIsNullAndRemoveImageHiddenIsTrue()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", removeImageHidden = "true", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var successMessage = "Contact Updated Successfully";
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = successMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(successMessage, target.TempData["SuccessMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResultWitherrorMessage_WhenResponseIsNotSuccess()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedErrorResponse = new ServiceResponse<string>
            {
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedErrorResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal(errorMessage, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsRedirectToActionResult_WhenResponseIsNotSuccess()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Something went wrong try after some time";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Edit(viewModel) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.PutHttpResponseMessage(It.IsAny<string>(), viewModel, It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenModelStateIsInvalid()
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
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("ProductDescription", "Product description is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }
        [Fact]
        public void Edit_ReturnsViewResult_WhenModelStateIsInvalid_WhenStateAndCountryIsNull()
        {
            //Arrange
            var expectedCountry = new List<CountryViewModel> { };
            var expectedState = new List<StateViewModel> { };
            var viewModel = new UpdateContactViewModel { ContactId = 1, FirstName = "FirstName 1", LastName = "lastname", StateId = 1, CountryId = 1, states = expectedState, countries = expectedCountry };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };
            target.ModelState.AddModelError("ProductDescription", "Product description is required.");

            //Act
            var actual = target.Edit(viewModel) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(viewModel, actual.Model);
            Assert.False(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
        }

        //Test case for Details
        [Fact]
        public void Details_ReturnsViewResult_WhenDetailsObtainedSuccessfully_WhenImageIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1",ProfilePic="sampleImage.png" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsViewResult_WhenDetailsObtainedSuccessfully_WhenImageIsNull()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1"};

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Data = expectedContacts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Details(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        //Test case for Delete()
        [Fact]
        public void Delete_ReturnsViewResult_WhenDetailsObtainedSuccessfully()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");

            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = true,
                Data = expectedContacts,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as ViewResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenSuccessIsFalse()
        {
            //Arrange
            var contactId = 1;
            var expectedContacts = new ContactViewModel { ContactId = 1, FirstName = "FirstName 1" };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
            {
                Success = false,
                Data = expectedContacts,
                Message = errorMessage,
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedServiceResponse))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenDataIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNotNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var errorMessage = "Error Occured";
            var expectedServiceResponse = new ServiceResponse<ContactViewModel>
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
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(errorMessage, target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStatusCodeIsBadRequest_ErrorResponseIsNull()
        {
            //Arrange
            var contactId = 1;
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("FakeEndPoint");
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null))
            };
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockTempDataProvider.Object);
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>())).Returns(expectedResponse);
            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                }
            };

            //Act
            var actual = target.Delete(contactId) as RedirectToActionResult;

            //Assert
            Assert.NotNull(actual);
            Assert.True(target.ModelState.IsValid);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal("Something went wrong try after some time", target.TempData["errorMesssage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.GetHttpResponseMessage<ContactViewModel>(It.IsAny<string>(), It.IsAny<HttpRequest>()), Times.Once);
        }

        //Delete Confirm
        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletedSuccessfully()
        {
            // Arrange
            var id = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Success",
                Success = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["successMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }
        [Fact]
        public void DeleteConfirm_ReturnsRedirectToAction_WhenDeletionFailed()
        {
            // Arrange
            var id = 1;

            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var expectedServiceResponse = new ServiceResponse<string>
            {
                Message = "Error",
                Success = false
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60)).Returns(expectedServiceResponse);
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                },

            };
            // Act
            var actual = target.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(actual);
            Assert.Equal("PaginatedIndex", actual.ActionName);
            Assert.Equal(expectedServiceResponse.Message, target.TempData["errorMessage"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<string>>(It.IsAny<string>(), HttpMethod.Delete, It.IsAny<HttpRequest>(), null, 60), Times.Once);

        }

        //PaginatedIndexForFavourite
        [Fact]
        public void PaginatedIndexForFavourite_ReturnsContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndexForFavourite(null, 1, 2,"asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndexForFavourite_ReturnsNoContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>();
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndexForFavourite(null, 1, 2, "asc") as ViewResult;
            //Assert
            Assert.Equal("No record found", target.TempData["noRecord"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndexForFavourite_ReturnsContacts_WhenLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndexForFavourite('f', 1, 2, "asc") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndexForFavourite_ReturnsContacts_WhenLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 11 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndexForFavourite('f', 6, 6, "asc") as RedirectToActionResult;
            //Assert
            Assert.Equal("PaginatedIndexForFavourite", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }

        //PaginatedInde
        [Fact]
        public void PaginatedIndex_ReturnsContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndex(null, 1, 2, "asc","yes") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndex_ReturnsNoContacts_WhenLetterIsNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>();
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = false,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });
            var mockDataProvider = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), mockDataProvider.Object);

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                TempData = tempData,
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndex(null, 1, 2, "asc", "no") as ViewResult;
            //Assert
            Assert.Equal("No record found", target.TempData["noRecord"]);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndex_ReturnsContacts_WhenLetterIsNotNull()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 3 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndex("f", 1, 2, "asc", "yes") as ViewResult;
            //Assert
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Exactly(2));
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }
        [Fact]
        public void PaginatedIndex_ReturnsContacts_WhenLetterIsNotNull_PageIsGreaterThanTotalCount()
        {
            //Arrange

            var contacts = new List<ContactViewModel>
            {
                new ContactViewModel{ ContactId=1, FirstName="FirstName 1"},
                new ContactViewModel{ ContactId=2, FirstName="FirstName 2"},
            };
            var response = new ServiceResponse<IEnumerable<ContactViewModel>>
            {
                Success = true,
                Data = contacts
            };
            var mockHttpClientService = new Mock<IHttpClientService>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockImage = new Mock<IAddImageFileToPathService>();
            mockConfiguration.Setup(c => c["EndPoint:CivicaApi"]).Returns("fakeEndPoint");
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
                .Returns(response);
            mockHttpClientService.Setup(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, mockHttpContext.Object.Request, null, 60))
               .Returns(new ServiceResponse<int> { Success = true, Data = 11 });

            var target = new ContactController(mockHttpClientService.Object, mockConfiguration.Object, mockImage.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object,
                },
            };

            //Act
            var actual = target.PaginatedIndex("f", 6, 6, "asc", "no") as RedirectToActionResult;
            //Assert
            Assert.Equal("PaginatedIndex", actual.ActionName);
            mockConfiguration.Verify(c => c["EndPoint:CivicaApi"], Times.Once);
            mockHttpClientService.Verify(c => c.ExecuteApiRequest<ServiceResponse<int>>(It.IsAny<string>(), HttpMethod.Get, It.IsAny<HttpRequest>(), null, 60), Times.Once);
        }


    }
}
