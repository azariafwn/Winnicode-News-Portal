using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestWinnicode.Controllers.Editor
{
    [Authorize(Roles = "Editor")]
    public class EditorController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }
    }
}
