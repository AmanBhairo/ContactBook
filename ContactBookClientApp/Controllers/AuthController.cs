using ContactBookClientApp.Implementation;
using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace ContactBookClientApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private readonly IAddImageFileToPathService _addImage;
        private readonly IJwtTokenHandler _tokenHandler;
        private string endPoint;

        public AuthController(IHttpClientService httpClientService, IConfiguration configuration, IAddImageFileToPathService addImage, IJwtTokenHandler tokenHandler)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _addImage = addImage;
            _tokenHandler = tokenHandler;
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);
                    string token = serviceResponse.Data;

                    //Below code won't be working for ajax call
                    //Response.Cookies.Append("jwtToken", token, new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.Strict,
                    //});

                    //Below code if we want to access cookie in a ajax call.
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    //TempData["SuccessMessage"] = serviceResponse.Message;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = _tokenHandler.ReadJwtToken(token);
                    var loginId = jwtToken.Claims.First(claim => claim.Type == "LoginId").Value;


                    Response.Cookies.Append("LoginId", loginId, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(1),
                    });
                    //Get user details
                    var userDetails = UserDetailById(loginId);

                    //// Store user image in cookie
                    if (userDetails != null && userDetails.ImageByte != null)
                    {
                        var image = Convert.ToBase64String(userDetails.ImageByte);

                        // Split image into smaller chunks if necessary to fit cookie size limit
                        int chunkSize = 3800; // safe size under 4KB considering other cookie data
                        int totalChunks = (image.Length + chunkSize - 1) / chunkSize;

                        for (int i = 0; i < totalChunks; i++)
                        {
                            string chunk = image.Substring(i * chunkSize, Math.Min(chunkSize, image.Length - i * chunkSize));
                            Response.Cookies.Append($"image_chunk_{i}", chunk, new CookieOptions
                            {
                                HttpOnly = false,
                                Secure = true,
                                SameSite = SameSiteMode.None,
                                Expires = DateTime.UtcNow.AddDays(1),
                            });
                        }
                    }
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex", "Contact");

                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);
                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong.Please try after sometime.";
                    }
                    return RedirectToAction("Login");
                }


            }
            return View(viewModel);
        }


        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
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

                var apiUrl = $"{endPoint}Auth/Register";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["errorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                    }
                }

                return RedirectToAction("Register");

            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateUser()
        {
            var userName = User.Identity.Name;
            var apiUrl = $"{endPoint}Auth/GetUserByUserName/" + userName;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateUserViewModel updateUserViewModel = serviceResponse.Data;
                    return View(updateUserViewModel);
                }
                else
                {
                    TempData["errorMesssage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex", "Contact");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMesssage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMesssage"] = "Something went wrong try after some time";
                }
                return RedirectToAction("PaginatedIndex", "Contact");
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(UpdateUserViewModel updateUser)
        {
            if (ModelState.IsValid)
            {
                if (updateUser.File != null && updateUser.File.Length > 0)
                {
                    var fileName = _addImage.AddImageFileToPath(updateUser.File);
                    updateUser.ProfilePic = fileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        updateUser.File.CopyTo(memoryStream);
                        updateUser.ImageByte = memoryStream.ToArray();
                    }
                }
                else if (updateUser.removeImageHidden == "true")
                {
                    updateUser.ProfilePic = null;
                    updateUser.ImageByte = null;
                }

                var apiUrl = $"{endPoint}Auth/EditUser";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateUser, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("PaginatedIndex", "Contact");
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
                        return RedirectToAction("PaginatedIndex", "Contact");
                    }
                }
            }
            return View(updateUser);
        }


        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ValidateForgotPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("UpdatePassword", new { userName = viewModel.UserName });
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["errorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                    }
                }

                return RedirectToAction("ForgotPassword");

            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdatePassword(string userName)
        {
            UpdatePasswordModel model = new UpdatePasswordModel();
            model.UserName = userName;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdatePassword(UpdatePasswordModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/AddNewPassword";
                var response = _httpClientService.PutHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    if (serviceResponse.Message == string.Empty)
                    {
                        TempData["successMessage"] = "Password updated successfully";
                    }
                    else
                    {
                        TempData["successMessage"] = serviceResponse.Message;
                    }

                    return RedirectToAction("PaginatedIndex", "Contact");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["errorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["errorMesssage"] = "Something went wrong try after some time";
                    }
                }

                return RedirectToAction("ForgotPassword");

            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ChnagePassword()
        {
            UpdatePasswordModel model = new UpdatePasswordModel();
            model.UserName = @User.Identity.Name;
            return View(model);
        }


        [HttpGet]
        [ExcludeFromCodeCoverage]
        public UpdateUserViewModel UserDetailById(string userName)
        {

            var apiUrl = $"{endPoint}Auth/GetUserByUserName/" + userName;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(data);
                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return serviceResponse.Data;
                }
                else
                {
                    throw new Exception(serviceResponse.Message);

                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    throw new Exception(errorResponse.Message);
                }
                else
                {
                    throw new Exception("Something went wrong. Please try after some time.");
                }
            }

        }
    }
}
