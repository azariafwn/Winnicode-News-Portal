namespace TestWinnicode.Models
{
    public class Berita
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public DateTime Tanggal_Publish { get; set; }
        public string Isi { get; set; }
        public string Status { get; set; }
        public int Jumlah_View { get; set; }

        public int SubKategoriId { get; set; }
        public SubKategori SubKategori { get; set; }
        public bool IsHeadline { get; set; }
        public bool IsSubHeadline { get; set; }
        public int? PenulisId { get; set; }
        public Penulis Penulis { get; set; }


    }

}
