﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.Models.Reader;
using TestWinnicode.ViewModels.Reader;
using TestWinnicode.ViewModels.Penulis;

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

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return RedirectToAction("Login", "Account");

            var penulis = await _context.Penulis
                .Include(p => p.KategoriFokus)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (penulis == null) return NotFound();

            var artikel = await _context.Berita
                .Where(b => b.PenulisId == penulis.Id)
                .ToListAsync();

            var total = artikel.Count;
            var draft = artikel.Count(b => b.Status == "Draft");
            var ditolak = artikel.Count(b => b.Status == "Ditolak");
            var menunggu = artikel.Count(b => b.Status == "Ditinjau");
            var cakupanSubkategori = await _context.Berita
                .Where(b => b.PenulisId == penulis.Id)
                .Select(b => b.SubKategoriId)
                .Distinct()
                .CountAsync();

            var model = new PenulisDashboardViewModel
            {
                NamaLengkap = user.NamaLengkap,
                TotalArtikel = total,
                JumlahArtikelDraft = draft,
                JumlahArtikelDitolak = ditolak,
                JumlahArtikelMenunggu = menunggu,
                CakupanSubKategori = cakupanSubkategori
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
        public async Task<IActionResult> TulisArtikel(TulisArtikelViewModel model, string submitType)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var penulis = await _context.Penulis.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (!ModelState.IsValid || penulis == null)
            {
                ViewBag.KategoriList = await _context.Kategori.ToListAsync();
                return View(model);
            }

            // Simpan gambar
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

            // Tentukan status
            string status = (submitType == "draft") ? "Draft" : "Ditinjau";

            // Simpan ke database
            var berita = new Berita
            {
                Judul = model.Judul,
                PenulisId = penulis.Id,
                SubKategoriId = model.SubKategoriId,
                Tag = model.Tag,
                Gambar = gambarPath,
                Isi = model.IsiArtikel,
                Tanggal_Publish = DateTime.Now,
                Status = status,
                Jumlah_View = 0,
                IsHeadline = false,
                IsSubHeadline = false
            };

            _context.Berita.Add(berita);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArtikelSaya");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteArtikel(int id)
        {
            var artikel = await _context.Berita.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            _context.Berita.Remove(artikel);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArtikelSaya");
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
        public async Task<IActionResult> EditProfil(PenulisProfilViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            var penulis = await _context.Penulis.FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (user != null && penulis != null)
            {
                user.NamaLengkap = model.NamaLengkap;
                user.Email = model.Email;
                user.Gender = model.Gender;
                user.TanggalLahir = model.TanggalLahir;
                user.NomorTelepon = model.NomorTelepon;
                user.Alamat = model.Alamat;

                penulis.Deskripsi = model.Deskripsi;

                await _context.SaveChangesAsync();
            }

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

            // Hanya bisa diedit jika statusnya Draft atau Ditolak
            if (artikel.Status != "Draft" && artikel.Status != "Ditolak")
            {
                // Opsional: Redirect atau tampilkan pesan error
                TempData["ErrorMessage"] = "Artikel ini tidak dapat diedit karena sedang ditinjau atau sudah terbit.";
                return RedirectToAction("ArtikelSaya");
            }

            var viewModel = new EditArtikelViewModel
            {
                Id = artikel.Id,
                Judul = artikel.Judul,
                Isi = artikel.Isi,
                Status = artikel.Status,
                KomentarEditor = artikel.KomentarEditor
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditArtikel(EditArtikelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var artikel = await _context.Berita.FindAsync(model.Id);
            if (artikel == null)
                return NotFound();

            artikel.Judul = model.Judul;
            artikel.Isi = model.Isi;
            artikel.Status = model.Status; // ← Update status sesuai tombol yang diklik
            artikel.Tanggal_Publish = DateTime.Now;

            if (artikel.Status == "Ditinjau")
            {
                artikel.KomentarEditor = "";
            }

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
