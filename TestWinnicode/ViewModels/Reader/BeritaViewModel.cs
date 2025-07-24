using System.Collections.Generic;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.ViewModels.Reader
{
    public class BeritaViewModel
    {
        public Berita BeritaDetail { get; set; }
        public List<Berita> TrendingList { get; set; }
        public List<Berita> TerbaruList { get; set; }
        
        public List<KomentarViewModel> KomentarList { get; set; }
        public int JumlahKomentar { get; set; }

        public int JumlahLike { get; set; }
        public int JumlahDislike { get; set; }
        public bool? UserLikeStatus { get; set; }
    }
}
