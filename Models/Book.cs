namespace LibraryProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int AuthorId { get; set; }
        public string? FilePath { get; set; }
        public string? CoverImagePath { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
        public int PageNumber { get; set; }
        public int YearPublished { get; set; }
        public decimal Rating { get; set; }

        public Author? Author { get; set; }
        public User? CreatedBy { get; set; }

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    }
}
