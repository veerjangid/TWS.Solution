using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWS.Web.Models;

namespace TWS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // If user is authenticated, redirect to dashboard
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        return View();
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        ViewData["UserName"] = User.Identity?.Name ?? "User";
        ViewData["UserRole"] = User.IsInRole("Investor") ? "Investor" :
                               User.IsInRole("Advisor") ? "Advisor" :
                               User.IsInRole("OperationsTeam") ? "Operations Team" : "User";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
