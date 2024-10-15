using System.Data;
using Company.DAL.Models;
using Company.PL.ViewModels.RoleManager;
using Company.PL.ViewModels.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //Create -Index - Details -Edit - Delete 
        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? Searchinput) 
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(Searchinput))
            {
                //_userManager.Users.ToList();--> get all users but as ApplicationUser Type --> I Need it As UserViewModel
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name,
                    
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name.ToLower().Contains(Searchinput.ToLower()))
                    .Select(R => new RoleViewModel()
                    {
                        
                        Id = R.Id,
                        RoleName = R.Name,
                       

                    }).ToListAsync();
            }

            return View(roles);
            ;
        }



        public IActionResult Create()
        { 
             return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {

            if(ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName,

                };

                var result = await _roleManager.CreateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }


            return View(model); 
        }


        public async Task<IActionResult> Details(string? id, string ViewName = "Details") 
        {

            if (id == null) { return BadRequest(); }


            var rolefromeDb = await _roleManager.FindByIdAsync(id);


            if (rolefromeDb == null) { return NotFound(); }

            var role = new RoleViewModel()
            {
                Id = rolefromeDb.Id,
                RoleName  = rolefromeDb.Name,
                
            };



            return View(ViewName, role);
        }

        public async Task<IActionResult> Update(string? id)
        {
            //if (id == null) { return BadRequest(); }
            return await Details(id, "Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id) { return BadRequest(); }

                if (ModelState.IsValid)
                {
                    //(POST)   Mapp -- EmployeeViewModel ---> Employee

                    var rolefromDb = await _roleManager.FindByIdAsync(id);

                    if (rolefromDb == null) { return NotFound(); }

                    // m3mlth Auto Map because i dont need to allow to change id and Role 
                    rolefromDb.Name = model.RoleName;

                    var result = await _roleManager.UpdateAsync(rolefromDb);

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


        public async Task<IActionResult> Delete(string? id)
        {

            //if (id == null) { return BadRequest(); }

            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id) { return BadRequest(); }


                if (ModelState.IsValid)
                {
                    var rolefromDb = await _roleManager.FindByIdAsync(id);
                    if (rolefromDb == null)
                    {
                        return NotFound();
                    }

                    var result = await _roleManager.DeleteAsync(rolefromDb);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }


                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }


            return View(model);


        }


        public async Task<IActionResult> AddOrRemoveUsers(string? roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            ////
            ViewData["RoleId"] = roleId;

            /////
            var usersInRole = new List<UserInRoleViewModel>(); // list h5zn feha kol userInRole el Checked and Not Checked 
                                                               // and this is the list i will prints in the action  AddOrRemoveUsers

            var Users = await _userManager.Users.ToListAsync();

            foreach (var user in Users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName // user.UserName -> FirstName 
                };

                if ( await _userManager.IsInRoleAsync(user , role.Name) )
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected= false;
                }

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);

        }

        [HttpPost]
		public async Task<IActionResult> AddOrRemoveUsers(string? roleId , List<UserInRoleViewModel> users )
        {
            //Mechanisim
			// add check --> check if role dont exist to specific user if yes --> Add
			// Remove Check --> Check if role  exist to specific user if yes -->Remove 


			var role = await _roleManager.FindByIdAsync(roleId);
			if (role == null)
			{
				return NotFound();
			}

            if(ModelState.IsValid)
            {

				// loop in each user is user has check in ---> add Role BUT*** check first if the role addel or not Okayyyy

				foreach (var user in users)
                {
					var appUser = await _userManager.FindByIdAsync(user.UserId);   //kda bgeeb mn Db (convert l (ApplicationUser)
                    if (appUser is not  null)
                    {
						if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser ,role.Name))
						{
							await _userManager.AddToRoleAsync(appUser, role.Name);
						}
						else if ( !user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
						{
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);	

					    }
					}					
				}  
                
                return RedirectToAction(nameof(Update) , new {id = roleId});
            }

            return View(users);

		}

	}


}
