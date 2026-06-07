
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;
using WebApi.MiddleWares;
using Application.Interfaces.Services;
using Application.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IMemberService, MemberService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleWare>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();