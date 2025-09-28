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

            modelBuilder.Entity<User>().HasData(
               new User { Id = 1, Username = "admin", PasswordHash = "hashed_password", Role = "Admin" });

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Лев Толстой" },
                new Author { Id = 2, Name = "Фёдор Достоевский" },
                new Author { Id = 3, Name = "Антон Чехов" },
                new Author { Id = 4, Name = "Александр Пушкин" },
                new Author { Id = 5, Name = "Михаил Булгаков" }
            );

            modelBuilder.Entity<Genre>().HasData(
               new Genre { Id = 1, Name = "Роман" },
               new Genre { Id = 2, Name = "Драма" },
               new Genre { Id = 3, Name = "Классика" },
               new Genre { Id = 4, Name = "Фантастика" },
               new Genre { Id = 5, Name = "Поэзия" },
               new Genre { Id = 6, Name = "Сатира" }
           );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Война и мир",
                    AuthorId = 1,
                    CreatedById = 1,
                    PageNumber = 1225,
                    YearPublished = 1869,
                    Rating = 4.8m,
                    Description = "Эпический роман о войне 1812 года",
                    CoverImagePath = "/Uploads/365442.jpg",
                    FilePath = "/Books/Atomic habits.pdf"
                },

                new Book
                {
                    Id = 2,
                    Title = "Преступление и наказание",
                    AuthorId = 2,
                    CreatedById = 1,
                    PageNumber = 672,
                    YearPublished = 1866,
                    Rating = 4.7m,
                    Description = "Роман о преступлении и моральных терзаниях",
                    CoverImagePath = "covers/crime_and_punishment.jpg",
                    FilePath = "books/crime_and_punishment.pdf"
                },

                 new Book
                 {
                     Id = 3,
                     Title = "Мастер и Маргарита",
                     AuthorId = 5,
                     CreatedById = 1,
                     PageNumber = 480,
                     YearPublished = 1967,
                     Rating = 4.9m,
                     Description = "Мистический роман о дьяволе в Москве",
                     CoverImagePath = "covers/master_and_margarita.jpg",
                     FilePath = "books/master_and_margarita.pdf"
                 },

                 new Book
                 {
                     Id = 4,
                     Title = "Евгений Онегин",
                     AuthorId = 4,
                     CreatedById = 1,
                     PageNumber = 320,
                     YearPublished = 1833,
                     Rating = 4.6m,
                     Description = "Роман в стихах",
                     CoverImagePath = "covers/eugene_onegin.jpg",
                     FilePath = "books/eugene_onegin.pdf"
                 },

                 new Book
                 {
                     Id = 5,
                     Title = "Вишнёвый сад",
                     AuthorId = 3,
                     CreatedById = 1,
                     PageNumber = 96,
                     YearPublished = 1904,
                     Rating = 4.5m,
                     Description = "Пьеса о судьбе дворянства",
                     CoverImagePath = "covers/cherry_orchard.jpg",
                     FilePath = "books/cherry_orchard.pdf"
                 });

            modelBuilder.Entity<BookGenre>().HasData(
               // Война и мир - Роман, Классика
               new BookGenre { Id = 1, BookId = 1, GenreId = 1 },
               new BookGenre { Id = 2, BookId = 1, GenreId = 3 },

               // Преступление и наказание - Роман, Драма, Классика
               new BookGenre { Id = 3, BookId = 2, GenreId = 1 },
               new BookGenre { Id = 4, BookId = 2, GenreId = 2 },
               new BookGenre { Id = 5, BookId = 2, GenreId = 3 },

               // Мастер и Маргарита - Роман, Фантастика, Сатира
               new BookGenre { Id = 6, BookId = 3, GenreId = 1 },
               new BookGenre { Id = 7, BookId = 3, GenreId = 4 },
               new BookGenre { Id = 8, BookId = 3, GenreId = 6 },

               // Евгений Онегин - Роман, Поэзия
               new BookGenre { Id = 9, BookId = 4, GenreId = 1 },
               new BookGenre { Id = 10, BookId = 4, GenreId = 5 },

               // Вишнёвый сад - Драма, Классика
               new BookGenre { Id = 11, BookId = 5, GenreId = 2 },
               new BookGenre { Id = 12, BookId = 5, GenreId = 3 }
           );

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
    }
}
