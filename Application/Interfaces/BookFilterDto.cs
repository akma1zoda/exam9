using Application.Results;

namespace Application.Interfaces;

public class BookFilterDto : PagedRequest
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}