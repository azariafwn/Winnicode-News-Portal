using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWinnicode.Data;
using TestWinnicode.ViewModels;

namespace TestWinnicode.Controllers.Penulis
{
    [Authorize(Roles = "Penulis")]
    public class PenulisController : Controller
    {
        private readonly AppDbContext _context;

        public PenulisController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null) return RedirectToAction("Login", "Account");

            var penulis = _context.Penulis.FirstOrDefault(p => p.UserId == user.Id);

            var model = new PenulisDashboardViewModel
            {
                NamaLengkap = user.NamaLengkap,
                TotalArtikel = penulis?.TotalArtikel ?? 0,
                JumlahArtikelDraft = penulis?.JumlahArtikelDraft ?? 0,
                JumlahArtikelDitolak = penulis?.JumlahArtikelDitolak ?? 0,
                JumlahArtikelMenunggu = penulis?.JumlahArtikelMenunggu ?? 0,
                CakupanKategori = penulis?.KategoriFokusId != null ? 1 : 0 // bisa dimodifikasi jika ingin hitung lebih kompleks
            };

            return View(model);
        }

        public IActionResult ArtikelSaya() => View();
        public IActionResult TulisArtikel() => View();
        public IActionResult Profil()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var penulis = _context.Penulis.FirstOrDefault(p => p.UserId == user.Id);
            var kategori = penulis?.KategoriFokusId != null
                ? _context.Kategori.FirstOrDefault(k => k.Id == penulis.KategoriFokusId)?.Nama
                : "-";

            var model = new PenulisProfilViewModel
            {
                NamaLengkap = user.NamaLengkap,
                Email = user.Email,
                Gender = user.Gender,
                TanggalLahir = user.TanggalLahir,
                NomorTelepon = user.NomorTelepon,
                Alamat = user.Alamat,
                TanggalBergabung = user.TanggalBergabung,
                Deskripsi = penulis?.Deskripsi ?? "-",
                TotalArtikel = penulis?.TotalArtikel ?? 0,
                TotalDibaca = penulis?.TotalDibaca ?? 0,
                KategoriFokus = kategori,
                JumlahArtikelDraft = penulis?.JumlahArtikelDraft ?? 0,
                JumlahArtikelDitolak = penulis?.JumlahArtikelDitolak ?? 0,
                JumlahArtikelMenunggu = penulis?.JumlahArtikelMenunggu ?? 0
            };

            return View(model);
        }

    }
}
