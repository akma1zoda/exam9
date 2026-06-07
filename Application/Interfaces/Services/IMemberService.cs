using Application.DTOs;
using Application.DTOs.MemberDto;
using Application.Results;

namespace Application.Interfaces.Services;

public interface IMemberService
{
    Task<Result<List<GetMemberDto>>> GetAllAsync();
    Task<Result<GetMemberDto>> GetByIdAsync(int id);
    Task<Result<GetMemberDto>> CreateAsync(CreateMemberDto dto);
    Task<Result<GetMemberDto>> UpdateAsync(int id, UpdateMemberDto dto);
    Task<Result<string>> DeleteAsync(int id);
    Task<Result<List<GetMemberDto>>> GetTopReadersAsync();
}