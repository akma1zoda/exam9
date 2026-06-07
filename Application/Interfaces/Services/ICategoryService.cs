using Application.DTOs;
using Application.DTOs.CategoryDto;
using Application.DTOs.ExtraTasksDtos;
using Application.Results;

namespace Application.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<List<GetCategoryDto>>> GetAllAsync();
    Task<Result<GetCategoryDto>> GetByIdAsync(int id);
    Task<Result<GetCategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<Result<GetCategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<List<CategoryWithBooksDto>>> GetWithBooksAsync();
}