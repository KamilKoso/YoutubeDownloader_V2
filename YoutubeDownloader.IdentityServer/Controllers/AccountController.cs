using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using YoutubeDownloader.Common.Auth;
using YoutubeDownloader.IdentityServer.Extensions;
using YoutubeDownloader.IdentityServer.Infrastructure;
using YoutubeDownloader.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using YoutubeDownloader.IdentityServer.Domain;

namespace YoutubeDownloader.IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly IEmailSender _emailSender;
        private readonly AppConfig _appConfig;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interaction,
            IPersistedGrantService persistedGrantService,
            IEmailSender emailSender,
            IOptions<AppConfig> appConfig,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _persistedGrantService = persistedGrantService;
            _emailSender = emailSender;
            _logger = logger;
            _appConfig = appConfig.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterLink()
        {
            return Redirect(_appConfig.ClientUrl + "/register");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new AppUser { UserName = model.Username, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailVerificationLink(user.Id, code, Request.Scheme);

            await SendAccountConfirmationEmail(user.Email, callbackUrl);

            _logger.LogInformation($"New user {model.Email} just registered.");
            return Ok(new { userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            await _userManager.ConfirmEmailAsync(user, code);

            return View();
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    if (!user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        return RedirectToAction(nameof(ResendVerificationEmail), "Account", new { email = user.Email });
                    }

                    if (user.EmailConfirmed)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberLogin, lockoutOnFailure: true);

                        if (result.Succeeded)
                        {
                            if (Url.IsLocalUrl(model.ReturnUrl))
                            {
                                return Redirect(model.ReturnUrl);
                            }

                            if (string.IsNullOrEmpty(model.ReturnUrl))
                            {
                                return Redirect(_appConfig.ClientUrl);
                            }

                            // user might have clicked on a malicious link
                            _logger.LogError($"User with ID {user.Id} trying to redirect to {model.ReturnUrl}.");
                            throw new Exception("invalid return URL");
                        }

                        if (result == SignInResult.LockedOut)
                        {
                            _logger.LogError($"User with ID {user.Id} account locked out.");
                            ModelState.AddModelError(string.Empty, "This account has been locked out, please try again later.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
                }
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            return await Logout(new LogoutInputModel { LogoutId = logoutId });
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var subjectId = HttpContext.User.Identity.GetSubjectId();

                // delete local authentication cookie
                await _signInManager.SignOutAsync();

                var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

                var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

                await _persistedGrantService.RemoveAllGrantsAsync(subjectId, logout?.ClientId);

                if (!string.IsNullOrEmpty(vm.PostLogoutRedirectUri))
                {
                    return Redirect(vm.PostLogoutRedirectUri);
                }
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendVerificationEmail(string email)
        {
            return View(new ResendVerificationEmailModel { Email = email, ResendRequested = false }) ;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerificationEmail(ResendVerificationEmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailVerificationLink(user.Id, code, Request.Scheme);

            await SendAccountConfirmationEmail(model.Email, callbackUrl);

            return View(new ResendVerificationEmailModel { Email = model.Email, ResendRequested = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset password", $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                _logger.LogInformation($"Reset password token requested for user {user.Id}");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string userId = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            } 
            else if(userId == null)
            {
                throw new ApplicationException("UserId must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code, UserId = userId };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                _logger.Log(LogLevel.Information, $"User with ID {user.Id} reset password with success.");
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            _logger.Log(LogLevel.Warning, $"Unsuccessful attempt to reset password for user with ID {user.Id}.");
            ModelState.AddModelError(string.Empty, "Something went wrong. Please contact technical support.");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Email = model.Email;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            return vm;
        }


        private async Task SendAccountConfirmationEmail(string emailReceiver, string emailConfirmationURl)
        {
            await _emailSender.SendEmailAsync(emailReceiver, "Please Verify Your Account",
               $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(emailConfirmationURl)}'>clicking here</a>.");
        }
    }
}
