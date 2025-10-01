using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Core.DTOs.Request.Investor;
using TWS.Core.DTOs.Response.Investor;
using TWS.Web.Services.HttpServices;
using TWS.Web.ViewModels.Investor;

namespace TWS.Web.Controllers
{
    /// <summary>
    /// Controller for investor profile management and type selection
    /// </summary>
    [Authorize(Roles = "Investor")]
    public class InvestorController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<InvestorController> _logger;

        public InvestorController(IApiClient apiClient, ILogger<InvestorController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        /// <summary>
        /// Display investor type selection page
        /// GET: /Investor/SelectType
        /// </summary>
        [HttpGet]
        public IActionResult SelectType()
        {
            return View();
        }

        #region Individual Type

        /// <summary>
        /// Display Individual investor type form
        /// GET: /Investor/SelectIndividual
        /// </summary>
        [HttpGet]
        public IActionResult SelectIndividual()
        {
            return View(new SelectInvestorTypeIndividualViewModel());
        }

        /// <summary>
        /// Submit Individual investor type
        /// POST: /Investor/SubmitIndividual
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitIndividual(SelectInvestorTypeIndividualViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectIndividual", model);
            }

            try
            {
                var request = new SelectInvestorTypeIndividualRequest
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsUSCitizen = model.IsUSCitizen,
                    IsAccredited = model.IsAccredited,
                    AccreditationType = model.AccreditationType
                };

                var response = await _apiClient.PostAsync<InvestorProfileResponse>(
                    "/api/investor/select-type/individual",
                    request);

                if (response.Success && response.Data != null)
                {
                    TempData["SuccessMessage"] = "Investor profile created successfully!";
                    return RedirectToAction("Dashboard", new { profileId = response.Data.Id });
                }

                TempData["ErrorMessage"] = response.Message ?? "Failed to create investor profile.";
                return View("SelectIndividual", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting Individual investor type");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View("SelectIndividual", model);
            }
        }

        #endregion

        #region Joint Type

        [HttpGet]
        public IActionResult SelectJoint()
        {
            return View(new SelectInvestorTypeJointViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitJoint(SelectInvestorTypeJointViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectJoint", model);
            }

            try
            {
                var request = new SelectInvestorTypeJointRequest
                {
                    IsJointInvestment = true,
                    JointAccountType = model.JointAccountType,
                    PrimaryFirstName = model.PrimaryFirstName,
                    PrimaryLastName = model.PrimaryLastName,
                    PrimaryIsUSCitizen = model.PrimaryIsUSCitizen,
                    SecondaryFirstName = model.SecondaryFirstName,
                    SecondaryLastName = model.SecondaryLastName,
                    SecondaryIsUSCitizen = model.SecondaryIsUSCitizen,
                    IsAccredited = model.IsAccredited,
                    AccreditationType = model.AccreditationType
                };

                var response = await _apiClient.PostAsync<InvestorProfileResponse>(
                    "/api/investor/select-type/joint",
                    request);

                if (response.Success && response.Data != null)
                {
                    TempData["SuccessMessage"] = "Joint investor profile created successfully!";
                    return RedirectToAction("Dashboard", new { profileId = response.Data.Id });
                }

                TempData["ErrorMessage"] = response.Message ?? "Failed to create investor profile.";
                return View("SelectJoint", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting Joint investor type");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View("SelectJoint", model);
            }
        }

        #endregion

        #region IRA Type

        [HttpGet]
        public IActionResult SelectIRA()
        {
            return View(new SelectInvestorTypeIRAViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitIRA(SelectInvestorTypeIRAViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectIRA", model);
            }

            try
            {
                var request = new SelectInvestorTypeIRARequest
                {
                    IRAType = model.IRAType,
                    NameOfIRA = model.NameOfIRA,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsUSCitizen = model.IsUSCitizen,
                    IsAccredited = model.IsAccredited,
                    AccreditationType = model.AccreditationType
                };

                var response = await _apiClient.PostAsync<InvestorProfileResponse>(
                    "/api/investor/select-type/ira",
                    request);

                if (response.Success && response.Data != null)
                {
                    TempData["SuccessMessage"] = "IRA investor profile created successfully!";
                    return RedirectToAction("Dashboard", new { profileId = response.Data.Id });
                }

                TempData["ErrorMessage"] = response.Message ?? "Failed to create investor profile.";
                return View("SelectIRA", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting IRA investor type");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View("SelectIRA", model);
            }
        }

        #endregion

        #region Trust Type

        [HttpGet]
        public IActionResult SelectTrust()
        {
            return View(new SelectInvestorTypeTrustViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTrust(SelectInvestorTypeTrustViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectTrust", model);
            }

            try
            {
                var request = new SelectInvestorTypeTrustRequest
                {
                    TrustName = model.TrustName,
                    IsUSTrust = model.IsUSTrust,
                    TrustType = model.TrustType,
                    IsAccredited = model.IsAccredited,
                    AccreditationType = model.AccreditationType
                };

                var response = await _apiClient.PostAsync<InvestorProfileResponse>(
                    "/api/investor/select-type/trust",
                    request);

                if (response.Success && response.Data != null)
                {
                    TempData["SuccessMessage"] = "Trust investor profile created successfully!";
                    return RedirectToAction("Dashboard", new { profileId = response.Data.Id });
                }

                TempData["ErrorMessage"] = response.Message ?? "Failed to create investor profile.";
                return View("SelectTrust", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting Trust investor type");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View("SelectTrust", model);
            }
        }

        #endregion

        #region Entity Type

        [HttpGet]
        public IActionResult SelectEntity()
        {
            return View(new SelectInvestorTypeEntityViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEntity(SelectInvestorTypeEntityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectEntity", model);
            }

            try
            {
                var request = new SelectInvestorTypeEntityRequest
                {
                    CompanyName = model.CompanyName,
                    IsUSCompany = model.IsUSCompany,
                    EntityType = model.EntityType,
                    IsAccredited = model.IsAccredited,
                    AccreditationType = model.AccreditationType
                };

                var response = await _apiClient.PostAsync<InvestorProfileResponse>(
                    "/api/investor/select-type/entity",
                    request);

                if (response.Success && response.Data != null)
                {
                    TempData["SuccessMessage"] = "Entity investor profile created successfully!";
                    return RedirectToAction("Dashboard", new { profileId = response.Data.Id });
                }

                TempData["ErrorMessage"] = response.Message ?? "Failed to create investor profile.";
                return View("SelectEntity", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting Entity investor type");
                TempData["ErrorMessage"] = "An unexpected error occurred.";
                return View("SelectEntity", model);
            }
        }

        #endregion

        #region Dashboard (Placeholder)

        /// <summary>
        /// Display investor dashboard
        /// GET: /Investor/Dashboard
        /// </summary>
        [HttpGet]
        public IActionResult Dashboard(int profileId)
        {
            // Placeholder - will be implemented later
            ViewBag.ProfileId = profileId;
            return View();
        }

        #endregion
    }
}
