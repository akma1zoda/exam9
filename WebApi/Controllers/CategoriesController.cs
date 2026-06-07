using Application.DTOs;
using Application.DTOs.CategoryDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await categoryService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await categoryService.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == Application.Results.ErrorType.NotFound)
            {
                return NotFound(result.Error);
            }
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var result = await categoryService.CreateAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var result = await categoryService.UpdateAsync(id, dto);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == Application.Results.ErrorType.NotFound)
            {
                return NotFound(result.Error);
            }
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await categoryService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            if (result.ErrorType == Application.Results.ErrorType.NotFound)
            {
                return NotFound(result.Error);
            }
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("with-books")]
    public async Task<IActionResult> GetWithBooks()
    {
        var result = await categoryService.GetWithBooksAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }
}