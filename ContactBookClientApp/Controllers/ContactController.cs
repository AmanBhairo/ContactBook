using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;

namespace ContactBookClientApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IAddImageFileToPathService _addImage;
        private string endPoint;

        public ContactController(IHttpClientService httpClientService, IConfiguration configuration, IAddImageFileToPathService addImage)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _addImage = addImage;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                ($"{endPoint}Contact/GetAllContacts", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return View(response.Data);
            }
            return View(new List<ContactViewModel>());
        }
        [AllowAnonymous]

        public IActionResult PaginatedIndex(string? letter, int page = 1, int pageSize = 2, string sort = "asc", string search = "no" )
        {
            var apiGetAllContactsUrl = $"{endPoint}Contact/GetAllContacts";

            ServiceResponse<IEnumerable<ContactViewModel>> responseForAllContacts = new ServiceResponse<IEnumerable<ContactViewModel>>();

            responseForAllContacts = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetAllContactsUrl, HttpMethod.Get, HttpContext.Request);

            if (responseForAllContacts.Success)
            {
                var ContactsForAlpbhabet = responseForAllContacts.Data;
                var firstLetters = ContactsForAlpbhabet.Select(c => c.FirstName.FirstOrDefault()).Distinct().OrderBy(l => l);

                ViewBag.FirstLetters = firstLetters;
            }

            var apiGetPositionsUrl = letter == null
                ? $"{endPoint}Contact/GetPaginatedContactsByLetter" + "?page=" + page + "&pageSize=" + pageSize + "&sort=" + sort + "&search="+search
                : $"{endPoint}Contact/GetPaginatedContactsByLetter" + "?page=" + page + "&pageSize=" + pageSize + "&letter=" + letter + "&sort="+ sort + "&search=" + search;

            
            var apiGetCountUrl = $"{endPoint}Contact/TotalContacts" + "?letter=" + letter + "&search=" + search;

            if (letter == null && search == "yes")
            {
                apiGetCountUrl = $"{endPoint}Contact/TotalContacts" + "?letter=" + letter + "&search=no";

            }

            ServiceResponse<int> countOfPosition = new ServiceResponse<int>();

            countOfPosition = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfPosition.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (totalPages != 0)
            {
                if (page > totalPages)
                {

                    return RedirectToAction("PaginatedIndex", new { page = totalPages, pageSize, letter, sort,search });
                }
            }
            

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.letter = letter;
            ViewBag.Sort = sort;
            ViewBag.Search = search;

            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetPositionsUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return View(response.Data);
            }
            else
            {
                TempData["noRecord"] = "No record found";
                return View(new List<ContactViewModel>());
            }
            
        }

        [AllowAnonymous]

        public IActionResult PaginatedIndexForFavourite(char? letter, int page = 1, int pageSize = 2,string sort ="asc")
        {
            var apiGetAllFavouriteContactsUrl = $"{endPoint}Contact/GetAllFavouriteContacts";

            ServiceResponse<IEnumerable<ContactViewModel>> responseForAllFavouriteContacts = new ServiceResponse<IEnumerable<ContactViewModel>>();

            responseForAllFavouriteContacts = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetAllFavouriteContactsUrl, HttpMethod.Get, HttpContext.Request);

            if (responseForAllFavouriteContacts.Success)
            {
                var FavouriteContactsForAlpbhabet = responseForAllFavouriteContacts.Data;
                var firstLetters = FavouriteContactsForAlpbhabet.Select(c => c.FirstName.FirstOrDefault()).Distinct().OrderBy(l => l);

                ViewBag.FirstLetters = firstLetters;
            }

                var apiGetPositionsUrl = letter == null
                ? $"{endPoint}Contact/GetPaginatedContactsByLetterForFavourite" + "?page=" + page + "&pageSize=" + pageSize + "&sort=" + sort
                : $"{endPoint}Contact/GetPaginatedContactsByLetterForFavourite" + "?page=" + page + "&pageSize=" + pageSize + "&letter=" + letter + "&sort=" + sort;


            var apiGetCountUrl = $"{endPoint}Contact/TotalContactsForFavourite" + "?letter=" + letter;

            ServiceResponse<int> countOfPosition = new ServiceResponse<int>();

            countOfPosition = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfPosition.Data;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            if (totalPages != 0)
            {
                if (page > totalPages)
                {

                    return RedirectToAction("PaginatedIndexForFavourite", new { page = totalPages, pageSize, letter, sort });
                }
            }


            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.letter = letter;
            ViewBag.Sort = sort;

            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetPositionsUrl, HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return View(response.Data);
            }
            else
            {
                TempData["noRecord"] = "No record found";
                return View(new List<ContactViewModel>());
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            AddContactViewModel addContactViewModel = new AddContactViewModel();
            addContactViewModel.countries = GetCountries();
            addContactViewModel.states = GetStates();
            return View(addContactViewModel);
        }
        [HttpPost]
        public IActionResult Create(AddContactViewModel viewModel)
        {
            //viewModel.countries = GetCountries();
            //viewModel.states = GetStates();
            if (ModelState.IsValid)
            {
                IFormFile imageFile = viewModel.File;
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = _addImage.AddImageFileToPath(imageFile);
                    viewModel.ProfilePic = fileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        viewModel.File.CopyTo(memoryStream);
                        viewModel.ImageByte = memoryStream.ToArray();
                    }
                }

                var apiUrl = $"{endPoint}Contact/CreateContact/";
                var response = _httpClientService.PostHttpResponseMessage<AddContactViewModel>(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["errorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                        return RedirectToAction("PaginatedIndex");
                    }
                }
            }
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateContactViewModel updateContactViewModel = serviceResponse.Data;
                    updateContactViewModel.countries = GetCountries();
                    updateContactViewModel.states = GetStates();
                    return View(updateContactViewModel);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("PaginatedIndex");
            }
        }
        [HttpPost]
        public IActionResult Edit(UpdateContactViewModel updateContact)
        {
            if (ModelState.IsValid)
            {
                if (updateContact.File != null && updateContact.File.Length > 0)
                {
                    var fileName = _addImage.AddImageFileToPath(updateContact.File);
                    updateContact.ProfilePic = fileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        updateContact.File.CopyTo(memoryStream);
                        updateContact.ImageByte = memoryStream.ToArray();
                    }
                }
                else if (updateContact.removeImageHidden == "true")
                {
                    updateContact.ProfilePic = null;
                    updateContact.ImageByte = null;
                }

                var apiUrl = $"{endPoint}Contact/EditContact";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateContact, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex");
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["errorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                        return RedirectToAction("PaginatedIndex");
                    }
                }
            }
            return View(updateContact);
        }

        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    if (serviceResponse.Data.ProfilePic == null || serviceResponse.Data.ProfilePic == "")
                    {
                        ViewBag.FileName = "MySampleImage.png";
                    }
                    else
                    {
                        ViewBag.FileName = serviceResponse.Data.ProfilePic;
                    }
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("PaginatedIndex");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("PaginatedIndex");
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/DeleteContact/" + contactId;
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);
            if (response.Success)
            {
                TempData["successMessage"] = response.Message;
                return RedirectToAction("PaginatedIndex");
            }
            else
            {
                TempData["errorMessage"] = response.Message;
            }
            return RedirectToAction("PaginatedIndex");
        }

        //private string AddImageFileToPath(IFormFile imageFile)

        //{

        //    // Process the uploaded file(eq. save it to disk)

        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", imageFile.FileName);

        //    // Save the file to storage and set path

        //    using (var stream = new FileStream(filePath, FileMode.Create))

        //    {

        //        imageFile.CopyTo(stream);

        //        return imageFile.FileName;
        //    }

        //}

        private List<CountryViewModel> GetCountries()
        {
            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                ($"{endPoint}Country/GetAllCountry", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<CountryViewModel>();
        }

        private List<StateViewModel> GetStates()
        {
            ServiceResponse<IEnumerable<StateViewModel>> response = new ServiceResponse<IEnumerable<StateViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>
                ($"{endPoint}State/GetAllState", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<StateViewModel>();
        }
    }
}
