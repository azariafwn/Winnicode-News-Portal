using System.ComponentModel.DataAnnotations;

namespace TestWinnicode.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Password lama wajib diisi.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Lama")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password baru wajib diisi.")]
        [StringLength(100, ErrorMessage = "{0} harus memiliki panjang minimal {2} dan maksimal {1} karakter.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password Baru")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Konfirmasi Password Baru")]
        [Compare("NewPassword", ErrorMessage = "Password baru dan konfirmasi password tidak cocok.")]
        public string ConfirmNewPassword { get; set; }
    }
}