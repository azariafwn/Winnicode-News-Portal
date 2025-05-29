namespace TestWinnicode.Models
{
    public class Kategori
    {
        public int Id { get; set; }
        public string Nama { get; set; }

        public ICollection<SubKategori> SubKategoris { get; set; }
    }
}
