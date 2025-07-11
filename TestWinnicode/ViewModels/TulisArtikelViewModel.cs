namespace TestWinnicode.ViewModels
{
    public class TulisArtikelViewModel
    {
        public string Judul { get; set; }
        public int KategoriId { get; set; } // Tambahkan ini
        public int SubKategoriId { get; set; }
        public string Tag { get; set; }
        public IFormFile Gambar { get; set; }
        public string IsiArtikel { get; set; }
    }


}
