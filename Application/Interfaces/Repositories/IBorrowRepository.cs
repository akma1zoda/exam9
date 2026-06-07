using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<IQueryable<Borrow>> GetAllAsync();
    Task<Borrow?> GetByIdAsync(int id);
    Task CreateAsync(Borrow borrow);
    Task UpdateAsync(Borrow borrow);
    Task DeleteAsync(Borrow borrow);
}
