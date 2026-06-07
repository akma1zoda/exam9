using Application.DTOs.BookDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IBookService
{
    Task<Result<List<GetBookDto>>> GetAllAsync();
    Task<Result<GetBookDto>> GetByIdAsync(int id);
    Task<Result<PagedResult<GetBookDto>>> GetFilteredAsync(BookFilterDto filter);
    Task<Result<GetBookDto>> CreateAsync(CreateBookDto dto);
    Task<Result<GetBookDto>> UpdateAsync(int id, UpdateBookDto dto);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<BookStatisticsDto>> GetStatisticsAsync();
    Task<Result<List<TopBorrowedBookDto>>> GetTopBorrowedAsync();
    Task<Result<BookDetailsDto>> GetDetailsAsync(int id);
    Task<Result<DashboardStatisticsDto>> GetDashboardStatisticsAsync();
}
