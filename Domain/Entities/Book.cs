namespace Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
    public int PublishedYear { get; set; }
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
    public List<Borrow> Borrows { get; set; } = new();
}
