using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWinnicode.ViewModels;
using TestWinnicode.ViewModels.Penulis;
using TestWinnicode.ViewModels.Reader;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Data;
using TestWinnicode.Models.Reader;

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
        public async Task<IActionResult> Berita(int id)
        {
            var berita = _context.Berita
                .Include(b => b.SubKategori).ThenInclude(sk => sk.Kategori)
                .Include(b => b.Penulis).ThenInclude(p => p.User)
                .FirstOrDefault(b => b.Id == id);

            if (berita == null)
            {
                return NotFound();
            }
            var likeCount = await _context.LikeDislikeBerita.CountAsync(ld => ld.BeritaId == id && ld.IsLike);
            var dislikeCount = await _context.LikeDislikeBerita.CountAsync(ld => ld.BeritaId == id && !ld.IsLike);
            bool? userLikeStatus = null;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
                if (user != null)
                {
                    var userLikeDislike = await _context.LikeDislikeBerita
                        .FirstOrDefaultAsync(ld => ld.BeritaId == id && ld.UserId == user.Id);
                    if (userLikeDislike != null)
                    {
                        userLikeStatus = userLikeDislike.IsLike;
                    }
                }
            }

            var komentarList = await _context.Komentar
                .Include(k => k.User)
                .Where(k => k.BeritaId == id)
                .OrderByDescending(k => k.Tanggal)
                .Select(k => new KomentarViewModel
                {
                    NamaUser = k.User.NamaLengkap,
                    IsiKomentar = k.Isi,
                    Tanggal = k.Tanggal
                })
                .ToListAsync();

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
                TerbaruList = terbaru,

                KomentarList = komentarList,
                JumlahKomentar = komentarList.Count,
                
                JumlahLike = likeCount,
                JumlahDislike = dislikeCount,
                UserLikeStatus = userLikeStatus
            };

            return View("Berita", viewModel);
        }
        [HttpPost]
        public IActionResult KirimKomentar(int beritaId, string komentar)
        {
            if (string.IsNullOrWhiteSpace(komentar)) return RedirectToAction("Berita", new { id = beritaId });

            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null) return RedirectToAction("Login", "Account");

            var newKomentar = new Komentar
            {
                BeritaId = beritaId,
                UserId = user.Id,
                Isi = komentar,
                Tanggal = DateTime.Now
            };

            _context.Komentar.Add(newKomentar);
            _context.SaveChanges();

            return RedirectToAction("Berita", new { id = beritaId });
        }


        [HttpGet]
        public IActionResult ProfilUser(bool edit = false)
        {
            if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Login", "Account");
            }

            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Cek dan isi default untuk menghindari DBNull
            user.NamaLengkap ??= "Pengguna";
            user.Email ??= "user@example.com";
            user.Gender ??= "-";
            user.TanggalLahir ??= new DateTime(2000, 1, 1);
            user.NomorTelepon ??= "-";
            user.Alamat ??= "-";
            user.TanggalBergabung ??= DateTime.Now;

            ViewBag.IsEdit = edit;
            return View(user);
        }

        [HttpPost]
        public IActionResult ProfilUser(string NamaLengkap, string Email, string Gender, DateTime? TanggalLahir, string NomorTelepon, string Alamat)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                user.NamaLengkap = NamaLengkap;
                user.Email = Email;
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
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index");
            }

            var hasil = _context.Berita
                .Include(b => b.SubKategori)
                .ThenInclude(sk => sk.Kategori)
                .Where(b =>
                        b.Judul.ToLower().Contains(query.ToLower()) ||
                        b.Isi.ToLower().Contains(query.ToLower()) ||
                        b.SubKategori.Nama.ToLower().Contains(query.ToLower()) ||
                        b.SubKategori.Kategori.Nama.ToLower().Contains(query.ToLower())
                )

                .OrderByDescending(b => b.Tanggal_Publish)
                .ToList();

            return View("SearchResult", hasil);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeDislike([FromBody] LikeDislikeRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null) return Unauthorized(new { success = false, message = "User tidak ditemukan." });

            var existing = await _context.LikeDislikeBerita
                .FirstOrDefaultAsync(ld => ld.BeritaId == request.BeritaId && ld.UserId == user.Id);

            bool? newUserStatus = null; // Status baru: true=like, false=dislike, null=netral

            if (existing != null)
            {
                // Jika user mengklik tombol yang sama lagi (misal, klik like saat sudah like)
                if (existing.IsLike == request.IsLike)
                {
                    _context.LikeDislikeBerita.Remove(existing); // Hapus record (batal like/dislike)
                    newUserStatus = null; // Status menjadi netral
                }
                else // Jika user beralih (dari like ke dislike atau sebaliknya)
                {
                    existing.IsLike = request.IsLike;
                    existing.Timestamp = DateTime.Now;
                    _context.LikeDislikeBerita.Update(existing);
                    newUserStatus = existing.IsLike; // Status sesuai aksi baru
                }
            }
            else // Jika belum ada record sebelumnya
            {
                _context.LikeDislikeBerita.Add(new LikeDislikeBerita
                {
                    BeritaId = request.BeritaId,
                    UserId = user.Id,
                    IsLike = request.IsLike,
                    Timestamp = DateTime.Now
                });
                newUserStatus = request.IsLike; // Status sesuai aksi baru
            }

            await _context.SaveChangesAsync();

            var likeCount = await _context.LikeDislikeBerita.CountAsync(ld => ld.BeritaId == request.BeritaId && ld.IsLike);
            var dislikeCount = await _context.LikeDislikeBerita.CountAsync(ld => ld.BeritaId == request.BeritaId && !ld.IsLike);

            return Json(new
            {
                success = true,
                newLikeCount = likeCount,
                newDislikeCount = dislikeCount,
                newUserStatus = newUserStatus // Kirim status baru ke client
            });
        }


        public class LikeDislikeRequest
        {
            public int BeritaId { get; set; }
            public bool IsLike { get; set; }
        }


    }
}
