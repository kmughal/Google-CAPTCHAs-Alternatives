namespace Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Web.Filters;
    using Web.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
        }

        [ServiceFilter(typeof(RequestLimitFilter))]
        [HttpPost]
        public IActionResult RateLimitPost(string email, string password)
        {
            var clientIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var message = $"Email:{email} , Password : {password},Host address : {clientIpAddress}";
            _logger.LogInformation(message);
            if (ModelState.IsValid) return RedirectToAction("LoginSuccess", new { email });
            return View("RateLimit");
        }

        public IActionResult LoginSuccess(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        public IActionResult RateLimit()
        {
            return View();
        }

        public IActionResult Honeypot()
        {
            return View();
        }

        [HoneypotFilter]
        public IActionResult HoneypotPost()
        {
            var email = Request.Form["email"].ToString();
            if (ModelState.IsValid) return RedirectToAction("LoginSuccess", new { email });
            return View("Honeypot");
        }

        public IActionResult RandomQuestion() {
            return View();
        }

        [RandomQuestionValidationFilter]
         public IActionResult RandomQuestionPost() {
            var email = "Your answer was valid so you can proceed";
            if (ModelState.IsValid) return RedirectToAction("LoginSuccess", new { email });
            return View("RandomQuestion");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
