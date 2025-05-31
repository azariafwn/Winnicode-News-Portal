using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWinnicode.ViewModels;
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
            
            var viewModel = new HomeViewModel
            {
                TrendingList = _context.Berita
            .Include(b => b.SubKategori)
            .ThenInclude(sk => sk.Kategori)
            .OrderByDescending(b => b.Jumlah_View)
            .Take(10)
            .ToList(),

                TerbaruList = _context.Berita
            .Include(b => b.SubKategori)
            .ThenInclude(sk => sk.Kategori)
            .OrderByDescending(b => b.Tanggal_Publish)
            .Take(12)
            .ToList()
            };

            return View(viewModel);
        }
        public IActionResult Kategori(string nama)
        {
            // Ambil kategori berdasarkan nama (case-insensitive)
            var kategori = _context.Kategori
                .FirstOrDefault(k => k.Nama.ToLower() == nama.ToLower());

            if (kategori == null)
            {
                return NotFound();
            }

            var subKategoriList = _context.SubKategori
                .Where(sk => sk.KategoriId == kategori.Id)
                .Include(sk => sk.Kategori)
                .ToList();

            var beritaList = _context.Berita
                .Include(b => b.SubKategori)
                .Where(b => b.SubKategori.KategoriId == kategori.Id)
                .OrderByDescending(b => b.Tanggal_Publish)
                .Take(24)
                .ToList();

            var trendingList = _context.Berita
                .Where(b => b.SubKategori.KategoriId == kategori.Id)
                .OrderByDescending(b => b.Jumlah_View)
                .Take(10)
                .ToList();

            var terbaruList = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Where(b => b.SubKategori.KategoriId == kategori.Id)
                .OrderByDescending(b => b.Tanggal_Publish)
                .Take(10)
                .ToList();

            var viewModel = new KategoriViewModel
            {
                KategoriList = new List<Kategori> { kategori },
                SubKategoriList = subKategoriList,
                BeritaList = beritaList,
                TrendingList = trendingList,
                TerbaruList = terbaruList
            };

            return View(viewModel);
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

            var trending = _context.Berita
                .Include(b => b.SubKategori).ThenInclude(sk => sk.Kategori)
                .OrderByDescending(b => b.Jumlah_View)
                .Take(8)
                .ToList();

            var terbaru = _context.Berita
                .Include(b => b.SubKategori).ThenInclude(sk => sk.Kategori)
                .OrderByDescending(b => b.Tanggal_Publish)
                .Take(6)
                .ToList();

            var viewModel = new BeritaViewModel
            {
                BeritaDetail = berita,
                TrendingList = trending,
                TerbaruList = terbaru
            };

            return View("Berita", viewModel);
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
