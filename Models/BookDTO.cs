namespace LibraryProject.Models
{
    public class BookDTO
    {
            public int Id { get; set; }
            public string Title { get; set; }
            public string AuthorName { get; set; }
            public string CoverImagePath { get; set; }
            public string? FilePath { get; set; }
            public string Description { get; set; }
            public int? PageCount { get; set; }
            public int? PublicationYear { get; set; }
        
            public decimal? Rating { get; set; }

            public List<string> Genres { get; set; } = new List<string>();
    }
}
