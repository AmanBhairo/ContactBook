using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCContactRecords.Data;
using MVCContactRecords.Models;
using MVCContactRecords.Services.Contract;

namespace MVCContactRecords.Controllers
{
    public class ContactController : Controller
    {
        private AppDbContext _context;
        private readonly IContactService _contactService;
        public ContactController(AppDbContext appDbContext, IContactService contactService)
        {
            _context = appDbContext;
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            var suppliers = _contactService.GetAllCategory();
            if (suppliers != null && suppliers.Any())
            {
                return View(suppliers);
            }
            return View(new List<Contact>());
        }

        public IActionResult Index1(char? letter, int page = 1, int pageSize = 2)
        {
            ViewBag.CurrentPage = page; // Pass the current page number to the ViewBag
                                        // Get total count of categories
            var totalCount = _contactService.TotalContacts(letter);
            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var contacts = letter == null
       ? _contactService.GetPaginatedContacts(page, pageSize)
       : _contactService.GetPaginatedContacts(page, pageSize, letter);

            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Letter = letter;
            return View(contacts);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var supplier = _contactService.GetCategory(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]

        public IActionResult Edit(Contact supplier)
        {
            if (ModelState.IsValid)
            {
                var message = _contactService.ModifyCategory(supplier);
                if (message == "Category Already Exist" || message == "Something went wrong please try after sometime")
                {

                    TempData["ErrorMessage"] = message;

                }
                else
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }

            }
            return View(supplier);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            Contact supplier = new Contact();
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Create(Contact supplier)
        {
            if (ModelState.IsValid)
            {
                var result = _contactService.AddCategory(supplier);
                if (result == "Category Already Exist" || result == "Something went wrong try after some time")
                {
                    TempData["ErrorMessage"] = result;
                }
                else
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Index");
                }
            }
            return View(supplier);
        }

        public IActionResult Details(int id)
        {
            var supplier = _contactService.GetCategory(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }


        //[HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var supplier = _contactService.GetCategory(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int ContactId)
        {
            var result = _contactService.RemoveCategory(ContactId);
            if (result == "Category Deleted Successfully")
            {
                TempData["SuccessMessage"] = result;
            }
            else if (result == "Something went wrong please try after sometime")
            {
                TempData["SuccessMessage"] = result;
            }
            return RedirectToAction("Index1");
        }
    }
}
