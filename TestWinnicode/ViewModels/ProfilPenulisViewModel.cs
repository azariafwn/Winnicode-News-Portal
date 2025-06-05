using TestWinnicode.Models;

namespace TestWinnicode.ViewModels
{
    public class ProfilPenulisViewModel
    {
        public User User { get; set; }                   // Informasi dasar dari tabel User
        public Penulis Penulis { get; set; }             // Data tambahan dari tabel Penulis
        public List<Berita> BeritaTerbaru { get; set; }  // Daftar berita yang ditulis
        public int TotalView { get; set; }               // Total view semua berita
    }
}
