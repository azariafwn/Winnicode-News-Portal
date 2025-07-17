using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.Models.Editor
{
    public class Editor
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int? KategoriId { get; set; }

        [ForeignKey("KategoriId")]
        public Kategori Kategori { get; set; }

        public string Deskripsi { get; set; }
        public int TotalReview { get; set; }
    }
}
