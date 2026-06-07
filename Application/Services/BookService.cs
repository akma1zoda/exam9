using Application.DTOs;
using Application.DTOs.BookDto;
using Application.DTOs.BorrowDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BookService(IBookRepository bookRepository, ILogger<BookService> logger, IBorrowRepository borrowRepository, IMemberRepository memberRepository) : IBookService
{
    public async Task<Result<List<GetBookDto>>> GetAllAsync()
    {
        try
        {
            var books = await bookRepository.GetAllAsync();
            var result = await books
                .Select(b => new GetBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    PublishedYear = b.PublishedYear,
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category.Name
                }).ToListAsync();

            return Result<List<GetBookDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all books");
            return Result<List<GetBookDto>>.Fail("Error getting all books", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetBookDto>> GetByIdAsync(int id)
    {
        try
        {
            var book = await bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return Result<GetBookDto>.Fail("Book not found", ErrorType.NotFound);
            }
            return Result<GetBookDto>.Ok(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                PublishedYear = book.PublishedYear,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting book by id {Id}", id);
            return Result<GetBookDto>.Fail("Error getting book", ErrorType.Unknown);
        }
    }

    public async Task<Result<PagedResult<GetBookDto>>> GetFilteredAsync(BookFilterDto filter)
    {
        try
        {
            var query = await bookRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(b => b.Title.Contains(filter.SearchTerm) ||
                                     b.Author.Contains(filter.SearchTerm));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == filter.CategoryId.Value);
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(b => b.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= filter.MaxPrice.Value);
            }
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(b => new GetBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    PublishedYear = b.PublishedYear,
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category.Name
                }).ToListAsync();

            return Result<PagedResult<GetBookDto>>.Ok(
                PagedResult<GetBookDto>.Ok(items, totalCount, filter.Page, filter.PageSize));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting filtered books");
            return Result<PagedResult<GetBookDto>>.Fail("Error getting filtered books", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetBookDto>> CreateAsync(CreateBookDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Result<GetBookDto>.Fail("Title is required", ErrorType.Validation);
            }
            if (dto.Title.Length > 200)
            {
                return Result<GetBookDto>.Fail("Title max 200 characters", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(dto.Author))
            {
                return Result<GetBookDto>.Fail("Author is required", ErrorType.Validation);
            }
            if (dto.Author.Length > 100)
            {
                return Result<GetBookDto>.Fail("Author max 100 characters", ErrorType.Validation);
            }
            if (dto.Price < 0)
            {
                return Result<GetBookDto>.Fail("Price cannot be negative", ErrorType.Validation);
            }
            if (dto.PublishedYear < 1000 || dto.PublishedYear > 2100)
            {
                return Result<GetBookDto>.Fail("Invalid published year", ErrorType.Validation);
            }
            if (dto.CategoryId <= 0)
            {
                return Result<GetBookDto>.Fail("CategoryId is required", ErrorType.Validation);
            }
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                PublishedYear = dto.PublishedYear,
                CategoryId = dto.CategoryId
            };

            await bookRepository.CreateAsync(book);

            return Result<GetBookDto>.Ok(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                PublishedYear = book.PublishedYear,
                CategoryId = book.CategoryId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating book");
            return Result<GetBookDto>.Fail("Error creating book", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetBookDto>> UpdateAsync(int id, UpdateBookDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return Result<GetBookDto>.Fail("Title is required", ErrorType.Validation);
            }

            if (dto.Title.Length > 200)
            {
                return Result<GetBookDto>.Fail("Title max 200 characters", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(dto.Author))
            {
                return Result<GetBookDto>.Fail("Author is required", ErrorType.Validation);
            }
            if (dto.Author.Length > 100)
            {
                return Result<GetBookDto>.Fail("Author max 100 characters", ErrorType.Validation);
            }
            if (dto.Price < 0)
            {
                return Result<GetBookDto>.Fail("Price cannot be negative", ErrorType.Validation);
            }
            if (dto.PublishedYear < 1000 || dto.PublishedYear > 2100)
            {
                return Result<GetBookDto>.Fail("Invalid published year", ErrorType.Validation);
            }
            if (dto.CategoryId <= 0)
            {
                return Result<GetBookDto>.Fail("CategoryId is required", ErrorType.Validation);
            }

            var book = await bookRepository.GetByIdAsync(id);
            if (book is null)
            {
                return Result<GetBookDto>.Fail("Book not found", ErrorType.NotFound);
            }
            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Price = dto.Price;
            book.PublishedYear = dto.PublishedYear;
            book.CategoryId = dto.CategoryId;

            await bookRepository.UpdateAsync(book);

            return Result<GetBookDto>.Ok(new GetBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                PublishedYear = book.PublishedYear,
                CategoryId = book.CategoryId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating book ");
            return Result<GetBookDto>.Fail("Error updating book", ErrorType.Unknown);
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        try
        {
            var book = await bookRepository.GetByIdAsync(id);
            if (book is null)
                return Result<string>.Fail("Book not found", ErrorType.NotFound);

            await bookRepository.DeleteAsync(book);
            return Result<string>.Ok("Book deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting book ");
            return Result<string>.Fail("Error deleting book", ErrorType.Unknown);
        }
    }

    public async Task<Result<BookStatisticsDto>> GetStatisticsAsync()
    {
        var books = await bookRepository.GetAllAsync();
        var borrows = await borrowRepository.GetAllAsync();

        var totalBooks = await books.CountAsync();
        var averagePrice = await books.AverageAsync(b => b.Price);
        var totalBorrows = await borrows.CountAsync();

        return Result<BookStatisticsDto>.Ok(new BookStatisticsDto
        {
            TotalBooks = totalBooks,
            AveragePrice = averagePrice,
            TotalBorrows = totalBorrows
        });
    }

    public async Task<Result<List<TopBorrowedBookDto>>> GetTopBorrowedAsync()
    {
        var borrows = await borrowRepository.GetAllAsync();

        var result = await borrows
            .GroupBy(b => b.Book.Title)
            .Select(g => new TopBorrowedBookDto
            {
                BookTitle = g.Key,
                TotalBorrows = g.Count()
            })
            .OrderByDescending(x => x.TotalBorrows)
            .Take(5)
            .ToListAsync();

        return Result<List<TopBorrowedBookDto>>.Ok(result);
    }

    public async Task<Result<BookDetailsDto>> GetDetailsAsync(int id)
    {
        var books = await bookRepository.GetAllAsync();

        var book = await books
            .Include(b => b.Category)
            .Include(b => b.Borrows)
            .FirstOrDefaultAsync(b => b.Id == id);

        return Result<BookDetailsDto>.Ok(new BookDetailsDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            PublishedYear = book.PublishedYear,
            CategoryName = book.Category.Name,
            Borrows = book.Borrows.Select(b => new GetBorrowDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            }).ToList()
        });
    }

    public async Task<Result<DashboardStatisticsDto>> GetDashboardStatisticsAsync()
    {
        var books = await bookRepository.GetAllAsync();
        var members = await memberRepository.GetAllAsync();
        var borrows = await borrowRepository.GetAllAsync();

        var totalBooks = await books.CountAsync();
        var totalMembers = await members.CountAsync();
        var activeBorrows = await borrows.CountAsync(b => b.ReturnDate == null);
        var totalRevenue = await books.SumAsync(b => b.Price);

        return Result<DashboardStatisticsDto>.Ok(new DashboardStatisticsDto
        {
            TotalBooks = totalBooks,
            TotalMembers = totalMembers,
            ActiveBorrows = activeBorrows,
            TotalRevenue = totalRevenue
        });
    }
}