using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Route("api/admin/books")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AdminBooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.CreatedBy)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .Select(b => new AdminBookDTO
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
                Genres = string.Join(", ", b.BookGenres.Select(bg => bg.Genre.Name)),
                CoverImagePath = b.CoverImagePath
            })
            .ToArrayAsync();

            if (books.Count() == 0)
            {
                return NotFound("Книги не найдены.");
            }

            return Ok(books);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            if (bookId <= 0)
            {
                return BadRequest();
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                return NotFound($"Книга c Id {bookId} не найдена.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok("Книга успешно удалена.");

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBook(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Неверный Id книги");
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                 .ThenInclude(bg => bg.Genre)
                .Where(b => b.Id == id)
                .Select(b => new BookEditDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.Name,
                    Description = b.Description,
                    PageNumber = b.PageNumber,
                    YearPublished = b.YearPublished,
                    Rating = b.Rating,
                    CoverImagePath = b.CoverImagePath,
                    FilePath = b.FilePath,
                    GenreIds = b.BookGenres.Select(bg => bg.GenreId).ToList(),
                    Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
                }).FirstOrDefaultAsync();

            if (book == null) return NotFound($"Книга с ID {id} не найдена.");
            return Ok(book);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, BookEditModelDTO model)
        {
            if (id <= 0)
            {
                return BadRequest("Неверный Id книги");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound($"Книга с ID {id} не найдена.");

            book.Title = model.Title;
            book.AuthorId = model.AuthorId;
            book.Description = model.Description;
            book.PageNumber = model.PageNumber;
            book.YearPublished = model.YearPublished;
            book.Rating = model.Rating;

            _context.BookGenres.RemoveRange(book.BookGenres);
            foreach (var genreId in model.GenreIds)
            {
                book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genreId });
            }

            await _context.SaveChangesAsync();
            return Ok("Книга успешно обновлена.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookAddModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _context.Authors.FindAsync(model.AuthorId);

            if (author == null)
            {
                return BadRequest("Указанный автор не существует");
            }

            var user = await _context.Users.FindAsync(model.CreatedById);

            if (user == null)
            {
                return BadRequest("Пользователь с указанным Id не найден.");
            }


            var book = new Book
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                FilePath = model.FilePath,
                CoverImagePath = model.CoverImagePath,
                Description = model.Description,
                CreatedById = model.CreatedById,
                PageNumber = model.PageNumber,
                YearPublished = model.YearPublished,
                Rating = model.Rating
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            if (model.GenreIds != null && model.GenreIds.Any())
            {
                foreach (var genreId in model.GenreIds)
                {
                    var genreExists = await _context.Genres.AnyAsync(g => g.Id == genreId);
                    if (genreExists)
                    {
                        book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genreId });
                    }
                }
                await _context.SaveChangesAsync();
            }

            return Ok("Книга успешно добавлена");

        }

    }
}
