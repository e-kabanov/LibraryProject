using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext (DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
            .Property(b => b.Rating)
            .HasPrecision(3, 1); // 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
    }
}
