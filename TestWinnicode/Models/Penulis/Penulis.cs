using TestWinnicode.Models.Reader;

namespace TestWinnicode.Models.Penulis
{
    public class Penulis
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Deskripsi { get; set; }
        public int TotalArtikel { get; set; }
        public int TotalDibaca { get; set; }
        public int? KategoriFokusId { get; set; }  // FK ke tabel Kategori
        public Kategori? KategoriFokus { get; set; }  // properti navigasi

        public int JumlahArtikelDraft { get; set; }
        public int JumlahArtikelDitolak { get; set; }
        public int JumlahArtikelMenunggu { get; set; }

        public User User { get; set; }
        public string NamaLengkap => User?.NamaLengkap;
        public List<Berita> BeritaList { get; set; }

    }
}