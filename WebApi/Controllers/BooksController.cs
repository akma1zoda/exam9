using Application.DTOs;
using Application.DTOs.BookDto;
using Application.Interfaces;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await bookService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] BookFilterDto filter)
    {
        var result = await bookService.GetFilteredAsync(filter);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await bookService.GetByIdAsync(id);
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
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        var result = await bookService.CreateAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBookDto dto)
    {
        var result = await bookService.UpdateAsync(id, dto);
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
        var result = await bookService.DeleteAsync(id);
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

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics()
    {
        var result = await bookService.GetStatisticsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("top-borrowed")]
    public async Task<IActionResult> GetTopBorrowed()
    {
        var result = await bookService.GetTopBorrowedAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var result = await bookService.GetDetailsAsync(id);
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

    [HttpGet("dashboard/statistics")]
    public async Task<IActionResult> GetDashboardStatistics()
    {
        var result = await bookService.GetDashboardStatisticsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }
}