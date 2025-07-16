using TestWinnicode.Models;
using PenulisModel = TestWinnicode.Models.Penulis.Penulis;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.ViewModels.Penulis
{
    public class ProfilPenulisViewModel
    {
        public User User { get; set; }                   // Informasi dasar dari tabel User
        public PenulisModel Penulis { get; set; }             // Data tambahan dari tabel Penulis
        public List<Berita> BeritaTerbaru { get; set; }  // Daftar berita yang ditulis
        public int TotalView { get; set; }               // Total view semua berita
    }
}
