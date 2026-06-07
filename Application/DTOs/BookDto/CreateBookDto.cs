using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BookDto;

public class CreateBookDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = "";

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1000, 2100)]
    public int PublishedYear { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
