using Microsoft.AspNetCore.Mvc;

namespace Administrator.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OtpVerification()
        {
            return View();
        }
    }
}
