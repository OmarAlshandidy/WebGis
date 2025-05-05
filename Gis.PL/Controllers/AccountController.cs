using Gis.DAL.Models;
using Gis.PL.Dtos.Auth;
using Gis.PL.Healper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Email = Gis.PL.Healper.Email;

namespace Gis.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid) // Server Side Valdition 
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                }
                if (user is null)
                {

                    // Register 
                    user = new AppUser()
                    {
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        IsAgreed = model.IsAgreed

                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                ModelState.AddModelError("", "Invailed Sign Up !! ");

            }
            return View(model);
        }
        #endregion

        #region Sign In 
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemmeberMe, false);


                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }

                    }
                }
                ModelState.AddModelError("", "Invaile SignIn !");
            }
            return View();
        }

        //public IActionResult GoogleLogin()
        //{
        //    var prop = new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action("GoogleResponse")
        //    };
        //    return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        //}

        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    if (result?.Principal is not null)
        //    {
        //        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        //        var emailClaim = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
        //        var email = emailClaim?.Value;

        //        if (email is not null)
        //        {
        //            var user = await _userManager.FindByEmailAsync(email);
        //            if (user is null)
        //            {
        //                user = new AppUser
        //                {
        //                    UserName = email,
        //                    Email = email,
        //                    FirstName = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value,
        //                    LastName = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value
        //                };
        //                var resultCreate = await _userManager.CreateAsync(user);
        //                if (!resultCreate.Succeeded)
        //                    return RedirectToAction("SignIn");
        //            }

        //            // Sign in the user
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    return RedirectToAction("SignIn");
        //}
        //public IActionResult FacebookLogin()
        //{
        //    var prop = new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action("FacebookResponse")
        //    };
        //    return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        //}

        //public async Task<IActionResult> FacebookResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        //    if (result?.Principal is not null)
        //    {
        //        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        //        var emailClaim = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
        //        var email = emailClaim?.Value;

        //        if (email is not null)
        //        {
        //            var user = await _userManager.FindByEmailAsync(email);
        //            if (user is null)
        //            {
        //                user = new AppUser
        //                {
        //                    UserName = email,
        //                    Email = email,
        //                    FirstName = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value,
        //                    LastName = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value
        //                };
        //                var resultCreate = await _userManager.CreateAsync(user);
        //                if (!resultCreate.Succeeded)
        //                    return RedirectToAction("SignIn");
        //            }

        //            // Sign in the user
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    return RedirectToAction("SignIn");
        //}



        #endregion
        #region Sign Out
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Genrate Token 
                    var token =
                        await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create Url 

                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    if (url is not null)
                    {
                        //Create Email
                        var email = new Email()
                        {
                            To = model.Email,
                            Subject = "Reset Password",
                            Body = url
                        };
                        // Send Email 

                        var flag = EmailSettings.SendEmail(email);
                        if (flag)
                        {

                            // Check Your Inbox
                            return RedirectToAction("CheckYourInbox");
                        }
                    }
                }

            }
            ModelState.AddModelError("", "Invalid  Reset Password Operation !!");
            return View("ForgetPassword", model);
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }


        #endregion

        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null) return BadRequest("Invailed Opreation");

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Paswsord Operation");

            }
            return View();

        }
        #endregion

        #region Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }
}
