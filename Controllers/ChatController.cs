using Administrator.Helper;
using Administrator.Services.auth_project.Services;
using Administrator.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Administrator.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly string _routeApi;

        public ChatController(IChatService chatService, IConfiguration config)
        {
            _chatService = chatService;
            _routeApi = config["ApiUrl"];
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Auth");
            }
            string[]? roles = JwtHelper.GetRolesFromToken(token);
            bool isAdmin = roles != null && roles.Any(r =>
                r.Equals("admin", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("administrator", StringComparison.OrdinalIgnoreCase) ||
                r.Equals("authorization", StringComparison.OrdinalIgnoreCase));

            if (!isAdmin)
            {
                return RedirectToAction("UnauthorizedAccess", "Auth");
            }

            HttpContext.Session.SetString("Roles", string.Join(",", roles ?? new string[] { }));
            ViewBag.ApiUrl = _routeApi;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetHistory([FromBody] ChatHistoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId1) || string.IsNullOrWhiteSpace(request.UserId2))
                return BadRequest(new { message = "User IDs are required" });

            var result = await _chatService.GetChatHistoryAsync(request.UserId1, request.UserId2);

            return Ok(result);
        }
    }
}
