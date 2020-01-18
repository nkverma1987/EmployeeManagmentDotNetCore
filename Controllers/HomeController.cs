using CoreDemo1.Models;
using CoreDemo1.ViewModel;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CoreDemo1.Controllers
{
    //[Route("Home")]
    //we can also write like below for attribute routing
    //[Route("[controller]")]
    //we can also write like below for attribute routing
    //[Route("[controller]/[action]")]

    public class HomeController : Controller
    {
        private readonly IEmployeeRpository _employeeRpository;
        // IHostingEnvironment is being used to get the wwwroot folder path in the solution
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger logger;
        public HomeController(IEmployeeRpository employeeRepository, 
                              IHostingEnvironment hostingEnvironment,
                              ILogger<HomeController> logger)
        {
            this._employeeRpository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
        }
        //[Route("")]
        //[Route("Index")]
        //also can write like below
        //[Route("[action]")]
        //[Route("~/Home")]
        // [Route("~/")] // for http://localhost:49827/
        [AllowAnonymous]
        public ViewResult Index()
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            var model = _employeeRpository.GetEmployees();
            return View(model);
        }
        //[Route("Details/{id?}")]
        //also can write like below
        //[Route("[action]/{id?}")]
        //also can write like below
        //[Route("{id?}")]
        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            
            Employee employee = _employeeRpository.GetEmployee(id.Value);
            if(employee==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = employee,// _employeeRpository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };
            //return new ObjectResult(employee);
            //return Json(employee);
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFIleName = ProcessUploadedFile(model);

                #region code for multiple files upload

                //if (model.Photo != null && model.Photo.Count > 0)
                //{

                //    foreach (IFormFile photo in model.Photos)
                //    {
                //        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //        uniqueFIleName = Guid.NewGuid() + "_" + photo.FileName;
                //        string filePath = Path.Combine(uploadsFolder, uniqueFIleName);
                //        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //    }
                //}

                #endregion

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFIleName
                };
                _employeeRpository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        #region Edit functionality

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRpository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRpository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {
                    if(model.ExistingPhotoPath!=null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images",model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                _employeeRpository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFIleName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFIleName = Guid.NewGuid() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFIleName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFIleName;
        }

        #endregion
    }
}