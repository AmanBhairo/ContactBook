using ContactBookApi.Data.Implementation;
using ContactBookApi.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBookApi.Models;

namespace ContactBookApiTests.Repositories
{
    public class StateRepositoryTests
    {
        [Fact]
        public void GetAll_ReturnsStates_WhenStatesExist()
        {
            var statesList = new List<State>
            {
              new State{ StateId=1, StateName="State 1", CountryId = 1 },
              new State{ StateId=2, StateName="State 2", CountryId = 1},
             }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();

            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);

            //Act
            var actual = target.GetAllState();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);

        }

        [Fact]
        public void GetAll_ReturnsEmpty_WhenNoStatesExist()
        {
            var statesList = new List<State>().AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(c => c.GetEnumerator()).Returns(statesList.GetEnumerator());
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllState();
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(c => c.GetEnumerator(), Times.Once);

        }
        [Fact]
        public void GetStatesByCountryId_ReturnsStates_WhenStatesExist()
        {
            int countryId = 1;
            var statesList = new List<State>
            {
              new State{ StateId=1, StateName="State 1", CountryId = 1 },
              new State{ StateId=2, StateName="State 2", CountryId = 1},
             }.AsQueryable();

            var mockDbSet = new Mock<DbSet<State>>();

            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(statesList.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(statesList.Expression);

            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);

            //Act
            var actual = target.GetStateByCountryId(countryId);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);

        }

        [Fact]
        public void GetStatesByCountryId_ReturnsEmpty_WhenNoStatesExist()
        {
            int countryId = 1;
            var statesList = new List<State>().AsQueryable();
            var mockDbSet = new Mock<DbSet<State>>();
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Provider).Returns(statesList.Provider);
            mockDbSet.As<IQueryable<State>>().Setup(m => m.Expression).Returns(statesList.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.States).Returns(mockDbSet.Object);
            var target = new StateRepository(mockAbContext.Object);
            //Act
            var actual = target.GetStateByCountryId(countryId);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            Assert.Equal(statesList.Count(), actual.Count());
            mockAbContext.Verify(c => c.States, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Provider, Times.Once);
            mockDbSet.As<IQueryable<State>>().Verify(m => m.Expression, Times.Once);

        }
    }
}
