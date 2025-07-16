namespace TestWinnicode.Models.Reader
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public ICollection<SubKategori> SubKategoris { get; set; }
    }
}
