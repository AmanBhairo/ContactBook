using AutoFixture;
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
    public class ContactControllerTests
    {
        [Fact]
        public void GetContactsByChar_ReturnsOkWithContacts_WhenContactExists()
        {
            //Arrange
            char letter = 'a';
            var fixture = new Fixture();
            var contact = fixture.Create<IEnumerable<ContactDto>>();

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contact.Select(c => new ContactDto { ContactId = c.ContactId, CountryId = c.CountryId, StateId = c.StateId, FirstName = c.FirstName, LastName = c.LastName, ContactNumber = c.ContactNumber, Email = c.Email, ContactDescription = c.ContactDescription, ProfilePic = c.ProfilePic, Gender=c.Gender, Address = c.Address, Favourite = c.Favourite, ImageByte=c.ImageByte,State = c.State,Country = c.Country }) // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContacts(letter)).Returns(response);

            //Act
            var actual = target.GetContactsByChar(letter) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContacts(letter), Times.Once);
        }

        [Fact]
        public void GetContactsByChar_ReturnsNotFound_WhenContactExists()
        {
            //Arrange
            char letter = 'a';
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = new List<ContactDto>(),

            };

            var mockStateService = new Mock<IContactService>();
            var target = new ContactController(mockStateService.Object);
            mockStateService.Setup(c => c.GetContacts(letter)).Returns(response);

            //Act
            var actual = target.GetContactsByChar(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockStateService.Verify(c => c.GetContacts(letter), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsOkWithContacts_WhenContactExists()
        {
            //Arrange
            var fixture = new Fixture();
            var contact = fixture.Create<IEnumerable<ContactDto>>();

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contact.Select(c => new ContactDto { ContactId = c.ContactId, CountryId = c.CountryId, StateId = c.StateId, FirstName = c.FirstName, LastName = c.LastName, ContactNumber = c.ContactNumber, Email = c.Email, ContactDescription = c.ContactDescription, ProfilePic = c.ProfilePic, Gender = c.Gender, Address = c.Address, Favourite = c.Favourite, ImageByte = c.ImageByte, State = c.State, Country = c.Country }) // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllContacts()).Returns(response);

            //Act
            var actual = target.GetAllContacts() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllContacts(), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsNotFound_WhenContactExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = new List<ContactDto>(),

            };

            var mockStateService = new Mock<IContactService>();
            var target = new ContactController(mockStateService.Object);
            mockStateService.Setup(c => c.GetAllContacts()).Returns(response);

            //Act
            var actual = target.GetAllContacts() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockStateService.Verify(c => c.GetAllContacts(), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsOkWithContacts_WhenContactExists()
        {
            //Arrange
            var fixture = new Fixture();
            var contact = fixture.Create<IEnumerable<ContactDto>>();

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contact.Select(c => new ContactDto { ContactId = c.ContactId, CountryId = c.CountryId, StateId = c.StateId, FirstName = c.FirstName, LastName = c.LastName, ContactNumber = c.ContactNumber, Email = c.Email, ContactDescription = c.ContactDescription, ProfilePic = c.ProfilePic, Gender = c.Gender, Address = c.Address, Favourite = c.Favourite, ImageByte = c.ImageByte, State = c.State, Country = c.Country }) // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetAllFavouriteContacts()).Returns(response);

            //Act
            var actual = target.GetAllFavouriteContacts() as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetAllFavouriteContacts(), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsNotFound_WhenContactExists()
        {
            //Arrange
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = new List<ContactDto>(),

            };

            var mockStateService = new Mock<IContactService>();
            var target = new ContactController(mockStateService.Object);
            mockStateService.Setup(c => c.GetAllFavouriteContacts()).Returns(response);

            //Act
            var actual = target.GetAllFavouriteContacts() as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);

            mockStateService.Verify(c => c.GetAllFavouriteContacts(), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsOkWithContacts_WhenValidIdPassesAndContactExists()
        {
            //Arrange
            int ContactId =1;
            var fixture = new Fixture();
            var contact = fixture.Create<ContactDto>();

            var response = new ServiceResponse<ContactDto>
            {
                Success = true,
                Data = contact, // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(ContactId)).Returns(response);

            //Act
            var actual = target.GetContactById(ContactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(ContactId), Times.Once);
        }
        [Fact]
        public void GetContactById_ReturnsBadRequst_WhenValidIdPassedAndContactDoesNotExists()
        {
            //Arrange
            int ContactId = 1;
            ContactDto contact = null;

            var response = new ServiceResponse<ContactDto>
            {
                Success = false,
                Data = contact, // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetContact(ContactId)).Returns(response);

            //Act
            var actual = target.GetContactById(ContactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetContact(ContactId), Times.Once);
        }
        [Fact]
        public void GetContactById_ReturnsBadRequst_WhenInValidIdPassed()
        {
            //Arrange
            int ContactId = 0;
            ContactDto contact = null;

            var response = new ServiceResponse<ContactDto>
            {
                Success = false,
                Data = contact, // Convert to StateDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);

            //Act
            var actual = target.GetContactById(ContactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data", actual.Value);
        }

        [Fact]
        public void AddContact_ReturnsOk_WhenContactIsAddedSuccessfully()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddcontactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);
            
            //Act

            var actual = target.CreateContact(addContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void AddContact_ReturnsBadRequest_WhenContactIsNotAdded()
        {
            var fixture = new Fixture();
            var addContactDto = fixture.Create<AddcontactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.AddContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.CreateContact(addContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.AddContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void UpdateContact_ReturnsOk_WhenContactIsUpdatesSuccessfully()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenContactIsNotUpdated()
        {
            var fixture = new Fixture();
            var updateContactDto = fixture.Create<UpdateContactDto>();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.ModifyContact(It.IsAny<Contact>())).Returns(response);

            //Act

            var actual = target.UpdateContact(updateContactDto) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.ModifyContact(It.IsAny<Contact>()), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = true,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            //Act

            var actual = target.DeleteContact(contactId) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactNotDeleted()
        {

            var contactId = 1;
            var response = new ServiceResponse<string>
            {
                Success = false,
            };
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.RemoveContact(contactId)).Returns(response);

            //Act

            var actual = target.DeleteContact(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.RemoveContact(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_ReturnsBadRequest_WhenContactIdIsInvalid()
        {

            var contactId = 0;

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);

            //Act

            var actual = target.DeleteContact(contactId) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(400, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal("Please enter proper data", actual.Value);
        }

        [Fact]
        public void GetTotalContacts_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            //Arrange
            var search = "no";
            var contacts = new List<Contact>
                     {
                    new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                    new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                    };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null,search)).Returns(response);

            //Act
            var actual = target.GetTotalContacts(null,search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null,search), Times.Once);
        }

        [Fact]
        public void GetTotalContacts_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
                     {
                    new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                    new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                    };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = "d";
            var search = "no";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter,search)).Returns(response);

            //Act
            var actual = target.GetTotalContacts(letter,search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter,search), Times.Once);
        }

        [Fact]
        public void GetTotalContacts_ReturnsNotFound_WhenLetterIsNull()
        {
            //Arrange
            var search = "no";
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(null, search)).Returns(response);

            //Act
            var actual = target.GetTotalContacts(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(null, search), Times.Once);
        }

        [Fact]
        public void GetTotalContacts_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = "d";
            var search = "no";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContacts(letter, search)).Returns(response);

            //Act
            var actual = target.GetTotalContacts(letter, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetAllPaginatedContacts_ReturnsOkWithContacts_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize,sort)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize,sort), Times.Once);
        }

        [Fact]
        public void GetAllPaginatedContacts_ReturnsNotFound_WhenNoContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize,sort)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize,sort) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, sort), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsOkWithContacts_WhenLetterIsNotNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<ContactDto>
                    {
                       new ContactDto{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                       new ContactDto{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            string letter = "C";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize,letter, sort,search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsOkWithContacts_WhenLetterIsNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<ContactDto>
                    {
                       new ContactDto{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                       new ContactDto{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            string letter = null;
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, sort, search) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsNotFound_WhenLetterIsNotNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            string letter = "C";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, sort, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsNotFound_WhenLetterIsNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            string letter = null;
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(response);

            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, letter, sort, search) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }
        [Fact]
        public void GetTotalContactsForFavourite_ReturnsOkWithContacts_WhenLetterIsNull()
        {
            //Arrange
            var contacts = new List<Contact>
                     {
                    new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890",Favourite=true},
                    new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899",Favourite=true},
                    };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactsForFavourite(null)).Returns(response);

            //Act
            var actual = target.GetTotalContactsForFavourite(null) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContactsForFavourite(null), Times.Once);
        }

        [Fact]
        public void GetTotalContactsForFavourite_ReturnsOkWithContacts_WhenLetterIsNotNull()
        {
            //Arrange
            var contacts = new List<Contact>
                     {
                    new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890",Favourite=true},
                    new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899",Favourite=true},
                    };


            var response = new ServiceResponse<int>
            {
                Success = true,
                Data = contacts.Count
            };

            var letter = 'a' ;
            var search = "no";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactsForFavourite(letter)).Returns(response);

            //Act
            var actual = target.GetTotalContactsForFavourite(letter) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(2, response.Data);
            mockContactService.Verify(c => c.TotalContactsForFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetTotalContactsForFavourite_ReturnsNotFound_WhenLetterIsNull()
        {
            //Arrange
            var search = "no";
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactsForFavourite(null)).Returns(response);

            //Act
            var actual = target.GetTotalContactsForFavourite(null) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContactsForFavourite(null), Times.Once);
        }

        [Fact]
        public void GetTotalContactsForFavourite_ReturnsNotFound_WhenLetterIsNotNull()
        {
            //Arrange
            var response = new ServiceResponse<int>
            {
                Success = false,
                Data = 0
            };

            var letter = 'a';
            var search = "no";
            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.TotalContactsForFavourite(letter)).Returns(response);

            //Act
            var actual = target.GetTotalContactsForFavourite(letter) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            Assert.Equal(0, response.Data);
            mockContactService.Verify(c => c.TotalContactsForFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetAllPaginatedContactsForFavourite_ReturnsOkWithContacts_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899", Favourite = true},
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890", Favourite = true},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";

            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort), Times.Once);
        }

        [Fact]
        public void GetAllPaginatedContactsForFavourite_ReturnsNotFound_WhenNoContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetterForFavourite_ReturnsOkWithContacts_WhenLetterIsNotNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<ContactDto>
                    {
                       new ContactDto{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899",Favourite=true},
                       new ContactDto{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890",Favourite=true},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            char letter = 'c';
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, letter)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, letter, sort) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, letter), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetterForFavourite_ReturnsOkWithContacts_WhenLetterIsNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<ContactDto>
                    {
                       new ContactDto{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899",Favourite=true},
                       new ContactDto{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890",Favourite=true},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = true,
                //Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, null, sort) as OkObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(200, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, null), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetterForFavourite_ReturnsNotFound_WhenLetterIsNotNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            char letter = 'c';
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, letter)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, letter, sort) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, letter), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetterForFavourite_ReturnsNotFound_WhenLetterIsNull_WhenContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
                    {
                       new Contact{ContactId=1,FirstName="Contact 1", ContactNumber = "1234567890"},
                       new Contact{ContactId=2,FirstName="Contact 2", ContactNumber = "1234567899"},
                     };

            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string search = "no";
            var response = new ServiceResponse<IEnumerable<ContactDto>>
            {
                Success = false,
                Data = contacts.Select(c => new ContactDto { ContactId = c.ContactId, FirstName = c.FirstName, ContactNumber = c.ContactNumber }) // Convert to ContactDto
            };

            var mockContactService = new Mock<IContactService>();
            var target = new ContactController(mockContactService.Object);
            mockContactService.Setup(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, null)).Returns(response);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, null, sort) as NotFoundObjectResult;

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(404, actual.StatusCode);
            Assert.NotNull(actual.Value);
            Assert.Equal(response, actual.Value);
            mockContactService.Verify(c => c.GetPaginatedContactsForFavourite(page, pageSize, sort, null), Times.Once);
        }
    }
}
