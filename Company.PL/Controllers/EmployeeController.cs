using System.Net;
using System.Reflection.Metadata;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.ViewModels.Employees;
using Company.PL.wwwroot.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Company.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;  // h3ml be Mapp

        public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository ,IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }





        public async Task<IActionResult> Index(string? Searchinput)
        {
            var employee = Enumerable.Empty<Employee>();
            if(string.IsNullOrEmpty(Searchinput))
            {
                 employee =  await _employeeRepository.GetAllAsync();
            }
            else
            {
                employee = await _employeeRepository.GetByNameAsync(Searchinput);
            }

            //new sesiion 
            // if i need to send anthor thing else the entity of employeees how?
            // views Dictionary :Transfer data From Action To View [One Way Only *]

            //string Message = "Hello World ";  // 3ayz ab3tha in Index with el IEnumurable of Employees

            // 1 ViewData : Property i Was inherted from class Controller -- Dictionary---> Key , Value 
            // 2 ViewBage : Property i Was inherted from class Controller -- Dynamic
            // 3 TempData : Property i Was inherted from class Controller -- Dictionary---> Key , Value  --like ViewData  
            //Use When WE need to transfer data between two requests 

            //// 1 ViewData:
            //ViewData["Message"] = Message + "From View Data"; //**// Value is returned as Object //when you need to locate it in any variable in View you need to Cast
            //
            //// 2 ViewBage :
            //ViewBag.Message2 = Message + "From View Bag";     //**// No required Casting because its dynamic the data type determined in RunTime 
            //
            //// 3 TempData : 
            //TempData["Message2"] = Message + "From Temp Data"; //**//   Use When WE need to transfer data between two requests 
            //// will make example in 2 requests in create action 

            //*******************************************************************************************
            // the name of department dont show in index view because EF core dont load Navigational property by default
            // should load it by Explicite loading -Eager loading - Lazy loading  
            //noteeeeeeeeeeeeee   use it in main query treted with Db
            ////////////////////////////////////////////////////////////////////////////////
            /////////////////// MAPP///////////////////////////

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employee);


            return View(Result);
        }

        public async Task<IActionResult> Create()
        {
            // need to get all department when open create view --> need to use _departmentRepository 
            //so need to inject here object from DepartmentRepository 
            var departments = await _departmentRepository.GetAllAsync(); //Extra Information

            ViewData["Departments"] = departments;

            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");



                // now the data bindend in model as type EmployeeViewModel 
                // and i need to add model --> but add take Employee Parameter
                // Need Castingg     (POST)   EmployeeViewModel ---> Employee  -->This casting == Mapping 
                // 1 .Manual 
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Salary = model.Salary,
                //    Age = model.Age,
                //    HiringDate = model.HiringDate,
                //    IsActive = model.IsActive,
                //    WorkFor = model.WorkFor,
                //    WorkForId = model.WorkForId,
                //    Email = model.Email,
                //    PhoneNumber = model.PhoneNumber,
                //    //DateOfCreation = model.Dat   ****  its the  point now model dont have DateOfCreation because i dont need to view :)s

                //};


                // 2 .Automatic 
                // AutoMapper Package 

                var employee = _mapper.Map<Employee>(model);

                var count = await _employeeRepository.AddAsync(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is created successfully";
                }
                else
                {
                    TempData["Message"] = "Employee Is Not Created Successfully";
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(model);
        }





        public async Task<IActionResult> Details(int? id )
        {

            if(id == null) { return BadRequest(); }
             
            var employee = await _employeeRepository.GetAsync(id.Value);
            if (employee == null) { return NotFound(); }

            //Mapping (GET)- Employee --> EmployeeViewModel

            //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    WorkFor= employee.WorkFor,
            //    WorkForId= employee.WorkForId,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,

            //};

                        
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);



            return View(employeeViewModel);
        }


        public async Task<IActionResult> Update(int? id )
        {
            var departments = await _departmentRepository.GetAllAsync(); //Extra Information

            ViewData["Departments"] = departments;

            if (id == null) { return BadRequest(); }

            var employee = await _employeeRepository.GetAsync(id.Value);
            if (employee == null) { return NotFound(); }

            //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    WorkFor = employee.WorkFor,
            //    WorkForId = employee.WorkForId,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,

            //};
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update([FromRoute] int? id , EmployeeViewModel model)
        {
           try
            {
                if (id != model.Id) { return BadRequest(); }

                if (ModelState.IsValid)
                {
                    //(POST)   Mapp -- EmployeeViewModel ---> Employee

                    //Employee employee = new Employee()
                    //{
                    //    Id = model.Id,
                    //    Name = model.Name,
                    //    Address = model.Address,
                    //    Salary = model.Salary,
                    //    Age = model.Age,
                    //    HiringDate = model.HiringDate,
                    //    IsActive = model.IsActive,
                    //    WorkFor = model.WorkFor,
                    //    WorkForId = model.WorkForId,
                    //    Email = model.Email,
                    //    PhoneNumber = model.PhoneNumber,
                    //    //DateOfCreation = model.Dat   ****  its the  point now model dont have DateOfCreation because i dont need to view :)s

                    //};


                    if(model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");




                    //Auto maP
                    var employee = _mapper.Map<Employee>(model);


                    var count = await _employeeRepository.UpdateAsync(employee);
                    if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);   //mkan hnak lazem
            }

             return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return BadRequest(); }

            var employee = await _employeeRepository.GetAsync(id.Value);
            if (employee == null) { return NotFound(); }

            //EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            //{
            //    Id = employee.Id,
            //    Name = employee.Name,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    PhoneNumber = employee.PhoneNumber,
            //    WorkFor = employee.WorkFor,
            //    WorkForId = employee.WorkForId,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,

            //};
            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);    

            return View(employeeViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel model)
        {

            if (id != model.Id) { return BadRequest(); }

            //Employee employee = new Employee()
            //{
            //    Id = model.Id,
            //    Name = model.Name,
            //    Address = model.Address,
            //    Salary = model.Salary,
            //    Age = model.Age,
            //    HiringDate = model.HiringDate,
            //    IsActive = model.IsActive,
            //    WorkFor = model.WorkFor,
            //    WorkForId = model.WorkForId,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //    //DateOfCreation = model.Dat   ****  its the  point now model dont have DateOfCreation because i dont need to view :)s

            //};

            var employee = _mapper.Map<Employee>(model);

            await _employeeRepository.AddAsync(employee);
            DocumentSettings.DeleteFile(model.ImageName, "images");

            return RedirectToAction("Index");


        }
    }
}

