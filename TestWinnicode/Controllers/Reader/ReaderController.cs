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

            // Ambil 4 kategori (acak atau sesuai ID)
            var kategoriList = _context.Kategori
                .OrderBy(k => Guid.NewGuid()) // acak
                .Take(4)
                .ToList();

            var whatsOnDict = new Dictionary<string, List<Berita>>();

            foreach (var kategori in kategoriList)
            {
                var beritaPerKategori = _context.Berita
                    .Include(b => b.SubKategori)
                    .ThenInclude(sk => sk.Kategori)
                    .Where(b => b.SubKategori.KategoriId == kategori.Id)
                    .OrderByDescending(b => b.Tanggal_Publish)
                    .Take(6)
                    .ToList();

                whatsOnDict[kategori.Nama] = beritaPerKategori;
            }

            var viewModel = new HomeViewModel
            {
                WhatsOnByKategori = whatsOnDict,

                Headline = _context.Berita
                    .Include(b => b.SubKategori)
                    .ThenInclude(sk => sk.Kategori)
                    .FirstOrDefault(b => b.IsHeadline),

                SubHeadlineList = _context.Berita
                    .Include(b => b.SubKategori)
                    .ThenInclude(sk => sk.Kategori)
                    .Where(b => b.IsSubHeadline)
                    .OrderByDescending(b => b.Tanggal_Publish)
                    .Take(6)
                    .ToList(),

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
                    .ToList(),

                 PenulisList = _context.Penulis
                    .Include(p => p.User)
                    .Include(p => p.KategoriFokus)
                    .Take(7)
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

            var headline = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .FirstOrDefault(b => b.SubKategori.KategoriId == kategori.Id && b.IsHeadline);

            var subHeadlines = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Where(b => b.SubKategori.KategoriId == kategori.Id && b.IsSubHeadline)
                .Take(3)
                .ToList();

            // berita per subkategori untuk di halaman kategori
            var beritaPerSubKategori = subKategoriList.ToDictionary(
                sub => sub.Nama,
                sub => _context.Berita
                    .Include(b => b.SubKategori)
                    .Where(b => b.SubKategoriId == sub.Id)
                    .OrderByDescending(b => b.Tanggal_Publish)
                    .Take(6)
                    .ToList()
            );


            var viewModel = new KategoriViewModel
            {
                KategoriList = new List<Kategori> { kategori },
                SubKategoriList = subKategoriList,
                BeritaList = beritaList,
                TrendingList = trendingList,
                TerbaruList = terbaruList,
                Headline = headline,
                SubHeadlines = subHeadlines,
                BeritaPerSubKategori = beritaPerSubKategori
            };

            return View(viewModel);
        }
        public IActionResult Berita(int id)
        {
            var berita = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Include(b => b.Penulis)
                .ThenInclude(p => p.User)
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

        [HttpGet]
        public IActionResult ProfilUser(bool edit = false)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            ViewBag.IsEdit = edit;
            return View(user);
        }

        [HttpPost]
        public IActionResult ProfilUser(string Gender, DateTime? TanggalLahir, string NomorTelepon, string Alamat)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.Gender = Gender;
                user.TanggalLahir = TanggalLahir;
                user.NomorTelepon = NomorTelepon;
                user.Alamat = Alamat;

                _context.SaveChanges();
            }

            ViewBag.IsEdit = false;
            return RedirectToAction("ProfilUser");
        }

        public IActionResult ProfilPenulis(int id)
        {
            var penulis = _context.Penulis
                .Include(p => p.User)
                .Include(p => p.KategoriFokus)
                .FirstOrDefault(p => p.Id == id);

            if (penulis == null)
            {
                return NotFound();
            }

            var beritaTerbaru = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Where(b => b.PenulisId == id)
                .OrderByDescending(b => b.Tanggal_Publish)
                .Take(6)
                .ToList();

            var totalView = _context.Berita
                .Where(b => b.PenulisId == id)
                .Sum(b => b.Jumlah_View);

            var viewModel = new ProfilPenulisViewModel
            {
                User = penulis.User,
                Penulis = penulis,
                BeritaTerbaru = beritaTerbaru,
                TotalView = totalView
            };

            return View(viewModel);
        }

    }
}
