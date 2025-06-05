using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWinnicode.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }
        public string NamaLengkap { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime? TanggalLahir { get; set; }
        public string NomorTelepon { get; set; }
        public string Alamat { get; set; }
        public DateTime? TanggalBergabung { get; set; }

    }

    public enum UserRole
    {
        Reader,
        Penulis,
        Editor
    }
}
