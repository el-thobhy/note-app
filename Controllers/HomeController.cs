using System.Diagnostics;
using Administrator.Helper;
using Administrator.Services;
using Administrator.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Administrator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(
            ILogger<HomeController> logger,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Auth");
            }
            string[]? roles = JwtHelper.GetRolesFromToken(token);
            HttpContext.Session.SetString("Roles", string.Join(",", roles ?? new string[] { }));
            return View();
        }

        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountService.GetAccountsAsync();
            return Json(new { data = accounts });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
        {
            var admin = HttpContext.Session.GetString("UserName") ?? "";

            var result = await _accountService.UpdateUserRoleAsync(request, admin);
            if (result)
                return Ok(new { message = "Role updated successfully" });
            else
                return BadRequest(new { message = "Failed to update role" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request)
        {
            var admin = HttpContext.Session.GetString("UserName") ?? "";

            var success = await _accountService.DeleteAccountAsync(request, admin);
            if (success)
                return Ok(new { message = "Account deleted successfully" });
            else
                return StatusCode(500, new { message = "Failed to delete account" });
        }


    }
}
