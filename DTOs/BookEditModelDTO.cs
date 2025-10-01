namespace LibraryProject.DTOs
{
    public class BookEditModelDTO
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; }
        public int YearPublished { get; set; }
        public decimal Rating { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
    }
}
