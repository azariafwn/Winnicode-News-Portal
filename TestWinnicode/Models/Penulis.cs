using TestWinnicode.Models;

public class Penulis
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Deskripsi { get; set; }
    public int TotalArtikel { get; set; }
    public int TotalDibaca { get; set; }
    public int? KategoriFokusId { get; set; }  // FK ke tabel Kategori
    public Kategori? KategoriFokus { get; set; }  // properti navigasi

    public User User { get; set; }
    public List<Berita> BeritaList { get; set; }

}
