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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            var editor = await _context.Editor.FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (user != null && editor != null)
            {
                user.NamaLengkap = model.NamaLengkap;
                user.Email = model.Email;
                user.Gender = model.Gender;
                user.TanggalLahir = model.TanggalLahir;
                user.NomorTelepon = model.NomorTelepon;
                user.Alamat = model.Alamat;

                editor.Deskripsi = model.Deskripsi;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profil");
        }



        public async Task<IActionResult> ArtikelMasuk()
        {
            // Ambil user yang sedang login
            var username = User.Identity.Name;
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

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
                .Where(b => b.SubKategori.KategoriId == editor.KategoriId)
                .Join(_context.Penulis.Include(p => p.User),
                    berita => berita.PenulisId,
                    penulis => penulis.Id,
                    (berita, penulis) => new ArtikelEditorViewModel
                    {
                        Id = berita.Id,
                        Judul = berita.Judul,
                        Penulis = penulis.User.NamaLengkap,
                        Status = berita.Status,
                        TanggalEdit = berita.Tanggal_Publish
                    })
                .ToListAsync();

            return View(beritaList);
        }

    }

}
