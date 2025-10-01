using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.Account;
using TWS.Core.DTOs.Response.Account;
using TWS.Web.Services.HttpServices;
using TWS.Web.ViewModels.RequestAccount;

namespace TWS.Web.Controllers
{
    /// <summary>
    /// Controller for handling account requests from potential investors
    /// Public access - no authentication required
    /// </summary>
    public class RequestAccountController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<RequestAccountController> _logger;

        public RequestAccountController(IApiClient apiClient, ILogger<RequestAccountController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Displays the Request Account form
        /// GET: /RequestAccount
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RequestAccountViewModel());
        }

        /// <summary>
        /// Handles the submission of the Request Account form
        /// POST: /RequestAccount/Submit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(RequestAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                _logger.LogInformation("Submitting account request for email: {Email}", model.Email);

                // Map ViewModel to API DTO
                var request = new CreateAccountRequestRequest
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Message = model.Message
                };

                // Call API
                var response = await _apiClient.PostAsync<AccountRequestResponse>(
                    "/api/request-account",
                    request);

                if (response.Success)
                {
                    _logger.LogInformation("Account request submitted successfully for {Email}", model.Email);

                    TempData["SuccessMessage"] = "Thank you for your interest! We have received your request and will contact you shortly.";

                    // Clear form by returning a new model
                    return View("Index", new RequestAccountViewModel());
                }
                else
                {
                    _logger.LogWarning("Account request submission failed: {Message}", response.Message);

                    TempData["ErrorMessage"] = response.Message ?? "Failed to submit your request. Please try again.";
                    return View("Index", model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting account request for {Email}", model.Email);

                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return View("Index", model);
            }
        }
    }
}
