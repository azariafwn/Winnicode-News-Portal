using TestWinnicode.Models.Reader;
using System.Collections.Generic;

namespace TestWinnicode.ViewModels.Reader
{
    public class SubKategoriViewModel
    {
        // Untuk menampilkan nama subkategori dan breadcrumb
        public SubKategori SubKategoriDetail { get; set; }

        // Daftar berita terbaru di subkategori ini
        public List<Berita> BeritaSubKategori { get; set; }        

        // Daftar berita trending (site-wide) untuk sidebar
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
    }
}