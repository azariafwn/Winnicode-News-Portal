using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWinnicode.Data;

namespace TestWinnicode.Controllers.Editor
{
    [Authorize(Roles = "Editor")]
    public class EditorController : Controller
    {
        private readonly AppDbContext _context;

        public EditorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var jumlahArtikel = 10;
            var jumlahMenunggu = 10;
            var jumlahTerbit = 10;
            var jumlahDitolak = 10;

            ViewBag.JumlahArtikel = jumlahArtikel;
            ViewBag.Menunggu = jumlahMenunggu;
            ViewBag.Terbit = jumlahTerbit;
            ViewBag.Ditolak = jumlahDitolak;

            return View();
        }
    }

}
