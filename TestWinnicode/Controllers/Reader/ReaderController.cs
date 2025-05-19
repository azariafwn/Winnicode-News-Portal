using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestWinnicode.Controllers.Reader
{
    [Authorize(Roles = "Reader")]
    public class ReaderController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }
        public IActionResult Kategori()
        {
            return View();
        }
        public IActionResult Berita()
        {
            return View();
        }
    }
}
