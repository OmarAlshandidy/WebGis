using Gis.DAL.Models;
using Gis.PL.Dtos;
using Gis.PL.Dtos.Auth;
using Gis.PL.Healper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gis.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
          _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {
            
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
              users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
             users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                 Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            } 
            return View(users);
        }
        public async Task<ActionResult> Search(string SearchInput)
        {

            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
            return PartialView("UserPartailView/UserTablePartialView", users);
        }
        [HttpGet]
        public async Task<ActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var user  = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            var userDto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(ViewName, userDto);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string? id)
        {
           
            return await Details(id ,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invailed Operation !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invailed Operation !");
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

                return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string ? id)
        {

            return await Details(id, "Delete");


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {

            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invailed Operation !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invailed Operation !");
               

                var result = await _userManager.DeleteAsync(user);
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(model);
        }
    }
    }


