using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IQueryable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}
