namespace TestWinnicode.Models.Reader
{
    public class Komentar
    {
        public int Id { get; set; }
        public int BeritaId { get; set; }
        public int UserId { get; set; }
        public string Isi { get; set; }
        public DateTime Tanggal { get; set; }

        public Berita Berita { get; set; }
        public User User { get; set; }
    }
}
