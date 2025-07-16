namespace TestWinnicode.ViewModels.Editor
{
    public class EditorProfilViewModel
    {
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string NomorTelepon { get; set; }
        public string Alamat { get; set; }

        public DateTime? TanggalBergabung { get; set; }
        public string KategoriFokus { get; set; }
        public int TotalReview { get; set; }
        public string Deskripsi { get; set; }
    }
}
