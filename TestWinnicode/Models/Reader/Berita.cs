using PenulisModel = TestWinnicode.Models.Penulis.Penulis;

namespace TestWinnicode.Models.Reader
{
    public class Berita
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public DateTime Tanggal_Publish { get; set; }
        public string Isi { get; set; }
        public string Status { get; set; }
        public int Jumlah_View { get; set; }
        public string Tag { get; set; }
        public string Gambar { get; set; }

        public int SubKategoriId { get; set; }
        public SubKategori SubKategori { get; set; }
        public bool IsHeadline { get; set; }
        public bool IsSubHeadline { get; set; }
        public int? PenulisId { get; set; }
        public PenulisModel Penulis { get; set; }

        public string? KomentarEditor { get; set; }

    }

}
