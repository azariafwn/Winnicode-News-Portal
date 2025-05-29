using Microsoft.EntityFrameworkCore;
using TestWinnicode.Models;

namespace TestWinnicode.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<SubKategori> SubKategori { get; set; }
        public DbSet<Berita> Berita { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
