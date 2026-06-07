using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BorrowDto;

public class CreateBorrowDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
}
