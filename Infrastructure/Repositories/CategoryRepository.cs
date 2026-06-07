using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<IQueryable<Category>> GetAllAsync()
    {
        return context.Categories.AsQueryable();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories.FindAsync(id);
    }

    public async Task CreateAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }
}