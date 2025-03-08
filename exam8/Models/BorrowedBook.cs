namespace exam8.Models;

public class BorrowedBook
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
}