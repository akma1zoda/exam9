using Application.DTOs;
using Application.DTOs.BorrowDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IBorrowService
{
    Task<Result<List<GetBorrowDto>>> GetAllAsync();
    Task<Result<GetBorrowDto>> GetByIdAsync(int id);
    Task<Result<GetBorrowDto>> CreateAsync(CreateBorrowDto dto);
    Task<Result<GetBorrowDto>> UpdateAsync(int id, UpdateBorrowDto dto);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<List<GetBorrowDto>>> GetActiveAsync();
    Task<Result<List<GetBorrowDto>>> GetHistoryByMemberAsync(int memberId);
    Task<Result<List<MonthlyBorrowsDto>>> GetMonthlyBorrowsAsync();
}