using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
