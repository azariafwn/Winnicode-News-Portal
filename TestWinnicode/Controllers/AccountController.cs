using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestWinnicode.Data;
using TestWinnicode.Models;
using TestWinnicode.ViewModels;

namespace TestWinnicode.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || user.PasswordHash != HashPassword(password))
            {
                ViewBag.ErrorMessage = "Username atau password salah";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            // Redirect ke homepage berdasarkan role
            switch (user.Role)
            {
                case UserRole.Reader:
                    return RedirectToAction("Index", "Reader");
                case UserRole.Penulis:
                    return RedirectToAction("Index", "Penulis");
                case UserRole.Editor:
                    return RedirectToAction("Index", "Editor");
                default:
                    return RedirectToAction("Index", "Reader");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Reader");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string role, string namaLengkap, string email, string gender, DateTime? tanggalLahir, string nomorTelepon, string alamat)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                ViewBag.ErrorMessage = "Username sudah digunakan.";
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = Enum.Parse<UserRole>(role),
                NamaLengkap = namaLengkap,
                Email = email,
                Gender = gender,
                TanggalLahir = tanggalLahir,
                NomorTelepon = nomorTelepon,
                Alamat = alamat,
                TanggalBergabung = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Jika role adalah Penulis, tambahkan ke tabel Penulis
            if (user.Role == UserRole.Penulis)
            {
                var penulis = new TestWinnicode.Models.Penulis.Penulis
                {
                    UserId = user.Id,
                    Deskripsi = "",
                    TotalArtikel = 0,
                    TotalDibaca = 0,
                    KategoriFokusId = 1,
                    JumlahArtikelDraft = 0,
                    JumlahArtikelDitolak = 0,
                    JumlahArtikelMenunggu = 0
                };

                _context.Penulis.Add(penulis);
                await _context.SaveChangesAsync();
            }

            // Jika role adalah Editor, tambahkan ke tabel Editor
            if (user.Role == UserRole.Editor)
            {
                var editor = new TestWinnicode.Models.Editor.Editor
                {
                    UserId = user.Id,
                    KategoriId = 1,
                    Deskripsi = "",
                    TotalReview = 0
                };

                _context.Editor.Add(editor);
                await _context.SaveChangesAsync();
            }
                       
            ViewBag.SuccessMessage = "Akun berhasil dibuat. Silakan login.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                // Jika validasi gagal, kembalikan ke halaman profil dengan error
                TempData["PasswordError"] = ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage;
                return LocalRedirect(returnUrl);
            }

            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            // Verifikasi password lama
            if (user.PasswordHash != HashPassword(model.OldPassword))
            {
                TempData["PasswordError"] = "Password lama yang Anda masukkan salah.";
                return LocalRedirect(returnUrl);
            }

            // Update dengan password baru
            user.PasswordHash = HashPassword(model.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["PasswordSuccess"] = "Password Anda telah berhasil diubah.";
            return LocalRedirect(returnUrl);
        }
    }
}
