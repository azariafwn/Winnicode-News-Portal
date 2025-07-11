using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.Models;
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
        // Ambil semua kategori saat render halaman
        [HttpGet]
        public async Task<IActionResult> TulisArtikel()
        {
            var kategoriList = await _context.Kategori.ToListAsync();
            ViewBag.KategoriList = kategoriList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TulisArtikel(TulisArtikelViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var penulis = await _context.Penulis.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (!ModelState.IsValid || penulis == null)
            {
                ViewBag.KategoriList = await _context.Kategori.ToListAsync();
                return View(model);
            }

            string gambarPath = null;
            if (model.Gambar != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(folder);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Gambar.FileName);
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Gambar.CopyToAsync(stream);
                }

                gambarPath = "/uploads/" + fileName;
            }

            var berita = new Berita
            {
                Judul = model.Judul,
                PenulisId = penulis.Id,
                SubKategoriId = model.SubKategoriId,
                Tag = model.Tag,
                Gambar = gambarPath,
                Isi = model.IsiArtikel,
                Tanggal_Publish = DateTime.Now,
                Status = "Draft",
                Jumlah_View = 0,
                IsHeadline = false,
                IsSubHeadline = false
            };

            _context.Berita.Add(berita);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArtikelSaya");
        }



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
        public async Task<IActionResult> ArtikelSaya()
        {
            // Ambil username dari sesi login
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            // Ambil user dari database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ambil penulis dari tabel Penulis
            var penulis = await _context.Penulis.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (penulis == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ambil semua artikel milik penulis ini
            var artikelList = await _context.Berita
                .Where(b => b.PenulisId == penulis.Id)
                .OrderByDescending(b => b.Tanggal_Publish)
                .Select(b => new ArtikelPenulisViewModel
                {
                    Id = b.Id,
                    Judul = b.Judul,
                    Status = b.Status,
                    TanggalEdit = b.Tanggal_Publish
                })
                .ToListAsync();

            return View(artikelList);
        }


        [HttpGet]
        public async Task<IActionResult> Profil(bool edit = false)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return NotFound();

            var penulis = await _context.Penulis
                .Include(p => p.KategoriFokus)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var vm = new PenulisProfilViewModel
            {
                NamaLengkap = user.NamaLengkap,
                Email = user.Email,
                Gender = user.Gender,
                TanggalLahir = user.TanggalLahir,
                NomorTelepon = user.NomorTelepon,
                Alamat = user.Alamat,
                TanggalBergabung = user.TanggalBergabung,
                Role = user.Role.ToString(),
                KategoriFokus = penulis?.KategoriFokus?.Nama ?? "-",
                Deskripsi = penulis?.Deskripsi ?? "",
                TotalDibaca = penulis?.TotalDibaca ?? 0,
                TotalArtikel = penulis?.TotalArtikel ?? 0,
                JumlahArtikelDraft = penulis?.JumlahArtikelDraft ?? 0,
                JumlahArtikelDitolak = penulis?.JumlahArtikelDitolak ?? 0,
                JumlahArtikelMenunggu = penulis?.JumlahArtikelMenunggu ?? 0
            };

            ViewBag.IsEdit = edit;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfil(string Gender, DateTime TanggalLahir, string NomorTelepon, string Alamat)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return NotFound();

            user.Gender = Gender;
            user.TanggalLahir = TanggalLahir;
            user.NomorTelepon = NomorTelepon;
            user.Alamat = Alamat;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profil");
        }
        [HttpGet]
        public async Task<IActionResult> DetailArtikel(int id)
        {
            var artikel = await _context.Berita
                .Include(b => b.SubKategori)
                    .ThenInclude(sk => sk.Kategori)
                .Include(b => b.Penulis)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            var username = User.Identity.Name;
            if (artikel.Penulis?.User?.Username != username)
            {
                return Forbid();
            }


            if (artikel == null)
            {
                return NotFound();
            }

            return View("DetailArtikel", artikel);
        }

        [HttpGet]
        public async Task<IActionResult> EditArtikel(int id)
        {
            var artikel = await _context.Berita
                .Include(b => b.Penulis)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (artikel == null) return NotFound();

            var viewModel = new EditArtikelViewModel
            {
                Id = artikel.Id,
                Judul = artikel.Judul,
                Isi = artikel.Isi
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditArtikel(EditArtikelViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var artikel = await _context.Berita.FirstOrDefaultAsync(b => b.Id == model.Id);

            if (artikel == null) return NotFound();

            artikel.Judul = model.Judul;
            artikel.Isi = model.Isi;
            artikel.Tanggal_Publish = DateTime.Now; // update waktu edit

            await _context.SaveChangesAsync();
            return RedirectToAction("ArtikelSaya");
        }

        // Ambil subkategori via AJAX
        [HttpGet]
        public async Task<JsonResult> GetSubKategori(int kategoriId)
        {
            var subkategoriList = await _context.SubKategori
                .Where(s => s.KategoriId == kategoriId)
                .Select(s => new { s.Id, s.Nama })
                .ToListAsync();

            return Json(subkategoriList);
        }

    }
}
