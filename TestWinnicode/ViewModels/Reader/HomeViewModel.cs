using PenulisModel = TestWinnicode.Models.Penulis.Penulis;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.ViewModels.Reader
{
    public class HomeViewModel
    {
        public Berita Headline { get; set; }
        public List<Berita> SubHeadlineList { get; set; }
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
        public List<PenulisModel> PenulisList { get; set; }
        public Dictionary<string, List<Berita>> WhatsOnByKategori { get; set; }

    }
}
