using TestWinnicode.Models;

namespace TestWinnicode.ViewModels
{
    public class HomeViewModel
    {
        public Berita Headline { get; set; }
        public List<Berita> SubHeadlineList { get; set; }
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
        public List<Penulis> PenulisList { get; set; }
        public Dictionary<string, List<Berita>> WhatsOnByKategori { get; set; }

    }
}
