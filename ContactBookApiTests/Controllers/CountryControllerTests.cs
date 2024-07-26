using ContactBookApi.Controllers;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Controllers
{
    public class CountryControllerTests
    {
        [Fact]
        public void GetAllCountries_ReturnsOkWithCountries_WhenCountryExists()
        {
            //Arrange
            var countries = new List<Country>
             {
            new Country{CountryId=1,CountryName="Country 1"},
            new Country{CountryId=2,CountryName="Country 2"},
            };

            var response = new ServiceResponse<IEnumerable<CountryContactDto>>
            {
                Success = true,
                Data = countries.Select(c => new CountryContactDto { CountryId = c.CountryId, CountryName = c.CountryName }) // Convert to CountryDto
            };

            var mockCountryService = new Mock<ICountryService>();
            var target = new CountryController(mockCountryService.Object);
            mockCountryService.Setup(c => c.GetAllCountry()).Returns(response);

            //Act
            var actual = target.GetAllCountry() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCountryService.Verify(c => c.GetAllCountry(), Times.Once);
        }

        [Fact]
        public void GetAllCountries_ReturnsNotFound_WhenNoCountryExists()
        {
            //Arrange


            var response = new ServiceResponse<IEnumerable<CountryContactDto>>
            {
                Success = false,
                Data = new List<CountryContactDto>()

            };

            var mockCountryService = new Mock<ICountryService>();
            var target = new CountryController(mockCountryService.Object);
            mockCountryService.Setup(c => c.GetAllCountry()).Returns(response);

            //Act
            var actual = target.GetAllCountry() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockCountryService.Verify(c => c.GetAllCountry(), Times.Once);
        }
    }
}
