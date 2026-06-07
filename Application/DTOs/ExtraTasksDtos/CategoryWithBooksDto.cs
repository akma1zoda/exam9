using Application.DTOs.BookDto;

namespace Application.DTOs.ExtraTasksDtos;

public class CategoryWithBooksDto
{
public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public List<GetBookDto> Books { get; set; } = new();
}
