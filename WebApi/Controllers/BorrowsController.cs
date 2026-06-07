using Application.DTOs;
using Application.DTOs.BorrowDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/borrows")]
public class BorrowsController(IBorrowService borrowService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await borrowService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await borrowService.GetByIdAsync(id);
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
    public async Task<IActionResult> Create(CreateBorrowDto dto)
    {
        var result = await borrowService.CreateAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBorrowDto dto)
    {
        var result = await borrowService.UpdateAsync(id, dto);
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
        var result = await borrowService.DeleteAsync(id);
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

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await borrowService.GetActiveAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistoryByMember([FromQuery] int memberId)
    {
        var result = await borrowService.GetHistoryByMemberAsync(memberId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("monthly-borrows")]
    public async Task<IActionResult> GetMonthlyBorrows()
    {
        var result = await borrowService.GetMonthlyBorrowsAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }
}