using System.CodeDom;
using System.Drawing.Text;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
        // I need To retrive all data in Db First 
        // by Fun Get All in DepartmentReposatory 
        // so i need to creat an object frim it 
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // now clr will create an object from DepartmentController so the _departmentRepository will be null-Exception
        // so that we need to creat object from DepartmentRepository first ( need to inject it )
        public DepartmentController(IDepartmentRepository departmentRepository ,IMapper mapper) //ASK clr to creat obj from DepartmentRepository
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        //then go to add Service in program 

        public async Task<IActionResult> Index(string? CodeSearch )
        {
            var departments = Enumerable.Empty<Department>();

            if (string.IsNullOrWhiteSpace(CodeSearch))
            {
                 departments = await _departmentRepository.GetAllAsync();
                 // kda an b3t Elly rag3 mn GetAll ll View but as object -- but i need
                                           // to tread as Ienumrable of Departments  

            }
            else
            {
                departments =  _departmentRepository.GetByCode(CodeSearch);
            }

            var Result = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);


            return View(Result);

        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid) // make sure that the model come with the validation i put as client side in Department Model 
            {
                var department = _mapper.Map<Department>(model);

                var Count =await  _departmentRepository.AddAsync(department);
                if (Count > 0)                                  // da bnsba kbera hytm lw isValid = true 
                {                                               // da bnsba kbera hytm lw isValid = true 
                    return RedirectToAction("Index");           // da bnsba kbera hytm lw isValid = true 
                }                                               // da bnsba kbera hytm lw isValid = true 
            }         
           
               return View(model); // lw 8lt reg3l el model elly hwa 3amlo in same view to change on it 
            

            // ALSO YOU NEED A space inside view because if error come should return error messages in Department Model 
            // RETURN them with model /he already do this but you should make a space in view only  
        }


        public async Task<IActionResult> Details(int? id ) // da el id elly hgeeb details 
        {
            if (id == null)
            {
                return BadRequest(); // error 400
            }

            var department = await _departmentRepository.GetAsync(id.Value); // error because ana b3tt nullable int and GET take an int
            if (department == null) { return NotFound();  } //404  --> lw id department not exist 
            
            var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);

            return View(departmentViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest(); // error 400
            }


            var department = await _departmentRepository.GetAsync(id.Value); // error because ana b3tt nullable int and GET take an int
            if (department == null) { return NotFound(); } //404  --> lw id department not exist 

            var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);

            return View(departmentViewModel);

            //return Details( id , "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute]int? id ,DepartmentViewModel model)
        {
          try
            {


                if (id != model.Id) { return BadRequest(); }


                if (ModelState.IsValid)
                {
                    var department = _mapper.Map<Department>(model);


                    var count = await _departmentRepository.UpdateAsync(department);
                    if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty , ex.Message);
            }

            return View(model);
        }



        public async Task<IActionResult> Delete(int? id) // da el id elly hms7o 
        {
            if (id == null)
            {
                return BadRequest(); // error 400
            }

            var department = await _departmentRepository.GetAsync(id.Value); // error because ana b3tt nullable int and GET take an int
            if (department == null) { return NotFound(); } //404  --> lw id department not exist 


            var departmentViewModel = _mapper.Map<DepartmentViewModel>(department);

            return View(departmentViewModel);


            //return Details(id, "Delete");


        }


        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete([FromRoute]int id , DepartmentViewModel model)
        {
           
                if (id != model.Id) { return BadRequest(); }

                var department = _mapper.Map<Department>(model);

                await  _departmentRepository.DeleteAsync (department);
                return RedirectToAction("Index");
           

        }
    }
}
