using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.ViewModels.Editor;

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

        public async Task<IActionResult> Profil()
        {
            var username = User.Identity.Name;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();

            var editor = await _context.Editor
                .Include(e => e.Kategori)
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (editor == null) return NotFound();

            var model = new EditorProfilViewModel
            {
                NamaLengkap = user.NamaLengkap,
                Email = user.Email,
                Gender = user.Gender,
                TanggalLahir = user.TanggalLahir,
                NomorTelepon = user.NomorTelepon,
                Alamat = user.Alamat,
                TanggalBergabung = user.TanggalBergabung,
                KategoriFokus = editor.Kategori?.Nama,
                TotalReview = editor.TotalReview,
                Deskripsi = editor.Deskripsi
            };

            ViewBag.IsEdit = Request.Query["edit"] == "true";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profil(EditorProfilViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();

            user.Gender = model.Gender;
            user.TanggalLahir = model.TanggalLahir;
            user.NomorTelepon = model.NomorTelepon;
            user.Alamat = model.Alamat;

            await _context.SaveChangesAsync();

            return RedirectToAction("Profil");
        }

        public async Task<IActionResult> ArtikelMasuk()
        {
            // Ambil user yang sedang login
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Ambil editor dan kategori yang ditangani
            var editor = await _context.Editor
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (editor == null)
            {
                return NotFound("Editor tidak ditemukan");
            }

            // Ambil semua berita yang kategori subkategorinya sesuai cakupan editor
            var beritaList = await _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Include(b => b.Penulis)
                .Where(b => b.SubKategori.KategoriId == editor.KategoriId)
                .Select(b => new ArtikelEditorViewModel
                {
                    Id = b.Id,
                    Judul = b.Judul,
                    Penulis = b.Penulis.NamaLengkap,
                    Status = b.Status,
                    TanggalEdit = b.Tanggal_Publish // atau LastModified jika punya
                })
                .ToListAsync();

            return View(beritaList);
        }

    }

}
