using Application.DTOs;
using Application.DTOs.BookDto;
using Application.DTOs.CategoryDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<Result<List<GetCategoryDto>>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        var result = await categories
            .Select(c => new GetCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToListAsync();

        return Result<List<GetCategoryDto>>.Ok(result);
    }

    public async Task<Result<GetCategoryDto>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return Result<GetCategoryDto>.Fail("Category not found", ErrorType.NotFound);
        }
        return Result<GetCategoryDto>.Ok(new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        });
    }

    public async Task<Result<GetCategoryDto>> CreateAsync(CreateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result<GetCategoryDto>.Fail("Name is required", ErrorType.Validation);
            }
            if (dto.Name.Length > 100)
            {
                return Result<GetCategoryDto>.Fail("Name max 100 characters", ErrorType.Validation);
            }
            if (dto.Description.Length > 500)
            {
                return Result<GetCategoryDto>.Fail("Description max 500 characters", ErrorType.Validation);
            }
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await categoryRepository.CreateAsync(category);

            return Result<GetCategoryDto>.Ok(new GetCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating category");
            return Result<GetCategoryDto>.Fail("Error creating category", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetCategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Result<GetCategoryDto>.Fail("Name is required", ErrorType.Validation);
            }
            if (dto.Name.Length > 100)
            {
                return Result<GetCategoryDto>.Fail("Name max 100 characters", ErrorType.Validation);
            }
            if (dto.Description.Length > 500)
            {
                return Result<GetCategoryDto>.Fail("Description max 500 characters", ErrorType.Validation);
            }
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return Result<GetCategoryDto>.Fail("Category not found", ErrorType.NotFound);
            }
            category.Name = dto.Name;
            category.Description = dto.Description;

            await categoryRepository.UpdateAsync(category);

            return Result<GetCategoryDto>.Ok(new GetCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating category");
            return Result<GetCategoryDto>.Fail("Error updating category", ErrorType.Unknown);
        }
    }


    public async Task<Result<string>> DeleteAsync(int id)
    {
        try
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return Result<string>.Fail("Category not found", ErrorType.NotFound);
            }
            await categoryRepository.DeleteAsync(category);
            return Result<string>.Ok("Category deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting category ");
            return Result<string>.Fail("Error deleting category", ErrorType.Unknown);
        }
    }



    public async Task<Result<List<CategoryWithBooksDto>>> GetWithBooksAsync()
    {
        var categories = await categoryRepository.GetAllAsync();

        var result = await categories
            .Select(c => new CategoryWithBooksDto
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                Books = c.Books.Select(b => new GetBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price,
                    PublishedYear = b.PublishedYear,
                    CategoryId = b.CategoryId
                }).ToList()
            }).ToListAsync();

        return Result<List<CategoryWithBooksDto>>.Ok(result);
    }
}