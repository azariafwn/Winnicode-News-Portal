using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestWinnicode.Controllers.Penulis
{
    [Authorize(Roles = "Penulis")]
    public class PenulisController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }
    }
}
