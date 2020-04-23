using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Example.Models;
using RequestThrottle.Filter;

namespace Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ServiceFilter(typeof(IRequestThrottleFilterAttribute))]
        [HttpPost]
        public IActionResult IndexPost(string email, string password)
        {
            var clientIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var message = $"Email:{email} , Password : {password},Host address : {clientIpAddress}";
            _logger.LogInformation(message);
            if (ModelState.IsValid) return View("LoginSuccess", new { Email = email} );
            return View("Index");
        }

        public IActionResult LoginSuccess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
