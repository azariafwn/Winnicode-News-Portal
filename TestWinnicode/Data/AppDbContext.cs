using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using TestWinnicode.Models;
using TestWinnicode.Models.Editor;
using TestWinnicode.Models.Penulis;
using TestWinnicode.Models.Reader;

namespace TestWinnicode.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<SubKategori> SubKategori { get; set; }
        public DbSet<Berita> Berita { get; set; }
        public DbSet<Penulis> Penulis { get; set; }
        public DbSet<Editor> Editor { get; set; }
        public DbSet<Komentar> Komentar { get; set; }
        public DbSet<LikeDislikeBerita> LikeDislikeBerita { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LikeDislikeBerita>()
                .HasOne(ld => ld.User)
                .WithMany()
                .HasForeignKey(ld => ld.UserId);

            modelBuilder.Entity<LikeDislikeBerita>()
                .HasOne(ld => ld.Berita)
                .WithMany()
                .HasForeignKey(ld => ld.BeritaId);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
