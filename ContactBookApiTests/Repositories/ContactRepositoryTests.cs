using ContactBookApi.Data;
using ContactBookApi.Data.Implementation;
using ContactBookApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Repositories
{
    public class ContactRepositoryTests
    {
        [Fact]
        public void GetAllByLetter_ReturnsCategories_WhenContactsExists()
        {
            //Arrange
            char letter = 'C';
            var contacts = new List<Contact>
            {
                new Contact{  ContactId = 1,
                FirstName = "contact1"},
                new Contact{ ContactId = 2,
                FirstName = "contact2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.GetAll(letter);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }
        [Fact]
        public void GetAllByLetter_ReturnsCategories_WhenNoContactsExists()
        {
            //Arrange
            char letter = 'C';
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAll(letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }
        [Fact]
        public void GetAllContacts_ReturnsCategories_WhenCategoriesExists()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact{  ContactId = 1,
                FirstName = "C1"},
                new Contact{ ContactId = 2,
                FirstName = "C2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.GetAllContacts();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void GetAllContacts_ReturnsCategories_WhenCategoriesNotExists()
        {
            //Arrange
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllContacts();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsFavouriteContact_WhenFavouriteContactExists()
        {
            //Arrange
            var contacts = new List<Contact>
            {
                new Contact{  ContactId = 1,
                FirstName = "C1",
                Favourite=true},
                new Contact{ ContactId = 2,
                FirstName = "C2"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.GetAllFavouriteContacts();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(1, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsNoContacts_WhenFavouriteContactNotExists()
        {
            //Arrange
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetAllFavouriteContacts();
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockAbContext.Verify(c => c.Contacts, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));

        }

        [Fact]
        public void GetContactById_WhenContactIsNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void GetContactById_WhenContactIsNotNull()
        {
            //Arrange
            var id = 1;
            var contacts = new List<Contact>()
            {
              new Contact { ContactId = 1, FirstName = "Contact 1" },
                new Contact { ContactId = 2, FirstName = "Contact 2" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockAbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);
            //Act
            var actual = target.GetContact(id);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void InsertContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.InsertContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void InsertContact_ReturnsFalse()
        {
            //Arrange
            Contact contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.InsertContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void UpdateContact_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<Contact>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Contacts).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new ContactRepository(mockAppDbContext.Object);
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "C1"
            };


            //Act
            var actual = target.UpdateContact(contact);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(contact), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateContact_ReturnsFalse()
        {
            //Arrange
            Contact contact = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateContact(contact);
            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void DeleteContact_ReturnsTrue()
        {
            // Arrange
            var id = 1;
            var contact = new Contact { ContactId = id };
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Contacts.Find(id)).Returns(contact);
            var target = new ContactRepository(mockContext.Object);
            // Act
            var result = target.DeleteContact(id);

            // Assert
            Assert.True(result);
            mockContext.Verify(c => c.Contacts.Remove(contact), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            mockContext.Verify(c => c.Contacts.Find(id), Times.Once);
        }

        [Fact]
        public void DeleteContact_ReturnsFalse()
        {
            // Arrange
            var id = 1;
            Contact contact = null;
            var mockContext = new Mock<IAppDbContext>();
            mockContext.Setup(c => c.Contacts.Find(id)).Returns(contact);
            var target = new ContactRepository(mockContext.Object);
            // Act
            var result = target.DeleteContact(id);

            // Assert
            Assert.False(result);
            mockContext.Verify(c => c.Contacts.Find(id), Times.Once);
        }
        [Fact]
        public void ContactExists_ReturnsTrue()
        {
            //Arrange
            var number = "1234567890";
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Contact 1", ContactNumber="1234567890"},
                new Contact { ContactId = 2, FirstName = "Contact 2", ContactNumber="9876543216" },
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(number);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExists_ReturnsFalse()
        {
            //Arrange
            var number = "1234567890";
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(number);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsFalse()
        {
            //Arrange
            string phone = "1234567890";
            int id = 1;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void ContactExistsIdName_ReturnsTrue()
        {
            //Arrange
            string phone = "1234567890";
            int id = 3;
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, FirstName = "Contact 1", ContactNumber="1234567890" },
                new Contact { ContactId = 2, FirstName = "Contact 2" , ContactNumber="9876543219"},
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAbContext.Object);

            //Act
            var actual = target.ContactExists(id, phone);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExist_SearchIsYes()
        {
            string? letter = "o";
            string search = "yes";
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(1));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void TotalContacts_ReturnsCount_WhenContactsExist_SearchIsNo_LetterIsNotNull()
        {
            string? letter = "o";
            string search = "no";
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(1));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }
        [Fact]
        public void TotalContacts_ReturnsTotalCount_WhenContactsExist_SearchIsNo_LetterIsNull()
        {
            string? letter = null;
            string search = "no";
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContacts(letter, search);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(contacts.Count(), actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(1));
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExists_WhenSortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_WhenSearchIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactsExists_WhenSortIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsEmptyList_WhenNoContactsExists_WhenSearchIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterNull_SortIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            string letter = null;
            string search = "yes";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterNull_SortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            string letter = null;
            string search = "yes";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_SearchIsYes_SortIsNotAsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            string letter = "c";
            string search = "yes";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort,letter,search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_SearchIsYes_SortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            string letter = "c";
            string search = "yes";
            var contacts = new List<Contact>
      {
          new Contact{ContactId=1, FirstName="Contact 1"},
          new Contact{ContactId=2, FirstName="Contact 2"},
          new Contact{ContactId=3, FirstName="Contact 3"},

      }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_SearchIsNo_SortIsNotAsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            string letter = "C";
            string search = "no";
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1"},
                  new Contact{ContactId=2, FirstName="Contact 2"},
                  new Contact{ContactId=3, FirstName="Contact 3"},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_SearchIsNo_SortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            string letter = "C";
            string search = "no";
            var contacts = new List<Contact>
      {
          new Contact{ContactId=1, FirstName="Contact 1"},
          new Contact{ContactId=2, FirstName="Contact 2"},
          new Contact{ContactId=3, FirstName="Contact 3"},

      }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsWithNoContacts_WhenNoContactsExists_WhenLetterIsNotNull_SearchIsYes_SortIsNotAsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            string letter = "c";
            string search = "yes";
            var contacts = new List<Contact>{}.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsWithNoContacts_WhenNoContactsExists_WhenLetterIsNotNull_SearchIsYes_SortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            string letter = "c";
            string search = "yes";
            var contacts = new List<Contact>{}.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsWithNoContacts_WhenNoContactsExists_WhenLetterIsNotNull_SearchIsNo_SortIsNotAsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            string letter = "C";
            string search = "no";
            var contacts = new List<Contact>{}.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsWithNoContact_WhenNoContactsExists_WhenLetterIsNotNull_SearchIsNo_SortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            string letter = "C";
            string search = "no";
            var contacts = new List<Contact>{}.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContacts(page, pageSize, sort, letter, search);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(0, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsCount_WhenContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactsForFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNull()
        {
            char? letter = null;
            var contacts = new List<Contact>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactsForFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsCount_WhenContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact> {
                new Contact {ContactId = 1,FirstName="Contact 1"},
                new Contact {ContactId = 2,FirstName="Contact 2"}
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactsForFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsCountZero_WhenNoContactsExistWhenLetterIsNotNull()
        {
            char? letter = 'c';
            var contacts = new List<Contact>
            {

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.TotalContactsForFavourite(letter);

            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);

        }

        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsContacts_WhenContactsExists_WhenSortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1",Favourite=true},
                  new Contact{ContactId=2, FirstName="Contact 2",Favourite=true},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsEmptyList_WhenNoContactsExists_WhenSearchIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsContacts_WhenContactsExists_WhenSortIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1",Favourite=true},
                  new Contact{ContactId=2, FirstName="Contact 2",Favourite=true},

              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsEmptyList_WhenNoContactsExists_WhenSearchIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_WhenSortIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            char letter = 'C';
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1",Favourite=true},
                  new Contact{ContactId=2, FirstName="Contact 2",Favourite=true},
                  new Contact{ContactId=3, FirstName="Contact 3",Favourite=true},


              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort,letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsEmptyList_WhenNoContactsExists_WhenLetterIsNotNull_WhenSearchIsAsc()
        {
            string sort = "asc";
            int page = 1;
            int pageSize = 2;
            char letter = 'C';
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort,letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsContacts_WhenContactsExists_WhenLetterIsNotNull_WhenSortIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            char letter = 'C';
            var contacts = new List<Contact>
              {
                  new Contact{ContactId=1, FirstName="Contact 1",Favourite=true},
                  new Contact{ContactId=2, FirstName="Contact 2",Favourite=true},
                  new Contact{ContactId=3, FirstName="Contact 3",Favourite=true},
              }.AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);

            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort, letter);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(pageSize, actual.Count());
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsEmptyList_WhenNoContactsExists_WhenLetterIsNotNull_WhenSearchIsDsc()
        {
            string sort = "dsc";
            int page = 1;
            int pageSize = 2;
            char letter = 'C';
            var contacts = new List<Contact>().AsQueryable();
            var mockDbSet = new Mock<DbSet<Contact>>();
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Expression).Returns(contacts.Expression);
            mockDbSet.As<IQueryable<Contact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.Setup(c => c.Contacts).Returns(mockDbSet.Object);
            var target = new ContactRepository(mockAppDbContext.Object);
            //Act
            var actual = target.GetPaginatedContactsForFavourite(page, pageSize, sort,letter);
            //Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Expression, Times.Once);
            mockDbSet.As<IQueryable<Contact>>().Verify(c => c.Provider, Times.Exactly(3));
            mockAppDbContext.Verify(c => c.Contacts, Times.Once);
        }
    }
}
