﻿using Gis.DAL.Models;
using Gis.PL.Dtos;
using Gis.PL.Dtos.Auth;
using Gis.PL.Healper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G04.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
           _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult> Index(string SearchInput)
        {

            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,

                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData[key: "Message"] = "Role Is Created !!";

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);

        }
        [HttpGet]
        public async Task<ActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is not Found " });
            var roleDto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,

            };

            return View(ViewName, roleDto);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invailed Operation !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invailed Operation !");
                var roleResult = await _roleManager.FindByNameAsync(model.Name);
                if (roleResult is null)
                {
                    role.Name = model.Name;
                    var result = await _roleManager.UpdateAsync(role);
                    {
                        if (result.Succeeded)
                        {

                            return RedirectToAction("Index");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Operation ");



            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            return await Details(id, "Delete");


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {

            if (ModelState.IsValid)
            {

                if (id != model.Id) return BadRequest("Invailed Operation !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invailed Operation !");

                var result = await _roleManager.DeleteAsync(role);
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError("", "Invalid Operation ");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
           var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UserInRoleDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UserInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);


            }
            return View(usersInRole);
            


        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser( string  roleId,List<UserInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser =await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new {id = roleId});
            }

            return View(users);
        }


    }
}



