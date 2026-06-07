using Application.DTOs.BorrowDto;

namespace Application.DTOs.ExtraTasksDtos;

public class BookDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public decimal Price { get; set; }
    public int PublishedYear { get; set; }
    public string CategoryName { get; set; } = "";
    public List<GetBorrowDto> Borrows { get; set; } = new();
}
