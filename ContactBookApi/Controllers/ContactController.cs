using ContactBookApi.Data;
using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("GetContactsByletter/{letter}")]
        public IActionResult GetContactsByChar(char letter)
        {
            var response = _contactService.GetContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var response = _contactService.GetAllContacts();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllFavouriteContacts")]
        public IActionResult GetAllFavouriteContacts()
        {
            var response = _contactService.GetAllFavouriteContacts();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactById/{id}")]
        [Authorize]
        public IActionResult GetContactById(int id) 
        {
            if (id > 0)
            {
                var result = _contactService.GetContact(id);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest("Please enter proper data");
            }
        }

        [HttpPost("CreateContact")]
        [Authorize]
        public IActionResult CreateContact(AddcontactDto contactDto)
        {
            var contact = new Contact()
            {
                StateId = contactDto.StateId,
                CountryId = contactDto.CountryId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Email = contactDto.Email,
                ContactDescription = contactDto.ContactDescription,
                ProfilePic = contactDto.ProfilePic,
                Gender = contactDto.Gender,
                Address = contactDto.Address,
                Favourite = contactDto.Favourite,
                ImageByte = contactDto.ImageByte,
                BirthDate = contactDto.BirthDate,
            };
            var result = _contactService.AddContact(contact);
            return !result.Success ? BadRequest(result) : Ok(result);
        }

        [HttpPut("EditContact")]
        [Authorize]
        public IActionResult UpdateContact(UpdateContactDto contactDto)
        {
            var contact = new Contact()
            {
                ContactId = contactDto.ContactId,
                StateId = contactDto.StateId,
                CountryId = contactDto.CountryId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                ContactNumber = contactDto.ContactNumber,
                Email = contactDto.Email,
                ContactDescription = contactDto.ContactDescription,
                ProfilePic = contactDto.ProfilePic,
                Gender = contactDto.Gender,
                Address = contactDto.Address,
                Favourite = contactDto.Favourite,
                ImageByte = contactDto.ImageByte,
                BirthDate = contactDto.BirthDate,
            };

            var response = _contactService.ModifyContact(contact);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteContact/{id}")]
        [Authorize]
        public IActionResult DeleteContact(int id)
        {
            if (id > 0)
            {
                var result = _contactService.RemoveContact(id);
                return !result.Success ? BadRequest(result) : Ok(result);
            }
            else
            {
                return BadRequest("Please enter proper data");
            }
        }

        [HttpGet("TotalContacts")]
        public IActionResult GetTotalContacts(string? letter, string search ="no")
        {
            var response = _contactService.TotalContacts(letter, search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllPaginatedContacts")]
        public IActionResult GetPaginatedContacts(int page = 1, int pageSize = 2, string sort ="asc")
        {
            var response = _contactService.GetPaginatedContacts(page,pageSize, sort);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetPaginatedContactsByLetter")]
        public IActionResult GetPaginatedContacts(int page, int pageSize, string? letter, string sort = "asc", string search = "no")
        {
            var response = _contactService.GetPaginatedContacts(page, pageSize, sort, letter, search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("TotalContactsForFavourite")]
        public IActionResult GetTotalContactsForFavourite(char? letter)
        {
            var response = _contactService.TotalContactsForFavourite(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllPaginatedContactsForFavourite")]
        public IActionResult GetPaginatedContactsForFavourite(int page = 1, int pageSize = 2, string sort = "asc")
        {
            var response = _contactService.GetPaginatedContactsForFavourite(page, pageSize, sort);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetPaginatedContactsByLetterForFavourite")]
        public IActionResult GetPaginatedContactsForFavourite(int page, int pageSize, char? letter, string sort="asc")
        {
            var response = _contactService.GetPaginatedContactsForFavourite(page, pageSize, sort,letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //Report
        [HttpGet("GetContactRecordBasedOnBirthdayMonthReport/{month}")]
        public IActionResult GetContactRecordBasedOnBirthdayMonthReport(int  month)
        {
            var response = _contactService.GetContactRecordBasedOnBirthdayMonthReport(month);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactsByStateReport/{state}")]
        public IActionResult GetContactsByStateReport(int state)
        {
            var response = _contactService.GetContactsByState(state);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactsCountByCountryReport/{country}")]
        public IActionResult GetContactsCountByCountryReport(int country)
        {
            var response = _contactService.GetContactsCountByCountry(country);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactCountByGenderReport/{gender}")]
        public IActionResult GetContactCountByGenderReport(string gender)
        {
            var response = _contactService.GetContactCountByGender(gender);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
