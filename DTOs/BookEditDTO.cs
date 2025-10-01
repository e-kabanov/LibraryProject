namespace LibraryProject.DTOs
{
    public class BookEditDTO
    {
        // Основная информация
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        // Детальная информация
        public string Description { get; set; }
        public int PageNumber { get; set; }
        public int YearPublished { get; set; }
        public decimal Rating { get; set; }

        // Файлы
        public string CoverImagePath { get; set; }
        public string FilePath { get; set; }

        // Жанры
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<string> Genres { get; set; } = new List<string>();
    }
}
