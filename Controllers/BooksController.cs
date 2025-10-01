using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks ()
        {
            var books = await _context.Books
        .Include(b => b.Author)
        .Include(b => b.CreatedBy)
        .Include(b => b.BookGenres)
            .ThenInclude(bg => bg.Genre)
            .Select(b => new BookDTO
            {
                Id = b.Id,
                Title = b.Title,
                AuthorName = b.Author.Name,
                CoverImagePath = b.CoverImagePath,

            }).ToArrayAsync();

            if (books.Count() == 0)
            {
                return NotFound();
            }

            return Ok(books);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.CreatedBy)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Where(b => b.Id == id).Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.Name,
                    CoverImagePath = b.CoverImagePath,
                    FilePath = b.FilePath,
                    Description = b.Description,
                    PageCount = b.PageNumber,
                    PublicationYear = b.YearPublished,
                    Rating = b.Rating,
                    Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
                }).FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }
            

            return Ok(book);
        }

        [HttpGet("GetBookTitle")]
        public async Task<IActionResult> GetBookTitle(string title)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.CreatedBy)
                .Include(b => b.BookGenres)
                  .ThenInclude(bg => bg.Genre)
                  .Where(b => b.Title.Contains(title))
                  .Select(b => new BookDTO
                  {
                      Id = b.Id,
                      Title = b.Title,
                      AuthorName = b.Author.Name,
                      CoverImagePath = b.CoverImagePath
                  }).FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound($"Книга с названием {title} не найдена.");
            }

            return Ok(book);
        }

        [HttpGet("GetBookAuthor")]
        public async Task<IActionResult> BetBookAuthor(string name)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.CreatedBy)
                .Include(b => b.BookGenres)
                  .ThenInclude(bg => bg.Genre)
                  .Where(b => b.Author.Name.Contains(name))
                  .Select(b => new BookDTO
                  {
                      Id = b.Id,
                      Title = b.Title,
                      AuthorName = b.Author.Name,
                      CoverImagePath = b.CoverImagePath
                  }).ToArrayAsync();

            if (books.Count() == 0)
            {
                return NotFound($"Автор {name} не найден.");
            }

            return Ok(books);
            
        }

        //[HttpGet("admin")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetAllBooksForAdmin()
        //{
        //    var books = await _context.Books
        //    .Include(b => b.Author)
        //    .Include(b => b.CreatedBy)
        //    .Include(b => b.BookGenres)
        //        .ThenInclude(bg => bg.Genre)
        //    .Select(b => new AdminBookDTO
        //    {
        //        Id = b.Id,
        //        Title = b.Title,
        //        AuthorName = b.Author.Name,
        //        Genres = string.Join(", ", b.BookGenres.Select(bg => bg.Genre.Name)),
        //        CoverImagePath = b.CoverImagePath
        //    })
        //    .ToArrayAsync();

        //    if (books.Count() == 0)
        //    {
        //        return NotFound("Книги не найдены.");
        //    }

        //    return Ok(books);
        //}

        //[HttpDelete("admin-delete")]
        //[Authorize(Roles ="Admin")]
        //public async Task<IActionResult> DeleteBook(int bookId)
        //{
        //    if (bookId <= 0)
        //    {
        //        return BadRequest();
        //    }

        //    var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

        //    if (book == null)
        //    {
        //        return NotFound($"Книга c Id {bookId} не найдена.");
        //    }

        //    _context.Books.Remove(book);
        //    await _context.SaveChangesAsync();

        //    return Ok("Книга успешно удалена.");

        //}

        //[HttpPost("admin-edit")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetBookForEdit(int bookId)
        //{
        //    if (bookId <= 0)
        //    {
        //        return BadRequest("Неверный Id книги");
        //    }

        //    var book = await _context.Books
        //        .Include(b => b.Author)
        //        .Include(b => b.BookGenres)
        //         .ThenInclude(bg => bg.Genre)
        //        .Where(b => b.Id == bookId)
        //        .Select(b => new BookEditDTO
        //        {
        //            Id = b.Id,
        //            Title = b.Title,
        //            AuthorId = b.AuthorId,
        //            AuthorName = b.Author.Name,
        //            Description = b.Description,
        //            PageNumber = b.PageNumber,
        //            YearPublished = b.YearPublished,
        //            Rating = b.Rating,
        //            CoverImagePath = b.CoverImagePath,
        //            FilePath = b.FilePath,
        //            GenreIds = b.BookGenres.Select(bg => bg.GenreId).ToList(),
        //            Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
        //        }).FirstOrDefaultAsync();

        //    if (book == null) return NotFound($"Книга с ID {bookId} не найдена.");
        //    return Ok(book);
        //}

        //[HttpPut("admin-update/{id}")]
        //[Authorize(Roles ="Admin")]
        //public async Task<IActionResult> UpdateBookAfterEdit(int id, BookEditModelDTO model)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Неверный Id книги");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var book = await _context.Books
        //        .Include(b => b.Author)
        //        .Include(b => b.BookGenres)
        //        .FirstOrDefaultAsync(b => b.Id == id);

        //    if (book == null) return NotFound($"Книга с ID {id} не найдена.");

        //    book.Title = model.Title;
        //    book.AuthorId = model.AuthorId;  
        //    book.Description = model.Description;
        //    book.PageNumber = model.PageNumber;
        //    book.YearPublished = model.YearPublished;
        //    book.Rating = model.Rating;

        //    _context.BookGenres.RemoveRange(book.BookGenres);
        //    foreach (var genreId in model.GenreIds)
        //    {
        //        book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genreId });
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok("Книга успешно обновлена.");
        //}

        //[HttpPost("admin-add")]
        //[Authorize(Roles ="Admin")]
        //public async Task<IActionResult> AddBook(BookAddModelDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var author = await _context.Authors.FindAsync(model.AuthorId);

        //    if (author == null)
        //    {
        //        return BadRequest("Указанный автор не существует");
        //    }

        //    var user = await _context.Users.FindAsync(model.CreatedById);

        //     if (user == null)
        //    {
        //        return BadRequest("Пользователь с указанным Id не найден.");
        //    }


        //    var book = new Book
        //    {
        //        Title = model.Title,
        //        AuthorId = model.AuthorId,
        //        FilePath = model.FilePath,
        //        CoverImagePath = model.CoverImagePath,
        //        Description = model.Description,
        //        CreatedById = model.CreatedById,
        //        PageNumber = model.PageNumber,
        //        YearPublished = model.YearPublished,
        //        Rating = model.Rating
        //    };

        //    _context.Books.Add(book);
        //    await _context.SaveChangesAsync();

        //    if (model.GenreIds != null && model.GenreIds.Any())
        //    {
        //        foreach (var genreId in model.GenreIds)
        //        {
        //            var genreExists = await _context.Genres.AnyAsync(g => g.Id == genreId);
        //            if (genreExists)
        //            {
        //                book.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genreId });
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    return Ok("Книга успешно добавлена");

        //}

        //[HttpGet("admin-get-authors")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetAuthors()
        //{
        //    var authors = await _context.Authors.ToListAsync();

        //    if (authors.Count == 0)
        //    {
        //        return NotFound("Авторы не найдены");
        //    }

        //    return Ok(authors);
        //}

        //[HttpDelete("admin-delete-author")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteAuthor(int id)
        //{
        //    var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

        //    if (author == null)
        //    {
        //        return NotFound("Автор не найден.");
        //    }

        //    _context.Authors.Remove(author);
        //    await _context.SaveChangesAsync();

        //    return Ok("Автор успешно удален.");
        //}




    }
}
