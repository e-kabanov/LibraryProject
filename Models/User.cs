namespace LibraryProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
