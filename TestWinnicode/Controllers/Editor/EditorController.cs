﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.ViewModels.Editor;
using TestWinnicode.ViewModels.Penulis;

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
            var username = User.Identity.Name;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            var editor = await _context.Editor
                .FirstOrDefaultAsync(e => e.UserId == user.Id);

            if (editor == null)
                return NotFound("Editor tidak ditemukan");

            var kategoriId = editor.KategoriId;

            var jumlahMenunggu = await _context.Berita
                .Where(b => b.SubKategori.KategoriId == kategoriId && b.Status == "Ditinjau")
                .CountAsync();

            var jumlahTerbit = await _context.Berita
                .Where(b => b.SubKategori.KategoriId == kategoriId && b.Status == "Terbit")
                .CountAsync();

            var jumlahDitolak = await _context.Berita
                .Where(b => b.SubKategori.KategoriId == kategoriId && b.Status == "Ditolak")
                .CountAsync();

            var jumlahPenulis = await _context.Berita
                .Include(b => b.SubKategori)
                .Where(b => b.SubKategori.KategoriId == kategoriId &&
                            (b.Status == "Ditinjau" || b.Status == "Terbit" || b.Status == "Ditolak"))
                .Select(b => b.PenulisId)
                .Distinct()
                .CountAsync();


            var jumlahArtikel = jumlahMenunggu + jumlahDitolak + jumlahTerbit;

            // Kirim ke ViewBag
            ViewBag.JumlahArtikel = jumlahArtikel;
            ViewBag.Menunggu = jumlahMenunggu;
            ViewBag.Terbit = jumlahTerbit;
            ViewBag.Ditolak = jumlahDitolak;
            ViewBag.JumlahPenulis = jumlahPenulis;

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
                // Hanya update jika data tidak null
                if (!string.IsNullOrEmpty(model.NamaLengkap)) user.NamaLengkap = model.NamaLengkap;
                if (!string.IsNullOrEmpty(model.Email)) user.Email = model.Email;
                if (!string.IsNullOrEmpty(model.Gender)) user.Gender = model.Gender;
                if (model.TanggalLahir.HasValue) user.TanggalLahir = model.TanggalLahir;
                if (!string.IsNullOrEmpty(model.NomorTelepon)) user.NomorTelepon = model.NomorTelepon;
                if (!string.IsNullOrEmpty(model.Alamat)) user.Alamat = model.Alamat;

                if (!string.IsNullOrEmpty(model.Deskripsi)) editor.Deskripsi = model.Deskripsi;

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
                .Where(b =>
                    b.SubKategori.KategoriId == editor.KategoriId &&
                    (b.Status == "Ditinjau" || b.Status == "Ditolak" || b.Status == "Terbit")
                )
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

        // GET: Editor/LihatArtikel/5
        public async Task<IActionResult> DetailArtikel(int id)
        {
            var artikel = await _context.Berita
                .Include(b => b.Penulis)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (artikel == null)
                return NotFound();

            return View("Detail", artikel);
        }

        public async Task<IActionResult> EditArtikel(int id)
        {
            var artikel = await _context.Berita.FirstOrDefaultAsync(b => b.Id == id);
            if (artikel == null)
                return NotFound();

            var model = new EditArtikelEditorViewModel
            {
                Id = artikel.Id,
                Judul = artikel.Judul,
                Isi = artikel.Isi,
                Status = artikel.Status,
                KomentarEditor = artikel.KomentarEditor
            };

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditArtikel(EditArtikelEditorViewModel model)
        {
            // Tambahkan validasi ModelState jika diperlukan
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var artikel = await _context.Berita.FindAsync(model.Id);
            if (artikel == null)
                return NotFound();

            artikel.Judul = model.Judul;
            artikel.Isi = model.Isi;
            artikel.Status = model.Status;
            artikel.Tanggal_Publish = DateTime.Now;

            // Simpan komentar jika statusnya 'Ditolak'
            if (model.Status == "Ditolak")
            {
                artikel.KomentarEditor = model.KomentarEditor;
            }
            else
            {
                // Opsional: Hapus komentar jika artikel disetujui
                artikel.KomentarEditor = "";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ArtikelMasuk");
        }

        public async Task<IActionResult> HapusArtikel(int id)
        {
            var artikel = await _context.Berita.FindAsync(id);
            if (artikel == null)
                return NotFound();

            _context.Berita.Remove(artikel);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArtikelMasuk");
        }

    }

}
