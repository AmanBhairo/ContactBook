using ContactBookApi.Data.Contract;
using ContactBookApi.Models;
using ContactBookApi.Services.Implementation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApiTests.Services
{
    public class ContactServiceTests
    {
        [Fact]
        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNull()
        {

            // Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(null);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(null), Times.Once);
        }

        [Fact]
        public void GetContacts_ReturnsNoRecordFound_WhenNoContactsExistAndLetterIsNull()
        {
            // Arrange
            var contacts = new List<Contact>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(null)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(null);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(null), Times.Once);
        }

        [Fact]
        public void GetContacts_ReturnsContacts_WhenContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';
            // Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            mockRepository.Verify(r => r.GetAll(letter), Times.Once);
        }

        [Fact]
        public void GetContacts_ReturnsRecord_WhenNoContactsExistAndLetterIsNotNull()
        {
            var letter = 'a';

            // Arrange
            var contacts = new List<Contact>();


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAll(letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContacts(letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAll(letter), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsContacts_WhenContactsExist()
        {

            // Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllContacts()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetAllContacts();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count(), actual.Data.Count());
            mockRepository.Verify(r => r.GetAllContacts(), Times.Once);
        }

        [Fact]
        public void GetAllContacts_ReturnsNoRecordFound_WhenNoContactsExist()
        {
            // Arrange
            List<Contact> contacts = null;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllContacts()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetAllContacts();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAllContacts(), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsContacts_WhenContactsExist()
        {

            // Arrange
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = true,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetAllFavouriteContacts();

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(2, actual.Data.Count());
            mockRepository.Verify(r => r.GetAllFavouriteContacts(), Times.Once);
        }

        [Fact]
        public void GetAllFavouriteContacts_ReturnsNoRecordFound_WhenNoContactsExist()
        {
            // Arrange
            List<Contact> contacts = null;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetAllFavouriteContacts()).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetAllFavouriteContacts();

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetAllFavouriteContacts(), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsContacts_WhenContactsExist()
        {

            // Arrange
            int contactTd = 1;
            var contacts = new Contact()
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactTd)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactTd);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            mockRepository.Verify(r => r.GetContact(contactTd), Times.Once);
        }

        [Fact]
        public void GetContactById_ReturnsNoRecordFound_WhenNoContactsExist()
        {
            // Arrange
            int contactTd = 1;
            Contact contacts = null;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetContact(contactTd)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetContact(contactTd);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("N0 record found", actual.Message);
            mockRepository.Verify(r => r.GetContact(contactTd), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactAlreadyExists()
        {
            //Arrange
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(true);
            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Already Exist", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnsContactUpdatedSuccessfully()
        {
            //Arrange
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(true);
            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Contact Saved Successfully", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);
        }

        [Fact]
        public void AddContact_ReturnSomethingWentWrong()
        {
            //Arrange
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
            };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contact.ContactNumber)).Returns(false);
            mockRepository.Setup(r => r.InsertContact(contact)).Returns(false);
            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.AddContact(contact);

            //Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Something went wrong try after some time", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contact.ContactNumber), Times.Once);
            mockRepository.Verify(r => r.InsertContact(contact), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsAlreadyExists_WhenContactAlreadyExists()
        {
            var contactId = 1;
            var contact = new Contact()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.ContactExists(contactId, contact.ContactNumber)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(contact);


            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.Success);
            Assert.Equal("Contact Exists!", actual.Message);
            mockRepository.Verify(r => r.ContactExists(contactId, contact.ContactNumber), Times.Once);
        }
        [Fact]
        public void ModifyContact_ReturnsSomethingWentWrong_WhenContactNotFound()
        {
            var contactId = 1;
            var existingContact = new Contact()
            {
                ContactId = contactId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1

            };

            var updatedContact = new Contact()
            {
                ContactId = contactId,
                FirstName = "C1"
            };


            var mockRepository = new Mock<IContactRepository>();
            //mockRepository.Setup(r => r.ContactExist(contactId, updatedContact.Phone)).Returns(false);
            mockRepository.Setup(r => r.GetContact(updatedContact.ContactId)).Returns<IEnumerable<Contact>>(null);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.ModifyContact(existingContact);


            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            //mockRepository.Verify(r => r.ContactExist(contactId, updatedContact.Phone), Times.Once);
            mockRepository.Verify(r => r.GetContact(contactId), Times.Once);
        }

        [Fact]
        public void ModifyContact_ReturnsUpdatedSuccessfully_WhenContactModifiedSuccessfully()
        {

            //Arrange
            var existingContact = new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1

            };

            var updatedContact = new Contact
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(true);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Contact Updated Successfully!", actual.Message);

            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);


            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }
        [Fact]
        public void ModifyContact_ReturnsError_WhenContactModifiedFails()
        {

            //Arrange
            var existingContact = new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1
            };

            var updatedContact = new Contact
            {
                ContactId = 1,
                FirstName = "Contact 1"
            };

            var mockContactRepository = new Mock<IContactRepository>();

            mockContactRepository.Setup(c => c.ContactExists(updatedContact.ContactId, updatedContact.ContactNumber)).Returns(false);
            mockContactRepository.Setup(c => c.GetContact(updatedContact.ContactId)).Returns(existingContact);
            mockContactRepository.Setup(c => c.UpdateContact(existingContact)).Returns(false);

            var target = new ContactService(mockContactRepository.Object);

            //Act

            var actual = target.ModifyContact(updatedContact);


            //Assert
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong please try after sometime", actual.Message);
            mockContactRepository.Verify(c => c.GetContact(updatedContact.ContactId), Times.Once);
            mockContactRepository.Verify(c => c.UpdateContact(existingContact), Times.Once);

        }

        [Fact]
        public void RemoveContact_ReturnsDeletedSuccessfully_WhenDeletedSuccessfully()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(true);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Contact deleted Successfully", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void RemoveContact_SomethingWentWrong_WhenDeletionFailed()
        {
            var contactId = 1;


            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.DeleteContact(contactId)).Returns(false);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.RemoveContact(contactId);

            // Assert
            Assert.False(actual.Success);
            Assert.NotNull(actual);
            Assert.Equal("Something went wrong try after some time", actual.Message);
            mockRepository.Verify(r => r.DeleteContact(contactId), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsContactCount_WhenContactExist()
        {
            //Arrange
            string search = "no";
            string letter = "a";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter, search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void TotalContacts_ReturnsNoContactFound_WhenNoContactExist()
        {
            //Arrange
            string search = "no";
            string letter = "a";
            var contacts = new List<Contact> { };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContacts(letter, search)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContacts(letter, search);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            Assert.Equal("No contact found", actual.Message);
            mockRepository.Verify(r => r.TotalContacts(letter, search), Times.Once);
        }

        [Fact]
        public void GetPaginatedContacts_ReturnsContacts_WhenContactExist()
        {
            // Arrange
            int page =1;
            int pageSize = 2;
            string sort = "asc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page,pageSize,sort)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, sort);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            Assert.Equal(pageSize,actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, sort), Times.Once);
        }
        [Fact]
        public void GetPaginatedContacts_ReturnsNoRecordFound_WhenNoContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var contacts = new List<Contact>();

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, sort)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, sort);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, sort), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsContacts_WhenContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string letter = "a";
            string search = "no";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, sort,letter,search);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            Assert.Equal(pageSize, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsNoRecordFound_WhenNoContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string letter = "a";
            string search = "no";
            List<Contact> contacts = null;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, sort, letter, search);

            // Assert
            Assert.True(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsByLetter_ReturnsSomethingWent()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            string letter = "a";
            string search = "no";
            var contacts = new List<Contact>();

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContacts(page, pageSize, sort, letter, search);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Something went wrong", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContacts(page, pageSize, sort, letter, search), Times.Once);
        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsContactCount_WhenContactExist()
        {
            //Arrange
            string search = "no";
            char letter = 'a';
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John"

            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane"

            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContactsForFavourite(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContactsForFavourite(letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            mockRepository.Verify(r => r.TotalContactsForFavourite(letter), Times.Once);
        }

        [Fact]
        public void TotalContactsForFavourite_ReturnsNoContactFound_WhenNoContactExist()
        {
            //Arrange
            string search = "no";
            char letter = 'a';
            var contacts = new List<Contact> { };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.TotalContactsForFavourite(letter)).Returns(contacts.Count);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.TotalContactsForFavourite(letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Equal(contacts.Count, actual.Data);
            Assert.Equal("No contact found", actual.Message);
            mockRepository.Verify(r => r.TotalContactsForFavourite(letter), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsContacts_WhenContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContactsForFavourite(page, pageSize, sort);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            Assert.Equal(pageSize, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavourite_ReturnsNoRecordFound_WhenNoContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            List<Contact> contacts = null ;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContactsForFavourite(page, pageSize, sort);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort), Times.Once);
        }

        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsContacts_WhenContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            char letter = 'a';
            string search = "no";
            var contacts = new List<Contact>
        {
            new Contact
            {
                ContactId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                ContactNumber = "1234567890",
                Address = "123 Main St",
                ProfilePic = "file1.txt",
                Gender = "Male",
                Favourite = true,
                CountryId = 1,
                StateId = 1,
                Country = new Country
                {
                    CountryId = 1,
                    CountryName = "USA"
                },
                State = new State
                {
                    StateId = 1,
                    StateName = "California"
                }
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                ContactNumber = "9876543210",
                Address = "456 Elm St",
                ProfilePic = "file2.txt",
                Gender = "Female",
                Favourite = false,
                CountryId = 2,
                StateId = 2,
                Country = new Country
                {
                    CountryId = 2,
                    CountryName = "Canada"
                },
                State = new State
                {
                    StateId = 2,
                    StateName = "Ontario"
                }
            }
        };

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContactsForFavourite(page, pageSize, sort, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.NotNull(actual.Data);
            Assert.Equal(contacts.Count, actual.Data.Count());
            Assert.Equal(pageSize, actual.Data.Count());
            mockRepository.Verify(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsNoRecordFound_WhenNoContactExist()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            char letter = 'a';
            string search = "no";
            List<Contact> contacts = null;

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContactsForFavourite(page, pageSize, sort, letter);

            // Assert
            Assert.True(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("No record found!", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter), Times.Once);
        }
        [Fact]
        public void GetPaginatedContactsForFavouriteByLetter_ReturnsSomethingWent()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            string sort = "asc";
            char letter = 'a';
            string search = "no";
            var contacts = new List<Contact>();

            var mockRepository = new Mock<IContactRepository>();
            mockRepository.Setup(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter)).Returns(contacts);

            var contactService = new ContactService(mockRepository.Object);

            // Act
            var actual = contactService.GetPaginatedContactsForFavourite(page, pageSize, sort, letter);

            // Assert
            Assert.False(actual.Success);
            Assert.Null(actual.Data);
            Assert.Equal("Something went wrong", actual.Message);
            mockRepository.Verify(r => r.GetPaginatedContactsForFavourite(page, pageSize, sort, letter), Times.Once);
        }
    }
}
