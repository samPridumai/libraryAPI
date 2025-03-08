namespace exam8.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string CoverImageUrl { get; set; } = null!;
    public int PublicationYear { get; set; }
    public string Description { get; set; } = null!;
    public string Status { get; set; } = "available";
    public DateTime CreatedAt { get; set; }
}