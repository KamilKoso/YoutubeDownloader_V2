using System;
using System.Linq;
using System.Threading.Tasks;
using YoutubeDownloader.IdentityServer.Infrastructure.Services;
using YoutubeDownloader.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoutubeDownloader.IdentityServer.Domain;

namespace YoutubeDownloader.IdentityServer.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public ManageController(
          UserManager<AppUser> userManager,
          SignInManager<AppUser> signInManager,
          ILogger<ManageController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var user = await GetUser();

            var model = new IndexViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                ReturnUrl = returnUrl
            };

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword(string returnUrl)
        {
            var model = new ChangePasswordViewModel { ReturnUrl = returnUrl };
            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string button)
        {
            if (button == "cancel")
            {
                return Redirect(!string.IsNullOrEmpty(model.ReturnUrl)
                    ? model.ReturnUrl + $"?resultCode={ResultCode.PasswordCanceled}"
                    : "~/");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await GetUser();

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    _logger.Log(LogLevel.Error, $"An error occurs when User with ID '{user.Id}' was trying to change password. (Error: '${string.Join(",", changePasswordResult.Errors.Select(e => e.Description))}')");
                    model.HasError = true;
                    return View(model);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                return Redirect(!string.IsNullOrEmpty(model.ReturnUrl)
                    ? model.ReturnUrl + $"?resultCode={ResultCode.PasswordChanged}"
                    : "~/");
            }
        }

        private async Task<AppUser> GetUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return user;
        }

        private async Task UpdateUserProfile(IndexViewModel model)
        {
            var user = await GetUser();
            user.Email = model.Email;

            if (user.PhoneNumber != model.PhoneNumber)
            {
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = false;
                model.PhoneNumberConfirmed = false;
            }

            try
            {
                await _userManager.UpdateAsync(user);
                ViewBag.message = "User data has been updated.";
            }
            catch (Exception)
            {
                ViewBag.message = null;
                ModelState.AddModelError("", "There was an error with saving data, please try again");
            }
        }
    }
}
