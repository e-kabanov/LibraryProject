using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Route("api/admin/authors")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminAuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AdminAuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            if (authors.Count == 0)
            {
                return NotFound("Авторы не найдены");
            }

            return Ok(authors);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound("Автор не найден.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return Ok("Автор успешно удален.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (id <= 0)
            {
                return BadRequest("Неверный Id автора.");
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound($"Автор с {id} не найден.");
            }

            author.Name = model.Name;
            await _context.SaveChangesAsync();

            return Ok("Автор успешно обновлен.");


        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Неверный Id автора.");
            }

            var author = await _context.Authors
             .Where(a => a.Id == id)
             .Select(a => new AuthorDTO
             {
               Id = a.Id,
               Name = a.Name ?? string.Empty
             })
               .FirstOrDefaultAsync();

            if (author == null)
            {
                return NotFound($"Автор с Id {id} не найден.");
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author
            {
                Name = model.Name
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return Ok("Автор успешно создан.");
        }


    }
}
