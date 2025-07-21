using System;

namespace TestWinnicode.ViewModels.Penulis
{
    public class ArtikelPenulisViewModel
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public string Status { get; set; }
        public DateTime TanggalEdit { get; set; }

        public string? KomentarEditor { get; set; }
    }

}
