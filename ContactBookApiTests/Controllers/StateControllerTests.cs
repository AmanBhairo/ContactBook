using ContactBookApi.Controllers;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Controllers
{
    public class StateControllerTests
    {
        [Fact]
        public void GetAllStates_ReturnsOkWithStates_WhenStateExists()
        {
            //Arrange
            var states = new List<State>
             {
            new State{StateId=1,StateName="State 1", CountryId= 1},
            new State{StateId=2,StateName="State 2", CountryId= 2},
            };

            var response = new ServiceResponse<IEnumerable<StateContactDto>>
            {
                Success = true,
                Data = states.Select(c => new StateContactDto { StateId = c.StateId, StateName = c.StateName, CountryId = c.CountryId }) // Convert to StateDto
            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetAllState()).Returns(response);

            //Act
            var actual = target.GetAllState() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockStateService.Verify(c => c.GetAllState(), Times.Once);
        }

        [Fact]
        public void GetAllStates_ReturnsNotFound_WhenNoStateExists()
        {
            //Arrange

            var response = new ServiceResponse<IEnumerable<StateContactDto>>
            {
                Success = false,
                Data = new List<StateContactDto>(),

            };

            var mockStateService = new Mock<IStateService>();
            var target = new StateController(mockStateService.Object);
            mockStateService.Setup(c => c.GetAllState()).Returns(response);

            //Act
            var actual = target.GetAllState() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockStateService.Verify(c => c.GetAllState(), Times.Once);
        }

        [Fact]
        public void GetStateByCountryId_ReturnsOkWithState_WhenStateExists()
        {
            //Arrange
            int countryId = 1;
            var states = new List<State>
            {
                new State { StateId = 1, StateName = "State 1", CountryId = 1 },
                new State { StateId = 2, StateName = "State 2", CountryId = 1 }
            };

            var response = new ServiceResponse<IEnumerable<StateContactDto>>
            {
                Success = true,
                Data = states.Select(c => new StateContactDto { StateId = c.StateId, StateName = c.StateName, CountryId = c.CountryId })
            };
            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStateByCountryId(countryId)).Returns(response);

            //Act
            var actual = target.GetStateByCountryId(countryId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStateByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStateByCountryId_ReturnsNotFound_WhenStateNotExists()
        {
            //Arrange
            int countryId = 1;

            var response = new ServiceResponse<IEnumerable<StateContactDto>>
            {
                Success = false,
                Data = null
            };
            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);
            mockCategoryService.Setup(c => c.GetStateByCountryId(countryId)).Returns(response);

            //Act
            var actual = target.GetStateByCountryId(countryId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockCategoryService.Verify(c => c.GetStateByCountryId(countryId), Times.Once);
        }

        [Fact]
        public void GetStateByCountryId_ReturnsBadRequest_WhenCountryIdIsNotValid()
        {
            //Arrange
            int countryId = 0;

            var mockCategoryService = new Mock<IStateService>();
            var target = new StateController(mockCategoryService.Object);

            //Act
            var actual = target.GetStateByCountryId(countryId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data", actual.Value);
        }

    }
}
