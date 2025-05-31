using TestWinnicode.Models;
using System.Collections.Generic;

namespace TestWinnicode.ViewModels
{
    public class BeritaViewModel
    {
        public Berita BeritaDetail { get; set; }
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
    }
}
