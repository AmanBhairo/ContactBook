using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace ContactBookClientApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;

        public ReportController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }
        public IActionResult ContactRecordBasedOnBirthdayMonthReport()
        {
            var model = new List<ContactRecordReportViewModel>();
            List<string> months = new List<string>
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
                };

            ViewBag.Months = months;
            return View(model);
        }

        [HttpPost]
        public IActionResult ContactRecordBasedOnBirthdayMonthReport(int month)
        {
            List<string> months = new List<string>
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
                };
            ViewBag.Months = months;
            @ViewBag.Month = month;
            var apiUrl = $"{endPoint}Contact/GetContactRecordBasedOnBirthdayMonthReport/" + month;
            var response = _httpClientService.GetHttpResponseMessage<ServiceResponse<IEnumerable<ContactRecordReportViewModel>>>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ContactRecordReportViewModel>>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("ContactRecordBasedOnBirthdayMonthReport");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactRecordReportViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("ContactRecordBasedOnBirthdayMonthReport");
            }
        }

        public IActionResult GetContactsByStateReport()
        {
            //var model = new List<ContactRecordReportViewModel>();
            ContactRecordReportViewModel model = new ContactRecordReportViewModel();
            ViewBag.Country = GetCountries();
            ViewBag.States = GetStates();
            
            return View();
        }

        [HttpPost]
        public IActionResult GetContactsByStateReport(int CountryId,int StateId)
        {
            
            var apiUrl = $"{endPoint}Contact/GetContactsByStateReport/" + StateId;
            var response = _httpClientService.GetHttpResponseMessage<ServiceResponse<IEnumerable<ContactRecordReportViewModel>>>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<ContactRecordReportViewModel>>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    ViewBag.Country = GetCountries();
                    ViewBag.States = GetStates();
                    ViewBag.SelectedCountryId = CountryId; // Assuming you want to show selected StateId in Country dropdown
                    ViewBag.SelectedStateId = StateId;
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("GetContactsByStateReport");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactRecordReportViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("GetContactsByStateReport");
            }
        }

        public IActionResult GetContactsCountByCountryReport()
        {
            ViewBag.Country = GetCountries();
            return View();
        }

        [HttpPost]
        public IActionResult GetContactsCountByCountryReport(int CountryId)
        {
            ViewBag.Country = GetCountries();
            var apiUrl = $"{endPoint}Contact/GetContactsCountByCountryReport/" + CountryId;
            var response = _httpClientService.GetHttpResponseMessage<ServiceResponse<int>>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    ViewBag.CountryId = CountryId;
                    ViewBag.Response = serviceResponse.Data;
                    return View();
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("GetContactsCountByCountryReport");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("GetContactsCountByCountryReport");
            }
        }

        public IActionResult GetContactCountByGenderReport()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult GetContactCountByGenderReport(string gender)
        //{
        //    var apiUrl = $"{endPoint}Contact/GetContactCountByGenderReport/" + gender;
        //    var response = _httpClientService.GetHttpResponseMessage<ServiceResponse<int>>(apiUrl, HttpContext.Request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(data);
        //        if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
        //        {
        //            ViewBag.Response = serviceResponse.Data;
        //            ViewBag.SelectedGender = gender;
        //            return View();
        //        }
        //        else
        //        {
        //            TempData["errorMesssage"] = serviceResponse.Message;
        //            return RedirectToAction("GetContactCountByGenderReport");
        //        }
        //    }
        //    else
        //    {
        //        string errorData = response.Content.ReadAsStringAsync().Result;
        //        var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(errorData);
        //        if (errorResponse != null)
        //        {
        //            TempData["errorMesssage"] = errorResponse.Message;
        //        }
        //        else
        //        {
        //            TempData["errorMesssage"] = "Something went wrong try after some time";
        //        }
        //        return RedirectToAction("GetContactCountByGenderReport");
        //    }
        //}

        [HttpPost]
        public IActionResult GetContactCountByGenderReport(string gender)
        {
            var apiUrl = $"{endPoint}Contact/GetContactCountByGenderReport/" + gender;
            var response = _httpClientService.GetHttpResponseMessage<ServiceResponse<int>>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    ViewBag.Response = serviceResponse.Data;
                    ViewBag.SelectedGender = gender; // Set selected gender
                    return View();
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse.Message;
                    return RedirectToAction("GetContactCountByGenderReport");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<int>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong, try again later";
                }
                return RedirectToAction("GetContactCountByGenderReport");
            }
        }


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
