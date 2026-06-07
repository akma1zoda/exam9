using Application.DTOs;
using Application.DTOs.BorrowDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BorrowService(IBorrowRepository borrowRepository, ILogger<BorrowService> logger) : IBorrowService
{
    public async Task<Result<List<GetBorrowDto>>> GetAllAsync()
    {
        var borrows = await borrowRepository.GetAllAsync();
        var result = await borrows
            .Select(b => new GetBorrowDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            }).ToListAsync();

        return Result<List<GetBorrowDto>>.Ok(result);
    }

    public async Task<Result<GetBorrowDto>> GetByIdAsync(int id)
    {
        var borrow = await borrowRepository.GetByIdAsync(id);
        if (borrow == null)
        {
            return Result<GetBorrowDto>.Fail("Borrow not found", ErrorType.NotFound);
        }
        return Result<GetBorrowDto>.Ok(new GetBorrowDto
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            MemberId = borrow.MemberId,
            BorrowDate = borrow.BorrowDate,
            ReturnDate = borrow.ReturnDate
        });
    }

    public async Task<Result<GetBorrowDto>> CreateAsync(CreateBorrowDto dto)
    {
        try
        {
            if (dto.BookId <= 0)
            {
                return Result<GetBorrowDto>.Fail("BookId is required", ErrorType.Validation);
            }
            if (dto.MemberId <= 0)
            {
                return Result<GetBorrowDto>.Fail("MemberId is required", ErrorType.Validation);
            }
            var borrow = new Borrow
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.UtcNow
            };

            await borrowRepository.CreateAsync(borrow);

            return Result<GetBorrowDto>.Ok(new GetBorrowDto
            {
                Id = borrow.Id,
                BookId = borrow.BookId,
                MemberId = borrow.MemberId,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating borrow");
            return Result<GetBorrowDto>.Fail("Error creating borrow", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetBorrowDto>> UpdateAsync(int id, UpdateBorrowDto dto)
    {
        try
        {
            var borrow = await borrowRepository.GetByIdAsync(id);
            if (borrow == null)
            {
                return Result<GetBorrowDto>.Fail("Borrow not found", ErrorType.NotFound);
            }
            borrow.ReturnDate = dto.ReturnDate;

            await borrowRepository.UpdateAsync(borrow);

            return Result<GetBorrowDto>.Ok(new GetBorrowDto
            {
                Id = borrow.Id,
                BookId = borrow.BookId,
                MemberId = borrow.MemberId,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating borrow");
            return Result<GetBorrowDto>.Fail("Error updating borrow", ErrorType.Unknown);
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        try
        {
            var borrow = await borrowRepository.GetByIdAsync(id);
            if (borrow == null)
            {
                return Result<string>.Fail("Borrow not found", ErrorType.NotFound);
            }
            await borrowRepository.DeleteAsync(borrow);
            return Result<string>.Ok("Borrow deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting borrow");
            return Result<string>.Fail("Error deleting borrow", ErrorType.Unknown);
        }
    }

    public async Task<Result<List<GetBorrowDto>>> GetActiveAsync()
    {
        var borrows = await borrowRepository.GetAllAsync();

        var result = await borrows
            .Where(b => b.ReturnDate == null)
            .Select(b => new GetBorrowDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            }).ToListAsync();

        return Result<List<GetBorrowDto>>.Ok(result);
    }

    public async Task<Result<List<GetBorrowDto>>> GetHistoryByMemberAsync(int memberId)
    {
        var borrows = await borrowRepository.GetAllAsync();

        var result = await borrows
            .Where(b => b.MemberId == memberId)
            .Select(b => new GetBorrowDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            }).ToListAsync();

        return Result<List<GetBorrowDto>>.Ok(result);
    }

    public async Task<Result<List<MonthlyBorrowsDto>>> GetMonthlyBorrowsAsync()
    {
        var borrows = await borrowRepository.GetAllAsync();
        var a = DateTime.UtcNow.AddMonths(-6);

        var result = await borrows
            .Where(b => b.BorrowDate >= a)
            .GroupBy(b => b.BorrowDate.Year)
            .Select(g => new MonthlyBorrowsDto { Year = g.Key, Count = g.Count() })
            .OrderBy(x => x.Year)
            .ToListAsync();

        return Result<List<MonthlyBorrowsDto>>.Ok(result);
    }
}