using Application.DTOs;
using Application.DTOs.MemberDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController(IMemberService memberService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await memberService.GetAllAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await memberService.GetByIdAsync(id);
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
    public async Task<IActionResult> Create(CreateMemberDto dto)
    {
        var result = await memberService.CreateAsync(dto);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberDto dto)
    {
        var result = await memberService.UpdateAsync(id, dto);
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
        var result = await memberService.DeleteAsync(id);
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

    [HttpGet("top-readers")]
    public async Task<IActionResult> GetTopReaders()
    {
        var result = await memberService.GetTopReadersAsync();
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Data);
    }
}