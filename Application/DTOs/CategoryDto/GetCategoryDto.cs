namespace Application.DTOs.CategoryDto;

public class GetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}
