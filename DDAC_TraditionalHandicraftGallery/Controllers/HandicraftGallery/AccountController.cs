using DDAC_TraditionalHandicraftGallery.Models;
using DDAC_TraditionalHandicraftGallery.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DDAC_TraditionalHandicraftGallery.Controllers.HandicraftGallery
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditPassword()
        {
            return View("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // User not found, return an appropriate error view
                return View("Index", "Home"); // TODO: change to proper error handling
            }

            // Check if the old password is correct
            var isOldPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!isOldPasswordCorrect)
            {
                ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                return View("Index", model);
            }

            // Check if the new password and confirmation match
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "New password and confirmation password do not match.");
                return View("Index", model);
            }

            // Change the password
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                ViewBag.PasswordChangedSuccessfully = true;
                return View("Index", model);
            }
            else
            {
                // Add the errors from the result to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Index", model);
            }
        }
   

        public IActionResult ViewQuoteRequest()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
