using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MemberDto;

public class UpdateMemberDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = "";
}
