using LibraryProject.Data;
using LibraryProject.DTOs;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Route("api/admin/genres")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminGenresController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AdminGenresController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _context.Genres.ToListAsync();

            if (genres.Count == 0)
            {
                return NotFound("Жанры не найдены");
            }

            return Ok(genres);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null )
            {
                return NotFound("Жанр не найден.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok("Жанр успешно удален.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGenre(int id, GenreUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if (id <= 0)
            {
                return BadRequest("Неверный Id жанра.");
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound($"Автор с {id} не найден.");
            }

            genre.Name = model.Name;
            await _context.SaveChangesAsync();

            return Ok("Жанр успешно обновлен.");

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Неверный Id жанра.");
            }

            var genre = await _context.Genres
             .Where(g => g.Id == id)
             .Select(g => new GenreDTO
             {
                 Id = g.Id,
                 Name = g.Name ?? string.Empty
             })
               .FirstOrDefaultAsync();

            if (genre == null)
            {
                return NotFound($"Жанр с Id {id} не найден.");
            }

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = new Genre
            {
                Name = model.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return Ok("Жанр успешно создан.");
        }

    }
}
