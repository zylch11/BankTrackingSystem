using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BankTrackingSystem.Models;
using BankTrackingSystem.Data;

namespace BankTrackingSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApplicantMessagesRespository _applicantMessagesRespository;

    public HomeController(ILogger<HomeController> logger, IApplicantMessagesRespository applicantMessagesRespository)
    {
        _logger = logger;
        _applicantMessagesRespository = applicantMessagesRespository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details(string emailAddress)
    {
        var messages = await _applicantMessagesRespository.GetAllAgainstEmailAddress(emailAddress);
        return View(messages);
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
