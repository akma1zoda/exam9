using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CategoryDto;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [MaxLength(500)]
    public string? Description { get; set; }
}
