using DDAC_TraditionalHandicraftGallery.DataAccess;
using DDAC_TraditionalHandicraftGallery.Models;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Areas.Admin.Controllers.GalleryAdmin
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
            //_signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            var userList = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    userList.Add(user);
                }
            }
            return View(userList);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Check if user is in "User" role
            var isInRole = await _userManager.IsInRoleAsync(user, "User");
            if (!isInRole)
            {
                // User is not in role. Return a forbidden result, or handle in any other way.
                return new ForbidResult();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ApplicationUserViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {

                var hasher = new PasswordHasher<ApplicationUser>();
                var user = new ApplicationUser
                {
                    Id = userViewModel.Id,
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    EmailConfirmed = userViewModel.EmailConfirmed,
                    PasswordHash = hasher.HashPassword(null, userViewModel.Password),
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index");
                }
            }

            return View(userViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Check if user is in "User" role
            var isInRole = await _userManager.IsInRoleAsync(user, "User");
            if (!isInRole)
            {
                // User is not in role. Return a forbidden result, or handle in any other way.
                return new ForbidResult();
            }

            var userViewModel = new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            };


            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, ApplicationUserViewModel userViewModel)
        {
            if (id.Equals(userViewModel.Id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                // Check if user is in "User" role
                var isInRole = await _userManager.IsInRoleAsync(user, "User");
                if (!isInRole)
                {
                    // User is not in role. Return a forbidden result, or handle in any other way.
                    return new ForbidResult();
                }

                var hasher = new PasswordHasher<ApplicationUser>();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.EmailConfirmed = userViewModel.EmailConfirmed;
                user.PasswordHash = hasher.HashPassword(null, userViewModel.Password);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(userViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user is in "User" role
            var isInRole = await _userManager.IsInRoleAsync(user, "User");
            if (!isInRole)
            {
                // User is not in role. Return a forbidden result, or handle in any other way.
                return new ForbidResult();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Check if user is in "User" role
            var isInRole = await _userManager.IsInRoleAsync(user, "User");
            if (!isInRole)
            {
                // User is not in role. Return a forbidden result, or handle in any other way.
                return new ForbidResult();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }

}
