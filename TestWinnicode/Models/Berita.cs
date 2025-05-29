namespace TestWinnicode.Models
{
    public class Berita
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public string Penulis { get; set; }
        public DateTime Tanggal_Publish { get; set; }
        public string Isi { get; set; }
        public int Jumlah_View { get; set; }

        public int SubKategoriId { get; set; }
        public SubKategori SubKategori { get; set; }
    }

}
