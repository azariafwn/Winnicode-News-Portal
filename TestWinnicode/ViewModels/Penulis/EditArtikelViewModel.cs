using TestWinnicode.Models;
using System.ComponentModel.DataAnnotations;

namespace TestWinnicode.ViewModels.Penulis
{
    public class EditArtikelViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Judul { get; set; }

        [Required]
        public string Isi { get; set; }

        public string Status { get; set; }

        public string? KomentarEditor { get; set; }
    }
}

