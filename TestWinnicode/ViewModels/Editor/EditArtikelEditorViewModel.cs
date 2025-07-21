using Microsoft.AspNetCore.Mvc;

namespace TestWinnicode.ViewModels.Editor
{
    public class EditArtikelEditorViewModel
    {
        public int Id { get; set; }
        public string Judul { get; set; }
        public string Isi { get; set; }
        public string Status { get; set; }
    }
}
