using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookRepository(AppDbContext context) : IBookRepository
{
    public async Task<IQueryable<Book>> GetAllAsync()
    {
        return context.Books.AsQueryable();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }

    public async Task CreateAsync(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }
}