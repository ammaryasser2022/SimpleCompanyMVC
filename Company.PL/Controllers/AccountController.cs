using System.Linq.Expressions;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager; // for create token 

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            //cODE To registration after insert Data in SignUp Form
            //  by UserManager Package 
            if (ModelState.IsValid)//Server Side Validation // lw tmam hysh8l BUT**** momken Ykon Email Mawgod 2bl kda So..use FindByNameAsync
            {
                var User = await _userManager.FindByNameAsync(model.UserName);

                if (User == null)
                {
					User = await _userManager.FindByEmailAsync(model.Email);

                    if (User == null)
                    {
						User = new ApplicationUser()
						{
							UserName = model.UserName,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							IsAgree = model.IsAgree,
						};

						var Result = await _userManager.CreateAsync(User, model.Password);
						if (Result.Succeeded)
						{
							return RedirectToAction("SignIn");
						}

						foreach (var error in Result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}

					}

					ModelState.AddModelError(string.Empty, "Email is Already Exist :(");
					return View(model);

				}

				ModelState.AddModelError(string.Empty, "User Name is Already Exist :(");


				return View(model);

			}

			return View(model);
        }


        public IActionResult SignIn()
		{
			return View(); 
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var user = await _userManager.FindByEmailAsync(model.Email);
					if (user is not null)
					{
						//Check PassWord
						var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
						if (Flag)
						{
							//Sign In and Give A Token
							// Token Generate BY SignInManager ---> second Category so need Object mnha like _userManager 

							var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
							if (result.Succeeded)
							{
								return RedirectToAction("Index", "Home");
							}
							
						}

						
					}

					ModelState.AddModelError(string.Empty, "Invalid Login  !!");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty , ex.Message);
			}

			return View(model);
		}

		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}


		public IActionResult ForgetPassword()
		{
			return View();

		}
		//ResetPasswordUrl
		public async Task<IActionResult> ResetPasswordUrl(ForgetPassswordViewModel? model )
		{
			try
			{
				if (ModelState.IsValid)
				{
					var user = await _userManager.FindByEmailAsync(model.Email);
					if (user != null)
					{
						// 3 - Generate Token         // in SignIn  we use PasswordSignInAsync To create Token 
						var token = await _userManager.GeneratePasswordResetTokenAsync(user);
						// but generation of the token need to add service (AddDefaultTokenProviders) 


						//2 - Create Link (URL)
						// B3ml Action --> inside contain new pass and confirm new pass and send with this action the email to login
						var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme/*http/....*/ );
						//https://localhost:44342/Account/ResetPassword?email=Ammar@gmail.com&Token
						//but you should send a token with url to make sure that no one can edit in url and change pass to anothe email


						// 1 - create Email --  - Create class contain shape of Email --> in Models in DAL 
						// now creat object from Email  -- 
						//before it should create Url 
						var email = new DAL.Models.Email()
						{
							To = model.Email,
							Subject = "Reset Password",
							Body = url  // url da ana h3mlo w ab3to lw das 3leh yb2a hwa w h3ml feh token 3shan m7dsh y randon any link 

						};


						//Sending Email

						// I Will Create This Function in Helpers Folder 
						EmailSettings.SendEmail(email);

						return RedirectToAction("CheckYourInbox"); // h3ml action kda a2olo Check you Email Box 


					}
					ModelState.AddModelError(string.Empty, "Invalid Reset Password , Please Try Again !!");
				}
			}

			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			return View("ForgetPassword" , model);
		}

		public IActionResult CheckYourInbox()
		{
		   return View(); 
		}

		public IActionResult ResetPassword(string email , string token ) // recieve from query params email , token
		{
			TempData["email"] = email;
			TempData["token"] = token;


			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model) // recieve from url email , token
															    // but i dont make that 
																//because can any one change in url  ..and request will fail
																//hb3t data mn action l action 
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					// Put New pass to this email 
					var result = await _userManager.ResetPasswordAsync(user, token, model.Password );
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}

				}

			}
			ModelState.AddModelError(string.Empty, "Invalid Reset Password , Please Try Again !!");

			return View(model);
		}

		public IActionResult AccessDenied()
		{
			return  View();
		}
		
	}
}
 