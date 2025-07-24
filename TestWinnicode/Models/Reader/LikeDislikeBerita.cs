namespace TestWinnicode.Models.Reader
{
    public class LikeDislikeBerita
    {
        public int Id { get; set; }

        public int BeritaId { get; set; }
        public Berita Berita { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsLike { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
