using Application.DTOs;
using Application.DTOs.MemberDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Results;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger) : IMemberService
{
    public async Task<Result<List<GetMemberDto>>> GetAllAsync()
    {
        var members = await memberRepository.GetAllAsync();
        var result = await members
            .Select(m => new GetMemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                RegisteredAt = m.RegisteredAt
            }).ToListAsync();

        return Result<List<GetMemberDto>>.Ok(result);
    }

    public async Task<Result<GetMemberDto>> GetByIdAsync(int id)
    {
        var member = await memberRepository.GetByIdAsync(id);
        if (member == null)
        {
            return Result<GetMemberDto>.Fail("Member not found", ErrorType.NotFound);
        }
        return Result<GetMemberDto>.Ok(new GetMemberDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            RegisteredAt = member.RegisteredAt
        });
    }

    public async Task<Result<GetMemberDto>> CreateAsync(CreateMemberDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return Result<GetMemberDto>.Fail("FullName is required", ErrorType.Validation);
            }
            if (dto.FullName.Length > 150)
            {
                return Result<GetMemberDto>.Fail("FullName max 150 characters", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<GetMemberDto>.Fail("Email is required", ErrorType.Validation);
            }
            if (dto.Email.Length > 200)
            {
                return Result<GetMemberDto>.Fail("Email max 200 characters", ErrorType.Validation);
            }
            if (!dto.Email.Contains("@"))
            {
                return Result<GetMemberDto>.Fail("Invalid email format", ErrorType.Validation);
            }
            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                RegisteredAt = DateTime.UtcNow
            };

            await memberRepository.CreateAsync(member);

            return Result<GetMemberDto>.Ok(new GetMemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                RegisteredAt = member.RegisteredAt
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating member");
            return Result<GetMemberDto>.Fail("Error creating member", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetMemberDto>> UpdateAsync(int id, UpdateMemberDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return Result<GetMemberDto>.Fail("FullName is required", ErrorType.Validation);
            }
            if (dto.FullName.Length > 150)
            {
                return Result<GetMemberDto>.Fail("FullName max 150 characters", ErrorType.Validation);
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return Result<GetMemberDto>.Fail("Email is required", ErrorType.Validation);
            }
            if (dto.Email.Length > 200)
            {
                return Result<GetMemberDto>.Fail("Email max 200 characters", ErrorType.Validation);
            }
            if (!dto.Email.Contains("@"))
            {
                return Result<GetMemberDto>.Fail("Invalid email format", ErrorType.Validation);
            }
            var member = await memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return Result<GetMemberDto>.Fail("Member not found", ErrorType.NotFound);
            }
            member.FullName = dto.FullName;
            member.Email = dto.Email;

            await memberRepository.UpdateAsync(member);

            return Result<GetMemberDto>.Ok(new GetMemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                RegisteredAt = member.RegisteredAt
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating member");
            return Result<GetMemberDto>.Fail("Error updating member", ErrorType.Unknown);
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        try
        {
            var member = await memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return Result<string>.Fail("Member not found", ErrorType.NotFound);
            }
            await memberRepository.DeleteAsync(member);
            return Result<string>.Ok("Member deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting member");
            return Result<string>.Fail("Error deleting member", ErrorType.Unknown);
        }
    }

    public async Task<Result<List<GetMemberDto>>> GetTopReadersAsync()
    {
        var members = await memberRepository.GetAllAsync();

        var result = await members
            .OrderByDescending(m => m.Borrows.Count)
            .Take(3)
            .Select(m => new GetMemberDto
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                RegisteredAt = m.RegisteredAt
            }).ToListAsync();

        return Result<List<GetMemberDto>>.Ok(result);
    }
}