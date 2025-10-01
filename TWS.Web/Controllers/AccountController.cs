using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Web.Services.HttpServices;
using TWS.Web.ViewModels.Account;

namespace TWS.Web.Controllers;

public class AccountController : Controller
{
    private readonly IApiClient _apiClient;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IApiClient apiClient, ILogger<AccountController> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var loginRequest = new
            {
                email = model.Email,
                password = model.Password
            };

            var response = await _apiClient.PostAsync<LoginResponseDto>("/auth/login", loginRequest);

            if (response.Success && response.Data != null)
            {
                // Store JWT token in session
                HttpContext.Session.SetString("JwtToken", response.Data.Token);
                HttpContext.Session.SetString("UserEmail", response.Data.Email);
                HttpContext.Session.SetString("UserRole", response.Data.Role);

                // Set auth token in API client
                _apiClient.SetAuthToken(response.Data.Token);

                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.Data.UserId),
                    new Claim(ClaimTypes.Email, response.Data.Email),
                    new Claim(ClaimTypes.Name, $"{response.Data.FirstName} {response.Data.LastName}"),
                    new Claim(ClaimTypes.GivenName, response.Data.FirstName),
                    new Claim(ClaimTypes.Surname, response.Data.LastName),
                    new Claim(ClaimTypes.Role, response.Data.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var registerRequest = new
            {
                email = model.Email,
                password = model.Password,
                firstName = model.FirstName,
                lastName = model.LastName,
                role = model.Role
            };

            var response = await _apiClient.PostAsync<RegisterResponseDto>("/auth/register", registerRequest);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            return View(model);
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // Clear session
            HttpContext.Session.Clear();

            // Clear API client auth token
            _apiClient.ClearAuthToken();

            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User logged out successfully");

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return RedirectToAction(nameof(Login));
        }
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var response = await _apiClient.PostAsync<object>("/auth/forgot-password", model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Password reset link has been sent to your email.";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(Login));
        }

        var model = new ResetPasswordViewModel
        {
            Token = token,
            Email = email
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var resetRequest = new
            {
                email = model.Email,
                token = model.Token,
                newPassword = model.Password
            };

            var response = await _apiClient.PostAsync<object>("/auth/reset-password", resetRequest);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Password reset successful! Please login with your new password.";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred. Please try again.");
            return View(model);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyAccount()
    {
        try
        {
            var response = await _apiClient.GetAsync<UserProfileDto>("/user/profile");

            if (response.Success && response.Data != null)
            {
                var model = new MyAccountViewModel
                {
                    Email = response.Data.Email,
                    FirstName = response.Data.FirstName,
                    LastName = response.Data.LastName,
                    Role = response.Data.Role
                };

                return View(model);
            }

            _logger.LogError("Failed to load user profile");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading My Account page");
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(MyAccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("MyAccount", model);
        }

        try
        {
            var updateRequest = new
            {
                email = model.Email,
                firstName = model.FirstName,
                lastName = model.LastName
            };

            var response = await _apiClient.PutAsync<object>("/user/profile", updateRequest);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(MyAccount));
            }

            TempData["ErrorMessage"] = response.Message;
            return View("MyAccount", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            TempData["ErrorMessage"] = "An error occurred while updating your profile.";
            return View("MyAccount", model);
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please check your password requirements.";
            return RedirectToAction(nameof(MyAccount));
        }

        try
        {
            var changePasswordRequest = new
            {
                currentPassword = model.CurrentPassword,
                newPassword = model.NewPassword
            };

            var response = await _apiClient.PostAsync<object>("/auth/change-password", changePasswordRequest);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction(nameof(MyAccount));
            }

            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction(nameof(MyAccount));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            TempData["ErrorMessage"] = "An error occurred while changing your password.";
            return RedirectToAction(nameof(MyAccount));
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

// DTOs for API responses
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class RegisterResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UserProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}