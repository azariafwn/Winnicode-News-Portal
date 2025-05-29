using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.Models;

namespace TestWinnicode.Controllers.Reader
{
    [Authorize(Roles = "Reader")]
    public class ReaderController : Controller
    {
        private readonly AppDbContext _context;
        public ReaderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;
            return View();
        }
        public IActionResult Kategori()
        {
            return View();
        }
        public IActionResult Berita(int id)
        {
            var berita = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .FirstOrDefault(b => b.Id == id);


            if (berita == null)
            {
                return NotFound();
            }

            return View("Berita", berita);
        }

        public IActionResult ProfilUser()
        {
            return View();
        }
        public IActionResult ProfilPenulis()
        {
            return View();
        }
    }
}
