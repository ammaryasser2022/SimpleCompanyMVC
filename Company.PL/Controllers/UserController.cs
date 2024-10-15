using Company.DAL.Models;
using Company.PL.ViewModels.Employees;
using Company.PL.ViewModels.UserManager;
using Company.PL.wwwroot.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    [Authorize(Roles ="Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

        //Index - Details -Edit - Delete 
        public UserController(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
        }


        public async Task<IActionResult> Index(string? Searchinput)
        {
          var users = await _userManager.Users
        .Where(U => string.IsNullOrEmpty(Searchinput) || U.Email.ToLower().Contains(Searchinput.ToLower()))
        .Select(U => new UserViewModel
        {
            Id = U.Id,
            FirstName = U.FirstName,
            LastName = U.LastName,
            Email = U.Email

        }).ToListAsync();

            // After retrieving the users, fetch their roles in a separate loop
            foreach (var user in users)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }

            return View(users);
        }

        public async Task<IActionResult> Details(string? id , string ViewName = "Details")
        {

            if (id == null) { return BadRequest(); }


            var userfromDb = await _userManager.FindByIdAsync(id);


            if (userfromDb == null) { return NotFound(); }

            var user = new UserViewModel()
            {
                Id = userfromDb.Id,
                FirstName = userfromDb.FirstName,
                LastName = userfromDb.LastName,
                Email = userfromDb.Email,
                //Roles = _userManager.GetRolesAsync(userfromDb).Result
            };

             user.Roles = await _userManager.GetRolesAsync(userfromDb);
            

            return View(ViewName, user);
        }

        public async Task<IActionResult> Update(string? id)
        {
            //if (id == null) { return BadRequest(); }
            return await Details(id , "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id) { return BadRequest(); }

                if (ModelState.IsValid)
                {
                    //(POST)   Mapp -- EmployeeViewModel ---> Employee

                    var userfromDb = await _userManager.FindByIdAsync(id);

                    if (userfromDb == null) { return NotFound(); }

					// m3mlth Auto Map because i dont need to allow to change id and Role 
					userfromDb.FirstName = model.FirstName; 
					userfromDb.LastName = model.LastName;
					userfromDb.Email = model.Email;
					
					var result = await _userManager.UpdateAsync(userfromDb);

					if (result.Succeeded)
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


        public async Task<IActionResult> Delete(string? id )
        {

            //if (id == null) { return BadRequest(); }

            return await Details(id , "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id) { return BadRequest(); }


                if (ModelState.IsValid)
                {
                    var userfromDb = await _userManager.FindByIdAsync(id);
                    if (userfromDb == null)
                    {
                        return NotFound();
                    }

                    var result = await _userManager.DeleteAsync(userfromDb);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }


                }

            } catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty , ex.Message);    
            }
           

            return View(model);


        }
    }
}
