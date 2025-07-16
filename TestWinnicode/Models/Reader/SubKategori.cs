namespace TestWinnicode.Models.Reader
{
    public class SubKategori
    {
        public int Id { get; set; }
        public string Nama { get; set; }

        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }

        public ICollection<Berita> Beritas { get; set; }
    }
}
