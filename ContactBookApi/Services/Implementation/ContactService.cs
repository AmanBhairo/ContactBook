using ContactBookApi.Data.Contract;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;

namespace ContactBookApi.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetContacts(char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAll(letter);
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach(var contact in contacts)
                {
                    contactDtos.Add(new ContactDto() 
                    { 
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto() 
                        { 
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto() 
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }

                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetAllContacts()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllContacts();
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetAllFavouriteContacts()
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetAllFavouriteContacts();
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }

        public ServiceResponse<ContactDto> GetContact(int id)
        {
            var response = new ServiceResponse<ContactDto>();
            var contact = _contactRepository.GetContact(id);
            if(contact != null)
            {
                var contactDto = new ContactDto()
                {
                    ContactId = contact.ContactId,
                    StateId = contact.StateId,
                    CountryId = contact.CountryId,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    ContactNumber = contact.ContactNumber,
                    Email = contact.Email,
                    ContactDescription = contact.ContactDescription,
                    ProfilePic = contact.ProfilePic,
                    Gender = contact.Gender,
                    Address = contact.Address,
                    Favourite = contact.Favourite,
                    ImageByte = contact.ImageByte,
                    BirthDate = contact.BirthDate,
                    State = new StateContactDto()
                    {
                        StateId = contact.State.StateId,
                        StateName = contact.State.StateName,
                        CountryId = contact.State.CountryId,
                    },
                    Country = new CountryContactDto()
                    {
                        CountryId = contact.Country.CountryId,
                        CountryName = contact.Country.CountryName,
                    }
                };
                response.Data = contactDto;
            }
            else
            {
                response.Success = false;
                response.Message = "N0 record found";
            }
            return response;

        }

        public ServiceResponse<string> AddContact(Contact contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact Already Exist";
                return response;
            }

            var result = _contactRepository.InsertContact(contact);
            if (result)
            {
                response.Message = "Contact Saved Successfully";
            }
            else
            {
                response.Message = "Something went wrong try after some time";
            }
            return response;
        }

        public ServiceResponse<string> ModifyContact(Contact contact)
        {
            var response = new ServiceResponse<string>();
            if (_contactRepository.ContactExists(contact.ContactId, contact.ContactNumber))
            {
                response.Success = false;
                response.Message = "Contact Exists!";
                return response;
            }
            var existingCategory = _contactRepository.GetContact(contact.ContactId);
            var result = false;
            if (existingCategory != null)
            {
                existingCategory.StateId = contact.StateId;
                existingCategory.CountryId = contact.CountryId;
                existingCategory.FirstName = contact.FirstName;
                existingCategory.LastName = contact.LastName;
                existingCategory.ContactNumber = contact.ContactNumber;
                existingCategory.Email = contact.Email;
                existingCategory.ContactDescription = contact.ContactDescription;
                existingCategory.ProfilePic = contact.ProfilePic;
                existingCategory.Gender = contact.Gender;
                existingCategory.Address = contact.Address;
                existingCategory.Favourite = contact.Favourite;
                existingCategory.ImageByte = contact.ImageByte;
                existingCategory.BirthDate = contact.BirthDate;
                result = _contactRepository.UpdateContact(existingCategory);
            }
            if (result)
            {
                response.Message = "Contact Updated Successfully!";
            }
            else
            {
                response.Message = "Something went wrong please try after sometime";
            }
            return response;
        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            var result = _contactRepository.DeleteContact(id);
            if (result)
            {
                response.Success = true;
                response.Message = "Contact deleted Successfully";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong try after some time";
            }
            return response;
        }


        public ServiceResponse<int> TotalContacts(string? letter, string search)
        {
            var response = new ServiceResponse<int>();
            var result = _contactRepository.TotalContacts(letter, search);
            if (result > 0)
            {
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Message = "No contact found";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize,string sort)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, sort);
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContacts(int page, int pageSize, string sort, string? letter, string search)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContacts(page, pageSize, sort, letter,search);
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else if(contacts == null)
            {
                response.Success = true;
                response.Message = "No record found!";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong";

            }

            return response;
        }
        public ServiceResponse<int> TotalContactsForFavourite(char? letter)
        {
            var response = new ServiceResponse<int>();
            var result = _contactRepository.TotalContactsForFavourite(letter);
            if (result > 0)
            {
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Message = "No contact found";
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsForFavourite(int page, int pageSize, string sort)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContactsForFavourite(page, pageSize, sort);
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsForFavourite(int page, int pageSize, string sort, char? letter)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            var contacts = _contactRepository.GetPaginatedContactsForFavourite(page, pageSize, sort, letter);
            List<ContactDto> contactDtos = new List<ContactDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactDto()
                    {
                        ContactId = contact.ContactId,
                        StateId = contact.StateId,
                        CountryId = contact.CountryId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                        State = new StateContactDto()
                        {
                            StateId = contact.State.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.State.CountryId,
                        },
                        Country = new CountryContactDto()
                        {
                            CountryId = contact.Country.CountryId,
                            CountryName = contact.Country.CountryName,
                        }
                    });
                }
                response.Data = contactDtos;
            }
            else if (contacts == null)
            {
                response.Success = true;
                response.Message = "No record found!";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong";

            }

            return response;
        }

        //Report
        public ServiceResponse<IEnumerable<ContactRecordReportDto>> GetContactRecordBasedOnBirthdayMonthReport(int month)
        {
            var response = new ServiceResponse<IEnumerable<ContactRecordReportDto>>();
            var contacts = _contactRepository.GetContactRecordBasedOnBirthdayMonthReport(month);
            List<ContactRecordReportDto> contactDtos = new List<ContactRecordReportDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactRecordReportDto()
                    {
                        ContactId = contact.ContactId,
                        StateName = contact.StateName,
                        CountryName = contact.CountryName,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }
        public ServiceResponse<IEnumerable<ContactRecordReportDto>> GetContactsByState(int state)
        {
            var response = new ServiceResponse<IEnumerable<ContactRecordReportDto>>();
            var contacts = _contactRepository.GetContactsByState(state);
            List<ContactRecordReportDto> contactDtos = new List<ContactRecordReportDto>();
            if (contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    contactDtos.Add(new ContactRecordReportDto()
                    {
                        ContactId = contact.ContactId,
                        StateName = contact.StateName,
                        CountryName = contact.CountryName,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ContactNumber = contact.ContactNumber,
                        Email = contact.Email,
                        ContactDescription = contact.ContactDescription,
                        ProfilePic = contact.ProfilePic,
                        Gender = contact.Gender,
                        Address = contact.Address,
                        Favourite = contact.Favourite,
                        ImageByte = contact.ImageByte,
                        BirthDate = contact.BirthDate,
                    });
                }
                response.Data = contactDtos;
            }
            else
            {
                response.Success = false;
                response.Message = "No record found!";
            }

            return response;
        }
        public ServiceResponse<int> GetContactsCountByCountry(int country)
        {
            var response = new ServiceResponse<int>();
            var result = _contactRepository.GetContactsCountByCountry(country);
            if (result > 0)
            {
                response.Data = result;
            }
            else if(result == 0)
            {
                response.Success = true;
                response.Message = "No contact found";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong please try after sometime.";
            }
            return response;
        }

        public ServiceResponse<int> GetContactCountByGender(string gender)
        {
            var response = new ServiceResponse<int>();
            var genderInUpperCase = gender.ToUpper();
            char genderParam ='M';
            if(genderInUpperCase == "M" || genderInUpperCase =="MALE")
            {
                genderParam = 'M';
            }
            else if(genderInUpperCase == "F" || genderInUpperCase == "FEMALE")
            {
                genderParam = 'F';
            }
            else
            {
                response.Success = false;
                response.Message = "Please select valid Gender";
                return response;
            }
            var result = _contactRepository.GetContactCountByGender(genderParam);
            if (result > 0)
            {
                response.Data = result;
            }
            else if (result == 0)
            {
                response.Success = true;
                response.Message = "No contact found";
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong please try after sometime.";
            }
            return response;
        }
    }
}
