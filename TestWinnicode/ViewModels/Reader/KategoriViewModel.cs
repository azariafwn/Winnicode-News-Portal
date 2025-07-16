using System.Collections.Generic;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.ViewModels.Reader
{
    public class KategoriViewModel
    {
        public List<Kategori> KategoriList { get; set; }
        public List<SubKategori> SubKategoriList { get; set; }
        public List<Berita> BeritaList { get; set; }
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
        public Berita Headline { get; set; }
        public List<Berita> SubHeadlines { get; set; }
        public Dictionary<string, List<Berita>> BeritaPerSubKategori { get; set; }



    }
}
