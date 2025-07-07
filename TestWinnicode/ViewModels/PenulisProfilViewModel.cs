namespace TestWinnicode.ViewModels
{
    public class PenulisProfilViewModel
    {
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string NomorTelepon { get; set; }
        public string Alamat { get; set; }
        public DateTime? TanggalBergabung { get; set; }
        public string Role { get; set; } = "Penulis";

        public string Deskripsi { get; set; }
        public int TotalArtikel { get; set; }
        public int TotalDibaca { get; set; }
        public string KategoriFokus { get; set; }

        public int JumlahArtikelDraft { get; set; }
        public int JumlahArtikelDitolak { get; set; }
        public int JumlahArtikelMenunggu { get; set; }


    }
}
